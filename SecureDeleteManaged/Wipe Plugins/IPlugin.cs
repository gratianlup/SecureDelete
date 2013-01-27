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
	/// Interface that needs to be used and implemented by all plugins.
	/// </summary>
	public interface IPlugin
	{
		bool IsApplicationInstalled { get; }
		bool Load();
		bool Unload();
		byte[] SaveSettings();
		bool LoadSettings(byte[] data);
		bool HasOptionsDialog { get; }
		object GetOptionsDialog();
		bool HasIcon { get; }
		object GetIcon();
		IWipeObject[] GetWipeObjects();
		void Stop();
	}
}