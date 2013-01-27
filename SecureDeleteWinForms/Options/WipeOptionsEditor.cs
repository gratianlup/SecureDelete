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
using SecureDelete;

namespace SecureDeleteWinForms.Options {
    public partial class WipeOptionsEditor : UserControl {
        public WipeOptionsEditor() {
            InitializeComponent();
        }

        #region Properties

        private WipeOptions _options;
        public WipeOptions Options {
            get { return _options; }
            set {
                _options = value;
                HandleNewOptions();
            }
        }

        private WipeMethodManager _methodManager;
        public WipeMethodManager MethodManager {
            get { return _methodManager; }
            set { _methodManager = value; }
        }

        #endregion

        private void HandleNewOptions() {
            if(_options == null) {
                return;
            }

            WipeAdsCheckbox.Checked = _options.WipeAds;
            WipeFileNamesCheckbox.Checked = _options.WipeFileName;
            TotalDeleteCheckbox.Checked = _options.TotalDelete;
            UpdateMethodInfo(FileMethodNameLabel, _options.DefaultFileMethod);
            DestroyFreeSpaceCheckbox.Checked = _options.DestroyFreeSpaceFiles;
            UpdateMethodInfo(FreeSpaceMethodNameLabel, _options.DefaultFreeSpaceMethod);
            WipeUsedFileRecordCheckbox.Checked = _options.WipeUsedFileRecord;
            WipeUnusedFileRecordCheckbox.Checked = _options.WipeUnusedFileRecord;
            WipeUsedIndexRecordCheckbox.Checked = _options.WipeUsedIndexRecord;
            WipeUnusedIndexRecordCheckbox.Checked = _options.WipeUnusedIndexRecord;
        }

        private void UpdateMethodInfo(Label label, int method) {
            if(_methodManager != null && _methodManager.Methods.Count > 0) {
                MethodChangeButton.Enabled = true;
                WipeMethod m = _methodManager.GetMethod(method);

                if(m != null) {
                    label.Text = m.Name;
                }
                else {
                    label.Text = "Wipe method not found";
                }
            }
            else {
                label.Text = "No wipe method available";
            }
        }

        private void SelectMethod(Label label, ref int method) {
            WipeMethods w = new WipeMethods();
            w.MethodManager = _methodManager;
            w.SelectedMethod = _methodManager.GetMethodIndex(method);
            w.ShowSelected = true;
            w.ShowDialog(this);

            if(w.SelectedMethod < 0 || w.SelectedMethod >= _methodManager.Methods.Count) {
                method = WipeOptions.DefaultWipeMethod;
            }
            else {
                method = _methodManager.Methods[w.SelectedMethod].Id;
            }

            UpdateMethodInfo(label, method);
        }

        private void WipeOptions_BackColorChanged(object sender, EventArgs e) {
            WipePanel.BackColor = this.BackColor;
        }

        private void WipeOptions_EnabledChanged(object sender, EventArgs e) {
            WipeAdsCheckbox.Enabled = this.Enabled;
            WipeFileNamesCheckbox.Enabled = this.Enabled;
            TotalDeleteCheckbox.Enabled = this.Enabled;
            MethodChangeButton.Enabled = this.Enabled;
            FreeSpaceChangeButton.Enabled = this.Enabled;
            WipeUsedFileRecordCheckbox.Enabled = this.Enabled;
            WipeUnusedFileRecordCheckbox.Enabled = this.Enabled;
            WipeUsedIndexRecordCheckbox.Enabled = this.Enabled;
            WipeUnusedIndexRecordCheckbox.Enabled = this.Enabled;
        }

        private void WipeAdsCheckbox_CheckedChanged(object sender, EventArgs e) {
            _options.WipeAds = WipeAdsCheckbox.Checked;
        }

        private void WipeFileNamesCheckbox_CheckedChanged(object sender, EventArgs e) {
            _options.WipeFileName = WipeFileNamesCheckbox.Checked;
        }

        private void TotalDeleteCheckbox_CheckedChanged(object sender, EventArgs e) {
            _options.TotalDelete = TotalDeleteCheckbox.Checked;
        }

        private void MethodChangeButton_Click(object sender, EventArgs e) {
            int method = _options.DefaultFileMethod;
            SelectMethod(FileMethodNameLabel, ref method);
            _options.DefaultFileMethod = method;
        }

        private void FreeSpaceChangeButton_Click(object sender, EventArgs e) {
            int method = _options.DefaultFreeSpaceMethod;
            SelectMethod(FreeSpaceMethodNameLabel, ref method);
            _options.DefaultFreeSpaceMethod = method;
        }

        private void DestroyFreeSpaceCheckbox_CheckedChanged(object sender, EventArgs e) {
            _options.DestroyFreeSpaceFiles = DestroyFreeSpaceCheckbox.Checked;
        }

        private void WipeUsedFileRecordCheckbox_CheckedChanged(object sender, EventArgs e) {
            _options.WipeUsedIndexRecord = WipeUsedIndexRecordCheckbox.Checked;
        }

        private void WipeUnusedIndexRecordCheckbox_CheckedChanged(object sender, EventArgs e) {
            _options.WipeUnusedIndexRecord = WipeUnusedIndexRecordCheckbox.Checked;
        }
    }
}
