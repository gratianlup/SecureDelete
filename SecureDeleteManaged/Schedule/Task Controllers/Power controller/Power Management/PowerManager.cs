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
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using DebugUtils.Debugger;
using System.Windows.Forms;

namespace SecureDelete.Schedule {
    public enum PowerSource {
        AC,
        Battery
    }

    public enum PowerScheme {
        PowerSaver,
        Balanced,
        HighPerformance
    }


    public class PowerManager {
        #region Win32 Interop

        [DllImport("kernel32.dll")]
        public static extern bool GetSystemPowerStatus(SystemPowerStatus lpSystemPowerStatus);

        public enum BatteryFlag : byte {
            High = 1,
            Low = 2,
            Critical = 4,
            Charging = 8,
            NoSystemBattery = 128,
            Unknown = 255
        }

        [StructLayout(LayoutKind.Sequential)]
        public class SystemPowerStatus {
            public ACLineStatus ACLineStatus;
            public BatteryFlag BatteryFlag;
            public Byte BatteryLifePercent;
            public Byte Reserved1;
            public Int32 BatteryLifeTime;
            public Int32 BatteryFullLifeTime;

            public override string ToString() {
                StringBuilder builder = new StringBuilder();

                builder.Append("Battery: ");
                builder.AppendLine(ACLineStatus.ToString());
                builder.Append("Percent: ");
                builder.AppendLine(BatteryLifePercent.ToString());

                return builder.ToString();
            }
        }

        public enum ACLineStatus : byte {
            Offline = 0,
            Online = 1,
            Unknown = 255
        }

        #endregion

        #region Fiedls

        private object lockObject = new object();
        private PowerNotificationsReceiver receiver;

        #endregion

        #region Constructor

        public PowerManager() {
            _powerSource = PowerSource.AC;
            _powerScheme = PowerScheme.HighPerformance;
            _batteryLife = 100;
        }

        #endregion

        #region Properties

        private Control _baseControl;
        /// <summary>
        /// Used when running from WinForms.
        /// </summary>
        public Control BaseControl {
            get { return _baseControl; }
            set { _baseControl = value; }
        }

        private PowerSource _powerSource;
        public PowerSource PowerSource {
            get {
                lock(lockObject) {
                    return _powerSource;
                }
            }
            set {
                lock(lockObject) {
                    _powerSource = value;
                    NotifyStatusChagned();
                }
            }
        }

        private PowerScheme _powerScheme;
        public PowerScheme PowerScheme {
            get {
                lock(lockObject) {
                    return _powerScheme;
                }
            }
            set {
                lock(lockObject) {
                    _powerScheme = value;
                    NotifyStatusChagned();
                }
            }
        }

        private int _batteryLife;
        public int BatteryLife {
            get {
                lock(lockObject) {
                    if(_powerSource == PowerSource.AC) {
                        return 100;
                    }
                    else {
                        return _batteryLife;
                    }
                }
            }
            set {
                lock(lockObject) {
                    _batteryLife = value;
                    NotifyStatusChagned();
                }
            }
        }

        private bool _running;
        public bool Running {
            get {
                lock(lockObject) {
                    return _running;
                }
            }
            set {
                lock(lockObject) {
                    _running = value;
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler OnPowerStatusChanged;

        #endregion

        #region Private methods

        private void NotifyStatusChagned() {
            if(OnPowerStatusChanged != null) {
                OnPowerStatusChanged(this, null);
            }
        }

        private void WinFormsStart() {
            receiver.Show();
            receiver.Start();
        }

        private void StopImpl() {
            if(receiver != null) {
                if(receiver.IsHandleCreated) {
                    receiver.Close();
                    receiver.Dispose();
                    GC.SuppressFinalize(receiver);
                    receiver = null;
                }
            }
        }

        private void WinFormStop() {
            StopImpl();
        }

        #endregion

        #region Public methods

        public static bool IsUnderVista() {
            return Environment.OSVersion.Version.Major >= 6;
        }


        /// <summary>
        /// Method should only be used on XP systems.
        /// </summary>
        public bool GetPowerStatus() {
            SystemPowerStatus status = new SystemPowerStatus();
            bool result = GetSystemPowerStatus(status);

            if(result) {
                _powerSource = status.ACLineStatus == ACLineStatus.Online ? PowerSource.AC : PowerSource.Battery;
                _batteryLife = (int)(Math.Max((double)status.BatteryLifePercent * 0.3921, 100));
                Debug.Report("Power Status: \n{0}", status);
            }
            else {
                Debug.ReportWarning("Failed to get power status");
            }

            return result;
        }


        public bool Start() {
            // check if we are running on Vista+
            if(IsUnderVista() == false) {
                return false;
            }

            Debug.ReportWarning("Started listening to power notifications");

            // show the window
            receiver = new PowerNotificationsReceiver();
            receiver.Manager = this;

            if(_baseControl != null) {
                // WinForms start
                _baseControl.Invoke(new MethodInvoker(WinFormsStart));
            }
            else {
                receiver.Show();
                receiver.Start();
            }

            return true;
        }


        public void Stop() {
            if(Running == false) {
                return;
            }

            if(_baseControl != null) {
                // WinForms start
                _baseControl.Invoke(new MethodInvoker(WinFormStop));
            }
            else {
                StopImpl();
            }

            Running = false;
            Debug.ReportWarning("Stopped listening to power notifications");
        }

        #endregion
    }
}
