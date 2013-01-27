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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SecureDeleteWinForms {
    public partial class StatusWindowElement : UserControl {
        public StatusWindowElement() {
            InitializeComponent();
        }

        #region Properties

        private BackgoundTask _task;
        public SecureDeleteWinForms.BackgoundTask Task {
            get { return _task; }
            set {
                _task = value;
                StatusIconInternal.Image = _task.Image;
                StatusLabelInternal.Text = _task.Name;
                AttachEvents();
                OnBackgroundTaskProgressChanged(_task, null);
            }
        }

        #endregion

        #region Events

        public event EventHandler OnStopClicked;

        #endregion

        public void Stop() {
            if(_task == null) {
                return;
            }

            _task.Stop();
            DetachEvents();
            if(OnStopClicked != null) {
                OnStopClicked(this, null);
            }
        }

        private void AttachEvents() {
            if(_task == null) {
                return;
            }

            _task.OnProgressChanged += OnBackgroundTaskProgressChanged;
        }

        private void DetachEvents() {
            if(_task == null) {
                return;
            }

            _task.OnProgressChanged -= OnBackgroundTaskProgressChanged;
        }

        private void OnBackgroundTaskProgressChanged(object sender, EventArgs e) {
            if((BackgoundTask)sender == _task && this.Visible) {
                // update the UI
                double progress = Math.Max(ProgressBar.Minimum, Math.Min(ProgressBar.Maximum, _task.ProgressValue));

                ProgressBar.Style = _task.ProgressStyle;
                ProgressBar.Value = (int)progress;

                if(_task.ProgressStyle != ProgressBarStyle.Marquee) {
                    toolTip1.SetToolTip(ProgressBar, string.Format("{0:f2} %", (progress / (double)ProgressBar.Maximum) * 100));
                }
                else {
                    toolTip1.RemoveAll();
                }
            }
        }

        private void StopButton_Click(object sender, EventArgs e) {
            Stop();
        }
    }
}
