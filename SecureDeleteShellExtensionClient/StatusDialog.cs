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
using System.Runtime.InteropServices;
using SecureDelete;
using SecureDeleteWinForms;
using SecureDeleteWinForms.Modules;
using SecureDelete.WipeObjects;
using System.IO;

namespace ShellExtensionClient {
    public partial class StatusDialog : Form, IModule {
        #region Win32 Interop

        const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        const UInt32 SECTION_QUERY = 0x0001;
        const UInt32 SECTION_MAP_WRITE = 0x0002;
        const UInt32 SECTION_MAP_READ = 0x0004;
        const UInt32 SECTION_MAP_EXECUTE = 0x0008;
        const UInt32 SECTION_EXTEND_SIZE = 0x0010;
        const UInt32 SECTION_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SECTION_QUERY |
                                           SECTION_MAP_WRITE |
                                           SECTION_MAP_READ |
                                           SECTION_MAP_EXECUTE |
                                           SECTION_EXTEND_SIZE);
        const UInt32 FILE_MAP_ALL_ACCESS = SECTION_ALL_ACCESS;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint
           dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow,
           IntPtr dwNumberOfBytesToMap);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        const int STANDARD_OPERATION = 1;
        const int MOVE_OPERATION = 2;
        const int RECYCLE_BIN_OPERATION = 4;

        const int OBJECT_TYPE_FILE = 1;
        const int OBJECT_TYPE_FOLDER = 2;
        const int OBJECT_TYPE_DRIVE = 3;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SDHeader {
            public int Size;
            public int OperationType;
            public int ObjectCount;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string data1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string data2;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SDObject {
            public int Size;
            public int PathOffset;
            public int ObjectType;
            public int PathLength;
        }

        #endregion

        #region Fields

        private bool wiping;
        private long totalSize;
        private long completedSize;
        private long currentSize;
        private WipeSession session;
        private FileCopy fileCopy;
        private int operationType;
        private string moveFolder;
        private bool completed;

        #endregion

        #region Constructor

        public StatusDialog() {
            InitializeComponent();
            session = new WipeSession();
            fileCopy = new FileCopy();
        }

        #endregion

        #region Private methods

        private unsafe void StatusDialog_Load(object sender, EventArgs e) {
            // read data from file mapping
            long handle;
            string[] args = Environment.GetCommandLineArgs();

            if(args.Length < 2) {
                this.Close();
            }

            if(long.TryParse(args[1], out handle) == false) {
                this.Close();
            }

            IntPtr view = MapViewOfFile((IntPtr)handle, SECTION_MAP_READ, 0, 0, IntPtr.Zero);
            SDHeader header;
            int position = 0;

            // error
            if(view == IntPtr.Zero) {
                this.Close();
                return;
            }

            try {
                header = (SDHeader)Marshal.PtrToStructure(view, typeof(SDHeader));
                operationType = header.OperationType;
                position += header.Size;

                // move operation, get drop folder
                if(operationType == MOVE_OPERATION) {
                    moveFolder = header.data1;
                }

                // get wipe objects
                for(int i = 0; i < header.ObjectCount; i++) {
                    string path;
                    SDObject obj = (SDObject)Marshal.PtrToStructure((IntPtr)(view.ToInt64() + position), typeof(SDObject));
                    path = Marshal.PtrToStringUni((IntPtr)(view.ToInt64() + position + obj.PathOffset));
                    position += obj.Size;

                    if(obj.ObjectType == OBJECT_TYPE_FILE) {
                        FileWipeObject file = new FileWipeObject();
                        file.Path = path;
                        file.WipeMethodId = WipeOptions.DefaultWipeMethod;

                        // add to wipe list
                        session.Items.Add(file);

                        if(operationType == MOVE_OPERATION) {
                            // add to copy list
                            FileCopyItem item = FileCopyItem.MoveToFolder(path, moveFolder);

                            if(item != null) {
                                fileCopy.Items.Add(item);
                            }
                        }
                    }
                    else if(obj.ObjectType == OBJECT_TYPE_FOLDER) {
                        FolderWipeObject folder = new FolderWipeObject();
                        folder.Path = path;
                        folder.DeleteFolders = true;
                        folder.WipeSubfolders = true;
                        folder.UseMask = false;
                        folder.UseRegex = false;
                        folder.WipeMethodId = WipeOptions.DefaultWipeMethod;
                        session.Items.Add(folder);

                        if(operationType == MOVE_OPERATION) {
                            // add to copy list
                            FolderCopyItem item = FolderCopyItem.MoveToFolder(path, moveFolder, true);

                            if(item != null) {
                                fileCopy.Items.Add(item);
                            }
                        }
                    }
                }
            }
            finally {
                // cleanup
                UnmapViewOfFile(view);
                CloseHandle((IntPtr)handle);
            }

            // start
            if(operationType == MOVE_OPERATION) {
                // copy data first
                StartCopy();
            }
            else {
                // start wipe directly
                StartWipe();
            }
        }

        private void StartCopy() {
            fileCopy.OnCopyStarted += CopyStartedHandler;
            fileCopy.OnCopyProgressChanged += CopyProgressHandler;
            fileCopy.OnCopyCompleted += CopyCompletedHandler;
            fileCopy.OnTotalSizeComputed += CopyTotalSizeHandler;
            fileCopy.OnCopyStopped += CopyStoppedHandler;
            fileCopy.OverwriteExisting = true;
            fileCopy.ComputeTotalSize = true;
            fileCopy.StartAsync();
        }

        private void CopyStartedHandler(object sender, FileCopyEvent data) {
            this.Invoke(new EventHandler(CopyStartedUpdater), data, null);
        }

        private void CopyCompletedHandler(object sender, FileCopyEvent data) {
            completedSize += currentSize;
        }

        private void CopyStartedUpdater(object sender, EventArgs e) {
            if(sender is FileCopyEvent) {
                FileCopyEvent data = (FileCopyEvent)sender;
                CopyStatus.SecondaryText = "From: " + data.SourcePath;
                CopyStatus.SizeText = "To:    " + data.DestinationPath;
                CopyStatus.StepText = "";
                CopyStatus.ProgressValue = 0;
                CopyStatus.ProgressText = "0 %";
            }
        }

        private void CopyProgressHandler(object sender, FileCopyEvent data) {
            this.Invoke(new EventHandler(CopyProgressUpdater), data, null);
            currentSize = data.TotalSize;
        }

        private void CopyProgressUpdater(object sender, EventArgs e) {
            if(sender is FileCopyEvent) {
                FileCopyEvent data = (FileCopyEvent)sender;
                double progress = Math.Max(0, Math.Min(100.0, ((double)data.CompletedSize / (double)data.TotalSize) * 100.0));

                CopyStatus.ProgressText = string.Format("{0:f2} %", progress);
                CopyStatus.ProgressValue = (int)Math.Ceiling(progress);

                // total
                if(totalSize != 0) {
                    long copied = completedSize + data.CompletedSize;
                    progress = Math.Max(0, Math.Min(100.0, ((double)copied / (double)totalSize) * 100.0));
                    TotalProgressbar.Value = (int)Math.Ceiling(progress);
                    PercentLabel.Text = string.Format("{0:f2} %", progress);
                }
            }
        }

        private void CopyTotalSizeHandler(object sender, FileCopyEvent data) {
            totalSize = data.TotalSize;
        }

        private void CopyStoppedHandler(object sender, bool aborted) {
            // start wiping (if not stopped)
            if(aborted == false) {
                this.Invoke(new MethodInvoker(StartWipe));
            }
        }

        private void StartWipe() {
            reportTool.Visible = false;
            CopyPanel.Visible = false;
            wipeTool.Visible = true;

            // load options
            SDOptions options = new SDOptions();
            SDOptionsFile.TryLoadOptions(out options);

            // initialize wiping tool
            wipeTool.ParentControl = this;
            wipeTool.OnWipeStarted += EnterWipeMode;
            wipeTool.OnWipeStopped += ExitWipeMode;
            wipeTool.InitializeTool();
            wipeTool.Session = session;
            wipeTool.Options = options;
            wiping = true;
            wipeTool.Start();
        }

        private void EnterWipeMode(object sender, EventArgs e) {
            wiping = true;
        }

        private void ExitWipeMode(object sender, EventArgs e) {
            wiping = false;
            completed = true;
            StopButton.Text = "Close";
        }

        #endregion

        #region IModule Members

        public ModuleActionManager ActionManager {
            get { return null; }
        }

        public void Activated() {

        }

        public MenuStrip Menu {
            get { return null; }
        }

        public string ModuleName {
            get { return null; }
        }

        public event MenuActionDelegeate OnMenuAction;
        public event ModuleStatusDelegate OnStatusChanged;

        public SDOptions Options {
            get { return null; }
            set { }
        }

        public Control ParentControl {
            get { return null; }
            set { }
        }

        public ITool ChangeTool(ToolType type, bool setSize) {
            if(type == ToolType.Report) {
                reportTool.Visible = true;
                wipeTool.Visible = false;
                FooterPanel.Visible = false;
                wiping = false;

                return reportTool;
            }
            else {
                reportTool.Visible = false;
                wipeTool.Visible = true;
                FooterPanel.Visible = true;
                wiping = true;

                return wipeTool;
            }
        }

        public void ChangeToolHeaderText(string text) {
            this.Text = text;
        }

        public void ChangeToolHeaderIcon(Image icon) {
            // do nothing
        }

        #endregion

        private void StopButton_Click(object sender, EventArgs e) {
            if(StopOperation() && !wiping) {
                this.Close();
            }
        }

        private bool StopOperation() {
            if(completed) {
                return true;
            }

            if(ShouldStop()) {
                if(wiping) {
                    return StopWipe();
                }
                else {
                    // stop copy operation
                    if(operationType == MOVE_OPERATION && fileCopy != null) {
                        fileCopy.Stop();
                    }

                    this.Close();
                    return true;
                }
            }

            // not stopped
            return false;
        }

        private bool ShouldStop() {
            return MessageBox.Show("Do you really want to stop the wipe process ?", "SecureDelete", 
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
        }

        private bool StopWipe() {
            wipeTool.Stop();
            return true;
        }

        private void StatusDialog_FormClosing(object sender, FormClosingEventArgs e) {
            if(StopOperation() == false) {
                // operation not stopped, don't close window
                e.Cancel = true;
            }
        }

        private void OntopCheckbox_CheckedChanged(object sender, EventArgs e) {
            this.TopMost = OntopCheckbox.Checked;
        }
    }
}
