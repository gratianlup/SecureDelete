namespace SecureDeleteWinForms.Options
{
	partial class WipeOptionsEditor
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
			this.WipePanel = new System.Windows.Forms.Panel();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.WipeUnusedIndexRecordCheckbox = new System.Windows.Forms.CheckBox();
			this.WipeUsedIndexRecordCheckbox = new System.Windows.Forms.CheckBox();
			this.WipeUnusedFileRecordCheckbox = new System.Windows.Forms.CheckBox();
			this.WipeUsedFileRecordCheckbox = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.FreeSpaceMethodNameLabel = new System.Windows.Forms.Label();
			this.FreeSpaceChangeButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.DestroyFreeSpaceCheckbox = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.TotalDeleteCheckbox = new System.Windows.Forms.CheckBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.FileMethodNameLabel = new System.Windows.Forms.Label();
			this.MethodChangeButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.WipeFileNamesCheckbox = new System.Windows.Forms.CheckBox();
			this.WipeAdsCheckbox = new System.Windows.Forms.CheckBox();
			this.WipePanel.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// WipePanel
			// 
			this.WipePanel.BackColor = System.Drawing.SystemColors.Control;
			this.WipePanel.Controls.Add(this.groupBox3);
			this.WipePanel.Controls.Add(this.groupBox2);
			this.WipePanel.Controls.Add(this.groupBox1);
			this.WipePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WipePanel.Location = new System.Drawing.Point(0, 0);
			this.WipePanel.Name = "WipePanel";
			this.WipePanel.Size = new System.Drawing.Size(721, 413);
			this.WipePanel.TabIndex = 12;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.WipeUnusedIndexRecordCheckbox);
			this.groupBox3.Controls.Add(this.WipeUsedIndexRecordCheckbox);
			this.groupBox3.Controls.Add(this.WipeUnusedFileRecordCheckbox);
			this.groupBox3.Controls.Add(this.WipeUsedFileRecordCheckbox);
			this.groupBox3.Location = new System.Drawing.Point(12, 260);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(697, 121);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "MFT (only under NTFS file system)";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.AutoSize = true;
			this.label4.ForeColor = System.Drawing.Color.DimGray;
			this.label4.Location = new System.Drawing.Point(5, 101);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(320, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "These settings apply for wiping the free space and for Total Delete";
			// 
			// WipeUnusedIndexRecordCheckbox
			// 
			this.WipeUnusedIndexRecordCheckbox.AutoSize = true;
			this.WipeUnusedIndexRecordCheckbox.Location = new System.Drawing.Point(8, 76);
			this.WipeUnusedIndexRecordCheckbox.Name = "WipeUnusedIndexRecordCheckbox";
			this.WipeUnusedIndexRecordCheckbox.Size = new System.Drawing.Size(158, 17);
			this.WipeUnusedIndexRecordCheckbox.TabIndex = 3;
			this.WipeUnusedIndexRecordCheckbox.Text = "Wipe Unused Index Record";
			this.WipeUnusedIndexRecordCheckbox.UseVisualStyleBackColor = true;
			this.WipeUnusedIndexRecordCheckbox.CheckedChanged += new System.EventHandler(this.WipeUnusedIndexRecordCheckbox_CheckedChanged);
			// 
			// WipeUsedIndexRecordCheckbox
			// 
			this.WipeUsedIndexRecordCheckbox.AutoSize = true;
			this.WipeUsedIndexRecordCheckbox.Location = new System.Drawing.Point(8, 57);
			this.WipeUsedIndexRecordCheckbox.Name = "WipeUsedIndexRecordCheckbox";
			this.WipeUsedIndexRecordCheckbox.Size = new System.Drawing.Size(146, 17);
			this.WipeUsedIndexRecordCheckbox.TabIndex = 2;
			this.WipeUsedIndexRecordCheckbox.Text = "Wipe Used Index Record";
			this.WipeUsedIndexRecordCheckbox.UseVisualStyleBackColor = true;
			// 
			// WipeUnusedFileRecordCheckbox
			// 
			this.WipeUnusedFileRecordCheckbox.AutoSize = true;
			this.WipeUnusedFileRecordCheckbox.Location = new System.Drawing.Point(8, 38);
			this.WipeUnusedFileRecordCheckbox.Name = "WipeUnusedFileRecordCheckbox";
			this.WipeUnusedFileRecordCheckbox.Size = new System.Drawing.Size(148, 17);
			this.WipeUnusedFileRecordCheckbox.TabIndex = 1;
			this.WipeUnusedFileRecordCheckbox.Text = "Wipe Unused File Record";
			this.WipeUnusedFileRecordCheckbox.UseVisualStyleBackColor = true;
			// 
			// WipeUsedFileRecordCheckbox
			// 
			this.WipeUsedFileRecordCheckbox.AutoSize = true;
			this.WipeUsedFileRecordCheckbox.Location = new System.Drawing.Point(8, 19);
			this.WipeUsedFileRecordCheckbox.Name = "WipeUsedFileRecordCheckbox";
			this.WipeUsedFileRecordCheckbox.Size = new System.Drawing.Size(136, 17);
			this.WipeUsedFileRecordCheckbox.TabIndex = 0;
			this.WipeUsedFileRecordCheckbox.Text = "Wipe Used File Record";
			this.WipeUsedFileRecordCheckbox.UseVisualStyleBackColor = true;
			this.WipeUsedFileRecordCheckbox.CheckedChanged += new System.EventHandler(this.WipeUsedFileRecordCheckbox_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.panel4);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.DestroyFreeSpaceCheckbox);
			this.groupBox2.Location = new System.Drawing.Point(12, 156);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(697, 98);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Free Space";
			// 
			// panel4
			// 
			this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel4.Controls.Add(this.FreeSpaceMethodNameLabel);
			this.panel4.Controls.Add(this.FreeSpaceChangeButton);
			this.panel4.Location = new System.Drawing.Point(115, 42);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(576, 52);
			this.panel4.TabIndex = 14;
			// 
			// FreeSpaceMethodNameLabel
			// 
			this.FreeSpaceMethodNameLabel.AutoSize = true;
			this.FreeSpaceMethodNameLabel.Location = new System.Drawing.Point(2, 4);
			this.FreeSpaceMethodNameLabel.Name = "FreeSpaceMethodNameLabel";
			this.FreeSpaceMethodNameLabel.Size = new System.Drawing.Size(72, 13);
			this.FreeSpaceMethodNameLabel.TabIndex = 3;
			this.FreeSpaceMethodNameLabel.Text = "Method name";
			// 
			// FreeSpaceChangeButton
			// 
			this.FreeSpaceChangeButton.Image = global::SecureDeleteWinForms.Properties.Resources.Untitled;
			this.FreeSpaceChangeButton.Location = new System.Drawing.Point(5, 25);
			this.FreeSpaceChangeButton.Name = "FreeSpaceChangeButton";
			this.FreeSpaceChangeButton.Size = new System.Drawing.Size(88, 24);
			this.FreeSpaceChangeButton.TabIndex = 2;
			this.FreeSpaceChangeButton.Text = " Change";
			this.FreeSpaceChangeButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.FreeSpaceChangeButton.UseVisualStyleBackColor = true;
			this.FreeSpaceChangeButton.Click += new System.EventHandler(this.FreeSpaceChangeButton_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(5, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(111, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Default Wipe Method:";
			// 
			// DestroyFreeSpaceCheckbox
			// 
			this.DestroyFreeSpaceCheckbox.AutoSize = true;
			this.DestroyFreeSpaceCheckbox.Location = new System.Drawing.Point(8, 19);
			this.DestroyFreeSpaceCheckbox.Name = "DestroyFreeSpaceCheckbox";
			this.DestroyFreeSpaceCheckbox.Size = new System.Drawing.Size(152, 17);
			this.DestroyFreeSpaceCheckbox.TabIndex = 0;
			this.DestroyFreeSpaceCheckbox.Text = "Destroy Free Space Folder";
			this.DestroyFreeSpaceCheckbox.UseVisualStyleBackColor = true;
			this.DestroyFreeSpaceCheckbox.CheckedChanged += new System.EventHandler(this.DestroyFreeSpaceCheckbox_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.TotalDeleteCheckbox);
			this.groupBox1.Controls.Add(this.panel2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.WipeFileNamesCheckbox);
			this.groupBox1.Controls.Add(this.WipeAdsCheckbox);
			this.groupBox1.Location = new System.Drawing.Point(12, 9);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(697, 141);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "File";
			// 
			// TotalDeleteCheckbox
			// 
			this.TotalDeleteCheckbox.AutoSize = true;
			this.TotalDeleteCheckbox.Location = new System.Drawing.Point(8, 57);
			this.TotalDeleteCheckbox.Name = "TotalDeleteCheckbox";
			this.TotalDeleteCheckbox.Size = new System.Drawing.Size(84, 17);
			this.TotalDeleteCheckbox.TabIndex = 15;
			this.TotalDeleteCheckbox.Text = "Total Delete";
			this.TotalDeleteCheckbox.UseVisualStyleBackColor = true;
			this.TotalDeleteCheckbox.CheckedChanged += new System.EventHandler(this.TotalDeleteCheckbox_CheckedChanged);
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Controls.Add(this.FileMethodNameLabel);
			this.panel2.Controls.Add(this.MethodChangeButton);
			this.panel2.Location = new System.Drawing.Point(115, 83);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(576, 52);
			this.panel2.TabIndex = 14;
			// 
			// FileMethodNameLabel
			// 
			this.FileMethodNameLabel.AutoSize = true;
			this.FileMethodNameLabel.Location = new System.Drawing.Point(2, 5);
			this.FileMethodNameLabel.Name = "FileMethodNameLabel";
			this.FileMethodNameLabel.Size = new System.Drawing.Size(72, 13);
			this.FileMethodNameLabel.TabIndex = 3;
			this.FileMethodNameLabel.Text = "Method name";
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
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 87);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(111, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Default Wipe Method:";
			// 
			// WipeFileNamesCheckbox
			// 
			this.WipeFileNamesCheckbox.AutoSize = true;
			this.WipeFileNamesCheckbox.Location = new System.Drawing.Point(8, 37);
			this.WipeFileNamesCheckbox.Name = "WipeFileNamesCheckbox";
			this.WipeFileNamesCheckbox.Size = new System.Drawing.Size(106, 17);
			this.WipeFileNamesCheckbox.TabIndex = 1;
			this.WipeFileNamesCheckbox.Text = "Wipe File Names";
			this.WipeFileNamesCheckbox.UseVisualStyleBackColor = true;
			this.WipeFileNamesCheckbox.CheckedChanged += new System.EventHandler(this.WipeFileNamesCheckbox_CheckedChanged);
			// 
			// WipeAdsCheckbox
			// 
			this.WipeAdsCheckbox.AutoSize = true;
			this.WipeAdsCheckbox.Location = new System.Drawing.Point(8, 19);
			this.WipeAdsCheckbox.Name = "WipeAdsCheckbox";
			this.WipeAdsCheckbox.Size = new System.Drawing.Size(76, 17);
			this.WipeAdsCheckbox.TabIndex = 0;
			this.WipeAdsCheckbox.Text = "Wipe ADS";
			this.WipeAdsCheckbox.UseVisualStyleBackColor = true;
			this.WipeAdsCheckbox.CheckedChanged += new System.EventHandler(this.WipeAdsCheckbox_CheckedChanged);
			// 
			// WipeOptionsEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.WipePanel);
			this.Name = "WipeOptionsEditor";
			this.Size = new System.Drawing.Size(721, 413);
			this.BackColorChanged += new System.EventHandler(this.WipeOptions_BackColorChanged);
			this.EnabledChanged += new System.EventHandler(this.WipeOptions_EnabledChanged);
			this.WipePanel.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel WipePanel;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox WipeUnusedIndexRecordCheckbox;
		private System.Windows.Forms.CheckBox WipeUsedIndexRecordCheckbox;
		private System.Windows.Forms.CheckBox WipeUnusedFileRecordCheckbox;
		private System.Windows.Forms.CheckBox WipeUsedFileRecordCheckbox;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label FreeSpaceMethodNameLabel;
		private System.Windows.Forms.Button FreeSpaceChangeButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox DestroyFreeSpaceCheckbox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox TotalDeleteCheckbox;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label FileMethodNameLabel;
		private System.Windows.Forms.Button MethodChangeButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox WipeFileNamesCheckbox;
		private System.Windows.Forms.CheckBox WipeAdsCheckbox;
	}
}
