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
using System.Diagnostics;
using System.Threading;

namespace SecureDelete.Actions
{
	public class ActionExecutor
	{
		#region Fields

		private ManualResetEvent waitEvent;
		private IAction activeAction;
		private object lockObject;
		private bool stopped;
		private bool result;

		#endregion

		#region Constructor

		public ActionExecutor()
		{
			_actions = new List<IAction>();
			waitEvent = new ManualResetEvent(true);
			lockObject = new object();
		}

		#endregion

		#region Properties

		private List<IAction> _actions;
		public List<IAction> Actions
		{
			get { return _actions; }
			set { _actions = value; }
		}

		private WipeSession _session;
		public WipeSession Session
		{
			get { return _session; }
			set { _session = value; }
		}

		private bool _afterWipe;
		public bool AfterWipe
		{
			get { return _afterWipe; }
			set { _afterWipe = value; }
		}

		public bool Stopped
		{
			get { return GetStopped(); }
		}

		public bool Result
		{
			get { return result; }
		}

		#endregion

		#region Events

		public event EventHandler OnStopped;

		#endregion

		#region Private methods

		private void SetStopped(bool value)
		{
			lock (lockObject)
			{
				stopped = value;
			}
		}

		private bool GetStopped()
		{
			lock (lockObject)
			{
				return stopped;
			}
		}

		private void NotifyStopped()
		{
			if (OnStopped != null)
			{
				OnStopped(this, null);
			}
		}

		private void StartImpl(object sender,EventArgs e)
		{
			if (_actions == null || _actions.Count == 0)
			{
				return;
			}

			// reset
			stopped = false;
			waitEvent.Reset();
			result = true;

			for (int i = 0; i < _actions.Count; i++)
			{
				lock (lockObject)
				{
					activeAction = _actions[i];
				}

				if (activeAction.Enabled)
				{
					activeAction.Session = _session;
					activeAction.AfterWipe = _afterWipe;
					activeAction.BlockingMode = true;

					// start
					if (activeAction.Start() == false)
					{
						activeAction = null;
						waitEvent.Set();
						result = false;
						NotifyStopped();
					}
				}

				lock (lockObject)
				{
					activeAction = null;
				}

				// should stop ?
				if (GetStopped() == true)
				{
					waitEvent.Set();
					result = false;
					NotifyStopped();
				}
			}

			SetStopped(true);
			waitEvent.Set();
			NotifyStopped();
		}

		#endregion

		#region Public methods

		public void Start()
		{
			StartImpl(null, null);
		}		

		public void StartAsync()
		{
			EventHandler e = new EventHandler(StartImpl);
			e.BeginInvoke(null, null, null, null);
		}

		public void Stop()
		{
			lock (lockObject)
			{
				if (activeAction != null)
				{
					activeAction.Stop();
				}
			}

			waitEvent.Set();
		}

		public void EndStart()
		{
			// wait until all actions are executed or stopped
			waitEvent.WaitOne();
		}

		#endregion
	}
}