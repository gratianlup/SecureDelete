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
    /// Wrapper for a IPlugin interface derived object.
    /// Provides support for instantiating the plugin.
    /// </summary>
    public class Plugin {
        #region Fields

        private Type pluginType;
        private IPlugin _plugin;
        private PluginAttribute _attribute;

        #endregion

        #region Constructor

        public Plugin(Type plugin) {
            pluginType = plugin;
            _instantiated = false;
        }

        #endregion

        #region Properties

        public IPlugin PluginObject {
            get {
                if(_instantiated) {
                    return _plugin;
                }
                else {
                    Debug.ReportError("Trying to access invalid data. {0}", "Plugin not instantiated.");
                    throw new Exception("Plugin not instantiated.");
                }
            }
        }


        public PluginAttribute AttributeObject {
            get {
                if(_instantiated) {
                    return _attribute;
                }
                else {
                    Debug.ReportError("Trying to access invalid data. {0}", "Plugin not instantiated.");
                    throw new Exception("Plugin not instantiated.");
                }
            }
        }


        private bool _instantiated;
        public bool Instantiated {
            get { return _instantiated; }
        }


        public Guid Id {
            get { return _attribute.Id; }
        }


        public string Name {
            get {
                if(_attribute != null) {
                    return _attribute.Name;
                }

                return null;
            }
        }


        public bool HasDescription {
            get { return (_attribute != null && _attribute.Description != null && _attribute.Description != string.Empty); }
        }


        public string Description {
            get {
                if(HasDescription) {
                    return _attribute.Description;
                }
                else {
                    Debug.ReportError("Trying to access invalid data. {0}", "Description not specified.");
                    throw new Exception("Description not specified.");
                }
            }
        }


        public bool HasAuthor {
            get { return (_attribute != null && _attribute.Author != null && _attribute.Author != string.Empty); }
        }


        public string Author {
            get {
                if(HasAuthor) {
                    return _attribute.Author;
                }
                else {
                    Debug.ReportError("Trying to access invalid data. {0}", "Author not specified.");
                    throw new Exception("Author not specified.");
                }
            }
        }


        public bool HasMajorVersion {
            get { return (_attribute != null && _attribute.MajorVersion != int.MaxValue); }
        }


        public int MajorVersion {
            get {
                if(HasMajorVersion) {
                    return _attribute.MajorVersion;
                }
                else {
                    Debug.ReportError("Trying to access invalid data. {0}", "MajorVersion not specified.");
                    throw new Exception("MajorVersion not specified.");
                }
            }
        }


        public bool HasMinorVersion {
            get { return (_attribute != null && _attribute.MinorVersion != int.MaxValue); }
        }


        public int MinorVersion {
            get {
                if(HasMinorVersion) {
                    return _attribute.MinorVersion;
                }
                else {
                    Debug.ReportError("Trying to access invalid data. {0}", "MinorVersion not specified.");
                    throw new Exception("MinorVersion not specified.");
                }
            }
        }


        public string VersionString {
            get {
                if(HasMajorVersion == false) {
                    return string.Empty;
                }
                else {
                    string version = MajorVersion.ToString();

                    if(HasMinorVersion) {
                        version += "." + MinorVersion.ToString();
                    }
                    else {
                        version += ".0";
                    }

                    return version;
                }
            }
        }

        #endregion

        /// <summary>
        /// Create an instance of the plugin
        /// </summary>
        public bool CreateInstance() {
            if(_instantiated) {
                Debug.ReportWarning("Trying to instantiate a plugin more than once");
                return true;
            }

            if(pluginType != null) {
                _plugin = (IPlugin)Activator.CreateInstance(pluginType);
                object[] attributes = pluginType.GetCustomAttributes(typeof(PluginAttribute), false);

                foreach(Attribute a in attributes) {
                    if(typeof(PluginAttribute).IsAssignableFrom(a.GetType())) {
                        _attribute = (PluginAttribute)a;
                        _instantiated = true;
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Destroy the current instance of the plugin
        /// </summary>
        public void DestroyInstance() {
            if(_instantiated) {
                _plugin.Unload();
                _plugin = null;
                _attribute = null;
                _instantiated = false;
            }
        }


        public override string ToString() {
            StringBuilder builder = new StringBuilder();

            if(Name != null) {
                builder.Append(Name);
            }
            else {
                builder.Append("Unknown");
            }

            builder.Append(" | Version: ");
            builder.Append(VersionString);
            return builder.ToString();
        }
    }
}
