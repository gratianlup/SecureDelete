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
using SecureDelete;
using System.Reflection;
using SecureDelete.WipePlugin;

namespace SecureDeleteWinForms {
    public partial class AboutBox : Form {
        private bool pluginInfoLoaded;

        public AboutBox() {
            InitializeComponent();
            AboutSelector.Selected = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void AboutBox_KeyDown(object sender, KeyEventArgs e) {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Right) {
                throw new Exception("Test exception");
            }
        }

        private void AboutSelector_SelectedStateChanged(object sender, EventArgs e) {
            AboutPanel.Visible = AboutSelector.Selected;
            PluginPanel.Visible = !AboutSelector.Selected;

            if(AboutSelector.Selected) {
                PluginSelector.Selected = false;
            }
        }

        private void PluginSelector_SelectedStateChanged(object sender, EventArgs e) {
            AboutPanel.Visible = !PluginSelector.Selected;
            PluginPanel.Visible = PluginSelector.Selected;

            if(PluginSelector.Selected) {
                AboutSelector.Selected = false;
                LoadPluginInfo();
            }
        }

        private void AboutBox_Load(object sender, EventArgs e) {
            AboutSelector.Selected = true;
        }

        private void LoadPluginInfo() {
            if(pluginInfoLoaded) {
                return;
            }

            string[] files = SecureDeleteLocations.GetPluginAssemblies();

            if(files != null && files.Length > 0) {
                foreach(string f in files) {
                    try {
                        AssemblyName name = AssemblyName.GetAssemblyName(f);

                        if(name != null) {
                            ListViewItem item = new ListViewItem();
                            item.Text = name.Name;
                            item.SubItems.Add(name.Version.ToString());

                            PluginList.Items.Add(item);
                        }
                    }
                    catch { }
                }
            }

            pluginInfoLoaded = true;
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Right) {
                throw new Exception("Test exception");
            }
        }
    }
}