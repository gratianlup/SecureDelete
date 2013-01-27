namespace SecureDeleteWinForms
{
	partial class CustomActionEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomActionEditor));
			this.DirectoryTextbox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.ArgumentsTextbox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.BrowseButton = new System.Windows.Forms.Button();
			this.PathTextbox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.EnabledCheckbox = new System.Windows.Forms.CheckBox();
			this.TimeLimitCheckbox = new System.Windows.Forms.CheckBox();
			this.TimeLimitValue = new System.Windows.Forms.DateTimePicker();
			this.BrowseButton2 = new System.Windows.Forms.Button();
			this.VariablesMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.label4 = new System.Windows.Forms.Label();
			this.VariablesPanel = new System.Windows.Forms.Panel();
			this.VariablesList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.VariablesToolbar = new System.Windows.Forms.ToolStrip();
			this.AddButton = new System.Windows.Forms.ToolStripButton();
			this.RemoveButton = new System.Windows.Forms.ToolStripSplitButton();
			this.removeSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.VariablesLabel = new System.Windows.Forms.ToolStripLabel();
			this.VariableEditor = new System.Windows.Forms.TextBox();
			this.ErrorTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.VariablesPanel.SuspendLayout();
			this.VariablesToolbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// DirectoryTextbox
			// 
			this.DirectoryTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.DirectoryTextbox.Enabled = false;
			this.DirectoryTextbox.Location = new System.Drawing.Point(68, 106);
			this.DirectoryTextbox.Name = "DirectoryTextbox";
			this.DirectoryTextbox.Size = new System.Drawing.Size(493, 20);
			this.DirectoryTextbox.TabIndex = 18;
			this.DirectoryTextbox.TextChanged += new System.EventHandler(this.DirectoryTextbox_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(5, 109);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(49, 13);
			this.label3.TabIndex = 17;
			this.label3.Text = "Directory";
			// 
			// ArgumentsTextbox
			// 
			this.ArgumentsTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ArgumentsTextbox.Enabled = false;
			this.ArgumentsTextbox.Location = new System.Drawing.Point(68, 80);
			this.ArgumentsTextbox.Name = "ArgumentsTextbox";
			this.ArgumentsTextbox.Size = new System.Drawing.Size(493, 20);
			this.ArgumentsTextbox.TabIndex = 16;
			this.ArgumentsTextbox.TextChanged += new System.EventHandler(this.ArgumentsTextbox_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 83);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 13);
			this.label2.TabIndex = 15;
			this.label2.Text = "Arguments";
			// 
			// BrowseButton
			// 
			this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseButton.Enabled = false;
			this.BrowseButton.Image = ((System.Drawing.Image)(resources.GetObject("BrowseButton.Image")));
			this.BrowseButton.Location = new System.Drawing.Point(564, 52);
			this.BrowseButton.Name = "BrowseButton";
			this.BrowseButton.Size = new System.Drawing.Size(82, 24);
			this.BrowseButton.TabIndex = 14;
			this.BrowseButton.Text = "B&rowse";
			this.BrowseButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.BrowseButton.UseVisualStyleBackColor = true;
			this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
			// 
			// PathTextbox
			// 
			this.PathTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.PathTextbox.Enabled = false;
			this.PathTextbox.Location = new System.Drawing.Point(68, 54);
			this.PathTextbox.Name = "PathTextbox";
			this.PathTextbox.Size = new System.Drawing.Size(493, 20);
			this.PathTextbox.TabIndex = 13;
			this.PathTextbox.TextChanged += new System.EventHandler(this.PathTextbox_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 57);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "Application";
			// 
			// EnabledCheckbox
			// 
			this.EnabledCheckbox.AutoSize = true;
			this.EnabledCheckbox.Enabled = false;
			this.EnabledCheckbox.Location = new System.Drawing.Point(9, 6);
			this.EnabledCheckbox.Name = "EnabledCheckbox";
			this.EnabledCheckbox.Size = new System.Drawing.Size(65, 17);
			this.EnabledCheckbox.TabIndex = 11;
			this.EnabledCheckbox.Text = "Enabled";
			this.EnabledCheckbox.UseVisualStyleBackColor = true;
			this.EnabledCheckbox.CheckedChanged += new System.EventHandler(this.EnabledCheckbox_CheckedChanged);
			// 
			// TimeLimitCheckbox
			// 
			this.TimeLimitCheckbox.AutoSize = true;
			this.TimeLimitCheckbox.Location = new System.Drawing.Point(9, 26);
			this.TimeLimitCheckbox.Name = "TimeLimitCheckbox";
			this.TimeLimitCheckbox.Size = new System.Drawing.Size(118, 17);
			this.TimeLimitCheckbox.TabIndex = 19;
			this.TimeLimitCheckbox.Text = "Limit execution time";
			this.TimeLimitCheckbox.UseVisualStyleBackColor = true;
			this.TimeLimitCheckbox.CheckedChanged += new System.EventHandler(this.TimeLimitCheckbox_CheckedChanged);
			// 
			// TimeLimitValue
			// 
			this.TimeLimitValue.Enabled = false;
			this.TimeLimitValue.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.TimeLimitValue.Location = new System.Drawing.Point(133, 24);
			this.TimeLimitValue.Name = "TimeLimitValue";
			this.TimeLimitValue.ShowUpDown = true;
			this.TimeLimitValue.Size = new System.Drawing.Size(93, 20);
			this.TimeLimitValue.TabIndex = 20;
			this.TimeLimitValue.Value = new System.DateTime(2007, 2, 23, 0, 0, 0, 0);
			this.TimeLimitValue.ValueChanged += new System.EventHandler(this.TimeLimitValue_ValueChanged);
			// 
			// BrowseButton2
			// 
			this.BrowseButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseButton2.Enabled = false;
			this.BrowseButton2.Image = ((System.Drawing.Image)(resources.GetObject("BrowseButton2.Image")));
			this.BrowseButton2.Location = new System.Drawing.Point(564, 104);
			this.BrowseButton2.Name = "BrowseButton2";
			this.BrowseButton2.Size = new System.Drawing.Size(82, 24);
			this.BrowseButton2.TabIndex = 21;
			this.BrowseButton2.Text = "B&rowse";
			this.BrowseButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.BrowseButton2.UseVisualStyleBackColor = true;
			this.BrowseButton2.Click += new System.EventHandler(this.button1_Click);
			// 
			// VariablesMenu
			// 
			this.VariablesMenu.Name = "VariablesMenu";
			this.VariablesMenu.Size = new System.Drawing.Size(61, 4);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(5, 131);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(111, 13);
			this.label4.TabIndex = 24;
			this.label4.Text = "Environment variables";
			// 
			// VariablesPanel
			// 
			this.VariablesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.VariablesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.VariablesPanel.Controls.Add(this.VariablesList);
			this.VariablesPanel.Controls.Add(this.VariablesToolbar);
			this.VariablesPanel.Location = new System.Drawing.Point(9, 148);
			this.VariablesPanel.Name = "VariablesPanel";
			this.VariablesPanel.Size = new System.Drawing.Size(637, 100);
			this.VariablesPanel.TabIndex = 25;
			// 
			// VariablesList
			// 
			this.VariablesList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.VariablesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.VariablesList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VariablesList.FullRowSelect = true;
			this.VariablesList.LabelEdit = true;
			this.VariablesList.Location = new System.Drawing.Point(0, 25);
			this.VariablesList.Name = "VariablesList";
			this.VariablesList.Size = new System.Drawing.Size(635, 73);
			this.VariablesList.SmallImageList = this.imageList1;
			this.VariablesList.TabIndex = 1;
			this.VariablesList.UseCompatibleStateImageBehavior = false;
			this.VariablesList.View = System.Windows.Forms.View.Details;
			this.VariablesList.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.VariablesList_AfterLabelEdit);
			this.VariablesList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.VariablesList_MouseUp);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 213;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Value";
			this.columnHeader2.Width = 339;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "local.ico");
			// 
			// VariablesToolbar
			// 
			this.VariablesToolbar.BackColor = System.Drawing.Color.White;
			this.VariablesToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.VariablesToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddButton,
            this.RemoveButton,
            this.VariablesLabel});
			this.VariablesToolbar.Location = new System.Drawing.Point(0, 0);
			this.VariablesToolbar.Name = "VariablesToolbar";
			this.VariablesToolbar.Padding = new System.Windows.Forms.Padding(4, 0, 1, 0);
			this.VariablesToolbar.Size = new System.Drawing.Size(635, 25);
			this.VariablesToolbar.TabIndex = 0;
			this.VariablesToolbar.Text = "toolStrip1";
			// 
			// AddButton
			// 
			this.AddButton.Image = global::SecureDeleteWinForms.Properties.Resources.add_profile;
			this.AddButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(49, 22);
			this.AddButton.Text = "Add";
			this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
			// 
			// RemoveButton
			// 
			this.RemoveButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSelectedToolStripMenuItem,
            this.removeAllToolStripMenuItem});
			this.RemoveButton.Enabled = false;
			this.RemoveButton.Image = global::SecureDeleteWinForms.Properties.Resources.delete_profile;
			this.RemoveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RemoveButton.Name = "RemoveButton";
			this.RemoveButton.Size = new System.Drawing.Size(82, 22);
			this.RemoveButton.Text = "Remove";
			this.RemoveButton.ButtonClick += new System.EventHandler(this.RemoveButton_ButtonClick);
			// 
			// removeSelectedToolStripMenuItem
			// 
			this.removeSelectedToolStripMenuItem.Name = "removeSelectedToolStripMenuItem";
			this.removeSelectedToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.removeSelectedToolStripMenuItem.Text = "Remove Selected";
			this.removeSelectedToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedToolStripMenuItem_Click);
			// 
			// removeAllToolStripMenuItem
			// 
			this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
			this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.removeAllToolStripMenuItem.Text = "Remove All";
			this.removeAllToolStripMenuItem.Click += new System.EventHandler(this.removeAllToolStripMenuItem_Click);
			// 
			// VariablesLabel
			// 
			this.VariablesLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.VariablesLabel.Name = "VariablesLabel";
			this.VariablesLabel.Size = new System.Drawing.Size(66, 22);
			this.VariablesLabel.Text = "Variables: 0";
			// 
			// VariableEditor
			// 
			this.VariableEditor.Location = new System.Drawing.Point(304, 24);
			this.VariableEditor.Name = "VariableEditor";
			this.VariableEditor.Size = new System.Drawing.Size(185, 20);
			this.VariableEditor.TabIndex = 27;
			this.VariableEditor.Visible = false;
			this.VariableEditor.TextChanged += new System.EventHandler(this.VariableEditor_TextChanged);
			this.VariableEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VariableEditor_KeyDown);
			this.VariableEditor.Leave += new System.EventHandler(this.VariableEditor_Leave);
			// 
			// ErrorTooltip
			// 
			this.ErrorTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
			this.ErrorTooltip.ToolTipTitle = "Message";
			// 
			// CustomActionEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.VariableEditor);
			this.Controls.Add(this.VariablesPanel);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.BrowseButton2);
			this.Controls.Add(this.DirectoryTextbox);
			this.Controls.Add(this.TimeLimitValue);
			this.Controls.Add(this.TimeLimitCheckbox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.ArgumentsTextbox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.BrowseButton);
			this.Controls.Add(this.PathTextbox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.EnabledCheckbox);
			this.Name = "CustomActionEditor";
			this.Size = new System.Drawing.Size(653, 258);
			this.VariablesPanel.ResumeLayout(false);
			this.VariablesPanel.PerformLayout();
			this.VariablesToolbar.ResumeLayout(false);
			this.VariablesToolbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox DirectoryTextbox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox ArgumentsTextbox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button BrowseButton;
		private System.Windows.Forms.TextBox PathTextbox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox EnabledCheckbox;
		private System.Windows.Forms.CheckBox TimeLimitCheckbox;
		private System.Windows.Forms.DateTimePicker TimeLimitValue;
		private System.Windows.Forms.Button BrowseButton2;
		private System.Windows.Forms.ContextMenuStrip VariablesMenu;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel VariablesPanel;
		private System.Windows.Forms.ToolStrip VariablesToolbar;
		private System.Windows.Forms.ToolStripButton AddButton;
		private System.Windows.Forms.ToolStripSplitButton RemoveButton;
		private System.Windows.Forms.ToolStripLabel VariablesLabel;
		private System.Windows.Forms.ListView VariablesList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ToolStripMenuItem removeSelectedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.TextBox VariableEditor;
		private System.Windows.Forms.ToolTip ErrorTooltip;
	}
}
