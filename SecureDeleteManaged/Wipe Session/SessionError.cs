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

namespace SecureDelete {
    public enum ErrorSeverity {
        High,
        Medium,
        Low
    }


    [Serializable]
    public class WipeError {
        #region Properties

        private DateTime _time;
        public DateTime Time {
            get { return _time; }
            set { _time = value; }
        }

        private ErrorSeverity _severity;
        public ErrorSeverity Severity {
            get { return _severity; }
            set { _severity = value; }
        }

        private string _message;
        public string Message {
            get { return _message; }
            set { _message = value; }
        }

        #endregion

        #region Constructor

        public WipeError() {

        }

        public WipeError(DateTime time, ErrorSeverity severity, string message) {
            _time = time;
            _severity = severity;
            _message = message;
        }

        #endregion

        #region Public methods

        public void ReadNative(NativeMethods.WError error) {
            _time = new DateTime(error.time.wYear, error.time.wMonth, error.time.wDay, error.time.wHour, 
                                 error.time.wMinute, error.time.wSecond, error.time.wMilliseconds);

            switch(error.severity) {
                case NativeMethods.SEVERITY_HIGH: {
                    _severity = ErrorSeverity.High;
                    break;
                }
                case NativeMethods.SEVERITY_MEDIUM: {
                    _severity = ErrorSeverity.Medium;
                    break;
                }
                case NativeMethods.SEVERITY_LOW: {
                    _severity = ErrorSeverity.Low;
                    break;
                }
            }

            _message = error.message;
        }

        #endregion
    }


    [Serializable]
    public class FailedObject {
        #region Properties

        private WipeObjectType _type;
        public WipeObjectType Type {
            get { return _type; }
            set { _type = value; }
        }

        private string _path;
        public string Path {
            get { return _path; }
            set { _path = value; }
        }

        private WipeError _associatedError;
        public WipeError AssociatedError {
            get { return _associatedError; }
            set { _associatedError = value; }
        }

        #endregion

        #region Constructor

        public FailedObject() {

        }

        public FailedObject(WipeObjectType type, string path, WipeError error) {
            _type = type;
            _path = path;
            _associatedError = error;
        }

        #endregion

        #region Public methods

        public void ReadNative(NativeMethods.WSmallObject failed) {
            switch(failed.type) {
                case NativeMethods.TYPE_FILE: {
                    _type = WipeObjectType.File;
                    break;
                }
                case NativeMethods.TYPE_FOLDER: {
                    _type = WipeObjectType.Folder;
                    break;
                }
                case NativeMethods.TYPE_DRIVE: {
                    _type = WipeObjectType.Drive;
                    break;
                }
            }

            _path = failed.path;
        }

        #endregion
    }
}
