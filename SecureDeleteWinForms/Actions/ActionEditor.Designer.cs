namespace SecureDeleteWinForms
{
	partial class ActionEditor
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.ActionList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.customActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.logoffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.sendMailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.powershellScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RemoveButton = new System.Windows.Forms.ToolStripSplitButton();
			this.removeSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeDisabledActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.UpButton = new System.Windows.Forms.ToolStripButton();
			this.DownButton = new System.Windows.Forms.ToolStripButton();
			this.ActionCountLabel = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.TemplateLabel = new System.Windows.Forms.ToolStripLabel();
			this.TemplateList = new System.Windows.Forms.ToolStripComboBox();
			this.LoadTemplateButton = new System.Windows.Forms.ToolStripButton();
			this.RemoveTemplateButton = new System.Windows.Forms.ToolStripSplitButton();
			this.deleteSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteAllTemplatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SaveTemplateButton = new System.Windows.Forms.ToolStripButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.EditorHost = new System.Windows.Forms.Panel();
			this.ToolHeader = new System.Windows.Forms.Panel();
			this.ToolHeaderLabel = new System.Windows.Forms.Label();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.ToolHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.ActionList);
			this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.panel1);
			this.splitContainer1.Size = new System.Drawing.Size(654, 406);
			this.splitContainer1.SplitterDistance = 326;
			this.splitContainer1.SplitterWidth = 2;
			this.splitContainer1.TabIndex = 3;
			// 
			// ActionList
			// 
			this.ActionList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.ActionList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ActionList.FullRowSelect = true;
			this.ActionList.HideSelection = false;
			this.ActionList.Location = new System.Drawing.Point(0, 25);
			this.ActionList.Name = "ActionList";
			this.ActionList.Size = new System.Drawing.Size(654, 301);
			this.ActionList.TabIndex = 3;
			this.ActionList.UseCompatibleStateImageBehavior = false;
			this.ActionList.View = System.Windows.Forms.View.Details;
			this.ActionList.SelectedIndexChanged += new System.EventHandler(this.ActionList_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Type";
			this.columnHeader1.Width = 120;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Action";
			this.columnHeader2.Width = 300;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Enabled";
			this.columnHeader3.Width = 80;
			// 
			// toolStrip1
			// 
			this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1,
            this.RemoveButton,
            this.toolStripSeparator1,
            this.UpButton,
            this.DownButton,
            this.ActionCountLabel,
            this.toolStripSeparator3,
            this.TemplateLabel,
            this.TemplateList,
            this.LoadTemplateButton,
            this.RemoveTemplateButton,
            this.SaveTemplateButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 1, 0);
			this.toolStrip1.Size = new System.Drawing.Size(654, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customActionToolStripMenuItem,
            this.toolStripSeparator5,
            this.shutdownToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.logoffToolStripMenuItem,
            this.toolStripSeparator2,
            this.sendMailToolStripMenuItem,
            this.powershellScriptToolStripMenuItem});
			this.toolStripSplitButton1.Image = global::SecureDeleteWinForms.Properties.Resources.add_profile;
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size(61, 22);
			this.toolStripSplitButton1.Text = "Add";
			this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
			// 
			// customActionToolStripMenuItem
			// 
			this.customActionToolStripMenuItem.Image = global::SecureDeleteWinForms.Properties.Resources.run;
			this.customActionToolStripMenuItem.Name = "customActionToolStripMenuItem";
			this.customActionToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.customActionToolStripMenuItem.Text = "Custom Action";
			this.customActionToolStripMenuItem.Click += new System.EventHandler(this.customActionToolStripMenuItem_Click_2);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(161, 6);
			// 
			// shutdownToolStripMenuItem
			// 
			this.shutdownToolStripMenuItem.Name = "shutdownToolStripMenuItem";
			this.shutdownToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.shutdownToolStripMenuItem.Text = "Shutdown";
			this.shutdownToolStripMenuItem.Click += new System.EventHandler(this.shutdownToolStripMenuItem_Click_2);
			// 
			// restartToolStripMenuItem
			// 
			this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
			this.restartToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.restartToolStripMenuItem.Text = "Restart";
			this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click_2);
			// 
			// logoffToolStripMenuItem
			// 
			this.logoffToolStripMenuItem.Name = "logoffToolStripMenuItem";
			this.logoffToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.logoffToolStripMenuItem.Text = "Logoff";
			this.logoffToolStripMenuItem.Click += new System.EventHandler(this.logoffToolStripMenuItem_Click_2);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(161, 6);
			// 
			// sendMailToolStripMenuItem
			// 
			this.sendMailToolStripMenuItem.Image = global::SecureDeleteWinForms.Properties.Resources.mail;
			this.sendMailToolStripMenuItem.Name = "sendMailToolStripMenuItem";
			this.sendMailToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.sendMailToolStripMenuItem.Text = "Send Mail";
			this.sendMailToolStripMenuItem.Click += new System.EventHandler(this.sendMailToolStripMenuItem_Click_1);
			// 
			// powershellScriptToolStripMenuItem
			// 
			this.powershellScriptToolStripMenuItem.Image = global::SecureDeleteWinForms.Properties.Resources.powershell_small;
			this.powershellScriptToolStripMenuItem.Name = "powershellScriptToolStripMenuItem";
			this.powershellScriptToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.powershellScriptToolStripMenuItem.Text = "Powershell Script";
			this.powershellScriptToolStripMenuItem.Click += new System.EventHandler(this.powershellScriptToolStripMenuItem_Click);
			// 
			// RemoveButton
			// 
			this.RemoveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RemoveButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSelectedToolStripMenuItem,
            this.removeDisabledActionsToolStripMenuItem,
            this.toolStripSeparator4,
            this.removeAllToolStripMenuItem});
			this.RemoveButton.Enabled = false;
			this.RemoveButton.Image = global::SecureDeleteWinForms.Properties.Resources.delete_profile;
			this.RemoveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RemoveButton.Name = "RemoveButton";
			this.RemoveButton.Size = new System.Drawing.Size(32, 22);
			this.RemoveButton.Text = "toolStripSplitButton2";
			this.RemoveButton.ButtonClick += new System.EventHandler(this.RemoveButton_ButtonClick);
			// 
			// removeSelectedToolStripMenuItem
			// 
			this.removeSelectedToolStripMenuItem.Name = "removeSelectedToolStripMenuItem";
			this.removeSelectedToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.removeSelectedToolStripMenuItem.Text = "Remove Selected";
			this.removeSelectedToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedToolStripMenuItem_Click_1);
			// 
			// removeDisabledActionsToolStripMenuItem
			// 
			this.removeDisabledActionsToolStripMenuItem.Name = "removeDisabledActionsToolStripMenuItem";
			this.removeDisabledActionsToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.removeDisabledActionsToolStripMenuItem.Text = "Remove Disabled Actions";
			this.removeDisabledActionsToolStripMenuItem.Click += new System.EventHandler(this.removeDisabledActionsToolStripMenuItem_Click_1);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(205, 6);
			// 
			// removeAllToolStripMenuItem
			// 
			this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
			this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.removeAllToolStripMenuItem.Text = "Remove All";
			this.removeAllToolStripMenuItem.Click += new System.EventHandler(this.removeAllToolStripMenuItem_Click_1);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// UpButton
			// 
			this.UpButton.Enabled = false;
			this.UpButton.Image = global::SecureDeleteWinForms.Properties.Resources.FillUpHS;
			this.UpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.UpButton.Name = "UpButton";
			this.UpButton.Size = new System.Drawing.Size(42, 22);
			this.UpButton.Text = "Up";
			this.UpButton.Click += new System.EventHandler(this.UpButton_Click_1);
			// 
			// DownButton
			// 
			this.DownButton.Enabled = false;
			this.DownButton.Image = global::SecureDeleteWinForms.Properties.Resources.FillDownHS;
			this.DownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.DownButton.Name = "DownButton";
			this.DownButton.Size = new System.Drawing.Size(58, 22);
			this.DownButton.Text = "Down";
			this.DownButton.Click += new System.EventHandler(this.DownButton_Click_1);
			// 
			// ActionCountLabel
			// 
			this.ActionCountLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.ActionCountLabel.Name = "ActionCountLabel";
			this.ActionCountLabel.Size = new System.Drawing.Size(59, 22);
			this.ActionCountLabel.Text = "Actions: 0";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// TemplateLabel
			// 
			this.TemplateLabel.Name = "TemplateLabel";
			this.TemplateLabel.Size = new System.Drawing.Size(62, 22);
			this.TemplateLabel.Text = "Templates";
			// 
			// TemplateList
			// 
			this.TemplateList.BackColor = System.Drawing.SystemColors.Window;
			this.TemplateList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TemplateList.Name = "TemplateList";
			this.TemplateList.Size = new System.Drawing.Size(151, 25);
			this.TemplateList.SelectedIndexChanged += new System.EventHandler(this.TemplateList_SelectedIndexChanged_1);
			// 
			// LoadTemplateButton
			// 
			this.LoadTemplateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.LoadTemplateButton.Image = global::SecureDeleteWinForms.Properties.Resources.load_profile;
			this.LoadTemplateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.LoadTemplateButton.Name = "LoadTemplateButton";
			this.LoadTemplateButton.Size = new System.Drawing.Size(23, 22);
			this.LoadTemplateButton.Text = "toolStripButton2";
			this.LoadTemplateButton.ToolTipText = "Load selected template";
			this.LoadTemplateButton.Click += new System.EventHandler(this.LoadTemplateButton_Click_1);
			// 
			// RemoveTemplateButton
			// 
			this.RemoveTemplateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RemoveTemplateButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteSelectedToolStripMenuItem,
            this.deleteAllTemplatesToolStripMenuItem});
			this.RemoveTemplateButton.Image = global::SecureDeleteWinForms.Properties.Resources.delete_profile;
			this.RemoveTemplateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RemoveTemplateButton.Name = "RemoveTemplateButton";
			this.RemoveTemplateButton.Size = new System.Drawing.Size(32, 22);
			this.RemoveTemplateButton.Text = "toolStripButton3";
			this.RemoveTemplateButton.ToolTipText = "Delete selected template";
			// 
			// deleteSelectedToolStripMenuItem
			// 
			this.deleteSelectedToolStripMenuItem.Name = "deleteSelectedToolStripMenuItem";
			this.deleteSelectedToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.deleteSelectedToolStripMenuItem.Text = "Delete Selected Template";
			this.deleteSelectedToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedToolStripMenuItem_Click_1);
			// 
			// deleteAllTemplatesToolStripMenuItem
			// 
			this.deleteAllTemplatesToolStripMenuItem.Name = "deleteAllTemplatesToolStripMenuItem";
			this.deleteAllTemplatesToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.deleteAllTemplatesToolStripMenuItem.Text = "Delete All Templates";
			this.deleteAllTemplatesToolStripMenuItem.Click += new System.EventHandler(this.deleteAllTemplatesToolStripMenuItem_Click_1);
			// 
			// SaveTemplateButton
			// 
			this.SaveTemplateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.SaveTemplateButton.Image = global::SecureDeleteWinForms.Properties.Resources.add_profile;
			this.SaveTemplateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.SaveTemplateButton.Name = "SaveTemplateButton";
			this.SaveTemplateButton.Size = new System.Drawing.Size(23, 22);
			this.SaveTemplateButton.Text = "toolStripButton4";
			this.SaveTemplateButton.ToolTipText = "Save filters as template";
			this.SaveTemplateButton.Click += new System.EventHandler(this.SaveTemplateButton_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.EditorHost);
			this.panel1.Controls.Add(this.ToolHeader);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(654, 78);
			this.panel1.TabIndex = 2;
			// 
			// EditorHost
			// 
			this.EditorHost.AutoScroll = true;
			this.EditorHost.BackColor = System.Drawing.SystemColors.Control;
			this.EditorHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EditorHost.Location = new System.Drawing.Point(0, 24);
			this.EditorHost.Name = "EditorHost";
			this.EditorHost.Size = new System.Drawing.Size(654, 54);
			this.EditorHost.TabIndex = 3;
			// 
			// ToolHeader
			// 
			this.ToolHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.ToolHeader.Controls.Add(this.ToolHeaderLabel);
			this.ToolHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolHeader.Location = new System.Drawing.Point(0, 0);
			this.ToolHeader.Name = "ToolHeader";
			this.ToolHeader.Size = new System.Drawing.Size(654, 24);
			this.ToolHeader.TabIndex = 2;
			// 
			// ToolHeaderLabel
			// 
			this.ToolHeaderLabel.AutoSize = true;
			this.ToolHeaderLabel.ForeColor = System.Drawing.Color.White;
			this.ToolHeaderLabel.Location = new System.Drawing.Point(5, 5);
			this.ToolHeaderLabel.Name = "ToolHeaderLabel";
			this.ToolHeaderLabel.Size = new System.Drawing.Size(87, 13);
			this.ToolHeaderLabel.TabIndex = 1;
			this.ToolHeaderLabel.Text = "Action Properties";
			// 
			// ActionEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "ActionEditor";
			this.Size = new System.Drawing.Size(654, 406);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ToolHeader.ResumeLayout(false);
			this.ToolHeader.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListView ActionList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
		private System.Windows.Forms.ToolStripMenuItem customActionToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem shutdownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem logoffToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem sendMailToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton RemoveButton;
		private System.Windows.Forms.ToolStripMenuItem removeSelectedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeDisabledActionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton UpButton;
		private System.Windows.Forms.ToolStripButton DownButton;
		private System.Windows.Forms.ToolStripLabel ActionCountLabel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripLabel TemplateLabel;
		private System.Windows.Forms.ToolStripComboBox TemplateList;
		private System.Windows.Forms.ToolStripButton LoadTemplateButton;
		private System.Windows.Forms.ToolStripSplitButton RemoveTemplateButton;
		private System.Windows.Forms.ToolStripMenuItem deleteSelectedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteAllTemplatesToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton SaveTemplateButton;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel EditorHost;
		private System.Windows.Forms.Panel ToolHeader;
		private System.Windows.Forms.Label ToolHeaderLabel;
		private System.Windows.Forms.ToolStripMenuItem powershellScriptToolStripMenuItem;

	}
}
