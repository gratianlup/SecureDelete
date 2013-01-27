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
using System.IO;
using System.Diagnostics;
using System.Net.Mail;
using DebugUtils.Debugger;

namespace SecureDelete {
    public partial class CrashDialog : Form {
        public CrashDialog() {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e) {
            this.Height = 556;
        }

        private void CrashDialog_Load(object sender, EventArgs e) {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SecureDelete";
            linkLabel1.Text = path;

            if(Directory.Exists(path) == false) {
                MessageBox.Show("Nothing to report.");
                this.Close();
            }

            // load details
            try {
                string details = Path.Combine(path, "SecureDeleteDump.txt");
                DetailsBox.LoadFile(details, RichTextBoxStreamType.PlainText);
            }
            catch { }
        }

        private void button1_Click_1(object sender, EventArgs e) {
            if(DebugUtils.Debugger.Debug.DeserializeMessages(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + 
                                                             "\\SecureDelete\\SecureDeleteMessages.xml")) {
                DebugUtils.Debugger.Debug.GenerateHtmlReport();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SecureDelete";

            try {
                System.Diagnostics.Process.Start(path);
            }
            catch { }
        }
    }
}