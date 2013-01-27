// // Copyright (c) 2007 Gratian Lup. All rights reserved.
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
using System.IO;
using System.Text;
using System.Windows.Forms;
using SecureDelete.FileSearch;
using DebugUtils.Debugger;

namespace SecureDeleteWinForms {
    public partial class AttributeFilterView : UserControl, IFilterView {
        public AttributeFilterView() {
            InitializeComponent();
        }

        #region IFilterView Members

        public FilterViewType Type {
            get { return FilterViewType.Attribute; }
        }

        public int FilterViewHeight {
            get { return 34; }
        }

        private bool _filterViewEnabled;
        public bool FilterViewEnabled {
            get { return _filterViewEnabled; }
            set { _filterViewEnabled = value; SetEnabledState(_filterViewEnabled, true); }
        }

        private AttributeFilter _filter;
        public SecureDelete.FileSearch.FilterBase Filter {
            get {
                return _filter;
            }
            set {
                Debug.Assert(value is AttributeFilter, "Filters is not a SizeFilter");
                _filter = value as AttributeFilter;

                InterpretFilter();
            }
        }

        private void InterpretFilter() {
            checkBox1.Checked = _filter.Enabled;
            NameTextbox.Text = _filter.Name == null ? string.Empty : _filter.Name;

            if(_filter.Condition == FilterCondition.IS) {
                comboBox1.SelectedIndex = 0;
            }
            else {
                comboBox1.SelectedIndex = 1;
            }

            if(_filter.AttributeValue == FileAttributes.Archive) {
                comboBox2.SelectedIndex = 0;
            }
            else if(_filter.AttributeValue == FileAttributes.Compressed) {
                comboBox2.SelectedIndex = 1;
            }
            else if(_filter.AttributeValue == FileAttributes.Encrypted) {
                comboBox2.SelectedIndex = 2;
            }
            else if(_filter.AttributeValue == FileAttributes.Hidden) {
                comboBox2.SelectedIndex = 3;
            }
            else if(_filter.AttributeValue == FileAttributes.ReadOnly) {
                comboBox2.SelectedIndex = 4;
            }
            else {
                comboBox2.SelectedIndex = 5;
            }

            StatusIndicator.Visible = checkBox1.Checked;
        }

        public event EventHandler OnRemove;
        public event EventHandler OnStateChanged;
        public event EventHandler OnLayoutChanged;

        #endregion

        private void button1_Click(object sender, EventArgs e) {
            if(OnRemove != null) {
                OnRemove(this, null);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");

            if(comboBox1.SelectedIndex == 0) {
                _filter.Condition = FilterCondition.IS;
            }
            else {
                _filter.Condition = FilterCondition.IS_NOT;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");
            _filter.Enabled = checkBox1.Checked;
        }

        private void SetEnabledState(bool enabled, bool enabledCheckbox) {
            if(enabledCheckbox) {
                checkBox1.Enabled = enabled;
                button1.Enabled = enabled;
            }

            label1.Enabled = enabled && _filter.Enabled;
            comboBox1.Enabled = enabled && _filter.Enabled;
            label2.Enabled = enabled && _filter.Enabled;
            label3.Enabled = enabled && _filter.Enabled;
            NameTextbox.Enabled = enabled && _filter.Enabled;
            comboBox2.Enabled = enabled && _filter.Enabled;
            StatusIndicator.Visible = checkBox1.Checked;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");

            if(comboBox2.SelectedIndex == 0) {
                _filter.AttributeValue = FileAttributes.Archive;
            }
            else if(comboBox2.SelectedIndex == 1) {
                _filter.AttributeValue = FileAttributes.Compressed;
            }
            else if(comboBox2.SelectedIndex == 2) {
                _filter.AttributeValue = FileAttributes.Encrypted;
            }
            else if(comboBox2.SelectedIndex == 3) {
                _filter.AttributeValue = FileAttributes.Hidden;
            }
            else if(comboBox2.SelectedIndex == 4) {
                _filter.AttributeValue = FileAttributes.ReadOnly;
            }
            else {
                _filter.AttributeValue = FileAttributes.System;
            }
        }

        private void button1_Click_1(object sender, EventArgs e) {
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

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e) {
            Debug.AssertNotNull(_filter, "Filters not set");

            _filter.Enabled = checkBox1.Checked;
            SetEnabledState(checkBox1.Checked, false);
            StatusIndicator.Visible = checkBox1.Checked;

            if(OnStateChanged != null) {
                OnStateChanged(this, null);
            }
        }
    }
}

