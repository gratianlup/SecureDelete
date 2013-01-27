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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SecureDelete.Schedule;

namespace SecureDeleteWinForms {
    public partial class MonthlyScheduleEditor : UserControl {
        public MonthlyScheduleEditor() {
            InitializeComponent();
        }

        private MonthlySchedule _schedule;
        public MonthlySchedule Schedule {
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

            // load days
            for(int i = 0; i < 31; i++) {
                DayList.Items[i].Checked = _schedule.Days.ContainsKey(i + 1);
            }

            // last day
            DayList.Items[DayList.Items.Count - 1].Checked = _schedule.Days.ContainsKey(MonthlySchedule.LastDay);
        }

        private void AddDay(int day) {
            if(_schedule.Days.ContainsKey(day) == false) {
                _schedule.Days.Add(day, day);
            }
        }

        private void RemoveDay(int day) {
            if(_schedule.Days.ContainsKey(day) == false) {
                _schedule.Days.Remove(day);
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

        private void EndDatePicker_ValueChanged(object sender, EventArgs e) {
            SetEndTime();
        }

        private void EndTimePicker_ValueChanged(object sender, EventArgs e) {
            SetEndTime();
        }

        private void DayList_ItemChecked(object sender, ItemCheckedEventArgs e) {
            int day;

            if(e.Item.Index == DayList.Items.Count - 1) {
                day = MonthlySchedule.LastDay;
            }
            else {
                day = e.Item.Index + 1;
            }

            if(e.Item.Checked) {
                AddDay(day);
            }
            else {
                RemoveDay(day);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            for(int i = 0; i < 32; i++) {
                DayList.Items[i].Checked = false;
            }
        }
    }
}
