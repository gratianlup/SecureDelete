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
using SecureDelete.Actions;
using TextParser;
using TextParser.IncludedFilters;
using System.IO;
using System.Management.Automation.Runspaces;

namespace SecureDeleteWinForms {
    public partial class PowershellActionEditor : UserControl, IActionEditor {
        public PowershellActionEditor() {
            InitializeComponent();
        }

        #region IActionEditor Members

        public ActionEditorType Type {
            get { return ActionEditorType.PowershellActionEditor; }
        }

        private PowershellAction _action;
        public IAction Action {
            get { return _action; }
            set {
                if((value is PowershellAction) == false) {
                    throw new Exception("Invalid value");
                }

                _action = (PowershellAction)value;
                HandleNewAction();
            }
        }

        public string TypeString {
            get { return "PowerShell Script"; }
        }

        public string ActionString {
            get {
                if(_action == null) {
                    return "";
                }
                else {
                    return _action.UseScriptFile ? "Script file" : "Custom script";
                }
            }
        }

        public event EventHandler OnStatusChanged;

        public int RequiredSize {
            get { return 254; }
        }

        #endregion

        private void HandleNewAction() {
            EnabledCheckbox.Checked = _action.Enabled;

            if(_action.UseScriptFile) {
                FileOptionbox.Checked = true;
            }
            else {
                CustomOptionbox.Checked = true;
            }

            LocationTextBox.Text = _action.ScriptFile;
            PluginFolderCheckbox.Checked = _action.UsePluginFolder;
            PluginFolderTextobx.Text = _action.PluginFolder;
            TimeLimitCheckbox.Checked = _action.HasMaximumExecutionTime;

            if(_action.HasMaximumExecutionTime && _action.MaximumExecutionTime == null) {
                _action.MaximumExecutionTime = new TimeSpan(TimeLimitValue.MinDate.Ticks);
            }

            TimeLimitValue.Value = new DateTime(TimeLimitValue.MinDate.Ticks + 
                                                _action.MaximumExecutionTime.Ticks);
        }

        private void SendStatusChanged() {
            if(OnStatusChanged != null) {
                OnStatusChanged(this, null);
            }
        }

        private void StyleBrowseButton_Click(object sender, EventArgs e) {
            CommonDialog dialog = new CommonDialog();
            dialog.Title = "Select script file";
            dialog.Filter.Add(new FilterEntry("Text file", "*.txt"));
            dialog.Filter.Add(new FilterEntry("All files", "*.*"));

            if(dialog.ShowOpen()) {
                LocationTextBox.Text = dialog.FileName;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            _action.UsePluginFolder = PluginFolderCheckbox.Checked;
            SetEnabledState();
        }

        private void PluginButton_Click(object sender, EventArgs e) {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select the folder.";
            dialog.ShowNewFolderButton = false;

            if(dialog.ShowDialog() == DialogResult.OK) {
                PluginFolderTextobx.Text = dialog.SelectedPath;
            }
        }

        private void CustomOptionbox_CheckedChanged(object sender, EventArgs e) {
            _action.UseScriptFile = !CustomOptionbox.Checked;
            SetEnabledState();
            SendStatusChanged();
        }

        private void SetEnabledState() {
            FileOptionbox.Enabled = EnabledCheckbox.Checked;
            CustomOptionbox.Enabled = EnabledCheckbox.Checked;

            PluginLabel.Enabled = PluginFolderCheckbox.Checked && EnabledCheckbox.Checked;
            PluginFolderTextobx.Enabled = PluginFolderCheckbox.Checked && EnabledCheckbox.Checked;
            PluginButton.Enabled = PluginFolderCheckbox.Checked && EnabledCheckbox.Checked;
            PluginFolderCheckbox.Enabled = EnabledCheckbox.Checked;
            LocationButton.Enabled = FileOptionbox.Checked && EnabledCheckbox.Checked;
            LocationLabel.Enabled = FileOptionbox.Checked && EnabledCheckbox.Checked;
            LocationTextBox.Enabled = FileOptionbox.Checked && EnabledCheckbox.Checked;
            EditButton.Enabled = CustomOptionbox.Checked && EnabledCheckbox.Checked;
            TimeLimitValue.Enabled = TimeLimitCheckbox.Checked && EnabledCheckbox.Checked;
        }

        private void EnabledCheckbox_CheckedChanged(object sender, EventArgs e) {
            _action.Enabled = EnabledCheckbox.Checked;
            SendStatusChanged();
            SetEnabledState();
        }

        private void LocationTextBox_TextChanged(object sender, EventArgs e) {
            _action.ScriptFile = LocationTextBox.Text;
        }

        private void PluginFolderTextobx_TextChanged(object sender, EventArgs e) {
            _action.PluginFolder = PluginFolderTextobx.Text;
        }

        private void EditButton_Click(object sender, EventArgs e) {
            PowerShellScriptEditor editor = new PowerShellScriptEditor();
            editor.Script = _action.Script;
            editor.PluginFolder = PluginFolderCheckbox.Checked ? PluginFolderTextobx.Text : null;

            if(editor.ShowDialog() == DialogResult.OK) {
                _action.Script = editor.Script;
            }
        }

        private void PluginFolderCheckbox_CheckedChanged(object sender, EventArgs e) {
            PluginLabel.Enabled = PluginFolderCheckbox.Checked && EnabledCheckbox.Checked;
            PluginFolderTextobx.Enabled = PluginFolderCheckbox.Checked && EnabledCheckbox.Checked;
            PluginButton.Enabled = PluginFolderCheckbox.Checked && EnabledCheckbox.Checked;
        }

        private void FileOptionbox_CheckedChanged(object sender, EventArgs e) {
            _action.UseScriptFile = !CustomOptionbox.Checked;
            SetEnabledState();
            SendStatusChanged();
        }

        private void TimeLimitCheckbox_CheckedChanged(object sender, EventArgs e) {
            SetEnabledState();
        }

        private void TimeLimitValue_ValueChanged(object sender, EventArgs e) {
            _action.MaximumExecutionTime = TimeSpan.FromTicks(TimeLimitValue.Value.Ticks - 
                                                              TimeLimitValue.MinDate.Ticks);
        }
    }
}
