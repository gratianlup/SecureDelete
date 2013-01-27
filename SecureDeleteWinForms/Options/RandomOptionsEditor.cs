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
using SecureDelete;

namespace SecureDeleteWinForms.Options {
    public partial class RandomOptionsEditor : UserControl {
        public RandomOptionsEditor() {
            InitializeComponent();
        }

        #region Properties

        private RandomOptions _options;
        public RandomOptions Options {
            get { return _options; }
            set {
                _options = value;
                HandleNewOptions();
            }
        }

        #endregion

        private void HandleNewOptions() {
            if(_options == null) {
                return;
            }

            if(_options.RandomProvider == RandomProvider.ISAAC) {
                IsaacOptionbox.Checked = true;
            }
            else if(_options.RandomProvider == RandomProvider.Mersenne) {
                MersenneOptionbox.Checked = true;
            }

            SlowPoolCheckbox.Checked = _options.UseSlowPool;
            PreventCheckbox.Checked = _options.PreventWriteToSwap;
            ReseedCheckbox.Checked = _options.Reseed;
            ReseedIntervalTrackbar.Enabled = ReseedCheckbox.Checked;
            ReseedIntervalTrackbar.Value = Math.Max(1, (int)_options.ReseedInterval.TotalMinutes);
        }

        private void RandomOptions_BackColorChanged(object sender, EventArgs e) {
            RandomPanel.BackColor = this.BackColor;
        }

        private void RandomOptions_EnabledChanged(object sender, EventArgs e) {
            IsaacOptionbox.Enabled = this.Enabled;
            MersenneOptionbox.Enabled = this.Enabled;
            SlowPoolCheckbox.Enabled = this.Enabled;
            PreventCheckbox.Enabled = this.Enabled;
            ReseedCheckbox.Enabled = this.Enabled;
            ReseedIntervalTrackbar.Enabled = this.Enabled && ReseedCheckbox.Checked;
        }

        private void IsaacOptionbox_CheckedChanged(object sender, EventArgs e) {
            _options.RandomProvider = RandomProvider.ISAAC;
        }

        private void MersenneOptionbox_CheckedChanged(object sender, EventArgs e) {
            _options.RandomProvider = RandomProvider.Mersenne;
        }

        private void SlowPoolCheckbox_CheckedChanged(object sender, EventArgs e) {
            _options.UseSlowPool = SlowPoolCheckbox.Checked;
        }

        private void PreventCheckbox_CheckedChanged(object sender, EventArgs e) {
            _options.PreventWriteToSwap = PreventCheckbox.Checked;
        }

        private void ReseedCheckbox_CheckedChanged(object sender, EventArgs e) {
            _options.Reseed = ReseedCheckbox.Checked;
            ReseedIntervalTrackbar.Enabled = _options.Reseed;
        }

        private void ReseedIntervalTrackbar_ValueChanged(object sender, EventArgs e) {
            _options.ReseedInterval = TimeSpan.FromMinutes(ReseedIntervalTrackbar.Value);
            _options.PoolUpdateInterval = TimeSpan.FromMilliseconds(_options.ReseedInterval.TotalMilliseconds / 5);
            ReseedLabel.Text = _options.ReseedInterval.ToString();
        }
    }
}
