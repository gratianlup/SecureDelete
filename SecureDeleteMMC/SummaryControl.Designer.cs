namespace SecureDeleteMMC
{
	partial class SummaryControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SummaryControl));
			this.HeaderPanel = new System.Windows.Forms.Panel();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.PanelHost = new SecureDeleteWinForms.PanelExHost();
			this.AboutPanel = new SecureDeleteWinForms.PanelEx();
			this.button4 = new System.Windows.Forms.Button();
			this.PluginList = new System.Windows.Forms.ListView();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.PowershellLabel = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.PluginLabel = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.panelEx2 = new SecureDeleteWinForms.PanelEx();
			this.button1 = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.ReportPanel = new SecureDeleteWinForms.PanelEx();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.CategoryList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.CategoryCountLabel = new System.Windows.Forms.Label();
			this.FailedCountLabel = new System.Windows.Forms.Label();
			this.ErrorCountLabel = new System.Windows.Forms.Label();
			this.ReportCountLabel = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.HeaderPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.PanelHost.SuspendLayout();
			this.AboutPanel.SuspendLayout();
			this.panelEx2.SuspendLayout();
			this.ReportPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			this.SuspendLayout();
			// 
			// HeaderPanel
			// 
			this.HeaderPanel.BackColor = System.Drawing.Color.White;
			this.HeaderPanel.Controls.Add(this.pictureBox2);
			this.HeaderPanel.Controls.Add(this.pictureBox1);
			this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
			this.HeaderPanel.Name = "HeaderPanel";
			this.HeaderPanel.Size = new System.Drawing.Size(533, 90);
			this.HeaderPanel.TabIndex = 1;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::SecureDeleteMMC.Properties.Resources.SecureDelete_about2;
			this.pictureBox2.Location = new System.Drawing.Point(3, 12);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(257, 61);
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Right;
			this.pictureBox1.Image = global::SecureDeleteMMC.Properties.Resources.crash2;
			this.pictureBox1.Location = new System.Drawing.Point(347, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(186, 90);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// PanelHost
			// 
			this.PanelHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.PanelHost.Animate = true;
			this.PanelHost.AutoScroll = true;
			this.PanelHost.Controls.Add(this.AboutPanel);
			this.PanelHost.Controls.Add(this.panelEx2);
			this.PanelHost.Controls.Add(this.ReportPanel);
			this.PanelHost.Location = new System.Drawing.Point(10, 96);
			this.PanelHost.Name = "PanelHost";
			this.PanelHost.PanelDistance = 8;
			this.PanelHost.Size = new System.Drawing.Size(510, 609);
			this.PanelHost.TabIndex = 2;
			// 
			// AboutPanel
			// 
			this.AboutPanel.AllowCollapse = true;
			this.AboutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.AboutPanel.BackColor = System.Drawing.SystemColors.Window;
			this.AboutPanel.Collapsed = false;
			this.AboutPanel.CollapsedSize = 24;
			this.AboutPanel.Controls.Add(this.button4);
			this.AboutPanel.Controls.Add(this.PluginList);
			this.AboutPanel.Controls.Add(this.PowershellLabel);
			this.AboutPanel.Controls.Add(this.linkLabel1);
			this.AboutPanel.Controls.Add(this.PluginLabel);
			this.AboutPanel.Controls.Add(this.panel2);
			this.AboutPanel.Controls.Add(this.label3);
			this.AboutPanel.Controls.Add(this.label2);
			this.AboutPanel.Controls.Add(this.label1);
			this.AboutPanel.ExpandedSize = 155;
			this.AboutPanel.GradientColor1 = System.Drawing.Color.White;
			this.AboutPanel.GradientColor2 = System.Drawing.SystemColors.Control;
			this.AboutPanel.Location = new System.Drawing.Point(1, 1);
			this.AboutPanel.Name = "AboutPanel";
			this.AboutPanel.Size = new System.Drawing.Size(506, 155);
			this.AboutPanel.Subtitle = null;
			this.AboutPanel.TabIndex = 2;
			this.AboutPanel.TextColor = System.Drawing.SystemColors.WindowText;
			this.AboutPanel.Title = "SecureDelete";
			// 
			// button4
			// 
			this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button4.BackColor = System.Drawing.SystemColors.Control;
			this.button4.Image = global::SecureDeleteMMC.Properties.Resources.wrench;
			this.button4.Location = new System.Drawing.Point(361, 34);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(133, 24);
			this.button4.TabIndex = 13;
			this.button4.Text = "Change Settings";
			this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// PluginList
			// 
			this.PluginList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.PluginList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
			this.PluginList.FullRowSelect = true;
			this.PluginList.Location = new System.Drawing.Point(25, 156);
			this.PluginList.Name = "PluginList";
			this.PluginList.Size = new System.Drawing.Size(469, 105);
			this.PluginList.TabIndex = 10;
			this.PluginList.UseCompatibleStateImageBehavior = false;
			this.PluginList.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Assembly";
			this.columnHeader3.Width = 243;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Version";
			this.columnHeader4.Width = 126;
			// 
			// PowershellLabel
			// 
			this.PowershellLabel.AutoSize = true;
			this.PowershellLabel.Location = new System.Drawing.Point(24, 111);
			this.PowershellLabel.Name = "PowershellLabel";
			this.PowershellLabel.Size = new System.Drawing.Size(122, 13);
			this.PowershellLabel.TabIndex = 6;
			this.PowershellLabel.Text = "PowerShell Installed: No";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Image = global::SecureDeleteMMC.Properties.Resources.plugin;
			this.linkLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(7, 9);
			this.linkLabel1.Location = new System.Drawing.Point(194, 131);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(67, 17);
			this.linkLabel1.TabIndex = 5;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "       View list";
			this.linkLabel1.UseCompatibleTextRendering = true;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// PluginLabel
			// 
			this.PluginLabel.AutoSize = true;
			this.PluginLabel.Location = new System.Drawing.Point(24, 131);
			this.PluginLabel.Name = "PluginLabel";
			this.PluginLabel.Size = new System.Drawing.Size(95, 13);
			this.PluginLabel.TabIndex = 4;
			this.PluginLabel.Text = "Installed Plugins: 0";
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Location = new System.Drawing.Point(27, 98);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(467, 1);
			this.panel2.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(24, 74);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(152, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Copyright (C) 2008 Lup Gratian";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(24, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(63, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Version: 1.0";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(24, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "SecureDelete";
			// 
			// panelEx2
			// 
			this.panelEx2.AllowCollapse = true;
			this.panelEx2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelEx2.BackColor = System.Drawing.SystemColors.Window;
			this.panelEx2.Collapsed = false;
			this.panelEx2.CollapsedSize = 24;
			this.panelEx2.Controls.Add(this.button1);
			this.panelEx2.Controls.Add(this.label9);
			this.panelEx2.Controls.Add(this.label8);
			this.panelEx2.Controls.Add(this.label7);
			this.panelEx2.Controls.Add(this.label6);
			this.panelEx2.ExpandedSize = 120;
			this.panelEx2.GradientColor1 = System.Drawing.Color.White;
			this.panelEx2.GradientColor2 = System.Drawing.SystemColors.Control;
			this.panelEx2.Location = new System.Drawing.Point(1, 165);
			this.panelEx2.Name = "panelEx2";
			this.panelEx2.Size = new System.Drawing.Size(506, 120);
			this.panelEx2.Subtitle = null;
			this.panelEx2.TabIndex = 1;
			this.panelEx2.TextColor = System.Drawing.SystemColors.WindowText;
			this.panelEx2.Title = "Schedule";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.BackColor = System.Drawing.SystemColors.Control;
			this.button1.Image = global::SecureDeleteMMC.Properties.Resources.wrench;
			this.button1.Location = new System.Drawing.Point(361, 34);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(133, 24);
			this.button1.TabIndex = 14;
			this.button1.Text = "Change Settings";
			this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(24, 94);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(57, 13);
			this.label9.TabIndex = 4;
			this.label9.Text = "Queued: 0";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(24, 74);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(49, 13);
			this.label8.TabIndex = 3;
			this.label8.Text = "Active: 0";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(24, 54);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(58, 13);
			this.label7.TabIndex = 2;
			this.label7.Text = "Enabled: 0";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(24, 34);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(102, 13);
			this.label6.TabIndex = 1;
			this.label6.Text = "Scheduled Tasks: 0";
			// 
			// ReportPanel
			// 
			this.ReportPanel.AllowCollapse = true;
			this.ReportPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ReportPanel.BackColor = System.Drawing.SystemColors.Window;
			this.ReportPanel.Collapsed = false;
			this.ReportPanel.CollapsedSize = 24;
			this.ReportPanel.Controls.Add(this.button3);
			this.ReportPanel.Controls.Add(this.button2);
			this.ReportPanel.Controls.Add(this.pictureBox4);
			this.ReportPanel.Controls.Add(this.pictureBox3);
			this.ReportPanel.Controls.Add(this.CategoryList);
			this.ReportPanel.Controls.Add(this.linkLabel2);
			this.ReportPanel.Controls.Add(this.CategoryCountLabel);
			this.ReportPanel.Controls.Add(this.FailedCountLabel);
			this.ReportPanel.Controls.Add(this.ErrorCountLabel);
			this.ReportPanel.Controls.Add(this.ReportCountLabel);
			this.ReportPanel.ExpandedSize = 132;
			this.ReportPanel.GradientColor1 = System.Drawing.Color.White;
			this.ReportPanel.GradientColor2 = System.Drawing.SystemColors.Control;
			this.ReportPanel.Location = new System.Drawing.Point(1, 294);
			this.ReportPanel.Name = "ReportPanel";
			this.ReportPanel.Size = new System.Drawing.Size(506, 132);
			this.ReportPanel.Subtitle = null;
			this.ReportPanel.TabIndex = 0;
			this.ReportPanel.TextColor = System.Drawing.SystemColors.WindowText;
			this.ReportPanel.Title = "Reports";
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.BackColor = System.Drawing.SystemColors.Control;
			this.button3.Image = global::SecureDeleteMMC.Properties.Resources.wrench;
			this.button3.Location = new System.Drawing.Point(361, 33);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(133, 24);
			this.button3.TabIndex = 14;
			this.button3.Text = "Change Settings";
			this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.BackColor = System.Drawing.SystemColors.Control;
			this.button2.Image = global::SecureDeleteMMC.Properties.Resources.report_delete;
			this.button2.Location = new System.Drawing.Point(361, 63);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(133, 24);
			this.button2.TabIndex = 13;
			this.button2.Text = "Remove Reports";
			this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// pictureBox4
			// 
			this.pictureBox4.Image = global::SecureDeleteMMC.Properties.Resources.bullet_go;
			this.pictureBox4.Location = new System.Drawing.Point(28, 71);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(16, 16);
			this.pictureBox4.TabIndex = 11;
			this.pictureBox4.TabStop = false;
			// 
			// pictureBox3
			// 
			this.pictureBox3.Image = global::SecureDeleteMMC.Properties.Resources.bullet_go;
			this.pictureBox3.Location = new System.Drawing.Point(28, 53);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(16, 16);
			this.pictureBox3.TabIndex = 10;
			this.pictureBox3.TabStop = false;
			// 
			// CategoryList
			// 
			this.CategoryList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.CategoryList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.CategoryList.FullRowSelect = true;
			this.CategoryList.Location = new System.Drawing.Point(25, 133);
			this.CategoryList.Name = "CategoryList";
			this.CategoryList.Size = new System.Drawing.Size(469, 155);
			this.CategoryList.SmallImageList = this.imageList1;
			this.CategoryList.TabIndex = 9;
			this.CategoryList.UseCompatibleStateImageBehavior = false;
			this.CategoryList.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Category Name";
			this.columnHeader1.Width = 265;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Reports";
			this.columnHeader2.Width = 82;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "bullet_blue.png");
			this.imageList1.Images.SetKeyName(1, "date.png");
			this.imageList1.Images.SetKeyName(2, "date_error.png");
			// 
			// linkLabel2
			// 
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Image = global::SecureDeleteMMC.Properties.Resources.report;
			this.linkLabel2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.linkLabel2.LinkArea = new System.Windows.Forms.LinkArea(7, 9);
			this.linkLabel2.Location = new System.Drawing.Point(194, 106);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(67, 17);
			this.linkLabel2.TabIndex = 8;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "       View list";
			this.linkLabel2.UseCompatibleTextRendering = true;
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			// 
			// CategoryCountLabel
			// 
			this.CategoryCountLabel.AutoSize = true;
			this.CategoryCountLabel.Location = new System.Drawing.Point(24, 106);
			this.CategoryCountLabel.Name = "CategoryCountLabel";
			this.CategoryCountLabel.Size = new System.Drawing.Size(69, 13);
			this.CategoryCountLabel.TabIndex = 7;
			this.CategoryCountLabel.Text = "Categories: 0";
			// 
			// FailedCountLabel
			// 
			this.FailedCountLabel.AutoSize = true;
			this.FailedCountLabel.Location = new System.Drawing.Point(44, 73);
			this.FailedCountLabel.Name = "FailedCountLabel";
			this.FailedCountLabel.Size = new System.Drawing.Size(84, 13);
			this.FailedCountLabel.TabIndex = 5;
			this.FailedCountLabel.Text = "Failed objects: 0";
			// 
			// ErrorCountLabel
			// 
			this.ErrorCountLabel.AutoSize = true;
			this.ErrorCountLabel.Location = new System.Drawing.Point(44, 54);
			this.ErrorCountLabel.Name = "ErrorCountLabel";
			this.ErrorCountLabel.Size = new System.Drawing.Size(46, 13);
			this.ErrorCountLabel.TabIndex = 4;
			this.ErrorCountLabel.Text = "Errors: 0";
			// 
			// ReportCountLabel
			// 
			this.ReportCountLabel.AutoSize = true;
			this.ReportCountLabel.Location = new System.Drawing.Point(24, 34);
			this.ReportCountLabel.Name = "ReportCountLabel";
			this.ReportCountLabel.Size = new System.Drawing.Size(56, 13);
			this.ReportCountLabel.TabIndex = 3;
			this.ReportCountLabel.Text = "Reports: 0";
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 90);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(533, 1);
			this.panel1.TabIndex = 3;
			// 
			// SummaryControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.PanelHost);
			this.Controls.Add(this.HeaderPanel);
			this.Name = "SummaryControl";
			this.Size = new System.Drawing.Size(533, 708);
			this.HeaderPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.PanelHost.ResumeLayout(false);
			this.AboutPanel.ResumeLayout(false);
			this.AboutPanel.PerformLayout();
			this.panelEx2.ResumeLayout(false);
			this.panelEx2.PerformLayout();
			this.ReportPanel.ResumeLayout(false);
			this.ReportPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel HeaderPanel;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private SecureDeleteWinForms.PanelExHost PanelHost;
		private SecureDeleteWinForms.PanelEx ReportPanel;
		private System.Windows.Forms.Panel panel1;
		private SecureDeleteWinForms.PanelEx panelEx2;
		private SecureDeleteWinForms.PanelEx AboutPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label PluginLabel;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label PowershellLabel;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label ReportCountLabel;
		private System.Windows.Forms.Label FailedCountLabel;
		private System.Windows.Forms.Label ErrorCountLabel;
		private System.Windows.Forms.Label CategoryCountLabel;
		private System.Windows.Forms.LinkLabel linkLabel2;
		private System.Windows.Forms.ListView CategoryList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ListView PluginList;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.ImageList imageList1;
	}
}
