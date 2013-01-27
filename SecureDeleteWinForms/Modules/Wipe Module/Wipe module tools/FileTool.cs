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
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using SecureDelete.WipeObjects;
using SecureDeleteWinForms.Modules;
using DebugUtils.Debugger;
using SecureDelete;

namespace SecureDeleteWinForms.WipeTools {
    public partial class FileTool : UserControl, ITool {
        public FileTool() {
            InitializeComponent();
        }

        #region Properties

        public string ModuleName {
            get { return "FileTool"; }
        }

        public ToolType Type {
            get { return ToolType.File; }
        }

        public int RequiredSize {
            get { return 150; }
        }

        private FileWipeObject _file;
        public FileWipeObject File {
            get { return _file; }
            set { _file = value; }
        }

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set { _options = value; }
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

        public void InitializeTool() {
            if(_file != null) {
                FileTextbox.Text = _file.Path;
                BrowseButton.Enabled = true;

                UpdateMethodInfo();
            }
        }

        private void UpdateMethodInfo() {
            if(_options.MethodManager != null && _options.MethodManager.Methods.Count > 0) {
                MethodChangeButton.Enabled = true;
                WipeMethod m = _options.MethodManager.GetMethod(_file.WipeMethodId == WipeOptions.DefaultWipeMethod ? 
                                                                _options.WipeOptions.DefaultFileMethod : _file.WipeMethodId);
                if(m != null) {
                    MethodNameLabel.Text = m.Name;

                    if(_file.WipeMethodId == WipeOptions.DefaultWipeMethod) {
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

        public void DisposeTool() {
            _file = null;
            FileTextbox.Text = string.Empty;
            MethodChangeButton.Enabled = false;
            SaveButton.Enabled = false;
            BrowseButton.Enabled = false;
        }

        #endregion

        private void CancelButton_Click(object sender, EventArgs e) {
            if(OnClose != null) {
                OnClose(this, null);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e) {
            Debug.AssertNotNull(_parentControl, "ParentControl not set");
            Debug.Assert(_parentControl is WipeModule, "ParentControl is not a WipeModule object");

            WipeModule wipeModule = (WipeModule)_parentControl;

            if(_objectIndex < wipeModule.ObjectList.Items.Count) {
                string path = FileTextbox.Text.Trim();

                if(CheckPathValid(path) == false) {
                    FilePathErrorLabel.Visible = true;
                    FileTextbox.BackColor = Color.FromArgb(255, 120, 120);
                    FileTextbox.Focus();
                    FileTextbox.SelectAll();
                }
                else {
                    _file.Path = path;
                    wipeModule.UpdateObject(_objectIndex, _file);
                    wipeModule.ObjectList.Select();
                    wipeModule.ObjectList.SelectedIndices.Clear();
                    wipeModule.ObjectList.SelectedIndices.Add(_objectIndex);
                }

                SaveButton.Enabled = false;
            }
        }

        private bool CheckPathValid(string path) {
            if(path == null) {
                return false;
            }

            if(path.Length == 0) {
                return false;
            }

            return System.IO.File.Exists(path);
        }

        private void BrowseButton_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Select file";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.ShowReadOnly = true;
            dialog.ValidateNames = true;

            if(dialog.ShowDialog() == DialogResult.OK) {
                FileTextbox.Text = dialog.FileName;
            }
        }

        private void FileTextbox_TextChanged(object sender, EventArgs e) {
            FileTextbox.BackColor = Color.White;
            FilePathErrorLabel.Visible = false;

            SaveButton.Enabled = BrowseButton.Enabled;
        }

        private void MethodChangeButton_Click(object sender, EventArgs e) {
            Debug.AssertNotNull(_options.MethodManager, "MethodManager not set");

            WipeMethods w = new WipeMethods();
            w.Options = _options;
            w.MethodManager = _options.MethodManager;
            w.SelectedMethod = _options.MethodManager.GetMethodIndex(_file.WipeMethodId);
            w.ShowSelected = true;
            w.ShowDialog(this);

            if(w.SelectedMethod < 0 || w.SelectedMethod >= _options.MethodManager.Methods.Count) {
                _file.WipeMethodId = WipeOptions.DefaultWipeMethod;
            }
            else {
                _file.WipeMethodId = _options.MethodManager.Methods[w.SelectedMethod].Id;
            }

            UpdateMethodInfo();
        }

        private void button1_Click(object sender, EventArgs e) {
            _file.WipeMethodId = WipeOptions.DefaultWipeMethod;
            UpdateMethodInfo();
        }
    }
}
