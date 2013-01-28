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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DebugUtils.Debugger;

namespace SecureDelete.Schedule {
    public partial class PowerNotificationsReceiver : Form {
        #region Constructor

        public PowerNotificationsReceiver() {
            InitializeComponent();
            this.Visible = false;
        }

        #endregion

        #region Win32 Interop

        [DllImport(@"User32", SetLastError = true, EntryPoint = "RegisterPowerSettingNotification", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr RegisterPowerSettingNotification(IntPtr hRecipient, ref Guid PowerSettingGuid, Int32 Flags);

        private const int WM_POWERBROADCAST = 0x0218;

        static Guid GUID_BATTERY_PERCENTAGE_REMAINING = new Guid("A7AD8041-B45A-4CAE-87A3-EECBB468A9E1");
        static Guid GUID_MONITOR_POWER_ON = new Guid(0x02731015, 0x4510, 0x4526, 0x99, 0xE6, 0xE5, 0xA1, 0x7E, 0xBD, 0x1A, 0xEA);
        static Guid GUID_ACDC_POWER_SOURCE = new Guid(0x5D3E9A59, 0xE9D5, 0x4B00, 0xA6, 0xBD, 0xFF, 0x34, 0xFF, 0x51, 0x65, 0x48);
        static Guid GUID_POWERSCHEME_PERSONALITY = new Guid(0x245D8541, 0x3943, 0x4422, 0xB0, 0x25, 0x13, 0xA7, 0x84, 0xF6, 0x79, 0xB7);
        static Guid GUID_MAX_POWER_SAVINGS = new Guid(0xA1841308, 0x3541, 0x4FAB, 0xBC, 0x81, 0xF7, 0x15, 0x56, 0xF2, 0x0B, 0x4A);
        static Guid GUID_MIN_POWER_SAVINGS = new Guid(0x8C5E7FDA, 0xE8BF, 0x4A96, 0x9A, 0x85, 0xA6, 0xE2, 0x3A, 0x8C, 0x63, 0x5C);
        static Guid GUID_BALANCED_POWER_SAVINGS = new Guid(0x381B4222, 0xF694, 0x41F0, 0x96, 0x85, 0xFF, 0x5B, 0xB2, 0x60, 0xDF, 0x2E);

        const int PBT_APMQUERYSUSPEND = 0x0000;
        const int PBT_APMQUERYSTANDBY = 0x0001;
        const int PBT_APMQUERYSUSPENDFAILED = 0x0002;
        const int PBT_APMQUERYSTANDBYFAILED = 0x0003;
        const int PBT_APMSUSPEND = 0x0004;
        const int PBT_APMSTANDBY = 0x0005;
        const int PBT_APMRESUMECRITICAL = 0x0006;
        const int PBT_APMRESUMESUSPEND = 0x0007;
        const int PBT_APMRESUMESTANDBY = 0x0008;
        const int PBT_APMBATTERYLOW = 0x0009;
        const int PBT_APMPOWERSTATUSCHANGE = 0x000A; // power status
        const int PBT_APMOEMEVENT = 0x000B;
        const int PBT_APMRESUMEAUTOMATIC = 0x0012;
        const int PBT_POWERSETTINGCHANGE = 0x8013; // DPPE

        const int DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000;
        const int DEVICE_NOTIFY_SERVICE_HANDLE = 0x00000001;

        // This structure is sent when the PBT_POWERSETTINGSCHANGE message is sent.
        // It describes the power setting that has changed and contains data about the change
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        internal struct POWERBROADCAST_SETTING {
            public Guid PowerSetting;
            public Int32 DataLength;
        }

        #endregion

        #region Properties

        private PowerManager _manager;
        public PowerManager Manager {
            get { return _manager; }
            set { _manager = value; }
        }

        #endregion

        #region Public methods

        protected override void OnShown(EventArgs e) {
            this.Hide();
            base.OnShown(e);
        }

        public void Start() {
            if(_manager == null) {
                throw new NoNullAllowedException("Manager not set");
            }

            // check if it's running on Vista+
            if(PowerManager.IsUnderVista() == false) {
                Debug.ReportWarning("Advanced power management available only under Vista+");
                return;
            }

            // register for notifications
            RegisterPowerSettingNotification(this.Handle, ref GUID_ACDC_POWER_SOURCE, DEVICE_NOTIFY_WINDOW_HANDLE);
            RegisterPowerSettingNotification(this.Handle, ref GUID_BATTERY_PERCENTAGE_REMAINING, DEVICE_NOTIFY_WINDOW_HANDLE);
            RegisterPowerSettingNotification(this.Handle, ref GUID_POWERSCHEME_PERSONALITY, DEVICE_NOTIFY_WINDOW_HANDLE);

            // set running state to true
            _manager.Running = true;
        }

        #endregion

        #region Private methods

        protected override void WndProc(ref Message m) {
            if(m.Msg == WM_POWERBROADCAST) {
                try {
                    if(m.WParam.ToInt64() == PBT_POWERSETTINGCHANGE) {
                        // power management notification
                        POWERBROADCAST_SETTING settings = (POWERBROADCAST_SETTING)Marshal.PtrToStructure(m.LParam, typeof(POWERBROADCAST_SETTING));
                        IntPtr dataPointer = (IntPtr)(m.LParam.ToInt64() + Marshal.SizeOf(settings));

                        if(settings.PowerSetting == GUID_POWERSCHEME_PERSONALITY &&
                           settings.DataLength == Marshal.SizeOf(typeof(Guid))) {
                            // power scheme changed
                            Guid newScheme = (Guid)Marshal.PtrToStructure(dataPointer, typeof(Guid));

                            if(newScheme == GUID_MAX_POWER_SAVINGS) {
                                _manager.PowerScheme = PowerScheme.PowerSaver;
                            }
                            else if(newScheme == GUID_MIN_POWER_SAVINGS) {
                                _manager.PowerScheme = PowerScheme.HighPerformance;
                            }
                            else {
                                _manager.PowerScheme = PowerScheme.Balanced;
                            }

                            Debug.Report("New power scheme: {0}", _manager.PowerScheme);
                        }
                        else if(settings.PowerSetting == GUID_ACDC_POWER_SOURCE &&
                                settings.DataLength == Marshal.SizeOf(typeof(Int32))) {
                            int value = (int)Marshal.PtrToStructure(dataPointer, typeof(Int32));
                            _manager.PowerSource = value == 0 ? PowerSource.AC : PowerSource.Battery;

                            Debug.Report("New power source: {0}", _manager.PowerSource);
                        }
                        else if(settings.PowerSetting == GUID_BATTERY_PERCENTAGE_REMAINING &&
                                 settings.DataLength == Marshal.SizeOf(typeof(Int32))) {
                            int value = (int)Marshal.PtrToStructure(dataPointer, typeof(Int32));
                            _manager.BatteryLife = value;

                            Debug.Report("Battery remaining percentage: {0}", value);
                        }
                    }
                }
                catch(Exception e) {
                    Debug.ReportError("Error while processing power management message. Exception: {0}", e.Message);
                }
            }
            else {
                base.WndProc(ref m);
            }
        }

        #endregion
    }
}