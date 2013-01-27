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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FileStore {
    public partial class StoreInfo : Form {
        public StoreInfo() {
            InitializeComponent();
        }

        private FileStore store;
        public FileStore Store {
            get { return store; }
            set { store = value; }
        }

        private void StoreInfo_Load(object sender, EventArgs e) {
            if(store == null) {
                return;
            }

            int totalSize = 0;
            int encryptedFiles = 0;
            int folderCount = -1;

            foreach(KeyValuePair<Guid, StoreFile> kvp in store.files) {
                totalSize += (int)kvp.Value.RealSize;

                if(kvp.Value.StoreMode == StoreMode.Encrypted) {
                    encryptedFiles++;
                }
            }

            Queue<StoreFolder> folders = new Queue<StoreFolder>();
            folders.Enqueue(store.Root);

            while(folders.Count > 0) {
                StoreFolder folder = folders.Dequeue();
                folderCount++;

                // add childs
                foreach(KeyValuePair<string, StoreFolder> kvp in folder.Subfolders) {
                    folders.Enqueue(kvp.Value);
                }
            }

            label8.Text = string.Format("{0} ({1} encrypted)", store.files.Count, encryptedFiles);
            label7.Text = folderCount.ToString();
            label6.Text = totalSize.ToString();
        }

        private void button2_Click(object sender, EventArgs e) {
            store.Root = new StoreFolder("root");
            store.files.Clear();
            StoreInfo_Load(null, null);
        }
    }
}
