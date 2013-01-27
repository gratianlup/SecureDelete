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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SecureDelete.Schedule;
using SecureDelete.Actions;
using SecureDelete;

namespace SecureDeleteWinForms {
    public partial class MailActionEditor : UserControl, IActionEditor {
        public MailActionEditor() {
            InitializeComponent();
        }

        #region IActionEditor Members

        public ActionEditorType Type {
            get { return ActionEditorType.MailActionEditor; }
        }

        public int RequiredSize {
            get { return 210; }
        }

        private MailAction _action;
        public IAction Action {
            get { return _action; }
            set {
                if((value is MailAction) == false) {
                    throw new Exception("Invalid value");
                }

                _action = (MailAction)value;
                HandleNewAction();
            }
        }

        public string TypeString {
            get {
                if(_action == null) {
                    return string.Empty;
                }

                return "Send Mail";
            }
        }

        public string ActionString {
            get {
                if(_action == null) {
                    return string.Empty;
                }

                return "Send mail to " + _action.ToAdress;
            }
        }

        public event EventHandler OnStatusChanged;

        #endregion

        private void HandleNewAction() {
            EnabledCheckbox.Checked = _action.Enabled;

            if(_action.ToAdress != null) {
                AdressTextbox.Text = _action.ToAdress;
            }
            else {
                AdressTextbox.Text = "";
            }

            if(_action.Server != null) {
                ServerTextbox.Text = _action.Server;
            }
            else {
                ServerTextbox.Text = "";
            }

            PortValue.Value = _action.Port;

            if(_action.User != null) {
                UserTextbox.Text = _action.User;
            }
            else {
                UserTextbox.Text = "";
            }
            
            if(_action.Password != null) {
                PasswordTextbox.Text = _action.Password;
            }
            else {
                PasswordTextbox.Text = "";
            }
            if(_action.Subject != null) {
                SubjectTextbox.Text = _action.Subject;
            }
            else {
                SubjectTextbox.Text = "";
            }

            ReportCheckbox.Checked = _action.SendFullReport;
        }

        private void SetEnabledState(bool state, bool checkboxState) {
            EnabledCheckbox.Enabled = checkboxState;
            AdressTextbox.Enabled = state;
            ServerTextbox.Enabled = state;
            PortValue.Enabled = state;
            UserTextbox.Enabled = state;
            PasswordTextbox.Enabled = state;
            SubjectTextbox.Enabled = state;
            ReportCheckbox.Enabled = state;
            TestButton.Enabled = state;
            LocalHostButton.Enabled = state;
            DefaultPortButton.Enabled = state;
            label1.Enabled = state;
            label2.Enabled = state;
            label3.Enabled = state;
            label4.Enabled = state;
            label5.Enabled = state;
            label6.Enabled = state;
        }

        private void SendStatusChanged() {
            if(OnStatusChanged != null) {
                OnStatusChanged(this, null);
            }
        }

        private void EnabledCheckbox_CheckedChanged(object sender, EventArgs e) {
            _action.Enabled = EnabledCheckbox.Checked;
            SetEnabledState(_action.Enabled, true);
            SendStatusChanged();
        }

        private void PathTextbox_TextChanged(object sender, EventArgs e) {
            _action.ToAdress = AdressTextbox.Text;
            SendStatusChanged();

            if(MailAction.IsValidMailAdress(_action.ToAdress)) {
                ValidLabel.Text = "Valid adress";
                ValidLabel.ForeColor = Color.FromName("ControlText");
            }
            else {
                ValidLabel.Text = "Invalid adress";
                ValidLabel.ForeColor = Color.Red;
            }
        }

        private void LocalHostButton_Click(object sender, EventArgs e) {
            ServerTextbox.Text = "localhost";
        }

        private void ServerTextbox_TextChanged(object sender, EventArgs e) {
            _action.Server = ServerTextbox.Text;
        }

        private void PortValue_ValueChanged(object sender, EventArgs e) {
            _action.Port = (int)PortValue.Value;
        }

        private void UserTextbox_TextChanged(object sender, EventArgs e) {
            _action.User = UserTextbox.Text;
        }

        private void PasswordTextbox_TextChanged(object sender, EventArgs e) {
            _action.Password = PasswordTextbox.Text;
        }

        private void SubjectTextbox_TextChanged(object sender, EventArgs e) {
            _action.Subject = SubjectTextbox.Text;
        }

        private void ReportCheckbox_CheckedChanged(object sender, EventArgs e) {
            _action.SendFullReport = ReportCheckbox.Checked;
        }

        private void button1_Click(object sender, EventArgs e) {
            PortValue.Value = MailAction.DefaultMailPort;
        }

        private void TestButton_Click(object sender, EventArgs e) {
            // create a test report
            WipeReport report = new WipeReport();

            report.Statistics = new WipeStatistics();
            report.Errors.Add(new WipeError(DateTime.Now, ErrorSeverity.High, "Test message"));
            report.Errors.Add(new WipeError(DateTime.Now, ErrorSeverity.Medium, "Test message"));
            report.Errors.Add(new WipeError(DateTime.Now, ErrorSeverity.Low, "Test message"));

            if(_action.Start() == false) {
                MessageBox.Show("Failed to send test mail message.", "SecureDelete", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                MessageBox.Show("Test mail message successfully sent.", "SecureDelete", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
