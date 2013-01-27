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
using System.Reflection;
using SecureDelete.Actions;

namespace SecureDeleteWinForms {
    public partial class PowerShellObjectResults : Form {
        public PowerShellObjectResults() {
            InitializeComponent();
        }

        private BridgeLogger _logger;
        public SecureDelete.Actions.BridgeLogger Logger {
            get { return _logger; }
            set {
                _logger = value;
                HandleNewLogger();
            }
        }

        private string GetBridgeName(BridgeLoggerItem member) {
            object[] attributes = member.Method.DeclaringType.GetCustomAttributes(typeof(Bridge), false);
            bool found = false;
            string name = "";

            if(attributes.Length > 0) {
                for(int j = 0; j < attributes.Length; j++) {
                    if(attributes[j] is Bridge) {
                        name = ((Bridge)attributes[j]).Name;
                        found = true;
                        break;
                    }
                }
            }

            // attribute not found
            if(found == false) {
                name = member.Method.DeclaringType.Name;
            }

            return name;
        }

        private bool IsProperty(BridgeLoggerItem member) {
            return member.Method.Name.StartsWith("get_") ||
                   member.Method.Name.StartsWith("set_");
        }

        private string GetMemberName(BridgeLoggerItem member) {
            string name = member.Method.Name;

            if(IsProperty(member)) {
                name = name.Substring(4);
            }

            return name;
        }

        private void HandleNewLogger() {
            MemberList.Items.Clear();

            if(_logger == null) {
                return;
            }

            string filterValue = SearchTextbox.Text.Trim().ToLower();
            bool filter = filterValue.Length > 0;

            for(int i = 0; i < _logger.Items.Count; i++) {
                ListViewItem item = new ListViewItem();
                BridgeLoggerItem member = _logger.Items[i];
                bool property = IsProperty(member);

                // filter by type
                if(property && !PropertyButton.Checked) {
                    continue;
                }

                if(!property == false && !MemberButton.Checked) {
                    continue;
                }

                // filter by member name
                if(filter && !member.Method.Name.ToLower().Contains(filterValue)) {
                    continue;
                }

                item.Text = member.Time.ToLongTimeString();
                item.SubItems.Add(GetBridgeName(member));
                item.SubItems.Add(GetMemberName(member));
                item.ImageIndex = property ? 1 : 0;
                item.Tag = _logger.Items[i];
                MemberList.Items.Add(item);
            }

            ActionLabel.Text = "Registered actions (" + MemberList.Items.Count.ToString() + ")";
        }

        private void MemberList_SelectedIndexChanged(object sender, EventArgs e) {
            if(MemberList.SelectedItems.Count > 0) {
                ShowDetails((BridgeLoggerItem)MemberList.SelectedItems[0].Tag);
            }
            else {
                ParameterList.Items.Clear();
                PropertyList.Items.Clear();
                ObjectLabel.Text = "Object:";
                MemberLabel.Text = "Member:";
                TypeLabel.Text = "Type:";
            }
        }

        private void ShowDetails(BridgeLoggerItem member) {
            ObjectLabel.Text = "Object: " + GetBridgeName(member);
            MemberLabel.Text = "Member: " + GetMemberName(member);
            string type;

            if(member.Method.Name.StartsWith("get_")) {
                type = "Property (get)";
            }
            else if(member.Method.Name.StartsWith("set_")) {
                type = "Property (set)";
            }
            else {
                type = "Method";
            }

            TypeLabel.Text = "Type: " + type;
            ParameterList.Items.Clear();

            for(int i = 0; i < member.Parameters.Count; i++) {
                ListViewItem item = new ListViewItem();

                if(member.Parameters[i].Info == null) {
                    item.Text = "value";
                }
                else {
                    item.Text = member.Parameters[i].Info.Name;
                }
                item.Tag = member.Parameters[i];

                if(member.Parameters[i].Value != null) {
                    item.SubItems.Add(member.Parameters[i].Value.ToString());
                    item.ImageIndex = 2;
                }

                ParameterList.Items.Add(item);
            }

            PropertyList.Items.Clear();

            if(ParameterList.Items.Count > 0) {
                ParameterList.Items[0].Selected = true;
            }
        }

        private void ParameterList_SelectedIndexChanged(object sender, EventArgs e) {
            if(ParameterList.SelectedItems.Count > 0) {
                ShowProperties(((MethodParameterInfo)ParameterList.SelectedItems[0].Tag).Value);
            }
        }

        private void ShowProperties(object obj) {
            PropertyList.Items.Clear();

            // get properties
            if(obj != null) {
                PropertyInfo[] properties = obj.GetType().GetProperties();

                foreach(PropertyInfo info in properties) {
                    try {
                        object value = info.GetValue(obj, null);

                        if(value != null) {
                            ListViewItem item = new ListViewItem();
                            item.Text = info.Name;
                            item.SubItems.Add(value.ToString());
                            item.ImageIndex = 1;
                            PropertyList.Items.Add(item);
                        }
                    }
                    catch { }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void PowerShellObjectResults_Load(object sender, EventArgs e) {
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.LoadPosition(this);

            // select first item
            if(MemberList.Items.Count > 0) {
                MemberList.Items[0].Selected = true;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            SearchTextbox.Text = "";
        }

        private void SearchTextbox_Click(object sender, EventArgs e) {
            HandleNewLogger();
        }

        private void MemberButton_Click(object sender, EventArgs e) {
            HandleNewLogger();
        }

        private void PropertyButton_Click(object sender, EventArgs e) {
            HandleNewLogger();
        }

        private void SearchTextbox_TextChanged(object sender, EventArgs e) {
            HandleNewLogger();

            if(SearchTextbox.Text.Length > 0 && MemberList.Items.Count == 0) {
                SearchTextbox.BackColor = Color.Tomato;
            }
            else {
                SearchTextbox.BackColor = Color.FromName("Window");
            }
        }

        private void PowerShellObjectResults_FormClosing(object sender, FormClosingEventArgs e) {
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.SavePosition(this);
        }
    }
}
