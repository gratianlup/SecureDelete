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
using System.Windows.Forms;
using SecureDelete.FileSearch;
using SecureDeleteWinForms.Modules;
using DebugUtils.Debugger;
using System.Threading;
using SecureDelete;
using System.Text.RegularExpressions;

namespace SecureDeleteWinForms.WipeTools {
    public partial class SearchTool : UserControl, ITool {
        #region Fields

        private FileSearcher searcher;
        WipeModule wipeModule;
        BackgoundTask searchTask;

        #endregion

        public SearchTool() {
            InitializeComponent();
            searcher = new FileSearcher();
            FilterBox.FileFilter = new FileFilter();
        }


        public string ModuleName {
            get { return "FileTool"; }
        }

        public ToolType Type {
            get { return ToolType.Search; }
        }

        public int RequiredSize {
            get { return 350; }
        }

        private Control _parentControl;
        public Control ParentControl {
            get { return _parentControl; }
            set { _parentControl = value; }
        }

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set {
                _options = value;
                FilterBox.Options = _options;
            }
        }

        private bool _searching;
        public bool Searching {
            get { return _searching; }
            set { _searching = value; }
        }

        private Image _toolIcon;
        public Image ToolIcon {
            get { return _toolIcon; }
            set { _toolIcon = value; }
        }

        public event EventHandler OnClose;

        public void InitializeTool() {
            Debug.Assert(_parentControl is WipeModule, "ParentControl is not a WipeModule object");

            wipeModule = _parentControl as WipeModule;
            FilterBox.Options = _options;
            FilterBox.Initialize();
        }

        public void DisposeTool() {
            if(_searching) {
                searcher.Stop();
            }
        }

        private void OnFilesFound(object sender, FileSearchArgs e) {
            Thread.Sleep(50);
            wipeModule.Invoke(new FileSearchDelegate(OnFilesFoundDispatcher), null, e);
        }

        private void OnFilesFoundDispatcher(object sender, FileSearchArgs e) {
            if(e.files != null && e.files.Length > 0) {
                int count = e.files.Length;

                wipeModule.ObjectList.BeginUpdate();
                for(int i = 0; i < count; i++) {
                    wipeModule.AddFile(e.files[i]);
                }
                wipeModule.ObjectList.EndUpdate();
            }

            if(_searching == false) {
                e.stop = true;
            }

            if(e.lastSet) {
                StopSearch();
            }

            this.Update();
        }

        private MainForm GetMainForm(Control c) {
            if(c.Parent is MainForm) {
                return (MainForm)c.Parent;
            }
            else if(c.Parent != null) {
                return GetMainForm(c.Parent);
            }

            return null;
        }

        private void StartSearch() {
            _searching = true;

            // filters
            if(FilterCheckbox.Checked == true) {
                searcher.FileFilter = FilterBox.FileFilter;
            }
            else {
                searcher.FileFilter = null;
            }

            //parentModule.ObjectList.SuspendLayout();
            //parentModule.ObjectList.BeginUpdate();
            searcher.ResultChunkLength = 16;
            searcher.MaximumWaitTime = TimeSpan.FromSeconds(2);
            searcher.OnFilesFound += OnFilesFound;
            searcher.SearchFilesAsync(FolderTextbox.Text, (PatternTextbox.Text.Trim() == string.Empty ? 
                                      null : PatternTextbox.Text.Trim()), RegexCheckbox.Checked, 
                                      SubfoldersCheckbox.Checked);

            FolderTextbox.Enabled = false;
            PatternTextbox.Enabled = false;
            BrowseButton.Enabled = false;
            SearchButton.Text = "Stop";

            // register the task notifier
            MainForm form = GetMainForm(this);

            if(form != null) {
                searchTask = form.RegisterBackgroundTask("Search Status", _toolIcon, ProgressBarStyle.Marquee, 0);
                searchTask.OnStopped += OnSearchStopped;
            }
        }

        private void OnSearchStopped(object sender, EventArgs e) {
            StopSearch();
            searchTask = null;
        }

        private void StopSearch() {
            if(searcher.Stopped == false) {
                searcher.Stop();
            }

            _searching = false;
            SearchButton.Text = "Search";
            FolderTextbox.Enabled = true;
            PatternTextbox.Enabled = true;
            BrowseButton.Enabled = true;
            searcher.OnFilesFound -= OnFilesFound;

            if(searchTask != null && searchTask.Stopped == false) {
                searchTask.Stop();
            }
        }

        private void SearchButton_Click(object sender, EventArgs e) {
            if(_searching == false) {
                StartSearch();
            }
            else {
                StopSearch();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            if(OnClose != null) {
                OnClose(this, null);
            }
        }

        private void FilterCheckbox_CheckedChanged(object sender, EventArgs e) {
            FilterBox.FilterEnabled = FilterCheckbox.Checked;
        }

        private void BrowseButton_Click(object sender, EventArgs e) {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select the folder.";
            dialog.ShowNewFolderButton = false;

            if(dialog.ShowDialog() == DialogResult.OK) {
                FolderTextbox.Text = dialog.SelectedPath;
            }
        }

        private void ValidateRegex() {
            if(RegexCheckbox.Checked) {
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

        private void PatternTextbox_TextChanged(object sender, EventArgs e) {
            ValidateRegex();
        }

        private void RegexCheckbox_CheckedChanged(object sender, EventArgs e) {
            ValidateRegex();
        }

    }
}
