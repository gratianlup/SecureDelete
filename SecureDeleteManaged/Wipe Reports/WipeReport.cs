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
using System.IO;
using DebugUtils.Debugger;

namespace SecureDelete {
    [Serializable]
    public class WipeStatistics : ICloneable {
        private DateTime _startTime;
        public DateTime StartTime {
            get { return _startTime; }
            set { _startTime = value; }
        }

        private DateTime _endTime;
        public DateTime EndTime {
            get { return _endTime; }
            set { _endTime = value; }
        }

        private TimeSpan _duration;
        public TimeSpan Duration {
            get { return _duration; }
            set { _duration = value; }
        }

        private int _failedObjects;
        public int FailedObjects {
            get { return _failedObjects; }
            set { _failedObjects = value; }
        }

        private int _errors;
        public int Errors {
            get { return _errors; }
            set { _errors = value; }
        }

        private long _averageWriteSpeed;
        public long AverageWriteSpeed {
            get { return _averageWriteSpeed; }
            set { _averageWriteSpeed = value; }
        }

        private long _totalWipedBytes;
        public long TotalWipedBytes {
            get { return _totalWipedBytes; }
            set { _totalWipedBytes = value; }
        }

        private long _bytesInClusterTips;
        public long BytesInClusterTips {
            get { return _bytesInClusterTips; }
            set { _bytesInClusterTips = value; }
        }

        #region ICloneable Members

        public object Clone() {
            WipeStatistics temp = new WipeStatistics();
            temp._averageWriteSpeed = _averageWriteSpeed;
            temp._bytesInClusterTips = _bytesInClusterTips;
            temp._duration = _duration;
            temp._endTime = _endTime;
            temp._errors = _errors;
            temp._failedObjects = _failedObjects;
            temp._startTime = _startTime;
            temp._totalWipedBytes = _totalWipedBytes;
            return temp;
        }

        #endregion
    }


    [Serializable]
    public class WipeReport {
        public WipeReport() {
            _failedObjects = new List<FailedObject>();
            _errors = new List<WipeError>();
        }

        public WipeReport(Guid sessionGuid) {
            _failedObjects = new List<FailedObject>();
            _errors = new List<WipeError>();
            _sessionGuid = sessionGuid;
        }

        private Guid _sessionGuid;
        public Guid SessionGuid {
            get { return _sessionGuid; }
            set { _sessionGuid = value; }
        }

        private WipeStatistics _statistics;
        public WipeStatistics Statistics {
            get { return _statistics; }
            set { _statistics = value; }
        }

        private List<FailedObject> _failedObjects;
        public List<FailedObject> FailedObjects {
            get { return _failedObjects; }
            set { _failedObjects = value; }
        }

        private List<WipeError> _errors;
        public List<WipeError> Errors {
            get { return _errors; }
            set { _errors = value; }
        }
    }
}
