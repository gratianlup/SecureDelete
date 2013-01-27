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
using System.Threading;

namespace SecureDelete.Schedule
{
	class ScheduleTimer
	{
		#region Fields

		private Timer timer;

		#endregion

		#region Properties

		private bool _running;
		public bool Running
		{
			get { return _running; }
			set { _running = value; }
		}

		#endregion

		#region Events

		public EventHandler OnTimeEllapsed;

		#endregion

		#region Private methods

		private void TimeEllapsed(object state)
		{
			_running = false;

			// announce the parent
			if (OnTimeEllapsed != null)
			{
				OnTimeEllapsed(this, null);
			}
		}

		#endregion

		#region Public methods

		public bool StartTimer(TimeSpan dueTime)
		{
			if (_running)
			{
				return false;
			}

			if (timer != null)
			{
				timer.Dispose();
			}

			if (dueTime.TotalMilliseconds < 0)
			{
				return false;
			}

			timer = new Timer(TimeEllapsed, null, (long)dueTime.TotalMilliseconds,Timeout.Infinite);
			_running = true;

			return true;
		}


		public void StopTimer()
		{
			if (timer != null)
			{
				timer.Change(Timeout.Infinite, Timeout.Infinite);
				timer.Dispose();
				timer = null;
				_running = false;
			}
		}

		#endregion
	}
}
