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
using System.Drawing.Drawing2D;

namespace SecureDeleteWinForms {
    public partial class PanelEx : Panel {
        private const int DefaultCollapsedSize = 24;

        public PanelEx() {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        private Image panelImage = PanelResources.up;
        public TimeTracker timeTracker;
        private int startHeight;
        private int deltaHeight;

        private bool _collapsed;
        public bool Collapsed {
            get { return _collapsed; }
            set {
                _collapsed = value;

                // load image
                if(_collapsed) {
                    panelImage = PanelResources.down;
                }
                else {
                    panelImage = PanelResources.up;
                }

                // announce parent
                if(OnPanelStateChanged != null) {
                    OnPanelStateChanged(this, null);
                }
            }
        }

        private int _collapsedSize = DefaultCollapsedSize;
        public int CollapsedSize {
            get { return _collapsedSize; }
            set {
                _collapsedSize = value;

                if(OnPanelLayoutChanged != null) {
                    OnPanelLayoutChanged(this, null);
                }
            }
        }

        private int _expandedSize;

        public int ExpandedSize {
            get { return _expandedSize; }
            set {
                _expandedSize = value;

                if(OnPanelLayoutChanged != null) {
                    OnPanelLayoutChanged(this, null);
                }
            }
        }

        private bool _allowCollapse;
        public bool AllowCollapse {
            get { return _allowCollapse; }
            set { _allowCollapse = value; }
        }

        public int PanelSize {
            get {
                if(timeTracker != null && timeTracker.Running) {
                    timeTracker.Update();
                    return startHeight + (int)((double)deltaHeight * 
                           timeTracker.DeltaTimeToPercent());
                }
                else if(Collapsed) {
                    return _collapsedSize;
                }
                else {
                    return _expandedSize;
                }
            }
        }

        private string _title;
        public string Title {
            get { return _title; }
            set {
                _title = value;
                this.Invalidate();
                this.Update();
            }
        }

        private string _subtitle;
        public string Subtitle {
            get { return _subtitle; }
            set {
                _subtitle = value;
                this.Invalidate();
                this.Update();
            }
        }
        private Color _gradientColor1 = Color.White;
        public Color GradientColor1 {
            get { return _gradientColor1; }
            set {
                _gradientColor1 = value;
                this.Invalidate();
                this.Update();
            }
        }

        private Color _gradientColor2 = Color.FromName("Control");
        public Color GradientColor2 {
            get { return _gradientColor2; }
            set {
                _gradientColor2 = value;
                this.Invalidate();
                this.Update();
            }
        }

        private Color _textColor = Color.FromName("WindowText");
        public Color TextColor {
            get { return _textColor; }
            set {
                _textColor = value;
                this.Invalidate();
                this.Update();
            }
        }

        public event EventHandler OnPanelStateChanged;
        public event EventHandler OnPanelLayoutChanged;
        public event EventHandler OnAnimationCompleted;

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            LinearGradientBrush brush = new LinearGradientBrush(new RectangleF(0, 0, 10, _collapsedSize),
                                                                _gradientColor1, _gradientColor2,
                                                                LinearGradientMode.Vertical);

            e.Graphics.FillRectangle(brush, 0, 0, this.Width, _collapsedSize);
            e.Graphics.DrawImageUnscaled(panelImage, 5, 5);
            e.Graphics.DrawString(_title, this.Font, new SolidBrush(_textColor), 24, 6);

            if(_subtitle != null && _subtitle.Length > 0) {
                // draw the subtitle in the right side of the panel
                SizeF size = e.Graphics.MeasureString(_subtitle, this.Font);
                e.Graphics.DrawString(_subtitle, this.Font, new SolidBrush(_textColor), 
                                      this.Width - size.Width - 5,
                                      _collapsedSize / 2 - size.Height / 2);
            }

            // paint the border
            Pen p = new Pen(new SolidBrush(Color.FromName("ControlDark")));
            e.Graphics.DrawLine(p, 0, _collapsedSize, this.Width + 1, _collapsedSize);

            // dispose resources
            p.Dispose();
            brush.Dispose();
        }

        private void PanelEx_MouseUp(object sender, MouseEventArgs e) {
            if(_allowCollapse) {
                if(this.PointToClient(e.Location).Y <= _collapsedSize) {
                    Collapsed = !Collapsed;
                }
            }
        }

        private void AnimationCompletedHandler(object sender, EventArgs e) {
            if(OnAnimationCompleted != null) {
                OnAnimationCompleted(this, null);
            }
        }

        public void StartAnimation(int duration) {
            startHeight = this.Height;
            deltaHeight = (_collapsed ? _collapsedSize : _expandedSize) - startHeight;

            timeTracker = new TimeTracker();
            timeTracker.Duration = duration;
            timeTracker.OnTimeEllapsed += AnimationCompletedHandler;
            timeTracker.StartTimer();
        }

        private void InitializeComponent() {
            this.SuspendLayout();
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelEx_MouseUp);
            this.ResumeLayout(false);

        }
    }
}
