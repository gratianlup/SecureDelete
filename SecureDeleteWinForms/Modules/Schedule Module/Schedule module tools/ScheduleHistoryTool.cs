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

namespace SecureDeleteWinForms {
    public partial class ScheduleHistoryTool : UserControl {
        #region Constants

        private const int VisualizerSmallSize = 16;
        private const int VisualizerNormalSize = 24;
        private const int VisualizerLargeSize = 48;

        #endregion

        #region Fields

        private bool expanded;
        private IList<HistoryItem> historyList;

        #endregion

        #region Constructor

        public ScheduleHistoryTool() {
            InitializeComponent();

            Visualizer.NormalColor1 = new SolidBrush(Color.FromArgb(204, 226, 240));
            Visualizer.NormalColor2 = new SolidBrush(Color.FromArgb(168, 212, 240));
            Visualizer.ErrorColor1 = new SolidBrush(Color.Gold);
            Visualizer.ErrorColor2 = new SolidBrush(Color.FromArgb(245, 225, 113));
            Visualizer.FailedColor1 = new SolidBrush(Color.FromArgb(216, 75, 57));
            Visualizer.FailedColor2 = new SolidBrush(Color.FromArgb(199, 57, 36));
            Visualizer.BackgroundColor = Brushes.White;
            Visualizer.VisualWidth = VisualizerNormalSize;
            Visualizer.ShowStartTime = false;
            Visualizer.TextColor = Brushes.Black;
            Visualizer.ShowDurationHistogram = true;
            Visualizer.DurationHistogramColor = Pens.Black;
            Visualizer.PointColor = Brushes.Black;
            Visualizer.PointRadius = 6;
            Visualizer.SelectionColor = Pens.Navy;
            Visualizer.OnHoveredVisualChanged += OnHoverdVisualChanged;
        }

        #endregion

        #region Properties

        private HistoryManager _manager;
        public HistoryManager Manager {
            get { return _manager; }
            set {
                _manager = value;
                LoadHistory();
            }
        }

        private ScheduledTask _task;
        public ScheduledTask Task {
            get { return _task; }
            set {
                _task = value;
                LoadHistory();
            }
        }

        #endregion

        #region Private methods

        private void LoadHistory() {
            if(_manager == null || _task == null) {
                return;
            }

            // get the history items
            historyList = _manager.GetItems(_task.TaskId);
            FilterHistory();
        }

        private void FilterHistory() {
            bool showFailed = FailedButton.Checked;
            bool showWithErrors = ErrorButton.Checked;
            bool showOk = OkButton.Checked;

            // reset
            Visualizer.SuspendUpdate = true;
            Visualizer.Visuals.Clear();
            List.BeginUpdate();
            List.Items.Clear();

            if(historyList != null) {
                foreach(HistoryItem item in historyList) {
                    // filter
                    if((showOk && HistoryItem.IsOk(item)) ||
                       (showWithErrors && HistoryItem.IsWithErrors(item)) ||
                       (showFailed && HistoryItem.IsFailed(item))) {
                        HistoryVisual visual = new HistoryVisual(item);
                        Visualizer.Visuals.Add(visual);

                        // create a list view item
                        ListViewItem listItem = new ListViewItem();
                        listItem.Text = item.StartTime.ToString();
                        listItem.UseItemStyleForSubItems = false;

                        if(HistoryItem.IsOk(item)) {
                            listItem.SubItems.Add("Completed");
                        }
                        else if(HistoryItem.IsWithErrors(item)) {
                            listItem.SubItems.Add("Completed with Errors");
                            listItem.ImageIndex = 0;
                        }
                        else {
                            listItem.SubItems.Add("Failed");
                            listItem.ImageIndex = 1;
                        }

                        TimeSpan? duration = item.DeltaTime;

                        if(duration.HasValue) {
                            listItem.SubItems.Add(duration.Value.ToString());
                        }
                        else {
                            listItem.SubItems.Add("");
                        }

                        if(item.Statistics != null) {
                            listItem.SubItems.Add(item.Statistics.FailedObjects.ToString());
                            listItem.SubItems.Add(item.Statistics.Errors.ToString());
                        }
                        else {
                            listItem.SubItems.Add("");
                            listItem.SubItems.Add("");
                        }

                        if(item.ReportInfo != null) {
                            listItem.SubItems.Add("Yes");
                            listItem.SubItems[5].ForeColor = Color.Blue;
                        }
                        else {
                            listItem.SubItems.Add("No");
                        }

                        List.Items.Add(listItem);
                        listItem.Tag = visual;
                        visual.Tag = listItem;
                    }
                }

                Visualizer.SuspendUpdate = false;
                List.EndUpdate();
                ItemLabel.Text = "Items: " + List.Items.Count;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            Visualizer.ShowDurationHistogram = HistogramButton.Checked;
        }

        private void OnHoverdVisualChanged(object sender, HistoryVisual visual) {
            toolTip1.Hide(Visualizer);

            if(visual != null) {
                toolTip1.Show(visual.History.StartTime.ToString(), Visualizer);
            }
        }

        private void SizeValue_SelectedIndexChanged(object sender, EventArgs e) {
            if(SizeValue.SelectedIndex == 0) {
                Visualizer.VisualWidth = VisualizerSmallSize;
            }
            else if(SizeValue.SelectedIndex == 1) {
                Visualizer.VisualWidth = VisualizerNormalSize;
            }
            else {
                Visualizer.VisualWidth = VisualizerLargeSize;
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e) {
            if(Visualizer.ScrollbarHeight > 0 && expanded == false) {
                expanded = true;
                splitContainer1.SplitterDistance += Visualizer.ScrollbarHeight;
            }
        }

        private void DetailsButton_Click(object sender, EventArgs e) {
            splitContainer1.Panel1Collapsed = !DetailsButton.Checked;
            HistogramButton.Visible = DetailsButton.Checked;
            SizeLabel.Visible = DetailsButton.Checked;
            SizeValue.Visible = DetailsButton.Checked;
        }

        #endregion

        private void FailedButton_Click(object sender, EventArgs e) {
            FilterHistory();
        }

        private void ErrorButton_Click(object sender, EventArgs e) {
            FilterHistory();
        }

        private void OkButton_Click(object sender, EventArgs e) {
            FilterHistory();
        }

        private void Visualizer_OnSelectionChanged(object sender, HistoryVisual visual) {
            if(visual.Tag != null) {
                ListViewItem item = (ListViewItem)visual.Tag;
                item.Selected = true;
                item.EnsureVisible();
            }
        }

        private void List_SelectedIndexChanged(object sender, EventArgs e) {
            if(List.SelectedItems.Count > 0) {
                if(List.SelectedItems[0].Tag != null) {
                    Visualizer.SelectVisual((HistoryVisual)List.SelectedItems[0].Tag);
                }
            }
        }

        private void toolStripButton1_Click_2(object sender, EventArgs e) {
            LoadHistory();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            if(_task != null && _manager != null) {
                _manager.RemoveCategory(_task.TaskId);
                LoadHistory();
            }
        }
    }
}
