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
using SecureDelete.WipeObjects;
using SecureDeleteWinForms.Modules;
using SecureDelete.WipePlugin;
using System.IO;
using SecureDelete;
using DebugUtils.Debugger;
using System.Threading;

namespace SecureDeleteWinForms.WipeTools {
    public partial class PluginTool : UserControl, ITool {
        public PluginTool() {
            InitializeComponent();
            splitContainer1.Panel2Collapsed = true;
            loadingEvent = new ManualResetEvent(true);
        }

        #region Fields

        private PluginManager manager;
        private Plugin activePlugin;
        private WipeModule wipeModule;
        private string pluginFilter;
        private bool onlyInstalled;
        private ManualResetEvent loadingEvent;

        #endregion

        #region ITool Members

        public string ModuleName {
            get { return "PluginTool"; }
        }

        public ToolType Type {
            get { return ToolType.Plugin; }
        }

        public int RequiredSize {
            get { return 350; }
        }

        public void InitializeTool() {
            wipeModule = _parentControl as WipeModule;
            if(_insertMode == false) {
                InsertButton.Text = "Save changes";
            }
            else {
                InsertButton.Text = "Add Plugin";
            }

            MethodInvoker invoker = new MethodInvoker(LoadPlugins);
            invoker.BeginInvoke(null, null);
            UpdateMethodInfo();
        }

        public void DisposeTool() {
            if(manager != null) {
                manager.DestroyAllPlugins();
            }

            activePlugin = null;
        }

        private bool _insertMode;
        public bool InsertMode {
            get { return _insertMode; }
            set { _insertMode = value; }
        }

        private Image _toolIcon;
        public Image ToolIcon {
            get { return _toolIcon; }
            set { _toolIcon = value; }
        }
        public event EventHandler OnClose;

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

        private PluginWipeObject _plugin;
        public PluginWipeObject Plugin {
            get { return _plugin; }
            set { _plugin = value; }
        }

        private int _objectIndex;
        public int ObjectIndex {
            get { return _objectIndex; }
            set { _objectIndex = value; }
        }

        #endregion

        #region Plugin management

        private void LoadPlugins() {
            loadingEvent.WaitOne();
            loadingEvent.Reset();

            // destroy previously loaded plugins
            if(manager != null) {
                manager.DestroyAllPlugins();
            }
            else {
                manager = new PluginManager();
            }

            // load all libraries from the plugin folder
            string[] files = SecureDeleteLocations.GetPluginAssemblies();
            if(files == null) {
                return;
            }

            foreach(string file in files) {
                if(manager.AddPlugins(file) == false) {
                    Debug.ReportError("Failed to load plugins from file {0}", file);
                }
            }

            PluginList.BeginInvoke(new MethodInvoker(PopulatePluginList));
        }


        private void PopulatePluginList() {
            PluginList.Items.Clear();

            // filter settings
            string filter = FilterTextbox.Text.Trim().ToLower();
            bool filterByName = NameCheckbox.Checked && filter.Length > 0;
            bool filterByAuthor = AuthorCheckbox.Checked && filter.Length > 0;
            bool onlyInstalled = InstalledCheckbox.Checked;
            bool onlyEnabled = EnabledCheckbox.Checked;
            int count = manager.Plugins.Count;

            for(int i = 0; i < count; i++) {
                Plugin p = manager.Plugins[i];

                // filter
                if(filterByName && p.Name.ToLower().Contains(filter) == false) {
                    continue;
                }

                if(filterByAuthor && p.HasAuthor && p.Author.ToLower().Contains(filter) == false) {
                    continue;
                }

                if(onlyInstalled && p.PluginObject != null && p.PluginObject.IsApplicationInstalled == false) {
                    continue;
                }

                if(onlyEnabled && PluginEnabled(p.Id) == false) {
                    continue;
                }

                // add to list
                ListViewItem item = new ListViewItem();
                item.Text = p.Name;
                item.SubItems.Add(p.VersionString);
                item.Tag = p.Id;

                if(p.HasAuthor) {
                    item.SubItems.Add(p.Author);
                }

                if(PluginEnabled(p.Id)) {
                    item.Checked = true;
                }

                PluginList.Items.Add(item);
            }

            loadingEvent.Set();
        }


        private Plugin GetPlugin(string name, Guid id) {
            if(manager == null) {
                throw new Exception("Manager not allocated");
            }

            Plugin[] plugins = manager.GetPlugin(name);

            // no plugin with the specified name found
            if(plugins.Length == 0) {
                return null;
            }

            for(int i = 0; i < plugins.Length; i++) {
                if(plugins[i].Id == id) {
                    return plugins[i];
                }
            }

            return null;
        }


        private bool PluginEnabled(Guid id) {
            if(_plugin == null || _plugin.PluginIds == null) {
                return false;
            }

            for(int i = 0; i < _plugin.PluginIds.Count; i++) {
                if(_plugin.PluginIds[i].id == id) {
                    return true;
                }
            }

            return false;
        }


        private void SetPluginState(string name, Guid id, bool enabled) {
            if(enabled == false) {
                if(_plugin.PluginIds == null) {
                    return;
                }

                // remove it from the list
                for(int i = 0; i < _plugin.PluginIds.Count; i++) {
                    if(_plugin.PluginIds[i].id == id) {
                        _plugin.PluginIds.RemoveAt(i);
                        break;
                    }
                }
            }
            else {
                // get the plugin from the manager
                Plugin p = GetPlugin(name, id);

                if(p == null) {
                    return;
                }

                // add the id to the list
                StoreId storeId = PluginSettings.CreateStoreId(p);

                // allocate the list
                if(_plugin.PluginIds == null) {
                    _plugin.PluginIds = new List<StoreId>();
                }

                _plugin.PluginIds.Add(storeId);
            }
        }


        private void SavePluginSettings(Plugin p) {
            _plugin.PluginSettings.SavePluginSettings(p);
        }


        private void SaveDefaultSettings() {
            _plugin.PluginSettings.SettingsFile = SecureDeleteLocations.GetPluginDefaultSettingsFilePath();
            _plugin.PluginSettings.SaveSettings();
        }


        private void SaveSettings() {
            CommonDialog dialog = new CommonDialog();
            dialog.Filter.Add(new FilterEntry("Plugin settings file", "*.sdp"));

            if(dialog.ShowSave()) {
                _plugin.PluginSettings.SettingsFile = dialog.FileName;
                _plugin.PluginSettings.SaveSettings();
            }
        }


        private void LoadDefaultSettings() {
            _plugin.PluginSettings.SettingsFile = SecureDeleteLocations.GetPluginDefaultSettingsFilePath();
            _plugin.PluginSettings.LoadSettings();
        }


        private void LoadSettings() {
            CommonDialog dialog = new CommonDialog();
            dialog.Filter.Add(new FilterEntry("Plugin settings file", "*.sdp"));

            if(dialog.ShowOpen()) {
                _plugin.PluginSettings.SettingsFile = dialog.FileName;
                _plugin.PluginSettings.LoadSettings();
            }

            PluginList.SelectedIndices.Clear();

            if(PluginList.Items.Count > 0) {
                PluginList.SelectedIndices.Add(0);
                PluginList.Select();
            }
        }

        #endregion

        private void PluginList_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if(PluginList.SelectedIndices.Count == 0) {
                    return;
                }

                Plugin p = GetPlugin(PluginList.SelectedItems[0].Text, (Guid)PluginList.SelectedItems[0].Tag);
                activePlugin = p;

                if(p == null) {
                    return;
                }

                NameLabel.Text = p.Name;
                VersionLabel.Text = p.VersionString;
                AuthorLabel.Text = p.Author;
                DescriptionTextbox.Text = p.Description;
                OptionsButton.Enabled = p.PluginObject.HasOptionsDialog;
                activePlugin = p;

                // load icon
                if(p.PluginObject.HasIcon) {
                    object iconObject = p.PluginObject.GetIcon();

                    if(iconObject == null) {
                        Debug.ReportWarning("No icon object returned. Plugin :{0}", activePlugin);
                        return;
                    }

                    if(iconObject is Icon) {
                        PluginIcon.Image = ((Icon)iconObject).ToBitmap();
                    }
                    else if(iconObject is Image) {
                        PluginIcon.Image = (Image)iconObject;
                    }
                    else {
                        // invalid format
                        Debug.ReportWarning("Invalid icon format. Plugin: {0}", activePlugin);
                        return;
                    }
                }
                else {
                    PluginIcon.Image = PluginIcon.ErrorImage;
                }

                // show panel
                splitContainer1.Panel2Collapsed = !toolStripButton1.Checked;
            }
            catch(Exception ex) {
                Debug.ReportError("Exception {0} in plugin {1}", ex.Message, activePlugin);

                if(activePlugin != null) {
                    ShowPluginErrorDialog(activePlugin, ex);
                }
            }
        }

        private void PluginList_ItemChecked(object sender, ItemCheckedEventArgs e) {
            e.Item.BackColor = e.Item.Checked ? Color.LightGreen : Color.FromName("Window");
            SetPluginState(e.Item.Text, (Guid)e.Item.Tag, e.Item.Checked);
        }

        private void loadDefaultToolStripMenuItem_Click(object sender, EventArgs e) {
            LoadDefaultSettings();
        }

        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e) {
            LoadSettings();
        }

        private void saveDefaultToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveDefaultSettings();
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveSettings();
        }

        private void enableAllToolStripMenuItem_Click(object sender, EventArgs e) {
            for(int i = 0; i < PluginList.Items.Count; i++) {
                PluginList.Items[i].Checked = true;
            }
        }

        private void disableAllToolStripMenuItem_Click(object sender, EventArgs e) {
            for(int i = 0; i < PluginList.Items.Count; i++) {
                PluginList.Items[i].Checked = false;
            }
        }

        private void UpdateMethodInfo() {
            if(_options.MethodManager != null && _options.MethodManager.Methods.Count > 0) {
                MethodChangeButton.Enabled = true;
                WipeMethod m = _options.MethodManager.GetMethod(_plugin.WipeMethodId == WipeOptions.DefaultWipeMethod ? 
                                                                _options.WipeOptions.DefaultFileMethod : _plugin.WipeMethodId);

                if(m != null) {
                    MethodNameLabel.Text = m.Name;

                    if(_plugin.WipeMethodId == WipeOptions.DefaultWipeMethod) {
                        MethodNameLabel.Text += " (default)";
                    }
                }
                else {
                    MethodNameLabel.Text = "Wipe method not found";
                }
            }
            else {
                MethodNameLabel.Text = "No wipe method available";
            }
        }


        private void MethodChangeButton_Click(object sender, EventArgs e) {
            Debug.AssertNotNull(_options.MethodManager, "MethodManager not set");

            WipeMethods w = new WipeMethods();
            w.Options = _options;
            w.MethodManager = _options.MethodManager;
            w.SelectedMethod = _options.MethodManager.GetMethodIndex(_plugin.WipeMethodId);
            w.ShowSelected = true;
            w.ShowDialog(this);

            if(w.SelectedMethod < 0 || w.SelectedMethod >= _options.MethodManager.Methods.Count) {
                _plugin.WipeMethodId = WipeOptions.DefaultWipeMethod;
            }
            else {
                _plugin.WipeMethodId = _options.MethodManager.Methods[w.SelectedMethod].Id;
            }

            UpdateMethodInfo();
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            if(OnClose != null) {
                OnClose(this, null);
            }
        }

        private void InsertButton_Click(object sender, EventArgs e) {
            if(_insertMode == true) {
                wipeModule.Session.Items.Add(_plugin);
                wipeModule.InsertObject(_plugin);
            }
            else {
                wipeModule.UpdateObject(_objectIndex, _plugin);
                wipeModule.ObjectList.Select();
                wipeModule.ObjectList.SelectedIndices.Clear();
                wipeModule.ObjectList.SelectedIndices.Add(_objectIndex);
            }
        }

        private void InstalledCheckbox_CheckedChanged(object sender, EventArgs e) {
            onlyInstalled = InstalledCheckbox.Checked;
            PopulatePluginList();
        }

        private void button1_Click(object sender, EventArgs e) {
            _plugin.WipeMethodId = WipeOptions.DefaultWipeMethod;
            UpdateMethodInfo();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            PopulatePluginList();

            if(FilterTextbox.Text.Length > 0 && PluginList.Items.Count == 0) {
                FilterTextbox.BackColor = Color.Tomato;
            }
            else {
                FilterTextbox.BackColor = Color.FromName("Window");
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            FilterTextbox.Text = "";
        }

        private void ShowPluginErrorDialog(Plugin plugin, Exception e) {
            PluginError errorDialog = new PluginError();
            errorDialog.Plugin = plugin;
            errorDialog.Exception = e;

            errorDialog.ShowDialog();
        }

        private void OptionsButton_Click(object sender, EventArgs e) {
            try {
                if(activePlugin != null && activePlugin.PluginObject.HasOptionsDialog) {
                    // load the settings
                    _plugin.PluginSettings.LoadPluginSettings(activePlugin);
                    object dialogObject = activePlugin.PluginObject.GetOptionsDialog();

                    if(dialogObject == null) {
                        Debug.ReportWarning("Error while getting options dialog for plugin {0}", activePlugin);
                        MessageBox.Show("Options dialog not available.", "SecureDelete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if((dialogObject is Form) == false) {
                        Debug.ReportWarning("Options dialog not a form object. Plugin: {0}", activePlugin);
                        MessageBox.Show("Options dialog not available.", "SecureDelete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Form dialog = dialogObject as Form;
                    dialog.StartPosition = FormStartPosition.CenterParent;
                    dialog.FormBorderStyle = FormBorderStyle.FixedSingle;
                    dialog.MaximizeBox = false;
                    dialog.MinimizeBox = false;
                    dialog.ShowInTaskbar = false;

                    if(dialog.ShowDialog(this) == DialogResult.OK) {
                        // save (default) settings
                        _plugin.PluginSettings.SettingsFile = SecureDeleteLocations.GetPluginDefaultSettingsFilePath();
                        _plugin.PluginSettings.SavePluginSettings(activePlugin);
                    }
                }
            }
            catch(Exception ex) {
                Debug.ReportError("Exception {0} in plugin {1}", ex.Message, activePlugin);
                ShowPluginErrorDialog(activePlugin, ex);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e) {
            SearchPanel.Visible = toolStripButton4.Checked;
            SearchPanel.BringToFront();
            PluginList.BringToFront();
            toolStripButton4.Image = toolStripButton4.Checked ? SecureDeleteWinForms.Properties.Resources.up1 :
                                                                SecureDeleteWinForms.Properties.Resources.down1;
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            splitContainer1.Panel2Collapsed = !toolStripButton1.Checked;
        }

        private void NameCheckbox_CheckedChanged(object sender, EventArgs e) {
            PopulatePluginList();
        }

        private void AuthorCheckbox_CheckedChanged(object sender, EventArgs e) {
            PopulatePluginList();
        }

        private void InstalledCheckbox_CheckedChanged_1(object sender, EventArgs e) {
            PopulatePluginList();
        }

        private void EnabledCheckbox_CheckedChanged(object sender, EventArgs e) {
            PopulatePluginList();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            ResetSettings();
        }

        private void ResetSettings() {
            if(_plugin != null) {
                _plugin.PluginSettings.RemoveAllSettings();
            }
        }

        private void inverseSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
            for(int i = 0; i < PluginList.Items.Count; i++) {
                PluginList.Items[i].Checked = !PluginList.Items[i].Checked;
            }
        }
    }
}
