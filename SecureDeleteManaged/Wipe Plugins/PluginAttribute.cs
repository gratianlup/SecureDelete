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
using System.Reflection;
using DebugUtils.Debugger;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace SecureDelete.WipePlugin
{
	/// <summary>
	/// Attribute that needs to be used with all plugins.
	/// Specifies information about the plugin (name, version, author, etc.)
	/// </summary>
	public class PluginAttribute : Attribute
	{
		#region Properties

		private Guid _id;
		public Guid Id
		{
			get { return _id; }
			set { _id = value; }
		}

		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private string _description;
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		private int _majorVersion;
		public int MajorVersion
		{
			get { return _majorVersion; }
			set { _majorVersion = value; }
		}

		private int _minorVersion;
		public int MinorVersion
		{
			get { return _minorVersion; }
			set { _minorVersion = value; }
		}

		private string _author;
		public string Author
		{
			get { return _author; }
			set { _author = value; }
		}

		#endregion

		#region Public methods

		public PluginAttribute(string id, string name, string description, int majorVersion, int minorVersion)
		{
			_id = new Guid(id);
			_name = name;
			_description = description;
			_majorVersion = majorVersion;
			_minorVersion = minorVersion;
		}

		public PluginAttribute(string id, string name)
		{
			_id = new Guid(id);
			_name = name;
			_description = null;
			_majorVersion = int.MaxValue;
			_minorVersion = int.MaxValue;
		}

		#endregion
	}
}