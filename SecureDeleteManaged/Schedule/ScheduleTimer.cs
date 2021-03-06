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
using System.Threading;

namespace SecureDelete.Schedule {
    class ScheduleTimer {
        #region Fields

        private Timer timer;

        #endregion

        #region Properties

        private bool _running;
        public bool Running {
            get { return _running; }
            set { _running = value; }
        }

        #endregion

        #region Events

        public EventHandler OnTimeEllapsed;

        #endregion

        #region Private methods

        private void TimeEllapsed(object state) {
            _running = false;

            // announce the parent
            if(OnTimeEllapsed != null) {
                OnTimeEllapsed(this, null);
            }
        }

        #endregion

        #region Public methods

        public bool StartTimer(TimeSpan dueTime) {
            if(_running) {
                return false;
            }

            if(timer != null) {
                timer.Dispose();
            }

            if(dueTime.TotalMilliseconds < 0) {
                return false;
            }

            timer = new Timer(TimeEllapsed, null, (long)dueTime.TotalMilliseconds, Timeout.Infinite);
            _running = true;
            return true;
        }


        public void StopTimer() {
            if(timer != null) {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                timer.Dispose();
                timer = null;
                _running = false;
            }
        }

        #endregion
    }
}
