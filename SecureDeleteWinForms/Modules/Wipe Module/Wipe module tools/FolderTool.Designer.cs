namespace SecureDeleteWinForms.WipeTools
{
	partial class FolderTool
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FolderTool));
			this.BrowseButton = new System.Windows.Forms.Button();
			this.FolderTextbox = new System.Windows.Forms.TextBox();
			this.CancelButton = new System.Windows.Forms.Button();
			this.InsertButton = new System.Windows.Forms.Button();
			this.PathLabel = new System.Windows.Forms.Label();
			this.FilterCheckbox = new System.Windows.Forms.CheckBox();
			this.SubfoldersCheckbox = new System.Windows.Forms.CheckBox();
			this.PatternTextbox = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.MethodNameLabel = new System.Windows.Forms.Label();
			this.MethodChangeButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.DeleteCheckbox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.ErrorTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.RegexPicturebox = new System.Windows.Forms.PictureBox();
			this.FilterBox = new SecureDeleteWinForms.WipeTools.FilterBox();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RegexPicturebox)).BeginInit();
			this.SuspendLayout();
			// 
			// BrowseButton
			// 
			this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseButton.Image = global::SecureDeleteWinForms.Properties.Resources.openHS;
			this.BrowseButton.Location = new System.Drawing.Point(616, 4);
			this.BrowseButton.Name = "BrowseButton";
			this.BrowseButton.Size = new System.Drawing.Size(82, 24);
			this.BrowseButton.TabIndex = 1;
			this.BrowseButton.Text = "Browse";
			this.BrowseButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.BrowseButton.UseVisualStyleBackColor = true;
			this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
			// 
			// FolderTextbox
			// 
			this.FolderTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.FolderTextbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.FolderTextbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
			this.FolderTextbox.Location = new System.Drawing.Point(3, 6);
			this.FolderTextbox.Name = "FolderTextbox";
			this.FolderTextbox.Size = new System.Drawing.Size(610, 20);
			this.FolderTextbox.TabIndex = 0;
			// 
			// CancelButton
			// 
			this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelButton.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelButton.Location = new System.Drawing.Point(695, 3);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(76, 24);
			this.CancelButton.TabIndex = 4;
			this.CancelButton.Text = "Close";
			this.CancelButton.UseVisualStyleBackColor = true;
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// InsertButton
			// 
			this.InsertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.InsertButton.BackColor = System.Drawing.SystemColors.Control;
			this.InsertButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.InsertButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.InsertButton.Location = new System.Drawing.Point(609, 3);
			this.InsertButton.Name = "InsertButton";
			this.InsertButton.Size = new System.Drawing.Size(84, 24);
			this.InsertButton.TabIndex = 3;
			this.InsertButton.Text = "Insert";
			this.InsertButton.UseVisualStyleBackColor = true;
			this.InsertButton.Click += new System.EventHandler(this.InsertButton_Click);
			// 
			// PathLabel
			// 
			this.PathLabel.AutoSize = true;
			this.PathLabel.Location = new System.Drawing.Point(3, 13);
			this.PathLabel.Name = "PathLabel";
			this.PathLabel.Size = new System.Drawing.Size(61, 13);
			this.PathLabel.TabIndex = 4;
			this.PathLabel.Text = "Folder Path";
			// 
			// FilterCheckbox
			// 
			this.FilterCheckbox.AutoSize = true;
			this.FilterCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.FilterCheckbox.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.FilterCheckbox.Location = new System.Drawing.Point(6, 104);
			this.FilterCheckbox.Name = "FilterCheckbox";
			this.FilterCheckbox.Size = new System.Drawing.Size(108, 18);
			this.FilterCheckbox.TabIndex = 9;
			this.FilterCheckbox.Text = "Enable file filters";
			this.FilterCheckbox.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.FilterCheckbox.UseVisualStyleBackColor = true;
			this.FilterCheckbox.CheckedChanged += new System.EventHandler(this.FilterCheckbox_CheckedChanged);
			// 
			// SubfoldersCheckbox
			// 
			this.SubfoldersCheckbox.AutoSize = true;
			this.SubfoldersCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SubfoldersCheckbox.Location = new System.Drawing.Point(6, 67);
			this.SubfoldersCheckbox.Name = "SubfoldersCheckbox";
			this.SubfoldersCheckbox.Size = new System.Drawing.Size(118, 18);
			this.SubfoldersCheckbox.TabIndex = 8;
			this.SubfoldersCheckbox.Text = "Include subfolders";
			this.SubfoldersCheckbox.UseVisualStyleBackColor = true;
			// 
			// PatternTextbox
			// 
			this.PatternTextbox.Location = new System.Drawing.Point(76, 35);
			this.PatternTextbox.Name = "PatternTextbox";
			this.PatternTextbox.Size = new System.Drawing.Size(262, 20);
			this.PatternTextbox.TabIndex = 7;
			this.PatternTextbox.TextChanged += new System.EventHandler(this.PatternTextbox_TextChanged);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.RegexPicturebox);
			this.panel1.Controls.Add(this.checkBox1);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.DeleteCheckbox);
			this.panel1.Controls.Add(this.FilterBox);
			this.panel1.Controls.Add(this.FilterCheckbox);
			this.panel1.Controls.Add(this.SubfoldersCheckbox);
			this.panel1.Controls.Add(this.PatternTextbox);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.panel4);
			this.panel1.Controls.Add(this.PathLabel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(777, 300);
			this.panel1.TabIndex = 9;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(344, 37);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(153, 17);
			this.checkBox1.TabIndex = 14;
			this.checkBox1.Text = "Treat as regular expression";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Controls.Add(this.button1);
			this.panel2.Controls.Add(this.MethodNameLabel);
			this.panel2.Controls.Add(this.MethodChangeButton);
			this.panel2.Location = new System.Drawing.Point(76, 242);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(692, 52);
			this.panel2.TabIndex = 13;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(99, 25);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 24);
			this.button1.TabIndex = 4;
			this.button1.Text = "Default";
			this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// MethodNameLabel
			// 
			this.MethodNameLabel.AutoSize = true;
			this.MethodNameLabel.Location = new System.Drawing.Point(2, 5);
			this.MethodNameLabel.Name = "MethodNameLabel";
			this.MethodNameLabel.Size = new System.Drawing.Size(72, 13);
			this.MethodNameLabel.TabIndex = 3;
			this.MethodNameLabel.Text = "Method name";
			// 
			// MethodChangeButton
			// 
			this.MethodChangeButton.Image = global::SecureDeleteWinForms.Properties.Resources.Untitled;
			this.MethodChangeButton.Location = new System.Drawing.Point(5, 25);
			this.MethodChangeButton.Name = "MethodChangeButton";
			this.MethodChangeButton.Size = new System.Drawing.Size(88, 24);
			this.MethodChangeButton.TabIndex = 2;
			this.MethodChangeButton.Text = " Change";
			this.MethodChangeButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.MethodChangeButton.UseVisualStyleBackColor = true;
			this.MethodChangeButton.Click += new System.EventHandler(this.MethodChangeButton_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 247);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "Wipe method";
			// 
			// DeleteCheckbox
			// 
			this.DeleteCheckbox.AutoSize = true;
			this.DeleteCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.DeleteCheckbox.Location = new System.Drawing.Point(6, 85);
			this.DeleteCheckbox.Name = "DeleteCheckbox";
			this.DeleteCheckbox.Size = new System.Drawing.Size(128, 18);
			this.DeleteCheckbox.TabIndex = 11;
			this.DeleteCheckbox.Text = "Delete empty folders";
			this.DeleteCheckbox.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 38);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Pattern";
			// 
			// panel4
			// 
			this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel4.Controls.Add(this.BrowseButton);
			this.panel4.Controls.Add(this.FolderTextbox);
			this.panel4.Location = new System.Drawing.Point(73, 3);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(701, 31);
			this.panel4.TabIndex = 5;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.CancelButton);
			this.panel3.Controls.Add(this.InsertButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 300);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(777, 30);
			this.panel3.TabIndex = 8;
			// 
			// ErrorTooltip
			// 
			this.ErrorTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
			this.ErrorTooltip.ToolTipTitle = "Error";
			// 
			// RegexPicturebox
			// 
			this.RegexPicturebox.Image = global::SecureDeleteWinForms.Properties.Resources.warning1;
			this.RegexPicturebox.Location = new System.Drawing.Point(503, 37);
			this.RegexPicturebox.Name = "RegexPicturebox";
			this.RegexPicturebox.Size = new System.Drawing.Size(16, 16);
			this.RegexPicturebox.TabIndex = 15;
			this.RegexPicturebox.TabStop = false;
			this.ErrorTooltip.SetToolTip(this.RegexPicturebox, "Invalid regular expression entered");
			this.RegexPicturebox.Visible = false;
			// 
			// FilterBox
			// 
			this.FilterBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.FilterBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.FilterBox.FileFilter = ((SecureDelete.FileSearch.FileFilter)(resources.GetObject("FilterBox.FileFilter")));
			this.FilterBox.FilterEnabled = false;
			this.FilterBox.Location = new System.Drawing.Point(3, 128);
			this.FilterBox.Name = "FilterBox";
			this.FilterBox.Options = null;
			this.FilterBox.Size = new System.Drawing.Size(765, 108);
			this.FilterBox.TabIndex = 10;
			// 
			// FolderTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel3);
			this.Name = "FolderTool";
			this.Size = new System.Drawing.Size(777, 330);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.RegexPicturebox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BrowseButton;
		private System.Windows.Forms.TextBox FolderTextbox;
		private System.Windows.Forms.Button CancelButton;
		private SecureDeleteWinForms.WipeTools.FilterBox FilterBox;
		private System.Windows.Forms.Button InsertButton;
		private System.Windows.Forms.Label PathLabel;
		private System.Windows.Forms.CheckBox FilterCheckbox;
		private System.Windows.Forms.CheckBox SubfoldersCheckbox;
		private System.Windows.Forms.TextBox PatternTextbox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.CheckBox DeleteCheckbox;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label MethodNameLabel;
		private System.Windows.Forms.Button MethodChangeButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ToolTip ErrorTooltip;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.PictureBox RegexPicturebox;
	}
}
