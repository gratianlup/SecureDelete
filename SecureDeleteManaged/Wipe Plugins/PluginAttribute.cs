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
    /// Attribute that needs to be used with all plugins.
    /// Specifies information about the plugin (name, version, author, etc.)
    /// </summary>
    public class PluginAttribute : Attribute {
        #region Properties

        private Guid _id;
        public Guid Id {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        private string _description;
        public string Description {
            get { return _description; }
            set { _description = value; }
        }

        private int _majorVersion;
        public int MajorVersion {
            get { return _majorVersion; }
            set { _majorVersion = value; }
        }

        private int _minorVersion;
        public int MinorVersion {
            get { return _minorVersion; }
            set { _minorVersion = value; }
        }

        private string _author;
        public string Author {
            get { return _author; }
            set { _author = value; }
        }

        #endregion

        #region Public methods

        public PluginAttribute(string id, string name, string description, int majorVersion, int minorVersion) {
            _id = new Guid(id);
            _name = name;
            _description = description;
            _majorVersion = majorVersion;
            _minorVersion = minorVersion;
        }

        public PluginAttribute(string id, string name) {
            _id = new Guid(id);
            _name = name;
            _description = null;
            _majorVersion = int.MaxValue;
            _minorVersion = int.MaxValue;
        }

        #endregion
    }
}
