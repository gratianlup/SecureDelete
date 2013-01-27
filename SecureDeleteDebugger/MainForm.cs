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
using System.IO;
using DebugUtils.Debugger;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using WindowsMessageListener;

namespace Debugger {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
        }

        bool showFile = true;
        bool showErrors = true;
        bool showWarnings = true;
        bool showOthers = true;
        bool sdConsole;
        bool sdFile;
        bool sdStudio;
        bool sdBreak;
        bool sdMessage;
        bool sdSound;
        bool updateMessages;
        bool useOfficeLook;
        bool onTop;
        String sdSoundFile;
        Color selectionBackColor;
        Color selectionForeColor;
        Color[] errorColors = new Color[3];
        int messages;

        private void refresh() {
            bar1.Width = status1.Width - label1.Width - toolStripDropDownButton1.Width - 20;
            textBox1.Width = toolStrip1.Width - toolStripButton8.Width - toolStripButton9.Width - 
                             toolStripButton1.Width - toolStripButton2.Width - toolStripButton3.Width - 
                             toolStripLabel1.Width - toolStripButton4.Width - toolStripButton5.Width - 
                             toolStripButton6.Width - toolStripButton7.Width - 38;
            splitContainer1.Panel2Collapsed = !showFile;
            checkBox1.Checked = sdConsole;
            checkBox2.Checked = sdFile;
            checkBox3.Checked = sdStudio;
            checkBox4.Checked = sdBreak;
            checkBox4.Enabled = checkBox3.Checked;
            checkBox5.Checked = sdMessage;
            checkBox6.Checked = sdSound;
            checkBox6.Enabled = checkBox5.Checked;
            checkBox7.Checked = updateMessages;
            button2.BackColor = selectionBackColor;
            button4.BackColor = selectionForeColor;
            textBox2.Enabled = checkBox6.Checked && checkBox6.Enabled;
            textBox2.Text = sdSoundFile;

            int mw = tabPage1.Width;
            if(tabPage2.Width > mw) mw = tabPage3.Width;
            if(tabPage3.Width > mw) mw = tabPage3.Width;

            int mh = tabPage1.Height;
            if(tabPage2.Height > mh) mh = tabPage3.Height;
            if(tabPage3.Height > mh) mh = tabPage3.Height;

            rich1.Width = mw - 6;
            rich1.Height = mh - 6;

            if(useOfficeLook == true) {
                toolStrip1.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                status1.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                contextMenuStrip1.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                contextMenuStrip2.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            }
            else {
                toolStrip1.RenderMode = ToolStripRenderMode.System;
                status1.RenderMode = ToolStripRenderMode.System;
                contextMenuStrip1.RenderMode = ToolStripRenderMode.System;
                contextMenuStrip2.RenderMode = ToolStripRenderMode.System;
            }

            alwaysOnTopToolStripMenuItem.Checked = onTop;
            this.TopMost = onTop;
        }

        private void AddMessage(DebugMessage message) {
            // filter
            if(message.Type == DebugMessageType.Error && !showErrors) {
                return;
            }
            else if(message.Type == DebugMessageType.Unknown && !showOthers) {
                return;
            }
            else if(message.Type == DebugMessageType.Warning && !showWarnings) {
                return;
            }

            string filterValue = textBox1.Text.Trim();

            if(filterValue.Length > 0 && !message.Message.Contains(filterValue)) {
                return;
            }

            // add
            ListViewItem item = new ListViewItem();
            int index = List.Items.Count + 1;
            item.Tag = message;
            item.Text = index.ToString();
            item.SubItems.Add(message.Time.ToLongTimeString());
            item.SubItems.Add(Path.GetFileName(message.BaseMethod.File));
            item.SubItems.Add(message.BaseMethod.DeclaringObject);
            item.SubItems.Add(message.BaseMethod.Method);
            item.SubItems.Add(message.BaseMethod.Line.ToString());
            item.SubItems.Add(message.Message);

            if(message.Type == DebugMessageType.Error) {
                item.BackColor = index % 2 == 0 ? Color.FromArgb(255, 237, 237) : 
                                                    Color.FromArgb(255, 222, 222);
                item.ImageIndex = 0;
            }
            else if(message.Type == DebugMessageType.Warning) {
                item.BackColor = index % 2 == 0 ? Color.FromArgb(255, 255, 237) : 
                                                    Color.FromArgb(255, 255, 222);
                item.ImageIndex = 1;
            }
            else {
                item.BackColor = index % 2 == 0 ? Color.FromArgb(237, 245, 255) : 
                                                    Color.FromArgb(222, 230, 255);
                item.ImageIndex = 2;
            }

            List.Items.Add(item);
        }

        private void InterpretFile(int lines) {
            int pos = 0, lineCount = 1, last = 0;
            StringReader sr = new StringReader(rich1.Text);
            String line;

            while((line = sr.ReadLine()) != null) {
                if(lineCount == lines) {
                    last = line.Length;
                    break;
                }

                pos = pos + line.Length + 1;
                lineCount++;
            }

            rich1.SelectionStart = pos;
            rich1.SelectionLength = last;
            rich1.SelectionBackColor = selectionBackColor;
            rich1.SelectionColor = selectionForeColor;
            rich1.DeselectAll();
        }

        private void Form1_Resize(object sender, EventArgs e) {
            refresh();
        }

        private void button1_Click(object sender, EventArgs e) {
            showFile = !showFile;
            refresh();
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            showErrors = !showErrors;
            LoadMessages();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            showWarnings = !showWarnings;
            LoadMessages();
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            showOthers = !showOthers;
            LoadMessages();
        }

        private void toolStripButton4_Click(object sender, EventArgs e) {
            textBox1.Text = "";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            sdConsole = checkBox1.Checked;
            refresh();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            sdFile = checkBox2.Checked;
            refresh();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e) {
            sdBreak = checkBox4.Checked;
            refresh();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e) {
            sdSound = checkBox6.Checked;
            refresh();
        }

        private void textBox2_TextChanged(object sender, EventArgs e) {
            sdSoundFile = textBox2.Text;
        }

        private void checkBox3_CheckedChanged_1(object sender, EventArgs e) {
            sdStudio = checkBox3.Checked;
            refresh();
        }

        private void toolStripButton6_Click_1(object sender, EventArgs e) {
            showFile = toolStripButton6.Checked;
            refresh();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
            if(this.WindowState == FormWindowState.Minimized) {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
                showSecureDeleteDebuggerToolStripMenuItem.Text = "Hide Window";
            }
            else {
                showSecureDeleteDebuggerToolStripMenuItem.Text = "Show Window";
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
            }
        }

        private void splitContainer1_SplitterMoving(object sender, SplitterCancelEventArgs e) {
            refresh();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e) {
            refresh();
        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e) {
            sdBreak = checkBox4.Checked;
            refresh();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void showSecureDeleteDebuggerToolStripMenuItem_Click(object sender, EventArgs e) {
            if(this.WindowState == FormWindowState.Minimized) {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
                showSecureDeleteDebuggerToolStripMenuItem.Text = "Hide Window";
            }
            else {
                showSecureDeleteDebuggerToolStripMenuItem.Text = "Show Window";
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
            }
        }

        private void checkBox5_CheckedChanged_1(object sender, EventArgs e) {
            sdMessage = checkBox5.Checked;
            refresh();
        }

        private void tabControl1_TabStopChanged(object sender, EventArgs e) {
            textBox1.Text = "";
        }

        private void tabControl1_VisibleChanged(object sender, EventArgs e) {
            textBox1.Text = "";
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e) {
            if(e.TabPageIndex == 1) {
                splitContainer1.SplitterDistance = this.Height / 4;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            this.Opacity = 1.0;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            this.Opacity = 0.75;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e) {
            this.Opacity = 0.50;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e) {
            this.Opacity = 0.25;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e) {
            this.Opacity = 0.85;
        }

        private void splitContainer1_DoubleClick(object sender, EventArgs e) {
            splitContainer1.SplitterDistance = this.Height / 4;
        }

        private void checkBox6_CheckedChanged_1(object sender, EventArgs e) {
            sdSound = checkBox6.Checked;
            refresh();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e) {
            updateMessages = checkBox7.Checked;
            refresh();
        }

        private void button2_Click_1(object sender, EventArgs e) {
            if(colorDialog1.ShowDialog() == DialogResult.OK) {
                selectionBackColor = colorDialog1.Color;
                refresh();
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            if(colorDialog1.ShowDialog() == DialogResult.OK) {
                selectionForeColor = colorDialog1.Color;
                refresh();
            }
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e) {
            onTop = alwaysOnTopToolStripMenuItem.Checked;
            refresh();
        }

        private void toolStripButton5_Click(object sender, EventArgs e) {
            if(openFileDialog1.ShowDialog() == DialogResult.OK) {
                Debug.DeserializeMessages(openFileDialog1.FileName);
                LoadMessages();
            }
        }

        private DebugMessage DeserializeMessage(byte[] data) {
            if(data == null) {
                return null;
            }

            MemoryStream stream = new MemoryStream(data);
            XmlSerializer serializer = new XmlSerializer(typeof(DebugMessage));

            try {
                return (DebugMessage)serializer.Deserialize(stream);
            }
            catch {
                return null;
            }
        }

        private void LoadMessages() {
            List.Items.Clear();

            if(Debug.DebugMessages == null) {
                return;
            }

            foreach(DebugMessage message in Debug.DebugMessages) {
                AddMessage(message);
            }
        }

        protected override void WndProc(ref Message m) {
            if(m.Msg == WMListener.WM_COPYDATA) {
                WMListener.CopyDataStruct data = new WMListener.CopyDataStruct();
                data = (WMListener.CopyDataStruct)m.GetLParam(typeof(WMListener.CopyDataStruct));

                if(data.Length == 0) {
                    List.Items.Clear();
                    Debug.DebugMessages = new List<DebugMessage>();
                }
                else {
                    byte[] serializedMessage = new byte[data.Length];
                    Marshal.Copy(data.Data, serializedMessage, 0, data.Length);

                    // deserialize the message from the debugging library
                    DebugMessage message = DeserializeMessage(serializedMessage);

                    if(message != null) {
                        if(Debug.DebugMessages == null) {
                            Debug.DebugMessages = new List<DebugMessage>();
                        }

                        Debug.DebugMessages.Add(message);
                        AddMessage(message);
                    }
                }
            }

            base.WndProc(ref m);
        }

        private void toolStripButton7_Click(object sender, EventArgs e) {
            Debug.GenerateHtmlReport();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            LoadMessages();
        }

        private void List_SelectedIndexChanged(object sender, EventArgs e) {
            if(List.SelectedItems.Count > 0) {
                if(List.SelectedItems[0].Tag != null) {
                    DebugMessage message = (DebugMessage)List.SelectedItems[0].Tag;

                    ShowStack(message);
                    if(File.Exists(message.BaseMethod.File)) {
                        rich1.LoadFile(message.BaseMethod.File, RichTextBoxStreamType.PlainText);
                        InterpretFile(message.BaseMethod.Line);
                    }
                    else {
                        rich1.Text = "Source file not found !";
                    }
                }
            }
        }

        private void ShowStack(DebugMessage message) {
            Stack.Items.Clear();

            if(message.HasStack) {
                foreach(StackSegment segment in message.StackSegments) {
                    ListViewItem item = new ListViewItem();
                    item.Text = segment.DeclaringObject;
                    item.SubItems.Add(segment.Method);

                    Stack.Items.Add(item);
                }
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e) {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK) {
                Debug.SerializeMessages(saveFileDialog1.FileName);
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e) {
            splitContainer3.Panel2Collapsed = !toolStripButton9.Checked;
        }
    }
}
