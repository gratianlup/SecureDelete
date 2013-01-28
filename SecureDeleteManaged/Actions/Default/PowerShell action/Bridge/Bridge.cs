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

namespace SecureDelete.Actions {
    /// <summary>
    /// Interface that needs to be implemented by all bridge objects.
    /// </summary>
    public interface IBridge {
        bool TestMode { get; set; }
        BridgeLogger Logger { get; set; }
        string Name { get; }
        void Open();
        void Close();
    }


    public interface IFullAccessBridge {
        WipeSession Session { get; set; }
        bool AfterWipe { get; set; }
    }

    #region Attributes

    /// <summary>
    /// Attribute that needs to be applied to all bridge objects.
    /// </summary>
    public class Bridge : Attribute {
        private string _name;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        private bool _exposed;
        public bool Exposed {
            get { return _exposed; }
            set { _exposed = value; }
        }
    }


    /// <summary>
    /// Attribute that needs to be applied to all members of the object that should be exposed.
    /// </summary>
    public class BridgeMember : Attribute {
        private string _name;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        private string _signature;
        public string Signature {
            get { return _signature; }
            set { _signature = value; }
        }

        private string _description;
        public string Description {
            get { return _description; }
            set { _description = value; }
        }
    }

    /// <summary>
    /// Attribute that needs to be applied for each parameter of the exposed method.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class BridgeMemberParameter : Attribute {
        private int _order;
        public int Order {
            get { return _order; }
            set { _order = value; }
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
    }

    #endregion
}
