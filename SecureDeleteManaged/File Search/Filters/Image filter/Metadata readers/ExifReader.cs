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
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

namespace SecureDelete.FileSearch {
    public class ExifReader {
        #region Constants

        private const int TitleId = 270;
        private const int CameraMakerId = 271;
        private const int CameraModelId = 272;
        private const int OrientationId = 274;
        private const int SoftwareId = 305;
        private const int AuthorId = 315;
        private const int CopyrightId = 33432;
        private const int ExposureTimeId = 33434;
        private const int FNumberId = 33437;
        private const int ExposureProgramId = 34850;
        private const int IsoId = 34855;
        private const int TakenDateId = 36867;
        private const int ExposureBiasId = 37380;
        private const int MeteringModeId = 37383;
        private const int FlashFiredId = 37385;
        private const int FocalLengthId = 37386;

        #endregion

        #region Nested types

        public enum ImageOrientation {
            Correct,
            RotateRight,
            RotateLeft
        }

        public class Fraction {
            #region Fields

            public int Numerator;
            public int Denominator;

            #endregion

            #region Constructor

            public Fraction(int numerator, int denominator) {
                Numerator = numerator;
                Denominator = denominator;
            }

            public Fraction() {

            }

            #endregion
        }

        public enum ExposureProgram {
            NotDefined,
            Manual,
            Program,
            AperaturePriority,
            ShutterPriority,
            CreativeProgram,
            ActionProgram,
            PortaitMode,
            LandscapeMode
        }

        public enum MeteringMode {
            Unknown,
            AverageMetering,
            CenterWeightedAverageMetering,
            SpotMetering,
            MultiSpotMetering,
            MatrixMetering,
            PartialMetering
        }

        #endregion

        #region Public methods


        /// <summary>
        /// Camera maker
        /// </summary>
        public static string GetTitle(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(TitleId);
                return data.Ascii.GetString(item.Value);
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// Camera maker
        /// </summary>
        public static string GetMaker(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(CameraMakerId);
                return data.Ascii.GetString(item.Value);
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// Camera model
        /// </summary>
        public static string GetModel(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(CameraModelId);
                return data.Ascii.GetString(item.Value);
            }
            catch(Exception e) {
                return null;
            }
        }

        /// <summary>
        /// Orientation
        /// </summary>
        public static ImageOrientation? GetOrientation(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(OrientationId);
                int value = BitConverter.ToInt16(item.Value, 0);

                switch(value) {
                case 1: {
                        return ImageOrientation.Correct;
                    }
                    case 6: {
                        return ImageOrientation.RotateRight;
                    }
                    case 8: {
                        return ImageOrientation.RotateLeft;
                    }
                    default: {
                        return ImageOrientation.Correct;
                    }
                }
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// Software
        /// </summary>
        public static string GetSoftware(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(SoftwareId);
                return data.Ascii.GetString(item.Value);
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// Author
        /// </summary>
        public static string GetAuthor(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(AuthorId);
                return data.Ascii.GetString(item.Value);
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// Copyright
        /// </summary>
        public static string GetCopyright(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(CopyrightId);
                return data.Ascii.GetString(item.Value);
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// Exposure time
        /// </summary>
        public static Fraction GetExposureTime(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(ExposureTimeId);
                Fraction value = new Fraction(BitConverter.ToInt16(item.Value, 0), BitConverter.ToInt16(item.Value, 4));

                // simplify fraction
                if(value.Denominator % 10 == 0 && value.Numerator % 10 == 0) {
                    value.Denominator /= 10;
                    value.Numerator /= 10;
                }

                return value;
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// FNumber
        /// </summary>
        public static double? GetFNumber(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(FNumberId);
                int value = BitConverter.ToInt16(item.Value, 0);

                if(value < 100) {
                    return Math.Round((double)value / 10, 1);
                }
                else {
                    return value;
                }
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// Exposure program
        /// </summary>
        public static ExposureProgram? GetExposureProgram(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(ExposureProgramId);
                int value = BitConverter.ToInt16(item.Value, 0);

                switch(value) {
                    case 0: {
                        return ExposureProgram.NotDefined;
                    }
                    case 1: {
                        return ExposureProgram.Manual;
                    }
                    case 2: {
                        return ExposureProgram.Program;
                    }
                    case 3: {
                        return ExposureProgram.AperaturePriority;
                    }
                    case 4: {
                        return ExposureProgram.ShutterPriority;
                    }
                    case 5: {
                        return ExposureProgram.CreativeProgram;
                    }
                    case 6: {
                        return ExposureProgram.ActionProgram;
                    }
                    case 7: {
                        return ExposureProgram.PortaitMode;
                    }
                    case 8: {
                        return ExposureProgram.LandscapeMode;
                    }
                    default: {
                        return ExposureProgram.NotDefined;
                    }
                }
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// ISO
        /// </summary>
        public static int? GetIso(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(IsoId);
                return BitConverter.ToInt16(item.Value, 0);
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// Taken date
        /// </summary>
        public static DateTime? GetDateTaken(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(TakenDateId);
                return DateTime.Parse(data.Ascii.GetString(item.Value));
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// Exposure bias
        /// </summary>
        public static double? GetExposureBias(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(ExposureBiasId);
                return (double)BitConverter.ToInt16(item.Value, 0) / 2.0;
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// Metering mode
        /// </summary>
        public static MeteringMode? GetMeteringMode(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(ExposureBiasId);
                int value = BitConverter.ToInt16(item.Value, 0);

                switch(value) {
                    case 0: {
                        return MeteringMode.Unknown;
                    }
                    case 1: {
                        return MeteringMode.AverageMetering;
                    }
                    case 2: {
                        return MeteringMode.CenterWeightedAverageMetering;
                    }
                    case 3: {
                        return MeteringMode.SpotMetering;
                    }
                    case 4: {
                        return MeteringMode.MultiSpotMetering;
                    }
                    case 5: {
                        return MeteringMode.MatrixMetering;
                    }
                    case 6: {
                        return MeteringMode.PartialMetering;
                    }
                    default: {
                        return MeteringMode.AverageMetering;
                    }
                }
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// Flash fired
        /// </summary>
        public static bool? GetFlashFired(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(FlashFiredId);
                return BitConverter.ToInt16(item.Value, 0) == 1;
            }
            catch(Exception e) {
                return null;
            }
        }


        /// <summary>
        /// Focal length
        /// </summary>
        public static int? GetFocalLength(ImageData data) {
            try {
                PropertyItem item = data.Image.GetPropertyItem(FocalLengthId);
                return BitConverter.ToInt16(item.Value, 0);
            }
            catch(Exception e) {
                return null;
            }
        }

        #endregion
    }
}
