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
using System.Diagnostics;

namespace SecureDelete.Actions {
    #region Logger

    public class MethodParameterInfo {
        #region Properties

        private ParameterInfo _info;
        public System.Reflection.ParameterInfo Info {
            get { return _info; }
            set { _info = value; }
        }

        private object _value;
        public object Value {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region Constructor

        public MethodParameterInfo() {

        }

        public MethodParameterInfo(ParameterInfo info, object value) {
            _info = info;
            _value = value;
        }

        #endregion
    }


    public class BridgeLoggerItem {
        #region Properties

        private MethodBase _method;
        public MethodBase Method {
            get { return _method; }
            set { _method = value; }
        }

        private DateTime _time;
        public System.DateTime Time {
            get { return _time; }
            set { _time = value; }
        }

        private List<MethodParameterInfo> _parameters;
        public List<MethodParameterInfo> Parameters {
            get { return _parameters; }
            set { _parameters = value; }
        }

        #endregion
    }


    /// <summary>
    /// Helper object which logs member calls when running the script in test mode.
    /// </summary>
    public class BridgeLogger {
        #region Fields

        // make the class thread-safe
        private object lockObject;

        #endregion

        #region Constructor

        public BridgeLogger() {
            lockObject = new object();
            _items = new List<BridgeLoggerItem>();
        }

        #endregion

        #region Properties

        private List<BridgeLoggerItem> _items;
        public List<BridgeLoggerItem> Items {
            get { return _items; }
            set { _items = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Log a member call.
        /// </summary>
        /// <param name="parameters">The parameters the member received.</param>
        /// <remarks>The parameters need to be passed to this method in exactly the same order they appear in the member definition.</remarks>
        public void LogMethodCall(params object[] parameters) {
            BridgeLoggerItem item = new BridgeLoggerItem();
            item.Time = DateTime.Now;

            // get method info from stack
            StackTrace stackInfo = new StackTrace();
            StackFrame topFrame = stackInfo.GetFrame(1);

            MethodBase method = topFrame.GetMethod();
            item.Method = method;

            // add the parameters
            ParameterInfo[] paramInfo = method.GetParameters();
            item.Parameters = new List<MethodParameterInfo>();

            if(method.Name.StartsWith("set_")) {
                if(parameters.Length > 0) {
                    MethodParameterInfo param = new MethodParameterInfo();
                    param.Value = parameters[0];
                }
            }
            else {
                for(int i = 0; i < paramInfo.Length; i++) {
                    MethodParameterInfo param = new MethodParameterInfo();
                    param.Info = paramInfo[i];

                    if(parameters.Length > i) {
                        param.Value = parameters[i];
                    }

                    item.Parameters.Add(param);
                }
            }

            // add to the list
            _items.Add(item);
        }


        /// <summary>
        /// Clear the list of logged member calls.
        /// </summary>
        public void Clear() {
            if(_items != null) {
                _items.Clear();
            }
        }

        #endregion
    }

    #endregion

    #region Property reader

    public class PropertyReader {
        public object GetPropertyValue(string name, object obj, int index) {
            if(name == null || obj == null) {
                throw new ArgumentNullException();
            }

            try {
                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(name);
                return info.GetValue(name, index != -1 ? new object[] { index } : null);
            }
            catch {
                return null;
            }
        }
    }

    #endregion
}
