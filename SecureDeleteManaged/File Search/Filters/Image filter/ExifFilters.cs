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
    #region Title filter

    [Serializable]
    public sealed class ImageTitleFilter : ImageTextFilter {
        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.Title; }
        }

        public ImageTitleFilter() {
            _enabled = true;
        }

        protected override bool AllowProperty(bool condition, ImageData data) {
            string text = XmpReader.GetTitle(data);

            if(text == null) {
                // not found, get it from EXIF
                text = ExifReader.GetTitle(data);
            }

            // validate
            if(text == null || _value == null) {
                return !condition;
            }

            if(_regularExpression) {
                bool? value = IsRegexMatch(text);

                if(value.HasValue && value.Value == true) {
                    return condition;
                }
            }
            else {
                // normal match
                if(text.IndexOf(_value, _matchCase ? StringComparison.CurrentCulture : 
                                                     StringComparison.CurrentCultureIgnoreCase) >= 0) {
                    return condition;
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageTitleFilter temp = new ImageTitleFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._matchCase = _matchCase;
            temp._regularExpression = _regularExpression;
            temp._value = _value;
            return temp;
        }
    }

    #endregion

    #region Camera maker filter

    [Serializable]
    public sealed class ImageCameraMakerFilter : ImageTextFilter {
        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.CameraMaker; }
        }

        public ImageCameraMakerFilter() {
            _enabled = true;
        }

        protected override bool AllowProperty(bool condition, ImageData data) {
            string text = ExifReader.GetMaker(data);

            if(text == null || _value == null) {
                return !condition;
            }

            if(_regularExpression) {
                bool? value = IsRegexMatch(text);

                if(value.HasValue && value.Value == true) {
                    return condition;
                }
            }
            else {
                // normal match
                if(text.IndexOf(_value, _matchCase ? StringComparison.CurrentCulture : 
                                                     StringComparison.CurrentCultureIgnoreCase) >= 0) {
                    return condition;
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageCameraMakerFilter temp = new ImageCameraMakerFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._matchCase = _matchCase;
            temp._regularExpression = _regularExpression;
            temp._value = _value;
            return temp;
        }
    }

    #endregion

    #region Camera model filter

    [Serializable]
    public sealed class ImageCameraModelFilter : ImageTextFilter {
        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.CameraModel; }
        }

        public ImageCameraModelFilter() {
            _enabled = true;
        }

        protected override bool AllowProperty(bool condition, ImageData data) {
            string text = ExifReader.GetModel(data);

            if(text == null || _value == null) {
                return !condition;
            }

            if(_regularExpression) {
                bool? value = IsRegexMatch(text);

                if(value.HasValue && value.Value == true) {
                    return condition;
                }
            }
            else {
                // normal match
                if(text.IndexOf(_value, _matchCase ? StringComparison.CurrentCulture : 
                                                     StringComparison.CurrentCultureIgnoreCase) >= 0) {
                    return condition;
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageCameraModelFilter temp = new ImageCameraModelFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._matchCase = _matchCase;
            temp._regularExpression = _regularExpression;
            temp._value = _value;
            return temp;
        }
    }

    #endregion

    #region Software filter

    [Serializable]
    public sealed class ImageSoftwareFilter : ImageTextFilter {
        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.Software; }
        }

        public ImageSoftwareFilter() {
            _enabled = true;
        }

        protected override bool AllowProperty(bool condition, ImageData data) {
            string text = ExifReader.GetSoftware(data);

            if(text == null || _value == null) {
                return !condition;
            }

            if(_regularExpression) {
                bool? value = IsRegexMatch(text);

                if(value.HasValue && value.Value == true) {
                    return condition;
                }
            }
            else {
                // normal match
                if(text.IndexOf(_value, _matchCase ? StringComparison.CurrentCulture : 
                                                     StringComparison.CurrentCultureIgnoreCase) >= 0) {
                    return condition;
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageSoftwareFilter temp = new ImageSoftwareFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._matchCase = _matchCase;
            temp._regularExpression = _regularExpression;
            temp._value = _value;
            return temp;
        }
    }

    #endregion

    #region Author filter

    [Serializable]
    public sealed class ImageAuthorFilter : ImageTextFilter {
        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.Author; }
        }

        public ImageAuthorFilter() {
            _enabled = true;
        }

        protected override bool AllowProperty(bool condition, ImageData data) {
            // get the author from XMP
            List<string> authors = XmpReader.GetAuthors(data);

            if(authors != null && authors.Count > 0) {
                for(int i = 0; i < authors.Count; i++) {
                    if(_regularExpression) {
                        bool? value = IsRegexMatch(authors[i]);

                        if(value.HasValue && value.Value == true) {
                            return condition;
                        }
                    }
                    else {
                        // normal match
                        if(authors[i].IndexOf(_value, _matchCase ? StringComparison.CurrentCulture : 
                                                                   StringComparison.CurrentCultureIgnoreCase) >= 0) {
                            return condition;
                        }
                    }
                }
            }
            else {
                // get the author from EXIF
                string text = ExifReader.GetAuthor(data);

                if(text == null || _value == null) {
                    return !condition;
                }

                if(_regularExpression) {
                    bool? value = IsRegexMatch(text);

                    if(value.HasValue && value.Value == true) {
                        return condition;
                    }
                }
                else {
                    // normal match
                    if(text.IndexOf(_value, _matchCase ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase) >= 0) {
                        return condition;
                    }
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageAuthorFilter temp = new ImageAuthorFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._matchCase = _matchCase;
            temp._regularExpression = _regularExpression;
            temp._value = _value;
            return temp;
        }
    }

    #endregion

    #region Copyright filter

    [Serializable]
    public sealed class ImageCopyrightFilter : ImageTextFilter {
        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.Copyright; }
        }

        public ImageCopyrightFilter() {
            _enabled = true;
        }

        protected override bool AllowProperty(bool condition, ImageData data) {
            string text = ExifReader.GetCopyright(data);

            if(text == null || _value == null) {
                return !condition;
            }

            if(_regularExpression) {
                bool? value = IsRegexMatch(text);

                if(value.HasValue && value.Value == true) {
                    return condition;
                }
            }
            else {
                // normal match
                if(text.IndexOf(_value, _matchCase ? StringComparison.CurrentCulture : 
                                                     StringComparison.CurrentCultureIgnoreCase) >= 0) {
                    return condition;
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageCopyrightFilter temp = new ImageCopyrightFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._matchCase = _matchCase;
            temp._regularExpression = _regularExpression;
            temp._value = _value;
            return temp;
        }
    }

    #endregion

    #region Orientation filter

    [Serializable]
    public sealed class ImageOrientationFilter : ImageFilter {
        #region Properties

        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.Orientation; }
        }

        private ExifReader.ImageOrientation _value;
        public ExifReader.ImageOrientation Value {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region Constructor

        public ImageOrientationFilter() {
            _enabled = true;
        }

        #endregion

        #region Methods

        protected override bool AllowProperty(bool condition, ImageData data) {
            ExifReader.ImageOrientation? orientation = ExifReader.GetOrientation(data);

            if(orientation.HasValue && orientation.Value == _value) {
                return condition;
            }

            return !condition;
        }

        public override object Clone() {
            ImageOrientationFilter temp = new ImageOrientationFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._value = _value;
            return temp;
        }

        #endregion
    }

    #endregion

    #region Exposure time filter

    [Serializable]
    public sealed class ImageExposureTimeFilter : ImageFilter {
        #region Constants

        private const double Epsilon = 0.00001;

        #endregion

        #region Constructor

        public ImageExposureTimeFilter() {
            _enabled = true;
            _value = new ExifReader.Fraction();
        }

        #endregion

        #region Properties

        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.ExposureTime; }
        }

        private SizeImplication _sizeImplication;
        public SizeImplication SizeImplication {
            get { return _sizeImplication; }
            set { _sizeImplication = value; }
        }

        private ExifReader.Fraction _value;
        public ExifReader.Fraction Value {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region Methods

        protected override bool AllowProperty(bool condition, ImageData data) {
            ExifReader.Fraction time = ExifReader.GetExposureTime(data);

            if(time != null) {
                // invalid values, would throw a DivideByZeroException
                if(time.Denominator == 0 || _value.Denominator == 0) {
                    return !condition;
                }

                double a = (double)time.Numerator / (double)time.Denominator;
                double b = (double)_value.Numerator / (double)_value.Denominator;

                if(_sizeImplication == SizeImplication.Equals) {
                    if(Math.Abs(b - a) < Epsilon) {
                        return condition;
                    }
                }
                else if(_sizeImplication == SizeImplication.LessThan) {
                    if(b <= a) {
                        return condition;
                    }
                }
                else {
                    if(b >= a) {
                        return condition;
                    }
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageExposureTimeFilter temp = new ImageExposureTimeFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._value = _value;
            temp._sizeImplication = _sizeImplication;
            return temp;
        }

        #endregion
    }

    #endregion

    #region FNumber filter

    [Serializable]
    public sealed class ImageFNumberFilter : ImageNumberFilter {
        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.FNumber; }
        }

        public ImageFNumberFilter() {
            _enabled = true;
        }

        protected override bool AllowProperty(bool condition, ImageData data) {
            double? number = ExifReader.GetFNumber(data);

            if(number.HasValue) {
                if(_sizeImplication == SizeImplication.Equals) {
                    if(Math.Abs(number.Value - _value) < Epsilon) {
                        return condition;
                    }
                }
                else if(_sizeImplication == SizeImplication.LessThan) {
                    if(number.Value <= _value) {
                        return condition;
                    }
                }
                else {
                    if(number.Value >= _value) {
                        return condition;
                    }
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageFNumberFilter temp = new ImageFNumberFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._value = _value;
            temp._sizeImplication = _sizeImplication;
            return temp;
        }
    }

    #endregion

    #region Exposure program filter

    [Serializable]
    public sealed class ImageExposureProgramFilter : ImageFilter {
        #region Constructor

        public ImageExposureProgramFilter() {
            _enabled = true;
        }

        #endregion

        #region Properties

        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.ExposureProgram; }
        }

        private ExifReader.ExposureProgram _value;
        public ExifReader.ExposureProgram Value {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region Methods

        protected override bool AllowProperty(bool condition, ImageData data) {
            ExifReader.ExposureProgram? program = ExifReader.GetExposureProgram(data);

            if(program.HasValue) {
                if(program.Value == _value) {
                    return condition;
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageExposureProgramFilter temp = new ImageExposureProgramFilter();

            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._value = _value;

            return temp;
        }

        #endregion
    }

    #endregion

    #region Focal length filter

    [Serializable]
    public sealed class ImageFocalLengthFilter : ImageNumberFilter {
        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.FocalLength; }
        }

        public ImageFocalLengthFilter() {
            _enabled = true;
        }

        protected override bool AllowProperty(bool condition, ImageData data) {
            int? length = ExifReader.GetFocalLength(data);

            if(length.HasValue) {
                if(_sizeImplication == SizeImplication.Equals) {
                    if(length.Value == _value) {
                        return condition;
                    }
                }
                else if(_sizeImplication == SizeImplication.LessThan) {
                    if(length.Value <= _value) {
                        return condition;
                    }
                }
                else {
                    if(length.Value >= _value) {
                        return condition;
                    }
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageFocalLengthFilter temp = new ImageFocalLengthFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._value = _value;
            temp._sizeImplication = _sizeImplication;
            return temp;
        }
    }

    #endregion

    #region ISO filter

    [Serializable]
    public sealed class ImageIsoFilter : ImageNumberFilter {
        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.ISO; }
        }

        public ImageIsoFilter() {
            _enabled = true;
        }

        protected override bool AllowProperty(bool condition, ImageData data) {
            int? iso = ExifReader.GetIso(data);

            if(iso.HasValue) {
                if(_sizeImplication == SizeImplication.Equals) {
                    if(iso.Value == _value) {
                        return condition;
                    }
                }
                else if(_sizeImplication == SizeImplication.LessThan) {
                    if(iso.Value <= _value) {
                        return condition;
                    }
                }
                else {
                    if(iso.Value >= _value) {
                        return condition;
                    }
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageIsoFilter temp = new ImageIsoFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._value = _value;
            temp._sizeImplication = _sizeImplication;
            return temp;
        }
    }

    #endregion

    #region Date taken filter

    [Serializable]
    public sealed class ImageDateTakenFilter : ImageFilter {
        #region Constructor

        public ImageDateTakenFilter() {
            _enabled = true;
            _value = DateTime.Now;
        }

        #endregion

        #region Properties

        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.DateTaken; }
        }

        private DateImplication _dateImplication;
        public DateImplication DateImplication {
            get { return _dateImplication; }
            set { _dateImplication = value; }
        }

        private DateTime _value;
        public DateTime Value {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region Methods

        protected override bool AllowProperty(bool condition, ImageData data) {
            DateTime? date = ExifReader.GetDateTaken(data);

            if(date.HasValue) {
                if(_dateImplication == DateImplication.From) {
                    if(date.Value.Day == _value.Day && date.Value.Month == _value.Month &&
                        date.Value.Year == _value.Year) {
                        return condition;
                    }
                }
                else if(_dateImplication == DateImplication.OlderOrFrom) {
                    if(date.Value <= _value) {
                        return condition;
                    }
                }
                else {
                    if(date.Value >= _value) {
                        return condition;
                    }
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageDateTakenFilter temp = new ImageDateTakenFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._value = _value;
            temp._dateImplication = _dateImplication;
            return temp;
        }

        #endregion
    }

    #endregion

    #region Exposure bias filter

    [Serializable]
    public sealed class ImageExposureBiasFilter : ImageNumberFilter {
        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.ExposureBias; }
        }

        public ImageExposureBiasFilter() {
            _enabled = true;
        }

        protected override bool AllowProperty(bool condition, ImageData data) {
            double? bias = ExifReader.GetExposureBias(data);

            if(bias.HasValue) {
                if(_sizeImplication == SizeImplication.Equals) {
                    if(Math.Abs(bias.Value - _value) < Epsilon) {
                        return condition;
                    }
                }
                else if(_sizeImplication == SizeImplication.LessThan) {
                    if(bias.Value <= _value) {
                        return condition;
                    }
                }
                else {
                    if(bias.Value >= _value) {
                        return condition;
                    }
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageExposureBiasFilter temp = new ImageExposureBiasFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._value = _value;
            temp._sizeImplication = _sizeImplication;
            return temp;
        }
    }

    #endregion

    #region Metering mode filter

    [Serializable]
    public sealed class ImageMeteringModeFilter : ImageFilter {
        #region Constructor

        public ImageMeteringModeFilter() {
            _enabled = true;
        }

        #endregion

        #region Properties

        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.MeteringMode; }
        }

        private ExifReader.MeteringMode _value;
        public ExifReader.MeteringMode Value {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region Methods

        protected override bool AllowProperty(bool condition, ImageData data) {
            ExifReader.MeteringMode? mode = ExifReader.GetMeteringMode(data);

            if(mode.HasValue) {
                if(mode.Value == _value) {
                    return condition;
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageMeteringModeFilter temp = new ImageMeteringModeFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._value = _value;
            return temp;
        }

        #endregion
    }

    #endregion

    #region Flash fired filter

    [Serializable]
    public sealed class ImageFlashFiredFilter : ImageFilter {
        #region Constructor

        public ImageFlashFiredFilter() {
            _enabled = true;
        }

        #endregion

        #region Properties

        public override ImageFilter.ImageProperty PropertyType {
            get { return ImageProperty.FlashFired; }
        }

        private bool _value;
        public bool Value {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region Methods

        protected override bool AllowProperty(bool condition, ImageData data) {
            bool? fired = ExifReader.GetFlashFired(data);

            if(fired.HasValue) {
                if(fired.Value == _value) {
                    return condition;
                }
            }

            return !condition;
        }

        public override object Clone() {
            ImageFlashFiredFilter temp = new ImageFlashFiredFilter();
            temp._name = _name;
            temp._condition = _condition;
            temp._enabled = _enabled;
            temp._value = _value;
            return temp;
        }

        #endregion
    }

    #endregion
}
