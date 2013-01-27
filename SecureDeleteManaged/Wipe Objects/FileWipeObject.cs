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

namespace SecureDelete.WipeObjects
{
	[Serializable]
	public class FileWipeObject : IWipeObject
	{
		#region IWipeObject Members

		public WipeObjectType Type
		{
			get { return WipeObjectType.File; }
		}

		public bool SingleObject
		{
			get { return true; }
		}

		private WipeOptions _options;
		public WipeOptions Options
		{
			get { return _options; }
			set { _options = value; }
		}


		/// <summary>
		/// Get the file object
		/// </summary>
		public NativeMethods.WObject GetObject()
		{
			NativeMethods.WObject file = new NativeMethods.WObject();

			file.type = NativeMethods.TYPE_FILE;
			file.path = _path;
			file.aux = null;
			file.wipeMethod = _wipeMethodId == WipeOptions.DefaultWipeMethod ? _options.DefaultFileMethod : _wipeMethodId;
			file.wipeOptions = 0;

			// set the options
			if (_options.WipeAds)
			{
				file.wipeOptions |= NativeMethods.WIPE_ADS;
			}

			if (_options.WipeFileName)
			{
				file.wipeOptions |= NativeMethods.WIPE_FILENAME;
			}

			return file;
		}


		/// <summary>
		/// Not supported
		/// </summary>
		public NativeMethods.WObject[] GetObjects()
		{
			throw new Exception("GetObjects should not be called");
		}


		/// <summary>
		/// Not implemented
		/// </summary>
		public void Stop()
		{

		}

		#endregion

		#region Properties

		private string _path;
		public string Path
		{
			get { return _path; }
			set { _path = value; }
		}

		private int _wipeMethodId;
		public int WipeMethodId
		{
			get { return _wipeMethodId; }
			set { _wipeMethodId = value; }
		}

		#endregion
	}
}