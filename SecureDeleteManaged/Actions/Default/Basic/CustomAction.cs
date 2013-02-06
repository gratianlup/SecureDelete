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
using SecureDelete.Schedule;
using System.Threading;

namespace SecureDelete.Actions {
    [Serializable]
    public class EnvironmentVariable {
        #region Fields

        public string Name;
        public string Value;

        #endregion

        #region Constructor

        public EnvironmentVariable() {
            Name = string.Empty;
            Value = string.Empty;
        }

        public EnvironmentVariable(string name, string value) {
            Name = name;
            Value = value;
        }

        #endregion
    }


    [Serializable]
    public class CustomAction : ICloneable, IAction {
        #region Fields

        private const string StartFailedMessage = "Error while executing process {0}. Exception: {1}";
        private const string AbortedMessage = "Process {0} stopped by user.";
        private const string TimeExpiredMessage = "Maximum allowed time ({0}) expired for process {1}";

        #endregion

        #region Fields

        [NonSerialized]
        private Process process;

        [NonSerialized]
        private bool sucessfull;

        [NonSerialized]
        private ScheduleTimer timer;

        [NonSerialized]
        private ManualResetEvent exitEvent;

        #endregion

        #region Constructor

        public CustomAction() {
            _file = string.Empty;
            _enabled = true;
            _variables = new List<EnvironmentVariable>();
        }

        #endregion

        #region Properties

        private bool _enabled;
        public bool Enabled {
            get { return _enabled; }
            set { _enabled = value; }
        }

        [NonSerialized]
        private WipeSession _session;
        public SecureDelete.WipeSession Session {
            get { return _session; }
            set { _session = value; }
        }

        [NonSerialized]
        private bool _afterWipe;
        public bool AfterWipe {
            get { return _afterWipe; }
            set { _afterWipe = value; }
        }

        [NonSerialized]
        private bool _blockingMode;
        public bool BlockingMode {
            get { return _blockingMode; }
            set { _blockingMode = value; }
        }

        private bool _hasMaximumExecutionTime;
        public bool HasMaximumExecutionTime {
            get { return _hasMaximumExecutionTime; }
            set { _hasMaximumExecutionTime = value; }
        }

        private TimeSpan _maximumExecutionTime;
        public TimeSpan MaximumExecutionTime {
            get { return _maximumExecutionTime; }
            set { _maximumExecutionTime = value; }
        }

        private string _file;
        public virtual string File {
            get { return _file; }
            set { _file = value; }
        }

        private string _arguments;
        public virtual string Arguments {
            get { return _arguments; }
            set { _arguments = value; }
        }

        private string _startupDirectory;
        public virtual string StartupDirectory {
            get { return _startupDirectory; }
            set { _startupDirectory = value; }
        }

        private List<EnvironmentVariable> _variables;
        public List<EnvironmentVariable> Variables {
            get { return _variables; }
            set { _variables = value; }
        }

        #endregion

        #region Private methods

        private void ReleaseProcess() {
            if(process != null && process.HasExited) {
                process.Close();
                process.Dispose();
                process = null;
            }
        }

        private void StartTimer() {
            if(_hasMaximumExecutionTime && _maximumExecutionTime.TotalMilliseconds > 0) {
                if(timer != null) {
                    timer.StopTimer();
                }
                else {
                    timer = new ScheduleTimer();
                }

                timer.OnTimeEllapsed += OnTimeExpired;
                timer.StartTimer(_maximumExecutionTime);
            }
        }

        private void StopTimer() {
            if(timer != null) {
                timer.StopTimer();
                timer.OnTimeEllapsed -= OnTimeExpired;
            }
        }

        private void OnProcessExited(object sender, EventArgs e) {
            ReleaseProcess();
            exitEvent.Set();
        }

        private void OnTimeExpired(object sender, EventArgs e) {
            if(process != null && process.HasExited == false) {
                string name = process.ProcessName;

                // kill the process
                try {
                    process.Kill();
                }
                finally {
                    sucessfull = false;
                    ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, 
                                                    TimeExpiredMessage, _maximumExecutionTime, name);
                }
            }

            StopTimer();
        }

        #endregion

        #region Public methods

        public bool Start() {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = _file;

            // set command-line arguments
            if(_arguments != null) {
                info.Arguments = _arguments;
            }

            // set startup directory
            if(_startupDirectory != null) {
                info.WorkingDirectory = _startupDirectory;
            }

            // set environment variables
            if(_variables != null) {
                foreach(EnvironmentVariable variable in _variables) {
                    info.EnvironmentVariables.Add(variable.Name, variable.Value);
                }
            }

            // use the Shell to execute the application
            info.UseShellExecute = true;

            try {
                // start and wait for the process to exit
                process = new Process();
                process.StartInfo = info;
                process.EnableRaisingEvents = true;
                process.Exited += OnProcessExited;
                exitEvent = new ManualResetEvent(false);
                sucessfull = true;

                StartTimer();
                process.Start();

                // wait for the process to exit
                if(_blockingMode) {
                    EndStart();
                }

                return sucessfull;
            }
            catch(Exception e) {
                // something went wrong
                ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, 
                                                StartFailedMessage, _file, e.Message);
                return false;
            }
        }


        public void Stop() {
            if(process != null && process.HasExited == false) {
                string name = process.ProcessName;

                // kill the process
                try {
                    StopTimer();
                    process.Kill();
                }
                finally {
                    ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, 
                                                    AbortedMessage, name);
                }
            }

            sucessfull = false;
        }


        public bool EndStart() {
            if(process != null && process.HasExited == false) {
                exitEvent.WaitOne();
                StopTimer();
            }

            return sucessfull;
        }

        #endregion

        #region ICloneable Members

        public object Clone() {
            CustomAction temp = new CustomAction();

            if(_arguments != null) {
                temp._arguments = (string)_arguments.Clone();
            }

            temp._enabled = _enabled;
            temp._file = (string)_file.Clone();

            if(_startupDirectory != null) {
                temp._startupDirectory = (string)_startupDirectory;
            }

            temp._maximumExecutionTime = _maximumExecutionTime;
            return temp;
        }

        #endregion
    }
}
