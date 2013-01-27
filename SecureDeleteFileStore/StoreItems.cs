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
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace FileStore {
    public enum StoreItemType {
        File, Folder
    }

    public interface IStoreItem {
        StoreItemType Type { get; }
        string Name { get; set; }
    }
    [Flags]
    public enum StoreMode {
        Normal = 0,
        Encrypted = 1,
        Compressed = 2,
        EncryptedCompressed = Encrypted | Compressed
    }


    [Serializable]
    public class StoreFolder : IStoreItem {
        #region IStoreItem Members

        public StoreItemType Type {
            get { return StoreItemType.Folder; }
        }

        private string _name;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Fields

        public StoreFolder Child;
        public SortedList<string, StoreFolder> Subfolders;
        public SortedList<string, Guid> Files;

        #endregion

        #region Constructor

        public StoreFolder() {
            Subfolders = new SortedList<string, StoreFolder>();
            Files = new SortedList<string, Guid>();
        }

        public StoreFolder(string name)
            : this() {
            _name = name;
        }

        #endregion
    }


    [Serializable]
    public class StoreFile : IStoreItem {
        #region IStoreItem Members

        public StoreItemType Type {
            get { return StoreItemType.File; }
        }

        private string _name;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Fields

        public StoreMode StoreMode;
        public long RealSize;
        public long Size;
        public byte[] Data;

        #endregion

        #region Constructor

        public StoreFile() {
            Data = new byte[0];
        }

        public StoreFile(string name) {
            _name = name;
        }

        #endregion

        #region Public methods

        public void SetData(byte[] data, long realSize) {
            if(data == null || data.Length == 0) {
                ResetData();
            }
            else {
                Data = data;
                Size = data.Length;
                RealSize = realSize;
            }
        }


        public void ResetData() {
            Data = new byte[0];
            RealSize = 0;
            Size = 0;
        }

        #endregion
    }
}
