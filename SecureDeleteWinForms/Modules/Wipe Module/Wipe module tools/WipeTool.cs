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
using DebugUtils.Debugger;
using SecureDeleteWinForms.Modules;
using SecureDelete.Schedule;
using System.IO;
using SecureDelete.Actions;
using SecureDelete;

namespace SecureDeleteWinForms.WipeTools {
    public partial class WipeTool : UserControl, ITool {
        public WipeTool() {
            InitializeComponent();
            views = new List<ItemStatus>();
            usedViews = new List<int>();
            totalSpeed = new WipeSpeed();
            actionExecuter = new ActionExecutor();
            StatusViewHost.PanelDistance = 3;
        }

        #region Constants

        private const long GigabyteSize = 1024 * 1024 * 1024;
        private const long MegabyteSize = 1024 * 1024;
        private const long KilobyteSize = 1024;

        #endregion

        #region Fileds

        private IModule parentModule;
        private BackgoundTask wipeTask;
        private ActionExecutor actionExecuter;
        private bool stopped;

        #endregion

        #region Wipe status management

        private const int ViewHeigth = 110;
        private List<ItemStatus> views;
        private List<int> usedViews;
        private WipeSpeed totalSpeed;

        public ItemStatus GetItemStatus(int contextId) {
            for(int i = 0; i < views.Count; i++) {
                if(views[i].ContextId == contextId) {
                    return views[i];
                }
            }

            return null;
        }

        public void EnsureContextNumber(WipeStatus status) {
            usedViews.Clear();

            // add main view to the used list
            if(status.WipeStopped == false) {
                usedViews.Add(status.ContextId);
            }

            // main member
            if(GetItemStatus(status.ContextId) == null && status.WipeStopped == false) {
                views.Add(new ItemStatus(status.ContextId));
                RegisterView(views[views.Count - 1].View);
            }

            // children
            if(status.Children != null && status.Children.Length > 0) {
                int count = status.Children.Length;
                WipeStatus[] children = status.Children;

                for(int i = 0; i < count; i++) {
                    // skip contexts that don't wipe anymore
                    if(children[i].WipeStopped) {
                        continue;
                    }

                    if(GetItemStatus(children[i].ContextId) == null) {
                        views.Add(new ItemStatus(children[i].ContextId));
                        RegisterView(views[views.Count - 1].View);
                    }

                    usedViews.Add(children[i].ContextId);
                }
            }

            // remove unused views
            int position = 0;

            while(position < views.Count) {
                if(usedViews.IndexOf(views[position].ContextId) == -1) {
                    // remove
                    UnregisterView(views[position]);
                    position--;
                }

                position++;
            }
        }

        private void RegisterView(WipeStatusView view) {
            StatusViewHost.Controls.Add(view);
            view.ExpandedSize = 101;
            view.AllowCollapse = true;
            view.Width = StatusViewHost.Width - 3;
        }

        private void UnregisterView(ItemStatus view) {
            if(StatusViewHost.Controls.Contains(view.View) && views.Contains(view)) {
                StatusViewHost.Controls.Remove(view.View);
                views.Remove(view);
            }
        }

        private void UnregisterViews() {
            while(views.Count > 0) {
                UnregisterView(views[0]);
            }
        }

        private void UpdateViewStatus(WipeStatus status) {
            ItemStatus itemStatus = GetItemStatus(status.ContextId);
            if(itemStatus != null) {
                // reset the speed calculator if a new object is wiped
                if(status.ObjectIndex != itemStatus.ObjectIndex) {
                    itemStatus.ObjectIndex = status.ObjectIndex;
                    itemStatus.Speed.Reset();
                    itemStatus.View.Subtitle = "";
                }

                if(status.ObjectBytesToWipe > 0) {
                    double progress = Math.Max(0, Math.Min(100.0, ((double)status.ObjectBytesWiped / 
                                                                   (double)status.ObjectBytesToWipe) * 100.0));
                    itemStatus.View.ItemProgressBar.Value = (int)Math.Floor(progress);
                    itemStatus.View.PercentLabel.Text = string.Format("{0:f2} %", progress);
                }

                itemStatus.View.MainText = status.MainMessage;
                itemStatus.View.SecondaryStatusLabel.Text = status.AuxMessage;

                // don't show step status for drives
                if(status.ObjectType != WipeObjectType.Drive && status.ObjectType != WipeObjectType.MFT) {
                    itemStatus.View.StepLabel.Text = "Step " + status.ActualStep.ToString() + " of " + status.Steps.ToString();
                }
                else {
                    itemStatus.View.StepLabel.Text = "";
                }

                itemStatus.View.Subtitle = GetDurationString(itemStatus.Speed.UpdateSpeed(status.ObjectBytesWiped),
                                                             status.ObjectBytesWiped, status.ObjectBytesToWipe);
                if(status.ObjectType == WipeObjectType.ClusterTips) {
                    itemStatus.View.SizeLabel.Text = GetClusterTipsString(status.ObjectBytesWiped, status.ObjectBytesToWipe);
                }
                else if(status.ObjectType == WipeObjectType.MFT) {
                    itemStatus.View.SizeLabel.Text = GetMFTString(status.ObjectBytesWiped, status.ObjectBytesToWipe);
                }
                else {
                    itemStatus.View.SizeLabel.Text = GetSizeString(status.ObjectBytesWiped, status.ObjectBytesToWipe);
                }
            }
        }

        private void UpdateStatus(WipeStatus status) {
            TotalPanel.Title = "Total";
            TotalProgressbar.Style = ProgressBarStyle.Continuous;
            EnsureContextNumber(status);

            if(wipeTask != null) {
                wipeTask.ProgressStyle = ProgressBarStyle.Continuous;
            }

            // total status
            if(status.BytesToWipe > 0) {
                double progress = Math.Max(0, Math.Min(100.0, ((double)status.BytesWiped / 
                                                               (double)status.BytesToWipe) * 100.0));
                TotalProgressbar.Value = (int)Math.Floor(progress);
                PercentLabel.Text = string.Format("{0:f2} %", progress);
                TotalPanel.Subtitle = GetDurationString(totalSpeed.UpdateSpeed(status.BytesWiped), 
                                                        status.BytesWiped, status.BytesToWipe);
                if(wipeTask != null) {
                    wipeTask.ProgressValue = progress;
                }
            }

            // main member
            UpdateViewStatus(status);

            // children
            if(status.Children != null && status.Children.Length > 0) {
                int count = status.Children.Length;
                for(int i = 0; i < count; i++) {
                    UpdateViewStatus(status.Children[i]);
                }
            }
        }

        private string GetMFTString(long index, long total) {
            return index.ToString() + " of " + total.ToString() + " MFT Entries";
        }

        private string GetClusterTipsString(long index, long total) {
            return index.ToString() + " of " + total.ToString() + " Files";
        }

        private string GetSizeString(long wiped, long toWipe) {
            StringBuilder builder = new StringBuilder();
            double a = wiped;
            double b = toWipe;
            string sizeType = string.Empty;

            if(toWipe >= GigabyteSize) {
                a /= (double)GigabyteSize;
                b /= (double)GigabyteSize;
                sizeType = "GB";
            }
            else if(toWipe >= MegabyteSize) {
                a /= (double)MegabyteSize;
                b /= (double)MegabyteSize;
                sizeType = "MB";
            }
            else if(toWipe >= KilobyteSize) {
                a /= (double)KilobyteSize;
                b /= (double)KilobyteSize;
                sizeType = "KB";
            }
            else {
                sizeType = "B";
            }

            builder.AppendFormat("{0:F2}", a);
            builder.Append(" of ");
            builder.AppendFormat("{0:F2}", b);
            builder.Append(' ');
            builder.Append(sizeType);
            return builder.ToString();
        }


        private string GetDurationString(long speed, long wiped, long toWipe) {
            if(speed <= 0) {
                return string.Empty;
            }

            long seconds = Math.Max(0, toWipe - wiped) / speed;
            StringBuilder builder = new StringBuilder();
            builder.Append("Time Remaining: ");

            // append hours
            bool appendComma = false;

            if(seconds >= 3600) {
                builder.Append(seconds / 3600);
                builder.Append(seconds / 3600 == 0 ? " hour" : " hours");
                seconds %= 3600;
                appendComma = seconds > 0;
            }

            if(seconds >= 60) {
                if(appendComma) {
                    builder.Append(',');
                }

                builder.Append(seconds / 60);
                builder.Append(seconds / 60 == 0 ? " minute" : " minutes");
                seconds %= 60;
                appendComma = seconds > 0;
            }

            if(seconds >= 0) {
                if(appendComma) {
                    builder.Append(", ");
                }

                builder.Append(seconds);
                builder.Append(" seconds");
            }

            return builder.ToString();
        }

        #endregion

        #region Wipe control

        public void ExecuteBeforeWipeActions() {
            RegisterTask();

            if(wipeTask != null) {
                wipeTask.ProgressStyle = ProgressBarStyle.Marquee;
            }

            if(_options.ExecuteBeforeWipeActions) {
                TotalPanel.Title = "Running before wipe actions";
                TotalProgressbar.Style = ProgressBarStyle.Marquee;
                actionExecuter.Actions = _options.BeforeWipeActions;
                actionExecuter.Session = _session;
                actionExecuter.AfterWipe = false;
                actionExecuter.OnStopped += BeforeActionsFinished;
                actionExecuter.StartAsync();
            }
            else {
                StartWipeProcess();
            }
        }

        public void ExecuteAfterWipeActions() {
            StatusTimer.Enabled = false;

            if(_options.ExecuteAfterWipeActions && AfterCheckbox.Checked) {
                if(wipeTask != null) {
                    wipeTask.ProgressStyle = ProgressBarStyle.Marquee;
                }

                TotalPanel.Title = "Running after wipe actions";
                PercentLabel.Text = TotalPanel.Subtitle = "";
                TotalProgressbar.Style = ProgressBarStyle.Marquee;
                AfterCheckbox.Visible = false;

                actionExecuter.Actions = _options.AfterWipeActions;
                actionExecuter.Session = _session;
                actionExecuter.AfterWipe = true;
                actionExecuter.OnStopped += AfterActionsFinished;

                actionExecuter.StartAsync();
            }
            else {
                Stop();
            }
        }

        public void AfterActionsFinished(object sender, EventArgs e) {
            // start main wipe
            actionExecuter.OnStopped -= AfterActionsFinished;
            this.Invoke(new MethodInvoker(Stop));
        }

        public void BeforeActionsFinished(object sender, EventArgs e) {
            // start main wipe
            actionExecuter.OnStopped -= BeforeActionsFinished;
            this.Invoke(new MethodInvoker(StartWipeProcess));
        }

        public void StartWipeProcess() {
            if(stopped) {
                Stop();
            }

            if(_options.ExecuteBeforeWipeActions && 
               (actionExecuter.Result == false || _session.StoppedByBridge)) {
                Stop();
                return;
            }

            TotalPanel.Title = "Initializing...";
            TotalProgressbar.Style = ProgressBarStyle.Marquee;

            if(wipeTask != null) {
                wipeTask.ProgressStyle = ProgressBarStyle.Marquee;
            }

            _session.Options = _options.WipeOptions;

            if(_session.Options == null) {
                _session.Options = new WipeOptions();
            }

            _session.Options.WipeMethodsPath = _options.MethodFolder;
            _session.PluginAssemblies = new List<string>(SecureDeleteLocations.GetPluginAssemblies());

            // start
            _session.BeginStart(WipeStarted, null);
            StatusTimer.Enabled = true;
            totalSpeed.Reset();

            for(int i = 0; i < views.Count; i++) {
                views[i].Speed.Reset();
            }
        }

        public void Start() {
            if(_session.Status == SessionStatus.Stopped) {
                TotalProgressbar.Style = ProgressBarStyle.Marquee;
                TotalProgressbar.Value = 0;
                PercentLabel.Text = TotalPanel.Subtitle = "";
                UnregisterViews();

                if(OnWipeStarted != null) {
                    OnWipeStarted(this, null);
                }

                StatisicsPanel.Visible = false;
                CloseButton.Visible = false;
                stopped = false;
                _session.BeforeWipeErrors.Clear();
                _session.AfterWipeErrors.Clear();
                _session.BridgeItems.Clear();
                _session.StoppedByBridge = false;
                AfterCheckbox.Checked = true;
                AfterCheckbox.Visible = true;

                // run before wipe actions
                ExecuteBeforeWipeActions();
            }
            else if(_session.Status == SessionStatus.Paused) {
                // resume
                _session.Resume();
            }
        }

        private void WipeStarted(IAsyncResult result) {
            _session.EndStart();
        }

        private void RegisterTask() {
            IModule module = _parentControl as IModule;

            if(module != null) {
                MainForm form = module.ParentControl as MainForm;

                if(form != null) {
                    wipeTask = form.RegisterBackgroundTask("Wipe Status", _toolIcon, ProgressBarStyle.Marquee, 0);
                    wipeTask.OnStopped += WipeStopped;
                }
            }
        }

        private void UnregisterTask() {
            if(wipeTask != null) {
                wipeTask.Stop();
            }
        }

        private void WipeStopped(object sender, EventArgs e) {
            Stop();
        }

        private void ShowStatistics() {
            StatisicsPanel.Visible = true;
            DurationLabel.Text = _session.Statistics.Duration.ToString();
            WipedLabel.Text = _session.Statistics.TotalWipedBytes.ToString();
            WipedSlackLabel.Text = _session.Statistics.BytesInClusterTips.ToString();
            SpeedLabel.Text = _session.Statistics.AverageWriteSpeed.ToString() + " B/s (" + string.Format("{0:F2}",
                              ((double)_session.Statistics.AverageWriteSpeed / (double)MegabyteSize)) + " MB\\s)";

            int errors = _session.Statistics.Errors;

            if(_session.AfterWipeErrors != null) {
                errors += _session.AfterWipeErrors.Count;
            }
            if(_session.BeforeWipeErrors != null) {
                errors += _session.BeforeWipeErrors.Count;
            }

            ErrorLabel.Text = errors.ToString();
            FailedLabel.Text = _session.Statistics.FailedObjects.ToString();

            if(_session.Statistics.FailedObjects > 0) {
                FailedLabel.ForeColor = Color.Red;
            }
            else {
                FailedLabel.ForeColor = Color.FromName("ControlText");
            }
        }

        private void AfterStop() {
            try {
                // remove all views
                UnregisterViews();
                StatusTimer.Enabled = false;

                if(OnWipeStopped != null) {
                    OnWipeStopped(this, null);
                }

                UnregisterTask();
                AfterCheckbox.Visible = false;
                ShowStatistics();

                if(_options.SaveReport) {
                    SaveReport();
                }

                SaveReportLabel.Visible = _options.SaveReport == false;
                CloseButton.Visible = true;
            }
            catch(Exception e) {
                ShowStatistics();
            }
        }

        public void Stop() {
            if(actionExecuter.Stopped == false) {
                actionExecuter.Stop();
            }

            if(_session.Status != SessionStatus.Stopped) {
                _session.Stop();
            }

            if(stopped == false) {
                stopped = true;
                AfterStop();
            }
        }

        public void Pause() {

        }

        #endregion

        #region ITool Members

        public string ModuleName {
            get { return "WipeTool"; }
        }

        public ToolType Type {
            get { return ToolType.Wipe; }
        }

        public int RequiredSize {
            get { return 350; }
        }

        private WipeSession _session;
        public WipeSession Session {
            get { return _session; }
            set { _session = value; }
        }

        public void InitializeTool() {
            if(_parentControl != null) {
                parentModule = (IModule)_parentControl;
            }
        }

        public void DisposeTool() {
            Stop();
        }

        private Image _toolIcon;
        public Image ToolIcon {
            get { return _toolIcon; }
            set { _toolIcon = value; }
        }

        public event EventHandler OnClose;
        public event EventHandler OnWipeStarted;
        public event EventHandler OnWipeStopped;

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

        public bool FooterVisible {
            get { return FooterPanel.Visible; }
            set { FooterPanel.Visible = value; }
        }

        #endregion

        private void StatusTimer_Tick(object sender, EventArgs e) {
            if(_session.Context.Status != ContextStatus.Stopped) {
                WipeStatus status = new WipeStatus();

                if(_session.GetWipeStatus(ref status, true)) {
                    UpdateStatus(status);
                }
            }
            else {
                if(_session.Status != SessionStatus.BeginStart) {
                    ExecuteAfterWipeActions();
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ReportTool reportTool = (ReportTool)parentModule.ChangeTool(ToolType.Report, true);
            parentModule.ChangeToolHeaderText("Wipe Results");
            reportTool.DisposeTool();
            reportTool.Report = _session.GenerateReport();
            reportTool.InitializeTool();
            reportTool.SetActivePanel(ReportTool.PanelType.FailedObjects);
        }

        private void SaveReport() {
            if(_session.Status == SessionStatus.Stopped) {
                WipeReport report = _session.GenerateReport();

                // don't save if there are no erros
                if(_options.SaveReportOnErrors && report.Errors.Count == 0) {
                    return;
                }

                WipeReportManager manager = new WipeReportManager();
                string reportFilePath = SecureDeleteLocations.GetReportFilePath();
                string reportDirectory = SecureDeleteLocations.GetReportDirectory();

                if(File.Exists(reportFilePath)) {
                    manager.LoadReportCategories(reportFilePath);
                }

                manager.ReportDirectory = reportDirectory;

                if(_options.MaximumReportsPerSession > 0) {
                    manager.MaximumReportsPerSession = _options.MaximumReportsPerSession;
                }

                // add and save the report
                ReportInfo info;
                manager.AddReport(report, out info);
                manager.SaveReport(report, info);
                manager.SaveReportCategories(reportFilePath);
                SaveReportLabel.Visible = false;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            if(OnClose != null) {
                OnClose(this, null);
            }
        }

        private void SaveReportLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            SaveReport();
        }
    }


    public class WipeSpeed {
        private const int SpeedUpdateInterval = 3000; // 3 sec.
        private const int MaxSpeedSteps = 36;

        private int lastTick;
        private long[] values = new long[MaxSpeedSteps];
        private int[] times = new int[MaxSpeedSteps];
        private int position;
        private long speed;

        public WipeSpeed() {
            Reset();
        }

        public void Reset() {
            speed = -1;
            position = 0;
        }

        public long UpdateSpeed(long wiped) {
            values[position % MaxSpeedSteps] = wiped;
            times[position % MaxSpeedSteps] = Environment.TickCount;
            position++;

            // update ?
            if(Environment.TickCount - lastTick >= SpeedUpdateInterval) {
                int count = Math.Min(position, SpeedUpdateInterval);
                speed = 0;

                if(count < SpeedUpdateInterval) {
                    for(int i = 0; i < position; i++) {
                        if(i >= 1) {
                            long speedDiff = Math.Abs(values[i % MaxSpeedSteps] - values[(i - 1) % MaxSpeedSteps]);
                            int timeDiff = Math.Abs(times[i % MaxSpeedSteps] - times[(i - 1) % MaxSpeedSteps]);
                            speed += (1000 * speedDiff) / Math.Max(1, timeDiff);
                        }
                    }
                }
                else {
                    for(int i = 0; i < count; i++) {
                        speed += values[position % MaxSpeedSteps] - values[(position - 1) % MaxSpeedSteps];
                    }
                }

                speed /= Math.Max(1, count);
                lastTick = Environment.TickCount;
            }

            if(position == int.MaxValue) {
                position = MaxSpeedSteps;
            }

            return speed;
        }
    }


    public class ItemStatus {
        public ItemStatus() {
            View = new WipeStatusView();
            ObjectIndex = -1;
            Speed = new WipeSpeed();
        }

        public ItemStatus(int id) {
            View = new WipeStatusView();
            ContextId = id;
            ObjectIndex = -1;
            Speed = new WipeSpeed();
        }

        public int ContextId;
        public WipeStatusView View;
        public WipeSpeed Speed;
        public long ObjectIndex;
    }
}
