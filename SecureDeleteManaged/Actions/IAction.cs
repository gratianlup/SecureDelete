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
using System.Diagnostics;

namespace SecureDelete.Actions {
    /// <summary>
    /// Interface from which all actions need to derive.
    /// </summary>
    public interface IAction {
        bool Enabled { get; set; }
        WipeSession Session { get; set; }
        bool AfterWipe { get; set; }
        bool BlockingMode { get; set; }
        bool HasMaximumExecutionTime { get; set; }
        TimeSpan MaximumExecutionTime { get; set; }
        bool Start();
        void Stop();
        bool EndStart();
        object Clone();
    }


    public class ActionErrorReporter {
        public static void ReportError(WipeSession session, bool afterWipe, ErrorSeverity severity, 
                                       string format, params object[] args) {
            if(format == null) {
                throw new ArgumentNullException("format");
            }

            // report to the debugger
            switch(severity) {
                case ErrorSeverity.High: {
                    DebugUtils.Debugger.Debug.ReportError(format, args);
                    break;
                }
                case ErrorSeverity.Low: {
                    DebugUtils.Debugger.Debug.Report(format, args);
                    break;
                }
                case ErrorSeverity.Medium: {
                    DebugUtils.Debugger.Debug.ReportWarning(format, args);
                    break;
                }
            }

            // report to the session
            if(session != null) {
                WipeError error = new WipeError(DateTime.Now, severity, string.Format(format, args));

                if(afterWipe) {
                    if(session.AfterWipeErrors == null) {
                        session.AfterWipeErrors = new List<WipeError>();
                    }

                    session.AfterWipeErrors.Add(error);
                }
                else {
                    if(session.BeforeWipeErrors == null) {
                        session.BeforeWipeErrors = new List<WipeError>();
                    }

                    session.BeforeWipeErrors.Add(error);
                }
            }
        }
    }
}
