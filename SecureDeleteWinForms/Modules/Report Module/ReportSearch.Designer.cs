namespace SecureDeleteWinForms
{
	partial class ReportSearcher
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.SearchErrorsCheckbox = new System.Windows.Forms.CheckBox();
			this.SearchFailedCheckbox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.MatchCaseCheckbox = new System.Windows.Forms.CheckBox();
			this.SearchTextbox = new System.Windows.Forms.TextBox();
			this.PathLabel = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.ErrorCombobox = new System.Windows.Forms.ComboBox();
			this.ErrorCheckbox = new System.Windows.Forms.CheckBox();
			this.FailedCombobox = new System.Windows.Forms.ComboBox();
			this.FailedCheckbox = new System.Windows.Forms.CheckBox();
			this.DateValue = new System.Windows.Forms.DateTimePicker();
			this.DateCombobox = new System.Windows.Forms.ComboBox();
			this.DateCheckbox = new System.Windows.Forms.CheckBox();
			this.CancelButton = new System.Windows.Forms.Button();
			this.panel3 = new System.Windows.Forms.Panel();
			this.SearchButton = new System.Windows.Forms.Button();
			this.ErrorTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.FailedNumber = new System.Windows.Forms.NumericUpDown();
			this.ErrorNumber = new System.Windows.Forms.NumericUpDown();
			this.panel1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.FailedNumber)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ErrorNumber)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.groupBox2);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(706, 236);
			this.panel1.TabIndex = 7;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.SearchErrorsCheckbox);
			this.groupBox2.Controls.Add(this.SearchFailedCheckbox);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.button2);
			this.groupBox2.Controls.Add(this.MatchCaseCheckbox);
			this.groupBox2.Controls.Add(this.SearchTextbox);
			this.groupBox2.Controls.Add(this.PathLabel);
			this.groupBox2.Location = new System.Drawing.Point(6, 4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(694, 82);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			// 
			// SearchErrorsCheckbox
			// 
			this.SearchErrorsCheckbox.AutoSize = true;
			this.SearchErrorsCheckbox.Checked = true;
			this.SearchErrorsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.SearchErrorsCheckbox.Location = new System.Drawing.Point(165, 38);
			this.SearchErrorsCheckbox.Name = "SearchErrorsCheckbox";
			this.SearchErrorsCheckbox.Size = new System.Drawing.Size(53, 17);
			this.SearchErrorsCheckbox.TabIndex = 23;
			this.SearchErrorsCheckbox.Text = "Errors";
			this.SearchErrorsCheckbox.UseVisualStyleBackColor = true;
			// 
			// SearchFailedCheckbox
			// 
			this.SearchFailedCheckbox.AutoSize = true;
			this.SearchFailedCheckbox.Checked = true;
			this.SearchFailedCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.SearchFailedCheckbox.Location = new System.Drawing.Point(68, 38);
			this.SearchFailedCheckbox.Name = "SearchFailedCheckbox";
			this.SearchFailedCheckbox.Size = new System.Drawing.Size(91, 17);
			this.SearchFailedCheckbox.TabIndex = 22;
			this.SearchFailedCheckbox.Text = "Failed objects";
			this.SearchFailedCheckbox.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 39);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 13);
			this.label1.TabIndex = 21;
			this.label1.Text = "Search in";
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Image = global::SecureDeleteWinForms.Properties.Resources.delete_profile;
			this.button2.Location = new System.Drawing.Point(665, 10);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(23, 23);
			this.button2.TabIndex = 20;
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// MatchCaseCheckbox
			// 
			this.MatchCaseCheckbox.AutoSize = true;
			this.MatchCaseCheckbox.Location = new System.Drawing.Point(68, 59);
			this.MatchCaseCheckbox.Name = "MatchCaseCheckbox";
			this.MatchCaseCheckbox.Size = new System.Drawing.Size(82, 17);
			this.MatchCaseCheckbox.TabIndex = 18;
			this.MatchCaseCheckbox.Text = "Match case";
			this.MatchCaseCheckbox.UseVisualStyleBackColor = true;
			// 
			// SearchTextbox
			// 
			this.SearchTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.SearchTextbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.SearchTextbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.SearchTextbox.Location = new System.Drawing.Point(68, 12);
			this.SearchTextbox.Name = "SearchTextbox";
			this.SearchTextbox.Size = new System.Drawing.Size(594, 20);
			this.SearchTextbox.TabIndex = 17;
			// 
			// PathLabel
			// 
			this.PathLabel.AutoSize = true;
			this.PathLabel.Location = new System.Drawing.Point(6, 15);
			this.PathLabel.Name = "PathLabel";
			this.PathLabel.Size = new System.Drawing.Size(41, 13);
			this.PathLabel.TabIndex = 16;
			this.PathLabel.Text = "Search";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.ErrorNumber);
			this.groupBox1.Controls.Add(this.FailedNumber);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.ErrorCombobox);
			this.groupBox1.Controls.Add(this.ErrorCheckbox);
			this.groupBox1.Controls.Add(this.FailedCombobox);
			this.groupBox1.Controls.Add(this.FailedCheckbox);
			this.groupBox1.Controls.Add(this.DateValue);
			this.groupBox1.Controls.Add(this.DateCombobox);
			this.groupBox1.Controls.Add(this.DateCheckbox);
			this.groupBox1.Location = new System.Drawing.Point(6, 92);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(694, 103);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Advanced";
			// 
			// button1
			// 
			this.button1.Image = global::SecureDeleteWinForms.Properties.Resources.date;
			this.button1.Location = new System.Drawing.Point(367, 17);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 24);
			this.button1.TabIndex = 24;
			this.button1.Text = "Today";
			this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// ErrorCombobox
			// 
			this.ErrorCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ErrorCombobox.Enabled = false;
			this.ErrorCombobox.FormattingEnabled = true;
			this.ErrorCombobox.Items.AddRange(new object[] {
            "smaller than",
            "equals",
            "greater than"});
			this.ErrorCombobox.Location = new System.Drawing.Point(107, 73);
			this.ErrorCombobox.Name = "ErrorCombobox";
			this.ErrorCombobox.Size = new System.Drawing.Size(121, 21);
			this.ErrorCombobox.TabIndex = 22;
			// 
			// ErrorCheckbox
			// 
			this.ErrorCheckbox.AutoSize = true;
			this.ErrorCheckbox.Location = new System.Drawing.Point(9, 75);
			this.ErrorCheckbox.Name = "ErrorCheckbox";
			this.ErrorCheckbox.Size = new System.Drawing.Size(86, 17);
			this.ErrorCheckbox.TabIndex = 21;
			this.ErrorCheckbox.Text = "Error number";
			this.ErrorCheckbox.UseVisualStyleBackColor = true;
			this.ErrorCheckbox.CheckedChanged += new System.EventHandler(this.ErrorCheckbox_CheckedChanged);
			// 
			// FailedCombobox
			// 
			this.FailedCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FailedCombobox.Enabled = false;
			this.FailedCombobox.FormattingEnabled = true;
			this.FailedCombobox.Items.AddRange(new object[] {
            "smaller than",
            "equals",
            "greater than"});
			this.FailedCombobox.Location = new System.Drawing.Point(107, 46);
			this.FailedCombobox.Name = "FailedCombobox";
			this.FailedCombobox.Size = new System.Drawing.Size(121, 21);
			this.FailedCombobox.TabIndex = 19;
			// 
			// FailedCheckbox
			// 
			this.FailedCheckbox.AutoSize = true;
			this.FailedCheckbox.Location = new System.Drawing.Point(9, 48);
			this.FailedCheckbox.Name = "FailedCheckbox";
			this.FailedCheckbox.Size = new System.Drawing.Size(92, 17);
			this.FailedCheckbox.TabIndex = 18;
			this.FailedCheckbox.Text = "Failed number";
			this.FailedCheckbox.UseVisualStyleBackColor = true;
			this.FailedCheckbox.CheckedChanged += new System.EventHandler(this.FailedCheckbox_CheckedChanged);
			// 
			// DateValue
			// 
			this.DateValue.Enabled = false;
			this.DateValue.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.DateValue.Location = new System.Drawing.Point(234, 21);
			this.DateValue.Name = "DateValue";
			this.DateValue.Size = new System.Drawing.Size(127, 20);
			this.DateValue.TabIndex = 17;
			// 
			// DateCombobox
			// 
			this.DateCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DateCombobox.Enabled = false;
			this.DateCombobox.FormattingEnabled = true;
			this.DateCombobox.Items.AddRange(new object[] {
            "before",
            "on",
            "after"});
			this.DateCombobox.Location = new System.Drawing.Point(107, 20);
			this.DateCombobox.Name = "DateCombobox";
			this.DateCombobox.Size = new System.Drawing.Size(121, 21);
			this.DateCombobox.TabIndex = 16;
			// 
			// DateCheckbox
			// 
			this.DateCheckbox.AutoSize = true;
			this.DateCheckbox.Location = new System.Drawing.Point(9, 22);
			this.DateCheckbox.Name = "DateCheckbox";
			this.DateCheckbox.Size = new System.Drawing.Size(63, 17);
			this.DateCheckbox.TabIndex = 15;
			this.DateCheckbox.Text = "Created";
			this.DateCheckbox.UseVisualStyleBackColor = true;
			this.DateCheckbox.CheckedChanged += new System.EventHandler(this.DateCheckbox_CheckedChanged);
			// 
			// CancelButton
			// 
			this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelButton.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelButton.Location = new System.Drawing.Point(624, 3);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(76, 24);
			this.CancelButton.TabIndex = 4;
			this.CancelButton.Text = "Close";
			this.CancelButton.UseVisualStyleBackColor = false;
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.CancelButton);
			this.panel3.Controls.Add(this.SearchButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 236);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(706, 30);
			this.panel3.TabIndex = 6;
			// 
			// SearchButton
			// 
			this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SearchButton.BackColor = System.Drawing.SystemColors.Control;
			this.SearchButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SearchButton.Image = global::SecureDeleteWinForms.Properties.Resources.Project3;
			this.SearchButton.Location = new System.Drawing.Point(545, 3);
			this.SearchButton.Name = "SearchButton";
			this.SearchButton.Size = new System.Drawing.Size(78, 24);
			this.SearchButton.TabIndex = 3;
			this.SearchButton.Text = "Search";
			this.SearchButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.SearchButton.UseVisualStyleBackColor = true;
			this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
			// 
			// ErrorTooltip
			// 
			this.ErrorTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
			this.ErrorTooltip.ToolTipTitle = "Message";
			// 
			// FailedNumber
			// 
			this.FailedNumber.Location = new System.Drawing.Point(234, 47);
			this.FailedNumber.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
			this.FailedNumber.Name = "FailedNumber";
			this.FailedNumber.Size = new System.Drawing.Size(90, 20);
			this.FailedNumber.TabIndex = 25;
			// 
			// ErrorNumber
			// 
			this.ErrorNumber.Location = new System.Drawing.Point(234, 72);
			this.ErrorNumber.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
			this.ErrorNumber.Name = "ErrorNumber";
			this.ErrorNumber.Size = new System.Drawing.Size(90, 20);
			this.ErrorNumber.TabIndex = 26;
			// 
			// ReportSearcher
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel3);
			this.Name = "ReportSearcher";
			this.Size = new System.Drawing.Size(706, 266);
			this.Load += new System.EventHandler(this.ReportSearcher_Load);
			this.panel1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.FailedNumber)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ErrorNumber)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button SearchButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox ErrorCombobox;
		private System.Windows.Forms.CheckBox ErrorCheckbox;
		private System.Windows.Forms.ComboBox FailedCombobox;
		private System.Windows.Forms.CheckBox FailedCheckbox;
		private System.Windows.Forms.DateTimePicker DateValue;
		private System.Windows.Forms.ComboBox DateCombobox;
		private System.Windows.Forms.CheckBox DateCheckbox;
		private System.Windows.Forms.ToolTip ErrorTooltip;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox SearchTextbox;
		private System.Windows.Forms.Label PathLabel;
		private System.Windows.Forms.CheckBox MatchCaseCheckbox;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox SearchFailedCheckbox;
		private System.Windows.Forms.CheckBox SearchErrorsCheckbox;
		private System.Windows.Forms.NumericUpDown ErrorNumber;
		private System.Windows.Forms.NumericUpDown FailedNumber;
	}
}
