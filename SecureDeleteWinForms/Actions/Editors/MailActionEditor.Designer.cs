namespace SecureDeleteWinForms
{
	partial class MailActionEditor
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
			this.UserTextbox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.ServerTextbox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.AdressTextbox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.EnabledCheckbox = new System.Windows.Forms.CheckBox();
			this.PasswordTextbox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.SubjectTextbox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.ReportCheckbox = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.PortValue = new System.Windows.Forms.NumericUpDown();
			this.TestButton = new System.Windows.Forms.Button();
			this.LocalHostButton = new System.Windows.Forms.Button();
			this.ValidLabel = new System.Windows.Forms.Label();
			this.DefaultPortButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.PortValue)).BeginInit();
			this.SuspendLayout();
			// 
			// UserTextbox
			// 
			this.UserTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.UserTextbox.Enabled = false;
			this.UserTextbox.Location = new System.Drawing.Point(87, 105);
			this.UserTextbox.MinimumSize = new System.Drawing.Size(237, 4);
			this.UserTextbox.Name = "UserTextbox";
			this.UserTextbox.Size = new System.Drawing.Size(237, 20);
			this.UserTextbox.TabIndex = 26;
			this.UserTextbox.TextChanged += new System.EventHandler(this.UserTextbox_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 108);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(58, 13);
			this.label3.TabIndex = 25;
			this.label3.Text = "User name";
			// 
			// ServerTextbox
			// 
			this.ServerTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ServerTextbox.Enabled = false;
			this.ServerTextbox.Location = new System.Drawing.Point(87, 53);
			this.ServerTextbox.Name = "ServerTextbox";
			this.ServerTextbox.Size = new System.Drawing.Size(456, 20);
			this.ServerTextbox.TabIndex = 24;
			this.ServerTextbox.TextChanged += new System.EventHandler(this.ServerTextbox_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 26);
			this.label2.TabIndex = 23;
			this.label2.Text = "Outgoing mail server (SMTP)";
			// 
			// AdressTextbox
			// 
			this.AdressTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.AdressTextbox.Enabled = false;
			this.AdressTextbox.Location = new System.Drawing.Point(87, 28);
			this.AdressTextbox.Name = "AdressTextbox";
			this.AdressTextbox.Size = new System.Drawing.Size(456, 20);
			this.AdressTextbox.TabIndex = 21;
			this.AdressTextbox.TextChanged += new System.EventHandler(this.PathTextbox_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(66, 13);
			this.label1.TabIndex = 20;
			this.label1.Text = "Email adress";
			// 
			// EnabledCheckbox
			// 
			this.EnabledCheckbox.AutoSize = true;
			this.EnabledCheckbox.Enabled = false;
			this.EnabledCheckbox.Location = new System.Drawing.Point(9, 6);
			this.EnabledCheckbox.Name = "EnabledCheckbox";
			this.EnabledCheckbox.Size = new System.Drawing.Size(65, 17);
			this.EnabledCheckbox.TabIndex = 19;
			this.EnabledCheckbox.Text = "Enabled";
			this.EnabledCheckbox.UseVisualStyleBackColor = true;
			this.EnabledCheckbox.CheckedChanged += new System.EventHandler(this.EnabledCheckbox_CheckedChanged);
			// 
			// PasswordTextbox
			// 
			this.PasswordTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.PasswordTextbox.Enabled = false;
			this.PasswordTextbox.Location = new System.Drawing.Point(87, 129);
			this.PasswordTextbox.MinimumSize = new System.Drawing.Size(237, 4);
			this.PasswordTextbox.Name = "PasswordTextbox";
			this.PasswordTextbox.Size = new System.Drawing.Size(237, 20);
			this.PasswordTextbox.TabIndex = 28;
			this.PasswordTextbox.UseSystemPasswordChar = true;
			this.PasswordTextbox.TextChanged += new System.EventHandler(this.PasswordTextbox_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 132);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(53, 13);
			this.label4.TabIndex = 27;
			this.label4.Text = "Password";
			// 
			// SubjectTextbox
			// 
			this.SubjectTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.SubjectTextbox.Enabled = false;
			this.SubjectTextbox.Location = new System.Drawing.Point(87, 154);
			this.SubjectTextbox.Name = "SubjectTextbox";
			this.SubjectTextbox.Size = new System.Drawing.Size(559, 20);
			this.SubjectTextbox.TabIndex = 30;
			this.SubjectTextbox.TextChanged += new System.EventHandler(this.SubjectTextbox_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 157);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(43, 13);
			this.label5.TabIndex = 29;
			this.label5.Text = "Subject";
			// 
			// ReportCheckbox
			// 
			this.ReportCheckbox.AutoSize = true;
			this.ReportCheckbox.Location = new System.Drawing.Point(9, 186);
			this.ReportCheckbox.Name = "ReportCheckbox";
			this.ReportCheckbox.Size = new System.Drawing.Size(175, 17);
			this.ReportCheckbox.TabIndex = 31;
			this.ReportCheckbox.Text = "Add wipe report to the message";
			this.ReportCheckbox.UseVisualStyleBackColor = true;
			this.ReportCheckbox.CheckedChanged += new System.EventHandler(this.ReportCheckbox_CheckedChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 82);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(26, 13);
			this.label6.TabIndex = 32;
			this.label6.Text = "Port";
			// 
			// PortValue
			// 
			this.PortValue.Location = new System.Drawing.Point(87, 79);
			this.PortValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.PortValue.Name = "PortValue";
			this.PortValue.Size = new System.Drawing.Size(91, 20);
			this.PortValue.TabIndex = 33;
			this.PortValue.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
			this.PortValue.ValueChanged += new System.EventHandler(this.PortValue_ValueChanged);
			// 
			// TestButton
			// 
			this.TestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.TestButton.Image = global::SecureDeleteWinForms.Properties.Resources.mail;
			this.TestButton.Location = new System.Drawing.Point(514, 182);
			this.TestButton.Name = "TestButton";
			this.TestButton.Size = new System.Drawing.Size(132, 23);
			this.TestButton.TabIndex = 34;
			this.TestButton.Text = "Send test message";
			this.TestButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.TestButton.UseVisualStyleBackColor = true;
			this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
			// 
			// LocalHostButton
			// 
			this.LocalHostButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.LocalHostButton.Image = global::SecureDeleteWinForms.Properties.Resources.Computer;
			this.LocalHostButton.Location = new System.Drawing.Point(546, 51);
			this.LocalHostButton.Name = "LocalHostButton";
			this.LocalHostButton.Size = new System.Drawing.Size(100, 24);
			this.LocalHostButton.TabIndex = 35;
			this.LocalHostButton.Text = "Localhost";
			this.LocalHostButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.LocalHostButton.UseVisualStyleBackColor = true;
			this.LocalHostButton.Click += new System.EventHandler(this.LocalHostButton_Click);
			// 
			// ValidLabel
			// 
			this.ValidLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ValidLabel.ForeColor = System.Drawing.Color.Red;
			this.ValidLabel.Location = new System.Drawing.Point(546, 31);
			this.ValidLabel.Name = "ValidLabel";
			this.ValidLabel.Size = new System.Drawing.Size(86, 13);
			this.ValidLabel.TabIndex = 36;
			this.ValidLabel.Text = "Invalid adress";
			this.ValidLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DefaultPortButton
			// 
			this.DefaultPortButton.Location = new System.Drawing.Point(181, 77);
			this.DefaultPortButton.Name = "DefaultPortButton";
			this.DefaultPortButton.Size = new System.Drawing.Size(74, 23);
			this.DefaultPortButton.TabIndex = 37;
			this.DefaultPortButton.Text = "Default";
			this.DefaultPortButton.UseVisualStyleBackColor = true;
			this.DefaultPortButton.Click += new System.EventHandler(this.button1_Click);
			// 
			// MailActionEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.DefaultPortButton);
			this.Controls.Add(this.ValidLabel);
			this.Controls.Add(this.LocalHostButton);
			this.Controls.Add(this.TestButton);
			this.Controls.Add(this.PortValue);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.ReportCheckbox);
			this.Controls.Add(this.SubjectTextbox);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.PasswordTextbox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.UserTextbox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.ServerTextbox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.AdressTextbox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.EnabledCheckbox);
			this.Name = "MailActionEditor";
			this.Size = new System.Drawing.Size(653, 207);
			((System.ComponentModel.ISupportInitialize)(this.PortValue)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox UserTextbox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox ServerTextbox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox AdressTextbox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox EnabledCheckbox;
		private System.Windows.Forms.TextBox PasswordTextbox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox SubjectTextbox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox ReportCheckbox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown PortValue;
		private System.Windows.Forms.Button TestButton;
		private System.Windows.Forms.Button LocalHostButton;
		private System.Windows.Forms.Label ValidLabel;
		private System.Windows.Forms.Button DefaultPortButton;
	}
}
