namespace SecureDeleteWinForms
{
	partial class StartWipeDialog
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
			this.panel3 = new System.Windows.Forms.Panel();
			this.DontShow = new System.Windows.Forms.CheckBox();
			this.CancelButton = new System.Windows.Forms.Button();
			this.InsertButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.DontShow);
			this.panel3.Controls.Add(this.CancelButton);
			this.panel3.Controls.Add(this.InsertButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 130);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(419, 30);
			this.panel3.TabIndex = 10;
			// 
			// DontShow
			// 
			this.DontShow.AutoSize = true;
			this.DontShow.ForeColor = System.Drawing.Color.White;
			this.DontShow.Location = new System.Drawing.Point(7, 8);
			this.DontShow.Name = "DontShow";
			this.DontShow.Size = new System.Drawing.Size(127, 17);
			this.DontShow.TabIndex = 5;
			this.DontShow.Text = "Don\'t show this again";
			this.DontShow.UseVisualStyleBackColor = true;
			// 
			// CancelButton
			// 
			this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelButton.BackColor = System.Drawing.Color.AliceBlue;
			this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelButton.Location = new System.Drawing.Point(345, 3);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(71, 24);
			this.CancelButton.TabIndex = 4;
			this.CancelButton.Text = "No";
			this.CancelButton.UseVisualStyleBackColor = true;
			// 
			// InsertButton
			// 
			this.InsertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.InsertButton.BackColor = System.Drawing.Color.AliceBlue;
			this.InsertButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.InsertButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.InsertButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.InsertButton.Location = new System.Drawing.Point(275, 3);
			this.InsertButton.Name = "InsertButton";
			this.InsertButton.Size = new System.Drawing.Size(70, 24);
			this.InsertButton.TabIndex = 3;
			this.InsertButton.Text = "Yes";
			this.InsertButton.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(72, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(203, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "You are about to erase all data on the list.";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(72, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(309, 37);
			this.label2.TabIndex = 12;
			this.label2.Text = "Files cannot be recovered after erasing, please make sure that the list contains " +
				"only the data that you want to destroy,";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label3.Location = new System.Drawing.Point(72, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(215, 16);
			this.label3.TabIndex = 13;
			this.label3.Text = "Are you sure you want to continue ?";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::SecureDeleteWinForms.Properties.Resources.important;
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(53, 53);
			this.pictureBox1.TabIndex = 14;
			this.pictureBox1.TabStop = false;
			// 
			// StartWipeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(419, 160);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "StartWipeDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SecureDelete";
			this.Load += new System.EventHandler(this.StartWipeDialog_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartWipeDialog_FormClosing);
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Button InsertButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.PictureBox pictureBox1;
		public System.Windows.Forms.CheckBox DontShow;
	}
}