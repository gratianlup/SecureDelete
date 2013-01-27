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

namespace SecureDelete.Schedule
{
	public enum ScheduleType
	{
		OneTime, Daily, Weekly, Monthly	
	}


	[Serializable]
	public class ScheduleTime : ICloneable
	{
		public ScheduleTime()
		{
			StartTime = DateTime.Now;
			HasEndTime = false;
			EndTime = DateTime.Now;
			RecurenceValue = 1;
		}

		public DateTime StartTime;
		public bool     HasEndTime;
		public DateTime EndTime;
		public int   	RecurenceValue;

		#region ICloneable Members

		public object Clone()
		{
			ScheduleTime temp = new ScheduleTime();

			temp.StartTime = StartTime;
			temp.HasEndTime = HasEndTime;
			temp.EndTime = EndTime;
			temp.RecurenceValue = RecurenceValue;

			return temp;
		}

		#endregion
	}


	public delegate void TaskStartDelegate(ISchedule schedule);


	/// <summary>
	/// Interface that needs to be implemented by all schedule type classes
	/// </summary>
	public interface ISchedule : ICloneable
	{
		ScheduleType Type { get;}
		bool Enabled { get; set;}
		bool IsTimed { get;}
		bool Running { get;}
		ScheduleTime Time { get;set;}
		DateTime? GetScheduleTime();
		bool StartSchedule();
		bool StopSchedule();
		event TaskStartDelegate TaskStarted;
		object Clone();
	}

	[Serializable]
	public abstract class ScheduleBase : ISchedule, ICloneable
	{
		#region Constructor

		public ScheduleBase()
		{
			_time = new ScheduleTime();
		}

		#endregion

		public abstract ScheduleType Type { get;}

		protected bool _enabled;
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		[NonSerialized]
		protected bool _running;
		public bool Running
		{
			get { return _running; }
			set { _running = value; }
		}

		public abstract bool IsTimed { get;}

		protected ScheduleTime _time;
		public ScheduleTime Time
		{
			get { return _time; }
			set { _time = value; }
		}

		public abstract DateTime? GetScheduleTime();
		public abstract bool StartSchedule();
		public abstract bool StopSchedule();

		public abstract event TaskStartDelegate TaskStarted;

		public abstract object Clone();
	}
}
