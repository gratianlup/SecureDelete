namespace SecureDeleteWinForms
{
	partial class PowerShellObjects
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PowerShellObjects));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.ObjectList = new System.Windows.Forms.ListView();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.MemberList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.DescriptionBox = new System.Windows.Forms.RichTextBox();
			this.FilePathErrorLabel = new System.Windows.Forms.Label();
			this.CancelButton = new System.Windows.Forms.Button();
			this.SaveButton = new System.Windows.Forms.Button();
			this.panel3 = new System.Windows.Forms.Panel();
			this.ObjectLabel = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.ObjectList);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(631, 369);
			this.splitContainer1.SplitterDistance = 192;
			this.splitContainer1.SplitterWidth = 2;
			this.splitContainer1.TabIndex = 1;
			// 
			// ObjectList
			// 
			this.ObjectList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ObjectList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
			this.ObjectList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ObjectList.HideSelection = false;
			this.ObjectList.Location = new System.Drawing.Point(0, 0);
			this.ObjectList.Name = "ObjectList";
			this.ObjectList.Size = new System.Drawing.Size(192, 369);
			this.ObjectList.SmallImageList = this.imageList1;
			this.ObjectList.TabIndex = 0;
			this.ObjectList.UseCompatibleStateImageBehavior = false;
			this.ObjectList.View = System.Windows.Forms.View.Details;
			this.ObjectList.SelectedIndexChanged += new System.EventHandler(this.ObjectList_SelectedIndexChanged);
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Object";
			this.columnHeader3.Width = 150;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "logical.ico");
			this.imageList1.Images.SetKeyName(1, "variable.ico");
			this.imageList1.Images.SetKeyName(2, "class.ico");
			// 
			// splitContainer2
			// 
			this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.splitContainer2.Panel1.Controls.Add(this.MemberList);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.DescriptionBox);
			this.splitContainer2.Size = new System.Drawing.Size(437, 369);
			this.splitContainer2.SplitterDistance = 184;
			this.splitContainer2.SplitterWidth = 2;
			this.splitContainer2.TabIndex = 0;
			// 
			// MemberList
			// 
			this.MemberList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.MemberList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.MemberList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MemberList.FullRowSelect = true;
			this.MemberList.HideSelection = false;
			this.MemberList.Location = new System.Drawing.Point(0, 0);
			this.MemberList.Name = "MemberList";
			this.MemberList.Size = new System.Drawing.Size(437, 184);
			this.MemberList.SmallImageList = this.imageList1;
			this.MemberList.TabIndex = 1;
			this.MemberList.UseCompatibleStateImageBehavior = false;
			this.MemberList.View = System.Windows.Forms.View.Details;
			this.MemberList.SelectedIndexChanged += new System.EventHandler(this.MemberList_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 200;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Type";
			this.columnHeader2.Width = 100;
			// 
			// DescriptionBox
			// 
			this.DescriptionBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.DescriptionBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DescriptionBox.Location = new System.Drawing.Point(0, 0);
			this.DescriptionBox.Name = "DescriptionBox";
			this.DescriptionBox.ReadOnly = true;
			this.DescriptionBox.Size = new System.Drawing.Size(437, 183);
			this.DescriptionBox.TabIndex = 0;
			this.DescriptionBox.Text = "";
			// 
			// FilePathErrorLabel
			// 
			this.FilePathErrorLabel.AutoSize = true;
			this.FilePathErrorLabel.ForeColor = System.Drawing.Color.White;
			this.FilePathErrorLabel.Location = new System.Drawing.Point(9, 9);
			this.FilePathErrorLabel.Name = "FilePathErrorLabel";
			this.FilePathErrorLabel.Size = new System.Drawing.Size(78, 13);
			this.FilePathErrorLabel.TabIndex = 5;
			this.FilePathErrorLabel.Text = "Invalid file path";
			this.FilePathErrorLabel.Visible = false;
			// 
			// CancelButton
			// 
			this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelButton.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelButton.Location = new System.Drawing.Point(496, 3);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(76, 24);
			this.CancelButton.TabIndex = 4;
			this.CancelButton.Text = "Close";
			this.CancelButton.UseVisualStyleBackColor = false;
			// 
			// SaveButton
			// 
			this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SaveButton.BackColor = System.Drawing.SystemColors.Control;
			this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SaveButton.Image = global::SecureDeleteWinForms.Properties.Resources.save;
			this.SaveButton.Location = new System.Drawing.Point(388, 3);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(107, 24);
			this.SaveButton.TabIndex = 3;
			this.SaveButton.Text = "Save changes";
			this.SaveButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.SaveButton.UseVisualStyleBackColor = false;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.ObjectLabel);
			this.panel3.Controls.Add(this.button1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 369);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(631, 30);
			this.panel3.TabIndex = 5;
			// 
			// ObjectLabel
			// 
			this.ObjectLabel.AutoSize = true;
			this.ObjectLabel.ForeColor = System.Drawing.Color.White;
			this.ObjectLabel.Location = new System.Drawing.Point(5, 9);
			this.ObjectLabel.Name = "ObjectLabel";
			this.ObjectLabel.Size = new System.Drawing.Size(55, 13);
			this.ObjectLabel.TabIndex = 5;
			this.ObjectLabel.Text = "Objects: 0";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.BackColor = System.Drawing.SystemColors.Control;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(552, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(76, 24);
			this.button1.TabIndex = 4;
			this.button1.Text = "Close";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// PowerShellObjects
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(631, 399);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.panel3);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PowerShellObjects";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Exposed Objects";
			this.Load += new System.EventHandler(this.PowerShellObjects_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PowerShellObjects_FormClosing);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.ListView MemberList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ListView ObjectList;
		private System.Windows.Forms.RichTextBox DescriptionBox;
		private System.Windows.Forms.Label FilePathErrorLabel;
		private System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Label ObjectLabel;
		private System.Windows.Forms.ColumnHeader columnHeader3;

	}
}