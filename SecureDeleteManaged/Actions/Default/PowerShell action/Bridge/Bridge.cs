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

namespace SecureDelete.Actions
{
	/// <summary>
	/// Interface that needs to be implemented by all bridge objects.
	/// </summary>
	public interface IBridge
	{
		bool TestMode { get; set; }
		BridgeLogger Logger { get; set; }
		string Name { get; }
		void Open();
		void Close();
	}


	public interface IFullAccessBridge
	{
		WipeSession Session { get;set;}
		bool AfterWipe { get;set;}
	}

	#region Attributes

	/// <summary>
	/// Attribute that needs to be applied to all bridge objects.
	/// </summary>
	public class Bridge : Attribute
	{
		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private bool _exposed;
		public bool Exposed
		{
			get { return _exposed; }
			set { _exposed = value; }
		}
	}


	/// <summary>
	/// Attribute that needs to be applied to all members of the object that should be exposed.
	/// </summary>
	public class BridgeMember : Attribute
	{
		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private string _signature;
		public string Signature
		{
			get { return _signature; }
			set { _signature = value; }
		}

		private string _description;
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}
	}

	/// <summary>
	/// Attribute that needs to be applied for each parameter of the exposed method.
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class BridgeMemberParameter : Attribute
	{
		private int _order;
		public int Order
		{
			get { return _order; }
			set { _order = value; }
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
	}

	#endregion
}