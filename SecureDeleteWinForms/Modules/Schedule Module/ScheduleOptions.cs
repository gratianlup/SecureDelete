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
using System.Text;
using System.Windows.Forms;
using SecureDelete.Schedule;
using SecureDelete;

namespace SecureDeleteWinForms {
    public partial class ScheduleOptions : Form {
        public ScheduleOptions() {
            InitializeComponent();
            scheduleList = new List<ISchedule>();
            scheduleList.Add(new OneTimeSchedule());
            scheduleList.Add(new DailySchedule());
            scheduleList.Add(new WeeklySchedule());
            scheduleList.Add(new MonthlySchedule());
        }

        #region Fields

        private List<ISchedule> scheduleList;

        #endregion

        #region Properties

        private ScheduledTask _task;
        public ScheduledTask Task {
            get { return _task; }
            set {
                _task = value;
                HandleNewTask();
            }
        }

        private bool _editMode;
        public bool EditMode {
            get { return _editMode; }
            set { _editMode = value; }
        }

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set { _options = value; }
        }

        #endregion

        private void HandleNewTask() {
            if(_task == null) {
                return;
            }

            // modify a copy
            _task = (ScheduledTask)_task.Clone();
            NameTextbox.Text = _task.Name;

            if(_task.Schedule == null) {
                _task.Schedule = new OneTimeSchedule();
            }

            if(_task.Description != null) {
                DescriptionTextbox.Text = _task.Description;
            }

            SetDefaultSchedule(_task.Schedule);
            EnabledCheckbox.Checked = _task.Enabled;
            SaveReportsCheckbox.Checked = _task.SaveReport;
        }

        private ISchedule GetSchedule(ScheduleType type) {
            for(int i = 0; i < scheduleList.Count; i++) {
                if(scheduleList[i].Type == type) {
                    return scheduleList[i];
                }
            }

            return null;
        }

        private void SetDefaultSchedule(ISchedule schedule) {
            for(int i = 0; i < scheduleList.Count; i++) {
                if(scheduleList[i].Type == schedule.Type) {
                    scheduleList[i] = schedule;
                    break;
                }
            }

            if(schedule.Type == ScheduleType.OneTime) {
                radioButton4.Checked = true;
            }
            else if(schedule.Type == ScheduleType.Daily) {
                radioButton1.Checked = true;
            }
            else if(schedule.Type == ScheduleType.Weekly) {
                radioButton2.Checked = true;
            }
            else if(schedule.Type == ScheduleType.Monthly) {
                radioButton3.Checked = true;
            }
        }

        private void SetSelectorsState(Panel panel, bool state, PanelSelectControl leaveUnchanged) {
            foreach(PanelSelectControl c in panel.Controls) {
                if(c != leaveUnchanged) {
                    c.Selected = state;
                }
            }
        }

        private void ScheduleOptions_Load(object sender, EventArgs e) {
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.LoadPosition(this);

            if(_editMode) {
                SaveButton.Text = "Save";
            }
            else {
                SaveButton.Text = "Add Task";
            }

            if(_task == null) _task = new ScheduledTask();
            GeneralSelector.Selected = true;
        }

        private void WipeModuleSelector_SelectedStateChanged(object sender, EventArgs e) {
            GeneralPanel.Visible = GeneralSelector.Selected;

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void ScheduleSelector_SelectedStateChanged(object sender, EventArgs e) {
            SchedulePanel.Visible = ScheduleSelector.Selected;

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void LoadBeforeActionsPanel() {
            if(_task == null) {
                return;
            }

            BeforeActionEditor.Options = _options;
            BeforeActionEditor.Actions = _task.BeforeWipeActions;
        }

        private void LoadAfterActionsPanel() {
            if(_task == null) {
                return;
            }

            AfterActionEditor.Options = _options;
            AfterActionEditor.Actions = _task.AfterWipeActions;
        }


        private void BeforeSelector_SelectedStateChanged(object sender, EventArgs e) {
            BeforePanel.Visible = BeforeSelector.Selected;
            LoadBeforeActionsPanel();

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void AfterSelector_SelectedStateChanged(object sender, EventArgs e) {
            AfterPanel.Visible = AfterSelector.Selected;
            LoadAfterActionsPanel();

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void HostScheduleControl(UserControl control) {
            ScheduleControlHost.Controls.Clear();
            control.Dock = DockStyle.Fill;
            ScheduleControlHost.Controls.Add(control);
        }

        private void SetTaskSchedule(ScheduleType type) {
            switch(type) {
                case ScheduleType.OneTime: {
                    _task.Schedule = GetSchedule(ScheduleType.OneTime);
                    OneTimeScheduleEditor editor = new OneTimeScheduleEditor();
                    editor.Schedule = (OneTimeSchedule)_task.Schedule;
                    HostScheduleControl(editor);
                    break;
                }
                case ScheduleType.Daily: {
                    _task.Schedule = GetSchedule(ScheduleType.Daily);
                    DailyScheduleEditor editor = new DailyScheduleEditor();
                    editor.Schedule = (DailySchedule)_task.Schedule;
                    HostScheduleControl(editor);
                    break;
                }
                case ScheduleType.Weekly: {
                    _task.Schedule = GetSchedule(ScheduleType.Weekly);
                    WeeklyScheduleEditor editor = new WeeklyScheduleEditor();
                    editor.Schedule = (WeeklySchedule)_task.Schedule;
                    HostScheduleControl(editor);
                    break;
                    }
                case ScheduleType.Monthly: {
                    _task.Schedule = GetSchedule(ScheduleType.Monthly);
                    MonthlyScheduleEditor editor = new MonthlyScheduleEditor();
                    editor.Schedule = (MonthlySchedule)_task.Schedule;
                    HostScheduleControl(editor);
                    break;
                    }
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e) {
            if(radioButton4.Checked) {
                SetTaskSchedule(ScheduleType.OneTime);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
            if(radioButton1.Checked) {
                SetTaskSchedule(ScheduleType.Daily);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) {
            if(radioButton2.Checked) {
                SetTaskSchedule(ScheduleType.Weekly);
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e) {
            if(radioButton3.Checked) {
                SetTaskSchedule(ScheduleType.Monthly);
            }
        }

        private void NameTextbox_Validating(object sender, CancelEventArgs e) {
            if(NameTextbox.Text.Trim() == "") {
                NameTextbox.Focus();
                NameTextbox.SelectAll();
                ErrorTooltip.Show("Invalid name entered.", NameTextbox, 3000);
                e.Cancel = true;
            }
            else {
                _task.Name = NameTextbox.Text;
            }
        }

        private void DescriptionTextbox_TextChanged(object sender, EventArgs e) {
            _task.Description = DescriptionTextbox.Text;
        }

        private void OptionsSelector_SelectedStateChanged(object sender, EventArgs e) {
            OptionsPanel.Visible = OptionsSelector.Selected;
            if(OptionsSelector.Selected) {
                LoadOptionsPanel();
            }

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void LoadOptionsPanel() {
            UpdateOptionsStatus();
        }

        private void UpdateOptionsStatus() {
            if(_task.CustomOptions == null) {
                _task.CustomOptions = (WipeOptions)_options.WipeOptions.Clone();
            }

            WipeOptionsEditor.MethodManager = _options.MethodManager;
            WipeOptionsEditor.Options = _task.CustomOptions;
            RandomOptionsEditor.Options = _task.CustomOptions.RandomOptions;

            if(_task.UseCustomOptions) {
                CustomOptionsRadioButton.Checked = true;
            }
            else {
                DefaultOptionsRadioButton.Checked = true;
            }

            UpdateOptionsHostStatus();
        }

        private void UpdateOptionsHostStatus() {
            if(_task.UseCustomOptions) {
                foreach(PanelEx panel in OptionsHost.Controls) {
                    panel.GradientColor1 = Color.FromArgb(54, 84, 105);
                    panel.GradientColor2 = Color.LightSlateGray;
                    panel.TextColor = Color.White;
                }

                WipeOptionsEditor.Enabled = true;
                RandomOptionsEditor.Enabled = true;
            }
            else {
                foreach(PanelEx panel in OptionsHost.Controls) {
                    panel.GradientColor1 = Color.White;
                    panel.GradientColor2 = Color.FromName("Control");
                    panel.TextColor = Color.FromName("WindowText");
                }

                WipeOptionsEditor.Enabled = false;
                RandomOptionsEditor.Enabled = false;
            }
        }

        private void DefaultOptionsRadioButton_CheckedChanged(object sender, EventArgs e) {
            if(DefaultOptionsRadioButton.Checked) {
                _task.UseCustomOptions = false;
                UpdateOptionsStatus();
            }
        }

        private void CustomOptionsRadioButton_CheckedChanged(object sender, EventArgs e) {
            if(CustomOptionsRadioButton.Checked) {
                _task.UseCustomOptions = true;
                UpdateOptionsStatus();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            _task.SaveReport = SaveReportsCheckbox.Checked;
        }
        
        private void EnabledCheckbox_CheckedChanged(object sender, EventArgs e) {
            _task.Enabled = EnabledCheckbox.Checked;
        }

        private void ScheduleOptions_FormClosing(object sender, FormClosingEventArgs e) {
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.SavePosition(this);
        }
    }
}
