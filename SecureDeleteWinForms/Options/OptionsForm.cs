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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using DebugUtils.Debugger;
using System.IO;
using SecureDelete;
using SecureDelete.Actions;
using SecureDelete.Schedule;

namespace SecureDeleteWinForms {
    public enum OptionsFormStartPanel {
        General, Wipe, Random, Schedule, Shell, Reports
    }

    public partial class OptionsForm : Form {
        public OptionsForm() {
            InitializeComponent();
            powerManager = new PowerManager();
            powerManager.OnPowerStatusChanged += PowerChangedHandler;
            powerManager.Start();
        }

        private PowerManager powerManager;

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set { _options = value; }
        }

        private OptionsFormStartPanel _startPanel;
        public OptionsFormStartPanel StartPanel {
            get { return _startPanel; }
            set { _startPanel = value; HandleNewOptions(); }
        }

        private int fileMethod;
        private int freeSpaceMethod;

        private void UpdateMethodInfo(Label label, int method) {
            if(_options.MethodManager != null && _options.MethodManager.Methods.Count > 0) {
                MethodChangeButton.Enabled = true;
                WipeMethod m = _options.MethodManager.GetMethod(method);

                if(m != null) {
                    label.Text = m.Name;
                }
                else {
                    label.Text = "Wipe method not found";
                }
            }
            else {
                label.Text = "No wipe method available";
            }
        }

        private void SelectMethod(Label label, ref int method) {
            Debug.AssertNotNull(_options.MethodManager, "MethodManager not set");

            WipeMethods w = new WipeMethods();
            w.Options = _options;
            w.MethodManager = _options.MethodManager;
            w.SelectedMethod = _options.MethodManager.GetMethodIndex(method);
            w.ShowSelected = true;
            w.ShowDialog(this);

            if(w.SelectedMethod < 0 || w.SelectedMethod >= _options.MethodManager.Methods.Count) {
                method = WipeOptions.DefaultWipeMethod;
            }
            else {
                method = _options.MethodManager.Methods[w.SelectedMethod].Id;
            }

            UpdateMethodInfo(label, method);
        }

        private void HandleNewOptions() {
            if(_options == null) {
                return;
            }

            // general options
            ConfirmStartCheckbox.Checked = _options.WarnOnStart;
            SaveReportsCheckbox.Checked = _options.SaveReport;
            OnlyWhenErrorsCheckbox.Checked = _options.SaveReportOnErrors;
            LimitReportsCheckbox.Checked = _options.LimitReportNumber;
            MaximumReports.Value = Math.Max(1, _options.ReportsPerSession);
            DeleteOldCheckbox.Checked = _options.DeleteOldReports;
            DeleteOlder.Value = Math.Max(1, _options.OldestReportDate);
            TrayCheckbox.Checked = _options.ShowInTray;
            StatusWindowCheckbox.Checked = _options.ShowStatusWindow;

            // report style
            CustomStyleRadioButton.Checked = _options.CustomReportStyle;
            StyleTextBox.Text = _options.CustomStyleLocation;

            // password
            RequirePasswordCheckbox.Checked = _options.PasswordRequired;

            // file options
            WipeAdsCheckbox.Checked = _options.WipeOptions.WipeAds;
            WipeFileNamesCheckbox.Checked = _options.WipeOptions.WipeFileName;
            TotalDeleteCheckbox.Checked = _options.WipeOptions.TotalDelete;

            // free space options
            DestroyFreeSpaceCheckbox.Checked = _options.WipeOptions.DestroyFreeSpaceFiles;

            // mft options
            WipeUsedFileRecordCheckbox.Checked = _options.WipeOptions.WipeUsedFileRecord;
            WipeUnusedFileRecordCheckbox.Checked = _options.WipeOptions.WipeUnusedFileRecord;
            WipeUsedIndexRecordCheckbox.Checked = _options.WipeOptions.WipeUsedIndexRecord;
            WipeUnusedIndexRecordCheckbox.Checked = _options.WipeOptions.WipeUnusedIndexRecord;

            // wipe methods
            fileMethod = _options.WipeOptions.DefaultFileMethod;
            freeSpaceMethod = _options.WipeOptions.DefaultFreeSpaceMethod;
            UpdateMethodInfo(FileMethodNameLabel, fileMethod);
            UpdateMethodInfo(FreeSpaceMethodNameLabel, freeSpaceMethod);

            // after/before actions
            BeforeCheckbox.Checked = _options.ExecuteBeforeWipeActions;
            AfterCheckbox.Checked = _options.ExecuteAfterWipeActions;
            BeforeActionsLabel.Text = "Actions: " + (_options.BeforeWipeActions == null ? 
                                      "0" : _options.BeforeWipeActions.Count.ToString());
            AfterActionsLabel.Text = "Actions: " + (_options.AfterWipeActions == null ?
                                     "0" : _options.AfterWipeActions.Count.ToString());

            // random options
            RandomOptions r = _options.WipeOptions.RandomOptions;

            if(r.RandomProvider == RandomProvider.ISAAC) {
                IsaacOptionbox.Checked = true;
            }
            else {
                MersenneOptionbox.Checked = true;
            }

            SlowPoolCheckbox.Checked = r.UseSlowPool;
            PreventCheckbox.Checked = r.PreventWriteToSwap;
            ReseedCheckbox.Checked = r.ReseedInterval.TotalMinutes > 0;
            ReseedIntervalTrackbar.Value = Math.Max(1, (int)r.ReseedInterval.TotalMinutes);

            // log
            LogCheckbox.Checked = _options.WipeOptions.UseLogFile;
            LogTextbox.Text = _options.WipeOptions.LogFilePath;
            AppendLogCheckbox.Checked = _options.WipeOptions.AppendToLog;
            LogLimitCheckbox.Checked = _options.WipeOptions.UseLogFileSizeLimit;
            LogLimitValue.Value = Math.Max(LogLimitValue.Minimum, _options.WipeOptions.LogFileSizeLimit);

            // power management
            if(_options.PowerControllerSettings == null) {
                _options.PowerControllerSettings = new PowertTCSettings();
            }

            DisableTasksOnBatteryValue.Checked = _options.PowerControllerSettings.DisableOnBattery;
            StopTasksPowerSaverValue.Checked = _options.PowerControllerSettings.StopIfPowerSaverScheme;
            StopTaskBatteryPowerValue.Checked = _options.PowerControllerSettings.StopIfLowBatteryPower;
            BatteryValue.Value = _options.PowerControllerSettings.MinBatteryPower;

            // schedule
            QueueValue.Checked = _options.QueueTasks;
        }

        private void ReseedCheckbox_CheckedChanged(object sender, EventArgs e) {
            ReseedIntervalTrackbar.Enabled = ReseedCheckbox.Checked;
        }
        
        private void MethodChangeButton_Click(object sender, EventArgs e) {
            SelectMethod(FileMethodNameLabel, ref fileMethod);
        }

        private void button3_Click(object sender, EventArgs e) {
            SelectMethod(FreeSpaceMethodNameLabel, ref freeSpaceMethod);
        }

        private void OptionsForm_Load(object sender, EventArgs e) {
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.LoadPosition(this);

            switch(_startPanel) {
                case OptionsFormStartPanel.General: {
                    GeneralSelector.Selected = true;
                    break;
                }
                case OptionsFormStartPanel.Random: {
                    RandomSelector.Selected = true;
                    break;
                }
                case OptionsFormStartPanel.Wipe: {
                    WipeSelector.Selected = true;
                    break;
                }
                case OptionsFormStartPanel.Reports: {
                    ReportsSelector.Selected = true;
                    break;
                }
                case OptionsFormStartPanel.Schedule: {
                    ScheduleSelector.Selected = true;
                    break;
                }
            }

            HandleNewOptions();

            bool underVista = PowerManager.IsUnderVista();

            StopTaskBatteryPowerValue.Enabled = underVista;
            StopTasksPowerSaverValue.Enabled = underVista;
            BatteryValue.Enabled = underVista;
            PowerValue.Enabled = underVista;
            PowerSchemeLabel.Enabled = underVista;
            PowerTypeLabel.Enabled = underVista;
            label2.Enabled = underVista;

            powerManager.GetPowerStatus();
            PowerChangedHandler(null, null);

            // load shell settings
            ShellNormalCheckbox.Checked = _options.ShellFileOperation;
            ShellMoveCheckbox.Checked = _options.ShellMoveOperation;
            ShellRecycleCheckbox.Checked = _options.ShellRecycleOperation;
        }

        private void TotalDeleteCheckbox_CheckedChanged(object sender, EventArgs e) {
            WipeFileNamesCheckbox.Enabled = !TotalDeleteCheckbox.Checked;
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e) {
            if(ValidateOptions()) {
                SaveOptions();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateOptions() {
            if(RequirePasswordCheckbox.Checked) {
                if(Password1Textbox.Text.Trim() == "") {
                    GeneralSelector.Selected = true;
                    Password1Textbox.Focus();
                    Password1Textbox.SelectAll();
                    ErrorTooltip.Show("No password provided.", Password1Textbox, 3000);

                    return false;
                }
                else if(Password2Textbox.Text != Password1Textbox.Text) {
                    GeneralSelector.Selected = true;
                    Password2Textbox.Focus();
                    Password2Textbox.SelectAll();
                    ErrorTooltip.Show("Invalid verification password.", Password2Textbox, 3000);

                    return false;
                }
            }

            if(CustomStyleRadioButton.Checked) {
                if(File.Exists(StyleTextBox.Text) == false) {
                    GeneralSelector.Selected = true;
                    StyleTextBox.Focus();
                    StyleTextBox.SelectAll();
                    ErrorTooltip.Show("File not found.", StyleTextBox, 3000);

                    return false;
                }
            }

            return true;
        }

        private void SaveOptions() {
            _options.WipeOptions.WipeAds = WipeAdsCheckbox.Checked;
            _options.WipeOptions.WipeFileName = WipeFileNamesCheckbox.Checked;
            _options.WipeOptions.TotalDelete = TotalDeleteCheckbox.Checked;
            _options.WipeOptions.DestroyFreeSpaceFiles = DestroyFreeSpaceCheckbox.Checked;
            _options.WipeOptions.WipeUnusedFileRecord = WipeUnusedFileRecordCheckbox.Checked;
            _options.WipeOptions.WipeUnusedIndexRecord = WipeUnusedIndexRecordCheckbox.Checked;
            _options.WipeOptions.WipeUsedFileRecord = WipeUsedFileRecordCheckbox.Checked;
            _options.WipeOptions.WipeUsedIndexRecord = WipeUsedIndexRecordCheckbox.Checked;

            // password
            _options.PasswordRequired = RequirePasswordCheckbox.Checked;
            _options.Password = SDOptions.ComputePasswordHash(Password1Textbox.Text);

            // report style
            _options.CustomReportStyle = CustomStyleRadioButton.Checked;
            _options.CustomStyleLocation = StyleTextBox.Text;

            // general
            _options.LimitReportNumber = LimitReportsCheckbox.Checked;
            _options.ReportsPerSession = (int)MaximumReports.Value;
            _options.DeleteOldReports = DeleteOldCheckbox.Checked;
            _options.OldestReportDate = (int)DeleteOlder.Value;

            // default wipe methods
            _options.WipeOptions.DefaultFileMethod = fileMethod;
            _options.WipeOptions.DefaultFreeSpaceMethod = freeSpaceMethod;

            // random options
            RandomOptions r = _options.WipeOptions.RandomOptions;

            r.RandomProvider = IsaacOptionbox.Checked ? RandomProvider.ISAAC : RandomProvider.Mersenne;
            r.UseSlowPool = SlowPoolCheckbox.Checked;
            r.PreventWriteToSwap = PreventCheckbox.Checked;
            r.ReseedInterval = ReseedCheckbox.Checked ? TimeSpan.FromMinutes(ReseedIntervalTrackbar.Value) : TimeSpan.FromMinutes(0);
            r.PoolUpdateInterval = r.ReseedInterval.TotalMilliseconds > 0 ? TimeSpan.FromSeconds(20) : TimeSpan.FromSeconds(0);

            // general options
            _options.WarnOnStart = ConfirmStartCheckbox.Checked;
            _options.SaveReport = SaveReportsCheckbox.Checked;
            _options.SaveReportOnErrors = OnlyWhenErrorsCheckbox.Checked;
            _options.ShowInTray = TrayCheckbox.Checked;
            _options.ShowStatusWindow = StatusWindowCheckbox.Checked;

            // before/after actions
            _options.ExecuteBeforeWipeActions = BeforeCheckbox.Checked;
            _options.ExecuteAfterWipeActions = AfterCheckbox.Checked;

            // log file
            _options.WipeOptions.UseLogFile = LogCheckbox.Checked;
            _options.WipeOptions.LogFilePath = LogTextbox.Text;
            _options.WipeOptions.AppendToLog = AppendLogCheckbox.Checked;
            _options.WipeOptions.UseLogFileSizeLimit = LogLimitCheckbox.Checked;
            _options.WipeOptions.LogFileSizeLimit = (int)LogLimitValue.Value;

            // power management
            _options.PowerControllerSettings.DisableOnBattery = DisableTasksOnBatteryValue.Checked;
            _options.PowerControllerSettings.StopIfPowerSaverScheme = StopTasksPowerSaverValue.Checked;
            _options.PowerControllerSettings.StopIfLowBatteryPower = StopTaskBatteryPowerValue.Checked;
            _options.PowerControllerSettings.MinBatteryPower = (int)BatteryValue.Value;

            // schedule
            _options.QueueTasks = QueueValue.Checked;

            // shell settings
            _options.ShellFileOperation = ShellNormalCheckbox.Checked;
            _options.ShellMoveOperation = ShellMoveCheckbox.Checked;
            _options.ShellRecycleOperation = ShellRecycleCheckbox.Checked;
        }

        private void SetSelectorsState(Panel panel, bool state, PanelSelectControl leaveUnchanged) {
            foreach(PanelSelectControl c in panel.Controls) {
                if(c != leaveUnchanged) {
                    c.Selected = state;
                }
            }
        }

        private void WipeSelector_SelectedStateChanged(object sender, EventArgs e) {
            WipePanel.Visible = WipeSelector.Selected;

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void RandomSelector_SelectedStateChanged(object sender, EventArgs e) {
            RandomPanel.Visible = RandomSelector.Selected;

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void ReseedIntervalTrackbar_ValueChanged(object sender, EventArgs e) {
            ReseedLabel.Text = TimeSpan.FromMinutes(ReseedIntervalTrackbar.Value).ToString();
        }

        private void panelSelectControl1_SelectedStateChanged(object sender, EventArgs e) {
            GeneralPanel.Visible = GeneralSelector.Selected;

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }
        
        private void checkBox6_CheckedChanged(object sender, EventArgs e) {
            Password1Textbox.UseSystemPasswordChar = Password2Textbox.UseSystemPasswordChar = !checkBox6.Checked;
        }

        private void SaveReportsCheckbox_CheckedChanged(object sender, EventArgs e) {
            OnlyWhenErrorsCheckbox.Enabled = SaveReportsCheckbox.Checked;
            LimitReportsCheckbox.Enabled = SaveReportsCheckbox.Checked;
            MaximumReports.Enabled = SaveReportsCheckbox.Checked && LimitReportsCheckbox.Checked;
            DeleteOldCheckbox.Enabled = SaveReportsCheckbox.Checked;
            DeleteOlder.Enabled = SaveReportsCheckbox.Checked && DeleteOldCheckbox.Checked;
        }

        private void LimitReportsCheckbox_CheckedChanged(object sender, EventArgs e) {
            MaximumReports.Enabled = LimitReportsCheckbox.Checked;
        }

        private void DeleteOldCheckbox_CheckedChanged(object sender, EventArgs e) {
            DeleteOlder.Enabled = DeleteOldCheckbox.Checked;
        }

        private void RequirePasswordCheckbox_CheckedChanged(object sender, EventArgs e) {
            label11.Enabled = RequirePasswordCheckbox.Checked;
            Password1Textbox.Enabled = RequirePasswordCheckbox.Checked;
            Password2Textbox.Enabled = RequirePasswordCheckbox.Checked;
            checkBox6.Enabled = RequirePasswordCheckbox.Checked;
            label12.Enabled = RequirePasswordCheckbox.Checked;
        }

        private void ExportButton_Click(object sender, EventArgs e) {
            ImportExport dialog = new ImportExport();
            dialog.Options = _options;
            dialog.EnterExportMode(ImportExport.ExporPanelTab.General);
            dialog.ShowDialog();
        }

        private void ImportButton_Click(object sender, EventArgs e) {
            ImportExport dialog = new ImportExport();
            dialog.Options = _options;
            dialog.EnterImportMode();
            dialog.ShowDialog();

            // set the new options
            SDOptionsFile.TryLoadOptions(out _options);
            HandleNewOptions();
        }

        private void ScheduleSelector_SelectedStateChanged(object sender, EventArgs e) {
            SchedulePanel.Visible = ScheduleSelector.Selected;

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void button2_Click_1(object sender, EventArgs e) {
            // generate a test report
            WipeReport r = new WipeReport();
            r.Statistics = new WipeStatistics();

            for(int i = 0; i < 3; i++) {
                r.Errors.Add(new WipeError(DateTime.Now, ErrorSeverity.High, "Test"));
                r.Errors.Add(new WipeError(DateTime.Now, ErrorSeverity.Low, "Test"));
                r.Errors.Add(new WipeError(DateTime.Now, ErrorSeverity.Medium, "Test"));
                r.FailedObjects.Add(new FailedObject(WipeObjectType.File, "C:\\test.txt", null));
            }

            r.FailedObjects[0].AssociatedError = r.Errors[0];
            r.FailedObjects[1].AssociatedError = r.Errors[1];
            r.FailedObjects[2].AssociatedError = r.Errors[2];

            // load the style
            string style = "";

            try {
                if(DefaultStyleRadioButton.Checked) {
                    style = WebReportStyle.HTMLReportStyle;
                }
                else {
                    // load from file
                    string path = StyleTextBox.Text;

                    if(File.Exists(path) == false) {
                        MessageBox.Show("Style file could not be found.", "SecureDelete",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    style = File.ReadAllText(path);
                }

                // generate the report
                string reportPath = Path.GetTempPath() + Path.GetRandomFileName() + ".htm";
                ReportExporter.ExportAsHtml(r, reportPath, style);
                System.Diagnostics.Process.Start(reportPath);
            }
            catch(Exception ex) {
                Debug.ReportError("Failed to generate test report. Exception: {0}", ex.Message);
            }
        }

        private void CustomStyleRadioButton_CheckedChanged(object sender, EventArgs e) {
            StyleLabel.Enabled = CustomStyleRadioButton.Checked;
            StyleTextBox.Enabled = CustomStyleRadioButton.Checked;
            StyleBrowseButton.Enabled = CustomStyleRadioButton.Checked;
        }

        private void StyleBrowseButton_Click(object sender, EventArgs e) {
            CommonDialog dialog = new CommonDialog();
            dialog.Title = "Select style";

            if(dialog.ShowOpen()) {
                StyleTextBox.Text = dialog.FileName;
            }
        }

        private List<IAction> CloneActionList(List<IAction> list) {
            if(list == null) {
                return null;
            }

            List<IAction> copy = new List<IAction>();

            foreach(IAction action in list) {
                copy.Add((IAction)action.Clone());
            }

            return copy;
        }

        private void button1_Click(object sender, EventArgs e) {
            ActionEditorWindow dialog = new ActionEditorWindow();
            dialog.Options = _options;
            dialog.Actions = CloneActionList(_options.BeforeWipeActions);

            if(dialog.ShowDialog() == DialogResult.OK) {
                _options.BeforeWipeActions = dialog.Actions;
                BeforeActionsLabel.Text = "Actions: " + (_options.BeforeWipeActions == null ? 
                                          "0" : _options.BeforeWipeActions.Count.ToString());
            }
        }

        private void panelSelectControl1_SelectedStateChanged_1(object sender, EventArgs e) {
            ReportPanel.Visible = ReportsSelector.Selected;

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            BeforeButton.Enabled = BeforeCheckbox.Checked;
        }

        private void AfterCheckbox_CheckedChanged(object sender, EventArgs e) {
            AfterButton.Enabled = AfterCheckbox.Checked;
        }

        private void AfterButton_Click(object sender, EventArgs e) {
            ActionEditorWindow dialog = new ActionEditorWindow();
            dialog.Options = _options;
            dialog.Actions = CloneActionList(_options.AfterWipeActions);

            if(dialog.ShowDialog() == DialogResult.OK) {
                _options.AfterWipeActions = dialog.Actions;
                AfterActionsLabel.Text = "Actions: " + (_options.AfterWipeActions == null ?
                                         "0" : _options.AfterWipeActions.Count.ToString());
            }
        }

        private void TrayCheckbox_CheckedChanged(object sender, EventArgs e) {
            StatusWindowCheckbox.Enabled = TrayCheckbox.Checked;
        }

        private void LogCheckbox_CheckedChanged(object sender, EventArgs e) {
            label7.Enabled = LogCheckbox.Checked;
            AppendLogCheckbox.Enabled = LogCheckbox.Checked;
            LogTextbox.Enabled = LogCheckbox.Checked;
            LogBrowseButton.Enabled = LogCheckbox.Checked;
            LogLimitCheckbox.Enabled = LogCheckbox.Checked;
            LogLimitValue.Enabled = LogCheckbox.Checked && LogLimitCheckbox.Checked;
            LogLimitLabel.Enabled = LogCheckbox.Checked && LogLimitCheckbox.Checked;
            OpenLogButton.Enabled = LogCheckbox.Checked;
        }

        private void LogLimitCheckbox_CheckedChanged(object sender, EventArgs e) {
            LogLimitValue.Enabled = LogCheckbox.Checked && LogLimitCheckbox.Checked;
            LogLimitLabel.Enabled = LogCheckbox.Checked && LogLimitCheckbox.Checked;
        }

        private void button1_Click_1(object sender, EventArgs e) {
            string file = LogTextbox.Text.Trim();

            if(file.Length > 0) {
                try {
                    System.Diagnostics.Process.Start("notepad", file);
                }
                catch {
                    MessageBox.Show("Failed to open log file.", "SecureDelete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LogBrowseButton_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Select file";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.ShowReadOnly = true;
            dialog.ValidateNames = true;

            if(dialog.ShowDialog() == DialogResult.OK) {
                LogTextbox.Text = dialog.FileName;
            }
        }

        private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e) {
            powerManager.Stop();
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.SavePosition(this);
        }

        private void button1_Click_2(object sender, EventArgs e) {
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.Windows.Clear();
        }

        private void PowerChangedHandler(object sender, EventArgs e) {
            PowerTypeLabel.Text = "Power source: " + (powerManager.PowerSource == PowerSource.AC ? "AC" : "Battery");
            PowerValue.Value = powerManager.BatteryLife;
            toolTip1.SetToolTip(PowerValue, powerManager.BatteryLife.ToString() + " %");
            PowerSchemeLabel.Text = "Power scheme: ";

            if(PowerManager.IsUnderVista()) {
                if(powerManager.PowerScheme == PowerScheme.Balanced) {
                    PowerSchemeLabel.Text += "Balanced";
                }
                else if(powerManager.PowerScheme == PowerScheme.HighPerformance) {
                    PowerSchemeLabel.Text += "High performance";
                }
                else {
                    PowerSchemeLabel.Text += "Power saver";
                }
            }
            else {
                PowerSchemeLabel.Text += "Unavailable";
            }
        }

        private void StopTaskBatteryPowerValue_CheckedChanged(object sender, EventArgs e) {
            BatteryValue.Enabled = StopTaskBatteryPowerValue.Checked && PowerManager.IsUnderVista();
        }

        private void GeneralSelector_Load(object sender, EventArgs e) {

        }

        private void panelSelectControl4_SelectedStateChanged(object sender, EventArgs e) {
            ShellPanel.Visible = ShellSelector.Selected;

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void button5_Click(object sender, EventArgs e) {
            if(MessageBox.Show("Do you really want to remove all task history ?", "SecureDelete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                HistoryManager man = new HistoryManager();

                man.LoadHistory(SecureDeleteLocations.GetScheduleHistoryFile());
                man.Clear();
                man.SaveHistory(SecureDeleteLocations.GetScheduleHistoryFile());
            }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e) {
            HistoryValue.Enabled = OldHistoryCheckbox.Checked;
        }
    }
}