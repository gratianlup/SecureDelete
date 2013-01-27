namespace SecureDeleteWinForms.WipeTools
{
	partial class FilterBox
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
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.AddButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.sizeFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dateFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.attributeFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.pictureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.audioFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RemoveButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.removeDisabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.AdvancedButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.TemplatesLabel = new System.Windows.Forms.ToolStripLabel();
			this.TemplateList = new System.Windows.Forms.ToolStripComboBox();
			this.LoadTemplateButton = new System.Windows.Forms.ToolStripButton();
			this.RemoveTemplateButton = new System.Windows.Forms.ToolStripSplitButton();
			this.deleteSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteAllTemplatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AddTemplateButton = new System.Windows.Forms.ToolStripButton();
			this.FilterNumberLabel = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.DefaultNamesButton = new System.Windows.Forms.ToolStripButton();
			this.FilterHost = new System.Windows.Forms.Panel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.InvalidLabel = new System.Windows.Forms.Label();
			this.ExpressionLabel = new System.Windows.Forms.Label();
			this.ExpressionText = new System.Windows.Forms.RichTextBox();
			this.toolStrip1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
			this.toolStrip1.CanOverflow = false;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddButton,
            this.RemoveButton,
            this.toolStripSeparator1,
            this.AdvancedButton,
            this.toolStripSeparator2,
            this.TemplatesLabel,
            this.TemplateList,
            this.LoadTemplateButton,
            this.RemoveTemplateButton,
            this.AddTemplateButton,
            this.FilterNumberLabel,
            this.toolStripSeparator3,
            this.DefaultNamesButton});
			this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 1, 0);
			this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.toolStrip1.Size = new System.Drawing.Size(811, 25);
			this.toolStrip1.Stretch = true;
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// AddButton
			// 
			this.AddButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sizeFilterToolStripMenuItem,
            this.dateFilterToolStripMenuItem,
            this.attributeFilterToolStripMenuItem,
            this.toolStripSeparator4,
            this.pictureToolStripMenuItem,
            this.audioFilterToolStripMenuItem});
			this.AddButton.Image = global::SecureDeleteWinForms.Properties.Resources.add_profile;
			this.AddButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(58, 22);
			this.AddButton.Text = "Add";
			// 
			// sizeFilterToolStripMenuItem
			// 
			this.sizeFilterToolStripMenuItem.Name = "sizeFilterToolStripMenuItem";
			this.sizeFilterToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.sizeFilterToolStripMenuItem.Text = "Size Filter";
			this.sizeFilterToolStripMenuItem.Click += new System.EventHandler(this.sizeFilterToolStripMenuItem_Click);
			// 
			// dateFilterToolStripMenuItem
			// 
			this.dateFilterToolStripMenuItem.Name = "dateFilterToolStripMenuItem";
			this.dateFilterToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.dateFilterToolStripMenuItem.Text = "Date Filter";
			this.dateFilterToolStripMenuItem.Click += new System.EventHandler(this.dateFilterToolStripMenuItem_Click);
			// 
			// attributeFilterToolStripMenuItem
			// 
			this.attributeFilterToolStripMenuItem.Name = "attributeFilterToolStripMenuItem";
			this.attributeFilterToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.attributeFilterToolStripMenuItem.Text = "Attribute Filter";
			this.attributeFilterToolStripMenuItem.Click += new System.EventHandler(this.attributeFilterToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(147, 6);
			// 
			// pictureToolStripMenuItem
			// 
			this.pictureToolStripMenuItem.Name = "pictureToolStripMenuItem";
			this.pictureToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.pictureToolStripMenuItem.Text = "Image Filter";
			this.pictureToolStripMenuItem.Click += new System.EventHandler(this.pictureToolStripMenuItem_Click);
			// 
			// audioFilterToolStripMenuItem
			// 
			this.audioFilterToolStripMenuItem.Enabled = false;
			this.audioFilterToolStripMenuItem.Name = "audioFilterToolStripMenuItem";
			this.audioFilterToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.audioFilterToolStripMenuItem.Text = "Audio Filter";
			// 
			// RemoveButton
			// 
			this.RemoveButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeDisabledToolStripMenuItem,
            this.removeAllToolStripMenuItem});
			this.RemoveButton.Image = global::SecureDeleteWinForms.Properties.Resources.delete_profile;
			this.RemoveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RemoveButton.Name = "RemoveButton";
			this.RemoveButton.Size = new System.Drawing.Size(79, 22);
			this.RemoveButton.Text = "Remove";
			// 
			// removeDisabledToolStripMenuItem
			// 
			this.removeDisabledToolStripMenuItem.Name = "removeDisabledToolStripMenuItem";
			this.removeDisabledToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.removeDisabledToolStripMenuItem.Text = "Remove Disabled";
			this.removeDisabledToolStripMenuItem.Click += new System.EventHandler(this.removeDisabledToolStripMenuItem_Click);
			// 
			// removeAllToolStripMenuItem
			// 
			this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
			this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.removeAllToolStripMenuItem.Text = "Remove All";
			this.removeAllToolStripMenuItem.Click += new System.EventHandler(this.removeAllToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// AdvancedButton
			// 
			this.AdvancedButton.CheckOnClick = true;
			this.AdvancedButton.Image = global::SecureDeleteWinForms.Properties.Resources.FormulaEvaluatorHS;
			this.AdvancedButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.AdvancedButton.Name = "AdvancedButton";
			this.AdvancedButton.Size = new System.Drawing.Size(80, 22);
			this.AdvancedButton.Text = "Advanced";
			this.AdvancedButton.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// TemplatesLabel
			// 
			this.TemplatesLabel.Name = "TemplatesLabel";
			this.TemplatesLabel.Size = new System.Drawing.Size(62, 22);
			this.TemplatesLabel.Text = "Templates";
			// 
			// TemplateList
			// 
			this.TemplateList.BackColor = System.Drawing.SystemColors.Window;
			this.TemplateList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TemplateList.Name = "TemplateList";
			this.TemplateList.Size = new System.Drawing.Size(151, 25);
			this.TemplateList.SelectedIndexChanged += new System.EventHandler(this.TemplateList_SelectedIndexChanged);
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
			this.LoadTemplateButton.Click += new System.EventHandler(this.LoadTemplateButton_Click);
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
			this.RemoveTemplateButton.ButtonClick += new System.EventHandler(this.toolStripButton3_ButtonClick);
			// 
			// deleteSelectedToolStripMenuItem
			// 
			this.deleteSelectedToolStripMenuItem.Name = "deleteSelectedToolStripMenuItem";
			this.deleteSelectedToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.deleteSelectedToolStripMenuItem.Text = "Delete Selected Template";
			this.deleteSelectedToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedToolStripMenuItem_Click);
			// 
			// deleteAllTemplatesToolStripMenuItem
			// 
			this.deleteAllTemplatesToolStripMenuItem.Name = "deleteAllTemplatesToolStripMenuItem";
			this.deleteAllTemplatesToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.deleteAllTemplatesToolStripMenuItem.Text = "Delete All Templates";
			this.deleteAllTemplatesToolStripMenuItem.Click += new System.EventHandler(this.deleteAllTemplatesToolStripMenuItem_Click);
			// 
			// AddTemplateButton
			// 
			this.AddTemplateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.AddTemplateButton.Image = global::SecureDeleteWinForms.Properties.Resources.add_profile;
			this.AddTemplateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.AddTemplateButton.Name = "AddTemplateButton";
			this.AddTemplateButton.Size = new System.Drawing.Size(23, 22);
			this.AddTemplateButton.Text = "toolStripButton4";
			this.AddTemplateButton.ToolTipText = "Save filters as template";
			this.AddTemplateButton.Click += new System.EventHandler(this.TemplateAddButton_Click);
			// 
			// FilterNumberLabel
			// 
			this.FilterNumberLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.FilterNumberLabel.Enabled = false;
			this.FilterNumberLabel.Name = "FilterNumberLabel";
			this.FilterNumberLabel.Size = new System.Drawing.Size(50, 22);
			this.FilterNumberLabel.Text = "Filters: 0";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// DefaultNamesButton
			// 
			this.DefaultNamesButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.DefaultNamesButton.Image = global::SecureDeleteWinForms.Properties.Resources.Book_angleHS;
			this.DefaultNamesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.DefaultNamesButton.Name = "DefaultNamesButton";
			this.DefaultNamesButton.Size = new System.Drawing.Size(121, 22);
			this.DefaultNamesButton.Text = "Set default names";
			this.DefaultNamesButton.Visible = false;
			this.DefaultNamesButton.Click += new System.EventHandler(this.toolStripButton1_Click_1);
			// 
			// FilterHost
			// 
			this.FilterHost.AutoScroll = true;
			this.FilterHost.BackColor = System.Drawing.SystemColors.Window;
			this.FilterHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FilterHost.Location = new System.Drawing.Point(0, 0);
			this.FilterHost.Name = "FilterHost";
			this.FilterHost.Size = new System.Drawing.Size(811, 195);
			this.FilterHost.TabIndex = 1;
			// 
			// splitContainer1
			// 
			this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(0, 25);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.FilterHost);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.splitContainer1.Panel2.Controls.Add(this.InvalidLabel);
			this.splitContainer1.Panel2.Controls.Add(this.ExpressionLabel);
			this.splitContainer1.Panel2.Controls.Add(this.ExpressionText);
			this.splitContainer1.Panel2Collapsed = true;
			this.splitContainer1.Size = new System.Drawing.Size(811, 195);
			this.splitContainer1.SplitterDistance = 148;
			this.splitContainer1.SplitterWidth = 2;
			this.splitContainer1.TabIndex = 2;
			// 
			// InvalidLabel
			// 
			this.InvalidLabel.AutoSize = true;
			this.InvalidLabel.ForeColor = System.Drawing.Color.Gold;
			this.InvalidLabel.Location = new System.Drawing.Point(2, 19);
			this.InvalidLabel.Name = "InvalidLabel";
			this.InvalidLabel.Size = new System.Drawing.Size(49, 13);
			this.InvalidLabel.TabIndex = 2;
			this.InvalidLabel.Text = "INVALID";
			this.InvalidLabel.Visible = false;
			// 
			// ExpressionLabel
			// 
			this.ExpressionLabel.AutoSize = true;
			this.ExpressionLabel.ForeColor = System.Drawing.Color.White;
			this.ExpressionLabel.Location = new System.Drawing.Point(3, 2);
			this.ExpressionLabel.Name = "ExpressionLabel";
			this.ExpressionLabel.Size = new System.Drawing.Size(58, 13);
			this.ExpressionLabel.TabIndex = 1;
			this.ExpressionLabel.Text = "Expression";
			// 
			// ExpressionText
			// 
			this.ExpressionText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ExpressionText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ExpressionText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ExpressionText.Location = new System.Drawing.Point(67, 0);
			this.ExpressionText.Name = "ExpressionText";
			this.ExpressionText.Size = new System.Drawing.Size(744, 45);
			this.ExpressionText.TabIndex = 0;
			this.ExpressionText.Text = "";
			this.ExpressionText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExpressionText_KeyDown);
			this.ExpressionText.Leave += new System.EventHandler(this.ExpressionText_Leave);
			this.ExpressionText.TextChanged += new System.EventHandler(this.Expression_TextChanged);
			// 
			// FilterBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "FilterBox";
			this.Size = new System.Drawing.Size(811, 220);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripDropDownButton AddButton;
		private System.Windows.Forms.ToolStripMenuItem sizeFilterToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem attributeFilterToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton AdvancedButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.Panel FilterHost;
		private System.Windows.Forms.ToolStripDropDownButton RemoveButton;
		private System.Windows.Forms.ToolStripMenuItem removeDisabledToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripLabel FilterNumberLabel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripLabel TemplatesLabel;
		private System.Windows.Forms.ToolStripComboBox TemplateList;
		private System.Windows.Forms.ToolStripButton LoadTemplateButton;
		private System.Windows.Forms.ToolStripButton AddTemplateButton;
		private System.Windows.Forms.ToolStripMenuItem dateFilterToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton RemoveTemplateButton;
		private System.Windows.Forms.ToolStripMenuItem deleteSelectedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteAllTemplatesToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label ExpressionLabel;
		private System.Windows.Forms.Label InvalidLabel;
		private System.Windows.Forms.RichTextBox ExpressionText;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton DefaultNamesButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem pictureToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem audioFilterToolStripMenuItem;
	}
}
