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
    /// Provides support for obtaining the plugins contained in an assembly.
    /// </summary>
    public class PluginReader {
        /// <summary>
        /// Load and instantiate all plugins found in the given assembly
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public static Plugin[] LoadPluginsFromAssembly(Assembly assembly) {
            Debug.AssertNotNull(assembly, "Assembly is null");
            List<Plugin> plugins = new List<Plugin>();

            try {
                foreach(Type type in assembly.GetTypes()) {
                    if(type.IsAbstract == true || typeof(IPlugin).IsAssignableFrom(type) == false) {
                        continue;
                    }

                    // check if it has a plugin _attribute
                    if(type.GetCustomAttributes(typeof(PluginAttribute), false) != null) {
                        Plugin plugin = new Plugin(type);

                        if(plugin.CreateInstance()) {
                            // add the _plugin
                            plugins.Add(plugin);
                        }
                        else {
                            Debug.ReportWarning("Couldn't create plugin instance");
                        }
                    }
                }
            }
            catch(Exception e) {
                Debug.ReportError("Failed to load plugin types from assembly {0}. Exception: {1}", assembly, e.Message);
            }

            return plugins.ToArray();
        }


        /// <summary>
        /// Load an assembly
        /// </summary>
        /// <param name="assembly">The name of the assembly.</param>
        public static Assembly LoadAssembly(string assemblyPath) {
            Assembly assembly;
            assemblyPath = Environment.ExpandEnvironmentVariables(assemblyPath);

            if(Path.IsPathRooted(assemblyPath) == false) {
                assemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), assemblyPath);
            }

            // load the assembly
            try {
                assembly = Assembly.Load(assemblyPath);
            }
            catch(IOException) {
                assembly = Assembly.LoadFile(assemblyPath);
            }

            return assembly;
        }


        /// <summary>
        /// Load and instantiate all plugins found in the given assembly
        /// </summary>
        /// <param name="assembly">The name of the assembly.</param>
        public static Plugin[] LoadPluginsFromAssembly(string assemblyPath) {
            Debug.AssertNotNull(assemblyPath, "AssemblyPath is null");
            Assembly assembly = LoadAssembly(assemblyPath);

            if(assembly != null) {
                return LoadPluginsFromAssembly(assembly);
            }
            else {
                Debug.ReportWarning("Assembly not found: {0}", assembly);
            }

            return null;
        }
    }
}
