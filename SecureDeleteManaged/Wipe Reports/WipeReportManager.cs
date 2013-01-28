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

namespace SecureDelete {
    [Serializable]
    public class ReportCategory {
        public Guid Guid;
        public long ReportIndex;
        public SortedList<long, ReportInfo> Reports;
    }


    [Serializable]
    public class ReportInfo : IComparable<ReportInfo> {
        public Guid Guid;
        public long Index;
        public int FailedObjectCount;
        public int ErrorCount;
        public DateTime CreatedDate;

        #region IComparable<ReportInfo> Members

        public int CompareTo(ReportInfo other) {
            if(CreatedDate == other.CreatedDate) {
                return 0;
            }
            else if(CreatedDate > other.CreatedDate) {
                return -1;
            }

            return 1;
        }

        #endregion
    }


    public class WipeReportManager {
        #region Constants

        private const string DefaultReportExtension = ".sdr";
        private const int DefaultMaximumReportsPerSession = 500;

        #endregion

        #region Fields

        private Dictionary<Guid, ReportCategory> reportCategories;

        #endregion

        #region Constructor

        public WipeReportManager() {
            reportCategories = new Dictionary<Guid, ReportCategory>();
        }

        #endregion

        #region Properties

        public Dictionary<Guid, ReportCategory> Categories {
            get { return reportCategories; }
            set { reportCategories = value; }
        }

        private string _reportExtension = DefaultReportExtension;
        public string ReportExtension {
            get { return _reportExtension; }
            set { _reportExtension = value; }
        }

        private string _reportDirectory;
        public string ReportDirectory {
            get { return _reportDirectory; }
            set { _reportDirectory = value; }
        }

        private int _maximumReportsPerSession = DefaultMaximumReportsPerSession;
        public int MaximumReportsPerSession {
            get { return _maximumReportsPerSession; }
            set { _maximumReportsPerSession = value; }
        }

        private DateTime? _oldestReportDate;
        public DateTime? OldestReportDate {
            get { return _oldestReportDate; }
            set { _oldestReportDate = value; }
        }

        #endregion

        #region Private methods

        private byte[] SerializeReport(WipeReport report) {
            MemoryStream stream = new MemoryStream();

            try {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, report);

                return stream.ToArray();
            }
            catch(Exception e) {
                Debug.ReportError("Error while serializing wipe report. Exception: {0}", e.Message);
                return null;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }


        private WipeReport DeserializeReport(byte[] data) {
            if(data == null) {
                return null;
            }

            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);

            try {
                return (WipeReport)serializer.Deserialize(stream);
            }
            catch(Exception e) {
                Debug.ReportError("Error while deserializing wipe report. Exception: {0}", e.Message);
                return null;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }


        private byte[] SerializeReportCategories(Dictionary<Guid, ReportCategory> categories) {
            MemoryStream stream = new MemoryStream();

            try {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, categories);

                return stream.ToArray();
            }
            catch(Exception e) {
                Debug.ReportError("Error while serializing wipe report. Exception: {0}", e.Message);
                return null;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }


        private Dictionary<Guid, ReportCategory> DeserializeReportCategories(byte[] data) {
            if(data == null) {
                return null;
            }

            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);

            try {
                return (Dictionary<Guid, ReportCategory>)serializer.Deserialize(stream);
            }
            catch(Exception e) {
                Debug.ReportError("Error while deserializing report categories. Exception: {0}", e.Message);
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

        /// <summary>
        /// Verifies if the given report is available
        /// </summary>
        public bool ContainsReport(ReportInfo info) {
            if(reportCategories.ContainsKey(info.Guid)) {
                ReportCategory category = reportCategories[info.Guid];

                if(category.Reports != null) {
                    int count = category.Reports.Count;

                    for(int i = 0; i < count; i++) {
                        if(category.Reports.Values[i].Guid == info.Guid) {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Genereates the path of the report.
        /// </summary>
        private string GetReportPath(ReportInfo info) {
            return Path.Combine(_reportDirectory, info.Guid.ToString() + "-" + info.Index.ToString() + DefaultReportExtension);
        }


        /// <summary>
        /// Removes the report from the disk and from the database.
        /// </summary>
        public bool RemoveReport(ReportInfo info) {
            bool found = false;
            int index = 0;
            bool result = false;

            if(reportCategories.ContainsKey(info.Guid) == false) {
                return true;
            }

            ReportCategory category = reportCategories[info.Guid];

            if(category.Reports != null) {
                // find the report in the category
                int count = category.Reports.Count;

                for(int i = 0; i < count; i++) {
                    if(category.Reports.Values[i] == info) {
                        // found
                        found = true;
                        index = i;
                        break;
                    }
                }

                // found ?
                if(found) {
                    // delete the file
                    string path = GetReportPath(info);

                    try {
                        if(File.Exists(path)) {
                            File.Delete(path);
                            result = true;
                        }
                    }
                    catch(Exception e) {
                        Debug.ReportError("Failed to delete report. Path: {0}, Exception: {1}", path, e.Message);
                        result = false;
                    }

                    // remove from the category
                    category.Reports.RemoveAt(index);
                }
            }

            return result;
        }


        /// <summary>
        /// Removes Reports from the category if there are too many.
        /// </summary>
        /// <remarks>
        /// Older Reports are removed first.
        /// </remarks>
        public bool TrimCategoryExcess(Guid guid) {
            if(_maximumReportsPerSession == 0) {
                return false;
            }

            if(reportCategories.ContainsKey(guid) == false) {
                return false;
            }

            ReportCategory category = reportCategories[guid];

            if(category.Reports != null) {
                while(category.Reports.Count > _maximumReportsPerSession) {
                    int count = category.Reports.Count - _maximumReportsPerSession;

                    if(count > 0) {
                        for(int i = 0; i < count; i++) {
                            category.Reports.Values.RemoveAt(0);
                        }
                    }
                }

                // delete old reports
                if(_oldestReportDate.HasValue) {
                    DateTime date = _oldestReportDate.Value;
                    for(int i = 0; i < category.Reports.Count; i++) {
                        if(category.Reports.Values[i].CreatedDate < date) {
                            RemoveReport(category.Reports.Values[i]);
                            i--;
                        }
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Remove the excess from all categories.
        /// </summary>
        private void TrimAllCategories() {
            if(reportCategories == null) {
                return;
            }

            foreach(KeyValuePair<Guid, ReportCategory> kvp in reportCategories) {
                TrimCategoryExcess(kvp.Value.Guid);
            }
        }


        /// <summary>
        /// Add the report to the database.
        /// </summary>
        public bool AddReport(WipeReport report, out ReportInfo reportInfo) {
            ReportCategory category;

            // if the category is already in the list, use it
            if(reportCategories.ContainsKey(report.SessionGuid)) {
                category = reportCategories[report.SessionGuid];
            }
            else {
                // create a new category
                category = new ReportCategory();
                category.Guid = report.SessionGuid;
                category.Reports = new SortedList<long, ReportInfo>();
                category.ReportIndex = 0;

                // add the new category
                reportCategories.Add(category.Guid, category);
            }

            // create the ReportInfo structure
            ReportInfo info = new ReportInfo();
            info.Guid = category.Guid;

            if(report.FailedObjects != null) {
                info.FailedObjectCount = report.FailedObjects.Count;
            }
            if(report.Errors != null) {
                info.ErrorCount = report.Errors.Count;
            }

            info.CreatedDate = DateTime.Now;
            info.Index = category.ReportIndex++;

            // add the info
            category.Reports.Add(info.Index, info);
            reportInfo = info;
            TrimCategoryExcess(category.Guid);
            return true;
        }


        /// <summary>
        /// Return all Reports matching the given GUID
        /// </summary>
        public ReportInfo[] GetReports(Guid guid) {
            if(reportCategories.ContainsKey(guid)) {
                ReportCategory category = reportCategories[guid];

                if(category.Reports != null && category.Reports.Count > 0) {
                    int count = category.Reports.Count;
                    ReportInfo[] infos = new ReportInfo[count];
                    int position = 0;

                    for(int i = 0; i < count; i++) {
                        infos[position++] = category.Reports.Values[i];
                    }

                    return infos;
                }
            }

            return null;
        }


        /// <summary>
        /// Save the report to disk.
        /// </summary>
        public bool SaveReport(WipeReport report, ReportInfo info) {
            Debug.AssertNotNull(report, "Report is null");
            string path = GetReportPath(info);

            try {
                // create the store
                FileStore.FileStore store = new FileStore.FileStore();
                store.Encrypt = true;
                store.UseDPAPI = true;

                // add the file
                FileStore.StoreFile file = store.CreateFile("report.dat");
                byte[] data = SerializeReport(report);

                if(data == null) {
                    return false;
                }

                // write the file contents
                store.WriteFile(file, data, FileStore.StoreMode.Encrypted);
                return store.Save(path);
            }
            catch(Exception e) {
                Debug.ReportError("Error while saving report. Exception: {0}", e.Message);
                return false;
            }
        }


        /// <summary>
        /// Load the report from disk.
        /// </summary>
        public WipeReport LoadReport(ReportInfo info) {
            string path = GetReportPath(info);

            // check if the file exists
            if(File.Exists(path) == false) {
                Debug.ReportWarning("Report file not found. Path: {0}", path);
                return null;
            }

            try {
                // create the store
                FileStore.FileStore store = new FileStore.FileStore();
                store.Encrypt = true;
                store.UseDPAPI = true;

                // load store
                if(store.Load(path) == false) {
                    Debug.ReportError("Error while loading store from path {0}", path);
                    return null;
                }

                // deserialize
                return DeserializeReport(store.ReadFile("report.dat"));
            }
            catch(Exception e) {
                Debug.ReportError("Error while loading report. Exception: {0}", e.Message);
                return null;
            }
        }


        /// <summary>
        /// Save the report database to disk.
        /// </summary>
        public bool SaveReportCategories(string path) {
            Debug.AssertNotNull(path, "Path is null");

            try {
                // create the store
                FileStore.FileStore store = new FileStore.FileStore();
                store.Encrypt = true;
                store.UseDPAPI = true;

                // add the file
                FileStore.StoreFile file = store.CreateFile("categories.dat");
                byte[] data = SerializeReportCategories(reportCategories);

                if(data == null) {
                    return false;
                }

                // write the file contents
                store.WriteFile(file, data, FileStore.StoreMode.Encrypted);
                return store.Save(path);
            }
            catch(Exception e) {
                Debug.ReportError("Error while saving report. Exception: {0}", e.Message);
                return false;
            }
        }


        /// <summary>
        /// Load the report database from disk.
        /// </summary>
        public bool LoadReportCategories(string path) {
            if(File.Exists(path) == false) {
                Debug.ReportWarning("Report category file not found. Path: {0}", path);
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
                reportCategories = DeserializeReportCategories(store.ReadFile("categories.dat"));

                // check result
                if(reportCategories == null) {
                    // allocate new categories
                    reportCategories = new Dictionary<Guid, ReportCategory>();
                    return false;
                }
                else {
                    TrimAllCategories();
                    return true;
                }
            }
            catch(Exception e) {
                Debug.ReportError("Error while loading report categories. Exception: {0}", e.Message);
                return false;
            }
        }


        /// <summary>
        /// Removes the specified category (including all its Reports).
        /// </summary>
        public void RemoveCategory(Guid guid) {
            if(reportCategories.ContainsKey(guid) == false) {
                return;
            }

            ReportCategory category = reportCategories[guid];

            if(category.Reports != null) {
                while(category.Reports.Values.Count > 0) {
                    RemoveReport(category.Reports.Values[0]);
                }
            }

            // ensure the category is removed
            if(reportCategories.ContainsKey(guid)) {
                reportCategories.Remove(guid);
            }
        }


        /// <summary>
        /// Remove all categories.
        /// </summary>
        public void DestroyDatabase() {
            if(reportCategories.Count == 0) {
                return;
            }

            // get the list of categories
            ReportCategory[] categories = new ReportCategory[reportCategories.Count];
            int position = 0;

            foreach(KeyValuePair<Guid, ReportCategory> kvp in reportCategories) {
                categories[position++] = kvp.Value;
            }

            // remove them
            for(int i = 0; i < position; i++) {
                RemoveCategory(categories[i].Guid);
            }
        }


        /// <summary>
        /// Verifies if the given report exists on disk.
        /// </summary>
        public bool ReportExists(ReportInfo info) {
            return File.Exists(GetReportPath(info));
        }

        #endregion
    }
}
