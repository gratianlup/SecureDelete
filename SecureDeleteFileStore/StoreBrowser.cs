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
using System.IO;
using System.Security.Cryptography;

namespace FileStore {
    public partial class StoreBrowser : Form {
        FileStore store;
        TreeNode selectedFolder;
        TreeNode lastNode;
        string storePath;

        public FileStore Store {
            get { return store; }
            set {
                store = value;
                treeView1.Nodes.Clear();

                if(store != null) {
                    BuildTreeList(null, null);
                }
            }
        }

        public StoreBrowser() {
            InitializeComponent();
        }

        private void LoadStore(string file) {
            store = new FileStore();
            store.Load(file);
            storePath = file;

            if(store.Encrypt) {
                // show password window
                Password dialog = new Password();

                if(dialog.ShowDialog() == DialogResult.OK) {
                    SHA256Managed passwordHash = new SHA256Managed();
                    store.EncryptionKey = passwordHash.ComputeHash(Encoding.ASCII.GetBytes(dialog.textBox1.Text));
                    store.Load(file);
                }
            }

            treeView1.Nodes.Clear();
            BuildTreeList(null, null);
        }

        private string GetFolderPath(TreeNode node) {
            if(node.Parent == null) {
                return "";
            }
            else {
                return GetFolderPath(node.Parent) + "\\" + node.Text;
            }
        }

        private void BuildTreeList(TreeNode parent, string path) {
            if(parent == null) {
                parent = treeView1.Nodes.Add("root");
                parent.Tag = "";
                path = "";
            }

            string[] folders = store.GetSubfolders(path);

            if(folders != null) {
                foreach(string s in folders) {
                    // add
                    TreeNode child = new TreeNode(s);
                    parent.Nodes.Add(child);
                    child.Tag = path + "\\" + s;
                    BuildTreeList(child, path + "\\" + s);
                }
            }

            treeView1.SelectedNode = parent;
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            if(openFileDialog1.ShowDialog() == DialogResult.OK) {
                LoadStore(openFileDialog1.FileName);
                treeView1.ExpandAll();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
            string path = GetFolderPath(e.Node);
            toolStripStatusLabel2.Text = "Folder: " + path;
            selectedFolder = e.Node;

            LoadFiles(path);
        }

        private void LoadFiles(string path) {
            StoreFile[] files = store.GetFolderFilesEx(path);
            listView1.Items.Clear();

            if(files != null) {
                foreach(StoreFile f in files) {
                    // comment
                    ListViewItem item = new ListViewItem();

                    item.Text = f.Name;
                    item.SubItems.Add(f.Size.ToString());
                    item.SubItems.Add(f.RealSize.ToString());
                    item.SubItems.Add((f.StoreMode & StoreMode.Encrypted) == StoreMode.Encrypted ? "True" : "False");
                    item.SubItems.Add((f.StoreMode & StoreMode.Compressed) == StoreMode.Compressed ? "True" : "False");
                    item.ImageIndex = 0;
                    item.Tag = f;

                    listView1.Items.Add(item);
                }
            }

            toolStripStatusLabel1.Text = "Files: " + listView1.Items.Count.ToString();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
            if(listView1.SelectedItems.Count == 0) {
                return;
            }

            if(toolStripButton4.Checked == false) {
                return;
            }

            try {
                StoreFile file = (StoreFile)listView1.SelectedItems[0].Tag;
                byte[] data = store.ReadFile(file);

                if(data == null) {
                    textBox1.Text = "File empty!";
                    return;
                }

                int remaining = data.Length;
                int position = 0;
                StringBuilder builder = new StringBuilder();

                while(remaining > 0) {
                    int length = Math.Min(remaining, 16);
                    builder.AppendFormat("{0:X8}: ", position);
                    builder.Append(BitConverter.ToString(data, position, length));
                    builder.Append(" | ");

                    for(int i = 0; i < length; i++) {
                        byte value = data[position + i];
                        if(value != 0 && value != (byte)('\n')) {
                            builder.Append((char)value);
                        }
                        else {
                            builder.Append(' ');
                        }
                    }

                    builder.AppendLine();
                    position += length;
                    remaining -= length;
                }

                textBox1.Text = builder.ToString();
            }
            catch {
                textBox1.Text = "Error!\nFailed to read data.";
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            if(listView1.SelectedItems.Count > 0) {
                if(folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                    StoreFile file = (StoreFile)listView1.SelectedItems[0].Tag;
                    string path = Path.Combine(folderBrowserDialog1.SelectedPath, file.Name);

                    FileStream writer = File.Create(path);
                    writer.Write(store.ReadFile(file), 0, (int)file.RealSize);
                    writer.Close();
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            StoreInfo dialog = new StoreInfo();
            dialog.Store = store;

            dialog.ShowDialog();
            store = dialog.Store;
            treeView1.Nodes.Clear();
            BuildTreeList(null, null);
        }

        private void treeView1_MouseMove(object sender, MouseEventArgs e) {
            try {
                TreeNode node = treeView1.GetNodeAt(e.X, e.Y);

                if(node != null) {
                    if(lastNode != null && lastNode != node) {
                        lastNode.BackColor = Color.FromName("Window");
                    }

                    if(lastNode != node) {
                        lastNode = node;
                        string path = GetFolderPath(node);
                        int numFiles = store.GetFolderFiles(path).Length;
                        node.BackColor = Color.Gold;

                        toolStripStatusLabel3.Text = numFiles.ToString() + " files";
                    }
                }
                else {
                    toolStripStatusLabel3.Text = "";

                    if(lastNode != null) {
                        lastNode.BackColor = Color.FromName("Window");
                        lastNode = null;
                    }
                }
            }
            catch { }
        }

        private void toolStripButton4_Click(object sender, EventArgs e) {
            splitContainer2.Panel2Collapsed = !toolStripButton4.Checked;
        }

        private void toolStripButton5_Click(object sender, EventArgs e) {
            SearchBox.Text = "";

        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e) {

        }

        private void SearchBox_TextChanged(object sender, EventArgs e) {
            Queue<StoreFolder> folders = new Queue<StoreFolder>();
            folders.Enqueue(store.Root);
            string text = SearchBox.Text.Trim();
            listView1.Items.Clear();

            if(text.Length == 0) {
                if(selectedFolder != null) {
                    LoadFiles(selectedFolder.Tag as string);
                }

                return;
            }

            while(folders.Count > 0) {
                StoreFolder folder = folders.Dequeue();

                foreach(KeyValuePair<string, Guid> kvp in folder.Files) {
                    if(kvp.Key.Contains(text)) {
                        ListViewItem item = new ListViewItem();
                        StoreFile f = store.files[kvp.Value];

                        item.Text = f.Name;
                        item.SubItems.Add(f.Size.ToString());
                        item.SubItems.Add(f.RealSize.ToString());
                        item.SubItems.Add((f.StoreMode & StoreMode.Encrypted) == StoreMode.Encrypted ? "True" : "False");
                        item.SubItems.Add((f.StoreMode & StoreMode.Compressed) == StoreMode.Compressed ? "True" : "False");
                        item.ImageIndex = 0;
                        item.Tag = f;

                        listView1.Items.Add(item);
                    }
                }

                foreach(KeyValuePair<string, StoreFolder> kvp in folder.Subfolders) {
                    folders.Enqueue(kvp.Value);
                }

                toolStripStatusLabel1.Text = "Files: " + listView1.Items.Count.ToString();
            }
        }

        private void treeView1_MouseLeave(object sender, EventArgs e) {
            if(lastNode != null) {
                lastNode.BackColor = Color.FromName("Window");
                lastNode = null;
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e) {
            foreach(ListViewItem item in listView1.SelectedItems) {
                //store.DeleteFile(
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e) {
            AddFolder(selectedFolder);
        }

        private void AddFolder(TreeNode parent) {
            if(parent == null) {
                return;
            }

            TreeNode node = new TreeNode();
            node.Text = "New Folder";
            node.Tag = parent.Tag + "\\" + "New Folder";
            store.CreateFolder(node.Tag as string);
            selectedFolder.Nodes.Add(node);
            selectedFolder.Expand();
            node.BeginEdit();
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
            if(e.Node.Parent == null) {
                // cannot change name of root folder
                return;
            }

            // rename (move) the folder
            string source = e.Node.Tag as string;
            string destination = e.Node.Parent.Tag as string + e.Label;

            store.MoveFolder(source, destination);
            e.Node.Tag = destination;
        }

        private void toolStripButton10_Click(object sender, EventArgs e) {
            if(openFileDialog1.ShowDialog() == DialogResult.OK) {
                AddFile(selectedFolder, openFileDialog1.FileName);
            }
        }

        private void AddFile(TreeNode selectedFolder, string path) {
            if(selectedFolder == null) {
                return;
            }

            if(File.Exists(path)) {
                byte[] data = File.ReadAllBytes(path);
                string storePath = selectedFolder.Tag as string;
                string fileName = new FileInfo(path).Name;

                StoreFile file = store.CreateFile(storePath + "\\" + fileName);
                store.WriteFile(file, data, store.Encrypt ? StoreMode.Encrypted : StoreMode.Normal);

                // reload folder
                LoadFiles(storePath);
            }
        }

        private void listView1_DragDrop(object sender, DragEventArgs e) {
            string[] files = e.Data.GetData(DataFormats.FileDrop, true) as string[];

            if(files != null) {
                foreach(string file in files) {
                    AddFile(selectedFolder, file);
                }
            }
        }

        private void listView1_DragEnter(object sender, DragEventArgs e) {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e) {
            if(selectedFolder != null && selectedFolder.Parent != null) {
                store.DeleteFolder(selectedFolder.Tag as string);
                selectedFolder.Remove();
            }
        }

        private void toolStripButton6_Click_1(object sender, EventArgs e) {
            while(listView1.SelectedItems.Count > 0) {
                string path = selectedFolder.Tag as string + "\\" + (listView1.SelectedItems[0].Tag as StoreFile).Name;
                store.DeleteFile(path);
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e) {
            if(storePath == null) {
                if(saveFileDialog1.ShowDialog() == DialogResult.OK) {
                    storePath = saveFileDialog1.FileName;
                }
            }

            if(storePath != null) {
                store.Save(storePath);
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            toolStripStatusLabel3.Text = "";

            if(store == null) {
                store = new FileStore();
                BuildTreeList(null, null);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            this.Opacity = 1;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            this.Opacity = 0.8;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e) {
            this.Opacity = 0.6;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e) {
            this.Opacity = 0.4;
        }
    }
}
