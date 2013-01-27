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
using System.Reflection;
using SecureDelete.Actions;
using System.IO;

namespace SecureDeleteWinForms {
    public partial class PowerShellObjects : Form {
        public PowerShellObjects() {
            InitializeComponent();
        }

        private string _pluginFolder;
        public string PluginFolder {
            get { return _pluginFolder; }
            set { _pluginFolder = value; }
        }

        private Type[] GetObjects() {
            // get from executing assembly
            List<Type> objectList = new List<Type>();
            objectList.AddRange(BridgeFactory.GetBridgeObjectsTypes());

            // get from folder
            if(_pluginFolder != null && _pluginFolder.Length > 0) {
                try {
                    string[] files = Directory.GetFiles(_pluginFolder, "*.dll");

                    foreach(string file in files) {
                        objectList.AddRange(BridgeFactory.GetBridgeObjectsTypes(BridgeFactory.LoadAssembly(file)));
                    }
                }
                catch { 
                    // Error ignored.
                }
            }

            return objectList.ToArray();
        }

        private void PowerShellObjects_Load(object sender, EventArgs e) {
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.LoadPosition(this);
            LoadObjects();

            // select first
            if(ObjectList.Items.Count > 0) {
                ObjectList.Items[0].Selected = true;
            }
        }

        private void LoadObjects() {
            Type[] types = GetObjects();

            foreach(Type type in types) {
                Bridge bridge = (Bridge)(type.GetCustomAttributes(typeof(Bridge), false)[0]);

                if(bridge.Exposed) {
                    ListViewItem item = new ListViewItem(bridge.Name);
                    item.Tag = type;
                    item.ImageIndex = 2;
                    ObjectList.Items.Add(item);
                }
            }

            ObjectLabel.Text = "Objects: " + ObjectList.Items.Count.ToString();
        }

        private void ShowDetails(Type type) {
            MemberList.Items.Clear();
            MemberInfo[] info = type.GetMembers();

            // sort members by name
            for(int i = 0; i < info.Length - 1; i++) {
                for(int j = i + 1; j < info.Length; j++) {
                    if(info[i].GetCustomAttributes(typeof(BridgeMember), false).Length == 0 ||
                       info[j].GetCustomAttributes(typeof(BridgeMember), false).Length == 0) {
                        continue;
                    }

                    BridgeMember a = (BridgeMember)(info[i].GetCustomAttributes(typeof(BridgeMember), false)[0]);
                    BridgeMember b = (BridgeMember)(info[j].GetCustomAttributes(typeof(BridgeMember), false)[0]);

                    if(a.Name.CompareTo(b.Name) > 0) {
                        MemberInfo c = info[i];
                        info[i] = info[j];
                        info[j] = c;
                    }
                }
            }

            foreach(MemberInfo m in info) {
                if(m.GetCustomAttributes(typeof(BridgeMember), false).Length > 0) {
                    BridgeMember attribute = (BridgeMember)(m.GetCustomAttributes(typeof(BridgeMember), false)[0]);

                    ListViewItem item = new ListViewItem();
                    item.Text = attribute.Name;

                    if(m.MemberType == MemberTypes.Method) {
                        item.ImageIndex = 0;
                        item.SubItems.Add("Method");
                    }
                    else if(m.MemberType == MemberTypes.Property) {
                        item.ImageIndex = 1;
                        item.SubItems.Add("Property");
                    }

                    item.Tag = m;
                    MemberList.Items.Add(item);
                }
            }
        }

        private void ShowMemberDetails(MemberInfo member) {
            BridgeMember attribute = (BridgeMember)(member.GetCustomAttributes(typeof(BridgeMember), false)[0]);
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(attribute.Signature);
            builder.AppendLine("\nSummary:");
            builder.AppendLine(attribute.Description);

            if(member.GetCustomAttributes(typeof(BridgeMemberParameter), false).Length > 0) {
                builder.AppendLine("\nParameters:");
                object[] parameters = member.GetCustomAttributes(typeof(BridgeMemberParameter), false);

                // sort the list of parameters
                for(int i = 0; i < parameters.Length - 1; i++) {
                    for(int j = i + 1; j < parameters.Length; j++) {
                        BridgeMemberParameter a = (BridgeMemberParameter)parameters[i];
                        BridgeMemberParameter b = (BridgeMemberParameter)parameters[j];

                        if(a.Order > b.Order) {
                            BridgeMemberParameter c = a;
                            parameters[i] = parameters[j];
                            parameters[j] = c;
                        }
                    }
                }

                foreach(BridgeMemberParameter parameter in parameters) {
                    builder.Append(parameter.Name);
                    builder.Append(": ");
                    builder.AppendLine(parameter.Description);
                }
            }

            DescriptionBox.Text = builder.ToString();
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void ObjectList_SelectedIndexChanged(object sender, EventArgs e) {
            if(ObjectList.SelectedItems.Count > 0) {
                Type t = (Type)ObjectList.SelectedItems[0].Tag;
                ShowDetails(t);

                // select first member
                if(MemberList.Items.Count > 0) {
                    MemberList.Items[0].Selected = true;
                }
            }
        }

        private void MemberList_SelectedIndexChanged(object sender, EventArgs e) {
            if(MemberList.SelectedItems.Count > 0) {
                MemberInfo info = (MemberInfo)MemberList.SelectedItems[0].Tag;
                ShowMemberDetails(info);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void PowerShellObjects_FormClosing(object sender, FormClosingEventArgs e) {
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.SavePosition(this);
        }
    }
}
