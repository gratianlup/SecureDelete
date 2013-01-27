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
using SecureDelete.WipePlugin;
using DebugUtils.Debugger;

namespace SecureDelete.WipeObjects
{
	public delegate void PluginObjectException(Plugin plugin, Exception e);

	[Serializable]
	public class PluginWipeObject : IWipeObject
	{
		#region IWipeObject Members

		public WipeObjectType Type
		{
			get { return WipeObjectType.Plugin; }
		}

		public bool SingleObject
		{
			get { return false; }
		}

		private WipeOptions _options;
		public WipeOptions Options
		{
			get { return _options; }
			set { _options = value; }
		}

		public NativeMethods.WObject GetObject()
		{
			throw new Exception("GetObject should not be called");
		}


		public NativeMethods.WObject[] GetObjects()
		{
			List<NativeMethods.WObject> items = new List<NativeMethods.WObject>();

			if (_assemblies == null)
			{
				throw new Exception("Assemblies not set");
			}

			if (_assemblies.Count == 0 || _pluginIds == null || (_pluginIds != null && _pluginIds.Count == 0))
			{
				return null;
			}

			// initialize
			int count = _pluginIds.Count;
			manager = new PluginManager();
			loadedAssemblies = 0;
			stopped = false;

			try
			{
				Plugin p = null;

				for (int i = 0; i < _pluginIds.Count; i++)
				{
					if (stopped)
					{
						return null;
					}

					try
					{
						p = GetPlugin(_pluginIds[i]);

						if (p != null)
						{
							if (p.Instantiated == false)
							{
								Debug.ReportWarning("Plugin not instantiated. Plugin: {0}", p.Name);
								continue;
							}

							// load the plugin
							if (p.PluginObject.Load())
							{
								// set the settings
								if (_pluginSettings != null)
								{
									if (_pluginSettings.LoadPluginSettings(p) == false)
									{
										Debug.ReportWarning("Failed to load settings. Plugin: {0}", p.Name);
										continue;
									}
								}

								// set the active plugin
								activePlugin = p.PluginObject;

								// get the objects
								IWipeObject[] obj = activePlugin.GetWipeObjects();

								// reset plugin
								activePlugin = null;

								if (obj == null || obj.Length == 0)
								{
									continue;
								}

								// add the objects
								for (int j = 0; j < obj.Length; j++)
								{
									if (stopped)
									{
										return null;
									}

									if (obj[i] is PluginWipeObject)
									{
										// skip plugin objects
										continue;
									}

									obj[i].Options = _options;
									obj[i].WipeMethodId = _wipeMethodId;

									if (obj[i].SingleObject)
									{
										items.Add(obj[i].GetObject());
									}
									else
									{
										items.AddRange(obj[i].GetObjects());
									}
								}
							}
							else
							{
								Debug.ReportWarning("Failed to load plugin. Plugin: {0}", p.Name);
							}
						}
					}
					catch (Exception ex)
					{
						// report the exception to the session
						if (OnPluginException != null)
						{
							OnPluginException(p, ex);
						}
					}					
				}
			}
			catch (Exception e)
			{
				Debug.ReportError("Exception while running plugins. Exception {0}", e.Message);
			}
			finally
			{
				// unload the plugins
				manager.DestroyAllPlugins();
			}

			return items.ToArray();
		}


		public void Stop()
		{
			stopped = true;

			// stop active plugin
			if (activePlugin != null)
			{
				activePlugin.Stop();
			}
		}

		#endregion

		#region Events

		public event PluginObjectException OnPluginException;

		#endregion

		#region Fields

		private int loadedAssemblies;
		private bool stopped;
		
		[NonSerialized]
		private PluginManager manager;

		[NonSerialized]
		private IPlugin activePlugin;

		#endregion

		#region Properties

		private int _wipeMethodId;
		public int WipeMethodId
		{
			get { return _wipeMethodId; }
			set { _wipeMethodId = value; }
		}

		private List<string> _assemblies;
		public List<string> Assemblies
		{
			get { return _assemblies; }
			set { _assemblies = value; }
		}
		
		private PluginSettings _pluginSettings;
		public PluginSettings PluginSettings
		{
			get { return _pluginSettings; }
			set { _pluginSettings = value; }
		}

		private List<StoreId> _pluginIds;
		public List<StoreId> PluginIds
		{
			get { return _pluginIds; }
			set { _pluginIds = value; }
		}

		#endregion

		#region Private methods

		private Plugin GetPlugin(StoreId pluginId)
		{
			Plugin p = null;
			manager.PluginDirectory = _options.PluginDirectory;

			while (p == null)
			{
				Plugin[] list = manager.GetPlugin(pluginId.name);

				if (list == null || list.Length == 0)
				{
					if (loadedAssemblies == _assemblies.Count)
					{
						// not found
						return null;
					}
					else
					{
						// load the assembly
						manager.AddPlugins(_assemblies[loadedAssemblies]);
						loadedAssemblies++;
					}
				}
				else
				{
					// verify if the versions match
					for (int i = 0; i < list.Length; i++)
					{
						if (list[i].Id != pluginId.id)
						{
							continue;
						}

						bool majorMatch = (list[i].HasMajorVersion ? list[i].MajorVersion : int.MaxValue) == pluginId.majorVersion;
						bool minorMatch = (list[i].HasMinorVersion ? list[i].MinorVersion : int.MaxValue) == pluginId.minorVersion;

						if (majorMatch && minorMatch)
						{
							// found
							return list[i];
						}
					}
				}
			}

			return p;
		}

		#endregion
	}
}
