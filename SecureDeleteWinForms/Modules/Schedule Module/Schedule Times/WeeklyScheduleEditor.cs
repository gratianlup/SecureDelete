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

namespace SecureDeleteWinForms {
    public partial class WeeklyScheduleEditor : UserControl {
        public WeeklyScheduleEditor() {
            InitializeComponent();
        }

        private WeeklySchedule _schedule;
        public WeeklySchedule Schedule {
            get { return _schedule; }
            set { _schedule = value; UpdateControls(); }
        }

        private void UpdateControls() {
            if(_schedule == null) {
                return;
            }

            StartDatePicker.Value = _schedule.Time.StartTime;
            StartDatePicker.Value = _schedule.Time.StartTime;
            EndDatePicker.Value = _schedule.Time.EndTime;
            EndDatePicker.Value = _schedule.Time.EndTime;
            EndTimeCheckbox.Checked = _schedule.Time.HasEndTime;
            EndDatePicker.Enabled = _schedule.Time.HasEndTime;
            EndTimePicker.Enabled = _schedule.Time.HasEndTime;
            RecurenceUpDown.Value = _schedule.Time.RecurenceValue;

            checkBox2.Checked = IsDaySet(WeekDay.Sunday);
            checkBox3.Checked = IsDaySet(WeekDay.Monday);
            checkBox4.Checked = IsDaySet(WeekDay.Tuesday);
            checkBox5.Checked = IsDaySet(WeekDay.Wednesday);
            checkBox6.Checked = IsDaySet(WeekDay.Thursday);
            checkBox7.Checked = IsDaySet(WeekDay.Friday);
            checkBox8.Checked = IsDaySet(WeekDay.Saturday);
        }

        private bool IsDaySet(WeekDay day) {
            return ((int)_schedule.Days & (int)day) == (int)day;
        }

        private void SetDay(WeekDay day) {
            _schedule.Days = (WeekDay)((int)_schedule.Days | (int)day);
        }

        private void RemoveDay(WeekDay day) {
            _schedule.Days = (WeekDay)((int)_schedule.Days & ~((int)day));
        }

        private void SetRemoveDay(WeekDay day, bool set) {
            if(set) {
                SetDay(day);
            }
            else {
                RemoveDay(day);
            }
        }

        private DateTime GetDateTime(DateTimePicker date, DateTimePicker time) {
            return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day,
                                time.Value.Hour, time.Value.Minute, time.Value.Second, 0);
        }

        private void SetStartTime() {
            _schedule.Time.StartTime = GetDateTime(StartDatePicker, StartTimePicker);
        }

        private void SetEndTime() {
            _schedule.Time.EndTime = GetDateTime(StartDatePicker, StartTimePicker);
        }


        private void StartDatePicker_ValueChanged(object sender, EventArgs e) {
            SetStartTime();
        }

        private void StartTimePicker_ValueChanged(object sender, EventArgs e) {
            SetStartTime();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            _schedule.Time.HasEndTime = EndTimeCheckbox.Checked;
            EndDatePicker.Enabled = _schedule.Time.HasEndTime;
            EndTimePicker.Enabled = _schedule.Time.HasEndTime;
        }

        private void RecurenceUpDown_ValueChanged(object sender, EventArgs e) {
            _schedule.Time.RecurenceValue = (int)RecurenceUpDown.Value;
        }

        private void EndDatePicker_ValueChanged(object sender, EventArgs e) {
            SetEndTime();
        }

        private void EndTimePicker_ValueChanged(object sender, EventArgs e) {
            SetEndTime();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            SetRemoveDay(WeekDay.Sunday, checkBox2.Checked);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) {
            SetRemoveDay(WeekDay.Monday, checkBox3.Checked);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e) {
            SetRemoveDay(WeekDay.Tuesday, checkBox4.Checked);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e) {
            SetRemoveDay(WeekDay.Wednesday, checkBox5.Checked);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e) {
            SetRemoveDay(WeekDay.Thursday, checkBox6.Checked);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e) {
            SetRemoveDay(WeekDay.Friday, checkBox7.Checked);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e) {
            SetRemoveDay(WeekDay.Saturday, checkBox8.Checked);
        }
    }
}
