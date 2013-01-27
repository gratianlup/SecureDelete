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
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DebugUtils.Debugger;
using SecureDeleteWinForms.Modules;
using SecureDelete;

namespace SecureDeleteWinForms {
    public partial class MainForm : UserControl {
        public MainForm() {
            InitializeComponent();

            backgoundTasks = new List<BackgoundTask>();
            LoadOptions();
        }

        #region Properties

        [Description("Used in MMC mode")]
        private bool _minimal;
        public bool Minimal {
            get { return _minimal; }
            set {
                _minimal = value;
                SetMinimalMode(!_minimal);
            }
        }

        #endregion

        #region Fields

        private const string WipeModuleName = "WipeModule";
        private const string ReportModuleName = "ReportModule";
        private const string ScheduleModuleName = "ScheduleModule";

        private AboutBox splashScreen;
        private List<IModule> modules;
        private IModule activeModule;
        private MenuStrip activeMenu;

        private List<BackgoundTask> backgoundTasks;
        private BackgoundTask activeTask;

        #endregion

        #region Settings

        public event EventHandler OnModuleChanged;

        public IModule ActiveModule {
            get { return activeModule; }
        }

        public List<IModule> Modules {
            get { return modules; }
        }

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set { _options = value; }
        }

        public void LoadOptions() {
            SDOptionsFile.TryLoadOptions(out _options);

            // set the new options for each module
            if(modules != null) {
                foreach(IModule module in modules) {
                    module.Options = _options;
                }
            }
        }

        public void SaveOptions() {
            SDOptionsFile.TrySaveOptions(_options);
        }

        #endregion

        private void SetMinimalMode(bool value) {
            ModulePanel.Visible = value;

            if(activeMenu != null) {
                activeMenu.Visible = value;
            }
        }

        #region Module management

        private void LoadModules() {
            if(modules == null) {
                modules = new List<IModule>();
            }
            else {
                modules.Clear();
            }

            modules.Add(new WipeModule());
            modules.Add(new ReportModule());
            modules.Add(new ScheduleModule());

            foreach(IModule module in modules) {
                module.ParentControl = this;
                module.Options = _options;
                module.OnStatusChanged += HandleModuleStatusChanged;
            }
        }

        private void DeselectModuleSelectors() {
            WipeModuleSelector.Selected = false;
            ScheduleModuleSelector.Selected = false;
            ReportsModuleSelector.Selected = false;
        }

        public void ChangeModule(string moduleName) {
            Debug.AssertNotNull(moduleName, "ModuleName is null");

            if(activeModule != null && activeModule.ModuleName == moduleName) {
                return;
            }

            // find the module
            activeModule = null;

            for(int i = 0; i < modules.Count; i++) {
                if(modules[i].ModuleName == moduleName) {
                    activeModule = modules[i];
                }
            }

            if(activeModule != null) {
                ModuleHost.Controls.Clear();
                ModuleHost.Controls.Add((UserControl)activeModule);
                ((UserControl)activeModule).Dock = DockStyle.Fill;
                activeModule.Activated();
                this.SuspendLayout();

                // unload current menu
                if(activeMenu != null) {
                    if(this.Controls.Contains(activeMenu)) {
                        this.Controls.Remove(activeMenu);
                    }
                }

                // load new menu
                activeMenu = activeModule.Menu;
                activeMenu.Visible = !_minimal;

                if(activeMenu != null) {
                    activeModule.OnMenuAction -= HandleMenuAction;
                    activeModule.OnMenuAction += HandleMenuAction;
                    activeMenu.Dock = DockStyle.Top;
                    this.Controls.Add(activeMenu);
                }

                this.ResumeLayout();

                if(OnModuleChanged != null) {
                    OnModuleChanged(this, null);
                }
            }
        }

        private void HandleMenuAction(IModule module, MenuAction action) {
            switch(action) {
                case MenuAction.Options: {
                    OptionsForm f = new OptionsForm();
                    f.StartPanel = OptionsFormStartPanel.General;
                    f.Options = _options;

                    if(f.ShowDialog() == DialogResult.OK) {
                        SaveOptions();
                        HandleNewOptions();
                    }

                    break;
                }
                case MenuAction.About: {
                    AboutBox about = new AboutBox();
                    about.ShowDialog();
                    break;
                }
                case MenuAction.ImportExportSettings: {
                    ImportExport dialog = new ImportExport();
                    dialog.Options = _options;
                    dialog.ShowDialog();
                    LoadOptions();
                    break;
                }
            }
        }

        private void HandleNewOptions() {
            if(_options == null) {
                return;
            }

            notifyIcon1.Visible = _options.ShowInTray;
        }

        private void ReportsModuleSelector_Click(object sender, EventArgs e) {
            DeselectModuleSelectors();
            ReportsModuleSelector.Selected = true;
        }

        public void HandleModuleStatusChanged(IModule module, string status) {
            if(module == activeModule) {
                ModuleStatusLabel.Text = status;
            }
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e) {
            LoadModules();
            WipeModuleSelector.Selected = true;
            ChangeModule("WipeModule");
        }

        #region Action Manager

        public BackgoundTask RegisterBackgroundTask(string name, Image image, ProgressBarStyle progressStyle, int progressValue) {
            Debug.AssertNotNull(name, "Name is null");

            BackgoundTask task = new BackgoundTask();
            task.Name = name;
            task.Image = image;
            task.ProgressStyle = progressStyle;
            task.ProgressValue = progressValue;
            task.OnProgressChanged += OnBackgroundTaskProgressChanged;
            task.OnStopped += OnBackgroundTaskStopped;

            backgoundTasks.Add(task);
            ToolStripMenuItem menuItem = new ToolStripMenuItem(task.Name, task.Image, OnBackgroundTaskChanged);
            menuItem.Tag = task;
            TaskList.DropDownItems.Add(menuItem);
            task.AttachedControls.Add(new KeyValuePair<ToolStripMenuItem, ToolStripDropDownItem>(menuItem, TaskList));

            ToolStripMenuItem stopMenuItem = new ToolStripMenuItem("Stop " + task.Name, task.Image, OnBackgroundTaskStop);
            stopMenuItem.Tag = task;
            TaskStopList.DropDownItems.Add(stopMenuItem);
            task.AttachedControls.Add(new KeyValuePair<ToolStripMenuItem, ToolStripDropDownItem>(stopMenuItem, TaskStopList));

            OnBackgroundTaskChanged(task, null);
            return task;
        }

        public void StopActiveTask() {
            if(activeTask != null) {
                activeTask.Stop();
            }
        }

        public void StopAllTasks() {
            while(backgoundTasks.Count > 0) {
                backgoundTasks[0].Stop();
            }
        }

        private void OnBackgroundTaskProgressChanged(object sender, EventArgs e) {
            if((BackgoundTask)sender == activeTask) {
                // update the UI
                TaskProgress.Style = activeTask.ProgressStyle;
                TaskProgress.Value = (int)Math.Max(TaskProgress.Minimum, Math.Min(TaskProgress.Maximum, activeTask.ProgressValue));
            }
        }

        private void OnBackgroundTaskStop(object sender, EventArgs e) {
            BackgoundTask task = ((ToolStripMenuItem)sender).Tag as BackgoundTask;
            task.Stop();
        }

        private void OnBackgroundTaskStopped(object sender, EventArgs e) {
            BackgoundTask task = sender as BackgoundTask;
            task.Destroy();
            backgoundTasks.Remove(task);

            if(backgoundTasks.Count == 0) {
                // hide the controls
                TaskList.Visible = false;
                TaskStopList.Visible = false;
                TaskProgress.Visible = false;
            }
            else {
                // select a new task
                OnBackgroundTaskChanged(backgoundTasks[backgoundTasks.Count - 1], null);
            }
        }

        private void OnBackgroundTaskChanged(object sender, EventArgs e) {
            if(sender is ToolStripMenuItem) {
                activeTask = ((ToolStripMenuItem)sender).Tag as BackgoundTask;
            }
            else {
                activeTask = sender as BackgoundTask;
            }

            // show the controls
            TaskList.Visible = true;
            TaskStopList.Visible = true;
            TaskProgress.Visible = true;

            TaskList.Text = activeTask.Name;
            TaskList.Image = activeTask.Image;
            TaskProgress.Style = activeTask.ProgressStyle;
            TaskProgress.Value = (int)Math.Max(TaskProgress.Minimum, Math.Min(TaskProgress.Maximum, activeTask.ProgressValue));
        }

        private void TaskStopList_ButtonClick(object sender, EventArgs e) {
            StopActiveTask();
        }

        private void stopAllToolStripMenuItem_Click(object sender, EventArgs e) {
            StopAllTasks();
        }

        #endregion

        private void wipeMethodsFormToolStripMenuItem_Click(object sender, EventArgs e) {
            WipeMethods f = new WipeMethods();
            f.ShowSelected = true;
            f.MethodManager = _options.MethodManager;
            f.Show(this);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
            OptionsForm f = new OptionsForm();
            f.Options = _options;
            if(f.ShowDialog() == DialogResult.OK) {
                SaveOptions();
            }
        }

        private void WipeModuleSelector_Click(object sender, EventArgs e) {
            DeselectModuleSelectors();
            WipeModuleSelector.Selected = true;
            ChangeModule(WipeModuleName);
        }

        private void ReportsModuleSelector_Click_1(object sender, EventArgs e) {
            DeselectModuleSelectors();
            ReportsModuleSelector.Selected = true;
            ChangeModule(ReportModuleName);
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e) {
            ScheduleOptions opt = new ScheduleOptions();
            opt.ShowDialog();
        }

        private void ScheduleModuleSelector_Click(object sender, EventArgs e) {
            DeselectModuleSelectors();
            ScheduleModuleSelector.Selected = true;
            ChangeModule(ScheduleModuleName);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {

        }

        private StatusWindow statusWindow;

        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e) {
            if(_options.ShowStatusWindow) {
                if(statusWindow == null) {
                    statusWindow = new StatusWindow();
                }

                if(statusWindow.Visible == false && backgoundTasks.Count > 0) {
                    statusWindow.SetTasks(backgoundTasks);
                    statusWindow.Tag = this;
                    statusWindow.Show();
                }
            }
        }
    }
}
