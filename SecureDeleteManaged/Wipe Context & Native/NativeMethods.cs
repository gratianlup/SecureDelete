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
using System.Runtime.CompilerServices;

namespace SecureDelete {
    /// <summary>
    /// Wrapper for the SecureDeleteNative.dll library
    /// </summary>
    public class NativeMethods {
        #region Context error codes

        public const int ERRORCODE_NO_MORE_CONTEXTS = 1;
        public const int ERRORCODE_INVALID_CONTEXT = 2;
        public const int ERRORCODE_INVALID_REQUEST = 3;
        public const int ERRORCODE_INITIALIZATION = 4;
        public const int ERRORCODE_SUCCESS = 254;

        #endregion

        #region Context status codes

        public const int STATUS_WIPE = 1;
        public const int STATUS_PAUSED = 2;
        public const int STATUS_STOPPED = 3;

        #endregion

        #region Object types

        public const byte TYPE_FILE = 0;
        public const byte TYPE_FOLDER = 1;
        public const byte TYPE_DRIVE = 2;
        public const byte TYPE_MFT = 4;
        public const byte TYPE_CLUSTER_TIPS = 5;

        #endregion

        #region File options

        public const ushort WIPE_ADS = 0x01;
        public const ushort WIPE_FILENAME = 0x02;

        #endregion

        #region Folder options

        public const ushort WIPE_SUBFOLDERS = 0x100;
        public const ushort DELETE_FOLDERS = 0x200;
        public const ushort USE_MASK = 0x400;

        #endregion

        #region Drive options

        public const ushort WIPE_FREE_SPACE = 0x01;
        public const ushort WIPE_CLUSTER_TIPS = 0x02;
        public const ushort WIPE_MFT = 0x04;

        #endregion

        #region RNG options

        public const int RNG_ISAAC = 0;
        public const int RNG_MERSENNE = 1;

        #endregion

        #region Message severity

        public const int SEVERITY_HIGH = 3;
        public const int SEVERITY_MEDIUM = 2;
        public const int SEVERITY_LOW = 1;

        #endregion

        #region Structures

        [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 24)]
        public struct RngOptions {
            public int randomProvider;
            public bool useSlowPool;
            public bool preventWriteToSwap;
            public bool reseed;
            public int reseedInterval;
            public int poolUpdateInterval;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
        public struct WOptions {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string methodPath;

            public int useLogFile;
            public int appendLog;
            public int logSizeLimit;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string logFile;

            public int totalDelete;
            public int WipeUsedFileRecord;
            public int wipeUnusedFileRecord;
            public int wipeUnusedIndexRecord;
            public int wipeUsedIndexRecord;
            public int destroyFreeSpaceFiles;
            public RngOptions rngOptions;
            public int warnSystemFile;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
        public struct WObject {
            public byte type;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string path;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string aux;

            public int wipeMethod;
            public ushort wipeOptions;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WStatus {
            public int context;
            public long objectIndex;
            public byte stopped;
            public byte type;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string message;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string auxMessage;

            public long totalBytes;
            public long wipedBytes;
            public long clusterTipsBytes;
            public long objectBytes;
            public long objectWipedBytes;
            public ushort steps;
            public ushort step;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
        public struct WSmallObject {
            public byte type;
            public int log; // the associated error (the index)

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string path;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class SYSTEMTIME {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;

            public override string ToString() {
                return ("[SYSTEMTIME: " + this.wDay.ToString(CultureInfo.CurrentCulture) + "/"    + 
                                          this.wMonth.ToString(CultureInfo.CurrentCulture) + "/"  + 
                                          this.wYear.ToString(CultureInfo.CurrentCulture) + " "   + 
                                          this.wHour.ToString(CultureInfo.CurrentCulture) + ":"   + 
                                          this.wMinute.ToString(CultureInfo.CurrentCulture) + ":" + 
                                          this.wSecond.ToString(CultureInfo.CurrentCulture) + "]");
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WError {
            public SYSTEMTIME time;
            public int severity;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 520)]
            public string message;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a context
        /// </summary>
        /// <param name="context">The Id of the created context. </param>
        /// <returns>The error code of the operation.</returns>
        [DllImport("SecureDeleteNative.dll")]
        public static extern int CreateWipeContext(out int context);


        /// <summary>
        /// Destroys an existing context
        /// </summary>
        /// <param name="context">The id of the context to destroy.</param>
        /// <returns>The error code of the operation.</returns>
        [DllImport("SecureDeleteNative.dll")]
        public static extern int DestroyWipeContext(int context);


        [DllImport("SecureDeleteNative.dll")]
        public static extern int InitializeWipeContext(int context);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int StartWipeContext(int context);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int StopWipeContext(int context);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int PauseWipeContext(int context);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int ResumeWipeContext(int context);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int SetWipeOptions(int context, ref WOptions wipeOptions);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int InsertWipeObject(int context, ref WObject wipeObject);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int GetContextStatus(int context);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int GetWipeStatus(int context, out WStatus wipeStatus);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int GetFailedObjectNumber(int context, out int failedNumber);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int GetFailedObject(int context, int position, out WSmallObject failedObject);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int GetErrorNumber(int context, out int errorNumber);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int GetError(int context, int position, out WError wipeError);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int GetChildrenNumber(int context, out int childrenNumber);

        [DllImport("SecureDeleteNative.dll")]
        public static extern int GetChildWipeStatus(int context, int child, out WStatus wipeStatus);

        #endregion
    }
}
