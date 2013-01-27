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
	[Serializable]
	public class HistoryItem
	{
		#region Constructor

		public HistoryItem()
		{

		}

		public HistoryItem(Guid guid, DateTime startTime)
		{
			_sessionGuid = guid;
			_startTime = startTime;
		}

		#endregion

		#region Properties

		private Guid _sessionGuid;
		public Guid SessionGuid
		{
			get { return _sessionGuid; }
			set { _sessionGuid = value; }
		}

		private DateTime _startTime;
		public System.DateTime StartTime
		{
			get { return _startTime; }
			set { _startTime = value; }
		}

		private DateTime? _endTime;
		public DateTime? EndTime
		{
			get { return _endTime; }
			set { _endTime = value; }
		}

		public TimeSpan? DeltaTime
		{
			get
			{
				if (_endTime.HasValue == false)
				{
					return null;
				}

				return _endTime - _startTime;
			}
		}

		private bool _failed;
		public bool Failed
		{
			get { return _failed; }
			set { _failed = value; }
		}

		private WipeStatistics _statistics;
		public SecureDelete.WipeStatistics Statistics
		{
			get { return _statistics; }
			set { _statistics = value; }
		}

		private ReportInfo _reportInfo;
		public ReportInfo ReportInfo
		{
			get { return _reportInfo; }
			set { _reportInfo = value; }
		}

		#endregion

		#region Static methods

		public static bool IsOk(HistoryItem item)
		{
			return item._endTime.HasValue && item._failed == false;
		}

		public static bool IsFailed(HistoryItem item)
		{
			return item._endTime.HasValue == false;
		}

		public static bool IsWithErrors(HistoryItem item)
		{
			return item._endTime.HasValue && item._failed == true;
		}

		#endregion
	}
}
