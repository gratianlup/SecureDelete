// Copyright (c) 2007 Gratian Lup. All rights reserved.
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
// * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials provided
// with the distribution.
//
// * The name "SecureDelete" must not be used to endorse or promote
// products derived from this software without prior written permission.
//
// * Products derived from this software may not be called "SecureDelete" nor
// may "SecureDelete" appear in their names without prior written
// permission of the author.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using DebugUtils.Debugger;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace SecureDelete.WipePlugin {
    /// <summary>
    /// Provides support for managing the loaded plugins.
    /// </summary>
    public class PluginManager {
        #region Fields

        private List<Plugin> plugins;

        #endregion

        #region Constructor

        public PluginManager() {
            plugins = new List<Plugin>();
        }

        #endregion

        #region Deconstructor

        ~PluginManager() {
            ClearList();
        }

        #endregion

        #region Properties

        public int PluginCount {
            get { return plugins.Count; }
        }

        private string _pluginDirectory;
        public string PluginDirectory {
            get { return _pluginDirectory; }
            set { _pluginDirectory = value; }
        }

        public List<Plugin> Plugins {
            get { return plugins; }
        }

        #endregion

        /// <summary>
        /// Add all containing plugins from the given assembly
        /// </summary>
        /// <param name="path">The path of the assembly.</param>
        public bool AddPlugins(string path) {
            Debug.AssertNotNull(path, "Path is null");

            // check if the file exists. If not, try to load it from the plugin directory
            if(File.Exists(path) == false) {
                path = Path.Combine(_pluginDirectory, path);
            }

            Plugin[] p = PluginReader.LoadPluginsFromAssembly(path);

            // add the plugins to the category
            if(p != null && p.Length > 0) {
                for(int i = 0; i < p.Length; i++) {
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
        public bool AddPlugins(Assembly assembly) {
            Debug.AssertNotNull(assembly, "Assembly is null");
            Plugin[] p = PluginReader.LoadPluginsFromAssembly(assembly);

            // add the plugins to the category
            if(p != null && p.Length > 0) {
                for(int i = 0; i < p.Length; i++) {
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
        public Plugin[] GetPlugin(string name) {
            Debug.AssertNotNull(name, "Name is null");
            List<Plugin> p = new List<Plugin>();

            for(int i = 0; i < plugins.Count; i++) {
                if(plugins[i].Name != null && plugins[i].Name == name) {
                    p.Add(plugins[i]);
                }
            }

            return p.ToArray();
        }


        /// <summary>
        /// Destroy all instances of the plugins
        /// </summary>
        /// <remarks>The plugins are not removed from the manager. For this functionality, use ClearList instead.</remarks>
        public void DestroyAllPlugins() {
            for(int i = 0; i < plugins.Count; i++) {
                plugins[i].PluginObject.Unload();
                plugins[i].DestroyInstance();
            }

            plugins.Clear();
        }


        /// <summary>
        /// Clear the category of plugins
        /// </summary>
        public void ClearList() {
            DestroyAllPlugins();
            plugins.Clear();
        }
    }
}
