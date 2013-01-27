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
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace ShellExtensionClient {
    #region Copy items

    public enum CopyItemType {
        File, Folder
    }

    public interface ICopyItem {
        CopyItemType Type { get; }
        string SourcePath { get; set; }
        string DestinationPath { get; set; }
    }

    public class FileCopyItem : ICopyItem {
        #region ICopyItem Members

        public CopyItemType Type {
            get { return CopyItemType.File; }
        }

        private string _sourcePath;
        public string SourcePath {
            get { return _sourcePath; }
            set { _sourcePath = value; }
        }

        private string _destinationPath;
        public string DestinationPath {
            get { return _destinationPath; }
            set { _destinationPath = value; }
        }

        #endregion

        #region Constructor

        public FileCopyItem() {

        }

        public FileCopyItem(string src, string dest) {
            _sourcePath = src;
            _destinationPath = dest;
        }

        #endregion

        #region Public methods

        public static FileCopyItem MoveToFolder(string source, string destination) {
            FileCopyItem item = new FileCopyItem();

            try {
                item._sourcePath = source;
                item._destinationPath = Path.Combine(destination, Path.GetFileName(source));
            }
            catch {
                return null;
            }

            return item;
        }

        #endregion
    }


    public class FolderCopyItem : ICopyItem {
        #region ICopyItem Members

        public CopyItemType Type {
            get { return CopyItemType.Folder; }
        }

        private string _sourcePath;
        public string SourcePath {
            get { return _sourcePath; }
            set { _sourcePath = value; }
        }

        private string _destinationPath;
        public string DestinationPath {
            get { return _destinationPath; }
            set { _destinationPath = value; }
        }

        #endregion

        #region Properties

        private bool _copySubfolders;
        public bool CopySubfolders {
            get { return _copySubfolders; }
            set { _copySubfolders = value; }
        }

        #endregion

        #region Constructor

        public FolderCopyItem() {

        }

        public FolderCopyItem(string src, string dest) {
            _sourcePath = src;
            _destinationPath = dest;
        }

        public FolderCopyItem(string src, string dest, bool copySubfolders) {
            _sourcePath = src;
            _destinationPath = dest;
            _copySubfolders = copySubfolders;
        }

        #endregion

        #region Public methods

        public static FolderCopyItem MoveToFolder(string source, string destination, bool copySubfolders) {
            FolderCopyItem item = new FolderCopyItem();

            try {
                item._sourcePath = source;
                item._destinationPath = Path.Combine(destination, Path.GetFileName(source));
                item._copySubfolders = copySubfolders;
            }
            catch {
                return null;
            }

            return item;
        }

        #endregion
    }

    #endregion

    public class FileCopyEvent {
        public string SourcePath;
        public string DestinationPath;
        public long TotalSize;
        public long CompletedSize;
        public bool Failed;
    }

    public delegate void FileCopyDelegate(object sender, FileCopyEvent data);
    public delegate void FileCopyStoppedDelegate(object sender, bool aborted);


    public class FileCopy {
        #region Win32 Interop

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName,
                                      CopyProgressRoutine lpProgressRoutine, IntPtr lpData,
                                      ref Int32 pbCancel, CopyFileFlags dwCopyFlags);

        delegate CopyProgressResult CopyProgressRoutine(long TotalFileSize, long TotalBytesTransferred,
                                                        long StreamSize, long StreamBytesTransferred,
                                                        uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason,
                                                        IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);
        enum CopyProgressResult : uint {
            PROGRESS_CONTINUE = 0,
            PROGRESS_CANCEL = 1,
            PROGRESS_STOP = 2,
            PROGRESS_QUIET = 3
        }

        enum CopyProgressCallbackReason : uint {
            CALLBACK_CHUNK_FINISHED = 0x00000000,
            CALLBACK_STREAM_SWITCH = 0x00000001
        }

        [Flags]
        enum CopyFileFlags : uint {
            COPY_FILE_FAIL_IF_EXISTS = 0x00000001,
            COPY_FILE_RESTARTABLE = 0x00000002,
            COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,
            COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008
        }

        #endregion

        #region Fields

        private int stopped;
        private object stoppedLock;
        private string currentSource;
        private string currentDestination;

        #endregion

        #region Properties

        private List<ICopyItem> _items;
        public List<ICopyItem> Items {
            get { return _items; }
            set { _items = value; }
        }

        private bool _overwriteExisting;
        public bool OverwriteExisting {
            get { return _overwriteExisting; }
            set { _overwriteExisting = value; }
        }

        private bool _computeTotalSize;
        public bool ComputeTotalSize {
            get { return _computeTotalSize; }
            set { _computeTotalSize = value; }
        }

        #endregion

        #region Events

        public event FileCopyDelegate OnCopyStarted;
        public event FileCopyDelegate OnCopyCompleted;
        public event FileCopyDelegate OnCopyProgressChanged;
        public event FileCopyDelegate OnTotalSizeComputed;
        public event FileCopyStoppedDelegate OnCopyStopped;

        #endregion

        #region Constructor

        public FileCopy() {
            _items = new List<ICopyItem>();
            stoppedLock = new object();
        }

        #endregion

        #region Private methods

        private bool IsStopped() {
            lock(stoppedLock) {
                return stopped != 0;
            }
        }

        private void SetStopped(bool value) {
            lock(stoppedLock) {
                stopped = (value == false) ? 0 : 1;
            }
        }
        
        private void SendStartMessage(string src, string dest) {
            if(OnCopyStarted != null) {
                FileCopyEvent data = new FileCopyEvent();

                data.SourcePath = src;
                data.DestinationPath = dest;
                OnCopyStarted(this, data);
            }
        }
        
        private void SendCompletedMessage(string src, string dest, bool failed) {
            if(OnCopyCompleted != null) {
                FileCopyEvent data = new FileCopyEvent();

                data.SourcePath = src;
                data.DestinationPath = dest;
                data.Failed = failed;
                OnCopyCompleted(this, data);
            }
        }
        
        private void SendProgressChangedMessage(string src, string dest, long total, long completed) {
            if(OnCopyProgressChanged != null) {
                FileCopyEvent data = new FileCopyEvent();

                data.SourcePath = src;
                data.DestinationPath = dest;
                data.TotalSize = total;
                data.CompletedSize = completed;
                OnCopyProgressChanged(this, data);
            }
        }

        private CopyProgressResult CopyProgressHandler(long total, long transferred, long streamSize,
                                                       long StreamByteTrans, uint dwStreamNumber,
                                                       CopyProgressCallbackReason reason,
                                                       IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData) {
            // notify client
            SendProgressChangedMessage(currentSource, currentDestination, total, transferred);
            return CopyProgressResult.PROGRESS_CONTINUE;
        }

        private void CopyFile(string source, string destination) {
            // send start message
            SendStartMessage(source, destination);

            // set current file
            currentSource = source;
            currentDestination = destination;
            CopyFileFlags flags = CopyFileFlags.COPY_FILE_RESTARTABLE;

            if(_overwriteExisting == false) {
                flags |= CopyFileFlags.COPY_FILE_FAIL_IF_EXISTS;
            }

            if(CopyFileEx(source, destination, new CopyProgressRoutine(CopyProgressHandler),
                          IntPtr.Zero, ref stopped, flags) == false) {
                // operation failed
                SendCompletedMessage(source, destination, true);
            }
            else {
                // operation succeeded
                SendCompletedMessage(source, destination, false);
            }
        }

        private void CopyFolder(string source, string destination, bool subfolders) {
            int count;

            try {
                // check if source exists
                if(Directory.Exists(source) == false) {
                    return;
                }

                // create destination directory
                if(Directory.Exists(destination) == false) {
                    Directory.CreateDirectory(destination);
                }

                // copy subfolders
                if(subfolders) {
                    string[] folders = Directory.GetDirectories(source);

                    count = folders.Length;
                    for(int i = 0; i < count; i++) {
                        string folderName = Path.GetFileName(folders[i]);

                        // new copy subfolder
                        CopyFolder(Path.Combine(source, folderName), 
                                   Path.Combine(destination, folderName), subfolders);
                        if(IsStopped()) {
                            return;
                        }
                    }
                }

                // now copy the files
                string[] files = Directory.GetFiles(source);
                count = files.Length;

                for(int i = 0; i < count; i++) {
                    string file = files[i];
                    CopyFile(file, Path.Combine(destination, Path.GetFileName(file)));

                    if(IsStopped()) {
                        return;
                    }
                }
            }
            catch {
                // TODO: Exception should not be ignored.
            }
        }

        private void ProcessItems() {
            int count = _items.Count;

            for(int i = 0; i < count; i++) {
                ICopyItem item = _items[i];

                if(item.Type == CopyItemType.File) {
                    CopyFile(item.SourcePath, item.DestinationPath);
                }
                else {
                    FolderCopyItem folder = (FolderCopyItem)item;
                    CopyFolder(folder.SourcePath, folder.DestinationPath, folder.CopySubfolders);
                }
            }

            // send final message
            if(OnCopyStopped != null) {
                OnCopyStopped(this, IsStopped());
            }
        }


        private void CopyThread() {
            try {
                ComputeItemsSize();

                if(IsStopped() == false) {
                    ProcessItems();
                }
            }
            catch { }
        }

        #region Total size

        private void ComputeItemsSize() {
            if(_computeTotalSize && OnTotalSizeComputed != null) {
                FileCopyEvent data = new FileCopyEvent();
                data.TotalSize = ComputeSize();
                OnTotalSizeComputed(this, data);
            }
        }


        private long ComputeSize() {
            long size = 0;
            int count = _items.Count;

            for(int i = 0; i < count; i++) {
                ICopyItem item = _items[i];
                string source = item.SourcePath;

                if(item.Type == CopyItemType.File) {
                    try {
                        if(File.Exists(source)) {
                            // add size
                            size += new FileInfo(source).Length;
                        }
                    }
                    catch {
                        // TODO: Exception should not be ignored.
                    }

                    if(IsStopped()) {
                        break;
                    }
                }
                else {
                    FolderCopyItem folder = (FolderCopyItem)item;
                    size += ComputeFolderSize(folder.SourcePath, folder.CopySubfolders);

                    if(IsStopped()) {
                        break;
                    }
                }
            }

            return size;
        }


        private long ComputeFolderSize(string path, bool subfolders) {
            long size = 0;
            int count;

            try {
                // count the file size
                string[] files = Directory.GetFiles(path);
                count = files.Length;

                for(int i = 0; i < count; i++) {
                    string file = files[i];

                    if(File.Exists(file)) {
                        // add size
                        size += new FileInfo(file).Length;
                    }

                    // stop
                    if(IsStopped()) {
                        return size;
                    }
                }

                // add from subfolders
                if(subfolders) {
                    string[] folders = Directory.GetDirectories(path);
                    count = folders.Length;

                    for(int i = 0; i < count; i++) {
                        size += ComputeFolderSize(folders[i], subfolders);

                        // stop
                        if(IsStopped()) {
                            return size;
                        }
                    }
                }
            }
            catch {
                // TODO: Exception should not be ignored.
            }

            return size;
        }

        #endregion

        #endregion

        #region Public methods

        public void Start() {
            ComputeItemsSize();

            if(IsStopped() == false) {
                ProcessItems();
            }
        }


        public void StartAsync() {
            Thread t = new Thread(CopyThread);
            t.Name = "SDCopyThread";
            t.Priority = ThreadPriority.BelowNormal;
            SetStopped(false);
            t.Start();
        }

        public void Stop() {
            SetStopped(true);
        }

        #endregion
    }
}
