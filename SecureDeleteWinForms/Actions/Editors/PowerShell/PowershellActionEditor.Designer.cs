namespace SecureDeleteWinForms
{
	partial class PowershellActionEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PowershellActionEditor));
			this.EnabledCheckbox = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.EditButton = new System.Windows.Forms.Button();
			this.CustomOptionbox = new System.Windows.Forms.RadioButton();
			this.TimeLimitValue = new System.Windows.Forms.DateTimePicker();
			this.TimeLimitCheckbox = new System.Windows.Forms.CheckBox();
			this.PluginButton = new System.Windows.Forms.Button();
			this.PluginLabel = new System.Windows.Forms.Label();
			this.PluginFolderCheckbox = new System.Windows.Forms.CheckBox();
			this.PluginFolderTextobx = new System.Windows.Forms.TextBox();
			this.LocationButton = new System.Windows.Forms.Button();
			this.LocationTextBox = new System.Windows.Forms.TextBox();
			this.LocationLabel = new System.Windows.Forms.Label();
			this.FileOptionbox = new System.Windows.Forms.RadioButton();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// EnabledCheckbox
			// 
			this.EnabledCheckbox.AutoSize = true;
			this.EnabledCheckbox.Location = new System.Drawing.Point(9, 6);
			this.EnabledCheckbox.Name = "EnabledCheckbox";
			this.EnabledCheckbox.Size = new System.Drawing.Size(65, 17);
			this.EnabledCheckbox.TabIndex = 20;
			this.EnabledCheckbox.Text = "Enabled";
			this.EnabledCheckbox.UseVisualStyleBackColor = true;
			this.EnabledCheckbox.CheckedChanged += new System.EventHandler(this.EnabledCheckbox_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.LocationButton);
			this.groupBox2.Controls.Add(this.LocationTextBox);
			this.groupBox2.Controls.Add(this.LocationLabel);
			this.groupBox2.Controls.Add(this.FileOptionbox);
			this.groupBox2.Controls.Add(this.EditButton);
			this.groupBox2.Controls.Add(this.CustomOptionbox);
			this.groupBox2.Location = new System.Drawing.Point(9, 97);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(812, 136);
			this.groupBox2.TabIndex = 26;
			this.groupBox2.TabStop = false;
			// 
			// EditButton
			// 
			this.EditButton.Enabled = false;
			this.EditButton.Image = global::SecureDeleteWinForms.Properties.Resources.pen;
			this.EditButton.Location = new System.Drawing.Point(6, 35);
			this.EditButton.Name = "EditButton";
			this.EditButton.Size = new System.Drawing.Size(137, 23);
			this.EditButton.TabIndex = 31;
			this.EditButton.Text = "Edit Custom Script";
			this.EditButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.EditButton.UseVisualStyleBackColor = true;
			this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
			// 
			// CustomOptionbox
			// 
			this.CustomOptionbox.AutoSize = true;
			this.CustomOptionbox.Location = new System.Drawing.Point(6, 12);
			this.CustomOptionbox.Name = "CustomOptionbox";
			this.CustomOptionbox.Size = new System.Drawing.Size(109, 17);
			this.CustomOptionbox.TabIndex = 25;
			this.CustomOptionbox.TabStop = true;
			this.CustomOptionbox.Text = "Use custom script";
			this.CustomOptionbox.UseVisualStyleBackColor = true;
			this.CustomOptionbox.CheckedChanged += new System.EventHandler(this.CustomOptionbox_CheckedChanged);
			// 
			// TimeLimitValue
			// 
			this.TimeLimitValue.Enabled = false;
			this.TimeLimitValue.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.TimeLimitValue.Location = new System.Drawing.Point(133, 24);
			this.TimeLimitValue.Name = "TimeLimitValue";
			this.TimeLimitValue.ShowUpDown = true;
			this.TimeLimitValue.Size = new System.Drawing.Size(93, 20);
			this.TimeLimitValue.TabIndex = 28;
			this.TimeLimitValue.Value = new System.DateTime(2007, 2, 23, 0, 0, 0, 0);
			this.TimeLimitValue.ValueChanged += new System.EventHandler(this.TimeLimitValue_ValueChanged);
			// 
			// TimeLimitCheckbox
			// 
			this.TimeLimitCheckbox.AutoSize = true;
			this.TimeLimitCheckbox.Location = new System.Drawing.Point(9, 26);
			this.TimeLimitCheckbox.Name = "TimeLimitCheckbox";
			this.TimeLimitCheckbox.Size = new System.Drawing.Size(118, 17);
			this.TimeLimitCheckbox.TabIndex = 27;
			this.TimeLimitCheckbox.Text = "Limit execution time";
			this.TimeLimitCheckbox.UseVisualStyleBackColor = true;
			this.TimeLimitCheckbox.CheckedChanged += new System.EventHandler(this.TimeLimitCheckbox_CheckedChanged);
			// 
			// PluginButton
			// 
			this.PluginButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.PluginButton.Enabled = false;
			this.PluginButton.Image = ((System.Drawing.Image)(resources.GetObject("PluginButton.Image")));
			this.PluginButton.Location = new System.Drawing.Point(720, 67);
			this.PluginButton.Name = "PluginButton";
			this.PluginButton.Size = new System.Drawing.Size(95, 24);
			this.PluginButton.TabIndex = 34;
			this.PluginButton.Text = "Browse";
			this.PluginButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.PluginButton.UseVisualStyleBackColor = true;
			// 
			// PluginLabel
			// 
			this.PluginLabel.AutoSize = true;
			this.PluginLabel.Enabled = false;
			this.PluginLabel.Location = new System.Drawing.Point(25, 73);
			this.PluginLabel.Name = "PluginLabel";
			this.PluginLabel.Size = new System.Drawing.Size(48, 13);
			this.PluginLabel.TabIndex = 33;
			this.PluginLabel.Text = "Location";
			// 
			// PluginFolderCheckbox
			// 
			this.PluginFolderCheckbox.AutoSize = true;
			this.PluginFolderCheckbox.Enabled = false;
			this.PluginFolderCheckbox.Location = new System.Drawing.Point(9, 47);
			this.PluginFolderCheckbox.Name = "PluginFolderCheckbox";
			this.PluginFolderCheckbox.Size = new System.Drawing.Size(335, 17);
			this.PluginFolderCheckbox.TabIndex = 32;
			this.PluginFolderCheckbox.Text = "Load exposed objects from libraries found in the folowing directory";
			this.PluginFolderCheckbox.UseVisualStyleBackColor = true;
			this.PluginFolderCheckbox.CheckedChanged += new System.EventHandler(this.PluginFolderCheckbox_CheckedChanged);
			// 
			// PluginFolderTextobx
			// 
			this.PluginFolderTextobx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.PluginFolderTextobx.Enabled = false;
			this.PluginFolderTextobx.Location = new System.Drawing.Point(79, 69);
			this.PluginFolderTextobx.Name = "PluginFolderTextobx";
			this.PluginFolderTextobx.Size = new System.Drawing.Size(638, 20);
			this.PluginFolderTextobx.TabIndex = 31;
			// 
			// LocationButton
			// 
			this.LocationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.LocationButton.Enabled = false;
			this.LocationButton.Image = ((System.Drawing.Image)(resources.GetObject("LocationButton.Image")));
			this.LocationButton.Location = new System.Drawing.Point(711, 101);
			this.LocationButton.Name = "LocationButton";
			this.LocationButton.Size = new System.Drawing.Size(95, 24);
			this.LocationButton.TabIndex = 35;
			this.LocationButton.Text = "Browse";
			this.LocationButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.LocationButton.UseVisualStyleBackColor = true;
			// 
			// LocationTextBox
			// 
			this.LocationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.LocationTextBox.Enabled = false;
			this.LocationTextBox.Location = new System.Drawing.Point(76, 103);
			this.LocationTextBox.Name = "LocationTextBox";
			this.LocationTextBox.Size = new System.Drawing.Size(632, 20);
			this.LocationTextBox.TabIndex = 34;
			// 
			// LocationLabel
			// 
			this.LocationLabel.AutoSize = true;
			this.LocationLabel.Enabled = false;
			this.LocationLabel.Location = new System.Drawing.Point(22, 106);
			this.LocationLabel.Name = "LocationLabel";
			this.LocationLabel.Size = new System.Drawing.Size(48, 13);
			this.LocationLabel.TabIndex = 33;
			this.LocationLabel.Text = "Location";
			// 
			// FileOptionbox
			// 
			this.FileOptionbox.AutoSize = true;
			this.FileOptionbox.Location = new System.Drawing.Point(6, 80);
			this.FileOptionbox.Name = "FileOptionbox";
			this.FileOptionbox.Size = new System.Drawing.Size(116, 17);
			this.FileOptionbox.TabIndex = 32;
			this.FileOptionbox.TabStop = true;
			this.FileOptionbox.Text = "Load script from file";
			this.FileOptionbox.UseVisualStyleBackColor = true;
			this.FileOptionbox.CheckedChanged += new System.EventHandler(this.FileOptionbox_CheckedChanged);
			// 
			// PowershellActionEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.PluginButton);
			this.Controls.Add(this.PluginLabel);
			this.Controls.Add(this.PluginFolderCheckbox);
			this.Controls.Add(this.PluginFolderTextobx);
			this.Controls.Add(this.TimeLimitValue);
			this.Controls.Add(this.TimeLimitCheckbox);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.EnabledCheckbox);
			this.Name = "PowershellActionEditor";
			this.Size = new System.Drawing.Size(831, 240);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox EnabledCheckbox;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton CustomOptionbox;
		private System.Windows.Forms.DateTimePicker TimeLimitValue;
		private System.Windows.Forms.CheckBox TimeLimitCheckbox;
		private System.Windows.Forms.Button EditButton;
		private System.Windows.Forms.Button PluginButton;
		private System.Windows.Forms.Label PluginLabel;
		private System.Windows.Forms.CheckBox PluginFolderCheckbox;
		private System.Windows.Forms.TextBox PluginFolderTextobx;
		private System.Windows.Forms.Button LocationButton;
		private System.Windows.Forms.TextBox LocationTextBox;
		private System.Windows.Forms.Label LocationLabel;
		private System.Windows.Forms.RadioButton FileOptionbox;
	}
}
