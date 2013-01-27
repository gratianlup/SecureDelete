namespace SecureDeleteWinForms.WipeTools
{
	partial class SearchTool
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchTool));
			this.panel1 = new System.Windows.Forms.Panel();
			this.FilterBox = new SecureDeleteWinForms.WipeTools.FilterBox();
			this.RegexCheckbox = new System.Windows.Forms.CheckBox();
			this.FilterCheckbox = new System.Windows.Forms.CheckBox();
			this.SubfoldersCheckbox = new System.Windows.Forms.CheckBox();
			this.PatternTextbox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.BrowseButton = new System.Windows.Forms.Button();
			this.FolderTextbox = new System.Windows.Forms.TextBox();
			this.PathLabel = new System.Windows.Forms.Label();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.picturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editPatternsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zeroOrMoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.oneOrMoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.beginningOfLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.endOfLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.beginningOfWordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.endOfWordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nLineBreakToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.anyOneCharactersInTheSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.orToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.escapeSpecialCharacterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tagExpressionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CancelButton = new System.Windows.Forms.Button();
			this.panel3 = new System.Windows.Forms.Panel();
			this.SearchButton = new System.Windows.Forms.Button();
			this.RegexPicturebox = new System.Windows.Forms.PictureBox();
			this.panel1.SuspendLayout();
			this.panel4.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RegexPicturebox)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.panel1.Controls.Add(this.RegexPicturebox);
			this.panel1.Controls.Add(this.FilterBox);
			this.panel1.Controls.Add(this.RegexCheckbox);
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
			this.panel1.TabIndex = 7;
			// 
			// FilterBox
			// 
			this.FilterBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.FilterBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.FilterBox.FileFilter = ((SecureDelete.FileSearch.FileFilter)(resources.GetObject("FilterBox.FileFilter")));
			this.FilterBox.FilterEnabled = false;
			this.FilterBox.Location = new System.Drawing.Point(6, 110);
			this.FilterBox.Name = "FilterBox";
			this.FilterBox.Options = null;
			this.FilterBox.Size = new System.Drawing.Size(765, 184);
			this.FilterBox.TabIndex = 16;
			// 
			// RegexCheckbox
			// 
			this.RegexCheckbox.AutoSize = true;
			this.RegexCheckbox.Location = new System.Drawing.Point(278, 37);
			this.RegexCheckbox.Name = "RegexCheckbox";
			this.RegexCheckbox.Size = new System.Drawing.Size(153, 17);
			this.RegexCheckbox.TabIndex = 15;
			this.RegexCheckbox.Text = "Treat as regular expression";
			this.RegexCheckbox.UseVisualStyleBackColor = true;
			this.RegexCheckbox.CheckedChanged += new System.EventHandler(this.RegexCheckbox_CheckedChanged);
			// 
			// FilterCheckbox
			// 
			this.FilterCheckbox.AutoSize = true;
			this.FilterCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.FilterCheckbox.Location = new System.Drawing.Point(6, 91);
			this.FilterCheckbox.Name = "FilterCheckbox";
			this.FilterCheckbox.Size = new System.Drawing.Size(108, 18);
			this.FilterCheckbox.TabIndex = 9;
			this.FilterCheckbox.Text = "Enable file filters";
			this.FilterCheckbox.UseVisualStyleBackColor = true;
			this.FilterCheckbox.CheckedChanged += new System.EventHandler(this.FilterCheckbox_CheckedChanged);
			// 
			// SubfoldersCheckbox
			// 
			this.SubfoldersCheckbox.AutoSize = true;
			this.SubfoldersCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SubfoldersCheckbox.Location = new System.Drawing.Point(6, 70);
			this.SubfoldersCheckbox.Name = "SubfoldersCheckbox";
			this.SubfoldersCheckbox.Size = new System.Drawing.Size(128, 18);
			this.SubfoldersCheckbox.TabIndex = 8;
			this.SubfoldersCheckbox.Text = "Search in subfolders";
			this.SubfoldersCheckbox.UseVisualStyleBackColor = true;
			// 
			// PatternTextbox
			// 
			this.PatternTextbox.Location = new System.Drawing.Point(73, 35);
			this.PatternTextbox.Name = "PatternTextbox";
			this.PatternTextbox.Size = new System.Drawing.Size(199, 20);
			this.PatternTextbox.TabIndex = 7;
			this.PatternTextbox.TextChanged += new System.EventHandler(this.PatternTextbox_TextChanged);
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
			this.panel4.Location = new System.Drawing.Point(70, 3);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(704, 31);
			this.panel4.TabIndex = 5;
			// 
			// BrowseButton
			// 
			this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseButton.Image = global::SecureDeleteWinForms.Properties.Resources.openHS;
			this.BrowseButton.Location = new System.Drawing.Point(625, 4);
			this.BrowseButton.Name = "BrowseButton";
			this.BrowseButton.Size = new System.Drawing.Size(76, 23);
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
			this.FolderTextbox.Size = new System.Drawing.Size(618, 20);
			this.FolderTextbox.TabIndex = 0;
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
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.picturesToolStripMenuItem,
            this.editPatternsToolStripMenuItem,
            this.zeroOrMoreToolStripMenuItem,
            this.oneOrMoreToolStripMenuItem,
            this.toolStripSeparator1,
            this.beginningOfLineToolStripMenuItem,
            this.endOfLineToolStripMenuItem,
            this.beginningOfWordToolStripMenuItem,
            this.endOfWordToolStripMenuItem,
            this.nLineBreakToolStripMenuItem,
            this.toolStripSeparator2,
            this.anyOneCharactersInTheSetToolStripMenuItem,
            this.toolStripMenuItem2,
            this.orToolStripMenuItem,
            this.escapeSpecialCharacterToolStripMenuItem,
            this.tagExpressionToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(270, 324);
			// 
			// picturesToolStripMenuItem
			// 
			this.picturesToolStripMenuItem.Name = "picturesToolStripMenuItem";
			this.picturesToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.picturesToolStripMenuItem.Text = ". Any single character";
			// 
			// editPatternsToolStripMenuItem
			// 
			this.editPatternsToolStripMenuItem.Name = "editPatternsToolStripMenuItem";
			this.editPatternsToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.editPatternsToolStripMenuItem.Text = "Edit patterns";
			// 
			// zeroOrMoreToolStripMenuItem
			// 
			this.zeroOrMoreToolStripMenuItem.Name = "zeroOrMoreToolStripMenuItem";
			this.zeroOrMoreToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.zeroOrMoreToolStripMenuItem.Text = "* Zero or more";
			// 
			// oneOrMoreToolStripMenuItem
			// 
			this.oneOrMoreToolStripMenuItem.Name = "oneOrMoreToolStripMenuItem";
			this.oneOrMoreToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.oneOrMoreToolStripMenuItem.Text = "+ One or more";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(266, 6);
			// 
			// beginningOfLineToolStripMenuItem
			// 
			this.beginningOfLineToolStripMenuItem.Name = "beginningOfLineToolStripMenuItem";
			this.beginningOfLineToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.beginningOfLineToolStripMenuItem.Text = "^ Beginning of line";
			// 
			// endOfLineToolStripMenuItem
			// 
			this.endOfLineToolStripMenuItem.Name = "endOfLineToolStripMenuItem";
			this.endOfLineToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.endOfLineToolStripMenuItem.Text = "$ End of line";
			// 
			// beginningOfWordToolStripMenuItem
			// 
			this.beginningOfWordToolStripMenuItem.Name = "beginningOfWordToolStripMenuItem";
			this.beginningOfWordToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.beginningOfWordToolStripMenuItem.Text = "< Beginning of word";
			// 
			// endOfWordToolStripMenuItem
			// 
			this.endOfWordToolStripMenuItem.Name = "endOfWordToolStripMenuItem";
			this.endOfWordToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.endOfWordToolStripMenuItem.Text = "< End of word";
			// 
			// nLineBreakToolStripMenuItem
			// 
			this.nLineBreakToolStripMenuItem.Name = "nLineBreakToolStripMenuItem";
			this.nLineBreakToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.nLineBreakToolStripMenuItem.Text = "\\n Line break";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(266, 6);
			// 
			// anyOneCharactersInTheSetToolStripMenuItem
			// 
			this.anyOneCharactersInTheSetToolStripMenuItem.Name = "anyOneCharactersInTheSetToolStripMenuItem";
			this.anyOneCharactersInTheSetToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.anyOneCharactersInTheSetToolStripMenuItem.Text = "[ ] Any one characters in the set";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(269, 22);
			this.toolStripMenuItem2.Text = "[^ ] Any one characters not in the set";
			// 
			// orToolStripMenuItem
			// 
			this.orToolStripMenuItem.Name = "orToolStripMenuItem";
			this.orToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.orToolStripMenuItem.Text = "| Or";
			// 
			// escapeSpecialCharacterToolStripMenuItem
			// 
			this.escapeSpecialCharacterToolStripMenuItem.Name = "escapeSpecialCharacterToolStripMenuItem";
			this.escapeSpecialCharacterToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.escapeSpecialCharacterToolStripMenuItem.Text = "\\ Escape special character";
			// 
			// tagExpressionToolStripMenuItem
			// 
			this.tagExpressionToolStripMenuItem.Name = "tagExpressionToolStripMenuItem";
			this.tagExpressionToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.tagExpressionToolStripMenuItem.Text = "{ } Tag expression";
			// 
			// CancelButton
			// 
			this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelButton.BackColor = System.Drawing.Color.AliceBlue;
			this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelButton.Location = new System.Drawing.Point(695, 3);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(76, 24);
			this.CancelButton.TabIndex = 4;
			this.CancelButton.Text = "Close";
			this.CancelButton.UseVisualStyleBackColor = true;
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.CancelButton);
			this.panel3.Controls.Add(this.SearchButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 300);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(777, 30);
			this.panel3.TabIndex = 6;
			// 
			// SearchButton
			// 
			this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SearchButton.BackColor = System.Drawing.Color.AliceBlue;
			this.SearchButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SearchButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.SearchButton.Location = new System.Drawing.Point(609, 3);
			this.SearchButton.Name = "SearchButton";
			this.SearchButton.Size = new System.Drawing.Size(84, 24);
			this.SearchButton.TabIndex = 3;
			this.SearchButton.Text = "Search";
			this.SearchButton.UseVisualStyleBackColor = true;
			this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
			// 
			// RegexPicturebox
			// 
			this.RegexPicturebox.Image = global::SecureDeleteWinForms.Properties.Resources.warning1;
			this.RegexPicturebox.Location = new System.Drawing.Point(437, 37);
			this.RegexPicturebox.Name = "RegexPicturebox";
			this.RegexPicturebox.Size = new System.Drawing.Size(16, 16);
			this.RegexPicturebox.TabIndex = 17;
			this.RegexPicturebox.TabStop = false;
			this.RegexPicturebox.Visible = false;
			// 
			// SearchTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel3);
			this.Name = "SearchTool";
			this.Size = new System.Drawing.Size(777, 330);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.RegexPicturebox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Button BrowseButton;
		private System.Windows.Forms.TextBox FolderTextbox;
		private System.Windows.Forms.Label PathLabel;
		private System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button SearchButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox PatternTextbox;
		private System.Windows.Forms.CheckBox SubfoldersCheckbox;
		private System.Windows.Forms.CheckBox FilterCheckbox;
		private System.Windows.Forms.CheckBox RegexCheckbox;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem picturesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem editPatternsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zeroOrMoreToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem oneOrMoreToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem beginningOfLineToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem endOfLineToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem beginningOfWordToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem endOfWordToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nLineBreakToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem anyOneCharactersInTheSetToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem orToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem escapeSpecialCharacterToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tagExpressionToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private FilterBox FilterBox;
		private System.Windows.Forms.PictureBox RegexPicturebox;
	}
}
