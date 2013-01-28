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

namespace SecureDelete.Schedule {
    public enum ScheduleType {
        OneTime,
        Daily,
        Weekly,
        Monthly
    }


    [Serializable]
    public class ScheduleTime : ICloneable {
        public ScheduleTime() {
            StartTime = DateTime.Now;
            HasEndTime = false;
            EndTime = DateTime.Now;
            RecurenceValue = 1;
        }

        public DateTime StartTime;
        public bool HasEndTime;
        public DateTime EndTime;
        public int RecurenceValue;

        #region ICloneable Members

        public object Clone() {
            ScheduleTime temp = new ScheduleTime();
            temp.StartTime = StartTime;
            temp.HasEndTime = HasEndTime;
            temp.EndTime = EndTime;
            temp.RecurenceValue = RecurenceValue;
            return temp;
        }

        #endregion
    }


    public delegate void TaskStartDelegate(ISchedule schedule);


    /// <summary>
    /// Interface that needs to be implemented by all schedule type classes
    /// </summary>
    public interface ISchedule : ICloneable {
        ScheduleType Type { get; }
        bool Enabled { get; set; }
        bool IsTimed { get; }
        bool Running { get; }
        ScheduleTime Time { get; set; }
        DateTime? GetScheduleTime();
        bool StartSchedule();
        bool StopSchedule();
        event TaskStartDelegate TaskStarted;
        new object Clone();
    }

    [Serializable]
    public abstract class ScheduleBase : ISchedule, ICloneable {
        #region Constructor

        public ScheduleBase() {
            _time = new ScheduleTime();
        }

        #endregion

        public abstract ScheduleType Type { get; }

        protected bool _enabled;
        public bool Enabled {
            get { return _enabled; }
            set { _enabled = value; }
        }

        [NonSerialized]
        protected bool _running;
        public bool Running {
            get { return _running; }
            set { _running = value; }
        }

        public abstract bool IsTimed { get; }

        protected ScheduleTime _time;
        public ScheduleTime Time {
            get { return _time; }
            set { _time = value; }
        }

        public abstract DateTime? GetScheduleTime();
        public abstract bool StartSchedule();
        public abstract bool StopSchedule();

        public abstract event TaskStartDelegate TaskStarted;

        public abstract object Clone();
    }
}
