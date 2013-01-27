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
using System.IO;
using System.Windows.Forms;
using SecureDelete.WipeObjects;
using SecureDeleteWinForms.Modules;
using DebugUtils.Debugger;
using SecureDelete.FileSearch;
using SecureDelete;
using System.Text.RegularExpressions;

namespace SecureDeleteWinForms.WipeTools {
    public partial class FolderTool : UserControl, ITool {
        public FolderTool() {
            InitializeComponent();
        }

        private int insertCount = 0;
        private WipeModule wipeModule;

        public string ModuleName {
            get { return "FolderTool"; }
        }

        public ToolType Type {
            get { return ToolType.Folder; }
        }

        public int RequiredSize {
            get { return 350; }
        }

        private bool _insertMode;
        public bool InsertMode {
            get { return _insertMode; }
            set { _insertMode = value; }
        }

        private FolderWipeObject _folder;
        public FolderWipeObject Folder {
            get { return _folder; }
            set { _folder = value; }
        }

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set {
                _options = value;
                FilterBox.Options = _options;
            }
        }

        private Control _parentControl;
        public Control ParentControl {
            get { return _parentControl; }
            set { _parentControl = value; }
        }

        private Image _toolIcon;
        public Image ToolIcon {
            get { return _toolIcon; }
            set { _toolIcon = value; }
        }

        private int _objectIndex;
        public int ObjectIndex {
            get { return _objectIndex; }
            set { _objectIndex = value; }
        }

        public event EventHandler OnClose;

        private void UpdateMethodInfo() {
            if(_options.MethodManager != null && _options.MethodManager.Methods.Count > 0) {
                MethodChangeButton.Enabled = true;
                WipeMethod m = _options.MethodManager.GetMethod(_folder.WipeMethodId == WipeOptions.DefaultWipeMethod ? 
                                                                _options.WipeOptions.DefaultFileMethod : _folder.WipeMethodId);

                if(m != null) {
                    MethodNameLabel.Text = m.Name;

                    if(_folder.WipeMethodId == WipeOptions.DefaultWipeMethod) {
                        MethodNameLabel.Text += " (default)";
                    }
                }
                else {
                    MethodNameLabel.Text = "Wipe method not found";
                }
            }
            else {
                MethodNameLabel.Text = "No wipe method available";
            }
        }

        public void InitializeTool() {
            Debug.AssertType(_parentControl, typeof(WipeModule), "ParentControl is not a WipeModule object");
            Debug.AssertNotNull(_folder, "Folder not set");

            wipeModule = _parentControl as WipeModule;

            if(_insertMode == false) {
                InsertButton.Text = "Save changes";

                FolderTextbox.Text = _folder.Path;
                PatternTextbox.Text = _folder.UseMask ? _folder.Mask : string.Empty;
                SubfoldersCheckbox.Checked = _folder.WipeSubfolders;
                DeleteCheckbox.Checked = _folder.DeleteFolders;

                if(_folder.FileFilter != null) {
                    // always work with a copy of the file Filter
                    FilterBox.FileFilter = (FileFilter)_folder.FileFilter.Clone();
                    FilterCheckbox.Checked = _folder.FileFilter.Enabled;
                }
            }
            else {
                InsertButton.Text = "Add Folder";
                insertCount = 0;
            }

            UpdateMethodInfo();
        }
        
        private void CancelButton_Click(object sender, EventArgs e) {
            if(OnClose != null) {
                OnClose(this, null);
            }
        }

        private void FilterCheckbox_CheckedChanged(object sender, EventArgs e) {
            FilterBox.FilterEnabled = FilterCheckbox.Checked;
        }

        private bool FolderExists(string folder) {
            return (folder != null && folder != string.Empty && Directory.Exists(folder));
        }

        private void InsertButton_Click(object sender, EventArgs e) {
            string path = FolderTextbox.Text.Trim();

            if(FolderExists(path)) {
                if(_insertMode == true && insertCount > 0) {
                    FolderWipeObject temp = new FolderWipeObject();
                    temp.WipeMethodId = _folder.WipeMethodId;

                    _folder = temp;
                }

                _folder.Path = path;
                _folder.UseMask = PatternTextbox.Text.Trim().Length > 0;
                _folder.Mask = PatternTextbox.Text.Trim();
                _folder.WipeSubfolders = SubfoldersCheckbox.Checked;
                _folder.DeleteFolders = DeleteCheckbox.Checked;
                _folder.FileFilter = FilterBox.FileFilter;

                if(_insertMode == true) {
                    wipeModule.Session.Items.Add(_folder);
                    wipeModule.InsertObject(_folder);

                    // create a new instance of the file Filter
                    FileFilter temp = (FileFilter)FilterBox.FileFilter.Clone();
                    FilterBox.FileFilter = temp;
                    insertCount++;
                }
                else {
                    wipeModule.UpdateObject(_objectIndex, _folder);
                    wipeModule.ObjectList.Select();
                    wipeModule.ObjectList.SelectedIndices.Clear();
                    wipeModule.ObjectList.SelectedIndices.Add(_objectIndex);
                }
            }
            else {
                FolderTextbox.Focus();
                FolderTextbox.SelectAll();
                ErrorTooltip.Show("Folder path is invalid", FolderTextbox, 3000);
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e) {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select the folder.";
            dialog.ShowNewFolderButton = false;

            if(dialog.ShowDialog() == DialogResult.OK) {
                FolderTextbox.Text = dialog.SelectedPath;
            }
        }

        private void MethodChangeButton_Click(object sender, EventArgs e) {
            Debug.AssertNotNull(_options.MethodManager, "MethodManager not set");

            WipeMethods w = new WipeMethods();
            w.Options = _options;
            w.MethodManager = _options.MethodManager;
            w.SelectedMethod = _options.MethodManager.GetMethodIndex(_folder.WipeMethodId);
            w.ShowSelected = true;
            w.ShowDialog(this);

            if(w.SelectedMethod < 0 || w.SelectedMethod >= _options.MethodManager.Methods.Count) {
                _folder.WipeMethodId = WipeOptions.DefaultWipeMethod;
            }
            else {
                _folder.WipeMethodId = _options.MethodManager.Methods[w.SelectedMethod].Id;
            }

            UpdateMethodInfo();
        }

        private void button1_Click(object sender, EventArgs e) {
            _folder.WipeMethodId = WipeOptions.DefaultWipeMethod;
            UpdateMethodInfo();
        }

        private void PatternTextbox_TextChanged(object sender, EventArgs e) {
            ValidateRegex();
        }

        private void ValidateRegex() {
            if(checkBox1.Checked) {
                // try to compile regex
                try {
                    Regex.IsMatch("abc", PatternTextbox.Text);
                    RegexPicturebox.Visible = false;
                }
                catch {
                    RegexPicturebox.Visible = true;
                }
            }
            else {
                RegexPicturebox.Visible = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            ValidateRegex();
        }


        public void DisposeTool() {
            
        }
    }
}
