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
using System.Text.RegularExpressions;
using System.Xml;

namespace SecureDelete.FileSearch {
    public class ImageData : IHelperObject {
        #region Fields

        public FileStream Stream;       // used by EXIF and XMP filters
        public Image Image;             // used by EXIF filters
        public ASCIIEncoding Ascii;     // used by EXIF filters
        public XmlDocument XmpDocument; // used by XMP filters

        #endregion

        #region Public methods

        public bool LoadImage(string file) {
            try {
                Stream = File.OpenRead(file);
                Image = Image.FromStream(Stream, false, false);
                Ascii = new ASCIIEncoding();
            }
            catch(Exception e) {
                Debug.ReportError("Failed to open image {0}. Exception: {1}", file, e.Message);
                return false;
            }

            return true;
        }

        #endregion

        #region IHelperObject Members

        public void Dispose() {
            if(Stream != null) {
                Stream.Close();
            }

            if(Image != null) {
                Image.Dispose();
            }

            Image = null;
            Stream = null;
            XmpDocument = null;
        }

        #endregion
    }

    #region Abstract objects

    [Serializable]
    public abstract class ImageFilter : FilterBase {
        #region Constants

        private readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".tiff" };

        #endregion

        #region Nested types

        public enum ImageProperty {
            Title,
            CameraMaker,
            CameraModel,
            Orientation,
            Software,
            Author,
            Copyright,
            ExposureTime,
            FNumber,
            ExposureProgram,
            FocalLength,
            ISO,
            DateTaken,
            ExposureBias,
            MeteringMode,
            FlashFired, /* EXIF metadata */
            Rating,
            Tags /* XMP metadata */
        }

        #endregion

        #region Properties

        /// <summary>
        /// Abstract property that needs to be implemented in the derived objects
        /// </summary>
        public abstract ImageProperty PropertyType { get; }

        #endregion

        #region Private methods

        private bool IsImage(string file) {
            // check if it's an image
            FileInfo fileInfo = new FileInfo(file);
            string extension = fileInfo.Extension.ToLower();

            for(int i = 0; i < ImageExtensions.Length; i++) {
                if(extension == ImageExtensions[i]) {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Abstract method that needs to be implemented in the derived objects
        /// </summary>
        protected abstract bool AllowProperty(bool condition, ImageData data);

        public override bool Allow(string file) {
            Debug.AssertNotNull(file, "File is null");

            bool condition = _condition == FilterCondition.IS ? true : false;
            ImageData data = null;

            if(_helperObject != null) {
                if(_helperObject is ImageData) {
                    _helperObject = (ImageData)_helperObject;
                }
                else {
                    _helperObject.Dispose();
                }
            }
            else {
                // create a new helper object
                _helperObject = new ImageData();
                data = (ImageData)_helperObject;

                // load the picture
                if(IsImage(file) == false || data.LoadImage(file) == false) {
                    // failed
                    return !condition;
                }
            }

            return AllowProperty(condition, data);
        }

        #endregion
    }


    [Serializable]
    public abstract class ImageNumberFilter : ImageFilter {
        #region Constants

        protected const double Epsilon = 0.00001;

        #endregion

        #region Properties

        protected SizeImplication _sizeImplication;
        public SizeImplication SizeImplication {
            get { return _sizeImplication; }
            set { _sizeImplication = value; }
        }

        protected double _value;
        public double Value {
            get { return _value; }
            set { _value = value; }
        }

        #endregion
    }

    [Serializable]
    public abstract class ImageTextFilter : ImageFilter {
        #region Fields

        [NonSerialized]
        private Regex regex;

        #endregion

        #region Properties

        protected bool _matchCase;
        public bool MatchCase {
            get { return _matchCase; }
            set { _matchCase = value; }
        }

        protected bool _regularExpression;
        public bool RegularExpression {
            get { return _regularExpression; }
            set { _regularExpression = value; }
        }

        protected string _value;
        public string Value {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region Methods

        protected bool? IsRegexMatch(string text) {
            // validate data
            if(_value == null || text == null) {
                return null;
            }

            try {
                if(regex == null) {
                    // compile the expression so it executes faster
                    regex = new Regex(_value, RegexOptions.Compiled);
                }

                return regex.IsMatch(text);
            }
            catch(Exception e) {
                Debug.ReportError("Error in regex match. Exception: {0}", e.Message);
                return null;
            }
        }

        #endregion
    }

    #endregion

    #region Helper objects

    public class ImageFilterProvider {
        public static ImageFilter GetImageFilter(ImageFilter.ImageProperty type) {
            switch(type) {
                case ImageFilter.ImageProperty.Author: {
                    return new ImageAuthorFilter();
                }
                case ImageFilter.ImageProperty.CameraMaker: {
                    return new ImageCameraMakerFilter();
                }
                case ImageFilter.ImageProperty.CameraModel: {
                    return new ImageCameraModelFilter();
                }
                case ImageFilter.ImageProperty.Copyright: {
                    return new ImageCopyrightFilter();
                }
                case ImageFilter.ImageProperty.DateTaken: {
                    return new ImageDateTakenFilter();
                }
                case ImageFilter.ImageProperty.ExposureBias: {
                    return new ImageExposureBiasFilter();
                }
                case ImageFilter.ImageProperty.ExposureProgram: {
                    return new ImageExposureProgramFilter();
                }
                case ImageFilter.ImageProperty.ExposureTime: {
                    return new ImageExposureTimeFilter();
                }
                case ImageFilter.ImageProperty.FlashFired: {
                    return new ImageFlashFiredFilter();
                }
                case ImageFilter.ImageProperty.FNumber: {
                    return new ImageFNumberFilter();
                }
                case ImageFilter.ImageProperty.FocalLength: {
                    return new ImageFNumberFilter();
                }
                case ImageFilter.ImageProperty.ISO: {
                    return new ImageIsoFilter();
                }
                case ImageFilter.ImageProperty.MeteringMode: {
                    return new ImageMeteringModeFilter();
                }
                case ImageFilter.ImageProperty.Orientation: {
                    return new ImageOrientationFilter();
                }
                case ImageFilter.ImageProperty.Software: {
                    return new ImageSoftwareFilter();
                }
                case ImageFilter.ImageProperty.Title: {
                    return new ImageTitleFilter();
                }
            }

            return null;
        }
    }

    #endregion
}
