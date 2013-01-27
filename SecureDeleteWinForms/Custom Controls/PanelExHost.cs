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

namespace SecureDeleteWinForms {
    public partial class PanelExHost : Panel {
        private List<PanelEx> animatedPanels;
        private object animationLock = new object();

        public PanelExHost() {
            InitializeComponent();
            animatedPanels = new List<PanelEx>();
        }

        private bool _animate = true;
        public bool Animate {
            get { return _animate; }
            set { _animate = value; }
        }

        private int _panelDistance;
        public int PanelDistance {
            get { return _panelDistance; }
            set {
                _panelDistance = value;
                LayoutPanels();
            }
        }

        private void DrawBorders() {
            // start position
            int y = 0;
            Graphics g = Graphics.FromHwnd(this.Handle);
            g.Clear(this.BackColor);

            for(int i = 0; i < this.Controls.Count; i++) {
                PanelEx panel = (PanelEx)this.Controls[i];
                g.DrawRectangle(new Pen(new SolidBrush(Color.FromName("ControlDark"))), 
                                panel.Bounds.Left - 1, panel.Bounds.Top - 1, 
                                panel.Bounds.Width + 1, panel.Bounds.Height + 1);
                panel.Invalidate();
                y += panel.Height;
                y += _panelDistance + 1;
            }
        }

        private void LayoutPanels() {
            // start position
            int y = 0;

            for(int i = 0; i < this.Controls.Count; i++) {
                PanelEx panel = (PanelEx)this.Controls[i];
                panel.Top = y + 1 - this.VerticalScroll.Value + Padding.Top;
                panel.Left = 1 + Padding.Left;
                panel.Height = panel.PanelSize;
                panel.Width = this.Width - Padding.Right - Padding.Left - 2 - 
                             (this.VerticalScroll.Visible ? (this.Width - this.ClientSize.Width) : 2);
                y += panel.Height;
                y += _panelDistance + 1;
            }

            DrawBorders();
        }

        private void PanelLayoutChanged(object sender, EventArgs e) {
            PanelEx panel = (PanelEx)sender;

            LayoutPanels();
        }

        private void PanelStateChanged(object sender, EventArgs e) {
            PanelEx panel = (PanelEx)sender;

            if(_animate) {
                lock(animationLock) {
                    panel.OnAnimationCompleted += AnimationCompleted;
                    panel.StartAnimation(200);
                    animatedPanels.Add(panel);
                    timer1.Enabled = true;
                }
            }

            LayoutPanels();
        }

        private void AnimationCompleted(object sender, EventArgs e) {
            PanelEx panel = (PanelEx)sender;

            lock(animationLock) {
                if(animatedPanels.Contains(panel)) {
                    animatedPanels.Remove(panel);
                }
            }
        }

        protected override void OnControlAdded(ControlEventArgs e) {
            // allow only PanelEx controls
            if((e.Control is PanelEx) == false) {
                return;
            }

            PanelEx panel = (PanelEx)e.Control;
            panel.Left = 0;
            panel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            // attach events
            panel.OnPanelLayoutChanged += PanelLayoutChanged;
            panel.OnPanelStateChanged += PanelStateChanged;
            
            base.OnControlAdded(e);
            LayoutPanels();
        }

        protected override void OnControlRemoved(ControlEventArgs e) {
            // allow only PanelEx controls
            if((e.Control is PanelEx) == false) {
                return;
            }

            // detach events
            PanelEx panel = (PanelEx)e.Control;
            panel.OnPanelLayoutChanged -= PanelLayoutChanged;
            panel.OnPanelStateChanged -= PanelStateChanged;

            base.OnControlRemoved(e);
            LayoutPanels();
        }

        protected override void OnResize(EventArgs eventargs) {
            base.OnResize(eventargs);
            this.Invalidate();
            this.Update();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            LayoutPanels();
        }

        protected override void OnInvalidated(InvalidateEventArgs e) {
            base.OnInvalidated(e);
            LayoutPanels();
        }


        protected override void OnScroll(ScrollEventArgs se) {
            base.OnScroll(se);
            this.Invalidate();
            this.Update();
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);
            this.Invalidate();
            this.Update();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            lock(animationLock) {
                if(animatedPanels.Count == 0) {
                    timer1.Enabled = false;
                }
                else {
                    this.Invalidate();
                    this.Update();
                }
            }
        }
    }
}
