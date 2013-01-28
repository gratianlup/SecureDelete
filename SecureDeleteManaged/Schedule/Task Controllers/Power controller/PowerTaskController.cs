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
using System.Windows.Forms;
using DebugUtils.Debugger;

namespace SecureDelete.Schedule {
    [Serializable]
    public class PowertTCSettings : ITaskControllerSettings {
        #region Properties

        private bool _disableOnBattery;
        public bool DisableOnBattery {
            get { return _disableOnBattery; }
            set { _disableOnBattery = value; }
        }

        private bool _stopIfPowerSaverScheme;
        public bool StopIfPowerSaverScheme {
            get { return _stopIfPowerSaverScheme; }
            set { _stopIfPowerSaverScheme = value; }
        }

        private bool _stopIfLowBatteryPower;
        public bool StopIfLowBatteryPower {
            get { return _stopIfLowBatteryPower; }
            set { _stopIfLowBatteryPower = value; }
        }

        private int _minBatteryPower;
        public int MinBatteryPower {
            get { return _minBatteryPower; }
            set { _minBatteryPower = value; }
        }

        #endregion

        #region ITaskControllerSettings Members

        public ControllerType Type {
            get { return ControllerType.Power; }
        }

        #endregion
    }


    public class PowerTaskController : ITaskController {
        #region Fields

        private PowerManager powerManager;

        #endregion

        #region Constructor

        public PowerTaskController() {
            powerManager = new PowerManager();
            powerManager.OnPowerStatusChanged += PowerNotificationHandler;
        }

        public PowerTaskController(TaskManager parent)
            : this() {
            _parent = parent;
        }

        public PowerTaskController(TaskManager parent, Control baseControl)
            : this() {
            _parent = parent;
            BaseControl = baseControl;
        }

        #endregion

        #region Private methods

        private void PowerNotificationHandler(object sender, EventArgs e) {
            if(_settings == null) {
                throw new Exception("Settings not set");
            }
            else if(_parent == null) {
                throw new Exception("Task manager (parent) not set");
            }

            if(_settings.StopIfPowerSaverScheme && powerManager.PowerScheme == PowerScheme.PowerSaver) {
                _parent.StopAllTasks();
                Debug.ReportWarning("All tasks stopped (power scheme)");
            }
            else if(_settings.StopIfLowBatteryPower && powerManager.BatteryLife < _settings.MinBatteryPower) {
                _parent.StopAllTasks();
                Debug.ReportWarning("All tasks stopped (battery power)");
            }
        }

        #endregion

        #region ITaskController Members

        public ControllerType Type {
            get { return ControllerType.Power; }
        }

        private TaskManager _parent;
        public TaskManager Parent {
            get { return _parent; }
            set { _parent = value; }
        }

        private PowertTCSettings _settings;
        public ITaskControllerSettings Settings {
            get { return _settings; }
            set { _settings = (PowertTCSettings)value; }
        }

        private Control _baseControl;
        /// <summary>
        /// Used when running from WinForms.
        /// </summary>
        public Control BaseControl {
            get { return _baseControl; }
            set {
                _baseControl = value;
                powerManager.BaseControl = _baseControl;
            }
        }

        public bool Enabled {
            get {
                if(_settings == null) {
                    throw new Exception("Settings not set");
                }

                return _settings.DisableOnBattery != false;
            }
        }

        public bool AllowTaskStart {
            get {
                if(_settings == null) {
                    throw new Exception("Settings not set");
                }

                if(_settings.DisableOnBattery == false) {
                    if(powerManager.Running) {
                        if(_settings.StopIfPowerSaverScheme && powerManager.PowerScheme == PowerScheme.PowerSaver) {
                            return false;
                        }

                        if(_settings.StopIfLowBatteryPower && powerManager.BatteryLife < _settings.MinBatteryPower) {
                            return false;
                        }
                    }

                    return true;
                }
                else {
                    // check if computer is on battery
                    if(powerManager.GetPowerStatus() == false) {
                        // failed to get status, return true
                        return true;
                    }
                    else {
                        // allow only if not on battery
                        return powerManager.PowerSource != PowerSource.Battery;
                    }
                }
            }
        }

        public void TaskStarted(ScheduledTask task) {
            if(_settings == null) {
                throw new Exception("Settings not set");
            }

            if(_settings.StopIfLowBatteryPower || _settings.StopIfPowerSaverScheme) {
                if(powerManager.Running == false) {
                    powerManager.Start();
                }
            }
            else {
                if(powerManager.Running) {
                    powerManager.Stop();
                }
            }
        }

        public void TaskStopped(ScheduledTask task, int remaining) {
            if(_settings == null) {
                throw new Exception("Settings not set");
            }

            if(remaining == 0 && powerManager.Running) {
                powerManager.Stop();
            }
        }

        #endregion
    }
}
