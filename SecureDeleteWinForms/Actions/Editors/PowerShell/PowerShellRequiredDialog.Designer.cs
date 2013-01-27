namespace SecureDeleteWinForms
{
	partial class PowerShellRequiredDialog
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
			this.PrimaryText = new System.Windows.Forms.Label();
			this.SecondaryText = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.DownloadButton = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// PrimaryText
			// 
			this.PrimaryText.AutoSize = true;
			this.PrimaryText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.PrimaryText.Location = new System.Drawing.Point(58, 9);
			this.PrimaryText.Name = "PrimaryText";
			this.PrimaryText.Size = new System.Drawing.Size(283, 16);
			this.PrimaryText.TabIndex = 0;
			this.PrimaryText.Text = "Windows PowerShell not found on you system.";
			// 
			// SecondaryText
			// 
			this.SecondaryText.AutoSize = true;
			this.SecondaryText.Location = new System.Drawing.Point(58, 35);
			this.SecondaryText.Name = "SecondaryText";
			this.SecondaryText.Size = new System.Drawing.Size(320, 13);
			this.SecondaryText.TabIndex = 1;
			this.SecondaryText.Text = "PowerShell needs to be installed in order to run PowerShell scripts.";
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.DownloadButton);
			this.panel3.Controls.Add(this.button1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 105);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(406, 30);
			this.panel3.TabIndex = 6;
			// 
			// DownloadButton
			// 
			this.DownloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DownloadButton.BackColor = System.Drawing.SystemColors.Control;
			this.DownloadButton.Image = global::SecureDeleteWinForms.Properties.Resources.personal_data;
			this.DownloadButton.Location = new System.Drawing.Point(233, 3);
			this.DownloadButton.Name = "DownloadButton";
			this.DownloadButton.Size = new System.Drawing.Size(92, 24);
			this.DownloadButton.TabIndex = 5;
			this.DownloadButton.Text = "Download";
			this.DownloadButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.DownloadButton.UseVisualStyleBackColor = true;
			this.DownloadButton.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.BackColor = System.Drawing.SystemColors.Control;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(327, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(76, 24);
			this.button1.TabIndex = 4;
			this.button1.Text = "Close";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::SecureDeleteWinForms.Properties.Resources.powershell;
			this.pictureBox1.Location = new System.Drawing.Point(3, 4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 48);
			this.pictureBox1.TabIndex = 7;
			this.pictureBox1.TabStop = false;
			// 
			// PowerShellRequiredDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(406, 135);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.SecondaryText);
			this.Controls.Add(this.PrimaryText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PowerShellRequiredDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "PowerShell";
			this.panel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.PictureBox pictureBox1;
		public System.Windows.Forms.Button DownloadButton;
		public System.Windows.Forms.Label SecondaryText;
		public System.Windows.Forms.Label PrimaryText;
	}
}