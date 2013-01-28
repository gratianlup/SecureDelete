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
using System.Runtime.Serialization.Formatters.Binary;
using DebugUtils.Debugger;

namespace SecureDelete.Schedule {
    public delegate void ManagerTaskStatusChangedDelegate(Guid id, TaskStatus status);

    public class TaskManager {
        #region Constants

        public const string TaskFileExtension = ".sdt";

        #endregion

        #region Fields

        private List<ScheduledTask> taskQueue;
        private object queueLock = new object();

        #endregion

        #region Constructor

        public TaskManager() {
            taskQueue = new List<ScheduledTask>();
            _taskList = new List<ScheduledTask>();
            _taskHistory = new HistoryManager();
            _taskControllers = new List<ITaskController>();
        }

        #endregion

        #region Properties

        private List<ScheduledTask> _taskList;
        public List<ScheduledTask> TaskList {
            get { return _taskList; }
            set { _taskList = value; }
        }

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set {
                _options = value;

                // set the new options for all tasks
                foreach(ScheduledTask task in _taskList) {
                    task.Options = _options;
                }
            }
        }

        private List<ITaskController> _taskControllers;
        public List<ITaskController> TaskControllers {
            get { return _taskControllers; }
            set { _taskControllers = value; }
        }

        private HistoryManager _taskHistory;
        public HistoryManager TaskHistory {
            get { return _taskHistory; }
            set { _taskHistory = value; }
        }

        #endregion

        #region Events

        public event ManagerTaskStatusChangedDelegate OnTaskStatusChanged;

        #endregion

        #region Private methods

        private void StartTasks() {
            foreach(ScheduledTask task in TaskList) {
                if(task.Enabled && task.Status == TaskStatus.Stopped) {
                    StartTaskSchedule(task);
                }
            }
        }


        private void PrepareForStart(ScheduledTask task) {
            LoadOptions();
            task.Options = _options;
            task.HistoryManager = _taskHistory;
            task.TaskControllers = _taskControllers;
            AttachTaskEvents(task);
        }


        private void AttachTaskEvents(ScheduledTask task) {
            task.OnTaskStarted += TaskStarted;
            task.OnTaskStatusChanged += TaskStatusChanged;
            task.OnTaskCompleted += TaskCompleted;
        }


        private void DetachTaskEvents(ScheduledTask task) {
            task.OnTaskStarted -= TaskStarted;
            task.OnTaskStatusChanged -= TaskStatusChanged;
            task.OnTaskCompleted -= TaskCompleted;
        }


        public void StartTaskSchedule(ScheduledTask task) {
            PrepareForStart(task);
            task.StartSchedule();
        }


        private void TaskStarted(ScheduledTask task) {
            // add it to the queue
            lock(queueLock) {
                taskQueue.Add(task);
            }

            // wait for previous tasks to finnish
            if(_options.QueueTasks) {
                int position;
                lock(queueLock) {
                    position = taskQueue.IndexOf(task);
                }

                while(position > 0) {
                    task.Status = TaskStatus.Queued;

                    // get first task
                    ScheduledTask first;
                    lock(queueLock) {
                        first = taskQueue[0];
                    }

                    first.WaitForFinnish();

                    // update position
                    lock(queueLock) {
                        position = taskQueue.IndexOf(task);
                    }
                }
            }

            // start
            if(task.Status != TaskStatus.Stopped && TaskCanStart()) {
                task.StartWipe();
            }
        }


        private void TaskCompleted(ScheduledTask task) {
            lock(queueLock) {
                // remove from the queue
                if(taskQueue.Contains(task)) {
                    taskQueue.Remove(task);
                }

                ControllerTaskStopped(task, taskQueue.Count);
            }
        }


        private void TaskStatusChanged(ScheduledTask task, TaskStatus status) {
            if(OnTaskStatusChanged != null) {
                OnTaskStatusChanged(task.TaskId, status);
            }
        }


        private void HandleNewTask(ScheduledTask task, bool start) {
            task.Options = _options;

            if(start) {
                StartTaskSchedule(task);
            }
        }


        private bool TryStopTask(ScheduledTask task) {
            if(task.Status == TaskStatus.Waiting) {
                // stop the timer
                task.Stop();
                return true;
            }
            else if(task.Status == TaskStatus.Stopped) {
                return true;
            }
            else {
                return false;
            }
        }


        /// <summary>
        /// Stop a task, event if it's in wiping mode
        /// </summary>
        private bool ForceStopTask(ScheduledTask task) {
            return task.Stop();
        }

        #region Task controller helper methods

        private void ControllerTaskStopped(ScheduledTask task, int remaining) {
            foreach(ITaskController controller in _taskControllers) {
                controller.TaskStopped(task, remaining);
            }
        }

        public void StopAllTasks() {
            if(_taskList == null) {
                return;
            }

            foreach(ScheduledTask task in _taskList) {
                ForceStopTask(task);
            }
        }


        public void PauseAllTasks() {
            if(_taskList == null) {
                return;
            }

            foreach(ScheduledTask task in _taskList) {
                if(task.Status == TaskStatus.Wiping) {
                    task.Pause();
                }
            }
        }


        public void ResumeAllTasks() {
            if(_taskList == null) {
                return;
            }

            foreach(ScheduledTask task in _taskList) {
                if(task.Status == TaskStatus.Paused) {
                    task.Resume();
                }
            }
        }

        #endregion


        private bool TaskCanStart() {
            foreach(ITaskController controller in _taskControllers) {
                if(controller.Enabled && controller.AllowTaskStart == false) {
                    Debug.ReportWarning("Task start blocked");
                    return false;
                }
            }

            return true;
        }


        private void SetControllerSettings() {
            foreach(ITaskController controller in _taskControllers) {
                switch(controller.Type) {
                    case ControllerType.Power: {
                        controller.Settings = _options.PowerControllerSettings;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Public methods

        public static byte[] SerializeTask(ScheduledTask task) {
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            try {
                // serialize in memory
                serializer.Serialize(stream, task);
                return stream.ToArray();
            }
            catch(Exception e) {
                Debug.ReportError("Error while serializing task. Exception: {0}", e.Message);
                return null;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }


        public static ScheduledTask DeserializeTask(byte[] data) {
            if(data == null) {
                return null;
            }

            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);

            try {
                return (ScheduledTask)serializer.Deserialize(stream);
            }
            catch(Exception e) {
                Debug.ReportError("Error while deserializing task. Exception: {0}", e.Message);
                return null;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }


        public static bool LoadTask(string path, out ScheduledTask task) {
            // check the parameters
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            task = null;

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
                task = DeserializeTask(store.ReadFile("task.dat"));
                return true;
            }
            catch(Exception e) {
                Debug.ReportError("Error while loading task. Exception: {0}", e.Message);
                return false;
            }
        }


        public static bool SaveTask(string path, ScheduledTask task) {
            // check the parameters
            if(task == null || path == null) {
                throw new ArgumentNullException("task | path");
            }

            try {
                // create the store
                FileStore.FileStore store = new FileStore.FileStore();
                store.Encrypt = true;
                store.UseDPAPI = true;

                // add the file
                FileStore.StoreFile file = store.CreateFile("task.dat");
                byte[] data = SerializeTask(task);

                if(data == null) {
                    return false;
                }

                // write the file contents
                store.WriteFile(file, data, FileStore.StoreMode.Encrypted);
                return store.Save(path);
            }
            catch(Exception e) {
                Debug.ReportError("Error while saving task. Exception: {0}", e.Message);
                return false;
            }
        }


        public bool LoadOptions() {
            _options = new SDOptions();

            if(SDOptionsFile.LoadOptions(SecureDeleteLocations.GetOptionsFilePath(), ref _options) == false) {
                Debug.ReportError("Failed to load settings");
                return false;
            }

            _options.MethodFolder = SecureDeleteLocations.GetMethodsFolder();
            Options = _options;
            SetControllerSettings();
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Used on the client side only.</remarks>
        /// <returns></returns>
        public bool AddTask(ScheduledTask task, bool start) {
            if(task == null) {
                throw new ArgumentNullException("task");
            }

            // check if already in list
            foreach(ScheduledTask t in _taskList) {
                if(task.TaskId == t.TaskId) {
                    return false;
                }
            }

            // save
            if(SaveTask(SecureDeleteLocations.CombinePath(SecureDeleteLocations.GetScheduledTasksDirectory(),
                                                          task.TaskId.ToString() + TaskFileExtension), task) == false) {
                return false;
            }

            // add to the list
            _taskList.Add(task);
            HandleNewTask(task, start);
            return true;
        }


        public bool LoadAndHandleTask(string path) {
            // load the task from the disk
            ScheduledTask task = null;

            if(LoadTask(path, out task) == false) {
                return false;
            }

            // check if it is already in the list
            bool found = false;

            for(int i = 0; i < _taskList.Count; i++) {
                if(_taskList[i].TaskId == task.TaskId) {
                    // try to stop the task
                    if(TryStopTask(_taskList[i]) == false) {
                        return false;
                    }

                    // replace
                    _taskList[i] = task;
                    found = true;
                }
            }

            if(found == false) {
                // add as a new task
                _taskList.Add(task);
            }

            HandleNewTask(task, true);
            return true;
        }


        public bool LoadAndHandleTask(Guid id) {
            return LoadAndHandleTask(SecureDeleteLocations.CombinePath(SecureDeleteLocations.GetScheduledTasksDirectory(),
                                                                       id.ToString() + TaskFileExtension));
        }


        public bool RemoveTask(Guid id) {
            ScheduledTask task = GetTaskById(id);

            // not found
            if(task == null) {
                return false;
            }

            // don't remove if it is running
            if(TryStopTask(task) == false) {
                return false;
            }

            // remove it from the queue
            if(taskQueue.Contains(task)) {
                taskQueue.Remove(task);
            }

            _taskList.Remove(task);
            return true;
        }


        public ScheduledTask GetTaskById(Guid id) {
            int count = _taskList.Count;

            for(int i = 0; i < count; i++) {
                if(_taskList[i].TaskId == id) {
                    return _taskList[i];
                }
            }

            return null;
        }


        public void LoadTasks() {
            string path = SecureDeleteLocations.GetScheduledTasksDirectory();

            try {
                string[] files = Directory.GetFiles(path, SecureDeleteLocations.TaskFilePattern);

                foreach(string file in files) {
                    // load the task
                    LoadAndHandleTask(file);
                }
            }
            catch(Exception e) {
                Debug.ReportError("Error while loading task. Exception: {0}", e.Message);
            }

            // load task history
            if(_taskHistory.LoadHistory(SecureDeleteLocations.GetScheduleHistoryFile()) == false) {
                Debug.ReportError("Failed  to load task history.");
            }
        }


        /// <summary>
        /// Schedule the task.
        /// </summary>
        public bool StartTask(Guid id) {
            ScheduledTask task = GetTaskById(id);

            if(task == null) {
                return false;
            }

            if(task.Status != TaskStatus.Stopped) {
                return false;
            }

            StartTaskSchedule(task);
            return true;
        }


        /// <summary>
        /// Start the task in wipe mode.
        /// </summary>
        public bool ForceStartTask(Guid id) {
            ScheduledTask task = GetTaskById(id);

            if(task == null) {
                return false;
            }

            if(task.Status != TaskStatus.Stopped && task.Status != TaskStatus.Waiting) {
                return false;
            }

            PrepareForStart(task);
            return task.StartWipe();
        }

        /// <summary>
        /// Stop a task, event if it's in wiping mode
        /// </summary>
        public bool ForceStopTask(Guid id) {
            ScheduledTask task = GetTaskById(id);

            if(task == null) {
                return false;
            }

            return task.Stop();
        }

        #endregion
    }
}
