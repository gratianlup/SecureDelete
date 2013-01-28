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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DebugUtils.Debugger;
using System.Runtime.Serialization.Formatters.Binary;

namespace SecureDelete.Schedule {
    [Serializable]
    public class HistoryCategory {
        #region Constructor

        public HistoryCategory() {
            Items = new SortedList<DateTime, HistoryItem>();
        }

        #endregion

        public Guid Guid;
        public SortedList<DateTime, HistoryItem> Items;
    }


    public class HistoryManager {
        #region Constructor

        public HistoryManager() {
            _categories = new Dictionary<Guid, HistoryCategory>();
        }

        #endregion

        #region Properties

        private Dictionary<Guid, HistoryCategory> _categories;
        public Dictionary<Guid, HistoryCategory> Categories {
            get { return _categories; }
            set { _categories = value; }
        }

        #endregion

        #region Private methods

        private byte[] SerializeHistory(Dictionary<Guid, HistoryCategory> categories) {
            MemoryStream stream = new MemoryStream();

            try {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, categories);
                return stream.ToArray();
            }
            catch(Exception e) {
                Debug.ReportError("Error while serializing task history. Exception: {0}", e.Message);
                return null;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }


        private Dictionary<Guid, HistoryCategory> DeserializeHistory(byte[] data) {
            if(data == null) {
                return null;
            }

            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);

            try {
                return (Dictionary<Guid, HistoryCategory>)serializer.Deserialize(stream);
            }
            catch(Exception e) {
                Debug.ReportError("Error while deserializing task history. Exception: {0}", e.Message);
                return null;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }

        #endregion

        #region Public methods

        public bool Add(HistoryItem item) {
            if(item == null) {
                throw new ArgumentNullException("item");
            }

            HistoryCategory category = null;

            if(_categories.ContainsKey(item.SessionGuid)) {
                category = _categories[item.SessionGuid];
            }
            else {
                // create new category
                category = new HistoryCategory();
                category.Guid = item.SessionGuid;
                _categories.Add(category.Guid, category);
            }

            // add the item to the category
            try {
                category.Items.Add(item.StartTime, item);
            }
            catch(Exception e) {
                Debug.ReportError("Error while adding history item to category. Exception {0}", e.Message);
                return false;
            }

            return true;
        }


        public IList<HistoryItem> GetItems(Guid guid) {
            if(_categories.ContainsKey(guid)) {
                return _categories[guid].Items.Values;
            }

            return null;
        }


        public void Remove(HistoryItem item) {
            if(_categories.ContainsKey(item.SessionGuid)) {
                HistoryCategory category = _categories[item.SessionGuid];

                if(category.Items.ContainsKey(item.StartTime)) {
                    category.Items.Remove(item.StartTime);
                }
            }
        }


        public void RemoveCategory(Guid guid) {
            if(_categories.ContainsKey(guid)) {
                _categories.Remove(guid);
            }
        }


        public void Clear() {
            _categories = new Dictionary<Guid, HistoryCategory>();
        }


        /// <summary>
        /// Save the history database to disk.
        /// </summary>
        public bool SaveHistory(string path) {
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            try {
                // create the store
                FileStore.FileStore store = new FileStore.FileStore();
                store.Encrypt = true;
                store.UseDPAPI = true;

                // add the file
                FileStore.StoreFile file = store.CreateFile("history.dat");
                byte[] data = SerializeHistory(_categories);

                if(data == null) {
                    return false;
                }

                // write the file contents
                store.WriteFile(file, data, FileStore.StoreMode.Encrypted);
                return store.Save(path);
            }
            catch(Exception e) {
                Debug.ReportError("Error while saving task history. Exception: {0}", e.Message);
                return false;
            }
        }


        /// <summary>
        /// Load the report database from disk.
        /// </summary>
        public bool LoadHistory(string path) {
            // check if the file exists
            if(File.Exists(path) == false) {
                Debug.ReportWarning("History file not found. Path: {0}", path);
                return false;
            }

            try {
                // create the store
                FileStore.FileStore store = new FileStore.FileStore();
                store.Encrypt = true;
                store.UseDPAPI = true;

                // load store
                if(store.Load(path) == false) {
                    Debug.ReportError("Error while loading store from path {0}", path);
                    return false;
                }

                // deserialize
                _categories = DeserializeHistory(store.ReadFile("history.dat"));

                if(_categories == null) {
                    // load failed, allocate history categories
                    _categories = new Dictionary<Guid, HistoryCategory>();
                    return false;
                }

                return true;
            }
            catch(Exception e) {
                Debug.ReportError("Error while loading task history. Exception: {0}", e.Message);
                return false;
            }
        }

        #endregion
    }
}
