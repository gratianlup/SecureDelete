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
using System.Runtime.Serialization;

namespace SecureDelete.Schedule {
    [Serializable]
    public class OneTimeSchedule : ScheduleBase {
        #region Fields

        [NonSerialized]
        ScheduleTimer timer;

        #endregion

        #region Constructor

        public OneTimeSchedule() {
            timer = new ScheduleTimer();
            _enabled = true;
        }

        #endregion

        #region Destructor

        ~OneTimeSchedule() {
            if(timer != null) {
                timer.StopTimer();
            }
        }

        #endregion

        #region Properties

        public override ScheduleType Type {
            get { return ScheduleType.OneTime; }
        }

        public override bool IsTimed {
            get { return true; }
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

        #endregion

        #region Public methods

        public override DateTime? GetScheduleTime() {
            // verify if the time is in the valid range
            if(DateTime.Now > _time.StartTime) {
                // schedule expired
                return null;
            }

            return _time.StartTime;
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
            OneTimeSchedule temp = new OneTimeSchedule();
            temp._enabled = _enabled;
            temp._running = _running;
            temp._time = (ScheduleTime)_time.Clone();
            return temp;
        }

        #endregion
    }
}
