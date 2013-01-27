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

namespace SecureDelete
{
	public enum ErrorSeverity
	{
		High, Medium, Low
	}


	[Serializable]
	public class WipeError
	{
		#region Properties

		private DateTime _time;
		public DateTime Time
		{
			get { return _time; }
			set { _time = value; }
		}

		private ErrorSeverity _severity;
		public ErrorSeverity Severity
		{
			get { return _severity; }
			set { _severity = value; }
		}

		private string _message;
		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}

		#endregion

		#region Constructor

		public WipeError()
		{

		}

		public WipeError(DateTime time, ErrorSeverity severity, string message)
		{
			_time = time;
			_severity = severity;
			_message = message;
		}

		#endregion

		#region Public methods

		public void ReadNative(NativeMethods.WError error)
		{
			_time = new DateTime(error.time.wYear, error.time.wMonth, error.time.wDay, error.time.wHour, error.time.wMinute, error.time.wSecond, error.time.wMilliseconds);

			switch (error.severity)
			{
				case NativeMethods.SEVERITY_HIGH:
					{
						_severity = ErrorSeverity.High;
						break;
					}
				case NativeMethods.SEVERITY_MEDIUM:
					{
						_severity = ErrorSeverity.Medium;
						break;
					}
				case NativeMethods.SEVERITY_LOW:
					{
						_severity = ErrorSeverity.Low;
						break;
					}
			}

			_message = error.message;
		}

		#endregion
	}


	[Serializable]
	public class FailedObject
	{
		#region Properties

		private WipeObjectType _type;
		public WipeObjectType Type
		{
			get { return _type; }
			set { _type = value; }
		}

		private string _path;
		public string Path
		{
			get { return _path; }
			set { _path = value; }
		}

		private WipeError _associatedError;
		public WipeError AssociatedError
		{
			get { return _associatedError; }
			set { _associatedError = value; }
		}

		#endregion

		#region Constructor

		public FailedObject()
		{

		}

		public FailedObject(WipeObjectType type, string path, WipeError error)
		{
			_type = type;
			_path = path;
			_associatedError = error;
		}

		#endregion

		#region Public methods

		public void ReadNative(NativeMethods.WSmallObject failed)
		{
			switch (failed.type)
			{
				case NativeMethods.TYPE_FILE:
					{
						_type = WipeObjectType.File;
						break;
					}
				case NativeMethods.TYPE_FOLDER:
					{
						_type = WipeObjectType.Folder;
						break;
					}
				case NativeMethods.TYPE_DRIVE:
					{
						_type = WipeObjectType.Drive;
						break;
					}
			}

			_path = failed.path;
		}

		#endregion
	}
}
