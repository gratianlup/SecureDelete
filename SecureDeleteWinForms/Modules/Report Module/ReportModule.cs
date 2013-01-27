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
using System.IO;
using System.Resources;
using System.Collections;
using SecureDelete;

namespace SecureDeleteWinForms.Modules {
    public partial class ReportModule : UserControl, IModule {
        public static string ReportModuleName = "ReportModule";

        public ReportModule() {
            InitializeComponent();
            LoadTools();
            InitializeActions();
        }

        #region IModule Members

        public static Image ModuleImage {
            get {
                return SecureDeleteWinForms.Properties.Resources.report;
            }
        }

        public static Dictionary<string, Image> ActionImages {
            get {
                Dictionary<string, Image> images = new Dictionary<string, Image>();

                images.Add("View", SecureDeleteWinForms.Properties.Resources.pen);
                images.Add("Remove", SecureDeleteWinForms.Properties.Resources.delete_profile);
                images.Add("Search", SecureDeleteWinForms.Properties.Resources.Project3);

                return images;
            }
        }

        private void InitializeActions() {
            _actionManager = new ModuleActionManager();

            _actionManager.Actions.Add("View", new ModuleAction(ActionType.PanelAction, "View", "View",
                                                                delegate() { return ViewButton.Enabled; },
                                                                toolStripButton2_Click));

            _actionManager.Actions.Add("RemoveSelected", new ModuleAction(ActionType.PanelAction, "Remove Selected", "Remove",
                                                                delegate() { return removeToolStripMenuItem.Enabled; },
                                                                removeToolStripMenuItem_Click));

            _actionManager.Actions.Add("RemoveAll", new ModuleAction(ActionType.PanelAction, "Remove All", "Remove",
                                                                delegate() { return removeAllToolStripMenuItem.Enabled; },
                                                                removeAllToolStripMenuItem_Click));

            _actionManager.Actions.Add("Search", new ModuleAction(ActionType.PanelAction, "Search", "Search",
                                                                delegate() { return SearchButton.Enabled; },
                                                                toolStripButton1_Click));
        }

        public string ModuleName {
            get { return ReportModuleName; }
        }

        private ModuleActionManager _actionManager;
        public ModuleActionManager ActionManager {
            get { return _actionManager; }
            set { _actionManager = value; }
        }

        public MenuStrip Menu {
            get {
                return MenuStrip;
            }
        }

        private Control _parentControl;
        public System.Windows.Forms.Control ParentControl {
            get { return _parentControl; }
            set { _parentControl = value; }
        }

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set { _options = value; }
        }

        public event ModuleStatusDelegate OnStatusChanged;
        public event MenuActionDelegeate OnMenuAction;

        public void Activated() {
            HideToolPanel();
            LoadReports();
            UpdateStatus();
        }

        private void SendMenuAction(MenuAction action) {
            if(OnMenuAction != null) {
                OnMenuAction(this, action);
            }
        }


        #endregion

        #region Tool management

        private List<ITool> tools;
        private ITool activeTool;
        private bool toolPanelClosed;

        private ReportTool reportViewer;

        private void LoadTools() {
            if(tools == null) {
                tools = new List<ITool>();
            }
            else {
                tools.Clear();
            }

            tools.Add(new ReportTool());
            tools.Add(new ReportSearcher());

            foreach(ITool tool in tools) {
                tool.ParentControl = this;
                tool.Options = _options;
                tool.OnClose += OnToolClosed;
            }
        }

        public ITool ChangeTool(ToolType type, bool setSize) {
            if(activeTool != null && activeTool.Type == type) {
                ShowToolPanel();

                if(setSize) {
                    SetToolHostSize(activeTool.RequiredSize);
                }

                return activeTool;
            }

            activeTool = null;

            for(int i = 0; i < tools.Count; i++) {
                if(tools[i].Type == type) {
                    activeTool = tools[i];
                    break;
                }
            }

            if(activeTool != null) {
                ToolHost.Controls.Clear();
                ToolHost.Controls.Add((UserControl)activeTool);
                ((UserControl)activeTool).Dock = DockStyle.Fill;

                ShowToolPanel();

                if(setSize) {
                    SetToolHostSize(activeTool.RequiredSize);
                }
            }

            return activeTool;
        }

        private void OnToolClosed(object sender, EventArgs e) {
            HideToolPanel();
        }

        private void SetToolHostSize(int size) {
            if(ToolHost.Height < size) {
                splitContainer2.SplitterDistance = Math.Max(100, splitContainer2.Height - size - ToolHeader.Height);
            }
        }

        public void ChangeToolHeaderText(string text) {
            ToolHeaderLabel.Text = text;
        }

        public void ChangeToolHeaderIcon(Image icon) {
            ToolHeaderIcon.Image = icon;
        }

        private void ToolCloseButton_Click(object sender, EventArgs e) {
            HideToolPanel();
        }

        private void HideToolPanel() {
            splitContainer2.Panel2Collapsed = true;
            toolPanelClosed = true;
        }

        private void ShowToolPanel() {
            splitContainer2.Panel2Collapsed = false;
            toolPanelClosed = false;
        }

        #endregion

        #region Report management

        #region Constants

        private Color ReportNotFoundColor = Color.LightCoral;

        #endregion

        #region Fields

        private WipeReportManager reportManager;

        #endregion

        private void UpdateStatus() {
            if(OnStatusChanged != null) {
                OnStatusChanged(this, string.Format("Report Number: {0}", ReportList.Items.Count));
            }
        }

        private void LoadReports() {
            // check if the file exists
            string path = SecureDeleteLocations.GetReportFilePath();

            if(File.Exists(path) == false) {
                Debug.ReportWarning("Report file not found at {0}", path);
                return;
            }

            reportManager = new WipeReportManager();
            reportManager.ReportDirectory = SecureDeleteLocations.GetReportDirectory();

            if(reportManager.LoadReportCategories(path) == false) {
                Debug.ReportWarning("Failed to load the report database from {0}", path);
                return;
            }

            LoadCategories();
        }


        private void SaveReports() {
            string path = SecureDeleteLocations.CombinePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                                            SecureDeleteLocations.SecureDeleteFolder, SecureDeleteLocations.SecureDeleteReportFolder,
                                                            SecureDeleteLocations.SecureDeleteReportFile);
            if(reportManager.SaveReportCategories(path) == false) {
                Debug.ReportWarning("Failed to save report database to file {0}", path);
            }
        }


        private void LoadCategories() {
            ReportCategories.Nodes.Clear();
            TreeNode scheduleNode = new TreeNode("Scheduled Tasks");
            scheduleNode.ImageIndex = -1;
            scheduleNode.SelectedImageIndex = -1;
            int ct = 1;

            foreach(KeyValuePair<Guid, ReportCategory> kvp in reportManager.Categories) {
                TreeNode childNode;

                if(kvp.Key == WipeSession.DefaultSessionGuid) {
                    childNode = new TreeNode("SecureDelete");
                    ReportCategories.Nodes.Add(childNode);
                }
                else {
                    // check if the session exists
                    if(_options.SessionNames.ContainsKey(kvp.Key)) {
                        childNode = new TreeNode(_options.SessionNames[kvp.Key]);
                        scheduleNode.Nodes.Add(childNode);
                        childNode.ImageIndex = 1;
                        childNode.SelectedImageIndex = 1;
                    }
                    else {
                        childNode = new TreeNode(string.Format("Not Found #{0}", ct));
                        scheduleNode.Nodes.Add(childNode);
                        childNode.ImageIndex = 2;
                        childNode.SelectedImageIndex = 2;
                        ct++;
                    }
                }

                childNode.Tag = kvp.Key;
            }

            // add the scheduled tasks reports in the view
            ReportCategories.Nodes.Add(scheduleNode);
            scheduleNode.Expand();

            // select default _session category
            if(ReportCategories.Nodes.Count > 0 && ReportCategories.Nodes[0].Text == "SecureDelete") {
                ReportCategories.SelectedNode = ReportCategories.Nodes[0];
            }
        }


        private void LoadReportList(Guid id) {
            if(reportViewer != null) {
                reportViewer.StopPopulating();
            }

            ReportList.Items.Clear();
            ReportInfo[] reports = reportManager.GetReports(id);

            if(reports == null || reports.Length == 0) {
                return;
            }

            for(int i = 0; i < reports.Length; i++) {
                AddReport(reports[i]);
            }

            UpdateStatus();

            // select first report
            if(ReportList.Items.Count > 0) {
                ReportList.Select();
                ReportList.Focus();
                ReportList.Items[0].Selected = true;
            }
        }

        public void AddReport(ReportInfo info) {
            ListViewItem item = new ListViewItem();
            item.Text = info.CreatedDate.ToString();
            item.SubItems.Add(info.FailedObjectCount.ToString());
            item.SubItems.Add(info.ErrorCount.ToString());
            item.Tag = info;

            if(info.ErrorCount > 0) {
                item.ImageIndex = 3;
            }
            else {
                item.ImageIndex = 2;
            }

            if(reportManager.ReportExists(info) == false) {
                item.BackColor = ReportNotFoundColor;
            }

            ReportList.Items.Add(item);
        }

        private void LoadReport(ReportInfo info) {
            // show the report tool
            HideSearch();
            reportViewer = (ReportTool)ChangeTool(ToolType.Report, true);
            ChangeToolHeaderText("Report Details");
            ChangeToolHeaderIcon(SecureDeleteWinForms.Properties.Resources.file);

            reportViewer.StopPopulating();
            WipeReport report = reportManager.LoadReport(info);

            if(report != null) {
                reportViewer.Report = report;
            }
        }


        private void RemoveSelectedReports() {
            while(ReportList.SelectedIndices.Count > 0) {
                reportManager.RemoveReport((ReportInfo)ReportList.Items[ReportList.SelectedIndices[0]].Tag);
                ReportList.Items.RemoveAt(ReportList.SelectedIndices[0]);
            }

            SaveReports();
            UpdateStatus();
        }

        private void RemoveUnavailableReports() {
            for(int i = 0; i < ReportList.Items.Count; i++) {
                ReportInfo report = (ReportInfo)ReportList.Items[i].Tag;

                if(reportManager.ReportExists(report) == false) {
                    // remove it
                    reportManager.RemoveReport(report);
                    ReportList.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        private void RemoveAllReports() {
            for(int i = 0; i < ReportList.Items.Count; i++) {
                reportManager.RemoveReport((ReportInfo)ReportList.Items[i].Tag);
            }

            ReportList.Items.Clear();
            SaveReports();
            UpdateStatus();
        }

        private void RemoveSelectedCategories() {
            if(ReportCategories.SelectedNode != null &&
               ReportCategories.SelectedNode.Tag is Guid) {
                reportManager.RemoveCategory((Guid)ReportCategories.SelectedNode.Tag);
                ReportCategories.Nodes.Remove(ReportCategories.SelectedNode);
                SaveReports();
            }

            UpdateStatus();
        }

        private void RemoveCategories(TreeNode parent) {
            for(int i = 0; i < parent.Nodes.Count; i++) {
                if(parent.Nodes[i].Nodes != null && 
                   parent.Nodes[i].Nodes.Count > 0) {
                    RemoveCategories(parent.Nodes[i]);
                }

                if(parent.Nodes[i].Tag is Guid) {
                    Guid id = (Guid)parent.Nodes[i].Tag;
                    reportManager.RemoveCategory(id);

                    if(id != WipeSession.DefaultSessionGuid) {
                        parent.Nodes.RemoveAt(i);
                        i--;
                    }
                }
            }

            UpdateStatus();
        }


        private void RemoveAllCategories() {
            for(int i = 0; i < ReportCategories.Nodes.Count; i++) {
                if(ReportCategories.Nodes[i].Nodes != null && 
                   ReportCategories.Nodes[i].Nodes.Count > 0) {
                    RemoveCategories(ReportCategories.Nodes[i]);
                }

                if(ReportCategories.Nodes[i].Tag is Guid) {
                    Guid id = (Guid)ReportCategories.Nodes[i].Tag;
                    reportManager.RemoveCategory(id);

                    if(id != WipeSession.DefaultSessionGuid) {
                        ReportCategories.Nodes.RemoveAt(i);
                        i--;
                    }
                }
            }

            UpdateStatus();
        }

        #endregion

        #region Export

        private void ExportAsText() {
            if(ReportList.SelectedItems.Count <= 0) {
                return;
            }

            CommonDialog dialog = new CommonDialog();
            dialog.Filter.Add(new FilterEntry("Text file", "*.txt"));
            dialog.Title = "Export Report";

            if(dialog.ShowSave()) {
                WipeReport report = reportManager.LoadReport((ReportInfo)ReportList.SelectedItems[0].Tag);

                if(report == null) {
                    return;
                }

                ReportExporter.ExportAsText(report, dialog.FileName);
            }
        }

        private void ExportAsXml() {
            if(ReportList.SelectedItems.Count <= 0) {
                return;
            }

            CommonDialog dialog = new CommonDialog();
            dialog.Filter.Add(new FilterEntry("XML file", "*.xml"));
            dialog.Title = "Export Report";

            if(dialog.ShowSave()) {
                WipeReport report = reportManager.LoadReport((ReportInfo)ReportList.SelectedItems[0].Tag);
                if(report == null) {
                    return;
                }

                ReportExporter.ExportAsXml(report, dialog.FileName);
            }
        }

        private void ExportAsHTML() {
            if(ReportList.SelectedItems.Count <= 0) {
                return;
            }

            CommonDialog dialog = new CommonDialog();
            dialog.Filter.Add(new FilterEntry("HTML file", "*.htm"));
            dialog.Title = "Export Report";

            if(dialog.ShowSave()) {
                WipeReport report = reportManager.LoadReport((ReportInfo)ReportList.SelectedItems[0].Tag);

                if(report == null) {
                    return;
                }

                // set style
                string style = "";

                if(_options.CustomReportStyle) {
                    try {
                        style = File.ReadAllText(_options.CustomStyleLocation);
                    }
                    catch(Exception ex) {
                        Debug.ReportError("Style could not be loaded. Exception: {0}", ex.Message);
                        MessageBox.Show("Style file could not be found.", "SecureDelete",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else {
                    style = WebReportStyle.HTMLReportStyle;
                }

                ReportExporter.ExportAsHtml(report, dialog.FileName, style);
            }
        }

        #endregion

        private void ReportCategories_AfterSelect(object sender, TreeViewEventArgs e) {
            if(e.Node.Tag != null && e.Node.Tag is Guid) {
                LoadReportList((Guid)e.Node.Tag);
            }
        }

        private void ReportList_SelectedIndexChanged(object sender, EventArgs e) {
            if(ReportList.SelectedItems.Count > 0) {
                if(activeTool == null || activeTool.Type != ToolType.ReportSearcher) {
                    LoadReport((ReportInfo)ReportList.SelectedItems[0].Tag);
                }
            }

            removeToolStripMenuItem.Enabled = ReportList.SelectedItems.Count > 0;
            removeSelectedToolStripMenuItem2.Enabled = ReportList.SelectedItems.Count > 0;
            ViewButton.Enabled = ReportList.SelectedItems.Count > 0;
            ViewMenu.Enabled = ReportList.SelectedItems.Count > 0;
            ExportButton.Enabled = ReportList.SelectedItems.Count > 0;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveSelectedReports();
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveAllReports();
        }

        private void RemoveButton_ButtonClick(object sender, EventArgs e) {
            RemoveSelectedReports();
        }

        private void ReportList_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Delete) {
                RemoveSelectedReports();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            RemoveSelectedCategories();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e) {
            RemoveSelectedCategories();
        }


        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            RemoveAllCategories();
        }

        private void xMLToolStripMenuItem_Click(object sender, EventArgs e) {
            ExportAsXml();
        }

        private void textFileToolStripMenuItem_Click(object sender, EventArgs e) {
            ExportAsText();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            SendMenuAction(MenuAction.About);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {
            SendMenuAction(MenuAction.Options);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            SendMenuAction(MenuAction.ExitApplication);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach(ListViewItem item in ReportList.Items) {
                item.Selected = true;
            }
        }

        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            ReportList.SelectedIndices.Clear();
        }

        private void removeSelectedToolStripMenuItem2_Click(object sender, EventArgs e) {
            RemoveSelectedReports();
        }

        private void removeAllToolStripMenuItem3_Click(object sender, EventArgs e) {
            RemoveAllReports();
        }

        private void removeSelectedToolStripMenuItem1_Click(object sender, EventArgs e) {
            RemoveSelectedCategories();
        }

        private void removeAllToolStripMenuItem2_Click(object sender, EventArgs e) {
            RemoveAllCategories();
        }

        private void removeUnavailableToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void xMLToolStripMenuItem1_Click(object sender, EventArgs e) {
            ExportAsXml();
        }

        private void textFileToolStripMenuItem1_Click(object sender, EventArgs e) {
            ExportAsText();
        }

        private void hTMLToolStripMenuItem_Click_1(object sender, EventArgs e) {
            ExportAsHTML();
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            if(SearchButton.Checked) {
                HideSearch();
            }
            else {
                ShowSearch();
            }
        }

        private void ShowSearch() {
            activeTool = ChangeTool(ToolType.ReportSearcher, true);
            ReportSearcher searcher = (ReportSearcher)activeTool;
            ChangeToolHeaderText("Search Reports");
            ChangeToolHeaderIcon(SearchButton.Image);

            SearchButton.Checked = true;
            SearchMenu.Checked = true;
            searcher.ReportManager = reportManager;
            searcher.InitializeTool();
        }

        private void HideSearch() {
            if(activeTool != null && activeTool.Type == ToolType.ReportSearcher) {
                HideToolPanel();
            }

            SearchButton.Checked = false;
            SearchMenu.Checked = false;
        }

        private void ReportList_DoubleClick(object sender, EventArgs e) {
            if(ReportList.SelectedItems.Count > 0) {
                LoadReport((ReportInfo)ReportList.SelectedItems[0].Tag);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            if(ReportList.SelectedItems.Count > 0) {
                LoadReport((ReportInfo)ReportList.SelectedItems[0].Tag);
            }
        }

        private void ToolCloseButton_Click_1(object sender, EventArgs e) {
            HideToolPanel();
        }

        private void searchReportsToolStripMenuItem_Click(object sender, EventArgs e) {
            if(SearchButton.Checked) {
                HideSearch();
            }
            else {
                ShowSearch();
            }
        }

        private void optionsToolStripMenuItem_Click_1(object sender, EventArgs e) {
            SendMenuAction(MenuAction.Options);
        }

        private void aboutToolStripMenuItem_Click_1(object sender, EventArgs e) {
            SendMenuAction(MenuAction.About);
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e) {
            SendMenuAction(MenuAction.ExitApplication);
        }

        private void viewReportToolStripMenuItem_Click(object sender, EventArgs e) {
            if(ReportList.SelectedItems.Count > 0) {
                LoadReport((ReportInfo)ReportList.SelectedItems[0].Tag);
            }
        }

        #region Sorting

        private void SetSortOrder(ReportSortOrder oldOrder, out ReportSortOrder newOrder) {
            if(oldOrder == ReportSortOrder.Ascending) {
                newOrder = ReportSortOrder.Descending;
            }
            else if(oldOrder == ReportSortOrder.Descending) {
                newOrder = ReportSortOrder.None;
            }
            else {
                newOrder = ReportSortOrder.Ascending;
            }
        }

        private void SetSortImage(ColumnHeader column, ReportSortOrder order) {
            if(order == ReportSortOrder.Ascending) {
                column.ImageKey = "up.png";
            }
            else if(order == ReportSortOrder.Descending) {
                column.ImageKey = "down.png";
            }
            else {
                HideSortImage(column);
            }

            ReportList.Refresh();
        }

        private void HideSortImage(ColumnHeader column) {
            column.ImageIndex = -1;
            column.ImageKey = string.Empty;
        }

        private void SortByColumn(ColumnHeader column, int index, ReportSortElement element) {
            ReportListComparer comparer = ReportList.ListViewItemSorter as ReportListComparer;
            ReportSortOrder sortOrder = ReportSortOrder.Ascending;

            if(comparer != null) {
                if(comparer.Element == element) {
                    SetSortOrder(comparer.SortOrder, out sortOrder);
                }
                else {
                    HideSortImage(comparer.Column);
                }
            }

            SetSortImage(column, sortOrder);

            if(sortOrder == ReportSortOrder.None) {
                ReportList.ListViewItemSorter = null;
            }
            else {
                ReportList.ListViewItemSorter = new ReportListComparer(element, sortOrder, index, column);
            }
        }

        private void ReportList_ColumnClick(object sender, ColumnClickEventArgs e) {
            ColumnHeader column = ReportList.Columns[e.Column];

            switch(e.Column) {
                case 0: {
                    SortByColumn(column, e.Column, ReportSortElement.CreatedDate);
                    break;
                }
                case 1: {
                    SortByColumn(column, e.Column, ReportSortElement.FailedNumber);
                    break;
                }
                case 2: {
                    SortByColumn(column, e.Column, ReportSortElement.ErrorNumber);
                    break;
                }
            }
        }

        #endregion
    }

    enum ReportSortElement {
        CreatedDate, FailedNumber, ErrorNumber
    }

    enum ReportSortOrder {
        Ascending, Descending, None
    }

    class ReportListComparer : IComparer {
        #region Properties

        private ReportSortElement _element;
        internal ReportSortElement Element {
            get { return _element; }
            set { _element = value; }
        }

        private ReportSortOrder _sortOrder;
        internal ReportSortOrder SortOrder {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        private int _subitemIndex;
        public int SubitemIndex {
            get { return _subitemIndex; }
            set { _subitemIndex = value; }
        }

        private ColumnHeader _column;
        public ColumnHeader Column {
            get { return _column; }
            set { _column = value; }
        }

        #endregion

        #region Constructor

        public ReportListComparer(ReportSortElement element, ReportSortOrder sortOrder, int subitemIndex, ColumnHeader column) {
            _element = element;
            _sortOrder = sortOrder;
            _subitemIndex = subitemIndex;
            _column = column;
        }

        #endregion

        #region IComparer Members

        public int Compare(object x, object y) {
            ListViewItem a = (ListViewItem)x;
            ListViewItem b = (ListViewItem)y;

            switch(_element) {
                case ReportSortElement.CreatedDate: {
                    if((a.Tag is ReportInfo) == false) {
                        return 0;
                    }
                    if((b.Tag is ReportInfo) == false) {
                        return 0;
                    }

                    ReportInfo ra = a.Tag as ReportInfo;
                    ReportInfo rb = b.Tag as ReportInfo;
                    return (_sortOrder == ReportSortOrder.Descending ? -1 : 1) *
                            DateTime.Compare(ra.CreatedDate, rb.CreatedDate);
                }
                case ReportSortElement.FailedNumber: {
                    if((a.Tag is ReportInfo) == false) {
                        return 0;
                    }
                    if((b.Tag is ReportInfo) == false) {
                        return 0;
                    }

                    ReportInfo ra = a.Tag as ReportInfo;
                    ReportInfo rb = b.Tag as ReportInfo;
                    return (_sortOrder == ReportSortOrder.Descending ? -1 : 1) * 
                            (ra.FailedObjectCount - rb.FailedObjectCount);
                }
                case ReportSortElement.ErrorNumber: {
                    if((a.Tag is ReportInfo) == false) {
                        return 0;
                    }
                    if((b.Tag is ReportInfo) == false) {
                        return 0;
                    }

                    ReportInfo ra = a.Tag as ReportInfo;
                    ReportInfo rb = b.Tag as ReportInfo;
                    return (_sortOrder == ReportSortOrder.Descending ? -1 : 1) * 
                            (ra.ErrorCount - rb.ErrorCount);
                }
            }

            return 0;
        }

        #endregion
    }
}
