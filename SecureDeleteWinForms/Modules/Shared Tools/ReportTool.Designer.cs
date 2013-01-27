namespace SecureDeleteWinForms
{
	partial class ReportTool
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportTool));
			this.TabPanel = new System.Windows.Forms.Panel();
			this.ErrorsButton = new SecureDeleteWinForms.PanelSelectControl();
			this.FailedObjectsButton = new SecureDeleteWinForms.PanelSelectControl();
			this.StatisticsButton = new SecureDeleteWinForms.PanelSelectControl();
			this.StatisticsPanel = new System.Windows.Forms.Panel();
			this.StatisticsBox = new System.Windows.Forms.RichTextBox();
			this.SeverityIcons = new System.Windows.Forms.ImageList(this.components);
			this.ErrorsPanel = new System.Windows.Forms.Panel();
			this.ErrorListView = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ErrorDetailsBox = new System.Windows.Forms.RichTextBox();
			this.SeverityToolbar = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.HighSeverityButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.MediumSeverityButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.LowSeverityButton = new System.Windows.Forms.ToolStripButton();
			this.ClearButton = new System.Windows.Forms.ToolStripButton();
			this.SearchTextbox = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.FailedObjectsPanel = new System.Windows.Forms.Panel();
			this.FailedListView = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.FailedDetailsPanel = new System.Windows.Forms.Panel();
			this.SeverityIcon = new System.Windows.Forms.PictureBox();
			this.MessageLabel = new System.Windows.Forms.Label();
			this.TimeLabel = new System.Windows.Forms.Label();
			this.SeverityLabel = new System.Windows.Forms.Label();
			this.TabPanel.SuspendLayout();
			this.StatisticsPanel.SuspendLayout();
			this.ErrorsPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SeverityToolbar.SuspendLayout();
			this.FailedObjectsPanel.SuspendLayout();
			this.FailedDetailsPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SeverityIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// TabPanel
			// 
			this.TabPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.TabPanel.Controls.Add(this.ErrorsButton);
			this.TabPanel.Controls.Add(this.FailedObjectsButton);
			this.TabPanel.Controls.Add(this.StatisticsButton);
			this.TabPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.TabPanel.Location = new System.Drawing.Point(0, 318);
			this.TabPanel.Name = "TabPanel";
			this.TabPanel.Size = new System.Drawing.Size(857, 24);
			this.TabPanel.TabIndex = 10;
			// 
			// ErrorsButton
			// 
			this.ErrorsButton.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.ErrorsButton.Location = new System.Drawing.Point(152, 0);
			this.ErrorsButton.Name = "ErrorsButton";
			this.ErrorsButton.Reversed = true;
			this.ErrorsButton.Selected = false;
			this.ErrorsButton.SelectedColor = System.Drawing.SystemColors.Control;
			this.ErrorsButton.SelectedTextColor = System.Drawing.Color.Black;
			this.ErrorsButton.SelectorText = "Errors";
			this.ErrorsButton.Size = new System.Drawing.Size(45, 21);
			this.ErrorsButton.TabIndex = 2;
			this.ErrorsButton.TextColor = System.Drawing.Color.White;
			this.ErrorsButton.Click += new System.EventHandler(this.panelSelectControl3_Click);
			// 
			// FailedObjectsButton
			// 
			this.FailedObjectsButton.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.FailedObjectsButton.Location = new System.Drawing.Point(65, 0);
			this.FailedObjectsButton.Name = "FailedObjectsButton";
			this.FailedObjectsButton.Reversed = true;
			this.FailedObjectsButton.Selected = false;
			this.FailedObjectsButton.SelectedColor = System.Drawing.SystemColors.Control;
			this.FailedObjectsButton.SelectedTextColor = System.Drawing.Color.Black;
			this.FailedObjectsButton.SelectorText = "Failed Objects";
			this.FailedObjectsButton.Size = new System.Drawing.Size(84, 21);
			this.FailedObjectsButton.TabIndex = 1;
			this.FailedObjectsButton.TextColor = System.Drawing.Color.White;
			this.FailedObjectsButton.Click += new System.EventHandler(this.FailedObjectsButton_Click);
			// 
			// StatisticsButton
			// 
			this.StatisticsButton.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.StatisticsButton.Location = new System.Drawing.Point(3, 0);
			this.StatisticsButton.Name = "StatisticsButton";
			this.StatisticsButton.Reversed = true;
			this.StatisticsButton.Selected = false;
			this.StatisticsButton.SelectedColor = System.Drawing.SystemColors.Control;
			this.StatisticsButton.SelectedTextColor = System.Drawing.SystemColors.ControlText;
			this.StatisticsButton.SelectorText = "Statistics";
			this.StatisticsButton.Size = new System.Drawing.Size(59, 21);
			this.StatisticsButton.TabIndex = 0;
			this.StatisticsButton.TextColor = System.Drawing.Color.White;
			this.StatisticsButton.Click += new System.EventHandler(this.StatisticsButton_Click);
			// 
			// StatisticsPanel
			// 
			this.StatisticsPanel.BackColor = System.Drawing.SystemColors.Control;
			this.StatisticsPanel.Controls.Add(this.StatisticsBox);
			this.StatisticsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StatisticsPanel.Location = new System.Drawing.Point(0, 0);
			this.StatisticsPanel.Name = "StatisticsPanel";
			this.StatisticsPanel.Size = new System.Drawing.Size(857, 318);
			this.StatisticsPanel.TabIndex = 11;
			// 
			// StatisticsBox
			// 
			this.StatisticsBox.BackColor = System.Drawing.SystemColors.Control;
			this.StatisticsBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.StatisticsBox.DetectUrls = false;
			this.StatisticsBox.Location = new System.Drawing.Point(5, 5);
			this.StatisticsBox.Name = "StatisticsBox";
			this.StatisticsBox.ReadOnly = true;
			this.StatisticsBox.Size = new System.Drawing.Size(852, 313);
			this.StatisticsBox.TabIndex = 0;
			this.StatisticsBox.Text = "";
			// 
			// SeverityIcons
			// 
			this.SeverityIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("SeverityIcons.ImageStream")));
			this.SeverityIcons.TransparentColor = System.Drawing.Color.Transparent;
			this.SeverityIcons.Images.SetKeyName(0, "error.png");
			this.SeverityIcons.Images.SetKeyName(1, "warning.png");
			this.SeverityIcons.Images.SetKeyName(2, "info.png");
			// 
			// ErrorsPanel
			// 
			this.ErrorsPanel.Controls.Add(this.ErrorListView);
			this.ErrorsPanel.Controls.Add(this.panel1);
			this.ErrorsPanel.Controls.Add(this.SeverityToolbar);
			this.ErrorsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ErrorsPanel.Location = new System.Drawing.Point(0, 0);
			this.ErrorsPanel.Name = "ErrorsPanel";
			this.ErrorsPanel.Size = new System.Drawing.Size(857, 318);
			this.ErrorsPanel.TabIndex = 13;
			// 
			// ErrorListView
			// 
			this.ErrorListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.ErrorListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ErrorListView.FullRowSelect = true;
			this.ErrorListView.Location = new System.Drawing.Point(0, 25);
			this.ErrorListView.Name = "ErrorListView";
			this.ErrorListView.Size = new System.Drawing.Size(857, 233);
			this.ErrorListView.SmallImageList = this.SeverityIcons;
			this.ErrorListView.TabIndex = 1;
			this.ErrorListView.UseCompatibleStateImageBehavior = false;
			this.ErrorListView.View = System.Windows.Forms.View.Details;
			this.ErrorListView.VirtualMode = true;
			this.ErrorListView.SelectedIndexChanged += new System.EventHandler(this.ErrorListView_SelectedIndexChanged);
			this.ErrorListView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.ErrorListView_RetrieveVirtualItem);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Id";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Time";
			this.columnHeader2.Width = 100;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Message";
			this.columnHeader3.Width = 600;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.ErrorDetailsBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 258);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(857, 60);
			this.panel1.TabIndex = 2;
			// 
			// ErrorDetailsBox
			// 
			this.ErrorDetailsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ErrorDetailsBox.BackColor = System.Drawing.SystemColors.Control;
			this.ErrorDetailsBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ErrorDetailsBox.Location = new System.Drawing.Point(5, 3);
			this.ErrorDetailsBox.Name = "ErrorDetailsBox";
			this.ErrorDetailsBox.ReadOnly = true;
			this.ErrorDetailsBox.Size = new System.Drawing.Size(852, 57);
			this.ErrorDetailsBox.TabIndex = 3;
			this.ErrorDetailsBox.Text = "";
			// 
			// SeverityToolbar
			// 
			this.SeverityToolbar.BackColor = System.Drawing.SystemColors.Control;
			this.SeverityToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.SeverityToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.HighSeverityButton,
            this.toolStripSeparator1,
            this.MediumSeverityButton,
            this.toolStripSeparator2,
            this.LowSeverityButton,
            this.ClearButton,
            this.SearchTextbox,
            this.toolStripLabel2,
            this.toolStripSeparator3});
			this.SeverityToolbar.Location = new System.Drawing.Point(0, 0);
			this.SeverityToolbar.Name = "SeverityToolbar";
			this.SeverityToolbar.Padding = new System.Windows.Forms.Padding(3, 0, 1, 0);
			this.SeverityToolbar.Size = new System.Drawing.Size(857, 25);
			this.SeverityToolbar.TabIndex = 0;
			this.SeverityToolbar.Text = "toolStrip1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(51, 22);
			this.toolStripLabel1.Text = "Severity:";
			// 
			// HighSeverityButton
			// 
			this.HighSeverityButton.Checked = true;
			this.HighSeverityButton.CheckOnClick = true;
			this.HighSeverityButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.HighSeverityButton.Image = global::SecureDeleteWinForms.Properties.Resources.error;
			this.HighSeverityButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.HighSeverityButton.Name = "HighSeverityButton";
			this.HighSeverityButton.Size = new System.Drawing.Size(53, 22);
			this.HighSeverityButton.Text = "High";
			this.HighSeverityButton.Click += new System.EventHandler(this.HighSeverityButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// MediumSeverityButton
			// 
			this.MediumSeverityButton.Checked = true;
			this.MediumSeverityButton.CheckOnClick = true;
			this.MediumSeverityButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.MediumSeverityButton.Image = global::SecureDeleteWinForms.Properties.Resources.warning;
			this.MediumSeverityButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.MediumSeverityButton.Name = "MediumSeverityButton";
			this.MediumSeverityButton.Size = new System.Drawing.Size(72, 22);
			this.MediumSeverityButton.Text = "Medium";
			this.MediumSeverityButton.Click += new System.EventHandler(this.MediumSeverityButton_Click_1);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// LowSeverityButton
			// 
			this.LowSeverityButton.Checked = true;
			this.LowSeverityButton.CheckOnClick = true;
			this.LowSeverityButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.LowSeverityButton.Image = global::SecureDeleteWinForms.Properties.Resources.info;
			this.LowSeverityButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.LowSeverityButton.Name = "LowSeverityButton";
			this.LowSeverityButton.Size = new System.Drawing.Size(49, 22);
			this.LowSeverityButton.Text = "Low";
			this.LowSeverityButton.Click += new System.EventHandler(this.LowSeverityButton_Click_1);
			// 
			// ClearButton
			// 
			this.ClearButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.ClearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ClearButton.Image = global::SecureDeleteWinForms.Properties.Resources.delete_profile;
			this.ClearButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ClearButton.Name = "ClearButton";
			this.ClearButton.Size = new System.Drawing.Size(23, 22);
			this.ClearButton.Text = "toolStripButton1";
			this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
			// 
			// SearchTextbox
			// 
			this.SearchTextbox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.SearchTextbox.BackColor = System.Drawing.SystemColors.Window;
			this.SearchTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SearchTextbox.Name = "SearchTextbox";
			this.SearchTextbox.Size = new System.Drawing.Size(150, 25);
			this.SearchTextbox.TextChanged += new System.EventHandler(this.SearchTextbox_TextChanged);
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(33, 22);
			this.toolStripLabel2.Text = "Filter";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// FailedObjectsPanel
			// 
			this.FailedObjectsPanel.BackColor = System.Drawing.SystemColors.Control;
			this.FailedObjectsPanel.Controls.Add(this.FailedListView);
			this.FailedObjectsPanel.Controls.Add(this.FailedDetailsPanel);
			this.FailedObjectsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FailedObjectsPanel.Location = new System.Drawing.Point(0, 0);
			this.FailedObjectsPanel.Name = "FailedObjectsPanel";
			this.FailedObjectsPanel.Size = new System.Drawing.Size(857, 318);
			this.FailedObjectsPanel.TabIndex = 14;
			// 
			// FailedListView
			// 
			this.FailedListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
			this.FailedListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FailedListView.FullRowSelect = true;
			this.FailedListView.HideSelection = false;
			this.FailedListView.Location = new System.Drawing.Point(0, 0);
			this.FailedListView.Name = "FailedListView";
			this.FailedListView.Size = new System.Drawing.Size(857, 258);
			this.FailedListView.TabIndex = 1;
			this.FailedListView.UseCompatibleStateImageBehavior = false;
			this.FailedListView.View = System.Windows.Forms.View.Details;
			this.FailedListView.SelectedIndexChanged += new System.EventHandler(this.FailedListView_SelectedIndexChanged);
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Id";
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Type";
			this.columnHeader5.Width = 150;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Path";
			this.columnHeader6.Width = 500;
			// 
			// FailedDetailsPanel
			// 
			this.FailedDetailsPanel.BackColor = System.Drawing.SystemColors.Control;
			this.FailedDetailsPanel.Controls.Add(this.SeverityIcon);
			this.FailedDetailsPanel.Controls.Add(this.MessageLabel);
			this.FailedDetailsPanel.Controls.Add(this.TimeLabel);
			this.FailedDetailsPanel.Controls.Add(this.SeverityLabel);
			this.FailedDetailsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.FailedDetailsPanel.Location = new System.Drawing.Point(0, 258);
			this.FailedDetailsPanel.Name = "FailedDetailsPanel";
			this.FailedDetailsPanel.Size = new System.Drawing.Size(857, 60);
			this.FailedDetailsPanel.TabIndex = 0;
			this.FailedDetailsPanel.Visible = false;
			// 
			// SeverityIcon
			// 
			this.SeverityIcon.Image = global::SecureDeleteWinForms.Properties.Resources.error;
			this.SeverityIcon.Location = new System.Drawing.Point(8, 7);
			this.SeverityIcon.Name = "SeverityIcon";
			this.SeverityIcon.Size = new System.Drawing.Size(16, 16);
			this.SeverityIcon.TabIndex = 4;
			this.SeverityIcon.TabStop = false;
			// 
			// MessageLabel
			// 
			this.MessageLabel.AutoSize = true;
			this.MessageLabel.Location = new System.Drawing.Point(32, 39);
			this.MessageLabel.Name = "MessageLabel";
			this.MessageLabel.Size = new System.Drawing.Size(50, 13);
			this.MessageLabel.TabIndex = 3;
			this.MessageLabel.Text = "Message";
			// 
			// TimeLabel
			// 
			this.TimeLabel.AutoSize = true;
			this.TimeLabel.Location = new System.Drawing.Point(32, 23);
			this.TimeLabel.Name = "TimeLabel";
			this.TimeLabel.Size = new System.Drawing.Size(30, 13);
			this.TimeLabel.TabIndex = 2;
			this.TimeLabel.Text = "Time";
			// 
			// SeverityLabel
			// 
			this.SeverityLabel.AutoSize = true;
			this.SeverityLabel.Location = new System.Drawing.Point(32, 7);
			this.SeverityLabel.Name = "SeverityLabel";
			this.SeverityLabel.Size = new System.Drawing.Size(48, 13);
			this.SeverityLabel.TabIndex = 1;
			this.SeverityLabel.Text = "Severity:";
			// 
			// ReportTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.StatisticsPanel);
			this.Controls.Add(this.FailedObjectsPanel);
			this.Controls.Add(this.ErrorsPanel);
			this.Controls.Add(this.TabPanel);
			this.Name = "ReportTool";
			this.Size = new System.Drawing.Size(857, 342);
			this.TabPanel.ResumeLayout(false);
			this.StatisticsPanel.ResumeLayout(false);
			this.ErrorsPanel.ResumeLayout(false);
			this.ErrorsPanel.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.SeverityToolbar.ResumeLayout(false);
			this.SeverityToolbar.PerformLayout();
			this.FailedObjectsPanel.ResumeLayout(false);
			this.FailedDetailsPanel.ResumeLayout(false);
			this.FailedDetailsPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SeverityIcon)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel TabPanel;
		private PanelSelectControl StatisticsButton;
		private PanelSelectControl FailedObjectsButton;
		private PanelSelectControl ErrorsButton;
		private System.Windows.Forms.Panel StatisticsPanel;
		private System.Windows.Forms.RichTextBox StatisticsBox;
		private System.Windows.Forms.ImageList SeverityIcons;
		private System.Windows.Forms.Panel ErrorsPanel;
		private System.Windows.Forms.ListView ErrorListView;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ToolStrip SeverityToolbar;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripButton HighSeverityButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton MediumSeverityButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton LowSeverityButton;
		private System.Windows.Forms.Panel FailedObjectsPanel;
		private System.Windows.Forms.Panel FailedDetailsPanel;
		private System.Windows.Forms.PictureBox SeverityIcon;
		private System.Windows.Forms.Label MessageLabel;
		private System.Windows.Forms.Label TimeLabel;
		private System.Windows.Forms.Label SeverityLabel;
		private System.Windows.Forms.ListView FailedListView;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripTextBox SearchTextbox;
		private System.Windows.Forms.ToolStripButton ClearButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RichTextBox ErrorDetailsBox;
	}
}
