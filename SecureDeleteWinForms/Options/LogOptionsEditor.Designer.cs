namespace SecureDeleteWinForms.Options
{
	partial class LogOptionsEditor
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
			this.groupBox12 = new System.Windows.Forms.GroupBox();
			this.OpenLogButton = new System.Windows.Forms.Button();
			this.LogBrowseButton = new System.Windows.Forms.Button();
			this.LogLimitLabel = new System.Windows.Forms.Label();
			this.LogLimitValue = new System.Windows.Forms.NumericUpDown();
			this.LogLimitCheckbox = new System.Windows.Forms.CheckBox();
			this.AppendLogCheckbox = new System.Windows.Forms.CheckBox();
			this.LogTextbox = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.LogCheckbox = new System.Windows.Forms.CheckBox();
			this.groupBox12.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.LogLimitValue)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox12
			// 
			this.groupBox12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox12.BackColor = System.Drawing.SystemColors.Control;
			this.groupBox12.Controls.Add(this.OpenLogButton);
			this.groupBox12.Controls.Add(this.LogBrowseButton);
			this.groupBox12.Controls.Add(this.LogLimitLabel);
			this.groupBox12.Controls.Add(this.LogLimitValue);
			this.groupBox12.Controls.Add(this.LogLimitCheckbox);
			this.groupBox12.Controls.Add(this.AppendLogCheckbox);
			this.groupBox12.Controls.Add(this.LogTextbox);
			this.groupBox12.Controls.Add(this.label7);
			this.groupBox12.Controls.Add(this.LogCheckbox);
			this.groupBox12.Location = new System.Drawing.Point(12, 9);
			this.groupBox12.Name = "groupBox12";
			this.groupBox12.Size = new System.Drawing.Size(697, 118);
			this.groupBox12.TabIndex = 12;
			this.groupBox12.TabStop = false;
			this.groupBox12.Text = "Log File";
			// 
			// OpenLogButton
			// 
			this.OpenLogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.OpenLogButton.BackColor = System.Drawing.SystemColors.Control;
			this.OpenLogButton.Image = global::SecureDeleteWinForms.Properties.Resources.eventlog;
			this.OpenLogButton.Location = new System.Drawing.Point(604, 66);
			this.OpenLogButton.Name = "OpenLogButton";
			this.OpenLogButton.Size = new System.Drawing.Size(87, 24);
			this.OpenLogButton.TabIndex = 11;
			this.OpenLogButton.Text = "Open log";
			this.OpenLogButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.OpenLogButton.UseVisualStyleBackColor = true;
			// 
			// LogBrowseButton
			// 
			this.LogBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.LogBrowseButton.BackColor = System.Drawing.SystemColors.Control;
			this.LogBrowseButton.Enabled = false;
			this.LogBrowseButton.Image = global::SecureDeleteWinForms.Properties.Resources.openHS;
			this.LogBrowseButton.Location = new System.Drawing.Point(604, 40);
			this.LogBrowseButton.Name = "LogBrowseButton";
			this.LogBrowseButton.Size = new System.Drawing.Size(87, 24);
			this.LogBrowseButton.TabIndex = 10;
			this.LogBrowseButton.Text = "Browse";
			this.LogBrowseButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.LogBrowseButton.UseVisualStyleBackColor = true;
			// 
			// LogLimitLabel
			// 
			this.LogLimitLabel.AutoSize = true;
			this.LogLimitLabel.Enabled = false;
			this.LogLimitLabel.Location = new System.Drawing.Point(219, 96);
			this.LogLimitLabel.Name = "LogLimitLabel";
			this.LogLimitLabel.Size = new System.Drawing.Size(21, 13);
			this.LogLimitLabel.TabIndex = 9;
			this.LogLimitLabel.Text = "KB";
			// 
			// LogLimitValue
			// 
			this.LogLimitValue.Enabled = false;
			this.LogLimitValue.Location = new System.Drawing.Point(125, 92);
			this.LogLimitValue.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
			this.LogLimitValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.LogLimitValue.Name = "LogLimitValue";
			this.LogLimitValue.Size = new System.Drawing.Size(88, 20);
			this.LogLimitValue.TabIndex = 8;
			this.LogLimitValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// LogLimitCheckbox
			// 
			this.LogLimitCheckbox.AutoSize = true;
			this.LogLimitCheckbox.Enabled = false;
			this.LogLimitCheckbox.Location = new System.Drawing.Point(6, 94);
			this.LogLimitCheckbox.Name = "LogLimitCheckbox";
			this.LogLimitCheckbox.Size = new System.Drawing.Size(113, 17);
			this.LogLimitCheckbox.TabIndex = 7;
			this.LogLimitCheckbox.Text = "Limit log file size to";
			this.LogLimitCheckbox.UseVisualStyleBackColor = true;
			// 
			// AppendLogCheckbox
			// 
			this.AppendLogCheckbox.AutoSize = true;
			this.AppendLogCheckbox.Enabled = false;
			this.AppendLogCheckbox.Location = new System.Drawing.Point(6, 73);
			this.AppendLogCheckbox.Name = "AppendLogCheckbox";
			this.AppendLogCheckbox.Size = new System.Drawing.Size(181, 17);
			this.AppendLogCheckbox.TabIndex = 6;
			this.AppendLogCheckbox.Text = "Append new errors to existing file";
			this.AppendLogCheckbox.UseVisualStyleBackColor = true;
			// 
			// LogTextbox
			// 
			this.LogTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.LogTextbox.Enabled = false;
			this.LogTextbox.Location = new System.Drawing.Point(60, 42);
			this.LogTextbox.Name = "LogTextbox";
			this.LogTextbox.Size = new System.Drawing.Size(541, 20);
			this.LogTextbox.TabIndex = 3;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Enabled = false;
			this.label7.Location = new System.Drawing.Point(25, 46);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(29, 13);
			this.label7.TabIndex = 2;
			this.label7.Text = "Path";
			// 
			// LogCheckbox
			// 
			this.LogCheckbox.AutoSize = true;
			this.LogCheckbox.Location = new System.Drawing.Point(8, 20);
			this.LogCheckbox.Name = "LogCheckbox";
			this.LogCheckbox.Size = new System.Drawing.Size(143, 17);
			this.LogCheckbox.TabIndex = 0;
			this.LogCheckbox.Text = "Write errors to the log file";
			this.LogCheckbox.UseVisualStyleBackColor = true;
			// 
			// LogOptionsEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox12);
			this.Name = "LogOptionsEditor";
			this.Size = new System.Drawing.Size(721, 137);
			this.groupBox12.ResumeLayout(false);
			this.groupBox12.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.LogLimitValue)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox12;
		private System.Windows.Forms.Button OpenLogButton;
		private System.Windows.Forms.Button LogBrowseButton;
		private System.Windows.Forms.Label LogLimitLabel;
		private System.Windows.Forms.NumericUpDown LogLimitValue;
		private System.Windows.Forms.CheckBox LogLimitCheckbox;
		private System.Windows.Forms.CheckBox AppendLogCheckbox;
		private System.Windows.Forms.TextBox LogTextbox;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckBox LogCheckbox;

	}
}
