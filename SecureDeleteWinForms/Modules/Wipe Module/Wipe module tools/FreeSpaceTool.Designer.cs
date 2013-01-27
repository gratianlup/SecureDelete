namespace SecureDeleteWinForms.WipeTools
{
	partial class FreeSpaceTool
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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("C:\\");
			this.VolumeListView = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.VolumeImages = new System.Windows.Forms.ImageList(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.button2 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.AdvancedButton = new System.Windows.Forms.Button();
			this.MFTCheckbox = new System.Windows.Forms.CheckBox();
			this.ClusterTipsCheckbox = new System.Windows.Forms.CheckBox();
			this.FreeSpaceCheckbox = new System.Windows.Forms.CheckBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.button3 = new System.Windows.Forms.Button();
			this.MethodNameLabel = new System.Windows.Forms.Label();
			this.MethodChangeButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.CancelButton = new System.Windows.Forms.Button();
			this.InsertButton = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// VolumeListView
			// 
			this.VolumeListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.VolumeListView.CheckBoxes = true;
			this.VolumeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
			this.VolumeListView.FullRowSelect = true;
			listViewItem1.StateImageIndex = 0;
			this.VolumeListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.VolumeListView.Location = new System.Drawing.Point(5, 31);
			this.VolumeListView.Name = "VolumeListView";
			this.VolumeListView.Size = new System.Drawing.Size(687, 222);
			this.VolumeListView.SmallImageList = this.VolumeImages;
			this.VolumeListView.TabIndex = 0;
			this.VolumeListView.UseCompatibleStateImageBehavior = false;
			this.VolumeListView.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Letter";
			this.columnHeader1.Width = 80;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Label";
			this.columnHeader2.Width = 160;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Total Space";
			this.columnHeader3.Width = 100;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Free Space";
			this.columnHeader4.Width = 100;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "% Free";
			this.columnHeader5.Width = 70;
			// 
			// VolumeImages
			// 
			this.VolumeImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.VolumeImages.ImageSize = new System.Drawing.Size(16, 16);
			this.VolumeImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.VolumeListView);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(698, 431);
			this.panel1.TabIndex = 7;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.BackColor = System.Drawing.SystemColors.Control;
			this.button2.Image = global::SecureDeleteWinForms.Properties.Resources.CheckBoxHS;
			this.button2.Location = new System.Drawing.Point(608, 5);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(84, 24);
			this.button2.TabIndex = 21;
			this.button2.Text = "Select All";
			this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.AdvancedButton);
			this.groupBox1.Controls.Add(this.MFTCheckbox);
			this.groupBox1.Controls.Add(this.ClusterTipsCheckbox);
			this.groupBox1.Controls.Add(this.FreeSpaceCheckbox);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.groupBox1.Location = new System.Drawing.Point(5, 259);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(687, 108);
			this.groupBox1.TabIndex = 20;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Wipe Options";
			// 
			// AdvancedButton
			// 
			this.AdvancedButton.Image = global::SecureDeleteWinForms.Properties.Resources.OptionsHS;
			this.AdvancedButton.Location = new System.Drawing.Point(4, 79);
			this.AdvancedButton.Name = "AdvancedButton";
			this.AdvancedButton.Size = new System.Drawing.Size(128, 23);
			this.AdvancedButton.TabIndex = 23;
			this.AdvancedButton.Text = "Advanced Options";
			this.AdvancedButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.AdvancedButton.UseVisualStyleBackColor = true;
			this.AdvancedButton.Click += new System.EventHandler(this.AdvancedButton_Click);
			// 
			// MFTCheckbox
			// 
			this.MFTCheckbox.AutoSize = true;
			this.MFTCheckbox.Location = new System.Drawing.Point(6, 59);
			this.MFTCheckbox.Name = "MFTCheckbox";
			this.MFTCheckbox.Size = new System.Drawing.Size(76, 17);
			this.MFTCheckbox.TabIndex = 22;
			this.MFTCheckbox.Text = "Wipe MFT";
			this.MFTCheckbox.UseVisualStyleBackColor = true;
			// 
			// ClusterTipsCheckbox
			// 
			this.ClusterTipsCheckbox.AutoSize = true;
			this.ClusterTipsCheckbox.Location = new System.Drawing.Point(6, 39);
			this.ClusterTipsCheckbox.Name = "ClusterTipsCheckbox";
			this.ClusterTipsCheckbox.Size = new System.Drawing.Size(109, 17);
			this.ClusterTipsCheckbox.TabIndex = 21;
			this.ClusterTipsCheckbox.Text = "Wipe Cluster Tips";
			this.ClusterTipsCheckbox.UseVisualStyleBackColor = true;
			// 
			// FreeSpaceCheckbox
			// 
			this.FreeSpaceCheckbox.AutoSize = true;
			this.FreeSpaceCheckbox.Location = new System.Drawing.Point(6, 19);
			this.FreeSpaceCheckbox.Name = "FreeSpaceCheckbox";
			this.FreeSpaceCheckbox.Size = new System.Drawing.Size(109, 17);
			this.FreeSpaceCheckbox.TabIndex = 20;
			this.FreeSpaceCheckbox.Text = "Wipe Free Space";
			this.FreeSpaceCheckbox.UseVisualStyleBackColor = true;
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Controls.Add(this.button3);
			this.panel2.Controls.Add(this.MethodNameLabel);
			this.panel2.Controls.Add(this.MethodChangeButton);
			this.panel2.Location = new System.Drawing.Point(76, 373);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(616, 52);
			this.panel2.TabIndex = 15;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(99, 25);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(88, 24);
			this.button3.TabIndex = 6;
			this.button3.Text = "Default";
			this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// MethodNameLabel
			// 
			this.MethodNameLabel.AutoSize = true;
			this.MethodNameLabel.Location = new System.Drawing.Point(4, 5);
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
			this.label2.Location = new System.Drawing.Point(3, 378);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 13);
			this.label2.TabIndex = 14;
			this.label2.Text = "Wipe method";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Volumes to wipe";
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.CancelButton);
			this.panel3.Controls.Add(this.InsertButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 431);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(698, 30);
			this.panel3.TabIndex = 6;
			// 
			// CancelButton
			// 
			this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelButton.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelButton.Location = new System.Drawing.Point(616, 3);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(76, 24);
			this.CancelButton.TabIndex = 4;
			this.CancelButton.Text = "Close";
			this.CancelButton.UseVisualStyleBackColor = false;
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// InsertButton
			// 
			this.InsertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.InsertButton.BackColor = System.Drawing.SystemColors.Control;
			this.InsertButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.InsertButton.Location = new System.Drawing.Point(492, 3);
			this.InsertButton.Name = "InsertButton";
			this.InsertButton.Size = new System.Drawing.Size(123, 24);
			this.InsertButton.TabIndex = 3;
			this.InsertButton.Text = "Add Free Space";
			this.InsertButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.InsertButton.UseVisualStyleBackColor = false;
			this.InsertButton.Click += new System.EventHandler(this.InsertButton_Click);
			// 
			// FreeSpaceTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel3);
			this.Name = "FreeSpaceTool";
			this.Size = new System.Drawing.Size(698, 461);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView VolumeListView;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ImageList VolumeImages;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Button InsertButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label MethodNameLabel;
		private System.Windows.Forms.Button MethodChangeButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button AdvancedButton;
		private System.Windows.Forms.CheckBox MFTCheckbox;
		private System.Windows.Forms.CheckBox ClusterTipsCheckbox;
		private System.Windows.Forms.CheckBox FreeSpaceCheckbox;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;

	}
}
