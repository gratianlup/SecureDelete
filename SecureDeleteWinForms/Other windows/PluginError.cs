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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SecureDelete.WipePlugin;

namespace SecureDeleteWinForms {
    public partial class PluginError : Form {
        public PluginError() {
            InitializeComponent();
        }

        private Plugin _plugin;
        public Plugin Plugin {
            get { return _plugin; }
            set { _plugin = value; }
        }

        private Exception _exception;
        public Exception Exception {
            get { return _exception; }
            set { _exception = value; }
        }

        private void PluginError_Load(object sender, EventArgs e) {
            NameLabel.Text = _plugin.Name;
            VersionLabel.Text = _plugin.VersionString;
            AuthorLabel.Text = _plugin.HasAuthor ? _plugin.Author : "Not available";

            // details
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Plugin Crash Report");
            builder.AppendLine(DateTime.Now.ToString());
            builder.AppendLine();
            builder.AppendLine("Name: " + _plugin.Name);
            builder.AppendLine("Version: " + _plugin.VersionString);
            builder.AppendLine("Author: " + (_plugin.HasAuthor ? _plugin.Author : "Not available"));
            builder.AppendLine();
            builder.AppendLine("Exception: " + _exception.Message);
            builder.AppendLine("Stack:");
            builder.Append(_exception.StackTrace);

            DetailsBox.Text = builder.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e) {
            this.Height = 434;
            button1.Enabled = false;
        }

        private void button2_Click_1(object sender, EventArgs e) {
            Clipboard.SetText(DetailsBox.Text);
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
