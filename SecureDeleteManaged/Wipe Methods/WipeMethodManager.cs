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
using DebugUtils.Debugger;

namespace SecureDelete
{
	/// <summary>
	/// Contains the ID's of the built-in wipe methods.
	/// </summary>
	public class WipeMethodType
	{
		public static int Random = 111909167;
		public static int Russian_GHOST = 153029903;
		public static int German_VSITR = 1642641729;
		public static int Gutmann = 547574234;
		public static int DOD = 643029365;
		public static int Schneier = 792101038;
	}


	public class WipeMethodManager
	{
		#region Constants

		public const string MethodFileExtension = ".sdm";

		#endregion

		#region Fields

		private List<WipeMethod> wipeMethods;
		Random rand;

		#endregion

		#region Constructor

		public WipeMethodManager()
		{
			wipeMethods = new List<WipeMethod>();
			rand = new Random((int)DateTime.Now.Ticks);
		}

		#endregion

		#region Properties

		public List<WipeMethod> Methods
		{
			get { return wipeMethods; }
		}

		private string _folder;
		public string Folder
		{
			get { return _folder; }
			set { _folder = value; }
		}

		#endregion

		public WipeMethod CreateMethod()
		{
			WipeMethod method = new WipeMethod();
			
			// get a valid ID
			bool valid = false;
			int id = rand.Next();

			while (valid == false)
			{
				valid = true;

				for (int i = 0; i < wipeMethods.Count; i++)
				{
					if (wipeMethods[i].Id == id)
					{
						valid = false;
						break;
					}
				}

				id = rand.Next();
			}

			// set the ID
			method.Id = id;

			// add to the category
			wipeMethods.Add(method);

			return method;
		}


		public bool RemoveMethod(WipeMethod method, bool removeFromDisk)
		{
			Debug.AssertNotNull(method, "Method is null");

			if (removeFromDisk)
			{
				try
				{
					string path = Path.Combine(_folder, method.Id.ToString() + MethodFileExtension);

					if (File.Exists(path))
					{
						File.Delete(path);
					}
					else
					{
						Debug.ReportError("Method not found. File: {0}", path);
					}
				}
				catch (Exception e)
				{
					Debug.ReportError("Error while deleting method. Method ID: {0}, Folder: {1}, Exception: {2}", method.Id, _folder, e.Message);
					return false;
				}
			}

			// remove from category
			wipeMethods.Remove(method);

			return true;
		}


		public bool SaveMethod(WipeMethod method)
		{
			Debug.AssertNotNull(method, "Method is null");

			string path = _folder;
			if (path[path.Length - 1] != '\\')
			{
				path += '\\';
			}

			path += method.Id.ToString() + ".sdm";
			return method.SaveNative(path);
		}


		public WipeMethod GetMethod(int id)
		{
			if (wipeMethods.Count == 0)
			{
				return null;
			}

			for (int i = 0; i < wipeMethods.Count; i++)
			{
				if (wipeMethods[i].Id == id)
				{
					return wipeMethods[i];
				}
			}

			return null;
		}


		public int GetMethodIndex(int id)
		{
			for (int i = 0; i < wipeMethods.Count; i++)
			{
				if (wipeMethods[i].Id == id)
				{
					return i;
				}
			}

			return -1;
		}


		public bool ScanFolder()
		{
			Debug.AssertNotNull(_folder, "Folder not set");

			// get the files
			try
			{
				string[] files = Directory.GetFiles(_folder, "*" + MethodFileExtension, SearchOption.TopDirectoryOnly);

				if (files != null && files.Length > 0)
				{
					for (int i = 0; i < files.Length; i++)
					{
						WipeMethod method = new WipeMethod();

						if (method.ReadNative(files[i]) == false)
						{
							Debug.ReportWarning("Failed to load method. File: {0}", files[i]);
						}
						else
						{
							// add to the category
							wipeMethods.Add(method);
						}
					}
				}
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while loading the methods. Folder: {0}, Exception: {1}", _folder, e.Message);
				return false;
			}

			return true;
		}
	}
}
