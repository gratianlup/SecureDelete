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
using DebugUtils.Debugger;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.IO;
using System.Reflection;
using SecureDelete.WipeObjects;

namespace SecureDelete.Actions {
    [Bridge(Name = "Session", Exposed = true)]
    public class SessionControlBridge : IBridge, IFullAccessBridge {
        #region Constants

        private const string BridgeName = "Session";
        private const string FileInsertOperation = "File {0} added to wipe list.";
        private const string FolderInsertOperation = "Folder {0} added to wipe list.";
        private const string DriveInsertOperation = "Drive {0} added to wipe list.";
        private const string StopWipeOperation = "Wipe process stopped by PowerShell script. Reason: {0}";

        #endregion

        #region Fileds

        private bool reportActions;

        #endregion

        #region  Properties

        public string Name {
            get { return BridgeName; }
        }

        private bool _testMode;
        public bool TestMode {
            get { return _testMode; }
            set { _testMode = value; }
        }

        private BridgeLogger _logger;
        public SecureDelete.Actions.BridgeLogger Logger {
            get { return _logger; }
            set { _logger = value; }
        }

        private WipeSession _session;
        public SecureDelete.WipeSession Session {
            get { return _session; }
            set { _session = value; }
        }

        private bool _afterWipe;
        public bool AfterWipe {
            get { return _afterWipe; }
            set { _afterWipe = value; }
        }

        [BridgeMember(Name = "HostName", 
                      Signature = "public string HostName { get; }", 
                      Description = "Get the name of the host assembly.")]
        public string HostName {
            get {
                if(_logger != null && _testMode) {
                    _logger.LogMethodCall();
                }

                return Assembly.GetExecutingAssembly().FullName;
            }
        }

        [BridgeMember(Name = "ReportInsertActions", 
                      Signature = "public bool ReportInsertActions { get; set; }", 
                      Description = "Report insert actions to the session.")]
        public bool ReportInsertActions {
            get {
                if(_logger != null && _testMode) {
                    _logger.LogMethodCall();
                }

                return reportActions;
            }
            set {
                if(_logger != null && _testMode) {
                    _logger.LogMethodCall();
                }

                reportActions = value;
            }
        }

        #endregion

        #region Constructor

        public SessionControlBridge() {

        }

        public SessionControlBridge(WipeSession session, bool afterWipe) {
            _session = session;
            _afterWipe = afterWipe;
        }

        #endregion

        #region Public methods

        public void Open() {
            reportActions = true;
        }

        public void Close() {

        }

        [BridgeMember(Name = "StopWipe", 
                      Signature = "public void StopWipe(string reason)", 
                      Description = "Abort the wipe operation.")]
        [BridgeMemberParameter(Name = "reason", Description = "Indicates why the wipe process was stopped.")]
        public void StopWipe(string reason) {
            if(_logger != null && _testMode) {
                _logger.LogMethodCall(reason);
            }

            if(_session == null) {
                // do nothing
                return;
            }

            // first report the error
            ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, StopWipeOperation, reason);

            // stop the session
            _session.StoppedByBridge = true;
            _session.Stop();
        }


        [BridgeMember(Name = "InsertFile", 
                      Signature = "public void InsertFile(string path)", 
                      Description = "Add a file to the wipe list.")]
        [BridgeMemberParameter(Name = "path", Description = "The path of the file to be added to the wipe list.")]
        public void InsertFile(string path) {
            if(_logger != null && _testMode) {
                _logger.LogMethodCall(path);
            }

            if(_session == null || _afterWipe) {
                return;
            }

            FileWipeObject file = new FileWipeObject();
            file.Path = path;
            file.WipeMethodId = WipeOptions.DefaultWipeMethod;

            // add to the list
            _session.BridgeItems.Add(file);

            // report to session
            if(reportActions) {
                ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.Low, FileInsertOperation, path);
            }
        }


        [BridgeMember(Name = "InsertFolder", 
                      Signature = "public void InsertFolder(string path, string pattern, bool regex, bool wipeSubfolders, bool deleteFolders)", 
                      Description = "Add a folder to the wipe list.")]
        [BridgeMemberParameter(Order = 1, Name = "path", Description = "The path of the folder to be added to the wipe list.")]
        [BridgeMemberParameter(Order = 2, Name = "pattern", Description = "The search string to match against the names of files. Can be NULL.")]
        [BridgeMemberParameter(Order = 3, Name = "regex", Description = "The search string represents a regular expression.")]
        [BridgeMemberParameter(Order = 4, Name = "wipeSubfolders", Description = "Wipe all subfolders of the given folder.")]
        [BridgeMemberParameter(Order = 5, Name = "deleteFolders", Description = "Delete empty folders after wipe.")]
        public void InsertFolder(string path, string pattern, bool regex, bool wipeSubfolders, bool deleteFolders) {
            if(_logger != null && _testMode) {
                _logger.LogMethodCall(path, pattern, regex, wipeSubfolders, deleteFolders);
            }

            if(_session == null || _afterWipe) {
                return;
            }

            FolderWipeObject folder = new FolderWipeObject();
            folder.Path = path;
            folder.WipeMethodId = WipeOptions.DefaultWipeMethod;
            folder.Mask = pattern;
            folder.UseRegex = regex;
            folder.WipeSubfolders = wipeSubfolders;
            folder.DeleteFolders = deleteFolders;

            // add to the list
            _session.BridgeItems.Add(folder);

            // report to session
            if(reportActions) {
                ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.Low, 
                                                FolderInsertOperation, path);
            }
        }


        [BridgeMember(Name = "InsertDrive", 
                      Signature = "public void InsertDrive(string name, bool wipeFreeSpace, bool wipeClusterTips, bool wipeMft)", 
                      Description = "Add a drive to the wipe list.")]
        [BridgeMemberParameter(Order = 1, Name = "name", Description = "The name of the drive to be added to the wipe list (i.e C).")]
        [BridgeMemberParameter(Order = 2, Name = "wipeFreeSpace", Description = "Enable wiping of free space.")]
        [BridgeMemberParameter(Order = 3, Name = "wipeClusterTips", Description = "Enable wiping of cluster tips.")]
        [BridgeMemberParameter(Order = 4, Name = "wipeMft", Description = "Enable wiping of MFT area.")]
        public void InsertDrive(char name, bool wipeFreeSpace, bool wipeClusterTips, bool wipeMft) {
            if(_logger != null && _testMode) {
                _logger.LogMethodCall(name, wipeFreeSpace, wipeClusterTips, wipeMft);
            }

            if(_session == null || _afterWipe) {
                return;
            }

            DriveWipeObject drive = new DriveWipeObject();
            drive.AddDrive(name);
            drive.WipeMethodId = WipeOptions.DefaultWipeMethod;
            drive.WipeFreeSpace = wipeFreeSpace;
            drive.WipeClusterTips = wipeClusterTips;
            drive.WipeMFT = wipeMft;

            // add to the list
            _session.BridgeItems.Add(drive);

            // report to session
            if(reportActions) {
                ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.Low, 
                                                DriveInsertOperation, name);
            }
        }


        [BridgeMember(Name = "ReportError", 
                      Signature = "public void ReportError(int severity, string message)", 
                      Description = "Log an error message.")]
        [BridgeMemberParameter(Order = 1, Name = "severity", Description = "The severity of the error.\n\nValues:\n0 - High\n1 - Medium\n2 - Low")]
        [BridgeMemberParameter(Order = 2, Name = "message", Description = "The message to be logged.")]
        [BridgeMemberParameter(Order = 3, Name = "parameters", Description = "A variable list of parameters.")]
        public void ReportError(int severity, string message, params object[] parameters) {
            if(_logger != null && _testMode) {
                _logger.LogMethodCall(severity, message);
            }

            if(_session == null) {
                return;
            }

            // set severity
            ErrorSeverity s = ErrorSeverity.High;

            switch(severity) {
                case 0: { s = ErrorSeverity.High; break;   }
                case 1: { s = ErrorSeverity.Medium; break; }
                case 2: { s = ErrorSeverity.Low; break;    }
            }

            ActionErrorReporter.ReportError(_session, _afterWipe, s, message, parameters);
        }

        #endregion
    }
}
