// Copyright (c) Gratian Lup. All rights reserved.
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

namespace SecureDelete {
    public class Minidump {
        #region Win32 Interop
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        public static extern int GetCurrentProcessId();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode,
                                               uint SecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes,
                                               int hTemplateFile);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("Dbghelp.dll")]
        public static extern bool MiniDumpWriteDump(IntPtr process, int processId, IntPtr file,
                                                    MinidumpType type, IntPtr a, IntPtr b, IntPtr c);

        [Flags]
        public enum MinidumpType {
            MiniDumpNormal = 0x00,
            MiniDumpWithDataSegs = 0x01,
            MiniDumpWithFullMemory = 0x02,
            MiniDumpWithHandleData = 0x04,
            MiniDumpFilterMemory = 0x08,
            MiniDumpScanMemory = 0x10,
            MiniDumpWithUnloadedModules = 0x20,
            MiniDumpWithIndirectlyReferencedMemory = 0x40,
            MiniDumpFilterModulePaths = 0x80,
            MiniDumpWithProcessThreadData = 0x100
        }

        #endregion

        #region Public methods

        public static bool WriteMinidump(IntPtr process, int processId, string path) {
            IntPtr fileHandle = CreateFile(path, 2, 0, 0, 2, 0x80, 0);
            bool result = MiniDumpWriteDump(process, processId, fileHandle, 
                                            MinidumpType.MiniDumpNormal, IntPtr.Zero, 
                                            IntPtr.Zero, IntPtr.Zero);
            CloseHandle(fileHandle);
            return result;
        }


        public static bool WriteMinidump(string path) {
            return WriteMinidump(GetCurrentProcess(), GetCurrentProcessId(), path);
        }

        #endregion
    }
}
