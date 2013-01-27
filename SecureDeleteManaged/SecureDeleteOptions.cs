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
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Microsoft.Win32;
using SecureDelete.FileSearch;
using SecureDelete.Schedule;
using SecureDelete.Actions;

namespace SecureDelete
{
	#region Filter templates

	[Serializable]
	public class FilterStoreItem
	{
		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private FileFilter _filter;
		public FileFilter Filter
		{
			get { return _filter; }
			set { _filter = value; }
		}
	}


	[Serializable]
	public class FilterStore
	{
		public FilterStore()
		{
			_items = new List<FilterStoreItem>();
		}

		private List<FilterStoreItem> _items;
		public List<FilterStoreItem> Items
		{
			get { return _items; }
			set { _items = value; }
		}


		public bool Add(FilterStoreItem item)
		{
			if (Contains(item.Name) == true)
			{
				return false;
			}

			_items.Add(item);
			return true;
		}


		public bool Contains(string name)
		{
			foreach (FilterStoreItem item in _items)
			{
				if (item.Name == name)
				{
					return true;
				}
			}

			return false;
		}


		public void Remove(string name)
		{
			for (int i = 0; i < _items.Count; i++)
			{
				if (_items[i].Name == name)
				{
					_items.RemoveAt(i);
					return;
				}
			}
		}
	}

	#endregion

	#region Action templates

	[Serializable]
	public class ActionStoreItem
	{
		#region Constructor

		public ActionStoreItem()
		{
			_actions = new List<IAction>();
		}

		#endregion

		#region Properties

		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private List<IAction> _actions;
		public List<IAction> Actions
		{
			get { return _actions; }
			set { _actions = value; }
		}

		#endregion
	}


	[Serializable]
	public class ActionStore
	{
		public ActionStore()
		{
			_items = new List<ActionStoreItem>();
		}

		private List<ActionStoreItem> _items;
		public List<ActionStoreItem> Items
		{
			get { return _items; }
			set { _items = value; }
		}


		public bool Add(ActionStoreItem item)
		{
			if (Contains(item.Name) == true)
			{
				return false;
			}

			_items.Add(item);
			return true;
		}


		public bool Contains(string name)
		{
			foreach (ActionStoreItem item in _items)
			{
				if (item.Name == name)
				{
					return true;
				}
			}

			return false;
		}


		public void Remove(string name)
		{
			for (int i = 0; i < _items.Count; i++)
			{
				if (_items[i].Name == name)
				{
					_items.RemoveAt(i);
					return;
				}
			}
		}
	}

	#endregion


	[Serializable]
	public class SDOptions
	{
		#region Constants

		public const int PasswordLength = 384;
		public const string SecureDeleteKey = "HKEY_CURRENT_USER\\Software\\SecureDelete";
		public const string ShellNormalEnabled = "ShellNormalEnabled";
		public const string ShellMoveEnabled = "ShellMoveEnabled";
		public const string ShellRecycleEnabled = "ShellRecycleEnabled";

		#endregion

		#region Properties

		private WipeOptions _wipeOptions;
		public WipeOptions WipeOptions
		{
			get { return _wipeOptions; }
			set { _wipeOptions = value; }
		}

		private string _methodFolder;
		public string MethodFolder
		{
			get { return _methodFolder; }
			set { _methodFolder = value; }
		}

		[NonSerialized]
		private WipeMethodManager _methodManager;
		public WipeMethodManager MethodManager
		{
			get { return _methodManager; }
			set { _methodManager = value; }
		}

		private FilterStore _filterStore;
		public FilterStore FilterStore
		{
			get { return _filterStore; }
			set { _filterStore = value; }
		}

		private ActionStore _actionStore;
		public ActionStore ActionStore
		{
			get { return _actionStore; }
			set { _actionStore = value; }
		}

		private Dictionary<Guid, string> _sessionNames;
		public Dictionary<Guid, string> SessionNames
		{
			get { return _sessionNames; }
			set { _sessionNames = value; }
		}

		private bool _saveReport;
		public bool SaveReport
		{
			get { return _saveReport; }
			set { _saveReport = value; }
		}

		private bool _saveReportOnErrors;
		public bool SaveReportOnErrors
		{
			get { return _saveReportOnErrors; }
			set { _saveReportOnErrors = value; }
		}

		private int _maximumReportsPerSession;
		public int MaximumReportsPerSession
		{
			get { return _maximumReportsPerSession; }
			set { _maximumReportsPerSession = value; }
		}

		private bool _showInTray;
		public bool ShowInTray
		{
			get { return _showInTray; }
			set { _showInTray = value; }
		}

		private bool _showStatusWindow;
		public bool ShowStatusWindow
		{
			get { return _showStatusWindow; }
			set { _showStatusWindow = value; }
		}

		private bool _warnOnStart;
		public bool WarnOnStart
		{
			get { return _warnOnStart; }
			set { _warnOnStart = value; }
		}

		private bool _limitReportNumber;
		public bool LimitReportNumber
		{
			get { return _limitReportNumber; }
			set { _limitReportNumber = value; }
		}

		private int _reportsPerSession;
		public int ReportsPerSession
		{
			get { return _reportsPerSession; }
			set { _reportsPerSession = value; }
		}

		private bool _deleteOldReports;
		public bool DeleteOldReports
		{
			get { return _deleteOldReports; }
			set { _deleteOldReports = value; }
		}

		private int _oldestReportDate;
		public int OldestReportDate
		{
			get { return _oldestReportDate; }
			set { _oldestReportDate = value; }
		}

		private bool _passwordRequired;
		public bool PasswordRequired
		{
			get { return _passwordRequired; }
			set { _passwordRequired = value; }
		}

		private byte[] _password;
		public byte[] Password
		{
			get { return _password; }
			set { _password = value; }
		}

		private bool _customReportStyle;
		public bool CustomReportStyle
		{
			get { return _customReportStyle; }
			set { _customReportStyle = value; }
		}

		private string _customStyleLocation;
		public string CustomStyleLocation
		{
			get { return _customStyleLocation; }
			set { _customStyleLocation = value; }
		}

		private bool _queueTasks;
		public bool QueueTasks
		{
			get { return _queueTasks; }
			set { _queueTasks = value; }
		}

		private List<IAction> _beforeWipeActions;
		public List<IAction> BeforeWipeActions
		{
			get { return _beforeWipeActions; }
			set { _beforeWipeActions = value; }
		}

		private List<IAction> _afterWipeActions;
		public List<IAction> AfterWipeActions
		{
			get { return _afterWipeActions; }
			set { _afterWipeActions = value; }
		}

		private bool _executeBeforeWipeActions;
		public bool ExecuteBeforeWipeActions
		{
			get { return _executeBeforeWipeActions; }
			set { _executeBeforeWipeActions = value; }
		}

		private bool _executeAfterWipeActions;
		public bool ExecuteAfterWipeActions
		{
			get { return _executeAfterWipeActions; }
			set { _executeAfterWipeActions = value; }
		}

		private bool _shellFileOperation;
		public bool ShellFileOperation
		{
			get { return _shellFileOperation; }
			set { _shellFileOperation = value; }
		}

		private bool _shellMoveOperation;
		public bool ShellMoveOperation
		{
			get { return _shellMoveOperation; }
			set { _shellMoveOperation = value; }
		}

		private bool _shellRecycleOperation;
		public bool ShellRecycleOperation
		{
			get { return _shellRecycleOperation; }
			set { _shellRecycleOperation = value; }
		}

		// power management settings
		private PowertTCSettings _powerControllerSettings;
		public PowertTCSettings PowerControllerSettings
		{
			get { return _powerControllerSettings; }
			set { _powerControllerSettings = value; }
		}

		#endregion

		#region Public methods

		public static byte[] ComputePasswordHash(string password)
		{
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}

			SHA384 sha = new SHA384Managed();
			byte[] data = Encoding.Default.GetBytes(password);

			return sha.ComputeHash(data);
		}

		#endregion
	}

	public class SDOptionsFile
	{
		#region Properties

		private static bool _encrypt;
		public static bool Encrypt
		{
			get { return _encrypt; }
			set { _encrypt = value; }
		}

		private static string _password;
		public static string Password
		{
			get { return _password; }
			set { _password = value; }
		}

		#endregion

		#region Private methods

		public static byte[] SerializeOptions(SDOptions options)
		{
			MemoryStream stream = new MemoryStream();

			try
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, options);

				return stream.ToArray();
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while serializing SDOptions. Exception: {0}", e.Message);
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


		public static SDOptions DeserializeOptions(byte[] data)
		{
			if (data == null)
			{
				return null;
			}

			BinaryFormatter serializer = new BinaryFormatter();
			MemoryStream stream = new MemoryStream(data);

			try
			{
				return (SDOptions)serializer.Deserialize(stream);
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while deserializing options. Exception: {0}", e.Message);
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

		public static bool SaveOptions(SDOptions options, string path)
		{
			Debug.AssertNotNull(options, "Options is null");
			Debug.AssertNotNull(path, "Path is null");

			try
			{
				// create the store
                FileStore.FileStore store = new FileStore.FileStore();
				store.Encrypt = _encrypt;

				// set encryption key
				if (_password != null)
				{
					SHA256Managed sha = new SHA256Managed();
					store.EncryptionKey = sha.ComputeHash(Encoding.ASCII.GetBytes(_password));
					store.UseDPAPI = false;
				}
				else
				{
					store.UseDPAPI = true;
				}

				// add the file
                FileStore.StoreFile file = store.CreateFile("options.dat");
				byte[] data = SerializeOptions(options);

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
				Debug.ReportError("Error while saving options. Exception: {0}", e.Message);
				return false;
			}
		}


		public static bool LoadDefaultOptions(string path, out SDOptions options)
		{
			// check the parameters
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}

			options = null;

			try
			{
				// deserialize
				options = DeserializeOptions(File.ReadAllBytes(path));
				return options != null;
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while loading session. Exception: {0}", e.Message);
				return false;
			}
		}


		public static bool LoadOptions(string path, ref SDOptions options)
		{
			// check the parameters
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}

			options = null;

			try
			{
				// create the store
                FileStore.FileStore store = new FileStore.FileStore();

				// set encryption key
				if (_password != null)
				{
					SHA256Managed sha = new SHA256Managed();
					store.EncryptionKey = sha.ComputeHash(Encoding.ASCII.GetBytes(_password));
				}

				// load store
				if (store.Load(path) == false)
				{
					Debug.ReportError("Error while loading store from path {0}", path);
					return false;
				}

				// deserialize
				options = DeserializeOptions(store.ReadFile("options.dat"));
				return options != null;
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while loading session. Exception: {0}", e.Message);
				return false;
			}
		}


		public static void TryLoadOptions(out SDOptions options)
		{
			options = new SDOptions();

			// load
			LoadOptions(SecureDeleteLocations.GetOptionsFilePath(), ref options);

			// create new options if load failed
			if (options == null)
			{
				options = new SDOptions();
			}

			// create new objects if needed
			if (options.FilterStore == null)
			{
				options.FilterStore = new FilterStore();
			}

			if (options.ActionStore == null)
			{
				options.ActionStore = new ActionStore();
			}

			// create session name list
			if (options.SessionNames == null)
			{
				options.SessionNames = new Dictionary<Guid, string>();
			}

			// create before/after lists
			if (options.BeforeWipeActions == null)
			{
				options.BeforeWipeActions = new List<IAction>();
			}
			if (options.AfterWipeActions == null)
			{
				options.AfterWipeActions = new List<IAction>();
			}

			// initialize the WipeMethodManager
			options.MethodFolder = SecureDeleteLocations.GetMethodsFolder();
			options.MethodManager = new WipeMethodManager();
			options.MethodManager.Folder = options.MethodFolder;
			if (options.MethodFolder == null || options.MethodManager.ScanFolder() == false)
			{
				Debug.ReportWarning("Failed to load wipe methods. Directory: {0}", options.MethodFolder);
			}

			if (options.WipeOptions == null)
			{
				options.WipeOptions = new WipeOptions();
			}

			// set registry options
			try
			{
				Registry.SetValue(SDOptions.SecureDeleteKey, SDOptions.ShellNormalEnabled, options.ShellFileOperation, RegistryValueKind.DWord);
				Registry.SetValue(SDOptions.SecureDeleteKey, SDOptions.ShellMoveEnabled, options.ShellMoveOperation, RegistryValueKind.DWord);
				Registry.SetValue(SDOptions.SecureDeleteKey, SDOptions.ShellRecycleEnabled, options.ShellRecycleOperation, RegistryValueKind.DWord);
			}
			catch (Exception e)
			{
				Debug.ReportWarning("Error while saving registry options. Exception: {0}", e.Message);
			}
		}


		public static bool TrySaveOptions(SDOptions options)
		{
			// check if the folder exists
			string path = SecureDeleteLocations.CombinePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), SecureDeleteLocations.SecureDeleteFolder);

			try
			{
				if (Directory.Exists(path) == false)
				{
					Directory.CreateDirectory(path);
				}

				// set registry options
				Registry.SetValue(SDOptions.SecureDeleteKey, SDOptions.ShellNormalEnabled, options.ShellFileOperation, RegistryValueKind.DWord);
				Registry.SetValue(SDOptions.SecureDeleteKey, SDOptions.ShellMoveEnabled, options.ShellMoveOperation, RegistryValueKind.DWord);
				Registry.SetValue(SDOptions.SecureDeleteKey, SDOptions.ShellRecycleEnabled, options.ShellRecycleOperation, RegistryValueKind.DWord);
			}
			catch(Exception e)
			{
				Debug.ReportError("Failed to create folder {0}. Exception: {1}", path, e.Message);
				return false;
			}

			return SaveOptions(options, SecureDeleteLocations.CombinePath(path, SecureDeleteLocations.SecureDeleteOptionsFile));
		}

		#endregion
	}


	public class SecureDeleteLocations
	{
		public const string SecureDeleteFolder = "SecureDelete";
		public const string SecureDeleteMethodsFolder = "methods";
		public const string SecureDeleteReportFolder = "reports";
		public const string SecureDeletePluginFolder = "plugins";
		public const string SecureDeleteDefaultPluginSettingsFile = "defaultSettings.dat";
		public const string SecureDeleteScheduledTasksFolder = "schedule";
		public const string SecureDeleteScheduleTasksFile = "tasks.dat";
		public const string SecureDeleteScheduleHistoryFile = "history.dat";
		public const string SecureDeleteOptionsFile = "options.dat";
		public const string SecureDeleteReportFile = "reports.dat";

		// ---

		public const string SessionFileExtension = ".sds";
		public const string TaskFileExtension = ".sdt";
		public const string TaskFilePattern = "*.sdt";

		// ---

		public static string CombinePath(params string[] paths)
		{
			if (paths == null || paths.Length == 0)
			{
				return string.Empty;
			}

			if (paths.Length == 1)
			{
				return paths[0];
			}

			string path = paths[0];

			try
			{
				for (int i = 1; i < paths.Length; i++)
				{
					path = Path.Combine(path, paths[i]);
				}
			}
			catch
			{
				return string.Empty;
			}

			return path;
		}

		public static string GetBaseDirectory()
		{
			return SecureDeleteLocations.CombinePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
													 SecureDeleteLocations.SecureDeleteFolder);
		}

		public static string GetOptionsFilePath()
		{
			return SecureDeleteLocations.CombinePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
															SecureDeleteLocations.SecureDeleteFolder, SecureDeleteLocations.SecureDeleteOptionsFile);
		}

		public static string GetReportFilePath()
		{
			return SecureDeleteLocations.CombinePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
															SecureDeleteLocations.SecureDeleteFolder, SecureDeleteLocations.SecureDeleteReportFolder,
															SecureDeleteLocations.SecureDeleteReportFile);
		}

		public static string GetReportDirectory()
		{
			return SecureDeleteLocations.CombinePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
													 SecureDeleteLocations.SecureDeleteFolder, SecureDeleteLocations.SecureDeleteReportFolder);
		}

		// located in program folder
		public static string GetPluginDirectory()
		{
			return SecureDeleteLocations.CombinePath(Environment.CurrentDirectory, SecureDeleteLocations.SecureDeletePluginFolder);
		}

		// located in AppData
		public static string GetPluginDefaultSettingsFilePath()
		{
			return SecureDeleteLocations.CombinePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
													 SecureDeleteLocations.SecureDeleteFolder, SecureDeleteLocations.SecureDeletePluginFolder,
													 SecureDeleteLocations.SecureDeleteDefaultPluginSettingsFile);
		}

		public static string GetScheduledTasksDirectory()
		{
			return SecureDeleteLocations.CombinePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
													 SecureDeleteLocations.SecureDeleteFolder, SecureDeleteLocations.SecureDeleteScheduledTasksFolder);
		}

		public static string GetScheduledTasksFile()
		{
			return SecureDeleteLocations.CombinePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
													 SecureDeleteLocations.SecureDeleteFolder, SecureDeleteLocations.SecureDeleteScheduledTasksFolder,
													 SecureDeleteLocations.SecureDeleteScheduleTasksFile);
		}

		public static string GetScheduleHistoryFile()
		{
			return SecureDeleteLocations.CombinePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
													 SecureDeleteLocations.SecureDeleteFolder, SecureDeleteLocations.SecureDeleteScheduledTasksFolder,
													 SecureDeleteLocations.SecureDeleteScheduleHistoryFile);
		}

		public static string GetMethodsFolder()
		{
			return SecureDeleteLocations.CombinePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
													 SecureDeleteLocations.SecureDeleteFolder, SecureDeleteLocations.SecureDeleteMethodsFolder);
		}

		// ----------------------------------------------
		public static string[] GetPluginAssemblies()
		{
			string path = SecureDeleteLocations.GetPluginDirectory();

			if (Directory.Exists(path))
			{
				return Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
			}
			else
			{
				return new string[] { };
			}
		}


		public static string GetSessionFile(Guid id)
		{
			return SecureDeleteLocations.CombinePath(SecureDeleteLocations.GetScheduledTasksDirectory(),
													 id.ToString() + SecureDeleteLocations.SessionFileExtension);
		}


		public static string GetTaskFile(Guid id)
		{
			return SecureDeleteLocations.CombinePath(SecureDeleteLocations.GetScheduledTasksDirectory(),
													 id.ToString() + SecureDeleteLocations.TaskFileExtension);
		}


		public static void InitializeLocations()
		{
			try
			{
				string path = GetBaseDirectory();

				if (Directory.Exists(path) == false)
				{
					// create settings folder
					Directory.CreateDirectory(path);
				}

				// options file
				path = GetOptionsFilePath();

				if (File.Exists(path) == false)
				{
					// try to copy the default one
					string defaultPath = Environment.CurrentDirectory + "\\defaultOptions.dat";

					if (File.Exists(defaultPath))
					{
						// load the default (unencrypted) settings
						SDOptions options;
						SDOptionsFile.LoadDefaultOptions(defaultPath, out options);
						SDOptionsFile.TrySaveOptions(options);
					}
				}

				// methods folder
				path = GetMethodsFolder();

				if (Directory.Exists(path) == false)
				{
					Directory.CreateDirectory(path);

					// copy default methods
					string[] methods = Directory.GetFiles(Environment.CurrentDirectory + "\\defaultMethods");

					foreach (string s in methods)
					{
						File.Copy(s, Path.Combine(path, new FileInfo(s).Name));
					}
				}
				 
				// create schedule and reports folders
				path = GetScheduledTasksDirectory();

				if (Directory.Exists(path) == false)
				{
					Directory.CreateDirectory(path);
				}

				// report directory
				path = GetReportDirectory();

				if (Directory.Exists(path) == false)
				{
					Directory.CreateDirectory(path);
				}

				// plugin directory
				path = Directory.GetParent(GetPluginDefaultSettingsFilePath()).FullName;

				if (Directory.Exists(path) == false)
				{
					Directory.CreateDirectory(path);
				}
			}
			catch { }
		}
	}
}