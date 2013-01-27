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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using SecureDelete.Schedule;

namespace SecureDeleteWinForms {
    public partial class HistoryVisualizer : UserControl {
        public delegate void SelectedVisualDelegate(object sender, HistoryVisual visual);

        #region Fields

        private bool invalidated;
        private int firstVisual;
        private HScrollBar ScrollBar;
        private int lastVisual;
        private Bitmap backScreen;
        private HistoryVisual selectedVisual;
        private HistoryVisual hoveredVisual;

        #endregion

        #region Constructor

        public HistoryVisualizer() {
            _visuals = new VisualList();
            _visuals.Parent = this;
            InitializeComponent();
        }

        #endregion

        #region Properties

        private VisualList _visuals;
        public VisualList Visuals {
            get { return _visuals; }
            set { _visuals = value; }
        }

        [Category("My Properties")]
        [Description("MyControl properties")]
        private int _visualWidth;
        public int VisualWidth {
            get { return _visualWidth; }
            set {
                _visualWidth = value;
                ScrollBar.SmallChange = ScrollBar.LargeChange = value;
                Invalidate();
            }
        }

        private int _visualHeight;
        public int VisualHeight {
            get { return _visualHeight; Invalidate(); }
        }

        private Brush _backgroundColor;
        public Brush BackgroundColor {
            get { return _backgroundColor; }
            set { _backgroundColor = value; Invalidate(); }
        }

        private Brush _normalColor1;
        public Brush NormalColor1 {
            get { return _normalColor1; }
            set { _normalColor1 = value; Invalidate(); }
        }

        private Brush _normalColor2;
        public Brush NormalColor2 {
            get { return _normalColor2; }
            set { _normalColor2 = value; Invalidate(); }
        }

        private Brush _errorColor1;
        public Brush ErrorColor1 {
            get { return _errorColor1; }
            set { _errorColor1 = value; Invalidate(); }
        }

        private Brush _errorColor2;
        public Brush ErrorColor2 {
            get { return _errorColor2; }
            set { _errorColor2 = value; Invalidate(); }
        }

        private Brush _failedColor1;
        public Brush FailedColor1 {
            get { return _failedColor1; }
            set { _failedColor1 = value; Invalidate(); }
        }

        private Brush _failedColor2;
        public Brush FailedColor2 {
            get { return _failedColor2; }
            set { _failedColor2 = value; Invalidate(); }
        }

        private Brush _pointColor;
        public Brush PointColor {
            get { return _pointColor; }
            set { _pointColor = value; Invalidate(); }
        }

        private int _pointRadius;
        public int PointRadius {
            get { return _pointRadius; }
            set { _pointRadius = value; Invalidate(); }
        }

        private bool _showStartTime;
        public bool ShowStartTime {
            get { return _showStartTime; }
            set { _showStartTime = value; Invalidate(); }
        }

        private Brush _textColor;
        public Brush TextColor {
            get { return _textColor; }
            set { _textColor = value; Invalidate(); }
        }

        private bool _showDurationHistogram;
        public bool ShowDurationHistogram {
            get { return _showDurationHistogram; }
            set { _showDurationHistogram = value; Invalidate(); }
        }

        private Pen _durationHistogramColor;
        public Pen DurationHistogramColor {
            get { return _durationHistogramColor; }
            set { _durationHistogramColor = value; Invalidate(); }
        }

        private Pen _selectionColor;
        public Pen SelectionColor {
            get { return _selectionColor; }
            set { _selectionColor = value; Invalidate(); }
        }

        private bool _suspendUpdate;
        public bool SuspendUpdate {
            get { return _suspendUpdate; }
            set {
                _suspendUpdate = value;

                if(_suspendUpdate == false && invalidated) {
                    UpdateDisplay();
                }
            }
        }

        public int ScrollbarHeight {
            get {
                if(ScrollBar.Visible == false) {
                    return 0;
                }
                else {
                    return ScrollBar.Height;
                }
            }
        }

        #endregion

        #region Private methods

        private void LayoutVisuals() {
            if(_visuals == null || invalidated == false) {
                return;
            }

            UpdateScrollBar();
            int count = _visuals.Count;

            for(int i = 0; i < count; i++) {
                HistoryVisual visual = _visuals[i];

                visual.X = i * _visualWidth;
                visual.Y = 0;
                visual.Width = _visualWidth;
                visual.Height = _visualHeight;

                // set background color
                if(HistoryItem.IsOk(visual.History)) {
                    visual.BackColor = i % 2 == 0 ? _normalColor1 : _normalColor2;
                }
                else if(HistoryItem.IsWithErrors(visual.History)) {
                    visual.BackColor = i % 2 == 0 ? _errorColor1 : _errorColor2;
                }
                else {
                    visual.BackColor = i % 2 == 0 ? _failedColor1 : _failedColor2;
                }
            }

            invalidated = false;
        }

        private void UpdateScrollBar() {
            ScrollBar.Maximum = Math.Max(0, (_visualWidth * _visuals.Count) - this.Width);
            ScrollBar.LargeChange = 1;
            ScrollBar.SmallChange = 1;
            ScrollBar.Visible = ScrollBar.Maximum > 0;
            _visualHeight = this.Height - (ScrollBar.Maximum > 0 ? ScrollBar.Height : 0);
        }

        private void Draw(Graphics g) {
            if(_visuals == null || _visualWidth == 0) {
                return;
            }

            if(invalidated) {
                LayoutVisuals();
            }

            g.FillRectangle(_backgroundColor, 0, -1, this.Width, _visualHeight + 1);

            int delta = Math.Max(0, ScrollBar.Value);
            firstVisual = delta / _visualWidth;
            lastVisual = Math.Min(_visuals.Count, (firstVisual + this.Width / _visualWidth) + 2);

            for(int i = firstVisual; i < lastVisual; i++) {
                _visuals[i].Draw(g, -delta);
            }

            if(_showDurationHistogram && _visuals.Count > 0) {
                DrawHistogram(g, delta);
            }
        }

        private void DrawHistogram(Graphics g, int delta) {
            int xa, ya;
            int xb, yb;
            int maxHeight;
            int startVisual = Math.Max(0, firstVisual - 1);
            TimeSpan duration;
            HistoryVisual visual;

            maxHeight = _visualHeight - _pointRadius;
            double maxDuration = GetMaxDuration();

            if(maxDuration == double.MinValue) {
                return;
            }

            // jump to the first item with a valid duration
            while(_visuals[startVisual].History.DeltaTime.HasValue == false && startVisual <= lastVisual) {
                startVisual++;
            }
            
            visual = _visuals[startVisual];
            duration = visual.History.DeltaTime.Value;
            xa = visual.X + _visualWidth / 2 - delta;
            ya = maxHeight - (int)((duration.TotalMilliseconds / maxDuration) * (double)maxHeight) + _pointRadius / 2;

            for(int i = startVisual + 1; i <= lastVisual; i++) {
                if(_visuals[i - 1].History.DeltaTime.HasValue) {
                    g.FillEllipse(_pointColor, xa - _pointRadius / 2, ya - _pointRadius / 2, _pointRadius, _pointRadius);
                }

                if(i < lastVisual) {
                    visual = _visuals[i];

                    if(visual.History.DeltaTime.HasValue) {
                        duration = visual.History.DeltaTime.Value;

                        xb = visual.X + _visualWidth / 2 - delta;
                        yb = maxHeight - (int)((duration.TotalMilliseconds / maxDuration) * (double)maxHeight) + _pointRadius / 2;

                        g.DrawLine(_durationHistogramColor, xa, ya, xb, yb);
                        
                        xa = xb;
                        ya = yb;
                    }
                }
            }
        }

        private void Invalidate() {
            invalidated = true;
            LayoutVisuals();
            UpdateDisplay();
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            // recreate back buffer
            if(backScreen != null) {
                backScreen.Dispose();
            }

            backScreen = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Invalidate();
        }

        private double GetMaxDuration() {
            double max = double.MinValue;

            for(int i = 0; i < _visuals.Count; i++) {
                TimeSpan? value = _visuals[i].History.DeltaTime;

                if(value.HasValue && value.Value.TotalMilliseconds > max) {
                    max = value.Value.TotalMilliseconds;
                }
            }

            return max;
        }

        private HistoryVisual GetVisualFromPoint(Point p) {
            int delta = Math.Max(0, ScrollBar.Value);
            firstVisual = delta / _visualWidth;
            lastVisual = Math.Min(_visuals.Count, (firstVisual + this.Width / _visualWidth) + 2);

            for(int i = firstVisual; i < lastVisual; i++) {
                HistoryVisual visual = _visuals[i];

                if((visual.X - delta) <= p.X && (visual.X + visual.Width - delta) >= p.X &&
                    visual.Y <= p.Y && (visual.Y + visual.Height) >= p.Y) {
                    return visual;
                }
            }

            return null;
        }

        public void SelectVisual(HistoryVisual visual) {
            if(selectedVisual != null) {
                selectedVisual.Selected = false;
            }

            selectedVisual = visual;
            selectedVisual.Selected = true;
            int delta = Math.Max(0, ScrollBar.Value);

            if((selectedVisual.X + selectedVisual.Width) - delta >= this.Width) {
                ScrollBar.Value = (selectedVisual.X + selectedVisual.Width) - this.Width;
            }
            else if(selectedVisual.X - delta < 0) {
                ScrollBar.Value = selectedVisual.X;
            }

            UpdateDisplay();
        }

        #endregion

        #region Events

        public event SelectedVisualDelegate OnSelectionChanged;
        public event SelectedVisualDelegate OnHoveredVisualChanged;

        #endregion

        #region Public methods

        public void ListModified() {
            invalidated = true;

            if(_suspendUpdate == false) {
                UpdateDisplay();
            }
        }

        #endregion

        private void HistoryVisualizer_Paint(object sender, PaintEventArgs e) {
            // use double-buffering
            if(backScreen == null) {
                backScreen = new Bitmap(this.Width, this.Height + 1, 
                                        System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }

            Graphics g = Graphics.FromImage(backScreen);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Draw(g);
            e.Graphics.DrawImageUnscaled(backScreen, 0, 0);
        }

        private void InitializeComponent() {
            this.ScrollBar = new System.Windows.Forms.HScrollBar();
            this.SuspendLayout();
            // 
            // ScrollBar
            // 
            this.ScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ScrollBar.Location = new System.Drawing.Point(0, 124);
            this.ScrollBar.Name = "ScrollBar";
            this.ScrollBar.Size = new System.Drawing.Size(593, 17);
            this.ScrollBar.TabIndex = 0;
            this.ScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ScrollBar_Scroll);
            // 
            // HistoryVisualizer
            // 
            this.Controls.Add(this.ScrollBar);
            this.Name = "HistoryVisualizer";
            this.Size = new System.Drawing.Size(593, 141);
            this.MouseLeave += new System.EventHandler(this.HistoryVisualizer_MouseLeave);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.HistoryVisualizer_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HistoryVisualizer_MouseMove);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HistoryVisualizer_MouseClick);
            this.ResumeLayout(false);
        }

        private void ScrollBar_Scroll(object sender, ScrollEventArgs e) {
            UpdateDisplay();
        }


        private void UpdateDisplay() {
            HistoryVisualizer_Paint(null, new PaintEventArgs(Graphics.FromHwnd(this.Handle), new Rectangle()));
        }


        private void HistoryVisualizer_MouseClick(object sender, MouseEventArgs e) {
            HistoryVisual visual = GetVisualFromPoint(new Point(e.X, e.Y));

            if(visual != null) {
                SelectVisual(visual);

                if(OnSelectionChanged != null) {
                    OnSelectionChanged(this, visual);
                }
            }
        }


        public void SelectNextVisual() {
            if(_visuals.Count == 0) {
                return;
            }

            if(selectedVisual == null) {
                SelectVisual(_visuals[0]);
            }

            int index = _visuals.IndexOf(selectedVisual);

            if(index + 1 < _visuals.Count) {
                SelectVisual(_visuals[index + 1]);
            }
        }


        public void SelectPreviousVisual() {
            if(_visuals.Count == 0) {
                return;
            }

            int index = _visuals.IndexOf(selectedVisual);

            if(index - 1 >= 0) {
                SelectVisual(_visuals[index - 1]);
            }
        }


        protected override void WndProc(ref Message m) {
            if((int)m.WParam == (int)System.Windows.Forms.Keys.Right) {
                SelectNextVisual();
            }
            else if((int)m.WParam == (int)System.Windows.Forms.Keys.Left) {
                SelectPreviousVisual();
            }
            else {
                base.WndProc(ref m);
            }
        }


        private void CheckHoveredVisual(Point p) {
            if(OnHoveredVisualChanged != null) {
                HistoryVisual visual = GetVisualFromPoint(p);

                if(visual != hoveredVisual) {
                    hoveredVisual = visual;
                    OnHoveredVisualChanged(this, visual);
                }
            }
        }


        private void HistoryVisualizer_MouseMove(object sender, MouseEventArgs e) {
            CheckHoveredVisual(new Point(e.X, e.Y));
        }


        private void HistoryVisualizer_MouseLeave(object sender, EventArgs e) {
            CheckHoveredVisual(new Point(-1, -1));
        }
    }

    public class VisualList : IList<HistoryVisual> {
        #region Fiedls

        private List<HistoryVisual> visuals = new List<HistoryVisual>();

        #endregion

        #region Properties

        private HistoryVisualizer _parent;
        public HistoryVisualizer Parent {
            get { return _parent; }
            set { _parent = value; }
        }

        #endregion

        #region Private methods

        private void ListModified() {
            if(_parent != null) {
                _parent.ListModified();
            }
        }

        #endregion

        #region IList Members

        public int Add(HistoryVisual value) {
            visuals.Add(value);
            value.Parent = _parent;
            ListModified();

            return 0;
        }

        public void Clear() {
            visuals.Clear();
            ListModified();
        }

        public bool Contains(HistoryVisual value) {
            return visuals.Contains(value);
        }

        public int IndexOf(HistoryVisual value) {
            return visuals.IndexOf(value);
        }

        public void Insert(int index, HistoryVisual value) {
            visuals.Insert(index, value);
            ListModified();
        }

        public bool IsFixedSize {
            get { return false; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public void Remove(HistoryVisual value) {
            visuals.Remove(value);
            value.Parent = null;
            ListModified();
        }

        public void RemoveAt(int index) {
            visuals[index].Parent = null;
            visuals.RemoveAt(index);
            ListModified();
        }

        public HistoryVisual this[int index] {
            get { return visuals[index]; }
            set {
                visuals[index] = value;
                ListModified();
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator() {
            return visuals.GetEnumerator();
        }

        #endregion

        #region ICollection<HistoryVisual> Members

        void ICollection<HistoryVisual>.Add(HistoryVisual item) {
            visuals.Add(item);
            item.Parent = _parent;
            ListModified();
        }

        public void CopyTo(HistoryVisual[] array, int arrayIndex) {
            return;
        }

        bool ICollection<HistoryVisual>.Remove(HistoryVisual item) {
            visuals.Remove(item);
            item.Parent = null;
            ListModified();
            return true;
        }

        public int Count {
            get { return visuals.Count; }
        }

        #endregion

        #region IEnumerable<HistoryVisual> Members

        IEnumerator<HistoryVisual> IEnumerable<HistoryVisual>.GetEnumerator() {
            return visuals.GetEnumerator();
        }

        #endregion
    }


    public class HistoryVisual {
        #region Fields

        public Brush BackColor;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public HistoryItem History;
        public HistoryVisualizer Parent;
        public bool Selected;
        public object Tag;

        #endregion

        #region Constructor

        public HistoryVisual() {

        }

        public HistoryVisual(HistoryItem history) {
            History = history;
        }

        #endregion

        #region Public methods

        public void Draw(Graphics g, int delta) {
            g.FillRectangle(BackColor, X + delta, Y - 1, Width + 1, Height + 1);

            // show start time
            if(Parent.ShowStartTime) {
                g.DrawString(History.StartTime.ToShortDateString(), Parent.Font, Parent.TextColor, X + delta, 0, 
                             new StringFormat(StringFormatFlags.DirectionVertical));
            }

            if(Selected) {
                g.DrawRectangle(Parent.SelectionColor, X + delta, Y, Width, Height - 1);
                g.DrawRectangle(Parent.SelectionColor, X + delta + 1, Y + 1, Width - 2, Height - 3);
            }
        }

        #endregion
    }
}
