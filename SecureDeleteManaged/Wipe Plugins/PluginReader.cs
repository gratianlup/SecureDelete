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
	/// Provides support for obtaining the plugins contained in an assembly.
	/// </summary>
	public class PluginReader
	{
		/// <summary>
		/// Load and instantiate all plugins found in the given assembly
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		public static Plugin[] LoadPluginsFromAssembly(Assembly assembly)
		{
			Debug.AssertNotNull(assembly, "Assembly is null");

			// fields
			List<Plugin> plugins = new List<Plugin>();

			try
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (type.IsAbstract == true || typeof(IPlugin).IsAssignableFrom(type) == false)
					{
						continue;
					}

					// check if it has a plugin _attribute
					if (type.GetCustomAttributes(typeof(PluginAttribute), false) != null)
					{
						Plugin plugin = new Plugin(type);

						if (plugin.CreateInstance())
						{
							// add the _plugin
							plugins.Add(plugin);
						}
						else
						{
							Debug.ReportWarning("Couldn't create plugin instance");
						}
					}
				}
			}
			catch (Exception e)
			{
				Debug.ReportError("Failed to load plugin types from assembly {0}. Exception: {1}", assembly, e.Message);
			}

			return plugins.ToArray();
		}


		/// <summary>
		/// Load an assembly
		/// </summary>
		/// <param name="assembly">The name of the assembly.</param>
		public static Assembly LoadAssembly(string assemblyPath)
		{
			Assembly assembly;
			assemblyPath = Environment.ExpandEnvironmentVariables(assemblyPath);

			if (Path.IsPathRooted(assemblyPath) == false)
			{
				assemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), assemblyPath);
			}

			// load the assembly
			try
			{
				assembly = Assembly.Load(assemblyPath);
			}
			catch (IOException)
			{
				assembly = Assembly.LoadFile(assemblyPath);
			}

			return assembly;
		}


		/// <summary>
		/// Load and instantiate all plugins found in the given assembly
		/// </summary>
		/// <param name="assembly">The name of the assembly.</param>
		public static Plugin[] LoadPluginsFromAssembly(string assemblyPath)
		{
			Debug.AssertNotNull(assemblyPath, "AssemblyPath is null");

			// fields
			Assembly assembly = LoadAssembly(assemblyPath);

			if (assembly != null)
			{
				return LoadPluginsFromAssembly(assembly);
			}
			else
			{
				Debug.ReportWarning("Assembly not found: {0}", assembly);
			}

			return null;
		}
	}
}