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
using System.Threading;
using System.Runtime.Serialization;
using DebugUtils.Debugger;
using SecureDelete.WipeObjects;
using SecureDelete.FileSearch;
using SecureDelete.WipePlugin;
using SecureDelete.Actions;

namespace SecureDelete {
    public enum SessionStatus {
        BeginStart,
        Wipe,
        Paused,
        Stopped,
        BeginActions,
        EndActions
    }


    [Serializable]
    public class WipeSession {
        #region Constants

        public static readonly Guid DefaultSessionGuid = new Guid("{019D8701-9A7F-4f22-8456-790940656CB8}");

        #endregion

        #region Fields

        private Guid guid;
        private List<IWipeObject> items;

        [NonSerialized]
        private WipeContext context;
        private WipeOptions options;

        [NonSerialized]
        private FileSearcher searcher;

        [NonSerialized]
        private SessionStatus status;

        [NonSerialized]
        private object startLock;

        [NonSerialized]
        private AutoResetEvent startEvent;

        [NonSerialized]
        private bool asyncStartResult;

        [NonSerialized]
        private bool endStartCalled;

        [NonSerialized]
        private WipeStatistics statistics;

        [NonSerialized]
        private IAction activeAction;

        [NonSerialized]
        private IWipeObject activeObject;

        #endregion

        #region Constructor

        public WipeSession() {
            guid = DefaultSessionGuid;
            items = new List<IWipeObject>();
            context = new WipeContext();
            options = new WipeOptions();
            searcher = new FileSearcher();
            startLock = new object();
            startEvent = new AutoResetEvent(false);
            statistics = new WipeStatistics();
            _beforeWipeErrors = new List<WipeError>();
            _afterWipeErrors = new List<WipeError>();
            _bridgeItems = new List<IWipeObject>();
            endStartCalled = true;
            status = SessionStatus.Stopped;
        }

        #endregion

        #region Properties

        public List<IWipeObject> Items {
            get { return items; }
            set { items = value; }
        }

        private List<IWipeObject> _bridgeItems;
        public List<IWipeObject> BridgeItems {
            get { return _bridgeItems; }
            set { _bridgeItems = value; }
        }

        public WipeContext Context {
            get { return context; }
        }

        public WipeOptions Options {
            get { return options; }
            set { options = value; }
        }

        public Guid SessionId {
            get { return guid; }
        }

        public WipeStatistics Statistics {
            get { return statistics; }
        }

        private bool _initialized;
        public bool Initialized {
            get { return _initialized; }
        }

        private List<string> _pluginAssemblies;
        public List<string> PluginAssemblies {
            get { return _pluginAssemblies; }
            set { _pluginAssemblies = value; }
        }

        public SessionStatus Status {
            get { return status; }
        }

        [NonSerialized]
        private List<WipeError> _beforeWipeErrors;
        public List<WipeError> BeforeWipeErrors {
            get { return _beforeWipeErrors; }
            set { _beforeWipeErrors = value; }
        }

        [NonSerialized]
        private List<WipeError> _afterWipeErrors;
        public List<WipeError> AfterWipeErrors {
            get { return _afterWipeErrors; }
            set { _afterWipeErrors = value; }
        }

        [NonSerialized]
        private bool _stoppedByBridge;
        public bool StoppedByBridge {
            get { return _stoppedByBridge; }
            set { _stoppedByBridge = value; }
        }
        #endregion

        #region Private methods

        private void AllocateObjects() {
            if(context == null) {
                context = new WipeContext();
            }

            if(searcher == null) {
                searcher = new FileSearcher();
            }

            if(startLock == null) {
                startLock = new object();
            }

            if(startEvent == null) {
                startEvent = new AutoResetEvent(false);
            }

            if(statistics == null) {
                statistics = new WipeStatistics();
            }

            if(_beforeWipeErrors == null) {
                _beforeWipeErrors = new List<WipeError>();
            }

            if(_afterWipeErrors == null) {
                _afterWipeErrors = new List<WipeError>();
            }

            if(_bridgeItems == null) {
                _bridgeItems = new List<IWipeObject>();
            }
        }


        private void StartAsync(object sender, EventArgs e) {
            try {
                asyncStartResult = StartImpl();
            }
            catch(Exception ex) {
                // add error
                _afterWipeErrors.Add(new WipeError(DateTime.Now, ErrorSeverity.High, "Unknown error."));
                Debug.ReportError("Error while wiping in asynchronous mode. Exception: {0}", ex.Message);
                status = SessionStatus.Stopped;
            }
            finally {
                asyncStartResult = false;
            }
        }


        private bool SendSingleWipeObject(IWipeObject item) {
            Debug.AssertNotNull(item, "Item is null");

            bool result = false;
            activeObject = item;

            if(item.SingleObject) {
                result = context.InsertObject(item.GetObject());
            }
            else {
                result = context.InserObjectRange(item.GetObjects());
            }

            // reset active object
            activeObject = null;
            return result;
        }


        private bool SendWipeObjects(List<IWipeObject> items) {
            if(items == null) {
                throw new ArgumentNullException("items");
            }

            int count = items.Count;

            for(int i = 0; i < items.Count; i++) {
                IWipeObject item = items[i];
                item.Options = options;

                switch(item.Type) {
                    case WipeObjectType.Folder: {
                        FolderWipeObject folder = item as FolderWipeObject;

                        if(folder == null) {
                            Debug.ReportError("Invalid Type");
                            continue;
                        }

                        folder.Searcher = searcher;
                        break;
                    }
                    case WipeObjectType.Plugin: {
                        PluginWipeObject plugin = item as PluginWipeObject;

                        if(plugin == null) {
                            Debug.ReportError("Invalid Type");
                            continue;
                        }

                        plugin.Assemblies = _pluginAssemblies;
                        plugin.OnPluginException += ReportPluginError;
                        break;
                        }
                }

                bool result = SendSingleWipeObject(item);

                // cleanup
                switch(item.Type) {
                    case WipeObjectType.Folder: {
                        FolderWipeObject folder = item as FolderWipeObject;
                        folder.Searcher = null;
                        break;
                        }
                    case WipeObjectType.Plugin: {
                        PluginWipeObject plugin = item as PluginWipeObject;
                        plugin.OnPluginException -= ReportPluginError;
                        break;
                    }
                }

                // check result
                if(result == false) {
                    Debug.ReportError("Failed to send wipe object");
                    return false;
                }

                if(status != SessionStatus.BeginStart) {
                    return false;
                }
            }

            return true;
        }


        private bool StartImpl() {
            bool result = false;

            lock(startLock) {
                if(items == null || items.Count == 0) {
                    return false;
                }

                startEvent.Reset();
                status = SessionStatus.BeginStart;

                // initialize the context
                if(Initialize() == false) {
                    // add error
                    _afterWipeErrors.Add(new WipeError(DateTime.Now, ErrorSeverity.High, "Initialization error."));
                    status = SessionStatus.Stopped;
                    return false;
                }

                if(status != SessionStatus.BeginStart) {
                    return false;
                }

                // send the wipe objects
                result = SendWipeObjects(_bridgeItems);
                result &= SendWipeObjects(items);

                if(result == true && status != SessionStatus.Stopped) {
                    // start!
                    result = context.SetContextStatus(ContextStatus.Wipe);

                    if(result == true) {
                        status = SessionStatus.Wipe;
                        statistics.StartTime = DateTime.Now;
                    }
                    else {
                        Debug.ReportWarning("Failed to start context");
                    }
                }
                else if(result == false) {
                    Debug.ReportWarning("Failed to send wipe objects");
                }

                startEvent.Set();
            }

            return result;
        }


        private void GenerateStatistics() {
            WipeStatus status = new WipeStatus();
            GetWipeStatus(ref status, false);

            statistics.EndTime = DateTime.Now;
            statistics.Duration = statistics.EndTime - statistics.StartTime;
            statistics.TotalWipedBytes = status.BytesWiped;
            statistics.BytesInClusterTips = statistics.BytesInClusterTips;

            if(statistics.Duration.TotalSeconds > 0) {
                statistics.AverageWriteSpeed = (long)(statistics.TotalWipedBytes / statistics.Duration.TotalSeconds);
            }

            int number;
            context.GetFailedObjectNumber(out number);
            statistics.FailedObjects = number;
            context.GetErrorNumber(out number);
            statistics.Errors = number + _beforeWipeErrors.Count + _afterWipeErrors.Count;
        }


        private void ReportPluginError(Plugin plugin, Exception e) {
            if(plugin == null) {
                return;
            }

            _beforeWipeErrors.Add(new WipeError(DateTime.Now, ErrorSeverity.High,
                                                string.Format("Critical error occured in plugin {0}", plugin)));
        }


        #endregion

        #region Public methods

        /// <summary>
        /// Initialize the wipe session. Needs to be called before Start or BeginStart.
        /// </summary>
        public bool Initialize() {
            Debug.Assert(options != null, "Options not set");

            _initialized = false;
            AllocateObjects();

            // get the status of the actual context
            ContextStatus status;

            if(context.IsOpen && context.IsInitialized) {
                if(context.GetContextStatus(out status)) {
                    if(status != ContextStatus.Stopped) {
                        throw new InvalidOperationException("Cannot create a new context while the current one is active");
                    }
                }

                // destroy the actual context
                context.DestroyContext();
            }

            // create a context
            if(context.CreateContext() == false) {
                Debug.ReportError("Failed to create context");
                return false;
            }

            // initialize the context
            if(context.InitializeContext(options.ToNative()) == false) {
                Debug.ReportError("Failed to initialize the context");
                return false;
            }

            // successfully initialized
            _initialized = true;
            return true;
        }


        /// <summary>
        /// Start wiping asynchronously
        /// </summary>
        /// <param name="callback">The method to be called when the operation has been completed.</param>
        /// <param name="state">The state object.</param>
        public bool BeginStart(AsyncCallback callback, object state) {
            Debug.AssertNotNull(callback, "Callback method not defined");

            if(items == null || items.Count == 0) {
                return false;
            }

            // run the task on the ThreadPool
            EventHandler e = new EventHandler(StartAsync);
            endStartCalled = false;
            e.BeginInvoke(null, null, callback, startEvent);
            return true;
        }


        /// <summary>
        /// Wait until the start operation has been completed
        /// </summary>
        public bool EndStart() {
            // wait until the start thread finishes
            startEvent.WaitOne();
            endStartCalled = true;
            return asyncStartResult;
        }


        /// <summary>
        /// Start wiping
        /// </summary>
        /// <returns></returns>
        public bool Start() {
            if(items == null || items.Count == 0) {
                return false;
            }

            return StartImpl();
        }


        /// <summary>
        /// Get the status of the wiping process
        /// </summary>
        /// <param name="s">The WipeStatus object where to store the status</param>
        /// <param name="includeChildren">Include the status of all children of the context</param>
        public bool GetWipeStatus(ref WipeStatus s, bool includeChildren) {
            s.SessionStatus = status;

            // get context status
            if(s.GetContextStatus(context) == false) {
                Debug.ReportWarning("Failed to get context status");
            }

            // get wipe status
            NativeMethods.WStatus wipeStatus = new NativeMethods.WStatus();

            if(context.GetWipeStatus(out wipeStatus) == true) {
                s.FromNative(wipeStatus);

                // get the status of the children
                int childrenNumber;

                if(includeChildren) {
                    if(context.GetChildrenNumber(out childrenNumber) == false) {
                        // failed to get number
                        s.Children = null;
                    }
                    else {
                        if(childrenNumber == 0) {
                            s.Children = null;
                        }
                        else {
                            wipeStatus = GetChildrenStatus(s, wipeStatus, childrenNumber);
                        }
                    }
                }
            }
            else {
                Debug.ReportWarning("Failed to read wipe status");
                return false;
            }

            return true;
        }

        private NativeMethods.WStatus GetChildrenStatus(WipeStatus s, NativeMethods.WStatus wipeStatus, int childrenNumber) {
            // allocate the array
            bool needsInit = false;

            if(s.Children == null || (s.Children != null && s.Children.Length != childrenNumber)) {
                s.Children = new WipeStatus[childrenNumber];
                needsInit = true;
            }

            // get the status
            for(int i = 0; i < childrenNumber; i++) {
                if(needsInit) {
                    s.Children[i] = new WipeStatus();
                }

                if(context.GetChildWipeStatus(i, out wipeStatus) == false) {
                    s.Children[i] = null;
                }
                else {
                    // set child data
                    s.Children[i].FromNative(wipeStatus);
                }
            }

            return wipeStatus;
        }


        /// <summary>
        /// Stop the wiping process
        /// </summary>
        public bool Stop() {
            bool result = false;

            if(status == SessionStatus.BeginStart) {
                if(context.IsOpen) {
                    context.SetContextStatus(ContextStatus.Stopped);
                }

                if(activeObject != null) {
                    activeObject.Stop();
                }

                status = SessionStatus.Stopped;
                result = true;
            }
            else if(status != SessionStatus.Stopped) {
                result = context.SetContextStatus(ContextStatus.Stopped);

                if(result == true) {
                    status = SessionStatus.Stopped;
                    GenerateStatistics();
                }
            }

            // remove bridge items
            if(_bridgeItems != null) {
                _bridgeItems.Clear();
            }

            return result;
        }


        /// <summary>
        /// Pause the wiping process
        /// </summary>
        public bool Pause() {
            Debug.Assert(status == SessionStatus.Paused, "Session already paused");
            return context.SetContextStatus(ContextStatus.Paused);
        }


        /// <summary>
        /// Resume the wiping process
        /// </summary>
        public bool Resume() {
            Debug.Assert(status != SessionStatus.Paused, "Session not paused");
            return context.SetContextStatus(ContextStatus.Wipe);
        }


        /// <summary>
        /// Get the category of wipe errors
        /// </summary>
        public List<WipeError> GetWipeErrors() {
            List<WipeError> wipeErrors = new List<WipeError>();
            ContextStatus status;
            bool valid = false;
            int count = _beforeWipeErrors.Count;

            // add before wipe errors
            for(int i = 0; i < count; i++) {
                wipeErrors.Add(_beforeWipeErrors[i]);
            }

            if(context.IsOpen) {
                // ensure the context is stopped
                if(context.GetContextStatus(out status)) {
                    if(status == ContextStatus.Stopped) {
                        valid = context.IsInitialized;
                    }
                }

                if(valid) {
                    // get the errors
                    NativeMethods.WError[] errors = context.GetErrors();

                    if(errors != null) {
                        for(int i = 0; i < errors.Length; i++) {
                            WipeError error = new WipeError();
                            error.ReadNative(errors[i]);
                            wipeErrors.Add(error);
                        }
                    }
                }
            }

            // add after wipe errors
            count = _afterWipeErrors.Count;

            for(int i = 0; i < count; i++) {
                wipeErrors.Add(_afterWipeErrors[i]);
            }

            return wipeErrors;
        }


        /// <summary>
        /// Get the category of failed objects
        /// </summary>
        /// <param name="getAssociatedError">Specifies whether or not to attach the associated error to the wipe object.</param>
        /// <returns></returns>
        public FailedObject[] GetFailedObjects(bool getAssociatedError) {
            ContextStatus status;
            bool valid = false;

            if(context.IsOpen) {
                if(context.GetContextStatus(out status)) {
                    if(status == ContextStatus.Stopped) {
                        valid = true;
                    }
                }

                if(valid) {
                    if(getAssociatedError) {
                        KeyValuePair<NativeMethods.WSmallObject, NativeMethods.WError>[] failed = 
                            context.GetFailedObjectsWithErrors();

                        if(failed == null || failed.Length == 0) {
                            return null;
                        }

                        FailedObject[] failedObjects = new FailedObject[failed.Length];

                        // transform from the native to the managed form
                        for(int i = 0; i < failed.Length; i++) {
                            failedObjects[i] = new FailedObject();
                            failedObjects[i].ReadNative(failed[i].Key);
                            failedObjects[i].AssociatedError = new WipeError();
                            failedObjects[i].AssociatedError.ReadNative(failed[i].Value);
                        }

                        return failedObjects;
                    }
                    else {
                        NativeMethods.WSmallObject[] failed = context.GetFailedObjects();

                        if(failed == null || failed.Length == 0) {
                            return null;
                        }

                        FailedObject[] failedObjects = new FailedObject[failed.Length];

                        // transform from the native to the managed form
                        for(int i = 0; i < failed.Length; i++) {
                            failedObjects[i] = new FailedObject();
                            failedObjects[i].ReadNative(failed[i]);
                        }

                        return failedObjects;
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Generate a report containing statistics, failed objects and errors
        /// </summary>
        /// <remarks>This method is not intended to be used in conjunction with scheduled tasks.</remarks>
        public WipeReport GenerateReport() {
            // check if wipe is stopped
            if(status != SessionStatus.Stopped) {
                Debug.ReportWarning("Report requested while not in stop state. Status: {0}", status);
                return null;
            }

            WipeReport report = new WipeReport(guid);
            List<WipeError> errors = null;
            report.Statistics = (WipeStatistics)statistics.Clone();
            report.Statistics.Errors += _beforeWipeErrors.Count + _afterWipeErrors.Count;

            // add the failed objects without their associated errors
            try {
                FailedObject[] failed = GetFailedObjects(true);

                if(failed != null && failed.Length > 0) {
                    report.FailedObjects.AddRange(failed);
                }
            }
            catch { }

            // add errors
            errors = GetWipeErrors();

            if(errors != null) {
                report.Errors.AddRange(errors);
            }

            return report;
        }


        public void GenerateGuid() {
            guid = Guid.NewGuid();
        }


        public void SetGuid(Guid newGuid) {
            guid = newGuid;
        }

        #endregion

        #region Serialization helpers

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context) {
            endStartCalled = true;
            AllocateObjects();
        }

        #endregion
    }
}
