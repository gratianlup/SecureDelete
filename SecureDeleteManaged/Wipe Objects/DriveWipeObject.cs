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

namespace SecureDelete.WipeObjects {
    [Serializable]
    public class DriveWipeObject : IWipeObject {
        #region IWipeObject Members

        public WipeObjectType Type {
            get { return WipeObjectType.Drive; }
        }

        public bool SingleObject {
            get { return true; }
        }

        private WipeOptions _options;
        public WipeOptions Options {
            get { return _options; }
            set { _options = value; }
        }


        public NativeMethods.WObject GetObject() {
            NativeMethods.WObject drive = new NativeMethods.WObject();
            StringBuilder builder = new StringBuilder();

            for(int i = 0; i < drives.Count; i++) {
                builder.Append(drives[i]);
            }

            drive.type = NativeMethods.TYPE_DRIVE;
            drive.path = builder.ToString();
            drive.aux = null;
            drive.wipeMethod = _wipeMethodId == WipeOptions.DefaultWipeMethod ? _options.DefaultFreeSpaceMethod : _wipeMethodId;
            drive.wipeOptions = 0;

            // set the options
            if(_wipeFreeSpace) {
                drive.wipeOptions |= NativeMethods.WIPE_FREE_SPACE;
            }

            if(_wipeClusterTips) {
                drive.wipeOptions |= NativeMethods.WIPE_CLUSTER_TIPS;
            }

            if(_wipeMFT) {
                drive.wipeOptions |= NativeMethods.WIPE_MFT;
            }

            return drive;
        }


        /// <summary>
        /// Not supported
        /// </summary>
        public NativeMethods.WObject[] GetObjects() {
            throw new Exception("GetObjects should not be called");
        }


        /// <summary>
        /// Not implemented
        /// </summary>
        public void Stop() {

        }

        #endregion

        #region Fields

        private List<char> drives;

        #endregion

        #region Constructor

        public DriveWipeObject() {
            drives = new List<char>();
        }

        #endregion

        #region Properties

        public List<char> Drives {
            get { return drives; }
        }

        private int _wipeMethodId;
        public int WipeMethodId {
            get { return _wipeMethodId; }
            set { _wipeMethodId = value; }
        }

        private bool _wipeFreeSpace;
        public bool WipeFreeSpace {
            get { return _wipeFreeSpace; }
            set { _wipeFreeSpace = value; }
        }

        private bool _wipeClusterTips;
        public bool WipeClusterTips {
            get { return _wipeClusterTips; }
            set { _wipeClusterTips = value; }
        }

        private bool _wipeMFT;
        public bool WipeMFT {
            get { return _wipeMFT; }
            set { _wipeMFT = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add a drive
        /// </summary>
        public bool AddDrive(char drive) {
            drive = char.ToUpper(drive);

            if(drive < 'A' || drive > 'Z') {
                return false;
            }

            // check if the drive is already in the category
            if(drives.Contains(drive)) {
                return true;
            }

            drives.Add(drive);
            return true;
        }


        /// <summary>
        /// Remove a drive
        /// </summary>
        public bool RemoveDrive(char drive) {
            drive = char.ToUpper(drive);
            if(drive < 'A' || drive > 'Z') {
                return false;
            }

            // check if the drive is already in the category
            if(drives.Contains(drive)) {
                // delete all occurrences
                do {
                    drives.Remove(drive);
                } while(drives.Contains(drive));

                return true;
            }

            return false;
        }


        /// <summary>
        /// Remove all drives
        /// </summary>
        public void ClearDrives() {
            drives.Clear();
        }

        #endregion
    }
}
