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
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;

namespace SecureDelete.Actions {
    [Serializable]
    public sealed class MailAction : IAction, ICloneable {
        #region Constants

        public const int DefaultMailPort = 25;
        public const string DefaultMailSubject = "SecureDelete";
        private const string DefaultBody = "Test message";
        private const string SendFailedMessage = "Failed to send mail. Exception: {0}";

        #endregion

        #region Fields

        [NonSerialized]
        private SmtpClient client;

        [NonSerialized]
        private ManualResetEvent sentEvent;

        #endregion

        #region Properties

        private bool _enabled;
        public bool Enabled {
            get { return _enabled; }
            set { _enabled = value; }
        }

        [NonSerialized]
        private WipeSession _session;
        public SecureDelete.WipeSession Session {
            get { return _session; }
            set { _session = value; }
        }

        [NonSerialized]
        private bool _afterWipe;
        public bool AfterWipe {
            get { return _afterWipe; }
            set { _afterWipe = value; }
        }

        [NonSerialized]
        private bool _blockingMode;
        public bool BlockingMode {
            get { return _blockingMode; }
            set { _blockingMode = value; }
        }

        private bool _hasMaximumExecutionTime;
        public bool HasMaximumExecutionTime {
            get { return _hasMaximumExecutionTime; }
            set { _hasMaximumExecutionTime = value; }
        }

        private TimeSpan _maximumExecutionTime;
        public TimeSpan MaximumExecutionTime {
            get { return _maximumExecutionTime; }
            set { _maximumExecutionTime = value; }
        }

        private string _toAdress;
        public string ToAdress {
            get { return _toAdress; }
            set { _toAdress = value; }
        }

        private string _fromAdress;
        public string FromAdress {
            get { return _fromAdress; }
            set { _fromAdress = value; }
        }

        private string _server;
        public string Server {
            get { return _server; }
            set { _server = value; }
        }

        private int _port;
        public int Port {
            get { return _port; }
            set { _port = value; }
        }

        private string _user;
        public string User {
            get { return _user; }
            set { _user = value; }
        }

        private string _password;
        public string Password {
            get { return _password; }
            set { _password = value; }
        }

        private string _subject;
        public string Subject {
            get { return _subject; }
            set { _subject = value; }
        }

        private bool _sendFullReport;
        public bool SendFullReport {
            get { return _sendFullReport; }
            set { _sendFullReport = value; }
        }

        [NonSerialized]
        private SmtpStatusCode _resultCode;
        public SmtpStatusCode ResultCode {
            get { return _resultCode; }
            set { _resultCode = value; }
        }

        #endregion

        #region Constructor

        public MailAction() {
            _port = DefaultMailPort;
            _subject = DefaultMailSubject;
        }

        #endregion

        #region Public methods

        public static bool IsValidMailAdress(string adress) {
            if(adress == null || adress.Length == 0) {
                return false;
            }

            // validate using regex
            return Regex.IsMatch(adress, @"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$");
        }


        public bool Start() {
            // validate the properties
            // mail adress
            if(IsValidMailAdress(_toAdress) == false) {
                return false;
            }

            // user and password
            if(_user != null && _user.Length > 0) {
                if(_password == null || _password.Length == 0) {
                    return false;
                }
            }

            // send the mail
            SmtpClient client = new SmtpClient();
            MailMessage message = new MailMessage();

            _fromAdress = "test@lg.com";
            message.To.Add(new MailAddress(_toAdress));
            message.From = new MailAddress(_fromAdress);
            message.Subject = _subject == null ? DefaultMailSubject : _subject;

            // get the wipe report and set it as body
            if(_session != null && _afterWipe) {
                message.Body = ReportExporter.GetHtmlString(_session.GenerateReport(), WebReportStyle.HTMLReportStyle);
            }
            else {
                message.Body = DefaultBody;
            }

            // initialize the Smtp client
            client = new SmtpClient(_server, _port);

            if(_user != null && _user.Length > 0) {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_user, _password);
            }

            // send it
            try {
                _resultCode = SmtpStatusCode.Ok;
                sentEvent.Reset();
                client.SendAsync(message, null);
            }
            catch(SmtpException e) {
                _resultCode = e.StatusCode;
                ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, SendFailedMessage, e.Message);
                return false;
            }
            catch(Exception e) {
                _resultCode = SmtpStatusCode.GeneralFailure;
                ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, SendFailedMessage, e.Message);
                return false;
            }

            return true;
        }


        public void Stop() {
            if(client != null) {
                try {
                    client.SendAsyncCancel();
                }
                catch(Exception e) {
                    Debug.ReportError("Failed to stop mail operation. Exception: {0}", e.Message);
                }
            }
        }


        public bool EndStart() {
            return true;
        }

        #endregion

        #region ICloneable Members

        public object Clone() {
            MailAction temp = new MailAction();

            return temp;
        }

        #endregion
    }
}
