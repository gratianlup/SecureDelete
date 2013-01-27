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
using SecureDelete.Actions;
using TextParser;
using TextParser.IncludedFilters;
using System.IO;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace SecureDeleteWinForms {
    public partial class PowerShellScriptEditor : Form {
        private class FontHelper {
            public FontFamily Font;

            public FontHelper(FontFamily font) {
                Font = font;
            }

            public override string ToString() {
                if(Font == null) {
                    return "";
                }
                else {
                    return Font.Name;
                }
            }
        }

        public PowerShellScriptEditor() {
            InitializeComponent();
            scriptExecutor = new ScriptExecutor();
            executorInitialized = new ManualResetEvent(false);
        }

        #region Properties

        private string _script;
        public string Script {
            get { return _script; }
            set { _script = value; }
        }

        private string _pluginFolder;
        public string PluginFolder {
            get { return _pluginFolder; }
            set { _pluginFolder = value; }
        }

        #endregion

        #region Fields

        private ScriptExecutor scriptExecutor;
        private StringBuilder resultBuilder;
        private bool updatingColor;
        private int appendCount;
        private BridgeLogger logger;
        private ManualResetEvent executorInitialized;

        #endregion

        #region Custom

        private void UpdateFont() {
            if(FontCombobox.SelectedIndex < 0) {
                return;
            }

            FontHelper font = (FontHelper)FontCombobox.Items[FontCombobox.SelectedIndex];
            float size = 0;

            if(float.TryParse(SizeCombobox.Text, out size) == false) {
                return;
            }

            ScriptEditor.Font = new Font(font.Font, size);
        }

        private void PowerShellScriptEditor_Load(object sender, EventArgs e) {
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.LoadPosition(this);
            ScriptEditor.Text = _script == null ? string.Empty : _script;

            // load the fonts
            foreach(FontFamily family in FontFamily.Families) {
                FontCombobox.Items.Add(new FontHelper(family));
            }

            // set default font
            for(int i = 0; i < FontCombobox.Items.Count; i++) {
                FontHelper font = (FontHelper)FontCombobox.Items[i];

                if(font.Font.Name == this.Font.Name) {
                    FontCombobox.SelectedIndex = i;
                    UpdateFont();
                }
            }

            SizeCombobox.SelectedIndex = 3;

            // initialize script executor in background
            scriptExecutor.OnNewOutput += NewDataHandler;
            scriptExecutor.OnStateChanged += StateChangedHandler;
            MethodInvoker invoker = new MethodInvoker(ExecutorInitialzer);
            invoker.BeginInvoke(null, null);
        }

        private void ExecutorInitialzer() {
            scriptExecutor.CreateRunspace();
            executorInitialized.Set();
        }

        #endregion

        private void FontCombobox_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateFont();
        }

        private void SizeCombobox_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateFont();
        }

        private void ScriptEditor_TextChanged(object sender, EventArgs e) {
            RunButton.Enabled = ScriptEditor.Text.Length > 0;
            StateLabel.Visible = false;
            _script = ScriptEditor.Text;
        }

        private void ExposedButton_Click(object sender, EventArgs e) {
            PowerShellObjects dialog = new PowerShellObjects();
            dialog.PluginFolder = _pluginFolder;
            dialog.ShowDialog();
        }

        private void StateChangedHandler(object sender, PipelineStateInfo state) {
            this.Invoke(new EventHandler(StateChangedInvoked), state, null);
        }

        private void NewDataHandler(object sender, string data) {
            this.Invoke(new EventHandler(NewDataInvoked), data, null);
        }

        private void NewDataInvoked(object sender, EventArgs e) {
            if(appendCount > 1000) {
                resultBuilder = new StringBuilder();
                appendCount = 0;
            }

            resultBuilder.AppendLine((string)sender);
            appendCount++;

            ResultsBox.Text = resultBuilder.ToString();
            ResultsBox.SelectionStart = resultBuilder.Length - 1;
            ResultsBox.SelectionLength = 0;
        }

        private void StateChangedInvoked(object sender, EventArgs e) {
            PipelineStateInfo state = (PipelineStateInfo)sender;

            if(state.State == PipelineState.Completed) {
                StateLabel.Text = "Completed";
                StateLabel.Image = SDResources.OK;
                ExitRunMode();
            }
            else if(state.State == PipelineState.Failed) {
                StateLabel.Text = "Failed";
                StateLabel.Image = SDResources.Delete.ToBitmap();
                ExitRunMode();

                // show reason
                if(state.Reason != null) {
                    ResultsBox.Text = "Failed to execute script.\n\n";
                    ResultsBox.Text += state.Reason.Message;

                    if(state.Reason.StackTrace != null) {
                        ResultsBox.Text += "\n\nStack trace:\n";
                        ResultsBox.Text += state.Reason.StackTrace;
                    }
                }
            }
            else if(state.State == PipelineState.Stopped) {
                StateLabel.Text = "Stopped by user";
                StateLabel.Image = SDResources.Stop.ToBitmap();
                ExitRunMode();
            }
            else if(state.State == PipelineState.Running) {
                StateLabel.Text = "Script running";
            }

            StateLabel.Visible = true;
        }

        private List<IBridge> GetBridgeObjectList(string pluginFolder) {
            // get from executing assembly
            List<IBridge> bridgeList = new List<IBridge>();
            bridgeList.AddRange(BridgeFactory.GetBridgeObjects());

            // get from folder
            if(pluginFolder != null && pluginFolder.Length > 0) {
                try {
                    string[] files = Directory.GetFiles(pluginFolder, "*.dll");

                    foreach(string file in files) {
                        bridgeList.AddRange(BridgeFactory.GetBridgeObjects(file));
                    }
                }
                catch {
                    // Error ignored.
                }
            }

            return bridgeList;
        }

        private void RunButton_Click(object sender, EventArgs e) {
            // check if PowerShell is is installed
            int version;
            if(PowershellAction.IsPowershellInstalled(out version) == false) {
                StateLabel.Visible = true;
                ResultsBox.Text = "PowerShell not installed.";

                PowerShellRequiredDialog dialog = new PowerShellRequiredDialog();
                dialog.PrimaryText.Text = "Windows PowerShell not found on your system.";
                dialog.SecondaryText.Text = "PowerShell needs to be installed in order to run PowerShell scripts.";
                dialog.DownloadButton.Visible = true;
                dialog.ShowDialog();
                return;
            }

            // wait until the executor is created
            executorInitialized.WaitOne();

            // add bridge objects
            SessionControlBridge bridge = new SessionControlBridge();
            scriptExecutor.BridgeList = GetBridgeObjectList(_pluginFolder);

            // set bridge objects to test mode
            logger = new BridgeLogger();
            foreach(IBridge b in scriptExecutor.BridgeList) {
                b.TestMode = true;
                b.Logger = logger;
            }

            // prepare to start
            ExposedResultsButton.Visible = false;
            resultBuilder = new StringBuilder();
            ResultsBox.Text = "";
            appendCount = 0;
            StateLabel.Image = SDResources.PowershellSmall;
            scriptExecutor.RunScriptAsync(ScriptEditor.Text);
            EnterRunMode();
        }

        private void EnterRunMode() {
            NewButton.Enabled = false;
            OpenButton.Enabled = false;
            RunButton.Enabled = false;
            StopButton.Enabled = true;
            ScriptEditor.ReadOnly = true;
        }

        private void ExitRunMode() {
            NewButton.Enabled = true;
            OpenButton.Enabled = true;
            RunButton.Enabled = true;
            StopButton.Enabled = false;
            ScriptEditor.ReadOnly = false;

            if(logger.Items.Count > 0) {
                ExposedResultsButton.Visible = true;
            }
        }

        private void NewButton_Click(object sender, EventArgs e) {
            ScriptEditor.Text = "";
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            CommonDialog dialog = new CommonDialog();
            dialog.Title = "Save script";
            dialog.Filter.Add(new FilterEntry("Text file", "*.txt"));

            if(dialog.ShowSave()) {
                try {
                    string path = dialog.FileName;
                    StreamWriter writer = new StreamWriter(path);
                    writer.Write(ScriptEditor.Text);
                    writer.Close();
                }
                catch {
                    MessageBox.Show("Failed to save script.", "SecureDelete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenButton_Click(object sender, EventArgs e) {
            CommonDialog dialog = new CommonDialog();
            dialog.Title = "Select script file";
            dialog.Filter.Add(new FilterEntry("Text file", "*.txt"));
            dialog.Filter.Add(new FilterEntry("All files", "*.*"));

            if(dialog.ShowOpen()) {
                try {
                    ScriptEditor.LoadFile(dialog.FileName, RichTextBoxStreamType.PlainText);
                }
                catch {
                    MessageBox.Show("Failed to load script.", "SecureDelete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e) {
            int version;

            if(PowershellAction.IsPowershellInstalled(out version)) {
                PowerShellRequiredDialog dialog = new PowerShellRequiredDialog();
                dialog.PrimaryText.Text = "Windows PowerShell found on your system.";
                dialog.SecondaryText.Text = "Version: " + version.ToString();
                dialog.DownloadButton.Visible = false;
                dialog.ShowDialog();
            }
            else {
                PowerShellRequiredDialog dialog = new PowerShellRequiredDialog();
                dialog.PrimaryText.Text = "Windows PowerShell not found on your system.";
                dialog.SecondaryText.Text = "PowerShell needs to be installed in order to run PowerShell scripts.";
                dialog.DownloadButton.Visible = true;
                dialog.ShowDialog();
            }
        }

        private void StopButton_Click(object sender, EventArgs e) {
            scriptExecutor.Stop();
        }

        private void ExposedResultsButton_Click(object sender, EventArgs e) {
            PowerShellObjectResults dialog = new PowerShellObjectResults();
            dialog.Logger = logger;
            dialog.ShowDialog();
        }

        private void ExposedResultsButton_MouseEnter(object sender, EventArgs e) {
            ExposedResultsButton.ForeColor = Color.FromName("WindowText");
        }

        private void ExposedResultsButton_MouseLeave(object sender, EventArgs e) {
            ExposedResultsButton.ForeColor = Color.White;
        }

        private void PowerShellScriptEditor_FormClosing(object sender, FormClosingEventArgs e) {
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.SavePosition(this);
        }
    }
}
