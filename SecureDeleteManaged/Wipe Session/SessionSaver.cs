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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using DebugUtils.Debugger;

namespace SecureDelete {
    public class SessionSaver {
        #region Public methods

        public static byte[] SerializeSession(WipeSession session) {
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            try {
                // serialize in memory
                serializer.Serialize(stream, session);
                return stream.ToArray();
            }
            catch(Exception e) {
                Debug.ReportError("Error while serializing session. Exception: {0}", e.Message);
                return null;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }


        public static bool SaveSession(WipeSession session, string path) {
            // check the parameters
            if(session == null || path == null) {
                throw new ArgumentNullException("session | path");
            }

            try {
                // create the store
                FileStore.FileStore store = new FileStore.FileStore();
                store.Encrypt = true;
                store.UseDPAPI = true;

                // add the file
                FileStore.StoreFile file = store.CreateFile("session.dat");
                byte[] data = SerializeSession(session);

                if(data == null) {
                    return false;
                }

                // write the file contents
                store.WriteFile(file, data, FileStore.StoreMode.Encrypted);
                return store.Save(path);
            }
            catch(Exception e) {
                Debug.ReportError("Error while saving session. Exception: {0}", e.Message);
                return false;
            }
        }

        #endregion
    }
}
