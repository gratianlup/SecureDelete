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
    /// <summary>
    /// The random generator to use
    /// </summary>
    public enum RandomProvider {
        Mersenne,
        ISAAC
    }


    [Serializable]
    public class RandomOptions : ICloneable {
        #region Properties

        private RandomProvider _randomProvider;
        public RandomProvider RandomProvider {
            get { return _randomProvider; }
            set { _randomProvider = value; }
        }

        private bool _useSlowPool;
        public bool UseSlowPool {
            get { return _useSlowPool; }
            set { _useSlowPool = value; }
        }

        private bool _preventWriteToSwap;
        public bool PreventWriteToSwap {
            get { return _preventWriteToSwap; }
            set { _preventWriteToSwap = value; }
        }

        private bool _reseed;
        public bool Reseed {
            get { return _reseed; }
            set { _reseed = value; }
        }

        private TimeSpan _reseedInterval;
        public TimeSpan ReseedInterval {
            get { return _reseedInterval; }
            set { _reseedInterval = value; }
        }

        private TimeSpan _poolUpdateInterval;
        public TimeSpan PoolUpdateInterval {
            get { return _poolUpdateInterval; }
            set { _poolUpdateInterval = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get the native variant of the RNG options
        /// </summary>
        public NativeMethods.RngOptions ToNative() {
            NativeMethods.RngOptions options = new NativeMethods.RngOptions();
            options.randomProvider = _randomProvider == RandomProvider.Mersenne ? 
                                                        NativeMethods.RNG_MERSENNE : 
                                                        NativeMethods.RNG_ISAAC;
            options.useSlowPool = _useSlowPool;
            options.preventWriteToSwap = _preventWriteToSwap;
            options.reseed = (int)_reseedInterval.TotalMilliseconds > 0;
            options.reseedInterval = (int)_reseedInterval.TotalMilliseconds;
            options.poolUpdateInterval = (int)_poolUpdateInterval.TotalMilliseconds;
            return options;
        }

        #endregion


        #region ICloneable Members

        public object Clone() {
            RandomOptions temp = new RandomOptions();
            temp._poolUpdateInterval = _poolUpdateInterval;
            temp._preventWriteToSwap = _preventWriteToSwap;
            temp._randomProvider = _randomProvider;
            temp._reseed = _reseed;
            temp._reseedInterval = _reseedInterval;
            temp._useSlowPool = _useSlowPool;
            return temp;
        }

        #endregion
    }


    [Serializable]
    public class WipeOptions : ICloneable {
        #region Constants

        public static int DefaultWipeMethod = -1;

        #endregion

        #region Constructor

        public WipeOptions() {
            _randomOptions = new RandomOptions();
        }

        #endregion

        #region Properties

        private string _wipeMethodsPath;
        public string WipeMethodsPath {
            get { return _wipeMethodsPath; }
            set { _wipeMethodsPath = value; }
        }

        private string _pluginDirectory;
        public string PluginDirectory {
            get { return _pluginDirectory; }
            set { _pluginDirectory = value; }
        }

        private bool _totalDelete;
        public bool TotalDelete {
            get { return _totalDelete; }
            set { _totalDelete = value; }
        }

        private bool _wipeUsedFileRecord;
        public bool WipeUsedFileRecord {
            get { return _wipeUsedFileRecord; }
            set { _wipeUsedFileRecord = value; }
        }

        private bool _wipeUnusedFileRecord;
        public bool WipeUnusedFileRecord {
            get { return _wipeUnusedFileRecord; }
            set { _wipeUnusedFileRecord = value; }
        }

        private bool _wipeUsedIndexRecord;
        public bool WipeUsedIndexRecord {
            get { return _wipeUsedIndexRecord; }
            set { _wipeUsedIndexRecord = value; }
        }

        private bool _wipeUnusedIndexRecord;
        public bool WipeUnusedIndexRecord {
            get { return _wipeUnusedIndexRecord; }
            set { _wipeUnusedIndexRecord = value; }
        }

        private bool _destroyFreeSpaceFiles;
        public bool DestroyFreeSpaceFiles {
            get { return _destroyFreeSpaceFiles; }
            set { _destroyFreeSpaceFiles = value; }
        }

        private RandomOptions _randomOptions;
        public RandomOptions RandomOptions {
            get { return _randomOptions; }
            set { _randomOptions = value; }
        }

        private int _defaultFileMethod;
        public int DefaultFileMethod {
            get { return _defaultFileMethod; }
            set { _defaultFileMethod = value; }
        }

        private int _defaultFreeSpaceMethod;
        public int DefaultFreeSpaceMethod {
            get { return _defaultFreeSpaceMethod; }
            set { _defaultFreeSpaceMethod = value; }
        }

        private bool _wipeAds;
        public bool WipeAds {
            get { return _wipeAds; }
            set { _wipeAds = value; }
        }

        private bool _wipeFileName;
        public bool WipeFileName {
            get { return _wipeFileName; }
            set { _wipeFileName = value; }
        }

        private bool _useLogFile;
        public bool UseLogFile {
            get { return _useLogFile; }
            set { _useLogFile = value; }
        }

        private string _logFilePath;
        public string LogFilePath {
            get { return _logFilePath; }
            set { _logFilePath = value; }
        }

        private bool _appendToLog;
        public bool AppendToLog {
            get { return _appendToLog; }
            set { _appendToLog = value; }
        }

        private bool _useLogFileSizeLimit;
        public bool UseLogFileSizeLimit {
            get { return _useLogFileSizeLimit; }
            set { _useLogFileSizeLimit = value; }
        }

        private int _logFileSizeLimit;
        public int LogFileSizeLimit {
            get { return _logFileSizeLimit; }
            set { _logFileSizeLimit = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get the native variant of the wipe options
        /// </summary>
        public NativeMethods.WOptions ToNative() {
            NativeMethods.WOptions options = new NativeMethods.WOptions();
            options.methodPath = _wipeMethodsPath;
            options.useLogFile = Convert.ToInt32(_useLogFile);
            options.logFile = _logFilePath;
            options.appendLog = Convert.ToInt32(_appendToLog);
            options.logSizeLimit = _useLogFileSizeLimit ? _logFileSizeLimit * 1024 : 0;
            options.totalDelete = Convert.ToInt32(_totalDelete);
            options.wipeUnusedFileRecord = Convert.ToInt32(_wipeUnusedFileRecord);
            options.wipeUnusedIndexRecord = Convert.ToInt32(_wipeUnusedIndexRecord);
            options.WipeUsedFileRecord = Convert.ToInt32(_wipeUsedFileRecord);
            options.wipeUsedIndexRecord = Convert.ToInt32(_wipeUsedIndexRecord);

            if(_randomOptions != null) {
                options.rngOptions = _randomOptions.ToNative();
            }

            return options;
        }

        #endregion

        #region ICloneable Members

        public object Clone() {
            WipeOptions temp = new WipeOptions();
            temp._defaultFileMethod = _defaultFileMethod;
            temp._defaultFreeSpaceMethod = _defaultFreeSpaceMethod;
            temp._destroyFreeSpaceFiles = _destroyFreeSpaceFiles;

            if(_pluginDirectory != null) {
                temp._pluginDirectory = (string)_pluginDirectory.Clone();
            }

            if(_randomOptions != null) {
                temp._randomOptions = (RandomOptions)_randomOptions.Clone();
            }

            temp._totalDelete = _totalDelete;
            temp._wipeAds = _wipeAds;
            temp._wipeFileName = _wipeFileName;

            if(_wipeMethodsPath != null) {
                temp._wipeMethodsPath = (string)_wipeMethodsPath.Clone();
            }

            temp._wipeUnusedFileRecord = _wipeUnusedFileRecord;
            temp._wipeUnusedIndexRecord = _wipeUnusedIndexRecord;
            temp._wipeUsedFileRecord = _wipeUsedFileRecord;
            temp._wipeUsedIndexRecord = _wipeUsedIndexRecord;

            if(_logFilePath != null) {
                temp._logFilePath = (string)_logFilePath.Clone();
            }

            temp._useLogFile = _useLogFile;
            temp._appendToLog = _appendToLog;
            temp._logFileSizeLimit = _logFileSizeLimit;
            return temp;
        }

        #endregion
    }
}
