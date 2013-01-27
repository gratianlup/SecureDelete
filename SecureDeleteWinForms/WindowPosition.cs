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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SecureDeleteWinForms {
    public class WindowPosition {
        public Point Location;
        public Size Size;
        public FormWindowState State;
    }


    public class WindowPositionManager {
        #region Properties

        private Dictionary<string, WindowPosition> _windows;
        public Dictionary<string, WindowPosition> Windows {
            get { return _windows; }
            set { _windows = value; }
        }

        #endregion

        #region Constructor

        public WindowPositionManager() {
            _windows = new Dictionary<string, WindowPosition>();
        }

        #endregion

        #region Public methods

        public void LoadPosition(Form form) {
            if(_windows.ContainsKey(form.Name)) {
                WindowPosition position = _windows[form.Name];
                form.Size = position.Size;
                form.Location = position.Location;
                form.WindowState = position.State;
            }
        }


        public void SavePosition(Form form) {
            if(form == null) {
                return;
            }

            WindowPosition position = new WindowPosition();
            position.State = form.WindowState;

            if(form.WindowState == FormWindowState.Normal) {
                position.Size = form.Size;
                position.Location = form.Location;
            }
            else {
                position.Size = form.RestoreBounds.Size;
                position.Location = form.RestoreBounds.Location;
            }

            // add to the list
            if(_windows.ContainsKey(form.Name)) {
                _windows[form.Name] = position;
            }
            else {
                _windows.Add(form.Name, position);
            }

            // save settings
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager = this;
            SecureDeleteWinForms.Properties.Settings.Default.Save();
        }

        #endregion
    }
}
