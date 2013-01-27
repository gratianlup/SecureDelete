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
using DebugUtils.Debugger;
using System.IO;
using System.Threading;
using SecureDeleteWinForms;
using DebugUtils.Debugger.Listeners;
using WindowsMessageListener;

namespace SecureDelete {
    class SecureDeleteCrashNotifier : ICrashNotifier {
        #region ICrashNotifier Members

        private string _crashDetails;
        public string CrashDetails {
            get { return _crashDetails; }
            set { _crashDetails = value; }
        }

        private string _debugMessagesFilePath;
        public string DebugMessagesFilePath {
            get { return _debugMessagesFilePath; }
            set { _debugMessagesFilePath = value; }
        }

        private string _dumpFilePath;
        public string DumpFilePath {
            get { return _dumpFilePath; }
            set { _dumpFilePath = value; }
        }

        public bool Launch() {
            try {
                // launch crash reporter
                string path = Path.Combine(Application.StartupPath, "SecureDeleteCrashReporter.exe");

                if(File.Exists(path)) {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo = new System.Diagnostics.ProcessStartInfo(path);
                    process.Start();
                }
            }
            catch { }
            finally {
                Environment.Exit(0);
            }

            return true;
        }

        private Exception _unhandledException;
        public Exception UnhandledException {
            get { return _unhandledException; }
            set { _unhandledException = value; }
        }

        #endregion
    }

    static class Program {
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
            ClearFolder(path);
            Minidump.WriteMinidump(Path.Combine(path, "minidump.dmp"));
            e.data = "Settings succesfully dumped";
        }


        public static void UnhadledExceptionHandler(object sender, UnhandledExceptionEventArgs e) {
            // create folder
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SecureDelete";

            if(Directory.Exists(path) == false) {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

            // configure the debugger
            Debug.BreakOnFailedAssertion = true;
            Debug.StoreMessages = true;
            Debug.SaveStackInfo = true;
            Debug.ExceptionManager.DumpHeader = "SecureDelete Dump File";
            Debug.ExceptionManager.DebugMessagesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SecureDelete\\SecureDeleteMessages.xml";
            Debug.ExceptionManager.DumpPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SecureDelete\\SecureDeleteDump.txt";
            Debug.ExceptionManager.Dump = true;
            Debug.ExceptionManager.SaveDebugMessages = true;
            Debug.ExceptionManager.OnWriteDump += Program.DumpSettings;
            Debug.ExceptionManager.OnUnhandledException += Program.UnhadledExceptionHandler;
            Debug.ExceptionManager.CrashNotifier = new SecureDeleteCrashNotifier();
            Debug.ExceptionManager.ReportCrash = true;
            Debug.ExceptionManager.AttachExceptionManager();

            Debug.AddListner(new WMListener());

            // run
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // initialize locations if running the first time
            SecureDeleteLocations.InitializeLocations();

            // show password dialog if enabled
            MainForm mainForm = new MainForm();
            mainForm.Inteface.LoadOptions();

            if(mainForm.Inteface.Options.PasswordRequired && mainForm.Inteface.Options.Password != null) {
                PasswordDialog dialog = new PasswordDialog();
                dialog.Password = mainForm.Inteface.Options.Password;

                Application.Run(dialog);
                if(dialog.DialogResult != DialogResult.OK) {
                    // invalid password or canceled by user
                    return;
                }
            }

            Application.Run(mainForm);
        }
    }
}
