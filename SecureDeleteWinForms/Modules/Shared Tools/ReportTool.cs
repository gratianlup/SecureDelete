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
using DebugUtils.Debugger;
using System.Threading;
using SecureDelete;

namespace SecureDeleteWinForms {
    public partial class ReportTool : UserControl, ITool {
        public enum PanelType {
            Statistics, FailedObjects, Errors
        }

        #region Fields

        private bool showHighSeverity;
        private bool showMediumSeverity;
        private bool showLowSeverity;
        private bool errorListInvalidated;
        private bool failedListInvalidated;
        private bool populateStopped;
        private BackgoundTask populateTask;
        private PanelType activePanel;

        private delegate void PopulateListDelegate(bool async);
        private delegate void InsertErrorsDelegate(List<ListViewItem> items);

        #endregion

        private WipeReport _report;
        public WipeReport Report {
            get { return _report; }
            set {
                _report = value;
                HandleNewReport();
            }
        }

        private void HandleNewReport() {
            showHighSeverity = true;
            showMediumSeverity = true;
            showLowSeverity = true;
            errorListInvalidated = true;
            failedListInvalidated = true;
            SetActivePanel(activePanel);
        }

        public ReportTool() {
            InitializeComponent();
            showHighSeverity = true;
            showMediumSeverity = true;
            showLowSeverity = true;
            errorList = new List<WipeError>();
            activePanel = PanelType.Errors;
        }

        private void SetShowHighSeverity(bool state) {
            StopPopulating();

            showHighSeverity = state;
            errorListInvalidated = true;
            RepopulateErrorList();
        }

        private void SetShowMediumSeverity(bool state) {
            StopPopulating();

            showMediumSeverity = state;
            errorListInvalidated = true;
            RepopulateErrorList();
        }

        private void SetShowLowSeverity(bool state) {
            StopPopulating();

            showLowSeverity = state;
            errorListInvalidated = true;
            RepopulateErrorList();
        }

        private void SetErrorButtonText() {
            int highSeverityCount = 0;
            int mediumSeverityCount = 0;
            int lowSeverityCount = 0;

            if(_report != null && _report.Errors != null && _report.Errors.Count > 0) {
                for(int i = 0; i < _report.Errors.Count; i++) {
                    WipeError error = _report.Errors[i];

                    if(error.Severity == ErrorSeverity.High) {
                        highSeverityCount++;
                    }
                    else if(error.Severity == ErrorSeverity.Medium) {
                        mediumSeverityCount++;
                    }
                    else {
                        lowSeverityCount++;
                    }
                }
            }

            HighSeverityButton.Text = "High (" + highSeverityCount.ToString() + ")";
            MediumSeverityButton.Text = "Medium (" + mediumSeverityCount.ToString() + ")";
            LowSeverityButton.Text = "Low (" + lowSeverityCount.ToString() + ")";
        }


        private void RepopulateListImpl(bool async) {
            List<ListViewItem> items = new List<ListViewItem>(32);
            InsertErrorsDelegate del = new InsertErrorsDelegate(InsertError);

            for(int i = 0; i < _report.Errors.Count; i++) {
                if(async && populateStopped) {
                    break;
                }

                WipeError error = _report.Errors[i];
                int iconIndex = -1;

                if(error.Severity == ErrorSeverity.High && showHighSeverity) {
                    iconIndex = 0;
                }
                else if(error.Severity == ErrorSeverity.Medium && showMediumSeverity) {
                    iconIndex = 1;
                }
                else if(error.Severity == ErrorSeverity.Low && showLowSeverity) {
                    iconIndex = 2;
                }

                if(iconIndex != -1) {
                    ListViewItem item = new ListViewItem();
                    item.Text = ((int)(i + 1)).ToString();
                    item.SubItems.Add(error.Time.ToLongTimeString());
                    item.SubItems.Add(error.Message);
                    item.ImageIndex = iconIndex;

                    if(i % 2 == 1) {
                        item.BackColor = Color.AliceBlue;
                    }

                    if(async) {
                        items.Add(item);

                        if((i >= 32 && i % 32 == 0) || i == _report.Errors.Count - 1) {
                            ErrorListView.Invoke(del, items);
                            Thread.Sleep(30);

                            if(i % 32 == 0) {
                                items.Clear();
                            }
                        }
                    }
                    else {
                        ErrorListView.Items.Add(item);
                    }
                }
            }

            this.Invoke(new EventHandler(PopulateTaskCompleted), null, null);
        }


        private void PopulateTaskCompleted(object sender, EventArgs e) {
            if(populateTask != null) {
                populateTask.Stop();
            }
        }


        private void InsertError(List<ListViewItem> items) {
            for(int i = 0; i < items.Count; i++) {
                ErrorListView.Items.Add(items[i]);
            }

            if(populateTask != null) {
                populateTask.ProgressValue = (int)(((double)ErrorListView.Items.Count / _report.Errors.Count) * 100);
            }
        }


        private void OnPopulateErrorsStopped(object sender, EventArgs e) {
            populateStopped = true;
        }


        private void RepopulateErrorList() {
            if(errorListInvalidated == false) {
                return;
            }

            if(_report == null) {
                return;
            }

            errorList.Clear();
            int count = _report.Errors.Count;
            List<WipeError> errors = _report.Errors;
            string filterValue = SearchTextbox.Text.Trim().ToLower();
            bool useFilter = filterValue.Length > 0;

            for(int i = 0; i < count; i++) {
                WipeError error = errors[i];
                bool allowed = false;

                // filter by type
                if(error.Severity == ErrorSeverity.High && showHighSeverity) {
                    allowed = true;
                }
                else if(error.Severity == ErrorSeverity.Medium && showMediumSeverity) {
                    allowed = true;
                }
                else if(error.Severity == ErrorSeverity.Low && showLowSeverity) {
                    allowed = true;
                }

                // filter by value
                if(allowed && useFilter) {
                    if(error.Message.ToLower().Contains(filterValue) == false) {
                        allowed = false;
                    }
                }

                // add to the list
                if(allowed) {
                    errorList.Add(error);
                }
            }

            ErrorListView.VirtualListSize = errorList.Count;
        }


        private void RepopulateFailedList() {
            if(failedListInvalidated == false) {
                return;
            }

            FailedListView.Items.Clear();

            if(_report != null && _report.FailedObjects != null && _report.FailedObjects.Count > 0) {
                for(int i = 0; i < _report.FailedObjects.Count; i++) {
                    FailedObject failedObject = _report.FailedObjects[i];
                    ListViewItem item = new ListViewItem();
                    item.Text = ((int)(FailedListView.Items.Count + 1)).ToString();
                    item.SubItems.Add(failedObject.Type.ToString());
                    item.SubItems.Add(failedObject.Path);
                    item.Tag = _report.FailedObjects[i];
                    FailedListView.Items.Add(item);
                }
            }

            FailedDetailsPanel.Visible = FailedListView.Items.Count > 0;
        }


        private void CreateStatistics() {
            if(_report == null || _report.Statistics == null) {
                return;
            }

            WipeStatistics statistics = _report.Statistics;
            StringBuilder builder = new StringBuilder();
            builder.Append("Start Time: ");
            builder.AppendLine(statistics.StartTime.ToString());
            builder.Append("End Time: ");
            builder.AppendLine(statistics.EndTime.ToString());
            builder.Append("Duration: ");
            builder.AppendLine(statistics.Duration.ToString());
            builder.AppendLine();
            builder.Append("Wiped Bytes: ");
            builder.AppendLine(statistics.TotalWipedBytes.ToString());
            builder.Append("Wiped Slack Space: ");
            builder.AppendLine(statistics.BytesInClusterTips.ToString());
            builder.Append("Average Write Speed: ");
            builder.AppendLine(statistics.AverageWriteSpeed.ToString());
            builder.AppendLine();
            builder.Append("Failed Objects: ");
            builder.AppendLine(statistics.FailedObjects.ToString());
            builder.Append("Errors: ");
            builder.AppendLine(statistics.Errors.ToString());
            StatisticsBox.Text = builder.ToString();
        }


        public void SetActivePanel(PanelType panel) {
            StopPopulating();

            if(panel == PanelType.Errors) {
                ErrorsButton.Selected = true;
                FailedObjectsButton.Selected = false;
                StatisticsButton.Selected = false;
                ErrorsPanel.Visible = true;
                StatisticsPanel.Visible = false;
                FailedObjectsPanel.Visible = false;

                SetErrorButtonText();
                RepopulateErrorList();
            }
            else if(panel == PanelType.FailedObjects) {
                ErrorsButton.Selected = false;
                FailedObjectsButton.Selected = true;
                StatisticsButton.Selected = false;
                StatisticsPanel.Visible = false;
                ErrorsPanel.Visible = false;
                FailedObjectsPanel.Visible = true;

                RepopulateFailedList();

                // select the first member
                if(FailedListView.Items.Count > 0) {
                    FailedListView.Items[0].Selected = true;
                }

                FailedListView.Select();
            }
            else {
                ErrorsButton.Selected = false;
                FailedObjectsButton.Selected = false;
                StatisticsButton.Selected = true;
                StatisticsPanel.Visible = true;
                ErrorsPanel.Visible = false;
                FailedObjectsPanel.Visible = false;

                CreateStatistics();
            }

            activePanel = panel;
        }

        #region ITool Members

        public string ModuleName {
            get { return "ReportTool"; }
        }

        public ToolType Type {
            get { return ToolType.Report; }
        }

        public int RequiredSize {
            get { return 200; }
        }

        public void InitializeTool() {
            errorListInvalidated = true;
            failedListInvalidated = true;
        }

        public void DisposeTool() {
            _report = null;
            FailedListView.Items.Clear();
            ErrorListView.Items.Clear();
            StatisticsBox.Text = string.Empty;
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

        #endregion

        private void panelSelectControl3_Click(object sender, EventArgs e) {
            SetActivePanel(PanelType.Errors);
        }

        private void FailedObjectsButton_Click(object sender, EventArgs e) {
            SetActivePanel(PanelType.FailedObjects);
        }

        private void ShowAssociatedErrorDetails(WipeError error) {
            SeverityLabel.Text = "Severity: " + error.Severity.ToString();
            TimeLabel.Text = "Time: " + error.Time.ToLongTimeString();
            MessageLabel.Text = "Message: " + error.Message;
            SeverityIcon.Image = SeverityIcons.Images[(int)error.Severity];
        }

        private void FailedListView_SelectedIndexChanged(object sender, EventArgs e) {
            if(FailedListView.SelectedItems.Count <= 0) {
                return;
            }

            // update failed object error
            FailedObject failed = (FailedObject)FailedListView.SelectedItems[0].Tag;

            if(failed.AssociatedError != null) {
                ShowAssociatedErrorDetails(failed.AssociatedError);
            }
        }

        private void StatisticsButton_Click(object sender, EventArgs e) {
            SetActivePanel(PanelType.Statistics);
        }

        private void HighSeverityButton_Click(object sender, EventArgs e) {
            SetShowHighSeverity(HighSeverityButton.Checked);
        }

        private void MediumSeverityButton_Click_1(object sender, EventArgs e) {
            SetShowMediumSeverity(MediumSeverityButton.Checked);
        }

        private void LowSeverityButton_Click_1(object sender, EventArgs e) {
            SetShowLowSeverity(LowSeverityButton.Checked);
        }

        public void StopPopulating() {
            populateStopped = true;
        }

        private List<WipeError> errorList;

        private ListViewItem GetErrorItem(int index) {
            WipeError error = errorList[index];
            int iconIndex = -1;

            if(error.Severity == ErrorSeverity.High) {
                iconIndex = 0;
            }
            else if(error.Severity == ErrorSeverity.Medium) {
                iconIndex = 1;
            }
            else if(error.Severity == ErrorSeverity.Low) {
                iconIndex = 2;
            }

            ListViewItem item = new ListViewItem();
            item.Text = ((int)(index + 1)).ToString();
            item.SubItems.Add(error.Time.ToLongTimeString());
            item.SubItems.Add(error.Message);
            item.ImageIndex = iconIndex;
            item.Tag = error;
            return item;
        }

        private void ErrorListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e) {
            e.Item = GetErrorItem(e.ItemIndex);
        }

        private void ClearButton_Click(object sender, EventArgs e) {
            SearchTextbox.Text = "";
        }

        private void SearchTextbox_TextChanged(object sender, EventArgs e) {
            RepopulateErrorList();

            if(_report.Errors != null && _report.Errors.Count > 0 && 
               SearchTextbox.Text.Length > 0 && errorList.Count == 0) {
                SearchTextbox.BackColor = Color.Tomato;
            }
            else {
                SearchTextbox.BackColor = Color.FromName("Window");
            }
        }

        private void ErrorListView_SelectedIndexChanged(object sender, EventArgs e) {
            if(ErrorListView.SelectedIndices.Count > 0) {
                ErrorDetailsBox.Text = errorList[ErrorListView.SelectedIndices[0]].Message;
            }
        }
    }
}
