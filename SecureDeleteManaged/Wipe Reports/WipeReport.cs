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
using DebugUtils.Debugger;

namespace SecureDelete
{
	[Serializable]
	public class WipeStatistics : ICloneable
	{
		private DateTime _startTime;
		public DateTime StartTime
		{
			get { return _startTime; }
			set { _startTime = value; }
		}

		private DateTime _endTime;
		public DateTime EndTime
		{
			get { return _endTime; }
			set { _endTime = value; }
		}

		private TimeSpan _duration;
		public TimeSpan Duration
		{
			get { return _duration; }
			set { _duration = value; }
		}

		private int _failedObjects;
		public int FailedObjects
		{
			get { return _failedObjects; }
			set { _failedObjects = value; }
		}

		private int _errors;
		public int Errors
		{
			get { return _errors; }
			set { _errors = value; }
		}

		private long _averageWriteSpeed;
		public long AverageWriteSpeed
		{
			get { return _averageWriteSpeed; }
			set { _averageWriteSpeed = value; }
		}

		private long _totalWipedBytes;
		public long TotalWipedBytes
		{
			get { return _totalWipedBytes; }
			set { _totalWipedBytes = value; }
		}

		private long _bytesInClusterTips;
		public long BytesInClusterTips
		{
			get { return _bytesInClusterTips; }
			set { _bytesInClusterTips = value; }
		}

		#region ICloneable Members

		public object Clone()
		{
			WipeStatistics temp = new WipeStatistics();

			temp._averageWriteSpeed = _averageWriteSpeed;
			temp._bytesInClusterTips = _bytesInClusterTips;
			temp._duration = _duration;
			temp._endTime = _endTime;
			temp._errors = _errors;
			temp._failedObjects = _failedObjects;
			temp._startTime = _startTime;
			temp._totalWipedBytes = _totalWipedBytes;

			return temp;
		}

		#endregion
	}


	[Serializable]
	public class WipeReport
	{
		public WipeReport()
		{
			_failedObjects = new List<FailedObject>();
			_errors = new List<WipeError>();
		}

		public WipeReport(Guid sessionGuid)
		{
			_failedObjects = new List<FailedObject>();
			_errors = new List<WipeError>();
			_sessionGuid = sessionGuid;
		}

		private Guid _sessionGuid;
		public Guid SessionGuid
		{
			get { return _sessionGuid; }
			set { _sessionGuid = value; }
		}

		private WipeStatistics _statistics;
		public WipeStatistics Statistics
		{
			get { return _statistics; }
			set { _statistics = value; }
		}

		private List<FailedObject> _failedObjects;
		public List<FailedObject> FailedObjects
		{
			get { return _failedObjects; }
			set { _failedObjects = value; }
		}

		private List<WipeError> _errors;
		public List<WipeError> Errors
		{
			get { return _errors; }
			set { _errors = value; }
		}
	}
}
