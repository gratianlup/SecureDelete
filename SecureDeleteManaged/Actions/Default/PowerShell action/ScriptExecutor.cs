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

// ***************************************************************      
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
    #region Event definitions

    public delegate void ScriptStateChangedDelegate(object sender, PipelineStateInfo state);
    public delegate void ScriptNewOutputDelegate(object sender, string output);

    #endregion

    public class ScriptExecutor {
        #region Fields

        private Runspace runspace;
        private Pipeline pipeline;

        #endregion

        #region Properties

        private List<IBridge> _bridgeList;
        public List<IBridge> BridgeList {
            get { return _bridgeList; }
            set { _bridgeList = value; }
        }

        #endregion

        #region Events

        public event ScriptStateChangedDelegate OnStateChanged;
        public event ScriptNewOutputDelegate OnNewOutput;

        #endregion

        #region Constructor

        public ScriptExecutor() {
            _bridgeList = new List<IBridge>();
        }

        #endregion

        #region Private methods

        private void ExportBridgeObjects() {
            if(_bridgeList == null || _bridgeList.Count == 0) {
                return;
            }

            foreach(IBridge bridge in _bridgeList) {
                runspace.SessionStateProxy.SetVariable(bridge.Name, bridge);
            }
        }

        private void PipelineStateChanged(object sender, PipelineStateEventArgs e) {
            // report to parent
            if(OnStateChanged != null) {
                OnStateChanged(this, e.PipelineStateInfo);
            }
        }

        private void PipelineNewOutput(object sender, EventArgs e) {
            Collection<PSObject> data = pipeline.Output.NonBlockingRead();

            if(data.Count > 0 && OnNewOutput != null) {
                // build the string
                StringBuilder builder = new StringBuilder();

                foreach(PSObject obj in data) {
                    builder.Append(obj);
                }

                // report to parent
                if(pipeline.PipelineStateInfo.State != PipelineState.Stopped &&
                   pipeline.PipelineStateInfo.State != PipelineState.Stopping) {
                    OnNewOutput(this, builder.ToString());
                }
            }
        }

        #endregion

        #region Public methods

        public bool CreateRunspace() {
            if(runspace != null && runspace.RunspaceStateInfo.State != RunspaceState.Closed) {
                return true;
            }

            try {
                runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();
                return true;
            }
            catch(Exception e) {
                Debug.ReportError("Failed to open runspace. Exception {0}", e.Message);
                return false;
            }
        }


        public void DestroyRunspace() {
            if(runspace == null) {
                return;
            }

            try {
                runspace.Close();
            }
            catch(Exception e) {
                Debug.ReportError("Failed to open runspace. Exception {0}", e.Message);
            }
        }


        public bool RunScriptAsync(string script) {
            if(runspace == null || runspace.RunspaceStateInfo.State != RunspaceState.Opened) {
                throw new Exception("Invalid runspace");
            }

            try {
                // create the pipeline
                pipeline = runspace.CreatePipeline();
                pipeline.Commands.AddScript(script);
                pipeline.Input.Close();

                // export the objects
                ExportBridgeObjects();

                // set events
                pipeline.StateChanged += PipelineStateChanged;
                pipeline.Output.DataReady += PipelineNewOutput;

                pipeline.InvokeAsync();
                return true;
            }
            catch(Exception e) {
                Debug.ReportError("Failed to start script. Exception {0}", e.Message);
                return false;
            }
        }


        public void Stop() {
            if(pipeline != null && pipeline.PipelineStateInfo.State != PipelineState.Stopped &&
               pipeline.PipelineStateInfo.State != PipelineState.Stopping) {
                pipeline.Stop();
            }
            else {
                throw new Exception("Pipeline not allocated or already stopped");
            }
        }

        #endregion
    }
}
