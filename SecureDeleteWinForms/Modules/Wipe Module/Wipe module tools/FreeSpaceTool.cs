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
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DebugUtils.Debugger;
using SecureDelete.WipeObjects;
using SecureDeleteWinForms.Modules;
using SecureDelete;

namespace SecureDeleteWinForms.WipeTools {
    public partial class FreeSpaceTool : UserControl, ITool {
        #region Shell32

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO {
            public IntPtr hIcon;
            public int iIcon;
            public int dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, out SHFILEINFO psfi, int cbFileInfo, int uFlags);

        public const int SHGFI_TYPENAME = 0x400;
        public const int SHGFI_ICON = 0x100;
        public const int SHGFI_SMALLICON = 0x1;
        public const int SHGFI_SYSICONINDEX = 0x4000;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr handle);

        #endregion

        #region Volume category

        public IntPtr GetFileIconHandle(string file) {
            SHFILEINFO fileInfo = new SHFILEINFO();
            IntPtr result = SHGetFileInfo(file, 0, out fileInfo, Marshal.SizeOf(fileInfo), SHGFI_ICON | SHGFI_SMALLICON | SHGFI_SYSICONINDEX);

            return fileInfo.hIcon;
        }

        public Image GetFileIconImage(IntPtr handle) {
            Image icon = null;

            try {
                icon = Icon.FromHandle(handle).ToBitmap();
                return icon;
            }
            catch {
                return new Bitmap(16, 16); ;
            }
        }

        private enum VolumeFormat {
            FAT, NTFS
        }

        private struct VolumeInfo {
            public char Letter;
            public string Name;
            public IntPtr IconHandle;
            public long TotalSpace;
            public long FreeSpace;
            public VolumeFormat Format;
        }

        private List<VolumeInfo> volumes;

        private string FormatSizeToGB(long value) {
            double gb = (double)value / 1073741824.0;

            return string.Format("{0:F2} GB", gb);
        }

        private string FreeSpacePercentage(long total, long free) {
            double percentage = ((double)free / (double)total) * 100.0;

            return string.Format("{0:F2}", percentage);
        }

        private bool GetVolumeIndex(char letter, out int index) {
            index = -1;

            for(int i = 0; i < volumes.Count; i++) {
                if(volumes[i].Letter == letter) {
                    index = i;
                    return true;
                }
            }

            return false;
        }

        private void DestroyVolumeList() {
            VolumeListView.Items.Clear();
            VolumeImages.Images.Clear();

            while(volumes.Count > 0) {
                DestroyIcon(volumes[0].IconHandle);
                volumes.RemoveAt(0);
            }
        }

        private void PopulateVolumeList() {
            DestroyVolumeList();
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach(DriveInfo info in drives) {
                try {
                    DriveType type = info.DriveType;

                    if(type == DriveType.Fixed || type == DriveType.Removable ||
                        type == DriveType.Unknown) {
                        VolumeInfo volInfo = new VolumeInfo();
                        volInfo.Letter = info.Name[0];
                        volInfo.Name = info.VolumeLabel;
                        volInfo.TotalSpace = info.TotalSize;
                        volInfo.FreeSpace = info.TotalFreeSpace;
                        volInfo.IconHandle = GetFileIconHandle(info.Name);
                        
                        // set the file system format
                        string format = info.DriveFormat;

                        switch(format) {
                            case "NTFS": {
                                volInfo.Format = VolumeFormat.NTFS;
                                break;
                            }
                            default: {
                                volInfo.Format = VolumeFormat.FAT;
                                break;
                            }
                        }

                        // add to the category
                        volumes.Add(volInfo);
                    }
                }
                catch(Exception e) {
                    Debug.ReportError("Failed to load volume info. Volume: {0}, Exception: {1}", info.Name, e.Message);
                }
            }

            // add to the ListView
            VolumeListView.Items.Clear();
            VolumeImages.Images.Clear();
        }

        #endregion

        #region Fields

        private WipeModule wipeModule;
        private int insertCount = 0;

        #endregion

        #region Properties

        public string ModuleName {
            get { return "FreeSpaceTool"; }
        }

        public ToolType Type {
            get { return ToolType.FreeSpace; }
        }

        public int RequiredSize {
            get { return 350; }
        }

        private Image _toolIcon;
        public Image ToolIcon {
            get { return _toolIcon; }
            set { _toolIcon = value; }
        }

        public event EventHandler OnClose;

        private Control _parentControl;
        public Control ParentControl {
            get { return _parentControl; }
            set { _parentControl = value; }
        }

        private DriveWipeObject _drive;
        public DriveWipeObject Drive {
            get { return _drive; }
            set {
                _drive = value;

                // call asynchronously
                MethodInvoker invoker = new MethodInvoker(LoadFromDriveObject);
                invoker.BeginInvoke(null, null);
            }
        }

        private bool _insertMode;
        public bool InsertMode {
            get { return _insertMode; }
            set { _insertMode = value; }
        }

        private int _objectIndex;
        public int ObjectIndex {
            get { return _objectIndex; }
            set { _objectIndex = value; }
        }

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set { _options = value; }
        }

        #endregion

        #region Private methods

        private void LoadFromDriveObject() {
            if(_drive == null) {
                return;
            }

            // create the category of available drives
            PopulateVolumeList();
            VolumeListView.Invoke(new EventHandler(SetVolumes), null, null);
        }

        private void SetVolumes(object sender, EventArgs e) {
            VolumeImages.Images.Clear();

            foreach(VolumeInfo info in volumes) {
                ListViewItem item = new ListViewItem();

                item.Text = info.Letter + ":\\";
                item.SubItems.Add(info.Name);
                item.SubItems.Add(FormatSizeToGB(info.TotalSpace));
                item.SubItems.Add(FormatSizeToGB(info.FreeSpace));
                item.SubItems.Add(FreeSpacePercentage(info.TotalSpace, info.FreeSpace));

                VolumeImages.Images.Add(GetFileIconImage(info.IconHandle));
                item.ImageIndex = VolumeImages.Images.Count - 1;

                VolumeListView.Items.Add(item);
            }

            for(int i = 0; i < _drive.Drives.Count; i++) {
                int index;
                if(GetVolumeIndex(_drive.Drives[i], out index) == true) {
                    VolumeListView.Items[index].Checked = true;
                }
            }
        }

        private void SaveToDriveObject() {
            Debug.AssertNotNull(_drive, "Drive not set");

            _drive.ClearDrives();

            for(int i = 0; i < volumes.Count; i++) {
                if(VolumeListView.Items[i].Checked == true) {
                    _drive.AddDrive(volumes[i].Letter);
                }
            }
        }

        #endregion

        #region Public methods

        public FreeSpaceTool() {
            InitializeComponent();

            volumes = new List<VolumeInfo>();
        }

        public void InitializeTool() {
            Debug.AssertType(_parentControl, typeof(WipeModule), "ParentControl is not a WipeModule object");
            Debug.AssertNotNull(_drive, "Drive not set");

            wipeModule = _parentControl as WipeModule;

            if(_insertMode == false) {
                InsertButton.Text = "Save changes";

                // load the options
                FreeSpaceCheckbox.Checked = _drive.WipeFreeSpace;
                ClusterTipsCheckbox.Checked = _drive.WipeClusterTips;
                MFTCheckbox.Checked = _drive.WipeMFT;
            }
            else {
                InsertButton.Text = "Add Free Space";
                insertCount = 0;
            }

            UpdateMethodInfo();
        }

        public void DisposeTool() {
            DestroyVolumeList();
        }

        #endregion

        private void UpdateMethodInfo() {
            if(_options.MethodManager != null && _options.MethodManager.Methods.Count > 0) {
                MethodChangeButton.Enabled = true;
                WipeMethod m = _options.MethodManager.GetMethod(_drive.WipeMethodId == WipeOptions.DefaultWipeMethod ? 
                                                                _options.WipeOptions.DefaultFreeSpaceMethod : _drive.WipeMethodId);

                if(m != null) {
                    MethodNameLabel.Text = m.Name;

                    if(_drive.WipeMethodId == WipeOptions.DefaultWipeMethod) {
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

        private void InsertButton_Click(object sender, EventArgs e) {
            if(_insertMode == true && insertCount > 0) {
                DriveWipeObject temp = new DriveWipeObject();
                temp.WipeMethodId = _drive.WipeMethodId;

                _drive = temp;
            }

            // get the settings
            SaveToDriveObject();
            _drive.WipeFreeSpace = FreeSpaceCheckbox.Checked;
            _drive.WipeClusterTips = ClusterTipsCheckbox.Checked;
            _drive.WipeMFT = MFTCheckbox.Checked;

            if(_insertMode == true) {
                wipeModule.Session.Items.Add(_drive);
                wipeModule.InsertObject(_drive);

                _drive = new DriveWipeObject();
                insertCount++;
            }
            else {
                wipeModule.UpdateObject(_objectIndex, _drive);
                wipeModule.ObjectList.Select();
                wipeModule.ObjectList.SelectedIndices.Clear();
                wipeModule.ObjectList.SelectedIndices.Add(_objectIndex);
            }
        }

        private void MethodChangeButton_Click(object sender, EventArgs e) {
            Debug.AssertNotNull(_options.MethodManager, "MethodManager not set");

            WipeMethods w = new WipeMethods();
            w.Options = _options;
            w.MethodManager = _options.MethodManager;
            w.SelectedMethod = _options.MethodManager.GetMethodIndex(_drive.WipeMethodId);
            w.ShowSelected = true;

            w.ShowDialog(this);

            if(w.SelectedMethod < 0 || w.SelectedMethod >= _options.MethodManager.Methods.Count) {
                _drive.WipeMethodId = WipeOptions.DefaultWipeMethod;
            }
            else {
                _drive.WipeMethodId = _options.MethodManager.Methods[w.SelectedMethod].Id;
            }
            UpdateMethodInfo();
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            if(OnClose != null) {
                OnClose(this, null);
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            foreach(ListViewItem item in VolumeListView.Items) {
                item.Checked = true;
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            _drive.WipeMethodId = WipeOptions.DefaultWipeMethod;
            UpdateMethodInfo();
        }

        private void AdvancedButton_Click(object sender, EventArgs e) {
            OptionsForm f = new OptionsForm();
            f.StartPanel = OptionsFormStartPanel.Wipe;
            f.Options = _options;

            if(f.ShowDialog() == DialogResult.OK) {
                SDOptionsFile.TrySaveOptions(_options);
            }
        }
    }
}
