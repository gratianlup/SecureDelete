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
	public enum WipeObjectType
	{
		File,Folder,Drive,Plugin,Registry,ClusterTips,MFT
	}

	/// <summary>
	/// Interface that needs to be used by all WipeObjects
	/// </summary>
	public interface IWipeObject
	{
		WipeObjectType          Type { get; }
		bool                    SingleObject { get; }
		WipeOptions             Options { get;set;}
		int                     WipeMethodId { get; set;}

		/// <summary>
		/// Used when SingleObject is true
		/// </summary>
		NativeMethods.WObject   GetObject();
		NativeMethods.WObject[] GetObjects();
		void Stop();
	}
}
