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
using System.Xml.Serialization;
using DebugUtils.Debugger;
using System.IO;

namespace WindowsMessageListener {
    public class WMListener : DebugUtils.Debugger.IDebugListener {
        #region Win32 Interop

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public extern static int SendMessage(IntPtr hWnd, int Msg, int wParam, ref CopyDataStruct lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [Flags]
        public enum SendMessageTimeoutFlags : uint {
            SMTO_NORMAL = 0x0000,
            SMTO_BLOCK = 0x0001,
            SMTO_ABORTIFHUNG = 0x0002,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
        }

        public const int WM_COPYDATA = 0x004a;
        public static IntPtr HWND_BROADCAST = new IntPtr(0xffff);

        [StructLayout(LayoutKind.Sequential)]
        public struct CopyDataStruct {
            public IntPtr ID;
            public int Length;
            public IntPtr Data;
        }

        #endregion

        #region Constants

        public const string DebuggerId = "SecureDeleteDebugger";
        public const string WindowName = "SecureDelete Debugger";

        #endregion

        #region Fields

        private IntPtr windowHandle;

        #endregion

        #region Constructor

        public WMListener() {
            _enabled = true;
        }

        #endregion

        #region Private methods

        private byte[] SerializeMessage(DebugMessage message) {
            try {
                MemoryStream stream = new MemoryStream();
                XmlSerializer serializer = new XmlSerializer(typeof(DebugMessage));
                serializer.Serialize(stream, message);
                return stream.ToArray();
            }
            catch {
                return null;
            }
        }

        #endregion

        #region IDebugListener Members

        public bool IsOpen {
            get { return _isOpen; }
        }

        public bool Close() {
            _isOpen = false;
            return true;
        }

        public unsafe void SendMessage(DebugMessage message) {
            byte[] data = SerializeMessage(message);

            if(data != null && data.Length > 0) {
                // send message
                CopyDataStruct copyData = new CopyDataStruct();
                copyData.ID = IntPtr.Zero;
                copyData.Length = data.Length;

                fixed(byte* datePointer = data) {
                    copyData.Data = (IntPtr)datePointer;
                    SendMessage(windowHandle, WM_COPYDATA, 0, ref copyData);
                }
            }
        }

        public void SendStartMessage() {
            CopyDataStruct copyData = new CopyDataStruct();
            copyData.ID = IntPtr.Zero;
            copyData.Data = IntPtr.Zero;
            copyData.Length = 0;
            SendMessage(windowHandle, WM_COPYDATA, 0, ref copyData);
        }

        public bool DumpMessage(DebugMessage message) {
            SendMessage(message);
            return true;
        }

        private bool _enabled;
        public bool Enabled {
            get { return _enabled; }
            set { _enabled = value; }
        }

        private bool _isOpen;
        public bool IsOpened {
            get { return _isOpen; }
        }

        private int _listnerId;
        public int ListnerId {
            get { return _listnerId; }
            set { _listnerId = value; }
        }

        public bool Open() {
            windowHandle = FindWindow(null, WindowName);
            SendStartMessage();
            _isOpen = true;
            return true;
        }

        #endregion
    }
}
