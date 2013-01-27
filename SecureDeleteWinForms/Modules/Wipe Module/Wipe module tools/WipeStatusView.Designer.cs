namespace SecureDeleteWinForms
{
	partial class WipeStatusView
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
			this.SecondaryStatusLabel = new System.Windows.Forms.Label();
			this.ItemProgressBar = new System.Windows.Forms.ProgressBar();
			this.PercentLabel = new System.Windows.Forms.Label();
			this.StepLabel = new System.Windows.Forms.Label();
			this.SizeLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// SecondaryStatusLabel
			// 
			this.SecondaryStatusLabel.AutoSize = true;
			this.SecondaryStatusLabel.Location = new System.Drawing.Point(5, 34);
			this.SecondaryStatusLabel.Name = "SecondaryStatusLabel";
			this.SecondaryStatusLabel.Size = new System.Drawing.Size(83, 13);
			this.SecondaryStatusLabel.TabIndex = 0;
			this.SecondaryStatusLabel.Text = "Wiping member:";
			// 
			// ItemProgressBar
			// 
			this.ItemProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemProgressBar.Location = new System.Drawing.Point(8, 54);
			this.ItemProgressBar.Name = "ItemProgressBar";
			this.ItemProgressBar.Size = new System.Drawing.Size(599, 19);
			this.ItemProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.ItemProgressBar.TabIndex = 1;
			// 
			// PercentLabel
			// 
			this.PercentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.PercentLabel.Location = new System.Drawing.Point(610, 57);
			this.PercentLabel.Name = "PercentLabel";
			this.PercentLabel.Size = new System.Drawing.Size(51, 13);
			this.PercentLabel.TabIndex = 2;
			this.PercentLabel.Text = "100.00 %";
			this.PercentLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// StepLabel
			// 
			this.StepLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.StepLabel.Location = new System.Drawing.Point(501, 78);
			this.StepLabel.Name = "StepLabel";
			this.StepLabel.Size = new System.Drawing.Size(106, 13);
			this.StepLabel.TabIndex = 3;
			this.StepLabel.Text = "Step: 1 of 3";
			this.StepLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// SizeLabel
			// 
			this.SizeLabel.AutoSize = true;
			this.SizeLabel.Location = new System.Drawing.Point(5, 78);
			this.SizeLabel.Name = "SizeLabel";
			this.SizeLabel.Size = new System.Drawing.Size(71, 13);
			this.SizeLabel.TabIndex = 7;
			this.SizeLabel.Text = "25 of 130 MB";
			// 
			// WipeStatusView
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.SizeLabel);
			this.Controls.Add(this.StepLabel);
			this.Controls.Add(this.PercentLabel);
			this.Controls.Add(this.ItemProgressBar);
			this.Controls.Add(this.SecondaryStatusLabel);
			this.Size = new System.Drawing.Size(664, 101);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.Label PercentLabel;
		public System.Windows.Forms.Label StepLabel;
		public System.Windows.Forms.ProgressBar ItemProgressBar;
		public System.Windows.Forms.Label SecondaryStatusLabel;
		public System.Windows.Forms.Label SizeLabel;
	}
}
