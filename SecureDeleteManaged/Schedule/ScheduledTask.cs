// ***************************************************************
//  SecureDelete   version:  1.0
//  -------------------------------------------------------------
//
//  Copyright (C) 2007 Lup Gratian - All Rights Reserved
//   
// *************************************************************** 

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using DebugUtils.Debugger;
using System.Runtime.Serialization;
using SecureDelete.Actions;

namespace SecureDelete.Schedule
{
	public enum TaskStatus
	{
		InitializingWiping, Wiping, Paused, Waiting, Stopping, Stopped, Queued
	}

	// delegates
	public delegate void TaskStatusChangedDelegate(ScheduledTask task,TaskStatus status);
	public delegate void TaskStartedDelegate(ScheduledTask task);
	public delegate void TaskCompletedDelegate(ScheduledTask task);

	[Serializable]
	public class ScheduledTask : ICloneable
	{
		#region Constants

		private const int StatusUpdateInterval = 200;

		#endregion

		#region Fields

		[NonSerialized]
		private object statusLock = new object();

		[NonSerialized]
		private object wipeStatusLock = new object();

		[NonSerialized]
		private WipeSession session;

		[NonSerialized]
		private HistoryItem historyItem;

		[NonSerialized]
		private ActionExecutor actionExecutor;

		[NonSerialized]
		private ManualResetEvent wipeCompletedEvent;

		#endregion

		#region Constructor

		public ScheduledTask()
		{
			Status = TaskStatus.Stopped;
			wipeCompletedEvent = new ManualResetEvent(false);
			_beforeWipeActions = new List<IAction>();
			_afterWipeActions = new List<IAction>();
		}

		#endregion

		#region Properties

		private Guid _taskId;
		public Guid TaskId
		{
			get { return _taskId; }
			set { _taskId = value; }
		}

		private TaskStatus _status;
		public TaskStatus Status
		{
			get
			{
				lock (statusLock)
				{
					return _status;
				}
			}
			set
			{
				lock (statusLock)
				{
					_status = value;
				}

				if (OnTaskStatusChanged != null)
				{
					OnTaskStatusChanged(this, _status);
				}
			}
		}

		[NonSerialized]
		private WipeStatus _currentWipeStatus;
		public WipeStatus CurrentWipeStatus
		{
			get
			{
				if (Status == TaskStatus.Wiping)
				{
					lock (wipeStatusLock)
					{
						return _currentWipeStatus;
					}
				}

				return null;
			}
			set
			{
				lock (wipeStatusLock)
				{
					_currentWipeStatus = value;
				}
			}
		}

		[NonSerialized]
		private SDOptions _options;
		public SDOptions Options
		{
			get { return _options; }
			set { _options = value; }
		}

		[NonSerialized]
		private List<ITaskController> _taskControllers;
		public List<ITaskController> TaskControllers
		{
			get { return _taskControllers; }
			set { _taskControllers = value; }
		}

		[NonSerialized]
		private HistoryManager _historyManager;
		public HistoryManager HistoryManager
		{
			get { return _historyManager; }
			set { _historyManager = value; }
		}

		private bool _useCustomOptions;
		public bool UseCustomOptions
		{
			get { return _useCustomOptions; }
			set { _useCustomOptions = value; }
		}

		private WipeOptions _customOptions;
		public WipeOptions CustomOptions
		{
			get { return _customOptions; }
			set { _customOptions = value; }
		}

		private bool _enabled;
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		private ISchedule _schedule;
		public ISchedule Schedule
		{
			get { return _schedule; }
			set { _schedule = value; }
		}

		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private string _description;
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		private bool _saveReport;
		public bool SaveReport
		{
			get { return _saveReport; }
			set { _saveReport = value; }
		}

		private List<IAction> _beforeWipeActions;
		public List<IAction> BeforeWipeActions
		{
			get { return _beforeWipeActions; }
			set { _beforeWipeActions = value; }
		}

		private List<IAction> _afterWipeActions;
		public List<IAction> AfterWipeActions
		{
			get { return _afterWipeActions; }
			set { _afterWipeActions = value; }
		}

		#endregion

		#region Events

		public event TaskStatusChangedDelegate OnTaskStatusChanged;
		public event TaskStartedDelegate OnTaskStarted;
		public event TaskCompletedDelegate OnTaskCompleted;

		#endregion

		#region Private methods

		private void AnnounceControllersStart()
		{
			if (_taskControllers != null)
			{
				foreach (ITaskController controller in _taskControllers)
				{
					controller.TaskStarted(this);
				}
			}
		}


		private bool RunBeforWipeActions()
		{
			if (_beforeWipeActions != null && _beforeWipeActions.Count > 0)
			{
				actionExecutor = new ActionExecutor();
				actionExecutor.Session = session;
				actionExecutor.AfterWipe = false;
				actionExecutor.Actions = _beforeWipeActions;

				actionExecutor.Start();
				
				// stop on error
				if (actionExecutor.Result == false)
				{
					return false;
				}
			}

			return true;
		}


		private bool RunAfterWipeActions()
		{
			if (_afterWipeActions != null && _afterWipeActions.Count > 0)
			{
				actionExecutor = new ActionExecutor();
				actionExecutor.Session = session;
				actionExecutor.AfterWipe = true;
				actionExecutor.Actions = _afterWipeActions;

				actionExecutor.Start();

				// stop on error
				if (actionExecutor.Result == false)
				{
					return false;
				}
			}

			return true;
		}


		private ReportInfo SaveSessionReport()
		{
			if (session == null)
			{
				return null;
			}

			if (session.Status == SessionStatus.Stopped)
			{
				WipeReport report = session.GenerateReport();

				if (report != null)
				{
					// save it
					WipeReportManager manager = new WipeReportManager();
					string reportFilePath = SecureDeleteLocations.GetReportFilePath();
					string reportDirectory = SecureDeleteLocations.GetReportDirectory();

					if (File.Exists(reportFilePath))
					{
						manager.LoadReportCategories(reportFilePath);
					}

					manager.ReportDirectory = reportDirectory;
					if (_options.MaximumReportsPerSession > 0)
					{
						manager.MaximumReportsPerSession = _options.MaximumReportsPerSession;
					}

					// add the report
					ReportInfo info;
					manager.AddReport(report, out info);

					// save
					manager.SaveReport(report, info);
					manager.SaveReportCategories(reportFilePath);

					return info;
				}
			}

			return null;
		}


		private void OnWipeStarted(IAsyncResult result)
		{
			session.EndStart();

			if (Status == TaskStatus.InitializingWiping)
			{
				Status = TaskStatus.Wiping;
			}
		}


		private void OnTimeEllapsed(ISchedule schedule)
		{
			if (OnTaskStarted != null)
			{
				OnTaskStarted(this);
			}
		}


		private bool LoadSession()
		{
			string path = SecureDeleteLocations.CombinePath(SecureDeleteLocations.GetScheduledTasksDirectory(),
														   _taskId.ToString() + SecureDeleteLocations.SessionFileExtension);

			if (File.Exists(path) == false)
			{
				return false;
			}

			return SessionLoader.LoadSession(path, out session);
		}


		private void AddStartHistoryItem()
		{
			historyItem = new HistoryItem(_taskId, DateTime.Now);

			if (_historyManager.Add(historyItem) == false)
			{
				Debug.ReportError("Failed to add history item for task {0}", _name);
			}

			if (_historyManager.SaveHistory(SecureDeleteLocations.GetScheduleHistoryFile()) == false)
			{
				Debug.ReportError("Failed to save history items");
			}
		}

		private void AddEndHistoryItem(ReportInfo info)
		{
			if (historyItem == null)
			{
				throw new Exception("History item not added.");
			}

			historyItem.EndTime = DateTime.Now;
			if (session != null)
			{
				historyItem.Statistics = session.Statistics;
				historyItem.Statistics.Errors += session.BeforeWipeErrors.Count + session.AfterWipeErrors.Count;
			}
			historyItem.ReportInfo = info;
			historyItem.Failed = historyItem.Statistics == null || historyItem.Statistics.FailedObjects > 0 || historyItem.Statistics.Errors > 0;

			if (_historyManager.SaveHistory(SecureDeleteLocations.GetScheduleHistoryFile()) == false)
			{
				Debug.ReportError("Failed to save history items");
			}
		}


		private void StopImpl(bool runAfterWipeActions)
		{
			// stop
			if (session != null)
			{
				session.Stop();
			}

			//  run "after" wipe actions
			if (runAfterWipeActions)
			{
				RunAfterWipeActions();
			}

			// generate reports
			ReportInfo reportInfo = null;

			if (_saveReport)
			{
				reportInfo = SaveSessionReport();
			}

			// log history item
			AddEndHistoryItem(reportInfo);

			// inform parent about task completition
			if (OnTaskCompleted != null)
			{
				OnTaskCompleted(this);
			}

			// set completed status
			wipeCompletedEvent.Set();

			// set stopped status
			Status = TaskStatus.Stopped;
		}
		

		private void WipeThread()
		{
			if (Status == TaskStatus.InitializingWiping)
			{
				try
				{
					// add the history item
					AddStartHistoryItem();

					// announce task controllers about the start
					AnnounceControllersStart();

					// load the session from disk
					if (LoadSession() == false)
					{
						Debug.ReportError("Failed to load session from file {0}", SecureDeleteLocations.GetSessionFile(_taskId));
						return;
					}

					// initialize the session
					if (_useCustomOptions)
					{
						session.Options = _customOptions;
					}
					else
					{
						session.Options = _options.WipeOptions;
					}

					session.Options.WipeMethodsPath = _options.MethodFolder;
					session.PluginAssemblies = new List<string>(SecureDeleteLocations.GetPluginAssemblies());

					// run start actions
					if (RunBeforWipeActions() == false)
					{
						// failed to run actions, stop wipe process
						StopImpl(false);
						return;
					}

					// start actual wiping
					if (session.Initialize())
					{
						if (Status != TaskStatus.InitializingWiping)
						{
							return;
						}

						// start
						if (session.BeginStart(OnWipeStarted, null) == false)
						{
							// failed to start
							return;
						}

						while ((session.Status == SessionStatus.BeginStart ||
							   (session.Status == SessionStatus.Wipe && session.Context.Status == ContextStatus.Wipe)) && 
								Status != TaskStatus.Stopping)
						{
							// update wipe status
							if (session.Status == SessionStatus.Wipe)
							{
								lock (wipeStatusLock)
								{
									if (_currentWipeStatus == null)
									{
										_currentWipeStatus = new WipeStatus();
									}

									session.GetWipeStatus(ref _currentWipeStatus, false);
								}
							}

							// pause / resume
							if (Status == TaskStatus.Paused)
							{
								if (session.Status != SessionStatus.Paused)
								{
									if (session.Pause() == false)
									{
										Debug.ReportError("Failed to pause");
									}
								}
							}
							else if (session.Status == SessionStatus.Paused)
							{
								if (Status == TaskStatus.Wiping)
								{
									if (session.Resume() == false)
									{
										Debug.ReportError("Failed to resume");
									}
								}
							}

							// wait some time until we query for the status again
							Thread.Sleep(StatusUpdateInterval);
						}
					}
					else
					{
						Debug.ReportError("Failed to initialize session");
					}
				}
				catch (Exception e)
				{
					Debug.ReportError("Error while wiping. Exception: {0}", e.Message);
				}
				finally
				{
					TaskStatus status = Status;

					if (status != TaskStatus.Stopped && status != TaskStatus.Waiting)
					{
						StopImpl(true);
					}
				}
			}
		}

		#endregion

		#region Public methods

		public bool StartSchedule()
		{
			// validate
			if (Status != TaskStatus.Stopped)
			{
				return false;
			}

			if (_enabled == false)
			{
				// disabled
				return false;
			}

			if (_schedule == null)
			{
				throw new Exception("Schedule not set");
			}

			// return if not enabled
			if (_schedule.Enabled == false)
			{
				return false;
			}

			// start waiting
			_schedule.TaskStarted += OnTimeEllapsed;
			_schedule.StartSchedule();
			Status = TaskStatus.Waiting;

			return true;
		}


		public bool StartWipe()
		{
			// run the task on a separate thread
			Thread t = new Thread(WipeThread);

			CurrentWipeStatus = null;
			wipeCompletedEvent.Reset();
			Status = TaskStatus.InitializingWiping;

			t.Start();
			return true;
		}


		public bool Stop()
		{
			TaskStatus status = Status;

			if (status == TaskStatus.Stopped)
			{
				return false;
			}
			else if (status == TaskStatus.Waiting)
			{
				// stop schedule
				_schedule.StopSchedule();
				Status = TaskStatus.Stopped;
			}
			else
			{
				// stop wiping
				if (actionExecutor != null)
				{
					actionExecutor.Stop();
				}

				Status = TaskStatus.Stopping;
			}

			return true;
		}


		public bool Pause()
		{
			if (Status != TaskStatus.Wiping)
			{
				return false;
			}

			Status = TaskStatus.Paused;

			return true;
		}


		public bool Resume()
		{
			if (Status != TaskStatus.Paused)
			{
				return false;
			}

			Status = TaskStatus.Wiping;

			return true;
		}

		public void WaitForFinnish()
		{
			wipeCompletedEvent.WaitOne();
		}

		#endregion

		#region Serialization helpers

		[OnDeserialized()]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			statusLock = new object();
			wipeStatusLock = new object();
			Status = TaskStatus.Stopped;
			wipeCompletedEvent = new ManualResetEvent(false);
			if (_beforeWipeActions == null)
			{
				_beforeWipeActions = new List<IAction>();
			}
			if (_afterWipeActions == null)
			{
				_afterWipeActions = new List<IAction>();
			}
		}

		[OnSerializing()]
		internal void OnSerializingMethod(StreamingContext context)
		{
			OnTaskStatusChanged = null;
			OnTaskStarted = null;
			OnTaskCompleted = null;
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			ScheduledTask temp = new ScheduledTask();

			temp._taskId = _taskId;
			if (_schedule != null)
			{
				temp._schedule = (ISchedule)_schedule.Clone();
			}
			if (_name != null)
			{
				temp._name = (string)_name.Clone();
			}
			if (_description != null)
			{
				temp._description = (string)_description.Clone();
			}
			temp._saveReport = _saveReport;

			foreach (IAction action in _beforeWipeActions)
			{
				temp._beforeWipeActions.Add((IAction)action.Clone());
			}

			foreach (IAction action in _afterWipeActions)
			{
				temp._afterWipeActions.Add((IAction)action.Clone());
			}

			temp._useCustomOptions = _useCustomOptions;
			if (_customOptions != null)
			{
				temp._customOptions = (WipeOptions)_customOptions.Clone();
			}

			temp._enabled = _enabled;

			return temp;
		}

		#endregion
	}
}
