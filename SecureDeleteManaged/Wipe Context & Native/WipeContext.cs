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
using System.Runtime.InteropServices;
using System.Globalization;
using System.Runtime;
using DebugUtils.Debugger;

namespace SecureDelete {
    /// <summary>
    /// The status of the context.
    /// </summary>
    public enum ContextStatus {
        Wipe,
        Paused,
        Stopped
    }


    /// <summary>
    /// Manages a native wipe context.
    /// </summary>
    public class WipeContext {
        #region Properties

        private int _contextId;
        public int ContextId {
            get { return _contextId; }
        }

        private bool _isOpen;
        public bool IsOpen {
            get { return _isOpen; }
        }

        private bool _isInitialized;
        public bool IsInitialized {
            get { return _isInitialized; }
        }

        private ContextStatus _status;
        public ContextStatus Status {
            get { return _status; }
        }

        #endregion

        #region Constructor

        public WipeContext() {
            _status = ContextStatus.Stopped;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Check the result received from the native methods
        /// </summary>
        private bool ValidResult(int result) {
            switch(result) {
                case NativeMethods.ERRORCODE_INITIALIZATION: {
                    Debug.ReportWarning("Context not initialized");
                    break;
                }
                case NativeMethods.ERRORCODE_INVALID_CONTEXT: {
                    Debug.ReportWarning("Invalid context Id");
                    break;
                }
                case NativeMethods.ERRORCODE_INVALID_REQUEST: {
                    Debug.ReportWarning("Invalid request");
                    break;
                }
                case NativeMethods.ERRORCODE_NO_MORE_CONTEXTS: {
                    Debug.ReportWarning("No more context slots available");
                    break;
                }
            }

            return (result == NativeMethods.ERRORCODE_SUCCESS);
        }


        /// <summary>
        /// Throw an exception if the context is not closed
        /// </summary>
        private void CheckContextClosed() {
            if(_isOpen == true) {
                throw new Exception("Context not closed");
            }
        }


        /// <summary>
        /// Trow an exception if the context is not open
        /// </summary>
        private void CheckContextOpen() {
            if(_isOpen == false) {
                throw new Exception("Context is not open");
            }
        }


        /// <summary>
        /// Trow an exception if the context is not initialized
        /// </summary>
        private void CheckContextInitialized() {
            if(_isOpen == false || _isInitialized == false) {
                throw new Exception("Context not initialized");
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a context
        /// </summary>
        public bool CreateContext() {
            CheckContextClosed();
            int result = NativeMethods.CreateWipeContext(out _contextId);

            if(result == NativeMethods.ERRORCODE_SUCCESS) {
                _isOpen = true;
                _status = ContextStatus.Stopped;
            }

            return ValidResult(result);
        }


        /// <summary>
        /// Destroy a context
        /// </summary>
        public bool DestroyContext() {
            CheckContextOpen();
            int result = NativeMethods.ERRORCODE_SUCCESS;

            // stop the context
            if(_status != ContextStatus.Stopped) {
                result = NativeMethods.StopWipeContext(_contextId);
            }

            if(result == NativeMethods.ERRORCODE_SUCCESS) {
                // destroy the context
                result = NativeMethods.DestroyWipeContext(_contextId);

                if(result == NativeMethods.ERRORCODE_SUCCESS) {
                    _isOpen = false;
                    _isInitialized = false;
                    _status = ContextStatus.Stopped;
                }
            }

            return ValidResult(result);
        }


        /// <summary>
        /// Initialize the context with the given settings
        /// </summary>
        public bool InitializeContext(NativeMethods.WOptions options) {
            CheckContextOpen();
            int result = NativeMethods.SetWipeOptions(_contextId, ref options);

            if(result == NativeMethods.ERRORCODE_SUCCESS) {
                // initialize the context
                result = NativeMethods.InitializeWipeContext(_contextId);

                if(result == NativeMethods.ERRORCODE_SUCCESS) {
                    _isInitialized = true;
                }
            }

            return ValidResult(result);
        }


        /// <summary>
        /// Set the status of the context (wipe,paused,stopped)
        /// </summary>
        public bool SetContextStatus(ContextStatus newStatus) {
            CheckContextInitialized();
            int result = NativeMethods.ERRORCODE_SUCCESS;

            switch(newStatus) {
                case ContextStatus.Wipe: {
                    if(_status == ContextStatus.Stopped) {
                        result = NativeMethods.StartWipeContext(_contextId);
                    }
                    else if(_status == ContextStatus.Paused) {
                        result = NativeMethods.ResumeWipeContext(_contextId);
                    }

                    break;
                }
                case ContextStatus.Paused: {
                    if(_status == ContextStatus.Wipe) {
                        result = NativeMethods.PauseWipeContext(_contextId);
                    }

                    break;
                }
                case ContextStatus.Stopped: {
                    if(_status == ContextStatus.Wipe || _status == ContextStatus.Paused) {
                        result = NativeMethods.StopWipeContext(_contextId);
                    }

                    break;
                }
            }

            _status = newStatus;
            return ValidResult(result);
        }


        /// <summary>
        /// Get the status from the native context
        /// </summary>
        public bool GetContextStatus(out ContextStatus status) {
            CheckContextOpen();
            status = ContextStatus.Stopped;
            int result = NativeMethods.GetContextStatus(_contextId);

            if(result != NativeMethods.ERRORCODE_INVALID_CONTEXT) {
                switch(result) {
                    case NativeMethods.STATUS_PAUSED: {
                        status = ContextStatus.Paused;
                        break;
                    }
                    case NativeMethods.STATUS_STOPPED: {
                        status = ContextStatus.Stopped;
                        break;
                    }
                    case NativeMethods.STATUS_WIPE: {
                        status = ContextStatus.Wipe;
                        break;
                    }
                }

                // synchronize the native status with the managed one
                _status = status;
                return true;
            }
               
            return false;
        }


        /// <summary>
        /// Get the wipe status of the context
        /// </summary>
        public bool GetWipeStatus(out NativeMethods.WStatus status) {
            CheckContextInitialized();
            int result = NativeMethods.GetWipeStatus(_contextId, out status);
            return ValidResult(result);
        }


        /// <summary>
        /// Get the number of children
        /// </summary>
        public bool GetChildrenNumber(out int childrenNumber) {
            CheckContextInitialized();
            int result = NativeMethods.GetChildrenNumber(_contextId, out childrenNumber);
            return ValidResult(result);
        }


        /// <summary>
        /// Get the wipe status of a child
        /// </summary>
        public bool GetChildWipeStatus(int child, out NativeMethods.WStatus status) {
            CheckContextInitialized();
            int result = NativeMethods.GetChildWipeStatus(_contextId, child, out status);
            return ValidResult(result);
        }


        /// <summary>
        /// Insert an object into the context
        /// </summary>
        public bool InsertObject(NativeMethods.WObject item) {
            CheckContextInitialized();

            // object can be inserted only when the context is stopped
            if(_status != ContextStatus.Stopped) {
                throw new Exception("Object can be inserted only when the context is stopped");
            }

            // insert the object
            int result = NativeMethods.InsertWipeObject(_contextId, ref item);
            return ValidResult(result);
        }


        /// <summary>
        /// Insert object from an array into the context
        /// </summary>
        public bool InserObjectRange(NativeMethods.WObject[] items) {
            // ignore 0-length array
            if(items == null || items.Length == 0) {
                return true;
            }

            // object can be inserted only when the context is stopped
            if(_status != ContextStatus.Stopped) {
                throw new Exception("Object can be inserted only when the context is stopped");
            }

            // insert the objects
            int result = NativeMethods.ERRORCODE_SUCCESS;
            int count = items.Length;

            for(int i = 0; i < count; i++) {
                result = NativeMethods.InsertWipeObject(_contextId, ref items[i]);

                if(ValidResult(result) == false) {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Get the error number
        /// </summary>
        public bool GetErrorNumber(out int errorNumber) {
            CheckContextInitialized();

            // object can be inserted only when the context is stopped
            if(_status != ContextStatus.Stopped) {
                throw new Exception("Object can be inserted only when the context is stopped");
            }

            int result = NativeMethods.GetErrorNumber(_contextId, out errorNumber);
            return ValidResult(result);
        }


        /// <summary>
        /// Get an error by its index
        /// </summary>
        public bool GetError(int index, out NativeMethods.WError error) {
            CheckContextInitialized();

            // object can be inserted only when the context is stopped
            if(_status != ContextStatus.Stopped) {
                throw new Exception("Object can be inserted only when the context is stopped");
            }

            int result = NativeMethods.GetError(_contextId, index, out error);
            return ValidResult(result);
        }


        /// <summary>
        /// Get all errors
        /// </summary>
        public NativeMethods.WError[] GetErrors() {
            CheckContextInitialized();
            int number;

            // object can be inserted only when the context is stopped
            if(_status != ContextStatus.Stopped) {
                throw new Exception("Object can be inserted only when the context is stopped");
            }

            if(GetErrorNumber(out number) == false) {
                return null;
            }

            // get the errors
            List<NativeMethods.WError> errors = new List<NativeMethods.WError>();

            for(int i = 0; i < number; i++) {
                NativeMethods.WError error = new NativeMethods.WError();

                if(GetError(i, out error) == false) {
                    // return what we could get so far
                    return errors.ToArray();
                }
                else {
                    errors.Add(error);
                }
            }

            // all errors could be retrieved
            return errors.ToArray();
        }


        /// <summary>
        /// Get the number of failed objects
        /// </summary>
        public bool GetFailedObjectNumber(out int failedNumber) {
            CheckContextInitialized();

            // object can be inserted only when the context is stopped
            if(_status != ContextStatus.Stopped) {
                throw new Exception("Object can be inserted only when the context is stopped");
            }

            int result = NativeMethods.GetFailedObjectNumber(_contextId, out failedNumber);
            return ValidResult(result);
        }


        /// <summary>
        /// Get an failed object, and optionally, its associated error
        /// </summary>
        public bool GetFailedObject(int index, out NativeMethods.WSmallObject item, 
                                    bool getAssociatedError, out NativeMethods.WError error) {
            CheckContextInitialized();

            // object can be inserted only when the context is stopped
            if(_status != ContextStatus.Stopped) {
                throw new Exception("Object can be inserted only when the context is stopped");
            }

            int result = NativeMethods.GetFailedObject(_contextId, index, out item);

            if(result == NativeMethods.ERRORCODE_SUCCESS && getAssociatedError) {
                // get the associated error
                result = NativeMethods.GetError(_contextId, item.log, out error);
            }
            else {
                error = new NativeMethods.WError();
            }

            return ValidResult(result);
        }


        /// <summary>
        /// Get all failed objects
        /// </summary>
        public NativeMethods.WSmallObject[] GetFailedObjects() {
            CheckContextInitialized();
            int number;

            // object can be inserted only when the context is stopped
            if(_status != ContextStatus.Stopped) {
                throw new Exception("Object can be inserted only when the context is stopped");
            }

            if(GetFailedObjectNumber(out number) == false) {
                return null;
            }

            // get the errors
            NativeMethods.WError dummyError = new NativeMethods.WError();
            List<NativeMethods.WSmallObject> objects = new List<NativeMethods.WSmallObject>();

            for(int i = 0; i < number; i++) {
                NativeMethods.WSmallObject item = new NativeMethods.WSmallObject();

                if(GetFailedObject(i, out item, false, out dummyError) == false) {
                    // return what we could get so far
                    return objects.ToArray();
                }
                else {
                    objects.Add(item);
                }
            }

            // all errors could be retrieved
            return objects.ToArray();
        }


        /// <summary>
        /// Get all failed objects with their associated errors
        /// </summary>
        public KeyValuePair<NativeMethods.WSmallObject, NativeMethods.WError>[] GetFailedObjectsWithErrors() {
            CheckContextInitialized();
            int number;

            // failed objects can be retrieved only if the context is stopped
            if(_status != ContextStatus.Stopped) {
                throw new Exception("Object can be inserted only when the context is stopped");
            }

            if(GetFailedObjectNumber(out number) == false) {
                return null;
            }

            // get the errors
            List<KeyValuePair<NativeMethods.WSmallObject, NativeMethods.WError>> objects = 
                new List<KeyValuePair<NativeMethods.WSmallObject, NativeMethods.WError>>();

            for(int i = 0; i < number; i++) {
                NativeMethods.WSmallObject item = new NativeMethods.WSmallObject();
                NativeMethods.WError error = new NativeMethods.WError();

                if(GetFailedObject(i, out item, true, out error) == false) {
                    // return what we could get so far
                    return objects.ToArray();
                }
                else {
                    KeyValuePair<NativeMethods.WSmallObject, NativeMethods.WError> pair = 
                        new KeyValuePair<NativeMethods.WSmallObject, NativeMethods.WError>(item, error);

                    objects.Add(pair);
                }
            }

            // all errors could be retrieved
            return objects.ToArray();
        }

        #endregion
    }
}
