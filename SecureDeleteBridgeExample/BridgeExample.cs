// Copyright (c) Gratian Lup. All rights reserved.
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
using SecureDelete;
using SecureDelete.Actions;

namespace BridgeExample {
    [Bridge(Name = "PluginBridge", Exposed = true)]
    public class ExampleBridge : IBridge {
        #region Constants

        private static string BridgeName = "ExampleBridge";

        #endregion

        #region  Properties

        public string Name {
            get { return BridgeName; }
        }

        private WipeSession _session;
        public SecureDelete.WipeSession Session {
            get { return _session; }
            set { _session = value; }
        }

        private bool _afterWipe;
        public bool AfterWipe {
            get { return _afterWipe; }
            set { _afterWipe = value; }
        }

        #endregion

        #region Constructor

        public ExampleBridge() {

        }

        public ExampleBridge(WipeSession session, bool afterWipe) {
            _session = session;
            _afterWipe = afterWipe;
        }

        #endregion

        #region Public methods

        [BridgeMember(Name = "ExampleBridgeMethod", Signature = "public void ExampleBridgeMethod(string message)", 
                      Description = "Example of a methods that accepts a string parameter.")]
        [BridgeMemberParameter(Name = "message", Description = "The message to be returned by the method.")]
        public string PluginExposedMethod(string message) {
            return "ExampleBridgeMethod received the following string: " + message;
        }

        #endregion

        public bool TestMode {
            get { return false; }
            set { }
        }

        public BridgeLogger Logger {
            get { return null; }
            set { }
        }

        public void Open() {
            // initialization code could be added here...
        }

        public void Close() {
            // termination code could be added here...
        }
    }
}
