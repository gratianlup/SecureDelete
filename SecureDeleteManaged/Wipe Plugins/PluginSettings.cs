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
	/// Represents the ID used to identify the plugin saved data.
	/// </summary>
	[Serializable]
	public struct StoreId
	{
		public Guid id;
		public string name;
		public int majorVersion;
		public int minorVersion;

		public override string ToString()
		{
			string result = name;

			if (majorVersion != int.MaxValue)
			{
				result += majorVersion.ToString();

				if (minorVersion != int.MaxValue)
				{
					result += "." + minorVersion.ToString();
				}
			}

			return result;
		}
	}


	/// <summary>
	/// Represents the data that a plugin stores.
	/// </summary>
	[Serializable]
	public struct StoreItem
	{
		public StoreId id;
		public byte[] data;
	}


	/// <summary>
	/// Provides a centralized storage for all plugin settings.
	/// </summary>
	public class PluginSettings
	{
		#region Nested types

		public enum AutoSaveMethod
		{
			None,
			Block,
			Backgoround
		}

		#endregion

		#region Fields

		private Dictionary<StoreId, StoreItem> settings;
		private object saveLock;

		#endregion

		#region Properties

		private string _settingsFile;
		public string SettingsFile
		{
			get { return _settingsFile; }
			set { _settingsFile = value; }
		}

		private AutoSaveMethod _autoSave;
		public AutoSaveMethod AutoSave
		{
			get { return _autoSave; }
			set { _autoSave = value; }
		}

		#endregion

		#region Constructor

		public PluginSettings()
		{
			settings = new Dictionary<StoreId, StoreItem>();
			saveLock = new object();
		}

		#endregion

		#region Private methods

		private void AutoSaveAsync()
		{
			SaveSettings();
		}


		private void PerformAutoSave()
		{
			if (_autoSave == AutoSaveMethod.None)
			{
				return;
			}

			if (_autoSave == AutoSaveMethod.Block)
			{
				SaveSettings();
			}
			else
			{
				// run on a separate thread
				Thread t = new Thread(AutoSaveAsync);

				t.Priority = ThreadPriority.BelowNormal;
				t.Start();
			}
		}


		public static StoreId CreateStoreId(Plugin plugin)
		{
			StoreId id = new StoreId();

			id.id = plugin.Id;
			id.name = plugin.Name;
			id.majorVersion = plugin.HasMajorVersion ? plugin.MajorVersion : int.MaxValue;
			id.minorVersion = plugin.HasMinorVersion ? plugin.MinorVersion : int.MaxValue;

			return id;
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Load the settings from the settings file
		/// </summary>
		public bool LoadSettings()
		{
			Debug.AssertNotNull(_settingsFile, "SettingsFile not set");

			if (File.Exists(_settingsFile) == false)
			{
				return false;
			}

			lock (saveLock)
			{
				BinaryFormatter serializer = new BinaryFormatter();
				FileStream reader = null;

				try
				{
					reader = new FileStream(_settingsFile, FileMode.Open);

					if (reader.CanRead)
					{
						settings = (Dictionary<StoreId, StoreItem>)serializer.Deserialize(reader);
						return true;
					}
				}
				catch (Exception e)
				{
					Debug.ReportError("Failed to deserialize settings. Exception: {0}", e.Message);
				}
				finally
				{
					if (reader != null && reader.CanRead)
					{
						reader.Close();
					}
				}
			}

			return false;
		}


		/// <summary>
		/// Save the setting to the settings file
		/// </summary>
		public bool SaveSettings()
		{
			Debug.AssertNotNull(_settingsFile, "SettingsFile not set");

			lock (saveLock)
			{
				if (settings.Count == 0)
				{
					return false;
				}

				BinaryFormatter serializer = new BinaryFormatter();
				FileStream writer = null;

				try
				{
					writer = new FileStream(_settingsFile, FileMode.OpenOrCreate);

					if (writer.CanWrite)
					{
						serializer.Serialize(writer, settings);
						return true;
					}
				}
				catch (Exception e)
				{
					Debug.ReportError("Failed to serialize settings. Exception: {0}", e.Message);
				}
				finally
				{
					if (writer != null && writer.CanWrite)
					{
						writer.Close();
					}
				}
			}

			return false;
		}


		/// <summary>
		/// Save the settings
		/// </summary>
		/// <param name="plugin">The plugin for which to save the settings.</param>
		public void SavePluginSettings(Plugin plugin)
		{
			// check the parameters
			Debug.AssertNotNull(plugin, "Plugin is null");
			Debug.Assert(plugin.Instantiated, "Cannot save data for uninstantiated plugin");

			StoreId id = CreateStoreId(plugin);
			StoreItem item;
			bool alreadyAdded = false;

			// check if the plugin is already added to the category
			if (settings.ContainsKey(id))
			{
				item = settings[id];
				alreadyAdded = true;
			}
			else
			{
				item = new StoreItem();
				item.id = id;
			}

			// get the settings
			try
			{
				item.data = plugin.PluginObject.SaveSettings();
			}
			catch (Exception e)
			{
				Debug.ReportWarning("Failed to save settings for plugin {0}. Exception: {1}", id, e.Message);
				return;
			}

			if (item.data != null && item.data.Length > 0)
			{
				if (alreadyAdded == false)
				{
					settings.Add(id, item);
				}
				else
				{
					settings[id] = item;
				}
			}

			// save the settings to the file
			if (_settingsFile != null)
			{
				SaveSettings();
			}
		}


		/// <summary>
		/// Load the settings
		/// </summary>
		/// <param name="plugin">The plugin for which to load the settings.</param>
		public bool LoadPluginSettings(Plugin plugin)
		{
			// check the parameters
			Debug.AssertNotNull(plugin, "Plugin is null");
			Debug.Assert(plugin.Instantiated, "Cannot load data for uninstantiated plugin");

			// check if the plugin is in the category
			StoreId id = CreateStoreId(plugin);

			if (settings.ContainsKey(id))
			{
				StoreItem item = settings[id];

				// load the settings
				if (item.data != null && item.data.Length > 0)
				{
					bool result = false;

					try
					{
						result = plugin.PluginObject.LoadSettings(item.data);
						return true;
					}
					catch (Exception e)
					{
						Debug.ReportWarning("Failed to load settings for plugin {0}. Exception: {1}", id, e.Message);
						result = false;
					}

					return result;
				}
			}

			return true;
		}


		/// <summary>
		/// Remove the settings
		/// </summary>
		/// <param name="plugin">The plugin for which to remove the settings.</param>
		public bool RemovePluginSettings(Plugin plugin)
		{
			// check the parameters
			Debug.AssertNotNull(plugin, "Plugin is null");
			Debug.Assert(plugin.Instantiated, "Cannot remove data for uninstantiated plugin");

			// check if the plugin is in the category
			StoreId id = CreateStoreId(plugin);

			if (settings.ContainsKey(id))
			{
				settings.Remove(id);

				// save the settings to the file
				if (_autoSave != AutoSaveMethod.None && _settingsFile != null)
				{
					SaveSettings();
				}
			}

			return false;
		}


		/// <summary>
		/// Remove all stored settings
		/// </summary>
		public void RemoveAllSettings()
		{
			settings.Clear();

			// save the settings to the file
			if (_autoSave != AutoSaveMethod.None && _settingsFile != null)
			{
				SaveSettings();
			}
		}

		#endregion
	}
}