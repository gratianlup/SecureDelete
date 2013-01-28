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
using System.Globalization;
using System.Runtime.Serialization;

namespace SecureDelete.Schedule {
    [Flags]
    public enum WeekDay {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64
    }


    [Serializable]
    public class WeeklySchedule : ScheduleBase {
        #region Fields

        [NonSerialized]
        ScheduleTimer timer;

        #endregion

        #region Constructor

        public WeeklySchedule() {
            timer = new ScheduleTimer();
            _enabled = true;
        }

        #endregion

        #region Destructor

        ~WeeklySchedule() {
            if(timer != null) {
                timer.StopTimer();
            }
        }

        #endregion

        #region Properties

        public override ScheduleType Type {
            get { return ScheduleType.Weekly; }
        }

        public override bool IsTimed {
            get { return true; }
        }

        private WeekDay _days;
        public WeekDay Days {
            get { return _days; }
            set { _days = value; }
        }

        #endregion

        #region Events

        public override event TaskStartDelegate TaskStarted;

        #endregion

        #region Private methods

        private void OnTimeEllapsed(object sender, EventArgs e) {
            StopSchedule();

            if(TaskStarted != null) {
                TaskStarted(this);
            }
        }


        private WeekDay ConvertToWeekDay(DayOfWeek day) {
            switch(day) {
                case DayOfWeek.Monday: {
                    return WeekDay.Monday;
                }
                case DayOfWeek.Thursday: {
                    return WeekDay.Thursday;
                }
                case DayOfWeek.Wednesday: {
                    return WeekDay.Wednesday;
                }
                case DayOfWeek.Tuesday: {
                    return WeekDay.Tuesday;
                }
                case DayOfWeek.Friday: {
                    return WeekDay.Friday;
                }
                case DayOfWeek.Saturday: {
                    return WeekDay.Saturday;
                }
                case DayOfWeek.Sunday: {
                    return WeekDay.Sunday;
                }
            }

            return WeekDay.Monday;
        }


        private bool ContainsDay(WeekDay day) {
            return ((int)_days & (int)day) == (int)day;
        }


        private DateTime? GetNextDay(DateTime now) {
            if(ContainsDay(ConvertToWeekDay(now.DayOfWeek))) {
                return now;
            }

            for(int i = 1; i <= 6; i++) {
                DateTime next = now.AddDays(i);

                if(ContainsDay(ConvertToWeekDay(next.DayOfWeek))) {
                    return next;
                }
            }

            return null;
        }


        private int GetWeekInYear(DateTime date) {
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;
            return calendar.GetWeekOfYear(date, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule,
                                          CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
        }

        #endregion

        #region Public methods

        public override DateTime? GetScheduleTime() {
            DateTime now = DateTime.Now;

            // compute the next schedule time
            DateTime? nextDay = GetNextDay(now);

            // no day selected
            if(nextDay.HasValue == false) {
                return null;
            }

            DateTime next = new DateTime(nextDay.Value.Year, nextDay.Value.Month, nextDay.Value.Day, _time.StartTime.Hour,
                                         _time.StartTime.Minute, _time.StartTime.Second, _time.StartTime.Millisecond);

            if(_time.RecurenceValue > 0) {
                TimeSpan diff = next - _time.StartTime;
                int recurence = _time.RecurenceValue;
                int weeksAfter = ((diff.Days + (diff.Milliseconds > 0 ? 1 : 0)) / 7) % recurence;

                if(weeksAfter != 0) {
                    if(weeksAfter > 1) {
                        next = next.AddDays(7);
                    }

                    int ct = 0;
                    int week = GetWeekInYear(next);

                    while(week == GetWeekInYear(next)) {
                        next = next.AddDays(1);
                    }

                    while(ContainsDay(ConvertToWeekDay(next.DayOfWeek)) == false && ct <= 7) {
                        next = next.AddDays(1);
                        ct++;
                    }
                }
            }

            // verify if the time is in the valid range
            if(_time.HasEndTime && next > _time.EndTime) {
                // schedule expired
                return null;
            }

            return next;
        }


        public override bool StartSchedule() {
            if(_enabled == false) {
                return false;
            }

            if(_running) {
                return false;
            }

            // get the due time
            DateTime? dueTime = GetScheduleTime();

            if(dueTime.HasValue) {
                if(timer != null) {
                    timer.StopTimer();
                }

                TimeSpan interval = dueTime.Value - DateTime.Now;
                timer.OnTimeEllapsed += OnTimeEllapsed;
                timer.StartTimer(interval);
                _running = true;
                return true;
            }

            return false;
        }


        public override bool StopSchedule() {
            if(_running && timer != null) {
                timer.StopTimer();
                _running = false;
                return true;
            }

            return false;
        }

        #endregion

        #region Serialization helpers

        [OnSerializing()]
        internal void OnSerializingMethod(StreamingContext context) {
            TaskStarted = null;
        }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context) {
            timer = new ScheduleTimer();
        }

        #endregion

        #region ICloneable Members

        public override object Clone() {
            WeeklySchedule temp = new WeeklySchedule();
            temp._enabled = _enabled;
            temp._running = _running;
            temp._time = (ScheduleTime)_time.Clone();
            temp._days = _days;
            return temp;
        }

        #endregion
    }
}
