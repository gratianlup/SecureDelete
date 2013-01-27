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
using System.IO;
using System.Windows.Forms;
using System.Threading;
using SecureDeleteWinForms.WipeTools;
using DebugUtils.Debugger;
using SecureDelete.WipeObjects;
using SecureDelete.WipePlugin;
using System.Collections;
using SecureDelete;

namespace SecureDeleteWinForms.Modules {
    public partial class WipeModule : UserControl, IModule {
        public const string WipeModuleName = "WipeModule";

        public WipeModule() {
            InitializeComponent();
            _session = new WipeSession();
            loadStopped = true;
            _async = true;
            InitializeActions();
        }

        private bool loadStopped;
        private BackgoundTask loadTask;

        public string ModuleName {
            get { return WipeModuleName; }
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
        public Control ParentControl {
            get { return _parentControl; }
            set { _parentControl = value; }
        }

        private bool _scheduleMode;
        public bool ScheduleMode {
            get { return _scheduleMode; }
            set {
                _scheduleMode = value;
                StartButton.Visible = _scheduleMode == false;
                PauseButton.Visible = _scheduleMode == false;
                StopButton.Visible = _scheduleMode == false;
                LastSplitter.Visible = _scheduleMode == false;
                MenuStrip.Visible = _scheduleMode == false;

                toolStrip1.ImageScalingSize = new Size(_scheduleMode == false ? 24 : 16,
                                                       _scheduleMode == false ? 24 : 16);
                SaveButton.Visible = _scheduleMode;
            }
        }

        public event EventHandler OnListSaved;
        public event ModuleStatusDelegate OnStatusChanged;
        public event MenuActionDelegeate OnMenuAction;

        public void Activated() {
            UpdateStatus();
        }

        private void WipeModule_Load(object sender, EventArgs e) {
            LoadTools();
            HideToolPanel();
        }

        #region Tool management

        private List<ITool> tools;
        private ITool activeTool;
        private bool toolPanelClosed;

        private void LoadTools() {
            if(tools == null) {
                tools = new List<ITool>();
            }
            else {
                tools.Clear();
            }

            tools.Add(new FileTool());
            tools.Add(new SearchTool());
            tools.Add(new FolderTool());
            tools.Add(new FreeSpaceTool());
            tools.Add(new ReportTool());
            tools.Add(new PluginTool());

            // special case for wipe tool
            WipeTool wipeTool = new WipeTool();
            wipeTool.OnWipeStarted += EnterWipeMode;
            wipeTool.OnWipeStopped += ExitWipeMode;
            tools.Add(wipeTool);

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

        private bool IsEditorTool() {
            return (activeTool != null && (activeTool.Type == ToolType.File ||
                                           (activeTool.Type == ToolType.Folder && ((FolderTool)activeTool).InsertMode == false) ||
                                           activeTool.Type == ToolType.FreeSpace ||
                                           activeTool.Type == ToolType.Plugin));
        }

        public static Image ModuleImage {
            get {
                return SecureDeleteWinForms.Properties.Resources.Computer;
            }
        }

        public static Dictionary<string, Image> ActionImages {
            get {
                Dictionary<string, Image> images = new Dictionary<string, Image>();
                images.Add("New", SecureDeleteWinForms.Properties.Resources.DocumentHS);
                images.Add("Open", SecureDeleteWinForms.Properties.Resources.openHS);
                images.Add("Save", SecureDeleteWinForms.Properties.Resources.saveHS);
                images.Add("File", SecureDeleteWinForms.Properties.Resources.file);
                images.Add("Folder", SecureDeleteWinForms.Properties.Resources.folder);
                images.Add("Drive", SecureDeleteWinForms.Properties.Resources.new_hdd);
                images.Add("Plugin", SecureDeleteWinForms.Properties.Resources.personal_data);
                images.Add("Search", SecureDeleteWinForms.Properties.Resources.Project3);
                images.Add("Edit", SecureDeleteWinForms.Properties.Resources.creion32);
                images.Add("Remove", SecureDeleteWinForms.Properties.Resources.delete_profile);
                images.Add("Start", SecureDeleteWinForms.Properties.Resources.dfsd);
                images.Add("Stop", SecureDeleteWinForms.Properties.Resources.safdgsd);
                return images;
            }
        }

        private void InitializeActions() {
            _actionManager = new ModuleActionManager();
            _actionManager.Actions.Add("New", new ModuleAction(ActionType.PanelAction, "New", "New",
                                                                delegate() { return newToolStripMenuItem.Enabled; },
                                                                newToolStripMenuItem_Click));

            _actionManager.Actions.Add("Open", new ModuleAction(ActionType.PanelAction, "Open", "Open",
                                                                delegate() { return openToolStripMenuItem.Enabled; },
                                                                openToolStripMenuItem_Click));

            _actionManager.Actions.Add("Save", new ModuleAction(ActionType.PanelAction, "Save", "Save",
                                                                delegate() { return saveToolStripMenuItem.Enabled; },
                                                                saveToolStripMenuItem_Click));

            _actionManager.Actions.Add("SaveAs", new ModuleAction(ActionType.PanelAction, "SaveAs", "SaveAs",
                                                                delegate() { return saveAsToolStripMenuItem.Enabled; },
                                                                saveAsToolStripMenuItem_Click));

            _actionManager.Actions.Add("File", new ModuleAction(ActionType.PanelAction, "File", "File",
                                                                delegate() { return fileToolStripMenuItem1.Enabled; },
                                                                fileToolStripMenuItem2_Click));

            _actionManager.Actions.Add("Folder", new ModuleAction(ActionType.PanelAction, "Folder", "Folder",
                                                                delegate() { return folderToolStripMenuItem1.Enabled; },
                                                                folderToolStripMenuItem1_Click));

            _actionManager.Actions.Add("Drive", new ModuleAction(ActionType.PanelAction, "Drive", "Drive",
                                                                delegate() { return freeSpaceToolStripMenuItem1.Enabled; },
                                                                freeSpaceToolStripMenuItem1_Click));

            _actionManager.Actions.Add("Plugin", new ModuleAction(ActionType.PanelAction, "Plugin", "Plugin",
                                                                delegate() { return pluginToolStripMenuItem.Enabled; },
                                                                pluginToolStripMenuItem_Click));

            _actionManager.Actions.Add("Search", new ModuleAction(ActionType.PanelAction, "Search", "Search",
                                                                delegate() { return searchFilesToolStripMenuItem.Enabled; },
                                                                searchFilesToolStripMenuItem_Click));

            _actionManager.Actions.Add("Edit", new ModuleAction(ActionType.PanelAction, "Edit", "Edit",
                                                                delegate() { return editSelectedItemToolStripMenuItem.Enabled; },
                                                                editSelectedItemToolStripMenuItem_Click));

            _actionManager.Actions.Add("RemoveSelected", new ModuleAction(ActionType.PanelAction, "RemoveSelected", "Remove",
                                                                delegate() { return removeSelectedToolStripMenuItem.Enabled; },
                                                                removeSelectedToolStripMenuItem_Click));

            _actionManager.Actions.Add("RemoveAll", new ModuleAction(ActionType.PanelAction, "RemoveAll", "Remove",
                                                                delegate() { return removeAllToolStripMenuItem1.Enabled; },
                                                                removeAllToolStripMenuItem1_Click));

            _actionManager.Actions.Add("Start", new ModuleAction(ActionType.PanelAction, "Start", "Start",
                                                                delegate() { return startToolStripMenuItem.Enabled; },
                                                                startToolStripMenuItem_Click));

            _actionManager.Actions.Add("Stop", new ModuleAction(ActionType.PanelAction, "Stop", "Stop",
                                                                delegate() { return stopToolStripMenuItem.Enabled; },
                                                                stopToolStripMenuItem_Click));

            _actionManager.Actions.Add("Options", new ModuleAction(ActionType.PanelAction, "Options", "Options",
                                                                delegate() { return optionsToolStripMenuItem.Enabled; },
                                                                optionsToolStripMenuItem_Click));
        }

        #endregion

        #region WipeSession management

        private string sessionFile;

        private WipeSession _session;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WipeSession Session {
            get { return _session; }
            set {
                _session = value;
                if(_session == null) {
                    ObjectList.Items.Clear();
                }
                else {
                    HandleNewSession();
                }
            }
        }

        private bool _async;
        public bool Async {
            get { return _async; }
            set { _async = value; }
        }

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set {
                _options = value;
                if(tools != null) {
                    foreach(ITool tool in tools) {
                        tool.Options = _options;
                    }
                }
            }
        }


        private void HandleNewSession() {
            if(_session == null) {
                _session = new WipeSession();
                return;
            }

            // clear the category
            this.Invoke((MethodInvoker)delegate() {
                ObjectList.Items.Clear();
            });

            // add the new items
            List<IWipeObject> wipeObjects = _session.Items;

            if(wipeObjects != null) {
                if(_async) {
                    int count = wipeObjects.Count;
                    int start = 0;
                    InsertObjectsDelegate del = new InsertObjectsDelegate(InsertObjects);

                    while(count > 0 && !loadStopped) {
                        int objectCount = Math.Min(32, count);
                        this.Invoke(del, wipeObjects, start, objectCount);
                        Thread.Sleep(30);

                        count -= objectCount;
                        start += objectCount;
                    }

                    // task completed
                    if(loadStopped == false) {
                        this.Invoke(new EventHandler(LoadTaskCompleted), null, null);
                    }
                }
                else {
                    ObjectList.BeginUpdate();
                    int count = wipeObjects.Count;

                    for(int i = 0; i < count; i++) {
                        InsertObject(wipeObjects[i]);
                    }

                    ObjectList.EndUpdate();
                }
            }
        }

        private delegate void InsertObjectsDelegate(List<IWipeObject> wipeObjects, int start, int count);

        public void InsertObjects(List<IWipeObject> wipeObjects, int start, int count) {
            if(wipeObjects == null) {
                throw new ArgumentNullException("objects");
            }

            int end = start + count;

            for(int i = start; i < end; i++) {
                UpdateObject(ObjectList.Items.Count, wipeObjects[i]);
            }

            HandleItemNumber();

            if(_async && loadTask != null) {
                loadTask.ProgressValue = (int)(((double)ObjectList.Items.Count / _session.Items.Count) * 100);
            }
        }

        public void InsertObject(IWipeObject wipeObject) {
            Debug.AssertNotNull(wipeObject, "WipeObject is null");

            UpdateObject(ObjectList.Items.Count, wipeObject);
            HandleItemNumber();
        }

        private void LoadTaskCompleted(object sender, EventArgs e) {
            if(loadTask != null) {
                loadTask.Stop();
            }
        }

        private void UpdateStatus() {
            if(OnStatusChanged != null) {
                OnStatusChanged(this, "Item Count: " + ObjectList.Items.Count.ToString());
            }

            _actionManager.StateChanged();
        }

        public void UpdateObject(int index, IWipeObject wipeObject) {
            Debug.AssertNotNull(wipeObject, "WipeObject is null");

            try {
                string[] itemStrings = new string[5];
                long fileSize = 0;
                int imageIndex = 0;

                if(wipeObject.Type == WipeObjectType.File) {
                    FileWipeObject file = (FileWipeObject)wipeObject;

                    try {
                        itemStrings[0] = Path.GetFileNameWithoutExtension(file.Path);
                        itemStrings[1] = Path.GetExtension(file.Path).ToLower();
                        itemStrings[4] = file.Path;
                        FileInfo info = new FileInfo(file.Path);

                        if(info.Length < 1024) {
                            itemStrings[2] = ((int)info.Length).ToString() + " B";
                        }
                        else {
                            itemStrings[2] = ((int)(info.Length / 1024)).ToString() + " KB";
                        }

                        fileSize = info.Length;
                        itemStrings[3] = info.CreationTime.ToString();
                    }
                    catch { }

                    // update the icon
                    imageIndex = 0;
                }
                else if(wipeObject.Type == WipeObjectType.Folder) {
                    FolderWipeObject folder = (FolderWipeObject)wipeObject;

                    try {
                        DirectoryInfo info = new DirectoryInfo(folder.Path);
                        itemStrings[0] = info.Name;
                        itemStrings[3] = info.CreationTime.ToString();
                        itemStrings[4] = folder.Path;
                    }
                    catch {
                        if(itemStrings[0] == null) {
                            itemStrings[0] = "FOLDER NOT FOUND";
                        }
                    }

                    // update the icon
                    imageIndex = 1;
                }
                else if(wipeObject.Type == WipeObjectType.Drive) {
                    DriveWipeObject drive = (DriveWipeObject)wipeObject;

                    try {
                        itemStrings[0] = "Free space ";

                        if(drive.Drives != null && drive.Drives.Count > 0) {
                            for(int i = 0; i < drive.Drives.Count; i++) {
                                itemStrings[0] += drive.Drives[i] + " ";
                            }
                        }
                    }
                    catch { }

                    imageIndex = 2;
                }
                else if(wipeObject.Type == WipeObjectType.Plugin) {
                    itemStrings[0] = "Plugins";
                    imageIndex = 3;
                }

                // update
                if(index == ObjectList.Items.Count) {
                    // add a new ListViewItem
                    ListViewItem item = ObjectList.Items.Add(new ListViewItem(itemStrings, imageIndex));
                    item.SubItems[2].Tag = fileSize;
                    item.Tag = wipeObject;
                }
                else {
                    ListViewItem item = ObjectList.Items[index];
                    item.SubItems.Clear();
                    item.Text = itemStrings[0];
                    item.SubItems.Add(itemStrings[1]);
                    item.SubItems.Add(itemStrings[2]);
                    item.SubItems.Add(itemStrings[3]);
                    item.SubItems.Add(itemStrings[4]);
                    item.SubItems[2].Tag = fileSize;
                    item.Tag = wipeObject;
                }

                if(_scheduleMode) {
                    SaveButton.Enabled = true;
                }
            }
            catch(Exception e) {
                Debug.ReportError("Error while generating WipeObject category member. Exception: {0}", e.Message);
            }
        }

        #endregion

        #region New wipe objects management

        public void AddFile(string file) {
            Debug.AssertNotNull(file, "File is null");

            FileWipeObject fileObject = new FileWipeObject();
            fileObject.Path = file;

            // set default options
            fileObject.WipeMethodId = WipeOptions.DefaultWipeMethod;
            _session.Items.Add(fileObject);
            InsertObject(fileObject);
        }

        public void AddFiles(string[] files) {
            Debug.AssertNotNull(files, "Files is null");

            if(files.Length == 0) {
                return;
            }

            for(int i = 0; i < files.Length; i++) {
                AddFile(files[i]);
            }
        }

        private void AddNewFiles() {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.ShowReadOnly = true;
            dialog.Title = "Add files";
            dialog.ValidateNames = true;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;

            if(dialog.ShowDialog() == DialogResult.OK) {
                AddFiles(dialog.FileNames);
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e) {
            AddNewFiles();
        }

        #endregion

        #region Selected items

        private void ObjectList_SelectedIndexChanged(object sender, EventArgs e) {
            HandleItemSelection(ObjectList.SelectedIndices);
        }

        private void HandleItemSelection(ListView.SelectedIndexCollection indices) {
            if(indices.Count == 0 || indices.Count > 1) {
                EditButton.Enabled = false;
            }
            else {
                EditButton.Enabled = true;

                // if in edit mode, edit the selected element
                if(toolPanelClosed == false && IsEditorTool()) {
                    EditSelectedItem();
                }
            }

            HandleItemNumber();
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            EditSelectedItem();
        }

        private void EditSelectedItem() {
            if(ObjectList.SelectedIndices.Count > 0) {
                //IWipeObject wipeObject = _session.Items[ObjectList.SelectedIndices[0]];
                IWipeObject wipeObject = (IWipeObject)ObjectList.SelectedItems[0].Tag;

                if(wipeObject.Type == WipeObjectType.File) {
                    FileTool fileTool = (FileTool)ChangeTool(ToolType.File, true);
                    fileTool.DisposeTool();
                    fileTool.File = (FileWipeObject)wipeObject;
                    fileTool.ObjectIndex = ObjectList.SelectedIndices[0];
                    fileTool.Options = _options;
                    fileTool.InitializeTool();
                    fileTool.ToolIcon = fileToolStripMenuItem.Image;

                    ChangeToolHeaderText("File Options");
                    ChangeToolHeaderIcon(fileToolStripMenuItem.Image);
                }
                else if(wipeObject.Type == WipeObjectType.Folder) {
                    FolderTool folderTool = (FolderTool)ChangeTool(ToolType.Folder, true);

                    if(folderTool != null) {
                        ChangeToolHeaderText("Folder Options");
                        ChangeToolHeaderIcon(folderToolStripMenuItem.Image);

                        folderTool.DisposeTool();
                        folderTool.ParentControl = this;
                        folderTool.Folder = (FolderWipeObject)wipeObject;
                        folderTool.Options = _options;
                        folderTool.InsertMode = false;
                        folderTool.ObjectIndex = ObjectList.SelectedIndices[0];
                        folderTool.InitializeTool();
                    }
                }
                else if(wipeObject.Type == WipeObjectType.Drive) {
                    FreeSpaceTool freeSpaceTool = (FreeSpaceTool)ChangeTool(ToolType.FreeSpace, true);

                    if(freeSpaceTool != null) {
                        ChangeToolHeaderText("Free Space Options");
                        ChangeToolHeaderIcon(freeSpaceToolStripMenuItem.Image);

                        freeSpaceTool.DisposeTool();
                        freeSpaceTool.ParentControl = this;
                        freeSpaceTool.Drive = (DriveWipeObject)wipeObject;
                        freeSpaceTool.Options = _options;
                        freeSpaceTool.InsertMode = false;
                        freeSpaceTool.ObjectIndex = ObjectList.SelectedIndices[0];
                        freeSpaceTool.InitializeTool();
                    }
                }
                else if(wipeObject.Type == WipeObjectType.Plugin) {
                    PluginTool pluginTool = (PluginTool)ChangeTool(ToolType.Plugin, true);

                    if(pluginTool != null) {
                        ChangeToolHeaderText("Plugin Options");
                        ChangeToolHeaderIcon(pluginsToolStripMenuItem.Image);

                        pluginTool.DisposeTool();
                        pluginTool.ParentControl = this;
                        pluginTool.Plugin = (PluginWipeObject)wipeObject;
                        pluginTool.Options = _options;
                        pluginTool.InsertMode = false;
                        pluginTool.ObjectIndex = ObjectList.SelectedIndices[0];
                        pluginTool.InitializeTool();
                    }
                }
            }
        }

        #endregion

        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveSelected();
        }

        private void RemoveSelected() {
            ListView.SelectedListViewItemCollection selected = ObjectList.SelectedItems;
            int lastIndex = 0;

            if(selected.Count > 0) {
                // delete from the _session
                ObjectList.BeginUpdate();

                while(selected.Count > 0) {
                    lastIndex = selected[0].Index;
                    _session.Items.RemoveAt(selected[0].Index);
                    ObjectList.Items.Remove(selected[0]);
                }

                ObjectList.EndUpdate();

                if(lastIndex < ObjectList.Items.Count) {
                    ObjectList.Items[lastIndex].Selected = true;
                }

                if(_scheduleMode) {
                    SaveButton.Enabled = true;
                }
            }

            HandleItemNumber();
        }

        private void HandleItemNumber() {
            RemoveButton.Enabled = ObjectList.Items.Count > 0;

            if(ObjectList.Items.Count == 0) {
                if(toolPanelClosed == false && IsEditorTool()) {
                    HideToolPanel();
                }
            }

            StartButton.Enabled = ObjectList.Items.Count > 0;
            startToolStripMenuItem.Enabled = ObjectList.Items.Count > 0;
            editSelectedItemToolStripMenuItem.Enabled = ObjectList.Items.Count > 0;
            removeSelectedToolStripMenuItem.Enabled = ObjectList.Items.Count > 0;
            removeAllToolStripMenuItem1.Enabled = ObjectList.Items.Count > 0;
            UpdateStatus();
        }

        private void RemoveAll() {
            _session.Items.Clear();
            ObjectList.Items.Clear();

            if(_scheduleMode) {
                SaveButton.Enabled = true;
            }

            HandleItemNumber();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e) {
            AddNewFiles();
        }

        private void RemoveButton_ButtonClick(object sender, EventArgs e) {
            RemoveSelected();
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveAll();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            ShowSearch();
        }

        public void ShowSearch() {
            SearchTool searchTool = (SearchTool)ChangeTool(ToolType.Search, true);

            if(searchTool != null) {
                if(searchTool.Searching == false) {
                    searchTool.InitializeTool();
                }

                searchTool.ToolIcon = SearchButton.Image;
                searchTool.ParentControl = this;
                ChangeToolHeaderText("Search for files");
                ChangeToolHeaderIcon(SearchButton.Image);
            }
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e) {
            AddFolder();
        }

        private void AddFolder() {
            FolderTool folderTool = (FolderTool)ChangeTool(ToolType.Folder, true);

            if(folderTool != null) {
                ChangeToolHeaderText("Insert Folder");
                ChangeToolHeaderIcon(folderToolStripMenuItem.Image);

                folderTool.DisposeTool();
                folderTool.ParentControl = this;
                folderTool.Folder = new FolderWipeObject();
                folderTool.Folder.WipeMethodId = WipeOptions.DefaultWipeMethod;
                folderTool.Options = _options;
                folderTool.InsertMode = true;
                folderTool.InitializeTool();
            }
        }

        private void freeSpaceToolStripMenuItem_Click(object sender, EventArgs e) {
            AddFreeSpace();
        }

        private void AddFreeSpace() {
            FreeSpaceTool tool = (FreeSpaceTool)ChangeTool(ToolType.FreeSpace, true);

            if(tool != null) {
                ChangeToolHeaderText("Insert Free Space");
                ChangeToolHeaderIcon(freeSpaceToolStripMenuItem.Image);

                tool.DisposeTool();
                tool.ParentControl = this;
                tool.InsertMode = true;
                tool.Drive = new DriveWipeObject();
                tool.Drive.WipeMethodId = WipeOptions.DefaultWipeMethod;
                tool.Options = _options;
                tool.InitializeTool();
            }
        }

        private void AddPlugin() {
            PluginTool tool = (PluginTool)ChangeTool(ToolType.Plugin, true);

            if(tool != null) {
                ChangeToolHeaderText("Insert Plugin");
                ChangeToolHeaderIcon(pluginsToolStripMenuItem.Image);

                tool.DisposeTool();
                tool.ToolIcon = freeSpaceToolStripMenuItem.Image;
                tool.ParentControl = this;
                tool.InsertMode = true;
                tool.Plugin = new PluginWipeObject();

                // load default settings
                string file = SecureDeleteLocations.GetPluginDefaultSettingsFilePath();
                tool.Plugin.PluginSettings = new PluginSettings();

                if(File.Exists(file)) {
                    tool.Plugin.PluginSettings.SettingsFile = file;
                    tool.Plugin.PluginSettings.AutoSave = PluginSettings.AutoSaveMethod.None;
                    tool.Plugin.PluginSettings.LoadSettings();
                }

                tool.Plugin.WipeMethodId = WipeOptions.DefaultWipeMethod;
                tool.Options = _options;
                tool.InitializeTool();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            StartWipe();
        }

        private void StartWipe() {
            if(_session.Items.Count <= 0) {
                return;
            }

            // ask the user before
            if(_options.WarnOnStart) {
                StartWipeDialog dialog = new StartWipeDialog();
                DialogResult result = dialog.ShowDialog();

                // save settings
                if(dialog.DontShow.Checked) {
                    _options.WarnOnStart = false;
                    SDOptionsFile.TrySaveOptions(_options);
                }

                if(result != DialogResult.OK) {
                    // don't wipe
                    return;
                }
            }

            // load the tool
            WipeTool wipeTool = (WipeTool)ChangeTool(ToolType.Wipe, true);

            if(wipeTool != null) {
                ChangeToolHeaderText("Wipe");
                ChangeToolHeaderIcon(StartButton.Image);
                wipeTool.ToolIcon = StartButton.Image;
                wipeTool.Session = _session;
                wipeTool.FooterVisible = true;
                wipeTool.InitializeTool();
                wipeTool.Start();
            }
        }

        public void EnterWipeMode(object sender, EventArgs e) {
            AddButton.Enabled = false;
            RemoveButton.Enabled = false;
            EditButton.Enabled = false;
            SearchButton.Enabled = false;
            StartButton.Enabled = false;
            PauseButton.Enabled = true;
            StopButton.Enabled = true;
            ObjectList.Enabled = false;

            startToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = true;
            pauseToolStripMenuItem.Enabled = true;
            _actionManager.StateChanged();
        }

        public void ExitWipeMode(object sender, EventArgs e) {
            AddButton.Enabled = true;
            RemoveButton.Enabled = true;
            EditButton.Enabled = true;
            SearchButton.Enabled = true;
            StartButton.Enabled = true;
            PauseButton.Enabled = false;
            StopButton.Enabled = false;
            ObjectList.Enabled = true;

            startToolStripMenuItem.Enabled = true;
            stopToolStripMenuItem.Enabled = false;
            pauseToolStripMenuItem.Enabled = false;
            _actionManager.StateChanged();
        }

        private void StopButton_Click(object sender, EventArgs e) {
            StopWipe();
        }

        private void StopWipe() {
            WipeTool wipeTool = (WipeTool)ChangeTool(ToolType.Wipe, true);

            wipeTool.Stop();
        }

        private void pluginsToolStripMenuItem_Click(object sender, EventArgs e) {
            AddPlugin();
        }

        private void SelectAll() {
            for(int i = 0; i < ObjectList.Items.Count; i++) {
                ObjectList.Items[i].Selected = true;
            }
        }


        private void DeselectAll() {
            for(int i = 0; i < ObjectList.Items.Count; i++) {
                ObjectList.Items[i].Selected = false;
            }
        }

        #region Menu

        private void SendMenuAction(MenuAction action) {
            if(OnMenuAction != null) {
                OnMenuAction(this, action);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            SendMenuAction(MenuAction.About);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            SendMenuAction(MenuAction.ExitApplication);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {
            SendMenuAction(MenuAction.Options);
        }

        #endregion

        private void searchFilesToolStripMenuItem_Click(object sender, EventArgs e) {
            ShowSearch();
        }

        private void fileToolStripMenuItem2_Click(object sender, EventArgs e) {
            AddNewFiles();
        }

        private void folderToolStripMenuItem1_Click(object sender, EventArgs e) {
            AddFolder();
        }

        private void freeSpaceToolStripMenuItem1_Click(object sender, EventArgs e) {
            AddFreeSpace();
        }

        private void pluginToolStripMenuItem_Click(object sender, EventArgs e) {
            AddPlugin();
        }

        private void editSelectedItemToolStripMenuItem_Click(object sender, EventArgs e) {
            EditSelectedItem();
        }

        private void removeSelectedToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveSelected();
        }

        private void removeAllToolStripMenuItem1_Click(object sender, EventArgs e) {
            RemoveAll();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            SelectAll();
        }

        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            DeselectAll();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e) {
            StartWipe();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e) {
            StopWipe();
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e) {
            throw new Exception("test");
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveSession();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveSessionAs();
        }

        private void SaveSession() {
            if(sessionFile != null) {
                SaveSessionTo(sessionFile);
            }
            else {
                SaveSessionAs();
            }
        }

        private void SaveSessionAs() {
            CommonDialog dialog = new CommonDialog();
            dialog.Handle = this.Handle;
            dialog.Title = "Save Session";
            dialog.Filter.Add(new FilterEntry("SecureDelete Session", "*.sds"));

            if(dialog.ShowSave()) {
                SaveSessionTo(dialog.FileName);
            }
        }

        private void SaveSessionTo(string path) {
            if(SessionSaver.SaveSession(_session, path) == false) {
                MessageBox.Show("Failed to save _session.", "SecureDelete", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                sessionFile = path;
            }
        }

        private void LoadSession() {
            CommonDialog dialog = new CommonDialog();
            dialog.Handle = this.Handle;
            dialog.Title = "Open Session";
            dialog.Filter.Add(new FilterEntry("SecureDelete Session", "*.sds"));

            if(dialog.ShowOpen()) {
                LoadSessionFrom(dialog.FileName);
            }
        }

        private delegate void LoadSessionDelegate(string path);

        private void OnLoadStopped(object sender, EventArgs e) {
            loadStopped = true;
            loadTask = null;
        }

        public void LoadSessionFrom(string path) {
            if(_async) {
                // register the task notifier
                if(_parentControl != null && _parentControl is MainForm) {
                    MainForm form = _parentControl as MainForm;
                    loadTask = form.RegisterBackgroundTask("Loading Session Status", null,
                                                           ProgressBarStyle.Continuous, 0);
                    loadTask.OnStopped += OnLoadStopped;
                }

                LoadSessionDelegate del = new LoadSessionDelegate(LoadSessionImpl);
                loadStopped = false;
                del.BeginInvoke(path, null, null);
            }
            else {
                LoadSessionImpl(path);
            }
        }

        private void LoadSessionImpl(string path) {
            WipeSession session;

            if(SessionLoader.LoadSession(path, out session) == false) {
                MessageBox.Show("Failed to open _session.", "SecureDelete", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                Session = session;
                sessionFile = path;
            }
        }

        private void CreateNewSession() {
            Session = new WipeSession();
            sessionFile = null;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            LoadSession();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            CreateNewSession();
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e) {
            EditSelectedItem();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveSelected();
        }

        private void SetSortOrder(ObjectSortOrder oldOrder, out ObjectSortOrder newOrder) {
            if(oldOrder == ObjectSortOrder.Ascending) {
                newOrder = ObjectSortOrder.Descending;
            }
            else if(oldOrder == ObjectSortOrder.Descending) {
                newOrder = ObjectSortOrder.None;
            }
            else {
                newOrder = ObjectSortOrder.Ascending;
            }
        }

        private void SetSortImage(ColumnHeader column, ObjectSortOrder order) {
            if(order == ObjectSortOrder.Ascending) {
                column.ImageKey = "up.png";
            }
            else if(order == ObjectSortOrder.Descending) {
                column.ImageKey = "down.png";
            }
            else {
                HideSortImage(column);
            }

            ObjectList.Refresh();
        }

        private void HideSortImage(ColumnHeader column) {
            column.ImageKey = string.Empty;
        }

        private void SortByColumn(ColumnHeader column, int index, ObjectSortElement element) {
            ObjectListComparer comparer = ObjectList.ListViewItemSorter as ObjectListComparer;
            ObjectSortOrder sortOrder = ObjectSortOrder.Ascending;

            if(comparer != null) {
                if(comparer.Element == element) {
                    SetSortOrder(comparer.SortOrder, out sortOrder);
                }
                else {
                    HideSortImage(comparer.Column);
                }
            }

            SetSortImage(column, sortOrder);

            if(sortOrder == ObjectSortOrder.None) {
                ObjectList.ListViewItemSorter = null;
            }
            else {
                ObjectList.ListViewItemSorter = new ObjectListComparer(element, sortOrder, index, column);
            }
        }

        private void ObjectList_ColumnClick(object sender, ColumnClickEventArgs e) {
            ColumnHeader column = ObjectList.Columns[e.Column];

            switch(e.Column) {
                case 0: {
                    SortByColumn(column, e.Column, ObjectSortElement.File);
                    break;
                }
                case 1: {
                    SortByColumn(column, e.Column, ObjectSortElement.Extension);
                    break;
                }
                case 2: {
                    SortByColumn(column, e.Column, ObjectSortElement.Size);
                    break;
                }
                case 3: {
                    SortByColumn(column, e.Column, ObjectSortElement.Date);
                    break;
                }
                case 4: {
                    SortByColumn(column, e.Column, ObjectSortElement.Path);
                    break;
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e) {
            if(OnListSaved != null) {
                OnListSaved(this, null);
            }

            SaveButton.Enabled = false;
        }

        private void WipeModule_EnabledChanged(object sender, EventArgs e) {
            toolStrip1.Enabled = this.Enabled;
            ObjectList.Enabled = this.Enabled;
        }

        private void ObjectList_DoubleClick(object sender, EventArgs e) {
            EditSelectedItem();
        }

        private void importExportSettingsToolStripMenuItem_Click(object sender, EventArgs e) {
            SendMenuAction(MenuAction.ImportExportSettings);
        }
    }

    enum ObjectSortElement {
        File, Extension, Size, Date, Path
    }

    enum ObjectSortOrder {
        Ascending, Descending, None
    }


    class ObjectListComparer : IComparer {
        #region Properties

        private ObjectSortElement _element;
        internal ObjectSortElement Element {
            get { return _element; }
            set { _element = value; }
        }

        private ObjectSortOrder _sortOrder;
        internal ObjectSortOrder SortOrder {
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

        public ObjectListComparer(ObjectSortElement element, ObjectSortOrder sortOrder, int subitemIndex, ColumnHeader column) {
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
                case ObjectSortElement.File: {
                        return (_sortOrder == ObjectSortOrder.Descending ? -1 : 1) * 
                                string.Compare(a.Text, b.Text);
                    }
                case ObjectSortElement.Extension: {
                        return (_sortOrder == ObjectSortOrder.Descending ? -1 : 1) * 
                                string.Compare(a.SubItems[_subitemIndex].Text, b.SubItems[_subitemIndex].Text);
                    }
                case ObjectSortElement.Size: {
                        long sizeA;
                        long sizeB;

                        if((a.SubItems[2].Tag is long) == false) {
                            return 0;
                        }

                        if((b.SubItems[2].Tag is long) == false) {
                            return 0;
                        }

                        sizeA = (long)a.SubItems[2].Tag;
                        sizeB = (long)b.SubItems[2].Tag;
                        return (int)((_sortOrder == ObjectSortOrder.Descending ? -1 : 1) * (sizeA - sizeB));
                    }
                case ObjectSortElement.Date: {
                        DateTime dateA;
                        DateTime dateB;

                        if(DateTime.TryParse(a.SubItems[_subitemIndex].Text, out dateA) == false) {
                            return 0;
                        }

                        if(DateTime.TryParse(b.SubItems[_subitemIndex].Text, out dateB) == false) {
                            return 0;
                        }

                        return (_sortOrder == ObjectSortOrder.Descending ? -1 : 1) * DateTime.Compare(dateA, dateB);
                    }
                case ObjectSortElement.Path: {
                        return (_sortOrder == ObjectSortOrder.Descending ? -1 : 1) * string.Compare(a.SubItems[_subitemIndex].Text, b.SubItems[_subitemIndex].Text);
                    }
            }

            return 0;
        }

        #endregion
    }
}
