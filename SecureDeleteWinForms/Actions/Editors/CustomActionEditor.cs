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
using SecureDelete.Schedule;
using SecureDelete.Actions;

namespace SecureDeleteWinForms {
    public partial class CustomActionEditor : UserControl, IActionEditor {
        public CustomActionEditor() {
            InitializeComponent();
        }

        #region IActionEditor Members

        public ActionEditorType Type {
            get { return ActionEditorType.CustomActionEditor; }
        }

        public int RequiredSize {
            get { return 260; }
        }

        private CustomAction _action;
        public IAction Action {
            get { return _action; }
            set {
                if((value is CustomAction) == false) {
                    throw new Exception("Invalid value");
                }

                _action = (CustomAction)value;
                HandleNewAction();
            }
        }

        public string TypeString {
            get {
                if(_action == null) {
                    return string.Empty;
                }

                if(_action is ShutdownAction) {
                    return "Shutdown";
                }
                else if(_action is RestartAction) {
                    return "Restart";
                }
                else if(_action is LogoffAction) {
                    return "Logoff";
                }
                else {
                    return "Custom Action";
                }
            }
        }

        public string ActionString {
            get {
                if(_action == null) {
                    return string.Empty;
                }

                string actionString = "";
                if(_action.File.Contains(" ")) {
                    actionString = "\"" + _action.File + "\"";
                }
                else {
                    actionString = _action.File;
                }

                if(_action.Arguments != null) {
                    actionString += " " + _action.Arguments;
                }

                return actionString;
            }
        }

        public event EventHandler OnStatusChanged;

        #endregion

        private bool IsLockedAction(IAction action) {
            return (action is ShutdownAction) ||
                   (action is RestartAction) ||
                   (action is LogoffAction);
        }

        private void SetEnabledState(bool state, bool checkboxState) {
            EnabledCheckbox.Enabled = checkboxState;
            PathTextbox.Enabled = state && !IsLockedAction(_action);
            ArgumentsTextbox.Enabled = state && !IsLockedAction(_action);
            DirectoryTextbox.Enabled = state && !IsLockedAction(_action);
            BrowseButton.Enabled = state && !IsLockedAction(_action);
            BrowseButton2.Enabled = state && !IsLockedAction(_action);
            VariablesToolbar.Enabled = state && !IsLockedAction(_action);
            VariablesList.Enabled = state && !IsLockedAction(_action);
            TimeLimitCheckbox.Enabled = state && !IsLockedAction(_action);
            TimeLimitValue.Enabled = state && TimeLimitCheckbox.Checked && !IsLockedAction(_action);
            label1.Enabled = state;
            label2.Enabled = state;
            label3.Enabled = state;
            label4.Enabled = state;
        }

        private void HandleNewAction() {
            if(IsLockedAction(_action)) {
                PathTextbox.Text = ArgumentsTextbox.Text = DirectoryTextbox.Text = "";
                EnabledCheckbox.Checked = _action.Enabled;
                SetEnabledState(_action.Enabled, true);
            }
            else {
                EnabledCheckbox.Checked = _action.Enabled;
                PathTextbox.Text = _action.File;

                if(_action.Arguments != null) {
                    ArgumentsTextbox.Text = _action.Arguments;
                }
                if(_action.StartupDirectory != null) {
                    DirectoryTextbox.Text = _action.StartupDirectory;
                }

                SetEnabledState(_action.Enabled, true);
                TimeLimitCheckbox.Checked = _action.HasMaximumExecutionTime;

                if(_action.HasMaximumExecutionTime && _action.MaximumExecutionTime == null) {
                    _action.MaximumExecutionTime = new TimeSpan(TimeLimitValue.MinDate.Ticks);
                }

                TimeLimitValue.Value = new DateTime(TimeLimitValue.MinDate.Ticks + 
                                                    _action.MaximumExecutionTime.Ticks);
                LoadVariables();
            }
        }

        private void SendStatusChanged() {
            if(OnStatusChanged != null) {
                OnStatusChanged(this, null);
            }
        }

        private void PathTextbox_TextChanged(object sender, EventArgs e) {
            _action.File = PathTextbox.Text;
            SendStatusChanged();
        }

        private void ArgumentsTextbox_TextChanged(object sender, EventArgs e) {
            _action.Arguments = ArgumentsTextbox.Text;
            SendStatusChanged();
        }

        private void EnabledCheckbox_CheckedChanged(object sender, EventArgs e) {
            _action.Enabled = EnabledCheckbox.Checked;
            SetEnabledState(_action.Enabled, true);
            SendStatusChanged();
        }

        private void DirectoryTextbox_TextChanged(object sender, EventArgs e) {
            _action.StartupDirectory = DirectoryTextbox.Text;
            SendStatusChanged();
        }

        private void TimeLimitCheckbox_CheckedChanged(object sender, EventArgs e) {
            TimeLimitValue.Enabled = TimeLimitCheckbox.Checked;
            _action.HasMaximumExecutionTime = TimeLimitCheckbox.Checked;
        }

        private void TimeLimitValue_ValueChanged(object sender, EventArgs e) {
            _action.MaximumExecutionTime = TimeSpan.FromTicks(TimeLimitValue.Value.Ticks - 
                                                              TimeLimitValue.MinDate.Ticks);
        }

        private void BrowseButton_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.ShowReadOnly = true;
            dialog.ValidateNames = true;

            if(dialog.ShowDialog() == DialogResult.OK) {
                PathTextbox.Text = dialog.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select the folder.";
            dialog.ShowNewFolderButton = false;

            if(dialog.ShowDialog() == DialogResult.OK) {
                DirectoryTextbox.Text = dialog.SelectedPath;
            }
        }

        #region Variables

        private class VariableEditorHelper {
            public ListViewItem Item;
            public int Column;

            public VariableEditorHelper(ListViewItem item, int column) {
                Item = item;
                Column = column;
            }
        }

        private void LoadVariables() {
            VariablesList.Items.Clear();

            if(_action.Variables != null) {
                for(int i = 0; i < _action.Variables.Count; i++) {
                    LoadVariable(_action.Variables[i]);
                }

                UpdateVariableInfo();
            }
        }

        private void LoadVariable(EnvironmentVariable variable) {
            ListViewItem item = new ListViewItem();
            item.Text = variable.Name;
            item.SubItems.Add(variable.Value);
            item.ImageIndex = 0;
            item.Tag = variable;
            VariablesList.Items.Add(item);
        }

        private void AddVariable() {
            if(_action.Variables == null) {
                _action.Variables = new List<EnvironmentVariable>();
            }

            EnvironmentVariable variable = new EnvironmentVariable();
            _action.Variables.Add(variable);
            LoadVariable(variable);

            if(VariablesList.Items.Count > 0) {
                VariablesList.Items[VariablesList.Items.Count - 1].BeginEdit();
            }

            UpdateVariableInfo();
        }

        private void RemoveSelected() {
            while(VariablesList.SelectedIndices.Count > 0) {
                EnvironmentVariable variable = (EnvironmentVariable)VariablesList.Items[VariablesList.SelectedIndices[0]].Tag;

                // remove from action
                if(_action.Variables.Contains(variable)) {
                    _action.Variables.Remove(variable);
                }

                VariablesList.Items.RemoveAt(VariablesList.SelectedIndices[0]);
            }

            UpdateVariableInfo();
        }

        private void RemoveAll() {
            foreach(ListViewItem item in VariablesList.Items) {
                EnvironmentVariable variable = (EnvironmentVariable)item.Tag;

                // remove from action
                if(_action.Variables.Contains(variable)) {
                    _action.Variables.Remove(variable);
                }
            }

            VariablesList.Items.Clear();
            UpdateVariableInfo();
        }

        private void UpdateVariableInfo() {
            VariablesLabel.Text = "Variables: " + VariablesList.Items.Count.ToString();
            RemoveButton.Enabled = VariablesList.Items.Count > 0;
        }

        private bool VariableInList(string name) {
            if(_action == null || _action.Variables == null) {
                return false;
            }

            foreach(EnvironmentVariable variable in _action.Variables) {
                if(variable.Name == name) {
                    return true;
                }
            }

            return false;
        }

        private void AddButton_Click(object sender, EventArgs e) {
            AddVariable();
        }

        #endregion



        private void VariablesList_MouseUp(object sender, MouseEventArgs e) {
            // Get the member on the row that is clicked.
            VariablesList.FullRowSelect = true;
            ListViewItem item = VariablesList.GetItemAt(e.X, e.Y);

            // Make sure that an member is clicked.
            if(item != null) {
                // Get the bounds of the member that is clicked.
                Rectangle clickedItem = item.Bounds;
                int delta = VariablesList.Columns[0].Width;
                int column = 0;

                if(item.GetSubItemAt(e.X, e.Y) == item.SubItems[1]) {
                    column = 1;
                }

                if(column == 0) {
                    VariableEditor.Visible = false;
                    VariablesList.FullRowSelect = false;
                    return;
                }

                // Assign calculated bounds to the ComboBox.
                VariableEditor.Left = item.Bounds.Left + delta + VariablesList.Left + VariablesPanel.Left + 1;
                VariableEditor.Top = item.Bounds.Top + VariablesList.Top + VariablesPanel.Top;
                VariableEditor.Width = VariablesList.Columns[column].Width;
                VariableEditor.Height = item.Bounds.Height;

                // Set default text for ComboBox to match the member that is clicked
                VariableEditor.Text = ((EnvironmentVariable)item.Tag).Value;
                VariableEditor.Tag = new VariableEditorHelper(item, column);

                // Display the ComboBox, and make sure that it is on top with focus.
                VariableEditor.Visible = true;
                VariableEditor.BringToFront();
                VariableEditor.Select();
                VariableEditor.Focus();
                VariablesList.FullRowSelect = false;
            }
            else {
                VariablesList.FullRowSelect = false;
            }
        }

        private void VariableEditor_TextChanged(object sender, EventArgs e) {
            if(VariableEditor.Tag == null) {
                return;
            }

            VariableEditorHelper helper = (VariableEditorHelper)VariableEditor.Tag;
            EnvironmentVariable variable = (EnvironmentVariable)helper.Item.Tag;
            variable.Value = VariableEditor.Text;
            helper.Item.SubItems[helper.Column].Text = VariableEditor.Text;
        }

        private void VariableEditor_Leave(object sender, EventArgs e) {
            VariableEditor.Visible = false;
            VariableEditor.Tag = null;
            VariablesList.SelectedIndices.Clear();
            VariablesList.FullRowSelect = false;
        }

        private void VariableEditor_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape) {
                VariableEditor_Leave(null, null);
            }
        }

        private void VariablesList_AfterLabelEdit(object sender, LabelEditEventArgs e) {
            if(e.Label == null || e.Label.Trim().Length == 0) {
                Rectangle bounds = VariablesList.Items[e.Item].GetBounds(ItemBoundsPortion.Label);
                ErrorTooltip.Show("Invalid value.", VariablesList, bounds.Left, bounds.Top + bounds.Height + 2, 3000);
                e.CancelEdit = true;
                VariablesList.Items[e.Item].BeginEdit();
            }
            else {
                if(VariableInList(e.Label)) {
                    Rectangle bounds = VariablesList.Items[e.Item].GetBounds(ItemBoundsPortion.Label);
                    ErrorTooltip.Show("A variable with this name already exists in the list.", 
                                      VariablesList, bounds.Left, bounds.Top + bounds.Height + 2, 3000);
                    e.CancelEdit = true;
                    VariablesList.Items[e.Item].BeginEdit();
                }
                else {
                    ((EnvironmentVariable)VariablesList.Items[e.Item].Tag).Name = e.Label;
                }
            }
        }

        private void RemoveButton_ButtonClick(object sender, EventArgs e) {
            RemoveSelected();
        }

        private void removeSelectedToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveSelected();
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveAll();
        }
    }
}
