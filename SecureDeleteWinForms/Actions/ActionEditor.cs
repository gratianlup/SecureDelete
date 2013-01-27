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
using SecureDelete.Schedule;
using SecureDelete.Actions;

namespace SecureDeleteWinForms {
    public partial class ActionEditor : UserControl {
        public ActionEditor() {
            InitializeComponent();
            LoadEditors();
            HideEditorHost();
        }

        #region Fields

        private IAction activeAction;
        private ListViewItem activeItem;

        #endregion

        #region Properties

        private List<IAction> _actions;
        public List<IAction> Actions {
            get { return _actions; }
            set {
                _actions = value;
                PopulateList();
            }
        }

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set { _options = value; }
        }

        #endregion

        private void PopulateList() {
            if(_actions == null) {
                return;
            }

            ActionList.Items.Clear();
            for(int i = 0; i < _actions.Count; i++) {
                ListViewItem item = CreateListItem(_actions[i]);
                ActionList.SelectedIndices.Clear();
                ActionList.SelectedIndices.Add(ActionList.Items.Count - 1);
                UpdateListviewItem(item);
            }

            // select first member
            if(ActionList.Items.Count > 0) {
                ActionList.SelectedIndices.Clear();
                ActionList.Items[0].Selected = true;
                ActionList.Focus();
            }

            LoadTemplates();
        }


        private ListViewItem CreateListItem(IAction action) {
            ListViewItem item = new ListViewItem();
            item.SubItems.AddRange(new string[] { "", "" });
            item.Tag = action;
            ActionList.Items.Add(item);

            return item;
        }

        private ListViewItem GetItemByAction(IAction action) {
            int count = ActionList.Items.Count;
            for(int i = 0; i < count; i++) {
                if(ActionList.Items[i].Tag == action) {
                    return ActionList.Items[i];
                }
            }

            // not found
            return null;
        }

        private void UpdateListviewItem(ListViewItem item) {
            IAction action = (IAction)item.Tag;
            UpdateAction(action, item);
        }

        private void UpdateAction(IAction action, ListViewItem item) {
            if(action == null || item == null || activeEditor == null) {
                return;
            }

            item.Text = activeEditor.TypeString;
            item.SubItems[1].Text = activeEditor.ActionString;

            // enabled state
            item.SubItems[2].Text = action.Enabled ? "True" : "False";
            item.ForeColor = action.Enabled ? Color.FromName("WindowText") : Color.FromName("GrayText");
        }

        private bool IsCustomAction(IAction action) {
            return (action is CustomAction);
        }

        private void UpdateActionPanel(IAction action) {
            if(action is CustomAction) {
                LoadCustomActionEditor(action);
            }
            else if(action is MailAction) {
                LoadMailActionEditor(action);
            }
            else if(action is PowershellAction) {
                LoadPowerShellActionEditor(action);
            }
        }

        private void LoadCustomActionEditor(IAction action) {
            CustomActionEditor editor = (CustomActionEditor)GetEditor(ActionEditorType.CustomActionEditor);
            SetEditorHostSize(editor);
            editor.Action = action;
        }

        private void LoadMailActionEditor(IAction action) {
            MailActionEditor editor = (MailActionEditor)GetEditor(ActionEditorType.MailActionEditor);
            SetEditorHostSize(editor);
            editor.Action = action;
        }

        private void LoadPowerShellActionEditor(IAction action) {
            PowershellActionEditor editor = (PowershellActionEditor)GetEditor(ActionEditorType.PowershellActionEditor);
            SetEditorHostSize(editor);
            editor.Action = action;
        }

        private void AddAction(IAction action) {
            _actions.Add(action);
            action.Enabled = true;
            ListViewItem item = CreateListItem(action);
            UpdateListviewItem(item);

            ActionList.SelectedIndices.Clear();
            ActionList.SelectedIndices.Add(ActionList.Items.Count - 1);
            UpdateListviewItem(ActionList.SelectedItems[0]);
            UpdateActionPanel((IAction)ActionList.SelectedItems[0].Tag);
            HandleActionNumberAndSelection();
        }

        private void RemoveAction(ListViewItem action) {
            _actions.Remove((IAction)action.Tag);
            ActionList.Items.Remove(action);
            HandleActionNumberAndSelection();
        }

        private void RemoveSelectedActions() {
            while(ActionList.SelectedItems.Count > 0) {
                RemoveAction(ActionList.SelectedItems[0]);
            }

            HandleActionNumberAndSelection();
        }

        private void RemoveDisabledActions() {
            for(int i = 0; i < ActionList.Items.Count; i++) {
                if(((IAction)ActionList.Items[i].Tag).Enabled == false) {
                    RemoveAction(ActionList.Items[i]);
                    i--;
                }
            }

            HandleActionNumberAndSelection();
        }

        private void RemoveAllActions() {
            while(ActionList.Items.Count > 0) {
                RemoveAction(ActionList.Items[0]);
            }

            HandleActionNumberAndSelection();
        }


        private void HandleActionNumberAndSelection() {
            ActionCountLabel.Text = "Actions: " + _actions.Count.ToString();
            RemoveButton.Enabled = _actions.Count > 0;

            if(ActionList.SelectedIndices.Count > 0) {
                int selectedIndex = ActionList.SelectedIndices[0];
                UpButton.Enabled = RemoveButton.Enabled && selectedIndex != 0;
                DownButton.Enabled = RemoveButton.Enabled && selectedIndex != _actions.Count - 1;
            }
            else {
                UpButton.Enabled = false;
                DownButton.Enabled = false;
            }

            SaveTemplateButton.Enabled = ActionList.Items.Count > 0;
            RemoveTemplateButton.Enabled = TemplateList.Items.Count > 0;
            LoadTemplateButton.Enabled = TemplateList.SelectedItem != null;

            if(ActionList.Items.Count == 0) {
                HideEditorHost();
            }
        }

        private void ActionList_SelectedIndexChanged(object sender, EventArgs e) {
            if(ActionList.SelectedIndices.Count > 0) {
                activeItem = ActionList.SelectedItems[0];
                activeAction = (IAction)activeItem.Tag;
                UpdateActionPanel(activeAction);
                HandleActionNumberAndSelection();
            }
        }

        private void MoveActionUp() {
            if(ActionList.SelectedIndices.Count == 0) {
                return;
            }

            int index = ActionList.SelectedIndices[0];

            if(index == 0) {
                return;
            }

            IAction temp = _actions[index];
            _actions.RemoveAt(index);
            _actions.Insert(index - 1, temp);

            ListViewItem item = ActionList.Items[index];
            ActionList.Items.RemoveAt(index);
            ActionList.Items.Insert(index - 1, item);
        }

        private void MoveActionDown() {
            if(ActionList.SelectedIndices.Count == 0) {
                return;
            }

            int index = ActionList.SelectedIndices[0];

            if(index == ActionList.Items.Count - 1) {
                return;
            }

            IAction temp = _actions[index];
            _actions.RemoveAt(index);
            _actions.Insert(index + 1, temp);

            ListViewItem item = ActionList.Items[index];
            ActionList.Items.RemoveAt(index);
            ActionList.Items.Insert(index + 1, item);
        }

        #region Templates

        private void LoadTemplates() {
            TemplateList.Items.Clear();

            foreach(ActionStoreItem item in _options.ActionStore.Items) {
                TemplateList.Items.Add(item.Name);
            }

            HandleActionNumberAndSelection();
        }

        private void SaveTemplate() {
            TemplateName form = new TemplateName();
            form.Text = "Save Action Template";

            if(form.ShowDialog() == DialogResult.OK) {
                ActionStoreItem item = new ActionStoreItem();
                item.Name = form.Name.Text;

                // save the actions
                if(ActionList.SelectedItems.Count > 0) {
                    foreach(ListViewItem lvi in ActionList.SelectedItems) {
                        item.Actions.Add((IAction)lvi.Tag);
                    }
                }
                else {
                    // save all
                    foreach(ListViewItem lvi in ActionList.Items) {
                        item.Actions.Add((IAction)lvi.Tag);
                    }
                }

                _options.ActionStore.Add(item);
                SDOptionsFile.TrySaveOptions(_options);
                LoadTemplates();
            }
        }

        private void LoadSelectedTemplate() {
            if(TemplateList.SelectedIndex >= 0 && 
               TemplateList.SelectedIndex < _options.FilterStore.Items.Count) {
                ActionStoreItem item = _options.ActionStore.Items[TemplateList.SelectedIndex];

                foreach(IAction action in item.Actions) {
                    AddAction((IAction)action.Clone());
                }

                HandleActionNumberAndSelection();
            }
        }

        private void RemoveAllTemplates() {
            TemplateList.Items.Clear();
            _options.ActionStore.Items.Clear();
            HandleActionNumberAndSelection();
        }

        private void RemoveSelectedTemplate() {
            if(TemplateList.SelectedIndex >= 0 && TemplateList.SelectedIndex < _options.FilterStore.Items.Count) {
                _options.ActionStore.Items.RemoveAt(TemplateList.SelectedIndex);
                TemplateList.Items.RemoveAt(TemplateList.SelectedIndex);
            }

            HandleActionNumberAndSelection();
        }

        #endregion

        private void TemplateList_SelectedIndexChanged(object sender, EventArgs e) {
            HandleActionNumberAndSelection();
        }

        private List<IActionEditor> editors;
        private IActionEditor activeEditor;

        private void LoadEditors() {
            if(editors == null) {
                editors = new List<IActionEditor>();
            }

            editors.Add(new CustomActionEditor());
            editors.Add(new MailActionEditor());
            editors.Add(new PowershellActionEditor());

            foreach(IActionEditor editor in editors) {
                editor.OnStatusChanged += OnActionStatusChanged;
            }
        }

        private IActionEditor GetEditor(ActionEditorType type) {
            foreach(IActionEditor editor in editors) {
                if(editor.Type == type) {
                    EditorHost.Controls.Clear();
                    EditorHost.Controls.Add((UserControl)editor);
                    ((UserControl)editor).Dock = DockStyle.Fill;
                    activeEditor = editor;
                    return editor;
                }
            }

            return null;
        }

        private void SetEditorHostSize(IActionEditor editor) {
            ShowEditorHost();
            int availableSize = splitContainer1.Height - splitContainer1.SplitterDistance;

            if(editor.RequiredSize > availableSize) {
                splitContainer1.SplitterDistance = Math.Max(50, splitContainer1.Height - 
                                                            editor.RequiredSize - ToolHeader.Height);
            }
        }

        private void ShowEditorHost() {
            splitContainer1.Panel2Collapsed = false;
        }

        private void HideEditorHost() {
            splitContainer1.Panel2Collapsed = true;
        }

        private void OnActionStatusChanged(object sender, EventArgs e) {
            IActionEditor editor = (IActionEditor)sender;
            UpdateAction(editor.Action, GetItemByAction(editor.Action));
        }
        
        private void sendMailToolStripMenuItem_Click(object sender, EventArgs e) {
            AddAction(new MailAction());
        }

        private void customActionToolStripMenuItem_Click_2(object sender, EventArgs e) {
            AddAction(new CustomAction());
        }

        private void shutdownToolStripMenuItem_Click_2(object sender, EventArgs e) {
            AddAction(new ShutdownAction());
        }

        private void UpButton_Click_1(object sender, EventArgs e) {
            MoveActionUp();
        }

        private void restartToolStripMenuItem_Click_2(object sender, EventArgs e) {
            AddAction(new RestartAction());
        }

        private void logoffToolStripMenuItem_Click_2(object sender, EventArgs e) {
            AddAction(new LogoffAction());
        }

        private void sendMailToolStripMenuItem_Click_1(object sender, EventArgs e) {
            AddAction(new MailAction());
        }

        private void RemoveButton_ButtonClick(object sender, EventArgs e) {
            RemoveSelectedActions();
        }

        private void removeDisabledActionsToolStripMenuItem_Click_1(object sender, EventArgs e) {
            RemoveDisabledActions();
        }

        private void removeAllToolStripMenuItem_Click_1(object sender, EventArgs e) {
            RemoveAllActions();
        }

        private void removeSelectedToolStripMenuItem_Click_1(object sender, EventArgs e) {
            RemoveSelectedActions();
        }

        private void DownButton_Click_1(object sender, EventArgs e) {
            MoveActionDown();
        }

        private void LoadTemplateButton_Click_1(object sender, EventArgs e) {
            LoadSelectedTemplate();
        }

        private void deleteSelectedToolStripMenuItem_Click_1(object sender, EventArgs e) {
            RemoveSelectedTemplate();
        }

        private void deleteAllTemplatesToolStripMenuItem_Click_1(object sender, EventArgs e) {
            RemoveAllTemplates();
        }

        private void SaveTemplateButton_Click(object sender, EventArgs e) {
            SaveTemplate();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e) {
            AddAction(new CustomAction());
        }

        private void powershellScriptToolStripMenuItem_Click(object sender, EventArgs e) {
            AddAction(new PowershellAction());
        }

        private void TemplateList_SelectedIndexChanged_1(object sender, EventArgs e) {
            HandleActionNumberAndSelection();
        }
    }
}
