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
using SecureDeleteWinForms.Modules;
using SecureDelete;
using SecureDelete.FileSearch;

namespace SecureDeleteWinForms {
    public partial class ReportSearcher : UserControl, ITool {
        public ReportSearcher() {
            InitializeComponent();
        }

        #region Fields

        private ReportModule reportModule;

        #endregion

        #region ITool Members

        public string ModuleName {
            get { return "ReportSearcher"; }
        }

        public ToolType Type {
            get { return ToolType.ReportSearcher; }
        }

        public int RequiredSize {
            get { return 240; }
        }

        public void InitializeTool() {
            reportModule = (ReportModule)_parentControl;
        }

        public void DisposeTool() {

        }

        private Image _toolIcon;
        public Image ToolIcon {
            get { return _toolIcon; }
            set { _toolIcon = value; }
        }

        public event EventHandler OnClose;

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

        private WipeReportManager _reportManager;
        public WipeReportManager ReportManager {
            get { return _reportManager; }
            set { _reportManager = value; }
        }

        #endregion

        #region Filter

        private bool filterText;
        private string text;
        private bool searchFailed;
        private bool searchErrors;
        private bool matchCase;

        private bool filterDate;
        private DateImplication dateImplication;
        private DateTime dateValue;

        private bool filterFailed;
        private SizeImplication failedImplication;
        private int failedValue;

        private bool filterError;
        private SizeImplication errorImplication;
        private int errorValue;

        private bool FilterReport(ReportInfo info) {
            if(info == null) {
                throw new ArgumentNullException();
            }

            bool result = true;

            // filter by date
            if(filterDate) {
                switch(dateImplication) {
                    case DateImplication.From: {
                        result &= info.CreatedDate.Day == dateValue.Day && info.CreatedDate.Month == dateValue.Month &&
                                    info.CreatedDate.Year == dateValue.Year;
                        break;
                    }
                    case DateImplication.NewerOrFrom: {
                        result &= info.CreatedDate >= dateValue;
                        break;
                    }
                    case DateImplication.OlderOrFrom: {
                        result &= info.CreatedDate <= dateValue;
                        break;
                    }
                }
            }

            // filter failed number
            if(filterFailed) {
                switch(failedImplication) {
                    case SizeImplication.Equals: {
                        result &= info.FailedObjectCount == failedValue;
                        break;
                    }
                    case SizeImplication.GreaterThan: {
                        result &= info.FailedObjectCount >= failedValue;
                        break;
                    }
                    case SizeImplication.LessThan: {
                        result &= info.FailedObjectCount <= failedValue;
                        break;
                    }
                }
            }

            // filter error number
            if(filterError) {
                switch(errorImplication) {
                    case SizeImplication.Equals: {
                        result &= info.ErrorCount == errorValue;
                        break;
                    }
                    case SizeImplication.GreaterThan: {
                        result &= info.ErrorCount >= errorValue;
                        break;
                    }
                    case SizeImplication.LessThan: {
                        result &= info.ErrorCount < errorValue;
                        break;
                    }
                }
            }

            if(filterText && result != false) {
                // load the report
                WipeReport report = _reportManager.LoadReport(info);

                if(report != null) {
                    // search failed objects first
                    bool found = false;
                    int count = 0;

                    if(searchFailed) {
                        count = report.FailedObjects.Count;
                        for(int i = 0; i < count && !found; i++) {
                            if(report.FailedObjects[i].Path.IndexOf(text, matchCase ? StringComparison.InvariantCulture :
                                                                    StringComparison.InvariantCultureIgnoreCase) != -1) {
                                found = true;
                            }
                        }
                    }

                    if(searchErrors) {
                        count = report.Errors.Count;
                        for(int i = 0; i < count && !found; i++) {
                            if(report.Errors[i].Message.IndexOf(text, matchCase ? StringComparison.InvariantCulture :
                                                                StringComparison.InvariantCultureIgnoreCase) != -1) {
                                found = true;
                            }
                        }
                    }

                    result &= found;
                }
            }

            return result;
        }

        #endregion

        #region Search

        private void SearchImpl() {
            // set options
            text = SearchTextbox.Text.Trim();
            matchCase = MatchCaseCheckbox.Checked;
            searchFailed = SearchFailedCheckbox.Checked;
            searchErrors = SearchErrorsCheckbox.Checked;

            filterText = text != "";
            filterDate = DateCheckbox.Checked;
            filterFailed = FailedCheckbox.Checked;
            filterError = ErrorCheckbox.Checked;

            if(filterDate) {
                switch(DateCombobox.SelectedIndex) {
                    case 0: {
                        dateImplication = DateImplication.OlderOrFrom;
                        break;
                    }
                    case 1: {
                        dateImplication = DateImplication.From;
                        break;
                    }
                    case 2: {
                        dateImplication = DateImplication.NewerOrFrom;
                        break;
                    }
                }

                dateValue = DateValue.Value;
            }

            if(filterFailed) {
                switch(FailedCombobox.SelectedIndex) {
                    case 0: {
                        failedImplication = SizeImplication.LessThan;
                        break;
                    }
                    case 1: {
                        failedImplication = SizeImplication.Equals;
                        break;
                    }
                    case 2: {
                        failedImplication = SizeImplication.GreaterThan;
                        break;
                    }
                }

                failedValue = (int)FailedNumber.Value;
            }

            if(filterError) {
                switch(ErrorCombobox.SelectedIndex) {
                    case 0: {
                        errorImplication = SizeImplication.LessThan;
                        break;
                    }
                    case 1: {
                        errorImplication = SizeImplication.Equals;
                        break;
                    }
                    case 2: {
                        errorImplication = SizeImplication.GreaterThan;
                        break;
                    }
                }

                errorValue = (int)ErrorNumber.Value;
            }

            reportModule.ReportList.Items.Clear();

            // filter all reports
            foreach(KeyValuePair<Guid, ReportCategory> category in _reportManager.Categories) {
                foreach(KeyValuePair<long, ReportInfo> info in category.Value.Reports) {
                    if(FilterReport(info.Value)) {
                        // add to the list
                        reportModule.AddReport(info.Value);
                    }
                }
            }
        }

        #endregion

        private void SearchButton_Click(object sender, EventArgs e) {
            SearchImpl();
        }

        private void button1_Click(object sender, EventArgs e) {
            DateCheckbox.Checked = true;
            DateCombobox.SelectedIndex = 1;
            DateValue.Value = DateTime.Now;
        }

        private void DateCheckbox_CheckedChanged(object sender, EventArgs e) {
            DateCombobox.Enabled = DateCheckbox.Checked;
            DateValue.Enabled = DateCheckbox.Checked;
        }

        private void FailedCheckbox_CheckedChanged(object sender, EventArgs e) {
            FailedCombobox.Enabled = FailedCheckbox.Checked;
            FailedNumber.Enabled = FailedCheckbox.Checked;
        }

        private void ErrorCheckbox_CheckedChanged(object sender, EventArgs e) {
            ErrorCombobox.Enabled = ErrorCheckbox.Checked;
            ErrorNumber.Enabled = ErrorCheckbox.Checked;
        }

        private void button2_Click(object sender, EventArgs e) {
            SearchTextbox.Text = "";
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            if(OnClose != null) {
                OnClose(this, null);
            }
        }

        private void ReportSearcher_Load(object sender, EventArgs e) {
            DateCombobox.SelectedIndex = 1;
            FailedCombobox.SelectedIndex = 1;
            ErrorCombobox.SelectedIndex = 1;
        }
    }
}
