// ***************************************************************
//  SecureDelete   version:  1.0
//  -------------------------------------------------------------
//
//  Copyright (C) 2007 Lup Gratian - All Rights Reserved
//   
// ***************************************************************   

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DebugUtils.Debugger;
using System.Runtime.Serialization.Formatters.Binary;

namespace SecureDelete.Schedule
{
	[Serializable]
	public class HistoryCategory
	{
		#region Constructor

		public HistoryCategory()
		{
			Items = new SortedList<DateTime, HistoryItem>();
		}

		#endregion

		public Guid Guid;
		public SortedList<DateTime, HistoryItem> Items;
	}


	public class HistoryManager
	{
		#region Constructor

		public HistoryManager()
		{
			_categories = new Dictionary<Guid, HistoryCategory>();
		}

		#endregion

		#region Properties

		private Dictionary<Guid, HistoryCategory> _categories;
		public Dictionary<Guid, HistoryCategory> Categories
		{
			get { return _categories; }
			set { _categories = value; }
		}

		#endregion

		#region Private methods

		private byte[] SerializeHistory(Dictionary<Guid, HistoryCategory> categories)
		{
			MemoryStream stream = new MemoryStream();

			try
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, categories);

				return stream.ToArray();
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while serializing task history. Exception: {0}", e.Message);
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


		private Dictionary<Guid, HistoryCategory> DeserializeHistory(byte[] data)
		{
			if (data == null)
			{
				return null;
			}

			BinaryFormatter serializer = new BinaryFormatter();
			MemoryStream stream = new MemoryStream(data);

			try
			{
				return (Dictionary<Guid, HistoryCategory>)serializer.Deserialize(stream);
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while deserializing task history. Exception: {0}", e.Message);
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

		#endregion

		#region Public methods

		public bool Add(HistoryItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}

			HistoryCategory category = null;

			if (_categories.ContainsKey(item.SessionGuid))
			{
				category = _categories[item.SessionGuid];
			}
			else
			{
				// create new category
				category = new HistoryCategory();
				category.Guid = item.SessionGuid;

				_categories.Add(category.Guid, category);
			}

			// add the item to the category
			try
			{
				category.Items.Add(item.StartTime, item);
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while adding history item to category. Exception {0}", e.Message);
				return false;
			}

			return true;
		}


		public IList<HistoryItem> GetItems(Guid guid)
		{
			if (_categories.ContainsKey(guid))
			{
				return _categories[guid].Items.Values;
			}

			return null;
		}


		public void Remove(HistoryItem item)
		{
			if (_categories.ContainsKey(item.SessionGuid))
			{
				HistoryCategory category = _categories[item.SessionGuid];

				if (category.Items.ContainsKey(item.StartTime))
				{
					category.Items.Remove(item.StartTime);
				}
			}
		}


		public void RemoveCategory(Guid guid)
		{
			if (_categories.ContainsKey(guid))
			{
				_categories.Remove(guid);
			}
		}


		public void Clear()
		{
			_categories = new Dictionary<Guid, HistoryCategory>();
		}


		/// <summary>
		/// Save the history database to disk.
		/// </summary>
		public bool SaveHistory(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}

			try
			{
				// create the store
                FileStore.FileStore store = new FileStore.FileStore();
				store.Encrypt = true;
				store.UseDPAPI = true;

				// add the file
                FileStore.StoreFile file = store.CreateFile("history.dat");
				byte[] data = SerializeHistory(_categories);

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
				Debug.ReportError("Error while saving task history. Exception: {0}", e.Message);
				return false;
			}
		}


		/// <summary>
		/// Load the report database from disk.
		/// </summary>
		public bool LoadHistory(string path)
		{
			// check if the file exists
			if (File.Exists(path) == false)
			{
				Debug.ReportWarning("History file not found. Path: {0}", path);
				return false;
			}

			try
			{
				// create the store
                FileStore.FileStore store = new FileStore.FileStore();
				store.Encrypt = true;
				store.UseDPAPI = true;

				// load store
				if (store.Load(path) == false)
				{
					Debug.ReportError("Error while loading store from path {0}", path);
					return false;
				}

				// deserialize
				_categories = DeserializeHistory(store.ReadFile("history.dat"));

				if (_categories == null)
				{
					// load failed, allocate history categories
					_categories = new Dictionary<Guid, HistoryCategory>();
					return false;
				}

				return true;
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while loading task history. Exception: {0}", e.Message);
				return false;
			}
		}

		#endregion
	}
}