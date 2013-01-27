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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SecureDeleteWinForms {
    public partial class PanelSelectControl : UserControl {
        public event EventHandler SelectedStateChanged;

        private bool _reversed;
        public bool Reversed {
            get { return _reversed; }
            set { _reversed = value; Selected = _selected; }
        }

        private Color _color;
        public Color Color {
            get { return _color; }
            set { _color = value; }
        }

        private Color _selectedColor;
        public Color SelectedColor {
            get { return _selectedColor; }
            set { _selectedColor = value; }
        }

        private Color _textColor;
        public Color TextColor {
            get { return _textColor; }
            set { _textColor = value; }
        }

        private Color _selectedTextColor;
        public Color SelectedTextColor {
            get { return _selectedTextColor; }
            set { _selectedTextColor = value; }
        }

        private bool _selected;
        public bool Selected {
            get { return _selected; }
            set {
                _selected = value;

                if(_selected == false) {
                    MainPicture.BackColor = _color;
                    RightPicture.BackColor = _color;
                    LeftPicture.BackColor = _color;
                    label1.ForeColor = _textColor;
                    label1.BackColor = _color;
                }
                else {
                    MainPicture.BackColor = _selectedColor;
                    RightPicture.BackColor = _selectedColor;
                    LeftPicture.BackColor = _selectedColor;
                    label1.ForeColor = _selectedTextColor;
                    label1.BackColor = _selectedColor;
                }

                if(_reversed) {
                    RightPicture.Image = SDResources.moduleSelectorRightReversed;
                    LeftPicture.Image = SDResources.moduleSelectorLeftReversed;
                    MainPicture.Image = SDResources.moduleSelectorReversed;
                }
                else {
                    RightPicture.Image = SDResources.moduleSelectorRight;
                    LeftPicture.Image = SDResources.moduleSelectorLeft;
                    MainPicture.Image = SDResources.moduleSelector;
                }

                if(SelectedStateChanged != null) {
                    SelectedStateChanged(this, new EventArgs());
                }
            }
        }

        public string SelectorText {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public PanelSelectControl() {
            InitializeComponent();
            _selected = false;
        }

        private void MainPicture_Click(object sender, EventArgs e) {
            Selected = true;
            this.OnClick(null);
        }

        private void LeftPicture_Click(object sender, EventArgs e) {
            Selected = true;
            this.OnClick(null);
        }

        private void RightPicture_Click(object sender, EventArgs e) {
            Selected = true;
            this.OnClick(null);
        }

        private void ModuleSelectControl_Load(object sender, EventArgs e) {
            Selected = false;
        }

        private void label1_Click(object sender, EventArgs e) {
            Selected = true;
            this.OnClick(null);
        }
    }
}
