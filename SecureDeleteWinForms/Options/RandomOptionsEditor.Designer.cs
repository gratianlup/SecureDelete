namespace SecureDeleteWinForms.Options
{
	partial class RandomOptionsEditor
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
			this.RandomPanel = new System.Windows.Forms.Panel();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.ReseedLabel = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.ReseedIntervalTrackbar = new System.Windows.Forms.TrackBar();
			this.ReseedCheckbox = new System.Windows.Forms.CheckBox();
			this.PreventCheckbox = new System.Windows.Forms.CheckBox();
			this.SlowPoolCheckbox = new System.Windows.Forms.CheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.MersenneOptionbox = new System.Windows.Forms.RadioButton();
			this.IsaacOptionbox = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.RandomPanel.SuspendLayout();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ReseedIntervalTrackbar)).BeginInit();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// RandomPanel
			// 
			this.RandomPanel.BackColor = System.Drawing.SystemColors.Control;
			this.RandomPanel.Controls.Add(this.groupBox5);
			this.RandomPanel.Controls.Add(this.groupBox4);
			this.RandomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RandomPanel.Location = new System.Drawing.Point(0, 0);
			this.RandomPanel.Name = "RandomPanel";
			this.RandomPanel.Size = new System.Drawing.Size(639, 311);
			this.RandomPanel.TabIndex = 13;
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox5.Controls.Add(this.ReseedLabel);
			this.groupBox5.Controls.Add(this.label6);
			this.groupBox5.Controls.Add(this.ReseedIntervalTrackbar);
			this.groupBox5.Controls.Add(this.ReseedCheckbox);
			this.groupBox5.Controls.Add(this.PreventCheckbox);
			this.groupBox5.Controls.Add(this.SlowPoolCheckbox);
			this.groupBox5.Location = new System.Drawing.Point(12, 100);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(615, 111);
			this.groupBox5.TabIndex = 3;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Random Generator Options";
			// 
			// ReseedLabel
			// 
			this.ReseedLabel.AutoSize = true;
			this.ReseedLabel.Location = new System.Drawing.Point(321, 82);
			this.ReseedLabel.Name = "ReseedLabel";
			this.ReseedLabel.Size = new System.Drawing.Size(62, 13);
			this.ReseedLabel.TabIndex = 5;
			this.ReseedLabel.Text = "30 seconds";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(24, 82);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(82, 13);
			this.label6.TabIndex = 4;
			this.label6.Text = "Reseed Interval";
			// 
			// ReseedIntervalTrackbar
			// 
			this.ReseedIntervalTrackbar.AutoSize = false;
			this.ReseedIntervalTrackbar.LargeChange = 30;
			this.ReseedIntervalTrackbar.Location = new System.Drawing.Point(110, 78);
			this.ReseedIntervalTrackbar.Maximum = 720;
			this.ReseedIntervalTrackbar.Minimum = 1;
			this.ReseedIntervalTrackbar.Name = "ReseedIntervalTrackbar";
			this.ReseedIntervalTrackbar.Size = new System.Drawing.Size(209, 29);
			this.ReseedIntervalTrackbar.TabIndex = 3;
			this.ReseedIntervalTrackbar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.ReseedIntervalTrackbar.Value = 30;
			this.ReseedIntervalTrackbar.ValueChanged += new System.EventHandler(this.ReseedIntervalTrackbar_ValueChanged);
			// 
			// ReseedCheckbox
			// 
			this.ReseedCheckbox.AutoSize = true;
			this.ReseedCheckbox.Location = new System.Drawing.Point(8, 59);
			this.ReseedCheckbox.Name = "ReseedCheckbox";
			this.ReseedCheckbox.Size = new System.Drawing.Size(63, 17);
			this.ReseedCheckbox.TabIndex = 2;
			this.ReseedCheckbox.Text = "Reseed";
			this.ReseedCheckbox.UseVisualStyleBackColor = true;
			this.ReseedCheckbox.CheckedChanged += new System.EventHandler(this.ReseedCheckbox_CheckedChanged);
			// 
			// PreventCheckbox
			// 
			this.PreventCheckbox.AutoSize = true;
			this.PreventCheckbox.Location = new System.Drawing.Point(8, 39);
			this.PreventCheckbox.Name = "PreventCheckbox";
			this.PreventCheckbox.Size = new System.Drawing.Size(289, 17);
			this.PreventCheckbox.TabIndex = 1;
			this.PreventCheckbox.Text = "Prevent the OS to write the random pool to the swap file";
			this.PreventCheckbox.UseVisualStyleBackColor = true;
			this.PreventCheckbox.CheckedChanged += new System.EventHandler(this.PreventCheckbox_CheckedChanged);
			// 
			// SlowPoolCheckbox
			// 
			this.SlowPoolCheckbox.AutoSize = true;
			this.SlowPoolCheckbox.Location = new System.Drawing.Point(8, 19);
			this.SlowPoolCheckbox.Name = "SlowPoolCheckbox";
			this.SlowPoolCheckbox.Size = new System.Drawing.Size(95, 17);
			this.SlowPoolCheckbox.TabIndex = 0;
			this.SlowPoolCheckbox.Text = "Use Slow Pool";
			this.SlowPoolCheckbox.UseVisualStyleBackColor = true;
			this.SlowPoolCheckbox.CheckedChanged += new System.EventHandler(this.SlowPoolCheckbox_CheckedChanged);
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.MersenneOptionbox);
			this.groupBox4.Controls.Add(this.IsaacOptionbox);
			this.groupBox4.Controls.Add(this.label5);
			this.groupBox4.Location = new System.Drawing.Point(12, 9);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(615, 85);
			this.groupBox4.TabIndex = 2;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Random Generator";
			// 
			// MersenneOptionbox
			// 
			this.MersenneOptionbox.AutoSize = true;
			this.MersenneOptionbox.Location = new System.Drawing.Point(8, 58);
			this.MersenneOptionbox.Name = "MersenneOptionbox";
			this.MersenneOptionbox.Size = new System.Drawing.Size(72, 17);
			this.MersenneOptionbox.TabIndex = 2;
			this.MersenneOptionbox.TabStop = true;
			this.MersenneOptionbox.Text = "Mersenne";
			this.MersenneOptionbox.UseVisualStyleBackColor = true;
			this.MersenneOptionbox.CheckedChanged += new System.EventHandler(this.MersenneOptionbox_CheckedChanged);
			// 
			// IsaacOptionbox
			// 
			this.IsaacOptionbox.AutoSize = true;
			this.IsaacOptionbox.Location = new System.Drawing.Point(8, 38);
			this.IsaacOptionbox.Name = "IsaacOptionbox";
			this.IsaacOptionbox.Size = new System.Drawing.Size(56, 17);
			this.IsaacOptionbox.TabIndex = 1;
			this.IsaacOptionbox.TabStop = true;
			this.IsaacOptionbox.Text = "ISAAC";
			this.IsaacOptionbox.UseVisualStyleBackColor = true;
			this.IsaacOptionbox.CheckedChanged += new System.EventHandler(this.IsaacOptionbox_CheckedChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(5, 19);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(194, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Select the random generator to be used";
			// 
			// RandomOptionsEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.RandomPanel);
			this.Name = "RandomOptionsEditor";
			this.Size = new System.Drawing.Size(639, 311);
			this.BackColorChanged += new System.EventHandler(this.RandomOptions_BackColorChanged);
			this.EnabledChanged += new System.EventHandler(this.RandomOptions_EnabledChanged);
			this.RandomPanel.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ReseedIntervalTrackbar)).EndInit();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel RandomPanel;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label ReseedLabel;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TrackBar ReseedIntervalTrackbar;
		private System.Windows.Forms.CheckBox ReseedCheckbox;
		private System.Windows.Forms.CheckBox PreventCheckbox;
		private System.Windows.Forms.CheckBox SlowPoolCheckbox;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RadioButton MersenneOptionbox;
		private System.Windows.Forms.RadioButton IsaacOptionbox;
		private System.Windows.Forms.Label label5;
	}
}
