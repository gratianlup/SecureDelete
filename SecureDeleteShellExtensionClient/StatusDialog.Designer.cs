namespace ShellExtensionClient
{
	partial class StatusDialog
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.wipeTool = new SecureDeleteWinForms.WipeTools.WipeTool();
			this.reportTool = new SecureDeleteWinForms.ReportTool();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.FooterPanel = new System.Windows.Forms.Panel();
			this.OntopCheckbox = new System.Windows.Forms.CheckBox();
			this.StopButton = new System.Windows.Forms.Button();
			this.CopyPanel = new System.Windows.Forms.Panel();
			this.panelExHost1 = new SecureDeleteWinForms.PanelExHost();
			this.TotalPanel = new SecureDeleteWinForms.PanelEx();
			this.PercentLabel = new System.Windows.Forms.Label();
			this.TotalProgressbar = new System.Windows.Forms.ProgressBar();
			this.CopyStatus = new SecureDeleteWinForms.WipeStatusView();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.FooterPanel.SuspendLayout();
			this.CopyPanel.SuspendLayout();
			this.panelExHost1.SuspendLayout();
			this.TotalPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// wipeTool
			// 
			this.wipeTool.AutoScroll = true;
			this.wipeTool.BackColor = System.Drawing.SystemColors.Control;
			this.wipeTool.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wipeTool.FooterVisible = false;
			this.wipeTool.Location = new System.Drawing.Point(0, 60);
			this.wipeTool.Name = "wipeTool";
			this.wipeTool.Options = null;
			this.wipeTool.ParentControl = null;
			this.wipeTool.Session = null;
			this.wipeTool.Size = new System.Drawing.Size(672, 259);
			this.wipeTool.TabIndex = 0;
			this.wipeTool.ToolIcon = null;
			// 
			// reportTool
			// 
			this.reportTool.Dock = System.Windows.Forms.DockStyle.Fill;
			this.reportTool.Location = new System.Drawing.Point(0, 60);
			this.reportTool.Name = "reportTool";
			this.reportTool.Options = null;
			this.reportTool.ParentControl = null;
			this.reportTool.Report = null;
			this.reportTool.Size = new System.Drawing.Size(672, 259);
			this.reportTool.TabIndex = 1;
			this.reportTool.ToolIcon = null;
			this.reportTool.Visible = false;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Controls.Add(this.pictureBox2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(672, 60);
			this.panel1.TabIndex = 2;
			// 
			// pictureBox2
			// 
			this.pictureBox2.BackColor = System.Drawing.Color.White;
			this.pictureBox2.Image = global::ShellExtensionClient.Properties.Resources.crash1;
			this.pictureBox2.Location = new System.Drawing.Point(0, -1);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(248, 50);
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.BackColor = System.Drawing.Color.White;
			this.pictureBox1.Image = global::ShellExtensionClient.Properties.Resources.crash_small;
			this.pictureBox1.Location = new System.Drawing.Point(540, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(132, 60);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// FooterPanel
			// 
			this.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.FooterPanel.Controls.Add(this.OntopCheckbox);
			this.FooterPanel.Controls.Add(this.StopButton);
			this.FooterPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.FooterPanel.Location = new System.Drawing.Point(0, 319);
			this.FooterPanel.Name = "FooterPanel";
			this.FooterPanel.Size = new System.Drawing.Size(672, 30);
			this.FooterPanel.TabIndex = 10;
			// 
			// OntopCheckbox
			// 
			this.OntopCheckbox.AutoSize = true;
			this.OntopCheckbox.ForeColor = System.Drawing.Color.White;
			this.OntopCheckbox.Location = new System.Drawing.Point(9, 8);
			this.OntopCheckbox.Name = "OntopCheckbox";
			this.OntopCheckbox.Size = new System.Drawing.Size(92, 17);
			this.OntopCheckbox.TabIndex = 6;
			this.OntopCheckbox.Text = "Always on top";
			this.OntopCheckbox.UseVisualStyleBackColor = true;
			this.OntopCheckbox.CheckedChanged += new System.EventHandler(this.OntopCheckbox_CheckedChanged);
			// 
			// StopButton
			// 
			this.StopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.StopButton.BackColor = System.Drawing.SystemColors.Control;
			this.StopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.StopButton.Location = new System.Drawing.Point(589, 3);
			this.StopButton.Name = "StopButton";
			this.StopButton.Size = new System.Drawing.Size(76, 24);
			this.StopButton.TabIndex = 5;
			this.StopButton.Text = "Stop";
			this.StopButton.UseVisualStyleBackColor = true;
			this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
			// 
			// CopyPanel
			// 
			this.CopyPanel.Controls.Add(this.panelExHost1);
			this.CopyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CopyPanel.Location = new System.Drawing.Point(0, 60);
			this.CopyPanel.Name = "CopyPanel";
			this.CopyPanel.Size = new System.Drawing.Size(672, 259);
			this.CopyPanel.TabIndex = 2;
			// 
			// panelExHost1
			// 
			this.panelExHost1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelExHost1.Animate = true;
			this.panelExHost1.Controls.Add(this.TotalPanel);
			this.panelExHost1.Controls.Add(this.CopyStatus);
			this.panelExHost1.Location = new System.Drawing.Point(6, 11);
			this.panelExHost1.Name = "panelExHost1";
			this.panelExHost1.PanelDistance = 8;
			this.panelExHost1.Size = new System.Drawing.Size(659, 186);
			this.panelExHost1.TabIndex = 19;
			// 
			// TotalPanel
			// 
			this.TotalPanel.AllowCollapse = false;
			this.TotalPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.TotalPanel.Collapsed = false;
			this.TotalPanel.CollapsedSize = 24;
			this.TotalPanel.Controls.Add(this.PercentLabel);
			this.TotalPanel.Controls.Add(this.TotalProgressbar);
			this.TotalPanel.ExpandedSize = 64;
			this.TotalPanel.ForeColor = System.Drawing.Color.White;
			this.TotalPanel.GradientColor1 = System.Drawing.Color.LightSteelBlue;
			this.TotalPanel.GradientColor2 = System.Drawing.Color.LightSlateGray;
			this.TotalPanel.Location = new System.Drawing.Point(1, 1);
			this.TotalPanel.Name = "TotalPanel";
			this.TotalPanel.Size = new System.Drawing.Size(657, 64);
			this.TotalPanel.Subtitle = "";
			this.TotalPanel.TabIndex = 19;
			this.TotalPanel.Title = "Total";
			// 
			// PercentLabel
			// 
			this.PercentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.PercentLabel.AutoSize = true;
			this.PercentLabel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.PercentLabel.Location = new System.Drawing.Point(605, 39);
			this.PercentLabel.Name = "PercentLabel";
			this.PercentLabel.Size = new System.Drawing.Size(51, 13);
			this.PercentLabel.TabIndex = 7;
			this.PercentLabel.Text = "100.00 %";
			// 
			// TotalProgressbar
			// 
			this.TotalProgressbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.TotalProgressbar.Location = new System.Drawing.Point(6, 36);
			this.TotalProgressbar.Name = "TotalProgressbar";
			this.TotalProgressbar.Size = new System.Drawing.Size(594, 19);
			this.TotalProgressbar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.TotalProgressbar.TabIndex = 6;
			// 
			// CopyStatus
			// 
			this.CopyStatus.AllowCollapse = false;
			this.CopyStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.CopyStatus.BackColor = System.Drawing.SystemColors.Control;
			this.CopyStatus.Collapsed = false;
			this.CopyStatus.CollapsedSize = 24;
			this.CopyStatus.ContextId = 0;
			this.CopyStatus.ExpandedSize = 100;
			this.CopyStatus.GradientColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(192)))), ((int)(((byte)(201)))));
			this.CopyStatus.GradientColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(175)))), ((int)(((byte)(186)))));
			this.CopyStatus.Location = new System.Drawing.Point(1, 74);
			this.CopyStatus.MainText = "Copying file";
			this.CopyStatus.Name = "CopyStatus";
			this.CopyStatus.ProgressText = "100.00 %";
			this.CopyStatus.ProgressValue = 0;
			this.CopyStatus.SecondaryText = "Wiping member:";
			this.CopyStatus.Size = new System.Drawing.Size(657, 100);
			this.CopyStatus.SizeText = "25 of 130 MB";
			this.CopyStatus.StepText = "Step: 1 of 3";
			this.CopyStatus.Subtitle = null;
			this.CopyStatus.TabIndex = 20;
			this.CopyStatus.Title = "Copying file";
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.Black;
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 59);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(672, 1);
			this.panel2.TabIndex = 2;
			// 
			// StatusDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(672, 349);
			this.Controls.Add(this.CopyPanel);
			this.Controls.Add(this.wipeTool);
			this.Controls.Add(this.reportTool);
			this.Controls.Add(this.FooterPanel);
			this.Controls.Add(this.panel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StatusDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SecureDelete";
			this.Load += new System.EventHandler(this.StatusDialog_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StatusDialog_FormClosing);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.FooterPanel.ResumeLayout(false);
			this.FooterPanel.PerformLayout();
			this.CopyPanel.ResumeLayout(false);
			this.panelExHost1.ResumeLayout(false);
			this.TotalPanel.ResumeLayout(false);
			this.TotalPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private SecureDeleteWinForms.WipeTools.WipeTool wipeTool;
		private SecureDeleteWinForms.ReportTool reportTool;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Panel FooterPanel;
		private System.Windows.Forms.Button StopButton;
		private System.Windows.Forms.CheckBox OntopCheckbox;
		private System.Windows.Forms.Panel CopyPanel;
		private SecureDeleteWinForms.PanelExHost panelExHost1;
		private SecureDeleteWinForms.PanelEx TotalPanel;
		private System.Windows.Forms.Label PercentLabel;
		private System.Windows.Forms.ProgressBar TotalProgressbar;
		private SecureDeleteWinForms.WipeStatusView CopyStatus;
		private System.Windows.Forms.Panel panel2;
	}
}