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
using DebugUtils.Debugger;

namespace SecureDelete {
    /// <summary>
    /// Contains the ID's of the built-in wipe methods.
    /// </summary>
    public class WipeMethodType {
        public static int Random = 111909167;
        public static int Russian_GHOST = 153029903;
        public static int German_VSITR = 1642641729;
        public static int Gutmann = 547574234;
        public static int DOD = 643029365;
        public static int Schneier = 792101038;
    }


    public class WipeMethodManager {
        #region Constants

        public const string MethodFileExtension = ".sdm";

        #endregion

        #region Fields

        private List<WipeMethod> wipeMethods;
        Random rand;

        #endregion

        #region Constructor

        public WipeMethodManager() {
            wipeMethods = new List<WipeMethod>();
            rand = new Random((int)DateTime.Now.Ticks);
        }

        #endregion

        #region Properties

        public List<WipeMethod> Methods {
            get { return wipeMethods; }
        }

        private string _folder;
        public string Folder {
            get { return _folder; }
            set { _folder = value; }
        }

        #endregion

        public WipeMethod CreateMethod() {
            WipeMethod method = new WipeMethod();
            bool valid = false;
            int id = rand.Next();

            while(valid == false) {
                valid = true;

                for(int i = 0; i < wipeMethods.Count; i++) {
                    if(wipeMethods[i].Id == id) {
                        valid = false;
                        break;
                    }
                }

                id = rand.Next();
            }

            method.Id = id;
            wipeMethods.Add(method);
            return method;
        }


        public bool RemoveMethod(WipeMethod method, bool removeFromDisk) {
            Debug.AssertNotNull(method, "Method is null");

            if(removeFromDisk) {
                try {
                    string path = Path.Combine(_folder, method.Id.ToString() + MethodFileExtension);

                    if(File.Exists(path)) {
                        File.Delete(path);
                    }
                    else {
                        Debug.ReportError("Method not found. File: {0}", path);
                    }
                }
                catch(Exception e) {
                    Debug.ReportError("Error while deleting method. Method ID: {0}, Folder: {1}, Exception: {2}", method.Id, _folder, e.Message);
                    return false;
                }
            }

            wipeMethods.Remove(method);
            return true;
        }


        public bool SaveMethod(WipeMethod method) {
            Debug.AssertNotNull(method, "Method is null");
            string path = _folder;

            if(path[path.Length - 1] != '\\') {
                path += '\\';
            }

            path += method.Id.ToString() + ".sdm";
            return method.SaveNative(path);
        }


        public WipeMethod GetMethod(int id) {
            if(wipeMethods.Count == 0) {
                return null;
            }

            for(int i = 0; i < wipeMethods.Count; i++) {
                if(wipeMethods[i].Id == id) {
                    return wipeMethods[i];
                }
            }

            return null;
        }


        public int GetMethodIndex(int id) {
            for(int i = 0; i < wipeMethods.Count; i++) {
                if(wipeMethods[i].Id == id) {
                    return i;
                }
            }

            return -1;
        }


        public bool ScanFolder() {
            Debug.AssertNotNull(_folder, "Folder not set");

            // get the files
            try {
                string[] files = Directory.GetFiles(_folder, "*" + MethodFileExtension, SearchOption.TopDirectoryOnly);

                if(files != null && files.Length > 0) {
                    for(int i = 0; i < files.Length; i++) {
                        WipeMethod method = new WipeMethod();

                        if(method.ReadNative(files[i]) == false) {
                            Debug.ReportWarning("Failed to load method. File: {0}", files[i]);
                        }
                        else {
                            // add to the category
                            wipeMethods.Add(method);
                        }
                    }
                }
            }
            catch(Exception e) {
                Debug.ReportError("Error while loading the methods. Folder: {0}, Exception: {1}", _folder, e.Message);
                return false;
            }

            return true;
        }
    }
}
