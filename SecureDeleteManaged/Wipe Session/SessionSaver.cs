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
	public class SessionSaver
	{
		#region Public methods

		public static byte[] SerializeSession(WipeSession session)
		{
			BinaryFormatter serializer = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();

			try
			{
				// serialize in memory
				serializer.Serialize(stream, session);
				return stream.ToArray();
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while serializing session. Exception: {0}", e.Message);
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


		public static bool SaveSession(WipeSession session,string path)
		{
			// check the parameters
			if (session == null || path == null)
			{
				throw new ArgumentNullException("session | path");
			}

			try
			{
				// create the store
                FileStore.FileStore store = new FileStore.FileStore();
				store.Encrypt = true;
				store.UseDPAPI = true;

				// add the file
                FileStore.StoreFile file = store.CreateFile("session.dat");
				byte[] data = SerializeSession(session);

				if (data == null)
				{
					return false;
				}

				// write the file contents
                store.WriteFile(file, data, FileStore.StoreMode.Encrypted);

				// save the store
				return store.Save(path);
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while saving session. Exception: {0}", e.Message);
				return false;
			}
		}

		#endregion
	}
}
