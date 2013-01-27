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
	/// Provides support for managing the loaded plugins.
	/// </summary>
	public class PluginManager
	{
		#region Fields

		private List<Plugin> plugins;

		#endregion

		#region Constructor

		public PluginManager()
		{
			plugins = new List<Plugin>();
		}

		#endregion

		#region Deconstructor

		~PluginManager()
		{
			ClearList();
		}

		#endregion

		#region Properties

		public int PluginCount
		{
			get { return plugins.Count; }
		}

		private string _pluginDirectory;
		public string PluginDirectory
		{
			get { return _pluginDirectory; }
			set { _pluginDirectory = value; }
		}

		public List<Plugin> Plugins
		{
			get { return plugins; }
		}

		#endregion

		/// <summary>
		/// Add all containing plugins from the given assembly
		/// </summary>
		/// <param name="path">The path of the assembly.</param>
		public bool AddPlugins(string path)
		{
			Debug.AssertNotNull(path, "Path is null");

			// check if the file exists. If not, try to load it from the plugin directory
			if (File.Exists(path) == false)
			{
				path = Path.Combine(_pluginDirectory, path);
			}

			Plugin[] p = PluginReader.LoadPluginsFromAssembly(path);

			// add the plugins to the category
			if (p != null && p.Length > 0)
			{
				for (int i = 0; i < p.Length; i++)
				{
					Debug.AssertNotNull(p[i].PluginObject, "PluginObject is null");
					plugins.Add(p[i]);
				}

				return true;
			}

			return false;
		}


		/// <summary>
		/// Add all containing plugins from the given assembly
		/// </summary>
		/// <param name="path">The assembly.</param>
		public bool AddPlugins(Assembly assembly)
		{
			Debug.AssertNotNull(assembly, "Assembly is null");

			Plugin[] p = PluginReader.LoadPluginsFromAssembly(assembly);

			// add the plugins to the category
			if (p != null && p.Length > 0)
			{
				for (int i = 0; i < p.Length; i++)
				{
					Debug.AssertNotNull(plugins[i].PluginObject, "PluginObject is null");
					plugins.Add(p[i]);
				}

				return true;
			}

			return false;
		}


		/// <summary>
		/// Get the plugins with the specified name
		/// </summary>
		/// <param name="name">The name of the plugins.</param>
		/// <returns></returns>
		public Plugin[] GetPlugin(string name)
		{
			Debug.AssertNotNull(name, "Name is null");

			List<Plugin> p = new List<Plugin>();

			for (int i = 0; i < plugins.Count; i++)
			{
				if (plugins[i].Name != null && plugins[i].Name == name)
				{
					p.Add(plugins[i]);
				}
			}

			return p.ToArray();
		}


		/// <summary>
		/// Destroy all instances of the plugins
		/// </summary>
		/// <remarks>The plugins are not removed from the manager. For this functionality, use ClearList instead.</remarks>
		public void DestroyAllPlugins()
		{
			for (int i = 0; i < plugins.Count; i++)
			{
				plugins[i].PluginObject.Unload();
				plugins[i].DestroyInstance();
			}

			plugins.Clear();
		}


		/// <summary>
		/// Clear the category of plugins
		/// </summary>
		public void ClearList()
		{
			DestroyAllPlugins();
			plugins.Clear();
		}
	}
}