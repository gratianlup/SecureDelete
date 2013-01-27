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
using System.IO;
using DebugUtils.Debugger;
using SecureDelete;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using SecureDelete.Schedule;

namespace SecureDeleteWinForms {
    public partial class ImportExport : Form {
        private enum WizardPanel {
            Welcome, Import, Export, Action, Select
        }

        private enum WizardMode {
            Import, Export
        }

        public enum ExporPanelTab {
            General, Methods, Schedule
        }

        private WizardPanel activePanel;
        private WizardMode mode;
        private List<string> actionErrors;
        private FileStore.FileStore store;
        private ExporPanelTab exportTab;

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set { _options = value; }
        }

        public ImportExport() {
            InitializeComponent();
            actionErrors = new List<string>();
        }

        public void EnterImportMode() {
            ImportOptionbox.Checked = true;
            activePanel = WizardPanel.Import;
        }

        public void EnterExportMode(ExporPanelTab tab) {
            ExportOptionbox.Checked = true;
            activePanel = WizardPanel.Select;
            exportTab = tab;
        }

        private void HidePanels() {
            WelcomePanel.Visible = false;
            ImportPanel.Visible = false;
            ExportPanel.Visible = false;
            SelectPanel.Visible = false;
            ActionPanel.Visible = false;
        }

        private void ChangePanel(WizardPanel panel) {
            HidePanels();

            switch(panel) {
                case WizardPanel.Welcome: {
                    WelcomePanel.Visible = true;
                    BackButton.Enabled = false;
                    break;
                }
                case WizardPanel.Select: {
                    SelectPanel.Visible = true;
                    BackButton.Enabled = true;
                    break;
                }
                case WizardPanel.Export: {
                    ExportPanel.Visible = true;
                    BackButton.Enabled = true;
                    break;
                }
                case WizardPanel.Import: {
                    ImportPanel.Visible = true;
                    BackButton.Enabled = true;
                    break;
                }
                case WizardPanel.Action: {
                    ActionPanel.Visible = true;
                    BackButton.Enabled = false;
                    NextButton.Enabled = false;
                    break;
                }
            }

            activePanel = panel;
        }

        private void BackButton_Click(object sender, EventArgs e) {
            switch(activePanel) {
                case WizardPanel.Export: {
                    ChangePanel(WizardPanel.Select);
                    NextButton.Text = "Next";
                    break;
                    }
                case WizardPanel.Select: {
                    if(ExportOptionbox.Checked) {
                        ChangePanel(WizardPanel.Welcome);
                        NextButton.Text = "Next";
                    }
                    else {
                        ChangePanel(WizardPanel.Import);
                        NextButton.Text = "Next";
                    }

                    break;
                    }
                case WizardPanel.Import: {
                    ChangePanel(WizardPanel.Welcome);
                    NextButton.Text = "Next";
                    break;
                }
            }
        }

        private void NextButton_Click(object sender, EventArgs e) {
            if(CanChange() == false) {
                return;
            }

            switch(activePanel) {
                case WizardPanel.Welcome: {
                        if(ExportOptionbox.Checked) {
                            ChangePanel(WizardPanel.Select);
                            NextButton.Text = "Next";
                            mode = WizardMode.Export;
                            HandleSelect();
                        }
                        else {
                            ChangePanel(WizardPanel.Import);
                            NextButton.Text = "Next";
                            mode = WizardMode.Import;
                        }

                        break;
                    }
                case WizardPanel.Select: {
                        if(ExportOptionbox.Checked) {
                            ChangePanel(WizardPanel.Export);
                            NextButton.Text = "Start";
                        }
                        else {
                            ChangePanel(WizardPanel.Action);
                            HandleAction();
                        }

                        break;
                    }
                case WizardPanel.Import: {
                        ChangePanel(WizardPanel.Select);
                        NextButton.Text = "Start";
                        HandleSelect();
                        break;
                    }
                case WizardPanel.Export: {
                        ChangePanel(WizardPanel.Action);
                        HandleAction();
                        break;
                    }
            }
        }

        private byte[] SerializeTaskList(Dictionary<Guid, string> taskList) {
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            try {
                // serialize in memory
                serializer.Serialize(stream, taskList);
                return stream.ToArray();
            }
            catch(Exception e) {
                Debug.ReportError("Error while serializing import/export task list. Exception: {0}", e.Message);
                return null;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }

        private Dictionary<Guid, string> DeserializeTaskList(byte[] data) {
            if(data == null) {
                return null;
            }

            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);

            try {
                return (Dictionary<Guid, string>)serializer.Deserialize(stream);
            }
            catch(Exception e) {
                Debug.ReportError("Error while deserializing import/export task list. Exception: {0}", e.Message);
                return null;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }

        private byte[] SerializeMethodList(Dictionary<int, string> taskList) {
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            try {
                // serialize in memory
                serializer.Serialize(stream, taskList);
                return stream.ToArray();
            }
            catch(Exception e) {
                Debug.ReportError("Error while serializing import/export method list. Exception: {0}", e.Message);
                return null;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }

        private Dictionary<int, string> DeserializeMethodList(byte[] data) {
            if(data == null) {
                return null;
            }

            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);

            try {
                return (Dictionary<int, string>)serializer.Deserialize(stream);
            }
            catch(Exception e) {
                Debug.ReportError("Error while deserializing import/export method list. Exception: {0}", e.Message);
                return null;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }

        private bool CanChange() {
            if(activePanel == WizardPanel.Export) {
                if(ExportPath.Text.Trim().Length == 0) {
                    ExportPath.Focus();
                    ExportPath.SelectAll();
                    ErrorTooltip.Show("Invalid file path.", ExportPath, 3000);
                    return false;
                }

                if(EncryptCheckbox.Checked) {
                    if(PasswordTextbox.Text.Trim().Length == 0) {
                        PasswordTextbox.Focus();
                        PasswordTextbox.SelectAll();
                        ErrorTooltip.Show("Invalid password.", PasswordTextbox, 3000);
                        return false;
                    }

                    if(PasswordVerificationTextbox.Text.Trim().Length == 0 || PasswordTextbox.Text != PasswordVerificationTextbox.Text) {
                        PasswordVerificationTextbox.Focus();
                        PasswordVerificationTextbox.SelectAll();
                        ErrorTooltip.Show("Invalid verification password.", PasswordVerificationTextbox, 3000);
                        return false;
                    }
                }
            }
            else if(activePanel == WizardPanel.Import) {
                if(File.Exists(ImportPath.Text) == false) {
                    ImportPath.Focus();
                    ImportPath.SelectAll();
                    ErrorTooltip.Show("Invalid file path.", ImportPath, 3000);
                    return false;
                }
            }

            return true;
        }

        private void HandleSelect() {
            if(mode == WizardMode.Export) {
                // enable all checkboxes
                GeneralCheckbox.Enabled = true;
                TasksCheckbox.Enabled = true;

                // load methods
                MethodList.Items.Clear();

                if(_options.MethodManager != null && _options.MethodManager.Methods != null) {
                    foreach(WipeMethod method in _options.MethodManager.Methods) {
                        ListViewItem item = new ListViewItem();
                        item.Text = method.Name;
                        item.Tag = method.Id;

                        MethodList.Items.Add(item);
                    }
                }

                // load tasks
                TaskList.Items.Clear();

                if(_options.SessionNames != null) {
                    foreach(KeyValuePair<Guid, string> kvp in _options.SessionNames) {
                        ListViewItem item = new ListViewItem();
                        item.Text = kvp.Value;
                        item.Tag = kvp.Key;

                        TaskList.Items.Add(item);
                    }
                }
            }
            else {
                // import
                // decrypt if necessarily
                if(store.Encrypt) {
                    SHA256Managed passwordHash = new SHA256Managed();
                    store.Encrypt = true;
                    store.EncryptionKey = passwordHash.ComputeHash(Encoding.ASCII.GetBytes(ImportPassword.Text));

                    // reload
                    store.Load(ImportPath.Text);
                }

                // general
                GeneralCheckbox.Enabled = store.FileExists("options.dat");
                PluginCheckbox.Enabled = store.FileExists("pluginSettings.dat");
                MethodList.Items.Clear();

                // get method list
                Dictionary<int, string> methods = DeserializeMethodList(store.ReadFile("methods\\list.dat"));

                if(methods != null) {
                    foreach(KeyValuePair<int, string> kvp in methods) {
                        if(store.FileExists("methods\\" + kvp.Key.ToString() + WipeMethodManager.MethodFileExtension)) {
                            // add to list
                            ListViewItem item = new ListViewItem();
                            item.Text = kvp.Value;
                            item.Tag = kvp.Key;
                            item.Checked = true;

                            MethodList.Items.Add(item);
                        }
                    }
                }

                MethodsCheckbox.Enabled = MethodList.Items.Count > 0;
                MethodsCheckbox.Checked = MethodList.Items.Count > 0;
                MethodSelector.Enabled = MethodList.Items.Count > 0;

                // task list
                TaskList.Items.Clear();

                // get list of tasks
                Dictionary<Guid, string> tasks = DeserializeTaskList(store.ReadFile("tasks\\list.dat"));

                if(tasks != null) {
                    foreach(KeyValuePair<Guid, string> kvp in tasks) {
                        if(store.FileExists("tasks\\" + kvp.Key.ToString() + SecureDeleteLocations.TaskFileExtension)) {
                            // add to list
                            ListViewItem item = new ListViewItem();
                            item.Text = kvp.Value;
                            item.Tag = kvp.Key;
                            item.Checked = true;

                            TaskList.Items.Add(item);
                        }
                    }
                }

                TasksCheckbox.Checked = TaskList.Items.Count > 0;
            }

            GeneralSelector.Selected = true;
        }

        private void Export() {
            FileStore.FileStore store = new FileStore.FileStore();
            FileStore.StoreFile file = null;
            FileStore.StoreMode storeMode = FileStore.StoreMode.Normal;

            if(EncryptCheckbox.Checked) {
                SHA256Managed passwordHash = new SHA256Managed();
                store.Encrypt = true;
                store.EncryptionKey = passwordHash.ComputeHash(Encoding.ASCII.GetBytes(PasswordTextbox.Text));
                storeMode = FileStore.StoreMode.Encrypted;
            }

            // general settings
            if(GeneralCheckbox.Checked) {
                file = store.CreateFile("options.dat");
                store.WriteFile(file, SDOptionsFile.SerializeOptions(_options), storeMode);
            }

            // default plugin settings
            if(PluginCheckbox.Checked) {
                file = store.CreateFile("pluginSettings.dat");
                if(store.WriteFile(file, SecureDeleteLocations.GetPluginDefaultSettingsFilePath(), storeMode) == false) {
                    actionErrors.Add("Failed to export default plugin settings");
                }
            }

            // add methods
            if(MethodsCheckbox.Checked) {
                store.CreateFolder("methods");
                Dictionary<int, string> methodList = new Dictionary<int, string>();

                // add methods
                foreach(ListViewItem item in MethodList.Items) {
                    if(item.Checked) {
                        int id = (int)item.Tag;
                        string methodFile = Path.Combine(SecureDeleteLocations.GetMethodsFolder(),
                                                         id.ToString() + WipeMethodManager.MethodFileExtension);

                        file = store.CreateFile("methods\\" + Path.GetFileName(methodFile));
                        if(store.WriteFile(file, methodFile, storeMode) == false) {
                            actionErrors.Add("Failed to export wipe method " + item.Text);
                        }

                        // add to methd list
                        methodList.Add(id, item.Text);
                    }
                }

                // store the method list
                file = store.CreateFile("methods\\list.dat");
                store.WriteFile(file, SerializeMethodList(methodList), storeMode);
            }

            // scheduled tasks
            if(TasksCheckbox.Checked) {
                store.CreateFolder("tasks");

                // add task list
                file = store.CreateFile("tasks\\list.dat");
                store.WriteFile(file, SerializeTaskList(_options.SessionNames), storeMode);

                foreach(ListViewItem item in TaskList.Items) {
                    if(item.Checked) {
                        Guid taskId = (Guid)item.Tag;
                        string taskFile = SecureDeleteLocations.GetTaskFile(taskId);
                        string sessionFile = SecureDeleteLocations.GetSessionFile(taskId);

                        file = store.CreateFile("tasks\\" + Path.GetFileName(taskFile));
                        ScheduledTask task = new ScheduledTask();
                        TaskManager.LoadTask(taskFile, out task);
                        store.WriteFile(file, TaskManager.SerializeTask(task), storeMode);

                        file = store.CreateFile("tasks\\" + Path.GetFileName(sessionFile));
                        WipeSession session = new WipeSession();
                        SessionLoader.LoadSession(sessionFile, out session);
                        store.WriteFile(file, SessionSaver.SerializeSession(session), storeMode);
                    }
                }
            }

            // save
            if(store.Save(ExportPath.Text) == false) {
                actionErrors.Add("Failed to export to file " + ExportPath.Text);
            }
        }

        private void WriteData(string path, byte[] data) {
            FileStream writer = new FileStream(path, FileMode.OpenOrCreate);

            if(writer.CanWrite) {
                writer.Write(data, 0, data.Length);
                writer.Close();
            }
        }

        private void Import() {
            // import general options
            if(GeneralCheckbox.Checked) {
                byte[] data = store.ReadFile("options.dat");

                if(data != null) {
                    SDOptionsFile.TrySaveOptions(SDOptionsFile.DeserializeOptions(data));
                }
            }

            // default plugin settings
            if(PluginCheckbox.Checked) {
                byte[] data = store.ReadFile("pluginSettings.dat");

                if(data != null) {
                    WriteData(SecureDeleteLocations.GetPluginDefaultSettingsFilePath(), data);
                }
            }

            // wipe methods
            if(MethodsCheckbox.Checked) {
                string methodsFolder = SecureDeleteLocations.GetMethodsFolder();

                // create folder
                if(Directory.Exists(methodsFolder) == false) {
                    Directory.CreateDirectory(methodsFolder);
                }

                foreach(ListViewItem item in MethodList.Items) {
                    int id = (int)item.Tag;
                    byte[] data = store.ReadFile("methods\\" + id.ToString() + WipeMethodManager.MethodFileExtension);

                    if(data != null) {
                        WriteData(Path.Combine(methodsFolder, id.ToString() + WipeMethodManager.MethodFileExtension), data);
                    }
                }
            }

            // tasks
            if(TasksCheckbox.Checked) {
                string taskFolder = SecureDeleteLocations.GetScheduledTasksDirectory();

                // create folder
                if(Directory.Exists(taskFolder) == false) {
                    Directory.CreateDirectory(taskFolder);
                }

                foreach(ListViewItem item in TaskList.Items) {
                    Guid id = (Guid)item.Tag;
                    string taskFile = SecureDeleteLocations.GetTaskFile(id);
                    string sessionFile = SecureDeleteLocations.GetSessionFile(id);
                    byte[] taskData = store.ReadFile("tasks\\" + Path.GetFileName(taskFile));
                    byte[] sessionData = store.ReadFile("tasks\\" + Path.GetFileName(sessionFile));

                    if(taskData != null && sessionData != null) {
                        TaskManager.SaveTask(taskFile, TaskManager.DeserializeTask(taskData));
                        SessionSaver.SaveSession(SessionLoader.DeserializeSession(sessionData), sessionFile);
                    }
                }
            }
        }

        private void HandleAction() {
            actionErrors.Clear();

            try {
                if(mode == WizardMode.Export) {
                    Export();
                }
                else {
                    Import();
                }
            }
            catch(Exception e) {
                actionErrors.Add("Unexpected error");
            }

            ErrorList.Visible = actionErrors.Count > 0;

            foreach(string s in actionErrors) {
                ErrorList.Items.Add(s);
            }
        }

        private void ImportExport_Load(object sender, EventArgs e) {
            ChangePanel(activePanel);

            if(activePanel == WizardPanel.Select) {
                mode = WizardMode.Export;
                HandleSelect();

                // set the active tab
                switch(exportTab) {
                    case ExporPanelTab.General: {
                        GeneralSelector.Selected = true;
                        GeneralCheckbox.Checked = true;
                        break;
                    }
                    case ExporPanelTab.Methods: {
                        MethodSelector.Selected = true;
                        MethodsCheckbox.Checked = true;
                        break;
                    }
                    case ExporPanelTab.Schedule: {
                        TaskSelector.Selected = true;
                        TasksCheckbox.Checked = true;
                        break;
                    }
                }
            }
            else {
                GeneralSelector.Selected = true;
            }
        }

        private void EncryptCheckbox_CheckedChanged(object sender, EventArgs e) {
            PasswordTextbox.Enabled = EncryptCheckbox.Checked;
            PasswordVerificationTextbox.Enabled = EncryptCheckbox.Checked;
            checkBox6.Enabled = EncryptCheckbox.Checked;
        }

        private void TasksCheckbox_CheckedChanged(object sender, EventArgs e) {
            TaskList.Enabled = TasksCheckbox.Checked;
        }

        private void ImportPath_TextChanged(object sender, EventArgs e) {
            string file = ImportPath.Text;

            if(File.Exists(file)) {
                // load store
                store = new FileStore.FileStore();
                if(store.Load(file) == false) {
                    NextButton.Enabled = false;
                }
                else {
                    PasswordGroup.Visible = store.Encrypt;
                    NextButton.Enabled = true;
                }
            }
            else {
                PasswordGroup.Visible = false;
                NextButton.Enabled = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e) {
            PasswordTextbox.UseSystemPasswordChar = !checkBox6.Checked;
            PasswordVerificationTextbox.UseSystemPasswordChar = !checkBox6.Checked;
        }

        private void SetSelectorsState(Panel panel, bool state, PanelSelectControl leaveUnchanged) {
            foreach(PanelSelectControl c in panel.Controls) {
                if(c != leaveUnchanged) {
                    c.Selected = state;
                }
            }
        }

        private void GeneralSelector_SelectedStateChanged(object sender, EventArgs e) {
            GeneralItemsPanel.Visible = GeneralSelector.Selected;

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void WipeSelector_SelectedStateChanged(object sender, EventArgs e) {
            MethodItemsPanel.Visible = MethodSelector.Selected;

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void RandomSelector_SelectedStateChanged(object sender, EventArgs e) {
            TaskItemsPanel.Visible = TaskSelector.Selected;

            if(((PanelSelectControl)sender).Selected) {
                SetSelectorsState(SelectorPanel, false, sender as PanelSelectControl);
            }
        }

        private void MethodsCheckbox_CheckedChanged(object sender, EventArgs e) {
            MethodList.Enabled = MethodsCheckbox.Checked;
        }

        private void button3_Click(object sender, EventArgs e) {
            foreach(ListViewItem item in MethodList.Items) {
                item.Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            foreach(ListViewItem item in TaskList.Items) {
                item.Checked = true;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void BrowseButton_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.ShowReadOnly = true;
            dialog.ValidateNames = true;

            if(dialog.ShowDialog() == DialogResult.OK) {
                ExportPath.Text = dialog.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.ShowReadOnly = true;
            dialog.ValidateNames = true;

            if(dialog.ShowDialog() == DialogResult.OK) {
                ImportPath.Text = dialog.FileName;
            }
        }
    }
}
