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
using DebugUtils.Debugger;

namespace SecureDelete
{
	public class SessionLoader
	{
		#region Public methods

		public static WipeSession DeserializeSession(byte[] data)
		{
			if (data == null)
			{
				return null;
			}

			BinaryFormatter serializer = new BinaryFormatter();
			MemoryStream stream = new MemoryStream(data);

			try
			{
				return (WipeSession)serializer.Deserialize(stream);
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while deserializing session. Exception: {0}", e.Message);
				return null;
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
		}


		public static bool LoadSession(string path, out WipeSession session)
		{
			// check the parameters
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}

			session = null;

			try
			{
				// create the store
                FileStore.FileStore store = new FileStore.FileStore();
				store.Encrypt = true; // use encryption
				store.UseDPAPI = true; // use default encryption

				// load store
				if (store.Load(path) == false)
				{
					Debug.ReportError("Error while loading store from path {0}", path);
					return false;
				}

				// deserialize
				session = DeserializeSession(store.ReadFile("session.dat"));
				return true;
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while loading session. Exception: {0}", e.Message);
				return false;
			}
		}

		#endregion
	}
}
