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
//// * Products derived from this software may not be called "SecureDelete" nor
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
using System.IO;
using SecureDelete.FileSearch;
using System.Windows.Forms;
using TextParser;
using DebugUtils.Debugger;
using SecureDelete;

namespace SecureDeleteWinForms.WipeTools {
    public partial class FilterBox : UserControl {
        class FilterStyle {
            #region Properties

            private Color _backColor1;
            public Color BackColor1 {
                get { return _backColor1; }
                set { _backColor1 = value; }
            }

            private Color _backColor2;
            public Color BackColor2 {
                get { return _backColor2; }
                set { _backColor2 = value; }
            }

            private Type[] _types;
            public Type[] Types {
                get { return _types; }
                set { _types = value; }
            }

            #endregion

            #region Constructor

            public FilterStyle() {

            }

            public FilterStyle(Type[] types, Color backColor1, Color backColor2) {
                _types = types;
                _backColor1 = backColor1;
                _backColor2 = backColor2;
            }

            #endregion
        }

        private const int FilterViewHeight = 34;
        private bool updatingColor;

        // _filter styles
        private FilterStyle[] filterStyles = new FilterStyle[] 
		{
			new FilterStyle(new Type[] { typeof(AttributeFilterView) }, Color.FromArgb(148,119,38), Color.FromArgb(173,139,45)), // gray
			new FilterStyle(new Type[] { typeof(DateFilterView) },      Color.FromArgb(95,102,45),  Color.FromArgb(119,128,56)), // yelloow
			new FilterStyle(new Type[] { typeof(ImageFilterView) },     Color.FromArgb(41,64,79),  Color.FromArgb(54,84,105)), // coral green
			new FilterStyle(new Type[] { typeof(SizeFilterView) },      Color.FromArgb(105,35,33),   Color.FromArgb(130,43,42)), // dark blue
		};
        
        public FilterBox() {
            InitializeComponent();

            fileFilter = new FileFilter();
            filterViews = new List<IFilterView>();
        }

        private void sizeFilterToolStripMenuItem_Click(object sender, EventArgs e) {
            AddSizeFilter(null, true);
        }

        private FileFilter fileFilter;
        private List<IFilterView> filterViews;
        private Parser parser;
        private CompletitionDialog completitionDialog;
        private int lastExpressionPosition;
        private bool loading;

        public FileFilter FileFilter {
            get { return fileFilter; }
            set {
                fileFilter = value;
                LoadFilters();
            }
        }

        private bool _filterEnabled;
        public bool FilterEnabled {
            get { return _filterEnabled; }
            set {
                _filterEnabled = value;

                if(fileFilter == null) {
                    fileFilter = new FileFilter();
                }

                fileFilter.Enabled = _filterEnabled;
                SetEnabledState(_filterEnabled);
                LoadTemplates();
            }
        }

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set { _options = value; }
        }

        public void Initialize() {
            Debug.AssertNotNull(_options, "SDOptions not set");

            LoadTemplates();
            FilterEnabled = false;
        }

        private void LoadTemplates() {
            if(_options == null || _options.FilterStore == null) {
                return;
            }

            FilterStore store = _options.FilterStore;
            TemplateList.Items.Clear();

            foreach(FilterStoreItem item in store.Items) {
                TemplateList.Items.Add(item.Name);
            }

            LoadTemplateButton.Enabled = _filterEnabled && TemplateList.SelectedItem != null;
            RemoveTemplateButton.Enabled = _filterEnabled && TemplateList.Items.Count > 0;
            TemplateList.Enabled = _filterEnabled && TemplateList.Items.Count > 0;
        }

        private void SetEnabledState(bool enabled) {
            AddButton.Enabled = enabled;
            removeDisabledToolStripMenuItem.Enabled = enabled;
            AdvancedButton.Enabled = enabled;
            TemplatesLabel.Enabled = enabled && TemplateList.Items.Count > 0;
            TemplateList.Enabled = enabled && TemplateList.Items.Count > 0;
            LoadTemplateButton.Enabled = enabled && TemplateList.SelectedItem != null;
            RemoveTemplateButton.Enabled = enabled && TemplateList.Items.Count > 0;
            AddTemplateButton.Enabled = enabled && filterViews.Count > 0;
            FilterNumberLabel.Enabled = enabled;

            if(filterViews.Count == 0) {
                RemoveButton.Enabled = enabled;
                removeAllToolStripMenuItem.Enabled = enabled;
            }
            else {
                RemoveButton.Enabled = true;
                removeAllToolStripMenuItem.Enabled = true;
            }

            // set views state
            foreach(IFilterView view in filterViews) {
                view.FilterViewEnabled = enabled;
            }

            ExpressionText.Enabled = enabled;
            ExpressionLabel.Enabled = enabled;
        }

        private void LoadFilters() {
            loading = true;
            FilterHost.SuspendLayout();
            filterViews.Clear();
            FilterHost.Controls.Clear();

            if(fileFilter == null) {
                FilterHost.ResumeLayout();
                return;
            }

            int count = fileFilter.FilterCount;
            for(int i = 0; i < count; i++) {
                FilterBase filter = fileFilter.GetFilter(i);

                if(filter is SizeFilter) {
                    AddSizeFilter(filter as SizeFilter, false);
                }
                else if(filter is DateFilter) {
                    AddDateFilter(filter as DateFilter, false);
                }
                else if(filter is AttributeFilter) {
                    AddAttributeFilter(filter as AttributeFilter, false);
                }
                else if(filter is ImageFilter) {
                    AddImageFilter(filter as ImageFilter, false);
                }
                else {
                    Debug.ReportWarning("Unsupported file Filter. Type: {0}", filter.GetType().FullName);
                }
            }

            LayoutFilterViews();
            FilterHost.ResumeLayout();

            if(fileFilter.ExpressionTree != null) {
                ExpressionEvaluator evaluator = new ExpressionEvaluator();
                ExpressionText.Text = evaluator.GetExpressionTreeString(fileFilter.ExpressionTree);
                AdvancedButton.Checked = true;
                splitContainer1.Panel2Collapsed = false;
                ExpressionText.Height = splitContainer1.Panel2.Height;
                UpdateTextColor(GetFilerNames());
            }

            loading = false;
        }

        private void LayoutFilterViews() {
            FilterHost.AutoScrollPosition = new Point(0, 0);
            int y = 0;

            for(int i = 0; i < filterViews.Count; i++) {
                UserControl view = (UserControl)filterViews[i];
                view.Top = y;
                view.Height = filterViews[i].FilterViewHeight;
                SetViewStyle(view, i);

                y += filterViews[i].FilterViewHeight;
            }
        }

        private void SetViewStyle(UserControl view, int position) {
            // find the style
            for(int i = 0; i < filterStyles.Length; i++) {
                Type[] types = filterStyles[i].Types;
                int count = types.Length;

                for(int j = 0; j < count; j++) {
                    if(view.GetType() == types[j]) {
                        // found
                        view.BackColor = position % 2 == 0 ? filterStyles[i].BackColor1 : filterStyles[i].BackColor2;
                        return;
                    }
                }
            }
        }

        #region Filter events

        private void OnFilterAutoRemove(object sender, EventArgs e) {
            if(sender is IFilterView) {
                RemoveFilter((IFilterView)sender, true, true);
                ParseExpression();
            }
        }

        private void OnStateChanged(object sender, EventArgs e) {
            ReplaceFilters();
            ParseExpression();
        }

        private void OnLayoutChanged(object sender, EventArgs e) {
            IFilterView view = (IFilterView)sender;

            if(((UserControl)view).Height != view.FilterViewHeight) {
                LayoutFilterViews();
            }
        }

        private void ReplaceFilters() {
            if(loading == false) {
                fileFilter.RemoveAllFilters();

                foreach(IFilterView view in filterViews) {
                    fileFilter.AddFilter(view.Filter);
                }
            }
        }

        #endregion

        private void AddFilterView(IFilterView view) {
            UserControl viewControl = (UserControl)view;
            viewControl.Width = FilterHost.ClientSize.Width;
            viewControl.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;

            view.OnRemove += OnFilterAutoRemove;
            view.OnStateChanged += OnStateChanged;
            view.OnLayoutChanged += OnLayoutChanged;

            filterViews.Add(view);
            FilterHost.Controls.Add(viewControl);
            LayoutFilterViews();
            UpdateFilterNumber();
            FilterHost.ScrollControlIntoView(viewControl);
        }

        private void UpdateFilterNumber() {
            FilterNumberLabel.Text = "Filters: " + filterViews.Count.ToString();
            AddTemplateButton.Enabled = filterViews.Count > 0;
            DefaultNamesButton.Visible = AdvancedButton.Checked;
            DefaultNamesButton.Enabled = filterViews.Count > 0;
        }

        private void AddSizeFilter(SizeFilter filter, bool addToFileFilter) {
            SizeFilterView view = new SizeFilterView();

            if(filter == null) {
                view.Filter = new SizeFilter();
            }
            else {
                view.Filter = filter;
            }

            if(addToFileFilter) {
                fileFilter.AddFilter(view.Filter);
            }

            AddFilterView(view);
        }

        private void AddDateFilter(DateFilter filter, bool addToFileFilter) {
            DateFilterView view = new DateFilterView();

            if(filter == null) {
                view.Filter = new DateFilter(FileDateType.CreationDate, DateImplication.From, DateTime.Now);
            }
            else {
                view.Filter = filter;
            }

            if(addToFileFilter) {
                fileFilter.AddFilter(view.Filter);
            }

            AddFilterView(view);
        }

        private void AddAttributeFilter(AttributeFilter filter, bool addToFileFilter) {
            AttributeFilterView view = new AttributeFilterView();

            if(filter == null) {
                view.Filter = new AttributeFilter(FileAttributes.Archive);
            }
            else {
                view.Filter = filter;
            }

            if(addToFileFilter) {
                fileFilter.AddFilter(view.Filter);
            }

            AddFilterView(view);
        }

        private void AddImageFilter(ImageFilter filter, bool addToFileFilter) {
            ImageFilterView view = new ImageFilterView();

            if(filter == null) {
                view.Filter = ImageFilterProvider.GetImageFilter(ImageFilter.ImageProperty.Author);
            }
            else {
                view.Filter = filter;
            }

            if(addToFileFilter) {
                fileFilter.AddFilter(view.Filter);
            }

            AddFilterView(view);
        }

        private void RemoveDisabledFilters() {
            int position = 0;
            FilterHost.SuspendLayout();

            while(position < filterViews.Count) {
                if(filterViews[position].Filter.Enabled == false) {
                    RemoveFilter(filterViews[position], false, true);
                    position--;
                }

                position++;
            }

            FilterHost.ResumeLayout();
            UpdateFilterNumber();
            LayoutFilterViews();
        }

        private void RemoveAllFilters() {
            while(filterViews.Count > 0) {
                RemoveFilter(filterViews[0], false, true);
            }

            ExpressionText.Text = "";
            UpdateFilterNumber();
        }

        private void RemoveFilter(IFilterView view, bool update, bool removeFromFileFilter) {
            FilterHost.Controls.Remove((UserControl)view);
            filterViews.Remove(view);

            if(removeFromFileFilter) {
                fileFilter.RemoveFilter(view.Filter);
            }

            if(update) {
                UpdateFilterNumber();
                LayoutFilterViews();
            }

            view.OnStateChanged -= OnStateChanged;
            view.OnRemove -= OnFilterAutoRemove;
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveAllFilters();
            FilterEnabled = _filterEnabled;
        }

        private void removeDisabledToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveDisabledFilters();
        }

        private void dateFilterToolStripMenuItem_Click(object sender, EventArgs e) {
            AddDateFilter(null, true);
        }

        private void attributeFilterToolStripMenuItem_Click(object sender, EventArgs e) {
            AddAttributeFilter(null, true);
        }

        #region Templates

        private void TemplateAddButton_Click(object sender, EventArgs e) {
            SaveAsTemplate();
        }

        private void SaveAsTemplate() {
            TemplateName form = new TemplateName();

            if(form.ShowDialog() == DialogResult.OK) {
                FilterStoreItem item = new FilterStoreItem();
                item.Name = form.Name.Text;
                item.Filter = (FileFilter)fileFilter.Clone();

                _options.FilterStore.Add(item);
                SDOptionsFile.TrySaveOptions(_options);
                LoadTemplates();
            }
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e) {
            if(TemplateList.SelectedIndex >= 0 && TemplateList.SelectedIndex < _options.FilterStore.Items.Count) {
                _options.FilterStore.Items.RemoveAt(TemplateList.SelectedIndex);
                TemplateList.Items.RemoveAt(TemplateList.SelectedIndex);
                LoadTemplateButton.Enabled = TemplateList.SelectedItem != null;
                RemoveTemplateButton.Enabled = TemplateList.Items.Count > 0;
                SDOptionsFile.TrySaveOptions(_options);
                LoadTemplates();
            }
        }

        private void deleteAllTemplatesToolStripMenuItem_Click(object sender, EventArgs e) {
            _options.FilterStore.Items.Clear();
            TemplateList.Items.Clear();
            RemoveTemplateButton.Enabled = false;
            SDOptionsFile.TrySaveOptions(_options);
            LoadTemplates();
        }

        private void LoadTemplateButton_Click(object sender, EventArgs e) {
            if(TemplateList.SelectedIndex >= 0 && TemplateList.SelectedIndex < _options.FilterStore.Items.Count) {
                RemoveAllFilters();
                fileFilter = (FileFilter)_options.FilterStore.Items[TemplateList.SelectedIndex].Filter.Clone();
                LoadFilters();
            }
        }

        #endregion

        #region Expressions

        private FilterName[] GetFilerNames() {
            List<FilterName> names = new List<FilterName>();

            for(int i = 0; i < fileFilter.FilterCount; i++) {
                FilterBase filter = fileFilter.GetFilter(i);

                if(filter.Name != null && filter.Name != string.Empty) {
                    names.Add(new FilterName(filter.Name, filter.Enabled, ExpressionType.Filter));
                }
            }

            return names.ToArray();
        }

        private bool ContainsParantesis(string text) {
            for(int i = 0; i < text.Length; i++) {
                if(text[i] == '(' || text[i] == ')') {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsName(string text, FilterName[] filterNames) {
            for(int i = 0; i < filterNames.Length; i++) {
                if(filterNames[i].Name.StartsWith(text)) {
                    return true;
                }
            }

            return false;
        }

        private void UpdateTextColor(FilterName[] filterNames) {
            if(parser == null) {
                parser = new Parser(new ExpressionFilter());
            }
            else {
                parser.FieldFilters.Clear();
            }

            int position = ExpressionText.SelectionStart;
            ExpressionText.HideSelection = true;
            ExpressionText.SuspendLayout();

            // add filter names
            Dictionary<string, FilterName> filterDictionary = new Dictionary<string, FilterName>();

            for(int i = 0; i < filterNames.Length; i++) {
                parser.FieldFilters.Add(filterNames[i].Name);

                if(filterDictionary.ContainsKey(filterNames[i].Name) == false) {
                    filterDictionary.Add(filterNames[i].Name, filterNames[i]);
                }
            }

            parser.Text = ExpressionText.Text + "  ";
            parser.InitParsing();
            TextBlock? block = parser.GetNextBlock();

            StringBuilder builder = new StringBuilder();
            StringBuilder textBuilder = new StringBuilder(ExpressionText.Text);
            int diff = 0;
            builder.Append("{\\rtf1\\ansi\\deff0");

            // color table
            //
            //           1      2    3       4         5
            // colors: black, blue, red, dark violet, gray
            builder.Append("{\\colortbl;\\red0\\green0\\blue0;\\red0\\green0\\blue255;\\red255\\green0\\blue0;\\red148\\green0\\blue211;\\red128\\green128\\blue128;}");

            // parse the text
            while(block.HasValue) {
                TextBlock b = block.Value;

                if(b.Type == TextBlockType.Keyword) {
                    /*ExpressionText.SelectionStart = b.StartIndex;
                    ExpressionText.SelectionLength = b.EndIndex - b.StartIndex + 1;
                    ExpressionText.SelectionColor = Color.DarkViolet;*/

                    if(b.EndIndex - b.StartIndex > 0) {
                        textBuilder.Replace(b.Text, "\\highlight0\\cf4" + b.Text, diff + b.StartIndex, b.EndIndex - b.StartIndex);
                        diff += 15;
                    }
                }
                else if(b.Type == TextBlockType.Field) {
                    int color = 5;
                    /*ExpressionText.SelectionStart = b.StartIndex;
                    ExpressionText.SelectionLength = b.EndIndex - b.StartIndex;
                    */
                    // set color
                    if(filterDictionary.ContainsKey(b.Text) == false ||
                      (filterDictionary.ContainsKey(b.Text) && filterDictionary[b.Text].Enabled == false)) {

                        //ExpressionText.SelectionColor = Color.Gray;
                        color = 5;
                    }
                    else {
                        //ExpressionText.SelectionColor = Color.Blue;
                        color = 2;
                    }

                    // append
                    if(b.EndIndex - b.StartIndex > 0) {
                        textBuilder.Replace(b.Text, "\\highlight0\\cf" + color.ToString() + b.Text, diff + b.StartIndex, b.EndIndex - b.StartIndex);
                        diff += 15;
                    }
                }
                else if(ContainsParantesis(b.Text) == false &&
                   ContainsName(b.Text, filterNames) == false) {
                    /*// mark as error
                    ExpressionText.SelectionStart = b.StartIndex;
                    ExpressionText.SelectionLength = b.EndIndex - b.StartIndex;
                    ExpressionText.SelectionBackColor = Color.LightCoral;*/

                    if(b.EndIndex - b.StartIndex > 0) {
                        textBuilder.Replace(b.Text, "\\highlight0\\cf3" + b.Text, diff + b.StartIndex, b.EndIndex - b.StartIndex);
                        diff += 15;
                    }
                }
                else {
                    // unknown
                    if(b.EndIndex - b.StartIndex > 0) {
                        textBuilder.Replace(b.Text, "\\highlight0\\cf1" + b.Text, diff + b.StartIndex, b.EndIndex - b.StartIndex);
                        diff += 15;
                    }
                }

                block = parser.GetNextBlock();
            }

            updatingColor = true;
            builder.Append(textBuilder.ToString());
            ExpressionText.Rtf = builder.ToString();
            ExpressionText.SelectionStart = position;
            ExpressionText.SelectionLength = 0;
            updatingColor = false;
        }

        private bool IsExpressionSeparator(char c) {
            int length = ExpressionFilter.separators.Length;

            for(int i = 0; i < length; i++) {
                if(c == ExpressionFilter.separators[i]) {
                    return true;
                }
            }

            return false;
        }

        private bool GetCurrentWord(out string word, out int start, out int end) {
            word = null;
            start = end = -1;
            int position = ExpressionText.SelectionStart;
            string text = ExpressionText.Text;

            if(text.Length == 0) {
                return false;
            }

            if(position - 1 >= 0 && position - 1 < text.Length && IsExpressionSeparator(text[position - 1])) {
                return false;
            }

            // left position
            start = position - 1;

            while(start >= 0) {
                if(IsExpressionSeparator(text[start])) {
                    start++;
                    break;
                }
                else {
                    start--;
                }
            }

            start = Math.Max(0, start);

            // right position
            end = position - 1;
            while(end >= 0 && end < text.Length - 1) {
                if(IsExpressionSeparator(text[end])) {
                    end--;
                    break;
                }
                else {
                    end++;
                }
            }

            // get word
            if(start != -1 && end != -1 && start <= end) {
                word = text.Substring(start, end - start + 1);
                return true;
            }
            else {
                return false;
            }
        }

        private void ParseExpression() {
            ExpressionEvaluator evaluator = new ExpressionEvaluator();
            evaluator.Filters = fileFilter;
            fileFilter.ExpressionTree = evaluator.EvaluateExpression(ExpressionText.Text);
            InvalidLabel.Visible = fileFilter.ExpressionTree == null && ExpressionText.Text.Trim() != string.Empty;

            // get the _filter names and update the color
            FilterName[] filterNames = GetFilerNames();
            UpdateTextColor(filterNames);

            // get current word
            string word;
            int start;
            int end;
            List<FilterName> suggestions = new List<FilterName>();

            if(GetCurrentWord(out word, out start, out end)) {
                // match the word with the _filter names
                int length = filterNames.Length;
                for(int i = 0; i < length; i++) {
                    if(filterNames[i].Name == word) {
                        suggestions.Clear();
                        break;
                    }

                    if(filterNames[i].Name.ToLower().Contains(word.ToLower())) {
                        // add the _filter name
                        suggestions.Add(filterNames[i]);
                    }
                }

                // match it with the logical operators
                if(ExpressionEvaluator.AndImplication.ToLower().StartsWith(word.ToLower())) {
                    suggestions.Add(new FilterName(ExpressionEvaluator.AndImplication, true, ExpressionType.Implication));
                }

                if(ExpressionEvaluator.OrImplication.ToLower().StartsWith(word.ToLower())) {
                    suggestions.Add(new FilterName(ExpressionEvaluator.OrImplication, true, ExpressionType.Implication));
                }
            }

            if(suggestions.Count > 0) {
                // show dialog if not already visible
                if(completitionDialog == null) {
                    completitionDialog = new CompletitionDialog();
                }

                // place the dialog
                Point p;
                Rectangle r;

                if(completitionDialog.Visible) {
                    p = ExpressionText.GetPositionFromCharIndex(start);
                    r = ExpressionText.RectangleToScreen(new Rectangle(p.X, p.Y - 2, 10, 10));
                }
                else {
                    p = ExpressionText.GetPositionFromCharIndex(start);
                    r = ExpressionText.RectangleToScreen(new Rectangle(p.X, p.Y - 2, 10, 10));
                }

                // set suggestions and show
                completitionDialog.ParentControl = ExpressionText;
                completitionDialog.ParentBox = this;
                completitionDialog.Suggestions = suggestions;
                completitionDialog.Show();
                completitionDialog.Left = r.X;
                completitionDialog.Top = r.Y - completitionDialog.Height;

                // move focus back on edit box
                ExpressionText.Focus();
            }
            else {
                if(completitionDialog != null) {
                    completitionDialog.Hide();
                }
            }
        }

        public void ReplaceCurrentWord(string word) {
            string temp;
            int start;
            int end;

            if(GetCurrentWord(out temp, out start, out end)) {
                string expression = ExpressionText.Text.Substring(0, start);
                expression += word;
                expression += ExpressionText.Text.Substring(end + 1);
                ExpressionText.Text = expression;
                ExpressionText.SelectionStart = start + word.Length + 1;
            }
        }

        private void Expression_TextChanged(object sender, EventArgs e) {
            if(updatingColor == false) {
                ParseExpression();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            splitContainer1.Panel2Collapsed = !AdvancedButton.Checked;
            ExpressionText.Height = splitContainer1.Panel2.Height;
            DefaultNamesButton.Visible = AdvancedButton.Checked;
        }

        private void toolStripButton3_ButtonClick(object sender, EventArgs e) {

        }

        private void TemplateList_SelectedIndexChanged(object sender, EventArgs e) {
            LoadTemplateButton.Enabled = TemplateList.SelectedItem != null;
        }

        private void ExpressionText_KeyDown(object sender, KeyEventArgs e) {
            if(completitionDialog != null && completitionDialog.Visible) {
                if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab) {
                    completitionDialog.InsertSelectedWord();
                    e.SuppressKeyPress = true;
                }
                else if(e.KeyCode == Keys.Up) {
                    completitionDialog.SelectUp();
                }
                else if(e.KeyCode == Keys.Down) {
                    completitionDialog.SelectDown();
                }
                else if(e.KeyCode == Keys.Escape) {
                    completitionDialog.Hide();
                }
            }
        }

        private void ExpressionText_Leave(object sender, EventArgs e) {
            if(completitionDialog != null && completitionDialog.Visible) {
                completitionDialog.Hide();
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e) {
            SetDefaultNames();
        }

        private void SetDefaultNames() {
            int[] count = new int[] { 1, 1, 1, 1 };

            foreach(IFilterView view in filterViews) {
                if(view is SizeFilterView) {
                    view.Filter.Name = string.Format("size{0}", count[0]);
                    count[0]++;
                }
                else if(view is DateFilterView) {
                    view.Filter.Name = string.Format("date{0}", count[1]);
                    count[1]++;
                }
                else if(view is AttributeFilterView) {
                    view.Filter.Name = string.Format("attribute{0}", count[2]);
                    count[2]++;
                }
                else if(view is ImageFilterView) {
                    view.Filter.Name = string.Format("image{0}", count[3]);
                    count[3]++;
                }

                view.Filter = view.Filter;
            }
        }

        #endregion

        private void pictureToolStripMenuItem_Click(object sender, EventArgs e) {
            AddImageFilter(null, true);
        }
    }

    public class FilterName {
        public string Name;
        public bool Enabled;
        public ExpressionType Type;

        public FilterName() { }

        public FilterName(string name, bool enabled, ExpressionType type) {
            Name = name;
            Enabled = enabled;
            Type = type;
        }
    }
}
