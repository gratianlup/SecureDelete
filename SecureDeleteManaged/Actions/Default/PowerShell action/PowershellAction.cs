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
using DebugUtils.Debugger;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.IO;
using SecureDelete.Schedule;
using System.Threading;

namespace SecureDelete.Actions
{
	[Serializable]
	public sealed class PowershellAction : IAction, ICloneable
	{
		#region Constants

		private const string PowerShellRegistryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\";
		private const string PowerShellRegistryValue = "Install";
		private const string NotInstalledMessage = "Cannot execute PowerShell script. Windows PowerShell not installed.";
		private const string FailedLoadMessage = "Cannot load PowerShell script from file {0}.";
		private const string FailedInitializationMessage = "Failed to initialize Windows Powershell.";
		private const string AbortedMessage = "PowerShell script stopped by user.";
		private const string TimeExpiredMessage = "Maximum allowed time ({0}) expired for PowerShell script.";
		private const string FailedRunMessage = "Failed to run Powershell script.\nException: {0}";
		private const int    MaxVersion = 5;

		#endregion

		#region Fields

		[NonSerialized]
		private bool sucessfull;

		[NonSerialized]
		private ScheduleTimer timer;

		[NonSerialized]
		private ManualResetEvent exitEvent;

		[NonSerialized]
		private ScriptExecutor executor;

		#endregion

		#region Properties

		private bool _enabled;
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		[NonSerialized]
		private WipeSession _session;	
		public SecureDelete.WipeSession Session
		{
			get { return _session; }
			set { _session = value; }
		}

		[NonSerialized]
		private bool _afterWipe;
		public bool AfterWipe
		{
			get { return _afterWipe; }
			set { _afterWipe = value; }
		}

		[NonSerialized]
		private bool _blockingMode;
		public bool BlockingMode
		{
			get { return _blockingMode; }
			set { _blockingMode = value; }
		}

		private bool _hasMaximumExecutionTime;
		public bool HasMaximumExecutionTime
		{
			get { return _hasMaximumExecutionTime; }
			set { _hasMaximumExecutionTime = value; }
		}

		private TimeSpan _maximumExecutionTime;
		public TimeSpan MaximumExecutionTime
		{
			get { return _maximumExecutionTime; }
			set { _maximumExecutionTime = value; }
		}

		private bool _useScriptFile;
		public bool UseScriptFile
		{
			get { return _useScriptFile; }
			set { _useScriptFile = value; }
		}

		private string _scriptFile;
		public string ScriptFile
		{
			get { return _scriptFile; }
			set { _scriptFile = value; }
		}

		private string _script;
		public string Script
		{
			get { return _script; }
			set { _script = value; }
		}

		private bool _usePluginFolder;
		public bool UsePluginFolder
		{
			get { return _usePluginFolder; }
			set { _usePluginFolder = value; }
		}

		private string _pluginFolder;
		public string PluginFolder
		{
			get { return _pluginFolder; }
			set { _pluginFolder = value; }
		}

		#endregion

		#region Private methods

		private List<IBridge> GetBridgeObjectList()
		{
			List<IBridge> bridgeList = new List<IBridge>();

			// get from executing assembly
			bridgeList.AddRange(BridgeFactory.GetBridgeObjects());

			// get from folder
			if (_pluginFolder != null && _pluginFolder.Length > 0)
			{
				try
				{
					string[] files = Directory.GetFiles(_pluginFolder, "*.dll");

					foreach (string file in files)
					{
						bridgeList.AddRange(BridgeFactory.GetBridgeObjects(file));
					}
				}
				catch { }
			}

			// initialize the full access bridge objects
			foreach (IFullAccessBridge bridge in bridgeList)
			{
				bridge.Session = _session;
				bridge.AfterWipe = _afterWipe;
			}

			return bridgeList;
		}

		private void OnScriptStateChanged(object sender, PipelineStateInfo info)
		{
			switch (info.State)
			{
				case PipelineState.Completed:
					{
						exitEvent.Set();
						break;
					}
				case PipelineState.Failed:
					{
						sucessfull = false;
						ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High,
														FailedRunMessage, info.Reason.Message);
						exitEvent.Set();
						break;
					}
			}
		}

		private bool InitExecutor()
		{
			executor = new ScriptExecutor();

			// attach events
			executor.OnStateChanged += OnScriptStateChanged;

			// create the runspace
			if (executor.CreateRunspace() == false)
			{
				return false;
			}

			// add the bridge objects
			executor.BridgeList = GetBridgeObjectList();

			return true;
		}

		private void StartTimer()
		{
			if (_hasMaximumExecutionTime && _maximumExecutionTime.TotalMilliseconds > 0)
			{
				if (timer != null)
				{
					timer.StopTimer();
				}
				else
				{
					timer = new ScheduleTimer();
				}

				timer.OnTimeEllapsed += OnTimeExpired;
				timer.StartTimer(_maximumExecutionTime);
			}
		}

		private void StopTimer()
		{
			if (timer != null)
			{
				timer.StopTimer();
				timer.OnTimeEllapsed -= OnTimeExpired;
			}
		}

		private void OnTimeExpired(object sender, EventArgs e)
		{
			if (executor != null)
			{
				executor.Stop();
				ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, TimeExpiredMessage);
			}

			StopTimer();
			sucessfull = false;
		}

		#endregion

		#region Public methods

		public static bool IsPowershellInstalled(out int version)
		{
			for (int i = 0; i < MaxVersion; i++)
			{
				string keyName = PowerShellRegistryKey + i.ToString();

				try
				{
					// get the value
					int installed = (int)Registry.GetValue(keyName, PowerShellRegistryValue, null);

					if (installed > 0)
					{
						version = installed;
						return true;
					}
				}
				catch { }
			}

			version = 0;
			return false;
		}


		public bool Start()
		{
			string script = null;
			int version;

			// check if PowerShell is installed
			if (IsPowershellInstalled(out version) == false)
			{
				ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, NotInstalledMessage);
				return false;
			}

			// set the script
			if (_useScriptFile)
			{
				// load the script from the file
				try
				{
					script = File.ReadAllText(_scriptFile);
				}
				catch
				{
					ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, FailedLoadMessage, _scriptFile);
					return false;
				}
			}
			else
			{
				script = _script;
			}

			// prepare the execution engine
			if (InitExecutor() == false)
			{
				ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, FailedInitializationMessage);
				return false;
			}

			// start
			exitEvent = new ManualResetEvent(false);

			if (executor.RunScriptAsync(script) == false)
			{
				ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, FailedInitializationMessage);
				return false;
			}

			// other tasks
			sucessfull = true;
			StartTimer();

			// wait
			if (_blockingMode)
			{
				EndStart();
			}

			return sucessfull;
		}


		public void Stop()
		{
			if (executor != null)
			{
				executor.Stop();
				ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, AbortedMessage);
			}

			sucessfull = false;
		}


		public bool EndStart()
		{
			if (executor != null)
			{
				exitEvent.WaitOne();
				executor.DestroyRunspace();
				executor = null;
			}

			return sucessfull;
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			PowershellAction temp = new PowershellAction();

			temp._enabled = _enabled;
			temp._maximumExecutionTime = _maximumExecutionTime;
			if (_pluginFolder != null)
			{
				temp._pluginFolder = (string)_pluginFolder.Clone();
			}
			if (_script != null)
			{
				temp._script = (string)_script;
			}
			if (_scriptFile != null)
			{
				temp._scriptFile = (string)_scriptFile.Clone();
			}
			temp._usePluginFolder = _usePluginFolder;
			temp._useScriptFile = _useScriptFile;

			return temp;
		}

		#endregion
	}
}