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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SecureDeleteWinForms.WipeTools {
    public partial class CompletitionDialog : Form {
        public CompletitionDialog() {
            InitializeComponent();
        }

        private List<FilterName> _suggestions;
        public List<FilterName> Suggestions {
            get { return _suggestions; }
            set {
                _suggestions = value;
                HandleNewSuggestions();
            }
        }

        private FilterBox _parentBox;
        public FilterBox ParentBox {
            get { return _parentBox; }
            set { _parentBox = value; }
        }

        private Control _parentControl;
        public Control ParentControl {
            get { return _parentControl; }
            set { _parentControl = value; }
        }

        private void HandleNewSuggestions() {
            SuggestionList.Items.Clear();

            if(_suggestions != null) {
                for(int i = 0; i < _suggestions.Count; i++) {
                    ListViewItem item = new ListViewItem();

                    item.Text = _suggestions[i].Name;
                    if(_suggestions[i].Type == SecureDelete.FileSearch.ExpressionType.Filter) {
                        item.ForeColor = _suggestions[i].Enabled ? Color.FromName("WindowText") : Color.Gray;
                        item.ImageIndex = 0;
                    }
                    else {
                        item.ForeColor = Color.DarkViolet;
                        item.ImageIndex = 1;
                    }

                    SuggestionList.Items.Add(item);
                }
            }

            // select first member
            if(SuggestionList.Items.Count > 0) {
                SuggestionList.Items[0].Selected = true;
            }
        }

        private void CompletitionDialog_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter) {
                // insert word
                InsertSelectedWord();
                e.SuppressKeyPress = true;
            }
            else if(e.KeyCode == Keys.Escape) {
                this.Hide();
            }
            else if(e.KeyCode != Keys.Up && e.KeyCode != Keys.Down) {
                if(_parentControl != null) {
                    _parentControl.Focus();
                }
            }
        }

        public void SelectUp() {
            this.Activate();
            SuggestionList.Focus();

            if(SuggestionList.SelectedIndices.Count > 0) {
                if(SuggestionList.SelectedIndices[0] != 0) {
                    SuggestionList.SelectedIndices.Add(SuggestionList.SelectedIndices[0] - 1);
                    SuggestionList.SelectedIndices.Remove(0);
                }
            }
            else if(SuggestionList.Items.Count > 0) {
                SuggestionList.SelectedIndices.Add(0);
            }
        }

        public void SelectDown() {
            this.Activate();
            SuggestionList.Focus();

            if(SuggestionList.SelectedIndices.Count > 0) {
                if(SuggestionList.SelectedIndices[0] != SuggestionList.Items.Count - 1) {
                    SuggestionList.SelectedIndices.Add(SuggestionList.SelectedIndices[0] + 1);
                    SuggestionList.SelectedIndices.Remove(0);
                }
            }
            else if(SuggestionList.Items.Count > 0) {
                SuggestionList.SelectedIndices.Add(0);
            }
        }

        public void InsertSelectedWord() {
            // select first member if no one is selected
            if(SuggestionList.SelectedItems.Count == 0 && SuggestionList.Items.Count > 0) {
                SuggestionList.Items[0].Selected = true;
            }

            if(SuggestionList.SelectedItems.Count > 0) {
                _parentBox.ReplaceCurrentWord(SuggestionList.SelectedItems[0].Text);
            }
        }

        private void SuggestionList_DoubleClick(object sender, EventArgs e) {
            InsertSelectedWord();
        }
    }
}
