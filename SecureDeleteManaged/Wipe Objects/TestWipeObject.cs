// ***************************************************************
//  SecureDelete   version:  1.0
//  -------------------------------------------------------------
//
//  Copyright (C) 2008 Lup Gratian - All Rights Reserved
//   
// ***************************************************************      

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SecureDelete.WipeObjects
{
	/// <summary>
	/// Class used only for testing the asynchronous wipe mode.
	/// </summary>
	class TestWipeObject : IWipeObject
	{
		ManualResetEvent w;

		#region IWipeObject Members

		public WipeObjectType Type
		{
			get { return WipeObjectType.Registry; }
		}

		public bool SingleObject
		{
			get { return true; }
		}

		private WipeOptions _options;
		public SecureDelete.WipeOptions Options
		{
			get { return _options; }
			set { _options = value; }
		}

		private int _wipeMethodId;
		public int WipeMethodId
		{
			get { return _wipeMethodId; }
			set { _wipeMethodId = value; }
		}
		public NativeMethods.WObject GetObject()
		{
			// wait for 30 seconds
			w = new ManualResetEvent(false);
			w.WaitOne(TimeSpan.FromSeconds(30), true);
			w = null;

			return new NativeMethods.WObject();
		}

		public NativeMethods.WObject[] GetObjects()
		{
			return null;
		}

		public void Stop()
		{
			if (w != null)
			{
				w.Set();
			}
		}

		#endregion
	}
}
