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
    [Serializable]
    public class HistoryItem {
        #region Constructor

        public HistoryItem() {

        }

        public HistoryItem(Guid guid, DateTime startTime) {
            _sessionGuid = guid;
            _startTime = startTime;
        }

        #endregion

        #region Properties

        private Guid _sessionGuid;
        public Guid SessionGuid {
            get { return _sessionGuid; }
            set { _sessionGuid = value; }
        }

        private DateTime _startTime;
        public System.DateTime StartTime {
            get { return _startTime; }
            set { _startTime = value; }
        }

        private DateTime? _endTime;
        public DateTime? EndTime {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public TimeSpan? DeltaTime {
            get {
                if(_endTime.HasValue == false) {
                    return null;
                }

                return _endTime - _startTime;
            }
        }

        private bool _failed;
        public bool Failed {
            get { return _failed; }
            set { _failed = value; }
        }

        private WipeStatistics _statistics;
        public SecureDelete.WipeStatistics Statistics {
            get { return _statistics; }
            set { _statistics = value; }
        }

        private ReportInfo _reportInfo;
        public ReportInfo ReportInfo {
            get { return _reportInfo; }
            set { _reportInfo = value; }
        }

        #endregion

        #region Static methods

        public static bool IsOk(HistoryItem item) {
            return item._endTime.HasValue && item._failed == false;
        }

        public static bool IsFailed(HistoryItem item) {
            return item._endTime.HasValue == false;
        }

        public static bool IsWithErrors(HistoryItem item) {
            return item._endTime.HasValue && item._failed == true;
        }

        #endregion
    }
}
