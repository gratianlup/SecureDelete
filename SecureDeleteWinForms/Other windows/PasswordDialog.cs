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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SecureDelete;

namespace SecureDeleteWinForms {
    public partial class PasswordDialog : Form {
        public PasswordDialog() {
            InitializeComponent();
        }

        private byte[] _password;
        public byte[] Password {
            get { return _password; }
            set { _password = value; }
        }

        private bool SamePasswords() {
            byte[] a = _password;
            byte[] b = SDOptions.ComputePasswordHash(PasswordTextbox.Text);

            for(int i = 0; i < a.Length; i++) {
                if(a[i] != b[i]) {
                    return false;
                }
            }

            return true;
        }

        private void CheckPasswords() {
            if(SamePasswords() == false) {
                MessageBox.Show("Invalid password.", "SecureDelete", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void InsertButton_Click(object sender, EventArgs e) {
            CheckPasswords();
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void PasswordTextbox_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Enter) {
                CheckPasswords();
            }
        }
    }
}