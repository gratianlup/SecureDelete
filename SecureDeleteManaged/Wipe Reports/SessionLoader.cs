// ***************************************************************
//  SecureDelete   version:  1.0
//  -------------------------------------------------------------
//
//  Copyright (C) 2007 Lup Gratian - All Rights Reserved
//   
// ***************************************************************         

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using LGratian.Debugger;

namespace SecureDelete
{
	public class SessionLoader
	{
		#region Properties

		private byte[] _key;
		public byte[] Key
		{
			get { return _key; }
			set { _key = value; }
		}

		private byte[] _IV;
		public byte[] IV
		{
			get { return _IV; }
			set { _IV = value; }
		}

		#endregion

		#region Private methods

		private bool DecryptStream(MemoryStream stream)
		{
			return true;
		}

		#endregion

		#region Public methods

		public bool DeserializeSession(string file, out WipeSession session)
		{
			session = null;
			return false;
		}

		#endregion
	}
}
