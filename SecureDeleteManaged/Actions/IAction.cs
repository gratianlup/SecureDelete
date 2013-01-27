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

namespace SecureDelete.Actions
{
	/// <summary>
	/// Interface from which all actions need to derive.
	/// </summary>
	public interface IAction
	{
		bool        Enabled { get;set; }
		WipeSession Session { get;set;}
		bool        AfterWipe { get;set;}
		bool        BlockingMode { get;set;}
		bool        HasMaximumExecutionTime { get;set;}
		TimeSpan    MaximumExecutionTime { get;set;}
		bool        Start();
		void        Stop();
		bool        EndStart();
		object      Clone();
	}


	public class ActionErrorReporter
	{
		public static void ReportError(WipeSession session, bool afterWipe, ErrorSeverity severity, string format, params object[] args)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}

			// report to the debugger
			switch (severity)
			{
				case ErrorSeverity.High:
					{
						DebugUtils.Debugger.Debug.ReportError(format, args);
						break;
					}
				case ErrorSeverity.Low:
					{
						DebugUtils.Debugger.Debug.Report(format, args);
						break;
					}
				case ErrorSeverity.Medium:
					{
						DebugUtils.Debugger.Debug.ReportWarning(format, args);
						break;
					}
			}

			// report to the session
			if (session != null)
			{
				WipeError error = new WipeError(DateTime.Now, severity, string.Format(format, args));

				if (afterWipe)
				{
					if (session.AfterWipeErrors == null)
					{
						session.AfterWipeErrors = new List<WipeError>();
					}

					session.AfterWipeErrors.Add(error);
				}
				else
				{
					if (session.BeforeWipeErrors == null)
					{
						session.BeforeWipeErrors = new List<WipeError>();
					}

					session.BeforeWipeErrors.Add(error);
				}
			}
		}
	}
}