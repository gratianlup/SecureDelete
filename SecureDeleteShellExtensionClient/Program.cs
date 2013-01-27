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
using System.Windows.Forms;
using SecureDelete;
using SecureDelete.WipeObjects;
using DebugUtils.Debugger;
using DebugUtils.Debugger.Listeners;
using System.IO;

namespace ShellExtensionClient {
    static class Program {
        #region Debugging

        private static void ClearFolder(string path) {
            if(Directory.Exists(path) == false) {
                return;
            }

            try {
                string[] childFolders = Directory.GetDirectories(path);
                foreach(string s in childFolders) {
                    ClearFolder(s);
                }

                // remove the files
                string[] files = Directory.GetFiles(path);
                foreach(string s in files) {
                    File.Delete(s);
                }
            }
            catch { }
        }


        public static void DumpSettings(object sender, CustomDataEventArgs e) {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SecureDelete";

            // clear folder and write minidump
            ClearFolder(path);
            Minidump.WriteMinidump(Path.Combine(path, "minidump.dmp"));

            e.data = "Settings succesfully dumped";
        }

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // check argument number
            if(Environment.GetCommandLineArgs().Length <= 1) {
                return;
            }

            // set up the debugger
            Debug.BreakOnFailedAssertion = true;
            Debug.StoreMessages = true;
            Debug.SaveStackInfo = true;
            Debug.ExceptionManager.DumpHeader = "SecureDelete Dump File";
            Debug.ExceptionManager.DebugMessagesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + 
                                                       "\\SecureDelete\\SecureDeleteMessages.xml";
            Debug.ExceptionManager.DumpPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                              "\\SecureDelete\\SecureDeleteDump.txt";
            Debug.ExceptionManager.Dump = true;
            Debug.ExceptionManager.SaveDebugMessages = true;
            Debug.ExceptionManager.OnWriteDump += DumpSettings;
            Debug.ExceptionManager.ReportCrash = true;
            Debug.ExceptionManager.AttachExceptionManager();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // show start dialog
            StartDialog startDialog = new StartDialog();
            Application.Run(startDialog);

            if(startDialog.DialogResult == System.Windows.Forms.DialogResult.OK) {
                // initialize
                StatusDialog statusDialog = new StatusDialog();
                Application.Run(statusDialog);
            }
        }
    }
}
