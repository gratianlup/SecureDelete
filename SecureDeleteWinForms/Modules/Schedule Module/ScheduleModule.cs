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
using SecureDelete;
using SecureDelete.Schedule;

namespace SecureDeleteWinForms.Modules {
    public partial class ScheduleModule : UserControl, IModule {
        public const string ScheduleModuleName = "ScheduleModule";

        public ScheduleModule() {
            InitializeComponent();

            InitializeActions();
            manager = new TaskManager();
            manager.OnTaskStatusChanged += TaskStatusChanged;

            // add controllers
            manager.TaskControllers.Add(new PowerTaskController(manager, this));
            HideDetailsPanel();
            LoadTasks();
        }

        #region Fields

        private ListViewItem activeTask;
        private TaskManager manager;
        private bool disableRescheduling;

        #endregion

        #region IModule Members

        public static Image ModuleImage {
            get {
                return SecureDeleteWinForms.Properties.Resources.TaskHS;
            }
        }

        public static Dictionary<string, Image> ActionImages {
            get {
                Dictionary<string, Image> images = new Dictionary<string, Image>();

                images.Add("NewTask", SecureDeleteWinForms.Properties.Resources.add_profile);
                images.Add("RemoveTask", SecureDeleteWinForms.Properties.Resources.delete_profile);
                images.Add("EditTask", SecureDeleteWinForms.Properties.Resources.pen);
                images.Add("StartTask", SecureDeleteWinForms.Properties.Resources.dfsd);
                images.Add("StopTask", SecureDeleteWinForms.Properties.Resources.safdgsd);
                images.Add("ImportTask", SecureDeleteWinForms.Properties.Resources.import);
                images.Add("ExportTask", SecureDeleteWinForms.Properties.Resources.export_profile3);

                return images;
            }
        }

        public string ModuleName {
            get { return ScheduleModuleName; }
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
            WipeItems.Options = _options;
            WipeItems.OnListSaved += ListSaved;
        }

        private void InitializeActions() {
            _actionManager = new ModuleActionManager();

            _actionManager.Actions.Add("New", new ModuleAction(ActionType.PanelAction, "NewTask", "NewTask",
                                                               delegate() { return toolStripButton1.Enabled; },
                                                               toolStripButton1_Click));

            _actionManager.Actions.Add("Remove", new ModuleAction(ActionType.PanelAction, "RemoveTask", "RemoveTask",
                                                                 delegate() { return RemoveButton.Enabled; },
                                                                 RemoveButton_Click));

            _actionManager.Actions.Add("Edit", new ModuleAction(ActionType.PanelAction, "EditTask", "EditTask",
                                                                delegate() { return EditButton.Enabled; },
                                                                toolStripButton2_Click));

            _actionManager.Actions.Add("Start", new ModuleAction(ActionType.PanelAction, "Start", "StartTask",
                                                                 delegate() { return StartButton.Enabled; },
                                                                 StartButton_Click));

            _actionManager.Actions.Add("Stop", new ModuleAction(ActionType.PanelAction, "StopButton", "StopTask",
                                                                delegate() { return toolStripButton1.Enabled; },
                                                                StopButton_Click));

            _actionManager.Actions.Add("Import", new ModuleAction(ActionType.PanelAction, "Import", "ImportTask",
                                                                  delegate() { return ImportButton.Enabled; },
                                                                  toolStripButton3_Click));

            _actionManager.Actions.Add("Export", new ModuleAction(ActionType.PanelAction, "Export", "ExportTask",
                                                                  delegate() { return ImportButton.Enabled; },
                                                                  ExportButton_Click));
        }


        public ITool ChangeTool(ToolType type, bool setSize) {
            return null;
        }

        #endregion

        private void ListSaved(object sender, EventArgs e) {
            if(activeTask != null) {
                ScheduledTask task = (ScheduledTask)activeTask.Tag;
                SessionSaver.SaveSession(WipeItems.Session, SecureDeleteLocations.GetSessionFile(task.TaskId));
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            AddTask();
        }

        private void LoadTasks() {
            manager.LoadTasks();

            // clear old tasks
            TaskList.Items.Clear();

            foreach(ScheduledTask task in manager.TaskList) {
                UpdateTaskItem(CreateTaskItem(task));
            }
        }

        private void AddTask() {
            WipeSession session = new WipeSession();
            session.GenerateGuid();
            ScheduledTask task = new ScheduledTask();
            task.TaskId = session.SessionId;
            task.Schedule = new OneTimeSchedule();
            task.Name = "Untitled task";
            task.Enabled = true;

            // show the dialog
            ScheduleOptions options = new ScheduleOptions();
            options.Options = _options;
            options.EditMode = false;
            options.Task = task;

            if(options.ShowDialog() == DialogResult.OK) {
                task = options.Task;
                manager.LoadOptions();
                manager.AddTask(task, true);

                SessionSaver.SaveSession(session, SecureDeleteLocations.GetSessionFile(task.TaskId));
                ListViewItem item = CreateTaskItem(task);
                UpdateTaskItem(item);

                // add to the <guid,taskName> mapping
                _options.SessionNames.Add(task.TaskId, task.Name);
                SDOptionsFile.TrySaveOptions(_options);
                TaskList.Items[TaskList.Items.Count - 1].Selected = true;
            }

            _actionManager.StateChanged();
        }

        private ListViewItem CreateTaskItem(ScheduledTask task) {
            ListViewItem item = new ListViewItem();
            item.SubItems.AddRange(new string[] { "", "" });
            item.Tag = task;
            TaskList.Items.Add(item);
            return item;
        }

        private void UpdateTaskItem(ListViewItem item) {
            ScheduledTask task = (ScheduledTask)item.Tag;
            item.Text = task.Name;
            string timeValue = "";

            if(task.Schedule.IsTimed) {
                DateTime? next = task.Schedule.GetScheduleTime();

                if(next.HasValue) {
                    timeValue = next.Value.ToString();
                }
                else {
                    timeValue = "Expired";
                }
            }

            // status
            switch(task.Status) {
                case TaskStatus.Wiping: {
                    // get status
                    WipeStatus status = task.CurrentWipeStatus;
                    item.SubItems[1].Text = "";

                    if(status != null) {
                        // compute percent
                        double percent = Math.Min(100, ((double)status.BytesWiped / (double)status.BytesToWipe) * 100.0);
                        item.SubItems[2].Text = "Wiping (" + string.Format("{0:F2}", percent) + ")";
                    }
                    else {
                        item.SubItems[2].Text = "Wiping";
                    }

                    break;
                }
                case TaskStatus.InitializingWiping: {
                    item.SubItems[1].Text = "";
                    item.SubItems[2].Text = "Wiping (initializing)";
                    HandleTaskStarted(task);
                    break;
                }
                case TaskStatus.Paused: {
                    item.SubItems[1].Text = "";
                    item.SubItems[2].Text = "Wiping (paused)";
                    break;
                }
                case TaskStatus.Waiting: {
                        item.SubItems[1].Text = timeValue;
                        item.SubItems[2].Text = "Waiting";
                        break;
                    }
                case TaskStatus.Stopping: {
                    item.SubItems[1].Text = "";
                    item.SubItems[2].Text = "Stopping";
                    break;
                }
                case TaskStatus.Stopped: {
                    item.SubItems[1].Text = "";
                    item.SubItems[2].Text = "Disabled";

                    // handle stopped event
                    HandleTaskStopped(task);

                    // try to reschedule
                    if(disableRescheduling == false) {
                        manager.StartTask(task.TaskId);
                    }

                    break;
                }
                case TaskStatus.Queued: {
                    item.SubItems[1].Text = "";
                    item.SubItems[2].Text = "Queued";
                    break;
                }
            }

            item.ForeColor = (task.Enabled ? Color.FromName("WindowText") : Color.Gray);
        }

        private void HandleTaskStarted(ScheduledTask task) {
            if(task == activeTask.Tag) {
                StartButton.Enabled = false;
                StopButton.Enabled = true;
                HideDetailsPanel();
                HistoryTool.Visible = false;
                WipeItems.Visible = false;
            }

            _actionManager.StateChanged();
        }

        private void HandleTaskStopped(ScheduledTask task) {
            if(activeTask != null && task == activeTask.Tag) {
                if(HistorySelector.Selected) {
                    // reload history info
                    HistoryTool.Task = task;
                    StartButton.Enabled = true;
                    StopButton.Enabled = false;
                    ShowDetailsPanel();

                    if(WipeSelector.Selected) {
                        WipeSelector.Selected = true;
                    }
                    else {
                        HistorySelector.Selected = true;
                    }
                }
            }

            _actionManager.StateChanged();
        }


        private void LoadSession(ListViewItem item) {
            ScheduledTask task = (ScheduledTask)item.Tag;

            if(task.Status != TaskStatus.Stopped && task.Status != TaskStatus.Waiting) {
                //TaskItemsHeader.Text = "Editing not allowed while task is running";
                WipeItems.Enabled = false;
                HistoryTool.Visible = false;
                WipeItems.Visible = false;
                HideDetailsPanel();
                return;
            }
            else {
                //TaskItemsHeader.Text = "Scheduled Task Items";

                // load the _session
                WipeItems.Enabled = true;
                WipeItems.Async = true;
                WipeItems.LoadSessionFrom(SecureDeleteLocations.GetSessionFile(task.TaskId));
                ShowDetailsPanel();

                if(WipeSelector.Selected) {
                    WipeSelector.Selected = true;
                }
                else {
                    HistorySelector.Selected = true;
                }

                // load history
                manager.TaskHistory.LoadHistory(SecureDeleteLocations.GetScheduleHistoryFile());
                HistoryTool.Manager = manager.TaskHistory;
                HistoryTool.Task = task;
            }
        }

        private void HideDetailsPanel() {
            MainContainer.Panel2Collapsed = true;
        }

        private void ShowDetailsPanel() {
            MainContainer.Panel2Collapsed = false;
        }

        private void TaskList_SelectedIndexChanged(object sender, EventArgs e) {
            if(TaskList.SelectedItems.Count > 0) {
                activeTask = TaskList.SelectedItems[0];
                HandleItemSelection((ScheduledTask)activeTask.Tag);
                LoadSession(activeTask);
            }

            RemoveButton.Enabled = TaskList.SelectedItems.Count > 0;
            EditButton.Enabled = TaskList.SelectedItems.Count > 0;
            ExportButton.Enabled = TaskList.SelectedItems.Count > 0;
            _actionManager.StateChanged();
        }

        private void HandleItemSelection(ScheduledTask scheduledTask) {
            TaskStatus status = scheduledTask.Status;

            if(status != TaskStatus.Wiping && status != TaskStatus.InitializingWiping &&
               status != TaskStatus.Stopping) {
                StartButton.Enabled = true;
                StopButton.Enabled = false;
            }
            else {
                StartButton.Enabled = false;
                StopButton.Enabled = true;
            }
        }

        private void EditTaskOptions() {
            if(activeTask != null) {
                ScheduledTask task = (ScheduledTask)activeTask.Tag;
                ScheduleOptions options = new ScheduleOptions();
                options.Options = _options;
                options.EditMode = true;
                options.Task = task;

                if(options.ShowDialog() == DialogResult.OK) {
                    ListViewItem taskItem = null;
                    ScheduledTask newTask = options.Task;

                    for(int i = 0; i < TaskList.Items.Count; i++) {
                        if((ScheduledTask)TaskList.Items[i].Tag == (ScheduledTask)activeTask.Tag) {
                            taskItem = TaskList.Items[i];
                            break;
                        }
                    }

                    TaskManager.SaveTask(SecureDeleteLocations.GetTaskFile(task.TaskId), newTask);

                    // update the <guid,taskName> mapping
                    if(_options.SessionNames.ContainsKey(newTask.TaskId)) {
                        _options.SessionNames[task.TaskId] = newTask.Name;
                    }
                    else {
                        _options.SessionNames.Add(task.TaskId, newTask.Name);
                    }

                    // save options
                    SDOptionsFile.TrySaveOptions(_options);
                    manager.LoadOptions();

                    // reschedule
                    disableRescheduling = true;

                    if(manager.LoadAndHandleTask(newTask.TaskId) == false) {
                        Debug.ReportWarning("Failed to handle edited task");
                    }
                    else {
                        // update interface
                        if(taskItem != null) {
                            taskItem.Tag = manager.GetTaskById(task.TaskId);
                            activeTask = taskItem;
                        }

                        UpdateTaskItem(activeTask);
                    }
                    disableRescheduling = false;
                }
            }
        }

        private void RemoveSelected() {
            if(activeTask != null) {
                if(MessageBox.Show("Do you really want to remove selected scheduled task ?",
                                   "SecureDelete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
                    // don't delete
                    return;
                }

                ScheduledTask task = (ScheduledTask)activeTask.Tag;

                if(manager.RemoveTask(task.TaskId)) {
                    // remove the session
                    string path = SecureDeleteLocations.GetSessionFile(task.TaskId);

                    if(File.Exists(path)) {
                        File.Delete(path);
                    }

                    // remove the task
                    path = SecureDeleteLocations.GetTaskFile(task.TaskId);

                    if(File.Exists(path)) {
                        File.Delete(path);
                    }

                    // remove from the listview
                    TaskList.Items.Remove(activeTask);

                    // remove it from the internal task list
                    if(_options.SessionNames.ContainsKey(task.TaskId)) {
                        _options.SessionNames.Remove(task.TaskId);
                        SDOptionsFile.TrySaveOptions(_options);
                    }
                }

                _actionManager.StateChanged();

                if(TaskList.Items.Count == 0) {
                    HideDetailsPanel();
                }
            }
        }

        private ScheduledTask GetTaskById(Guid id) {
            foreach(ScheduledTask task in manager.TaskList) {
                if(task.TaskId == id) {
                    return task;
                }
            }

            return null;
        }

        private ListViewItem GetItemByTask(ScheduledTask task) {
            foreach(ListViewItem item in TaskList.Items) {
                if(((ScheduledTask)(item.Tag)).TaskId == task.TaskId) {
                    return item;
                }
            }

            return null;
        }

        private delegate void TestDelegate(Guid taskId);

        private void TaskStatusDispatcher(Guid taskId) {
            ScheduledTask task = GetTaskById(taskId);

            if(task != null) {
                ListViewItem item = GetItemByTask(task);

                if(item != null) {
                    UpdateTaskItem(item);
                }
            }
        }

        private void TaskStatusChanged(Guid taskId, TaskStatus taskStatus) {
            if(this.IsHandleCreated) {
                this.Invoke(new TestDelegate(TaskStatusDispatcher), taskId);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            EditTaskOptions();
        }

        private void StartButton_Click(object sender, EventArgs e) {
            ForceTaskStart();
        }

        private void ForceTaskStart() {
            if(activeTask != null) {
                ScheduledTask task = (ScheduledTask)activeTask.Tag;
                manager.ForceStartTask(task.TaskId);
            }
        }

        private void TaskList_DoubleClick(object sender, EventArgs e) {
            EditTaskOptions();
        }

        private void RemoveButton_Click(object sender, EventArgs e) {
            RemoveSelected();
        }

        private void StatusTimer_Tick(object sender, EventArgs e) {
            for(int i = 0; i < TaskList.Items.Count; i++) {
                if(i < TaskList.Items.Count) {
                    UpdateTaskItem(TaskList.Items[i]);
                }
            }
        }


        private void ScheduleModuleSelector_SelectedStateChanged(object sender, EventArgs e) {
            if(HistorySelector.Selected) {
                HistoryTool.Visible = true;
                HistoryTool.Task = (ScheduledTask)activeTask.Tag;
                WipeItems.Visible = false;

                WipeSelector.Selected = false;
            }
        }

        private void WipeSelector_SelectedStateChanged(object sender, EventArgs e) {
            if(WipeSelector.Selected) {
                HistoryTool.Visible = false;
                WipeItems.Visible = true;

                HistorySelector.Selected = false;
            }
        }

        private void ExportButton_Click(object sender, EventArgs e) {
            ShowExportDialog();
        }

        private void ShowExportDialog() {
            ImportExport dialog = new ImportExport();
            dialog.Options = _options;
            dialog.EnterExportMode(ImportExport.ExporPanelTab.Schedule);

            dialog.ShowDialog();
        }

        private void StopButton_Click(object sender, EventArgs e) {
            TaskStop();
        }

        private void TaskStop() {
            if(activeTask != null) {
                ScheduledTask task = (ScheduledTask)activeTask.Tag;
                manager.ForceStopTask(task.TaskId);
            }

            StopButton.Enabled = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            ShowImportDialog();
        }

        private void ShowImportDialog() {
            ImportExport dialog = new ImportExport();
            dialog.Options = _options;
            dialog.EnterImportMode();

            dialog.ShowDialog();
            LoadTasks();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            AddTask();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            ShowImportDialog();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e) {
            ShowExportDialog();
        }

        private void forceStartToolStripMenuItem_Click(object sender, EventArgs e) {
            ForceTaskStart();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e) {
            TaskStop();
        }

        private void editSelectedItemToolStripMenuItem_Click(object sender, EventArgs e) {
            EditTaskOptions();
        }

        private void removeSelectedToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveSelected();
        }

        private void SendMenuAction(MenuAction action) {
            if(OnMenuAction != null) {
                OnMenuAction(this, action);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            SendMenuAction(MenuAction.Options);
        }

        private void importExportSettingsToolStripMenuItem_Click(object sender, EventArgs e) {
            SendMenuAction(MenuAction.ImportExportSettings);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            SendMenuAction(MenuAction.About);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            SendMenuAction(MenuAction.ExitApplication);
        }

        public void ChangeToolHeaderText(string text) {

        }

        public void ChangeToolHeaderIcon(Image icon) {
            
        }
    }
}
