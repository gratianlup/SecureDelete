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
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace SecureDeleteWinForms {
    public class BackgoundTask {
        #region Constructor

        public BackgoundTask() {
            _attachedControls = new List<KeyValuePair<ToolStripMenuItem, ToolStripDropDownItem>>();
        }

        #endregion

        #region Properties

        private string _name;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        private Image _image;
        public Image Image {
            get { return _image; }
            set { _image = value; }
        }

        private ProgressBarStyle _progressStyle;
        public ProgressBarStyle ProgressStyle {
            get { return _progressStyle; }
            set { _progressStyle = value; if(OnProgressChanged != null) OnProgressChanged(this, null); }
        }

        private double _progressValue;
        public double ProgressValue {
            get { return _progressValue; }
            set { _progressValue = value; if(OnProgressChanged != null) OnProgressChanged(this, null); }
        }

        private List<KeyValuePair<ToolStripMenuItem, ToolStripDropDownItem>> _attachedControls;
        public List<KeyValuePair<ToolStripMenuItem, ToolStripDropDownItem>> AttachedControls {
            get { return _attachedControls; }
            set { _attachedControls = value; }
        }

        private bool _stopped;
        public bool Stopped {
            get { return _stopped; }
            set { _stopped = value; }
        }

        #endregion

        #region Events

        public event EventHandler OnStopped;
        public event EventHandler OnProgressChanged;

        #endregion

        #region Public methods

        public void Stop() {
            _stopped = true;

            if(OnStopped != null) {
                OnStopped(this, null);
            }
        }

        public void Destroy() {
            foreach(KeyValuePair<ToolStripMenuItem, ToolStripDropDownItem> kvp in _attachedControls) {
                kvp.Value.DropDownItems.Remove(kvp.Key);
            }

            _attachedControls.Clear();
        }

        #endregion
    }
}
