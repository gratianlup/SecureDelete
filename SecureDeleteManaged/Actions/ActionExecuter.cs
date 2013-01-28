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
using System.Diagnostics;
using System.Threading;

namespace SecureDelete.Actions {
    public class ActionExecutor {
        #region Fields

        private ManualResetEvent waitEvent;
        private IAction activeAction;
        private object lockObject;
        private bool stopped;
        private bool result;

        #endregion

        #region Constructor

        public ActionExecutor() {
            _actions = new List<IAction>();
            waitEvent = new ManualResetEvent(true);
            lockObject = new object();
        }

        #endregion

        #region Properties

        private List<IAction> _actions;
        public List<IAction> Actions {
            get { return _actions; }
            set { _actions = value; }
        }

        private WipeSession _session;
        public WipeSession Session {
            get { return _session; }
            set { _session = value; }
        }

        private bool _afterWipe;
        public bool AfterWipe {
            get { return _afterWipe; }
            set { _afterWipe = value; }
        }

        public bool Stopped {
            get { return GetStopped(); }
        }

        public bool Result {
            get { return result; }
        }

        #endregion

        #region Events

        public event EventHandler OnStopped;

        #endregion

        #region Private methods

        private void SetStopped(bool value) {
            lock(lockObject) {
                stopped = value;
            }
        }

        private bool GetStopped() {
            lock(lockObject) {
                return stopped;
            }
        }

        private void NotifyStopped() {
            if(OnStopped != null) {
                OnStopped(this, null);
            }
        }

        private void StartImpl(object sender, EventArgs e) {
            if(_actions == null || _actions.Count == 0) {
                return;
            }

            // reset state
            stopped = false;
            waitEvent.Reset();
            result = true;

            for(int i = 0; i < _actions.Count; i++) {
                lock(lockObject) {
                    activeAction = _actions[i];
                }

                if(activeAction.Enabled) {
                    // action is enabled, execute it
                    activeAction.Session = _session;
                    activeAction.AfterWipe = _afterWipe;
                    activeAction.BlockingMode = true;

                    // start
                    if(activeAction.Start() == false) {
                        activeAction = null;
                        waitEvent.Set();
                        result = false;
                        NotifyStopped();
                    }
                }

                lock(lockObject) {
                    activeAction = null;
                }

                // should stop ?
                if(GetStopped()) {
                    waitEvent.Set();
                    result = false;
                    NotifyStopped();
                }
            }

            SetStopped(true);
            waitEvent.Set();
            NotifyStopped();
        }

        #endregion

        #region Public methods

        public void Start() {
            StartImpl(null, null);
        }

        public void StartAsync() {
            EventHandler e = new EventHandler(StartImpl);
            e.BeginInvoke(null, null, null, null);
        }

        public void Stop() {
            lock(lockObject) {
                if(activeAction != null) {
                    activeAction.Stop();
                }
            }

            waitEvent.Set();
        }

        public void EndStart() {
            // wait until all actions are executed or stopped
            waitEvent.WaitOne();
        }

        #endregion
    }
}
