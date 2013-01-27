// Copyright (c) Gratian Lup. All rights reserved.
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SecureDelete.FileSearch;
using DebugUtils.Debugger;

namespace SecureDeleteWinForms {
    public partial class ImageFilterView : UserControl, IFilterView {
        public ImageFilterView() {
            InitializeComponent();
            Height = DefaultViewHeight;
        }

        #region Constants

        private const int DefaultViewHeight = 34;
        private const int TagsViewHeigth = 116;

        #endregion

        #region Fields

        private bool loading;
        private ImageTagsFilter tagsFilter;
        private int viewHeight = DefaultViewHeight;

        #endregion

        #region IFilterView Members

        public event EventHandler OnRemove;
        public event EventHandler OnStateChanged;
        public event EventHandler OnLayoutChanged;

        public FilterViewType Type {
            get { return FilterViewType.Size; }
        }

        public int FilterViewHeight {
            get { return viewHeight; }
        }

        private bool _filterViewEnabled;
        public bool FilterViewEnabled {
            get { return _filterViewEnabled; }
            set { _filterViewEnabled = value; SetEnabledState(_filterViewEnabled, true); }
        }

        private ImageFilter _filter;
        public FilterBase Filter {
            get {
                return _filter;
            }
            set {
                Debug.Assert(value is ImageFilter, "Filters is not a ImageFilter");
                _filter = value as ImageFilter;
                InterpretFilter();
            }
        }

        private void InterpretFilter() {
            loading = true;
            checkBox1.Checked = _filter.Enabled;
            NameTextbox.Text = _filter.Name == null ? string.Empty : _filter.Name;

            if(_filter.Condition == FilterCondition.IS) {
                comboBox2.SelectedIndex = 0;
            }
            else {
                comboBox2.SelectedIndex = 1;
            }

            StatusIndicator.Visible = checkBox1.Checked;
            HandleNewFilter();
            loading = false;
        }

        private void HideFilterPanels() {
            TextPanel.Visible = false;
            DatePanel.Visible = false;
            NumberPanel.Visible = false;
            ProgramPanel.Visible = false;
            ExposureTimePanel.Visible = false;
            FlashPanel.Visible = false;
            MeteringPanel.Visible = false;
            OrientationPanel.Visible = false;
            RatingPanel.Visible = false;
            TagsPanel.Visible = false;
            TagsPanel2.Visible = false;
        }

        #endregion

        private void SetEnabledState(bool enabled, bool enabledCheckbox) {
            if(enabledCheckbox) {
                checkBox1.Enabled = enabled;
                button1.Enabled = enabled;
            }

            FilterType.Enabled = enabled && _filter.Enabled;
            comboBox2.Enabled = enabled && _filter.Enabled;
            TextFilterValue.Enabled = enabled && _filter.Enabled;
            label3.Enabled = enabled && _filter.Enabled;
            NameTextbox.Enabled = enabled && _filter.Enabled;
            StatusIndicator.Visible = checkBox1.Checked;
            SizeImplicationCombobox.Enabled = enabled && _filter.Enabled;
            SizeValue.Enabled = enabled && _filter.Enabled;
            DateImplicationCombobox.Enabled = enabled && _filter.Enabled;
            DateValue.Enabled = enabled && _filter.Enabled;
            ProgramCombobox.Enabled = enabled && _filter.Enabled;
            ExposureTimeLabel.Enabled = enabled && _filter.Enabled;
            ExposureTimeSizeCombobox.Enabled = enabled && _filter.Enabled;
            NumeratorTextbox.Enabled = enabled && _filter.Enabled;
            DenominatorTextbox.Enabled = enabled && _filter.Enabled;
            FlashLabel.Enabled = enabled && _filter.Enabled;
            OrientationCombobox.Enabled = enabled && _filter.Enabled;
            MeteringComboBox.Enabled = enabled && _filter.Enabled;
            RatingCombobox.Enabled = enabled && _filter.Enabled;
            StarLabel.Enabled = enabled && _filter.Enabled;
            Star1.Enabled = enabled && _filter.Enabled;
            Star2.Enabled = enabled && _filter.Enabled;
            Star3.Enabled = enabled && _filter.Enabled;
            Star4.Enabled = enabled && _filter.Enabled;
            Star5.Enabled = enabled && _filter.Enabled;
            TagList.Enabled = enabled && _filter.Enabled;
            TagLabel1.Enabled = enabled && _filter.Enabled;
            TagLabel2.Enabled = enabled && _filter.Enabled;
            TagImplicationCombobox.Enabled = enabled && _filter.Enabled;
            AddTagButton.Enabled = enabled && _filter.Enabled;
            RemoveTagButton.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e) {
            if(OnRemove != null) {
                OnRemove(this, null);
            }
        }

        private void NameTextbox_TextChanged(object sender, EventArgs e) {
            _filter.Name = NameTextbox.Text;

            if(OnStateChanged != null) {
                OnStateChanged(this, null);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            _filter.Enabled = checkBox1.Checked;
            SetEnabledState(checkBox1.Checked, false);
            StatusIndicator.Visible = checkBox1.Checked;

            if(OnStateChanged != null) {
                OnStateChanged(this, null);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");
            ImageTextFilter filter = (ImageTextFilter)_filter;
            filter.Value = TextFilterValue.Text;
        }

        private void MatchCaseCheckbox_CheckedChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");
            ImageTextFilter filter = (ImageTextFilter)_filter;
            filter.MatchCase = MatchCaseCheckbox.Checked;
        }

        private void RegexCheckbox_CheckedChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");
            ImageTextFilter filter = (ImageTextFilter)_filter;
            filter.RegularExpression = RegexCheckbox.Checked;
        }

        private void SendNewLayoutInfo(int height) {
            viewHeight = height;

            if(OnLayoutChanged != null) {
                OnLayoutChanged(this, null);
            }
        }

        private ImageFilter GetImageFilter(string name) {
            if(name == null || name.Length == 0) {
                return null;
            }

            switch(name.ToLower()) {
                case "author": {
                    return new ImageAuthorFilter();
                }
                case "artist": {
                    return new ImageAuthorFilter();
                }
                case "camera maker": {
                        return new ImageCameraMakerFilter();
                }
                case "camera model": {
                    return new ImageCameraModelFilter();
                }
                case "copyright": {
                    return new ImageCopyrightFilter();
                }
                case "date taken": {
                    return new ImageDateTakenFilter();
                }
                case "exposure bias": {
                    return new ImageExposureBiasFilter();
                }
                case "exposure program": {
                    return new ImageExposureProgramFilter();
                }
                case "exposure time": {
                        return new ImageExposureTimeFilter();
                }
                case "flash fired": {
                    return new ImageFlashFiredFilter();
                }
                case "f number": {
                    return new ImageFNumberFilter();
                }
                case "focal length": {
                    return new ImageFocalLengthFilter();
                }
                case "iso": {
                    return new ImageIsoFilter();
                }
                case "metering mode": {
                    return new ImageMeteringModeFilter();
                }
                case "orientation": {
                    return new ImageOrientationFilter();
                }
                case "software": {
                    return new ImageSoftwareFilter();
                }
                case "title": {
                    return new ImageTitleFilter();
                }
                case "rating": {
                    return new ImageRatingFilter();
                }
                case "tag list": {
                    return new ImageTagsFilter();
                }
            }

            // not found
            return null;
        }

        private void FilterType_SelectedIndexChanged(object sender, EventArgs e) {
            SetNewFilter(FilterType.Text);
        }

        private void SetNewFilter(string filterName) {
            if(loading) {
                return;
            }

            _filter = GetImageFilter(filterName);
            HandleNewFilter();

            if(OnStateChanged != null) {
                OnStateChanged(this, null);
            }
        }

        private void HandleNewFilter() {
            tagsFilter = null;

            if(_filter is ImageTextFilter) {
                HandleNewTextFilter();
            }
            else if(_filter is ImageDateTakenFilter) {
                HandleNewDateFilter();
            }
            else if(_filter is ImageNumberFilter) {
                HandleNewNumberFilter();
            }
            else if(_filter is ImageExposureProgramFilter) {
                HandleNewProgramFilter();
            }
            else if(_filter is ImageExposureTimeFilter) {
                HandleNewExposureTimeFilter();
            }
            else if(_filter is ImageFlashFiredFilter) {
                HandleNewFlashFilter();
            }
            else if(_filter is ImageMeteringModeFilter) {
                HandleNewMeteringFilter();
            }
            else if(_filter is ImageOrientationFilter) {
                HandleNewOrientationFilter();
            }
            else if(_filter is ImageTagsFilter) {
                HandleNewTagsFilter();
            }

            // relayout
            if(_filter is ImageTagsFilter) {
                SendNewLayoutInfo(TagsViewHeigth);
            }
            else {
                SendNewLayoutInfo(DefaultViewHeight);
            }
        }

        private void HandleNewTagsFilter() {
            if((_filter is ImageTagsFilter) == false) {
                throw new Exception("Invalid filter type");
            }

            tagsFilter = (ImageTagsFilter)_filter;
            HideFilterPanels();
            TagsPanel.Visible = true;
            TagsPanel2.Visible = true;

            switch(tagsFilter.TagImplication) {
                case TagImplication.All: { TagImplicationCombobox.SelectedIndex = 0; break; }
                case TagImplication.Some: { TagImplicationCombobox.SelectedIndex = 1; break; }
            }

            LoadTagList();
        }

        private void HandleNewRatingFilter() {
            if((_filter is ImageRatingFilter) == false) {
                throw new Exception("Invalid filter type");
            }

            HideFilterPanels();
            RatingPanel.Visible = true;
            ImageRatingFilter filter = (ImageRatingFilter)_filter;

            switch(filter.SizeImplication) {
                case SizeImplication.LessThan: { RatingCombobox.SelectedIndex = 0; break; }
                case SizeImplication.Equals: { RatingCombobox.SelectedIndex = 1; break; }
                case SizeImplication.GreaterThan: { RatingCombobox.SelectedIndex = 2; break; }
            }

            if(loading == false) {
                ShowStars(filter.Value);
            }
        }

        private void HandleNewOrientationFilter() {
            if((_filter is ImageOrientationFilter) == false) {
                throw new Exception("Invalid filter type");
            }

            HideFilterPanels();
            MeteringPanel.Visible = true;
            ImageOrientationFilter filter = (ImageOrientationFilter)_filter;

            switch(filter.Value) {
                case ExifReader.ImageOrientation.Correct: { OrientationCombobox.SelectedIndex = 0; break; }
                case ExifReader.ImageOrientation.RotateLeft: { OrientationCombobox.SelectedIndex = 1; break; }
                case ExifReader.ImageOrientation.RotateRight: { OrientationCombobox.SelectedIndex = 2; break; }
            }
        }

        private void HandleNewMeteringFilter() {
            if((_filter is ImageMeteringModeFilter) == false) {
                throw new Exception("Invalid filter type");
            }

            HideFilterPanels();
            MeteringPanel.Visible = true;
            ImageMeteringModeFilter filter = (ImageMeteringModeFilter)_filter;

            switch(filter.Value) {
                case ExifReader.MeteringMode.AverageMetering: { MeteringComboBox.SelectedIndex = 0; break; }
                case ExifReader.MeteringMode.CenterWeightedAverageMetering: { MeteringComboBox.SelectedIndex = 1; break; }
                case ExifReader.MeteringMode.MatrixMetering: { MeteringComboBox.SelectedIndex = 2; break; }
                case ExifReader.MeteringMode.MultiSpotMetering: { MeteringComboBox.SelectedIndex = 3; break; }
                case ExifReader.MeteringMode.PartialMetering: { MeteringComboBox.SelectedIndex = 4; break; }
                case ExifReader.MeteringMode.SpotMetering: { MeteringComboBox.SelectedIndex = 5; break; }
                case ExifReader.MeteringMode.Unknown: { MeteringComboBox.SelectedIndex = 6; break; }
            }
        }

        private void HandleNewFlashFilter() {
            if((_filter is ImageFlashFiredFilter) == false) {
                throw new Exception("Invalid filter type");
            }

            HideFilterPanels();
            FlashPanel.Visible = true;
        }

        private void HandleNewExposureTimeFilter() {
            if((_filter is ImageExposureTimeFilter) == false) {
                throw new Exception("Invalid filter type");
            }

            HideFilterPanels();
            ExposureTimePanel.Visible = true;
            ImageExposureTimeFilter filter = (ImageExposureTimeFilter)_filter;

            switch(filter.SizeImplication) {
                case SizeImplication.LessThan: { ExposureTimeSizeCombobox.SelectedIndex = 0; break; }
                case SizeImplication.Equals: { ExposureTimeSizeCombobox.SelectedIndex = 1; break; }
                case SizeImplication.GreaterThan: { ExposureTimeSizeCombobox.SelectedIndex = 2; break; }
            }

            if(loading == false) {
                NumeratorTextbox.Text = filter.Value.Numerator.ToString();
                DenominatorTextbox.Text = filter.Value.Denominator.ToString();
                ExposureTimeLabel.Text = GetFormattedTime(filter);
            }
        }

        private string GetFormattedTime(ImageExposureTimeFilter filter) {
            string time = "(";

            if(filter.Value.Denominator == 0) {
                time += "0";
            }
            else {
                time += string.Format("{0:f5}", ((double)filter.Value.Numerator / (double)filter.Value.Denominator));

                // trim unnecessary zeroes
                while(time.Length > 0 && time[time.Length - 1] == '0') {
                    time = time.Substring(0, time.Length - 1);
                }
            }

            time += ") seconds";
            return time;
        }

        private void HandleNewProgramFilter() {
            if((_filter is ImageExposureProgramFilter) == false) {
                throw new Exception("Invalid filter type");
            }

            HideFilterPanels();
            ProgramPanel.Visible = true;
            ImageExposureProgramFilter filter = (ImageExposureProgramFilter)_filter;

            switch(filter.Value) {
                case ExifReader.ExposureProgram.ActionProgram: { ProgramCombobox.SelectedIndex = 0; break; }
                case ExifReader.ExposureProgram.AperaturePriority: { ProgramCombobox.SelectedIndex = 1; break; }
                case ExifReader.ExposureProgram.CreativeProgram: { ProgramCombobox.SelectedIndex = 2; break; }
                case ExifReader.ExposureProgram.LandscapeMode: { ProgramCombobox.SelectedIndex = 3; break; }
                case ExifReader.ExposureProgram.Manual: { ProgramCombobox.SelectedIndex = 4; break; }
                case ExifReader.ExposureProgram.NotDefined: { ProgramCombobox.SelectedIndex = 5; break; }
                case ExifReader.ExposureProgram.PortaitMode: { ProgramCombobox.SelectedIndex = 6; break; }
                case ExifReader.ExposureProgram.Program: { ProgramCombobox.SelectedIndex = 7; break; }
                case ExifReader.ExposureProgram.ShutterPriority: { ProgramCombobox.SelectedIndex = 8; break; }
            }
        }

        private void HandleNewNumberFilter() {
            if((_filter is ImageNumberFilter) == false) {
                throw new Exception("Invalid filter type");
            }

            // special case
            if(_filter is ImageRatingFilter) {
                HandleNewRatingFilter();
                return;
            }

            HideFilterPanels();
            NumberPanel.Visible = true;

            ImageNumberFilter filter = (ImageNumberFilter)_filter;
            switch(filter.SizeImplication) {
                case SizeImplication.LessThan: { SizeImplicationCombobox.SelectedIndex = 0; break; }
                case SizeImplication.Equals: { SizeImplicationCombobox.SelectedIndex = 1; break; }
                case SizeImplication.GreaterThan: { SizeImplicationCombobox.SelectedIndex = 2; break; }
            }

            SizeValue.Text = filter.Value.ToString();

            if(loading) {
                switch(filter.PropertyType) {
                    case ImageFilter.ImageProperty.ExposureBias: { FilterType.SelectedIndex = 5; break; }
                    case ImageFilter.ImageProperty.FNumber: { FilterType.SelectedIndex = 9; break; }
                    case ImageFilter.ImageProperty.FocalLength: { FilterType.SelectedIndex = 10; break; }
                    case ImageFilter.ImageProperty.ISO: { FilterType.SelectedIndex = 11; break; }
                }
            }

            switch(filter.PropertyType) {
                case ImageFilter.ImageProperty.ExposureBias: { NumberFilterLabel.Text = "steps"; break; }
                case ImageFilter.ImageProperty.FNumber: { NumberFilterLabel.Text = ""; break; }
                case ImageFilter.ImageProperty.FocalLength: { NumberFilterLabel.Text = "mm"; break; }
                case ImageFilter.ImageProperty.ISO: { NumberFilterLabel.Text = ""; break; }
            }
        }

        private void HandleNewDateFilter() {
            if((_filter is ImageDateTakenFilter) == false) {
                throw new Exception("Invalid filter type");
            }

            HideFilterPanels();
            DatePanel.Visible = true;
            ImageDateTakenFilter filter = (ImageDateTakenFilter)_filter;

            switch(filter.DateImplication) {
                case DateImplication.NewerOrFrom: { DateImplicationCombobox.SelectedIndex = 0; break; }
                case DateImplication.From: { DateImplicationCombobox.SelectedIndex = 1; break; }
                case DateImplication.OlderOrFrom: { DateImplicationCombobox.SelectedIndex = 2; break; }
            }

            DateValue.Value = filter.Value;

            if(loading) {
                FilterType.SelectedIndex = 4;
            }
        }

        private void HandleNewTextFilter() {
            if((_filter is ImageTextFilter) == false) {
                throw new Exception("Invalid filter type");
            }

            HideFilterPanels();
            TextPanel.Visible = true;

            ImageTextFilter filter = (ImageTextFilter)_filter;
            TextFilterValue.Text = filter.Value;
            MatchCaseCheckbox.Checked = filter.MatchCase;
            RegexCheckbox.Checked = filter.RegularExpression;

            if(loading) {
                // update contextbox
                if(filter.PropertyType == ImageFilter.ImageProperty.Author) {
                    FilterType.SelectedIndex = 0;
                }
                else if(filter.PropertyType == ImageFilter.ImageProperty.CameraMaker) {
                    FilterType.SelectedIndex = 1;
                }
                else if(filter.PropertyType == ImageFilter.ImageProperty.CameraModel) {
                    FilterType.SelectedIndex = 2;
                }
                else if(filter.PropertyType == ImageFilter.ImageProperty.Copyright) {
                    FilterType.SelectedIndex = 3;
                }
                else if(filter.PropertyType == ImageFilter.ImageProperty.Software) {
                    FilterType.SelectedIndex = 14;
                }
                else if(filter.PropertyType == ImageFilter.ImageProperty.Title) {
                    FilterType.SelectedIndex = 15;
                }
            }
        }

        private void DateValue_ValueChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");
            ImageDateTakenFilter filter = (ImageDateTakenFilter)_filter;
            filter.Value = DateValue.Value;
        }

        private void DateImplicationCombobox_SelectedIndexChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");

            ImageDateTakenFilter filter = (ImageDateTakenFilter)_filter;
            switch(DateImplicationCombobox.SelectedIndex) {
                case 0: { filter.DateImplication = DateImplication.NewerOrFrom; break; }
                case 1: { filter.DateImplication = DateImplication.From; break; }
                case 2: { filter.DateImplication = DateImplication.OlderOrFrom; break; }
            }
        }

        private void SizeImplicationCombobox_SelectedIndexChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");
            ImageNumberFilter filter = (ImageNumberFilter)_filter;

            switch(SizeImplicationCombobox.SelectedIndex) {
                case 0: { filter.SizeImplication = SizeImplication.LessThan; break; }
                case 1: { filter.SizeImplication = SizeImplication.Equals; break; }
                case 2: { filter.SizeImplication = SizeImplication.GreaterThan; break; }
            }
        }

        private void SizeValue_Validating(object sender, CancelEventArgs e) {
            double num;

            if(double.TryParse(SizeValue.Text, out num) == false) {
                ErrorTooltip.Show("Invalid number", SizeValue, 3000);
                SizeValue.Focus();
                SizeValue.SelectAll();
            }
        }

        private void SizeValue_TextChanged(object sender, EventArgs e) {
            double num;

            if(double.TryParse(SizeValue.Text, out num)) {
                Debug.AssertNotNull(_filter, "Filters not set");
                ImageNumberFilter filter = (ImageNumberFilter)_filter;
                filter.Value = num;
            }
        }

        private void ProgramCombobox_SelectedIndexChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");
            ImageExposureProgramFilter filter = (ImageExposureProgramFilter)_filter;

            switch(ProgramCombobox.SelectedIndex) {
                case 0: { filter.Value = ExifReader.ExposureProgram.ActionProgram; break; }
                case 1: { filter.Value = ExifReader.ExposureProgram.AperaturePriority; break; }
                case 2: { filter.Value = ExifReader.ExposureProgram.CreativeProgram; break; }
                case 3: { filter.Value = ExifReader.ExposureProgram.LandscapeMode; break; }
                case 4: { filter.Value = ExifReader.ExposureProgram.Manual; break; }
                case 5: { filter.Value = ExifReader.ExposureProgram.NotDefined; break; }
                case 6: { filter.Value = ExifReader.ExposureProgram.PortaitMode; break; }
                case 7: { filter.Value = ExifReader.ExposureProgram.Program; break; }
                case 8: { filter.Value = ExifReader.ExposureProgram.ShutterPriority; break; }
            }
        }

        private void NumeratorTextbox_TextChanged(object sender, EventArgs e) {
            int num;

            if(int.TryParse(NumeratorTextbox.Text, out num)) {
                Debug.AssertNotNull(_filter, "Filters not set");
                ImageExposureTimeFilter filter = (ImageExposureTimeFilter)_filter;
                filter.Value.Numerator = num;
                ExposureTimeLabel.Text = GetFormattedTime(filter);
            }
        }

        private void NumeratorTextbox_Validating(object sender, CancelEventArgs e) {
            int num;

            if(int.TryParse(NumeratorTextbox.Text, out num) == false) {
                ErrorTooltip.Show("Invalid number", NumeratorTextbox, 3000);
                NumeratorTextbox.Focus();
                NumeratorTextbox.SelectAll();
            }
        }

        private void DenominatorTextbox_Validating(object sender, CancelEventArgs e) {
            int num;

            if(int.TryParse(DenominatorTextbox.Text, out num) == false) {
                ErrorTooltip.Show("Invalid number", DenominatorTextbox, 3000);
                DenominatorTextbox.Focus();
                DenominatorTextbox.SelectAll();
            }
        }

        private void DenominatorTextbox_TextChanged(object sender, EventArgs e) {
            int num;

            if(int.TryParse(DenominatorTextbox.Text, out num)) {
                Debug.AssertNotNull(_filter, "Filters not set");
                ImageExposureTimeFilter filter = (ImageExposureTimeFilter)_filter;
                filter.Value.Denominator = num;
                ExposureTimeLabel.Text = GetFormattedTime(filter);
            }
        }

        private void MeteringComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");
            ImageMeteringModeFilter filter = (ImageMeteringModeFilter)_filter;

            switch(MeteringComboBox.SelectedIndex) {
                case 0: { filter.Value = ExifReader.MeteringMode.AverageMetering; break; }
                case 1: { filter.Value = ExifReader.MeteringMode.CenterWeightedAverageMetering; break; }
                case 2: { filter.Value = ExifReader.MeteringMode.MatrixMetering; break; }
                case 3: { filter.Value = ExifReader.MeteringMode.MultiSpotMetering; break; }
                case 4: { filter.Value = ExifReader.MeteringMode.PartialMetering; break; }
                case 5: { filter.Value = ExifReader.MeteringMode.SpotMetering; break; }
                case 6: { filter.Value = ExifReader.MeteringMode.Unknown; break; }
            }
        }

        private void OrientationCombobox_SelectedIndexChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");

            ImageOrientationFilter filter = (ImageOrientationFilter)_filter;
            switch(OrientationCombobox.SelectedIndex) {
                case 0: { filter.Value = ExifReader.ImageOrientation.Correct; break; }
                case 1: { filter.Value = ExifReader.ImageOrientation.RotateLeft; break; }
                case 2: { filter.Value = ExifReader.ImageOrientation.RotateRight; break; }
            }
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e) {
            ShowStars(5);
        }

        private void ShowStars(double rating) {
            Star1.Image = rating > 0 ? SDResources.Star : SDResources.StarDisabled;
            Star2.Image = rating > 1 ? SDResources.Star : SDResources.StarDisabled;
            Star3.Image = rating > 2 ? SDResources.Star : SDResources.StarDisabled;
            Star4.Image = rating > 3 ? SDResources.Star : SDResources.StarDisabled;
            Star5.Image = rating > 4 ? SDResources.Star : SDResources.StarDisabled;

            int value = (int)rating;

            if(value == 0) {
                StarLabel.Text = "Not rated";
            }
            else if(value == 1) {
                StarLabel.Text = value.ToString() + " star";
            }
            else {
                StarLabel.Text = value.ToString() + " stars";
            }
        }

        private void Star4_MouseEnter(object sender, EventArgs e) {
            ShowStars(4);
        }

        private void Star3_MouseEnter(object sender, EventArgs e) {
            ShowStars(3);
        }

        private void Start2_MouseEnter(object sender, EventArgs e) {
            ShowStars(2);
        }

        private void Star1_MouseEnter(object sender, EventArgs e) {
            ShowStars(1);
        }

        private void Star5_Click(object sender, EventArgs e) {

        }

        private void Star5_MouseUp(object sender, MouseEventArgs e) {
            ImageRatingFilter filter = (ImageRatingFilter)_filter;
            filter.Value = 5;
            ShowStars((int)filter.Value);
        }

        private void Star5_MouseLeave(object sender, EventArgs e) {
            ImageRatingFilter filter = (ImageRatingFilter)_filter;
            ShowStars(filter.Value);
        }

        private void Star4_MouseLeave(object sender, EventArgs e) {
            ImageRatingFilter filter = (ImageRatingFilter)_filter;
            ShowStars(filter.Value);
        }

        private void Star3_MouseLeave(object sender, EventArgs e) {
            ImageRatingFilter filter = (ImageRatingFilter)_filter;
            ShowStars(filter.Value);
        }

        private void Star2_MouseLeave(object sender, EventArgs e) {
            ImageRatingFilter filter = (ImageRatingFilter)_filter;
            ShowStars(filter.Value);
        }

        private void Star1_MouseLeave(object sender, EventArgs e) {
            ImageRatingFilter filter = (ImageRatingFilter)_filter;
            ShowStars(filter.Value);
        }

        private void Star4_MouseUp(object sender, MouseEventArgs e) {
            ImageRatingFilter filter = (ImageRatingFilter)_filter;
            filter.Value = 4;
            ShowStars(filter.Value);
        }

        private void Star3_MouseUp(object sender, MouseEventArgs e) {
            ImageRatingFilter filter = (ImageRatingFilter)_filter;
            filter.Value = 3;
            ShowStars(filter.Value);
        }

        private void Star2_MouseUp(object sender, MouseEventArgs e) {
            ImageRatingFilter filter = (ImageRatingFilter)_filter;
            filter.Value = 2;
            ShowStars(filter.Value);
        }

        private void Star1_MouseUp(object sender, MouseEventArgs e) {
            ImageRatingFilter filter = (ImageRatingFilter)_filter;
            filter.Value = 1;
            ShowStars(filter.Value);
        }

        private void panel1_MouseEnter(object sender, EventArgs e) {
            ShowStars(0);
        }

        private void panel1_MouseLeave(object sender, EventArgs e) {
            ImageRatingFilter filter = (ImageRatingFilter)_filter;
            ShowStars(filter.Value);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e) {
            ImageRatingFilter filter = (ImageRatingFilter)_filter;
            filter.Value = 0;
            ShowStars(filter.Value);
        }

        private void RatingCombobox_SelectedIndexChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");
            ImageRatingFilter filter = (ImageRatingFilter)_filter;

            switch(RatingCombobox.SelectedIndex) {
                case 0: { filter.SizeImplication = SizeImplication.LessThan; break; }
                case 1: { filter.SizeImplication = SizeImplication.Equals; break; }
                case 2: { filter.SizeImplication = SizeImplication.GreaterThan; break; }
            }
        }

        private void YesNoCombobox_Leave(object sender, EventArgs e) {
            YesNoCombobox.Visible = false;
            YesNoCombobox.Tag = null;
            TagList.SelectedIndices.Clear();
            TagList.FullRowSelect = false;
        }

        private void TagList_MouseUp(object sender, MouseEventArgs e) {
            // Get the member on the row that is clicked.
            TagList.FullRowSelect = true;
            ListViewItem item = TagList.GetItemAt(e.X, e.Y);

            // Make sure that an member is clicked.
            if(item != null) {
                // Get the bounds of the member that is clicked.
                Rectangle clickedItem = item.Bounds;
                int delta = TagList.Columns[0].Width;
                int column = 0;
                int selectedIndex = 0;
                ImageTag tag = (ImageTag)item.Tag;

                if(item.GetSubItemAt(e.X, e.Y) == item.SubItems[1]) {
                    column = 1;
                    if(tag.MatchCase) {
                        selectedIndex = 0;
                    }
                    else {
                        selectedIndex = 1;
                    }
                }
                else if(item.GetSubItemAt(e.X, e.Y) == item.SubItems[2]) {
                    column = 2;
                    delta += TagList.Columns[1].Width;

                    if(tag.RegularExpression) {
                        selectedIndex = 0;
                    }
                    else {
                        selectedIndex = 1;
                    }
                }

                if(column == 0) {
                    YesNoCombobox.Visible = false;
                    TagList.FullRowSelect = false;
                    return;
                }

                // Assign calculated bounds to the ComboBox.
                YesNoCombobox.Left = item.Bounds.Left + delta + TagList.Left + TagsPanel.Left + 1;
                YesNoCombobox.Top = item.Bounds.Top + TagList.Top + TagsPanel.Top;
                YesNoCombobox.Width = TagList.Columns[column].Width;
                YesNoCombobox.Height = item.Bounds.Height;

                // Set default text for ComboBox to match the member that is clicked.
                YesNoCombobox.SelectedIndex = selectedIndex;
                YesNoCombobox.Tag = new YesNoComboboxHelper(item, column);

                // Display the ComboBox, and make sure that it is on top with focus.
                YesNoCombobox.Visible = true;
                YesNoCombobox.BringToFront();
                YesNoCombobox.Select();
                YesNoCombobox.Focus();
                TagList.FullRowSelect = false;
            }
            else {
                TagList.FullRowSelect = false;
            }
        }

        #region Image tags filter

        private void LoadTagList() {
            TagList.Items.Clear();

            if(tagsFilter != null && tagsFilter.Tags != null) {
                List<ImageTag> tags = tagsFilter.Tags;

                for(int i = 0; i < tags.Count; i++) {
                    ListViewItem item = new ListViewItem();

                    item.Text = tags[i].Value;
                    item.Checked = tags[i].Enabled;
                    item.SubItems.Add(tags[i].MatchCase ? "Yes" : "No");
                    item.SubItems.Add(tags[i].RegularExpression ? "Yes" : "No");
                    item.Tag = tags[i];

                    TagList.Items.Add(item);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            if(tagsFilter == null) {
                tagsFilter = new ImageTagsFilter();
            }

            tagsFilter.Tags.Add(new ImageTag("TagName", false, false));
            LoadTagList();
        }

        private void button3_Click(object sender, EventArgs e) {
            if(tagsFilter == null) {
                return;
            }

            while(TagList.SelectedIndices.Count > 0) {
                ImageTag tag = (ImageTag)TagList.Items[TagList.SelectedIndices[0]].Tag;
                if(tagsFilter.Tags.Contains(tag)) {
                    tagsFilter.Tags.Remove(tag);
                }

                TagList.Items.RemoveAt(TagList.SelectedIndices[0]);
            }
        }

        #endregion

        private void TagList_SelectedIndexChanged(object sender, EventArgs e) {
            RemoveTagButton.Enabled = _filter.Enabled && TagList.SelectedIndices.Count > 0;
        }

        private void TagList_AfterLabelEdit(object sender, LabelEditEventArgs e) {
            ImageTag tag = (ImageTag)TagList.Items[e.Item].Tag;
            tag.Value = e.Label;
        }

        private void YesNoCombobox_SelectedIndexChanged(object sender, EventArgs e) {
            if(YesNoCombobox.Tag == null) {
                return;
            }

            YesNoComboboxHelper helper = (YesNoComboboxHelper)YesNoCombobox.Tag;
            ImageTag tag = (ImageTag)helper.Item.Tag;

            if(helper.Column == 1) {
                tag.MatchCase = YesNoCombobox.SelectedIndex == 0;
            }
            else {
                tag.RegularExpression = YesNoCombobox.SelectedIndex == 0;
            }

            helper.Item.SubItems[helper.Column].Text = YesNoCombobox.SelectedIndex == 0 ? "Yes" : "No";
        }

        private class YesNoComboboxHelper {
            public ListViewItem Item;
            public int Column;

            public YesNoComboboxHelper(ListViewItem item, int column) {
                Item = item;
                Column = column;
            }
        }

        private void TagList_ItemChecked(object sender, ItemCheckedEventArgs e) {
            ImageTag tag = (ImageTag)e.Item.Tag;
            tag.Enabled = e.Item.Checked;
        }

        private void TagImplicationCombobox_SelectedIndexChanged(object sender, EventArgs e) {
            tagsFilter.TagImplication = TagImplicationCombobox.SelectedIndex == 0 ? TagImplication.All : TagImplication.Some;
        }
    }
}
