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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SecureDeleteWinForms {
    public partial class StatusWindow : Form {
        #region Win32 Interop

        private struct POINTAPI {
            public int x;
            public int y;
        }

        private struct RECT {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private struct WINDOWPLACEMENT {
            public int length;
            public int flags;
            public int showCmd;
            public POINTAPI ptMinPosition;
            public POINTAPI ptMaxPosition;
            public RECT rcNormalPosition;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        #endregion

        public StatusWindow() {
            InitializeComponent();
            statusElements = new List<StatusWindowElement>();
        }

        #region Fields

        private const int ElementHeight = 23;
        private const int MaximumHeight = 230;
        private List<StatusWindowElement> statusElements;

        #endregion

        private void ToolCloseButton_Click(object sender, EventArgs e) {
            this.Hide();
        }

        private Rectangle GetTrayBounds() {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();

            // find the window
            IntPtr handle = FindWindow("Shell_TrayWnd", "");
            if(GetWindowPlacement(handle, ref placement)) {
                return new Rectangle(placement.rcNormalPosition.left, placement.rcNormalPosition.top,
                                     placement.rcNormalPosition.right - placement.rcNormalPosition.left,
                                     placement.rcNormalPosition.bottom - placement.rcNormalPosition.top);
            }
            else {
                return new Rectangle();
            }
        }

        private void StatusWindow_VisibleChanged(object sender, EventArgs e) {
            if(this.Visible) {
                // layout the tasks
                int elementHeight = LayoutElements();
                this.Height = Math.Min(MaximumHeight, elementHeight + ToolHeader.Height * 2 + 60);

                // place the window
                Rectangle rect = GetTrayBounds();

                if(rect.Y == 0 && rect.X == 0 && rect.Width > rect.Height) {
                    // top
                    this.Left = rect.Width - this.Width;
                    this.Top = rect.Height;
                }
                else if(rect.Y == 0 && rect.X > 0) {
                    // right
                    this.Left = rect.X - this.Width;
                    this.Top = rect.Height - this.Height;
                }
                else if(rect.Y > 0 && rect.X == 0) {
                    // bottom - default
                    this.Left = rect.X + rect.Width - this.Width;
                    this.Top = rect.Y - this.Height;
                }
                else {
                    // left
                    this.Left = rect.Width;
                    this.Top = rect.Height - this.Height;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            while(statusElements.Count > 0) {
                statusElements[0].Stop();
            }

            /*
            for (int i = 0; i < statusElements.Count; i++)
            {
                statusElements[i].Stop();
            }
             */
        }

        private int LayoutElements() {
            int y = 0;

            for(int i = 0; i < statusElements.Count; i++) {
                statusElements[i].Top = y;
                statusElements[i].BackColor = i % 2 == 0 ? Color.White : Color.Beige;
                y += ElementHeight;
            }

            return y;
        }

        private void RegisterElement(StatusWindowElement element) {
            StatusHost.Controls.Add(element);
            element.Width = StatusHost.Width;
            element.Anchor |= AnchorStyles.Right;
        }

        private void UnregisterElement(StatusWindowElement element) {
            if(StatusHost.Controls.Contains(element)) {
                StatusHost.Controls.Remove(element);
            }
        }

        private void UnregisterAllElements() {
            StatusHost.Controls.Clear();
            statusElements.Clear();
        }

        public void SetTasks(List<BackgoundTask> tasks) {
            UnregisterAllElements();

            foreach(BackgoundTask task in tasks) {
                StatusWindowElement element = new StatusWindowElement();

                element.Task = task;
                element.OnStopClicked += OnTaskStopped;
                RegisterElement(element);

                statusElements.Add(element);
            }
        }

        private void HideWindow() {
            this.Visible = false;
        }

        private void OnTaskStopped(object sender, EventArgs e) {
            StatusWindowElement element = (StatusWindowElement)sender;

            UnregisterElement(element);
            statusElements.Remove(element);

            // hide window if 0 elements
            if(statusElements.Count == 0) {
                HideWindow();
            }
            else {
                LayoutElements();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if(this.Tag is MainForm) {
                MainForm form = (MainForm)this.Tag;

            }

            HideWindow();
        }
    }


    public class GlassHelper {
        [StructLayout(LayoutKind.Sequential)]
        public struct Margins {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;

            public Margins(int left, int right, int top, int bottom) {
                Left = left;
                Right = right;
                Top = top;
                Bottom = bottom;
            }
        }

        [DllImport("dwmapi.dll")]
        public static extern void DwmIsCompositionEnabled(ref bool pfEnabled);

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

        public static bool IsGlassEnalbed() {
            if(Environment.OSVersion.Version.Major < 6) {
                // not Vista+
                return false;
            }

            bool enabled = false;
            DwmIsCompositionEnabled(ref enabled);

            return enabled;
        }
    }
}
