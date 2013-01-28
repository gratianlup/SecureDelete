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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Threading;
using SecureDelete.FileSearch;

namespace SecureDelete.WipeObjects {
    [Serializable]
    public class FolderWipeObject : IWipeObject {
        #region Nested types

        class IntComparer : IComparer<int> {
            #region IComparer<int> Members

            public int Compare(int x, int y) {
                return y - x;
            }

            #endregion
        }

        #endregion

        #region Fields

        private bool stopped;

        [NonSerialized]
        private ManualResetEvent searchCompleted;

        [NonSerialized]
        private List<string> files;

        #endregion

        #region IWipeObject Members

        public WipeObjectType Type {
            get { return WipeObjectType.Folder; }
        }


        public bool SingleObject {
            get { return _fileFilter == null || (_fileFilter != null && _fileFilter.FilterCount == 0); }
        }

        private WipeOptions _options;
        public WipeOptions Options {
            get { return _options; }
            set { _options = value; }
        }

        /// <summary>
        /// Get the folder object. SingleObject should be set to true.
        /// </summary>
        public NativeMethods.WObject GetObject() {
            if(SingleObject == false) {
                throw new Exception("GetObjects should be called instead");
            }

            NativeMethods.WObject folder = new NativeMethods.WObject();
            folder.type = NativeMethods.TYPE_FOLDER;
            folder.path = _path;
            folder.aux = _mask;
            folder.wipeMethod = _wipeMethodId == WipeOptions.DefaultWipeMethod ? 
                                _options.DefaultFileMethod : _wipeMethodId;
            folder.wipeOptions = 0;

            // set the options
            if(_useMask) {
                folder.wipeOptions |= NativeMethods.USE_MASK;
            }

            if(_wipeSubfolders) {
                folder.wipeOptions |= NativeMethods.WIPE_SUBFOLDERS;
            }

            if(_deleteFolders) {
                folder.wipeOptions |= NativeMethods.DELETE_FOLDERS;
            }

            if(_options.WipeAds) {
                folder.wipeOptions |= NativeMethods.WIPE_ADS;
            }

            if(_options.WipeFileName) {
                folder.wipeOptions |= NativeMethods.WIPE_FILENAME;
            }

            return folder;
        }

        private int GetFolderLevel(string path) {
            int level = 1;

            if(path == null || path.Length < 3) {
                return 0;
            }

            int length = path.Length - 1;

            for(int i = 3; i < length; i++) {
                if(path[i] == '\\') {
                    level++;
                }
            }

            return level;
        }


        private void FileSearchHandler(object sender, FileSearchArgs e) {
            // add files to list
            if(e.files != null) {
                files.AddRange(e.files);
            }

            if(e.lastSet) {
                // search completed
                searchCompleted.Set();
            }
        }


        /// <summary>
        /// Get the file (and folder) objects. SingleObject should be set to false.
        /// </summary>
        public NativeMethods.WObject[] GetObjects() {
            if(SingleObject == true) {
                throw new Exception("GetObject should be called instead");
            }

            if(_fileFilter == null || _searcher == null) {
                return null;
            }

            try {
                // initialize file searcher
                stopped = false;
                searchCompleted = new ManualResetEvent(false);
                _searcher.OnFilesFound += FileSearchHandler;

                files = new List<string>();
                List<NativeMethods.WObject> items = new List<NativeMethods.WObject>();

                // first add the files
                _searcher.FileFilter = _fileFilter;
                _searcher.SearchFilesAsync(_path, _useMask ? _mask : null, _useRegex, _wipeSubfolders);
                searchCompleted.WaitOne();

                if(stopped) {
                    return null;
                }

                // convert the files to file objects
                int filesCount = files.Count;

                if(filesCount > 0) {
                    SortedDictionary<int, List<string>> folderTable = new SortedDictionary<int, List<string>>(new IntComparer());
                    int method = _wipeMethodId == WipeOptions.DefaultWipeMethod ? _options.DefaultFileMethod : _wipeMethodId;

                    for(int i = 0; i < filesCount; i++) {
                        if(stopped) {
                            return null;
                        }

                        NativeMethods.WObject file = new NativeMethods.WObject();
                        file.type = NativeMethods.TYPE_FILE;
                        file.path = files[i];
                        file.wipeMethod = method;
                        file.wipeOptions = 0;

                        // set the options
                        if(_options.WipeAds) {
                            file.wipeOptions |= NativeMethods.WIPE_ADS;
                        }

                        if(_options.WipeFileName) {
                            file.wipeOptions |= NativeMethods.WIPE_FILENAME;
                        }

                        items.Add(file);

                        if(_deleteFolders) {
                            string directoryPath = System.IO.Path.GetDirectoryName(files[i]);
                            int level = GetFolderLevel(directoryPath);
                            List<string> list;

                            if(folderTable.ContainsKey(level)) {
                                list = folderTable[level];
                            }
                            else {
                                list = new List<string>();
                                folderTable.Add(level, list);
                            }

                            if(list.Contains(directoryPath) == false) {
                                list.Add(directoryPath);
                            }
                        }
                    }

                    // add the folders
                    if(_deleteFolders) {
                        // add empty folders
                        string[] folders = Directory.GetDirectories(_path, "*", SearchOption.AllDirectories);

                        if(folders != null && folders.Length > 0) {
                            int length = folders.Length;
                            int level;

                            for(int i = 0; i < length; i++) {
                                if(stopped) {
                                    return null;
                                }

                                level = GetFolderLevel(folders[i]);

                                // check if the folder is in the list
                                List<string> list = null;

                                if(folderTable.ContainsKey(level)) {
                                    list = folderTable[level];
                                }

                                if(list == null || list.Contains(folders[i]) == false) {
                                    // add only empty folders
                                    if(Directory.GetFiles(folders[i]).Length == 0) {
                                        // allocate the list
                                        if(list == null) {
                                            list = new List<string>();
                                            folderTable.Add(level, list);
                                        }

                                        list.Add(folders[i]);
                                    }
                                }
                            }
                        }

                        // add the base folder
                        folderTable.Add(0, new List<string>(new string[] { _path }));

                        int levelCount = folderTable.Count;
                        foreach(KeyValuePair<int, List<string>> kvp in folderTable) {
                            // wipe stopped
                            if(stopped) {
                                return null;
                            }

                            List<string> list = kvp.Value;

                            int listCount = list.Count;
                            for(int i = 0; i < listCount; i++) {
                                NativeMethods.WObject folder = new NativeMethods.WObject();

                                folder.type = NativeMethods.TYPE_FOLDER;
                                folder.path = list[i];
                                folder.aux = "|";
                                folder.wipeMethod = _wipeMethodId;
                                folder.wipeOptions = NativeMethods.DELETE_FOLDERS | NativeMethods.USE_MASK;

                                items.Add(folder);
                            }
                        }
                    }

                    return items.ToArray();
                }
            }
            catch {
                // Ignore error.
            }
            finally {
                // cleanup
                searchCompleted = null;
                files = null;

                if(_searcher != null) {
                    _searcher.OnFilesFound -= FileSearchHandler;
                }
            }

            return null;
        }


        public void Stop() {
            stopped = true;

            // stop searching
            if(_searcher != null) {
                _searcher.Stop();
            }
        }

        #endregion

        #region Properties

        private string _path;
        public string Path {
            get { return _path; }
            set { _path = value; }
        }

        private string _mask;
        public string Mask {
            get { return _mask; }
            set { _mask = value; }
        }

        private int _wipeMethodId;
        public int WipeMethodId {
            get { return _wipeMethodId; }
            set { _wipeMethodId = value; }
        }

        private FileFilter _fileFilter;
        public FileFilter FileFilter {
            get { return _fileFilter; }
            set { _fileFilter = value; }
        }

        [NonSerialized]
        private FileSearch.FileSearcher _searcher;
        public FileSearch.FileSearcher Searcher {
            get { return _searcher; }
            set { _searcher = value; }
        }

        private bool _useMask;
        public bool UseMask {
            get { return _useMask; }
            set { _useMask = value; }
        }

        private bool _useRegex;
        public bool UseRegex {
            get { return _useRegex; }
            set { _useRegex = value; }
        }

        private bool _wipeSubfolders;
        public bool WipeSubfolders {
            get { return _wipeSubfolders; }
            set { _wipeSubfolders = value; }
        }

        private bool _deleteFolders;
        public bool DeleteFolders {
            get { return _deleteFolders; }
            set { _deleteFolders = value; }
        }

        #endregion
    }
}
