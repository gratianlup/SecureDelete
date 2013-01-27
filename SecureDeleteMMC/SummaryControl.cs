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
using Microsoft.ManagementConsole;
using System.Reflection;
using SecureDeleteWinForms;
using SecureDelete;

namespace SecureDeleteMMC {
    public partial class SummaryControl : UserControl, IFormViewControl {
        private bool pluginInfoLoaded;

        public SummaryControl() {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        #region IFormViewControl Members

        public void Initialize(FormView view) {
            UpdateData();
        }

        public void UpdateData() {
            PluginLabel.Text = "Installed Plugins: " + GetPluginCount();
            PowershellLabel.Text = "PowerShell Installed: " + GetPowerShellStatus();
            UpdateReportInfo();
        }

        #endregion

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ReportPanel.ExpandedSize = 300;
            linkLabel2.Visible = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            AboutPanel.ExpandedSize = 270;
            linkLabel1.Visible = false;

            LoadPluginInfo();
        }

        private string GetPluginCount() {
            string[] files = SecureDeleteLocations.GetPluginAssemblies();

            if(files == null) {
                return "0";
            }
            else {
                return files.Length.ToString();
            }
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
                    catch {
                        // TODO: Exception should not be ignored.
                    }
                }
            }

            pluginInfoLoaded = true;
        }

        private string GetPowerShellStatus() {
            int version;

            if(!SecureDelete.Actions.PowershellAction.IsPowershellInstalled(out version)) {
                return "No";
            }
            else {
                return "Yes";
            }
        }

        private void UpdateReportInfo() {
            WipeReportManager man = new WipeReportManager();
            man.ReportDirectory = SecureDeleteLocations.GetReportDirectory();
            man.LoadReportCategories(SecureDeleteLocations.GetReportFilePath());
            CategoryList.Items.Clear();

            // count
            int categories = 0;
            int reports = 0;
            int errors = 0;
            int failed = 0;

            SDOptions options = new SDOptions();
            SDOptionsFile.TryLoadOptions(out options);

            foreach(KeyValuePair<Guid, ReportCategory> category in man.Categories) {
                categories++;
                ListViewItem item = new ListViewItem();
                string name = "Not found";

                if(category.Value.Guid == WipeSession.DefaultSessionGuid) {
                    name = "Default";
                    item.ImageIndex = 0;
                }
                else {
                    if(options != null && options.SessionNames != null &&
                       options.SessionNames.ContainsKey(category.Value.Guid)) {
                        name = options.SessionNames[category.Value.Guid];
                        item.ImageIndex = 1;
                    }
                    else {
                        item.ImageIndex = 2;
                    }
                }

                item.Text = name;
                item.SubItems.Add(category.Value.Reports.Count.ToString());
                CategoryList.Items.Add(item);

                // count messages
                foreach(KeyValuePair<long, ReportInfo> report in category.Value.Reports) {
                    reports++;
                    errors += report.Value.ErrorCount;
                    failed += report.Value.FailedObjectCount;
                }
            }

            ReportCountLabel.Text = "Reports: " + reports.ToString();
            ErrorCountLabel.Text = "Errors: " + errors.ToString();
            FailedCountLabel.Text = "Failed objects: " + failed.ToString();
            CategoryCountLabel.Text = "Categories: " + categories.ToString();
        }

        private void button2_Click(object sender, EventArgs e) {
            WipeReportManager man = new WipeReportManager();
            man.ReportDirectory = SecureDeleteLocations.GetReportDirectory();
            man.LoadReportCategories(SecureDeleteLocations.GetReportFilePath());
            man.DestroyDatabase();
            man.SaveReportCategories(SecureDeleteLocations.GetReportFilePath());
            UpdateReportInfo();
        }

        private void button4_Click(object sender, EventArgs e) {
            SDOptions options = new SDOptions();
            SDOptionsFile.TryLoadOptions(out options);

            OptionsForm f = new OptionsForm();
            f.StartPanel = OptionsFormStartPanel.General;
            f.Options = options;

            if(f.ShowDialog() == DialogResult.OK) {
                SDOptionsFile.TrySaveOptions(options);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            SDOptions options = new SDOptions();
            SDOptionsFile.TryLoadOptions(out options);

            OptionsForm f = new OptionsForm();
            f.StartPanel = OptionsFormStartPanel.Schedule;
            f.Options = options;

            if(f.ShowDialog() == DialogResult.OK) {
                SDOptionsFile.TrySaveOptions(options);
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            SDOptions options = new SDOptions();
            SDOptionsFile.TryLoadOptions(out options);

            OptionsForm f = new OptionsForm();
            f.StartPanel = OptionsFormStartPanel.Reports;
            f.Options = options;

            if(f.ShowDialog() == DialogResult.OK) {
                SDOptionsFile.TrySaveOptions(options);
            }
        }
    }
}
