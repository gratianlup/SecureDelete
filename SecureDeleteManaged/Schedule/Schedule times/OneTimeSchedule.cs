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
using System.Runtime.Serialization;

namespace SecureDelete.Schedule
{
	[Serializable]
	public class OneTimeSchedule : ScheduleBase
	{
		#region Fields

		[NonSerialized]
		ScheduleTimer timer;

		#endregion

		#region Constructor

		public OneTimeSchedule()
		{
			timer = new ScheduleTimer();
			_enabled = true;
		}

		#endregion

		#region Destructor

		~OneTimeSchedule()
		{
			if (timer != null)
			{
				timer.StopTimer();
			}
		}

		#endregion

		#region Properties

		public override ScheduleType Type
		{
			get { return ScheduleType.OneTime; }
		}

		public override bool IsTimed
		{
			get { return true; }
		}

		#endregion

		#region Events

		public override event TaskStartDelegate TaskStarted;

		#endregion

		#region Private methods

		private void OnTimeEllapsed(object sender, EventArgs e)
		{
			StopSchedule();

			if (TaskStarted != null)
			{
				TaskStarted(this);
			}
		}

		#endregion

		#region Public methods

		public override DateTime? GetScheduleTime()
		{
			// verify if the time is in the valid range
			if (DateTime.Now > _time.StartTime)
			{
				// schedule expired
				return null;
			}

			return _time.StartTime;
		}


		public override bool StartSchedule()
		{
			if (_enabled == false)
			{
				return false;
			}

			if (_running)
			{
				return false;
			}

			// get the due time
			DateTime? dueTime = GetScheduleTime();

			if (dueTime.HasValue)
			{
				if (timer != null)
				{
					timer.StopTimer();
				}

				TimeSpan interval = dueTime.Value - DateTime.Now;
				timer.OnTimeEllapsed += OnTimeEllapsed;
				timer.StartTimer(interval);
				_running = true;

				return true;
			}

			return false;
		}


		public override bool StopSchedule()
		{
			if (_running && timer != null)
			{
				timer.StopTimer();
				_running = false;

				return true;
			}

			return false;
		}

		#endregion

		#region Serialization helpers

		[OnSerializing()]
		internal void OnSerializingMethod(StreamingContext context)
		{
			TaskStarted = null;
		}

		[OnDeserialized()]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			timer = new ScheduleTimer();
		}

		#endregion

		#region ICloneable Members

		public override object Clone()
		{
			OneTimeSchedule temp = new OneTimeSchedule();

			temp._enabled = _enabled;
			temp._running = _running;
			temp._time = (ScheduleTime)_time.Clone();

			return temp;
		}

		#endregion
	}
}
