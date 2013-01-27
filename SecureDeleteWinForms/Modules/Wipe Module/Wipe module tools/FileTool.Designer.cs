namespace SecureDeleteWinForms.WipeTools
{
	partial class FileTool
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
			this.panel3 = new System.Windows.Forms.Panel();
			this.FilePathErrorLabel = new System.Windows.Forms.Label();
			this.CancelButton = new System.Windows.Forms.Button();
			this.SaveButton = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.MethodNameLabel = new System.Windows.Forms.Label();
			this.MethodChangeButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.BrowseButton = new System.Windows.Forms.Button();
			this.FileTextbox = new System.Windows.Forms.TextBox();
			this.PathLabel = new System.Windows.Forms.Label();
			this.panel3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.FilePathErrorLabel);
			this.panel3.Controls.Add(this.CancelButton);
			this.panel3.Controls.Add(this.SaveButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 95);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(578, 30);
			this.panel3.TabIndex = 4;
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
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
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
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.panel4);
			this.panel1.Controls.Add(this.PathLabel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(578, 95);
			this.panel1.TabIndex = 5;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.Control;
			this.panel2.Controls.Add(this.button1);
			this.panel2.Controls.Add(this.MethodNameLabel);
			this.panel2.Controls.Add(this.MethodChangeButton);
			this.panel2.Location = new System.Drawing.Point(76, 35);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(494, 52);
			this.panel2.TabIndex = 7;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(99, 25);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 24);
			this.button1.TabIndex = 5;
			this.button1.Text = "Default";
			this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
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
			this.MethodChangeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.MethodChangeButton.Image = global::SecureDeleteWinForms.Properties.Resources.Untitled;
			this.MethodChangeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.MethodChangeButton.Location = new System.Drawing.Point(5, 25);
			this.MethodChangeButton.Name = "MethodChangeButton";
			this.MethodChangeButton.Size = new System.Drawing.Size(88, 24);
			this.MethodChangeButton.TabIndex = 2;
			this.MethodChangeButton.Text = " Change";
			this.MethodChangeButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.MethodChangeButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.MethodChangeButton.UseVisualStyleBackColor = true;
			this.MethodChangeButton.Click += new System.EventHandler(this.MethodChangeButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Wipe method";
			// 
			// panel4
			// 
			this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel4.Controls.Add(this.BrowseButton);
			this.panel4.Controls.Add(this.FileTextbox);
			this.panel4.Location = new System.Drawing.Point(78, 3);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(497, 31);
			this.panel4.TabIndex = 5;
			// 
			// BrowseButton
			// 
			this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseButton.Image = global::SecureDeleteWinForms.Properties.Resources.openHS;
			this.BrowseButton.Location = new System.Drawing.Point(412, 3);
			this.BrowseButton.Name = "BrowseButton";
			this.BrowseButton.Size = new System.Drawing.Size(82, 24);
			this.BrowseButton.TabIndex = 1;
			this.BrowseButton.Text = "Browse";
			this.BrowseButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.BrowseButton.UseVisualStyleBackColor = true;
			this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
			// 
			// FileTextbox
			// 
			this.FileTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.FileTextbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.FileTextbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.FileTextbox.Location = new System.Drawing.Point(3, 6);
			this.FileTextbox.Name = "FileTextbox";
			this.FileTextbox.Size = new System.Drawing.Size(406, 20);
			this.FileTextbox.TabIndex = 0;
			this.FileTextbox.TextChanged += new System.EventHandler(this.FileTextbox_TextChanged);
			// 
			// PathLabel
			// 
			this.PathLabel.AutoSize = true;
			this.PathLabel.Location = new System.Drawing.Point(3, 13);
			this.PathLabel.Name = "PathLabel";
			this.PathLabel.Size = new System.Drawing.Size(47, 13);
			this.PathLabel.TabIndex = 4;
			this.PathLabel.Text = "File path";
			// 
			// FileTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel3);
			this.Name = "FileTool";
			this.Size = new System.Drawing.Size(578, 125);
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label MethodNameLabel;
		private System.Windows.Forms.Button MethodChangeButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Button BrowseButton;
		private System.Windows.Forms.TextBox FileTextbox;
		private System.Windows.Forms.Label PathLabel;
		private System.Windows.Forms.Label FilePathErrorLabel;
		private System.Windows.Forms.Button button1;
	}
}
