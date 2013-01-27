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
using System.Reflection;
using System.Diagnostics;

namespace SecureDelete.Actions
{
	#region Logger

	public class MethodParameterInfo
	{
		#region Properties

		private ParameterInfo _info;
		public System.Reflection.ParameterInfo Info
		{
			get { return _info; }
			set { _info = value; }
		}

		private object _value;
		public object Value
		{
			get { return _value; }
			set { _value = value; }
		}

		#endregion

		#region Constructor

		public MethodParameterInfo()
		{

		}

		public MethodParameterInfo(ParameterInfo info, object value)
		{
			_info = info;
			_value = value;
		}

		#endregion
	}


	public class BridgeLoggerItem
	{
		#region Properties

		private MethodBase _method;
		public MethodBase Method
		{
			get { return _method; }
			set { _method = value; }
		}

		private DateTime _time;
		public System.DateTime Time
		{
			get { return _time; }
			set { _time = value; }
		}

		private List<MethodParameterInfo> _parameters;
		public List<MethodParameterInfo> Parameters
		{
			get { return _parameters; }
			set { _parameters = value; }
		}

		#endregion
	}


	/// <summary>
	/// Helper object which logs member calls when running the script in test mode.
	/// </summary>
	public class BridgeLogger
	{
		#region Fields

		// make the class thread-safe
		private object lockObject;

		#endregion

		#region Constructor

		public BridgeLogger()
		{
			lockObject = new object();
			_items = new List<BridgeLoggerItem>();
		}

		#endregion

		#region Properties

		private List<BridgeLoggerItem> _items;
		public List<BridgeLoggerItem> Items
		{
			get { return _items; }
			set { _items = value; }
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Log a member call.
		/// </summary>
		/// <param name="parameters">The parameters the member received.</param>
		/// <remarks>The parameters need to be passed to this method in exactly the same order they appear in the member definition.</remarks>
		public void LogMethodCall(params object[] parameters)
		{
			BridgeLoggerItem item = new BridgeLoggerItem();

			// basic info
			item.Time = DateTime.Now;

			// get method info from stack
			StackTrace stackInfo = new StackTrace();
			StackFrame topFrame = stackInfo.GetFrame(1);

			MethodBase method = topFrame.GetMethod();
			item.Method = method;

			// add the parameters
			ParameterInfo[] paramInfo = method.GetParameters();
			item.Parameters = new List<MethodParameterInfo>();

			if (method.Name.StartsWith("set_"))
			{
				if (parameters.Length > 0)
				{
					MethodParameterInfo param = new MethodParameterInfo();
					param.Value = parameters[0];
				}
			}
			else
			{
				for (int i = 0; i < paramInfo.Length; i++)
				{
					MethodParameterInfo param = new MethodParameterInfo();
					param.Info = paramInfo[i];

					if (parameters.Length > i)
					{
						param.Value = parameters[i];
					}

					// add to the list
					item.Parameters.Add(param);
				}
			}
			
			// add to the list
			_items.Add(item);
		}


		/// <summary>
		/// Clear the list of logged member calls.
		/// </summary>
		public void Clear()
		{
			if (_items != null)
			{
				_items.Clear();
			}
		}

		#endregion
	}

	#endregion

	#region Property reader

	public class PropertyReader
	{
		public object GetPropertyValue(string name, object obj, int index)
		{
			if (name == null ||obj == null)
			{
				throw new ArgumentNullException();
			}

			try
			{
				Type type = obj.GetType();
				PropertyInfo info = type.GetProperty(name);

				return info.GetValue(name, index != -1 ? new object[] { index } : null);
			}
			catch
			{
				return null;
			}
		}
	}

	#endregion
}