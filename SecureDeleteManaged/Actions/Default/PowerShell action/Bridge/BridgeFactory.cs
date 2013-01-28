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
using DebugUtils.Debugger;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.IO;
using System.Reflection;

namespace SecureDelete.Actions {
    public class BridgeFactory {
        public static List<Type> GetBridgeObjectsTypes(Assembly assembly) {
            List<Type> bridgeObjects = new List<Type>();

            // search the bridge objects in the exposed types
            foreach(Type type in assembly.GetExportedTypes()) {
                object[] attributes = type.GetCustomAttributes(typeof(Bridge), false);

                if(attributes.Length > 0 && ((Bridge)attributes[0]).Exposed) {
                    // add to the list
                    bridgeObjects.Add(type);
                }
            }

            return bridgeObjects;
        }

        public static List<Type> GetBridgeObjectsTypes() {
            return GetBridgeObjectsTypes(Assembly.GetExecutingAssembly());
        }


        public static List<IBridge> GetInstantiatedBridgeObjects(List<Type> types) {
            List<IBridge> bridgeObjects = new List<IBridge>();

            // create the instances
            try {
                foreach(Type type in types) {
                    bridgeObjects.Add((IBridge)Activator.CreateInstance(type));
                }
            }
            catch(Exception e) {
                Debug.ReportError("Error while instantiating bridge object. Exception {0}", e.Message);
                return new List<IBridge>();
            }

            return bridgeObjects;
        }


        /// <summary>
        ///  Instantiate the bridge objects found in the given assembly.
        /// </summary>
        public static List<IBridge> GetBridgeObjects(Assembly assembly) {
            List<Type> types = GetBridgeObjectsTypes(assembly);

            if(types != null) {
                return GetInstantiatedBridgeObjects(types);
            }

            // nothing found
            return new List<IBridge>();
        }

        /// <summary>
        ///  Instantiate the bridge objects found in the executing assembly.
        ///  </summary>
        public static List<IBridge> GetBridgeObjects() {
            return GetBridgeObjects(Assembly.GetExecutingAssembly());
        }


        public static Assembly LoadAssembly(string path) {
            Assembly assembly = null;
            path = Environment.ExpandEnvironmentVariables(path);

            if(Path.IsPathRooted(path) == false) {
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), path);
            }

            // load the assembly
            try {
                assembly = Assembly.Load(path);
            }
            catch(IOException) {
                try {
                    assembly = Assembly.LoadFile(path);
                }
                catch {
                    return null;
                }
            }

            return assembly;
        }

        /// <summary>
        ///  Instantiate the bridge objects found in the assembly located at the given path.
        ///  </summary>
        public static List<IBridge> GetBridgeObjects(string path) {
            Assembly assembly = LoadAssembly(path);

            if(assembly != null) {
                return GetBridgeObjects(assembly);
            }
            else {
                return new List<IBridge>();
            }
        }
    }
}
