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
	public class DailySchedule : ScheduleBase
	{
		#region Fields

		[NonSerialized]
		ScheduleTimer timer;

		#endregion

		#region Constructor

		public DailySchedule()
		{
			timer = new ScheduleTimer();
			_enabled = true;
		}

		#endregion

		#region Destructor

		~DailySchedule()
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
			get { return ScheduleType.Daily; }
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
			DateTime now = DateTime.Now;

			// compute the next schedule time
			DateTime next = new DateTime(now.Year, now.Month, now.Day, _time.StartTime.Hour, _time.StartTime.Minute,
									_time.StartTime.Second, _time.StartTime.Millisecond);

			if (_time.RecurenceValue > 0)
			{
				TimeSpan diff = now - _time.StartTime;
				int recurence = _time.RecurenceValue;

				next = next.AddDays(((recurence - 1) - (diff.Days % recurence)) + (diff.Milliseconds > 0 ? 1 : 0));
			}

			// verify if the time is in the valid range
			if (_time.HasEndTime && next > _time.EndTime)
			{
				// schedule expired
				return null;
			}

			return next;
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
				bool result = timer.StartTimer(interval);
				_running = result;

				return result;
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
			DailySchedule temp = new DailySchedule();

			temp._enabled = _enabled;
			temp._running = _running;
			temp._time = (ScheduleTime)_time.Clone();

			return temp;
		}

		#endregion
	}
}
