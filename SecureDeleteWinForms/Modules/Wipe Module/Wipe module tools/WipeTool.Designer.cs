namespace SecureDeleteWinForms.WipeTools
{
	partial class WipeTool
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
			this.StatisicsPanel = new System.Windows.Forms.Panel();
			this.SaveReportLabel = new System.Windows.Forms.LinkLabel();
			this.ErrorLabel = new System.Windows.Forms.Label();
			this.FailedLabel = new System.Windows.Forms.Label();
			this.SpeedLabel = new System.Windows.Forms.Label();
			this.WipedSlackLabel = new System.Windows.Forms.Label();
			this.WipedLabel = new System.Windows.Forms.Label();
			this.DurationLabel = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.label7 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.StatusTimer = new System.Windows.Forms.Timer(this.components);
			this.FooterPanel = new System.Windows.Forms.Panel();
			this.AfterCheckbox = new System.Windows.Forms.CheckBox();
			this.CloseButton = new System.Windows.Forms.Button();
			this.panelExHost1 = new SecureDeleteWinForms.PanelExHost();
			this.TotalPanel = new SecureDeleteWinForms.PanelEx();
			this.PercentLabel = new System.Windows.Forms.Label();
			this.TotalProgressbar = new System.Windows.Forms.ProgressBar();
			this.StatusViewHost = new SecureDeleteWinForms.PanelExHost();
			this.StatisicsPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.FooterPanel.SuspendLayout();
			this.panelExHost1.SuspendLayout();
			this.TotalPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// StatisicsPanel
			// 
			this.StatisicsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.StatisicsPanel.Controls.Add(this.SaveReportLabel);
			this.StatisicsPanel.Controls.Add(this.ErrorLabel);
			this.StatisicsPanel.Controls.Add(this.FailedLabel);
			this.StatisicsPanel.Controls.Add(this.SpeedLabel);
			this.StatisicsPanel.Controls.Add(this.WipedSlackLabel);
			this.StatisicsPanel.Controls.Add(this.WipedLabel);
			this.StatisicsPanel.Controls.Add(this.DurationLabel);
			this.StatisicsPanel.Controls.Add(this.linkLabel1);
			this.StatisicsPanel.Controls.Add(this.label7);
			this.StatisicsPanel.Controls.Add(this.label2);
			this.StatisicsPanel.Controls.Add(this.panel2);
			this.StatisicsPanel.Controls.Add(this.label6);
			this.StatisicsPanel.Controls.Add(this.label5);
			this.StatisicsPanel.Controls.Add(this.label4);
			this.StatisicsPanel.Controls.Add(this.label3);
			this.StatisicsPanel.Controls.Add(this.pictureBox1);
			this.StatisicsPanel.Location = new System.Drawing.Point(0, 0);
			this.StatisicsPanel.Name = "StatisicsPanel";
			this.StatisicsPanel.Size = new System.Drawing.Size(760, 174);
			this.StatisicsPanel.TabIndex = 10;
			this.StatisicsPanel.Visible = false;
			// 
			// SaveReportLabel
			// 
			this.SaveReportLabel.AutoSize = true;
			this.SaveReportLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.SaveReportLabel.Location = new System.Drawing.Point(172, 140);
			this.SaveReportLabel.Name = "SaveReportLabel";
			this.SaveReportLabel.Size = new System.Drawing.Size(74, 15);
			this.SaveReportLabel.TabIndex = 19;
			this.SaveReportLabel.TabStop = true;
			this.SaveReportLabel.Text = "Save Report";
			this.SaveReportLabel.Visible = false;
			this.SaveReportLabel.VisitedLinkColor = System.Drawing.Color.Blue;
			this.SaveReportLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SaveReportLabel_LinkClicked);
			// 
			// ErrorLabel
			// 
			this.ErrorLabel.AutoSize = true;
			this.ErrorLabel.Location = new System.Drawing.Point(211, 115);
			this.ErrorLabel.Name = "ErrorLabel";
			this.ErrorLabel.Size = new System.Drawing.Size(13, 13);
			this.ErrorLabel.TabIndex = 18;
			this.ErrorLabel.Text = "0";
			// 
			// FailedLabel
			// 
			this.FailedLabel.AutoSize = true;
			this.FailedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.FailedLabel.ForeColor = System.Drawing.Color.Red;
			this.FailedLabel.Location = new System.Drawing.Point(210, 98);
			this.FailedLabel.Name = "FailedLabel";
			this.FailedLabel.Size = new System.Drawing.Size(14, 13);
			this.FailedLabel.TabIndex = 17;
			this.FailedLabel.Text = "0";
			// 
			// SpeedLabel
			// 
			this.SpeedLabel.AutoSize = true;
			this.SpeedLabel.Location = new System.Drawing.Point(210, 67);
			this.SpeedLabel.Name = "SpeedLabel";
			this.SpeedLabel.Size = new System.Drawing.Size(13, 13);
			this.SpeedLabel.TabIndex = 16;
			this.SpeedLabel.Text = "0";
			// 
			// WipedSlackLabel
			// 
			this.WipedSlackLabel.AutoSize = true;
			this.WipedSlackLabel.Location = new System.Drawing.Point(210, 49);
			this.WipedSlackLabel.Name = "WipedSlackLabel";
			this.WipedSlackLabel.Size = new System.Drawing.Size(13, 13);
			this.WipedSlackLabel.TabIndex = 15;
			this.WipedSlackLabel.Text = "0";
			// 
			// WipedLabel
			// 
			this.WipedLabel.AutoSize = true;
			this.WipedLabel.Location = new System.Drawing.Point(210, 31);
			this.WipedLabel.Name = "WipedLabel";
			this.WipedLabel.Size = new System.Drawing.Size(13, 13);
			this.WipedLabel.TabIndex = 14;
			this.WipedLabel.Text = "0";
			// 
			// DurationLabel
			// 
			this.DurationLabel.AutoSize = true;
			this.DurationLabel.Location = new System.Drawing.Point(210, 13);
			this.DurationLabel.Name = "DurationLabel";
			this.DurationLabel.Size = new System.Drawing.Size(13, 13);
			this.DurationLabel.TabIndex = 13;
			this.DurationLabel.Text = "0";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.linkLabel1.Location = new System.Drawing.Point(92, 140);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(74, 15);
			this.linkLabel1.TabIndex = 12;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "View Details";
			this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Blue;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(92, 115);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(37, 13);
			this.label7.TabIndex = 10;
			this.label7.Text = "Errors:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label2.Location = new System.Drawing.Point(92, 98);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(78, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Failed items:";
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.BackColor = System.Drawing.Color.DarkGray;
			this.panel2.Location = new System.Drawing.Point(95, 88);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(672, 1);
			this.panel2.TabIndex = 8;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(92, 67);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(112, 13);
			this.label6.TabIndex = 7;
			this.label6.Text = "Average Write Speed:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(92, 49);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(102, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "Wiped Slack Space";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(92, 31);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Wiped Bytes:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(92, 13);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(50, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Duration:";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::SecureDeleteWinForms.Properties.Resources.sucessfull;
			this.pictureBox1.Location = new System.Drawing.Point(6, 10);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(77, 74);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// StatusTimer
			// 
			this.StatusTimer.Tick += new System.EventHandler(this.StatusTimer_Tick);
			// 
			// FooterPanel
			// 
			this.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.FooterPanel.Controls.Add(this.AfterCheckbox);
			this.FooterPanel.Controls.Add(this.CloseButton);
			this.FooterPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.FooterPanel.Location = new System.Drawing.Point(0, 237);
			this.FooterPanel.Name = "FooterPanel";
			this.FooterPanel.Size = new System.Drawing.Size(760, 30);
			this.FooterPanel.TabIndex = 9;
			this.FooterPanel.Visible = false;
			// 
			// AfterCheckbox
			// 
			this.AfterCheckbox.AutoSize = true;
			this.AfterCheckbox.ForeColor = System.Drawing.Color.White;
			this.AfterCheckbox.Location = new System.Drawing.Point(6, 7);
			this.AfterCheckbox.Name = "AfterCheckbox";
			this.AfterCheckbox.Size = new System.Drawing.Size(148, 17);
			this.AfterCheckbox.TabIndex = 6;
			this.AfterCheckbox.Text = "Execute afte-wipe actions";
			this.AfterCheckbox.UseVisualStyleBackColor = true;
			// 
			// CloseButton
			// 
			this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CloseButton.BackColor = System.Drawing.SystemColors.Control;
			this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CloseButton.Location = new System.Drawing.Point(677, 3);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(76, 24);
			this.CloseButton.TabIndex = 5;
			this.CloseButton.Text = "Close";
			this.CloseButton.UseVisualStyleBackColor = true;
			this.CloseButton.Visible = false;
			this.CloseButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// panelExHost1
			// 
			this.panelExHost1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelExHost1.Animate = true;
			this.panelExHost1.Controls.Add(this.TotalPanel);
			this.panelExHost1.Location = new System.Drawing.Point(6, 11);
			this.panelExHost1.Name = "panelExHost1";
			this.panelExHost1.PanelDistance = 0;
			this.panelExHost1.Size = new System.Drawing.Size(747, 68);
			this.panelExHost1.TabIndex = 18;
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
			this.TotalPanel.GradientColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(84)))), ((int)(((byte)(105)))));
			this.TotalPanel.GradientColor2 = System.Drawing.Color.LightSlateGray;
			this.TotalPanel.Location = new System.Drawing.Point(1, 1);
			this.TotalPanel.Name = "TotalPanel";
			this.TotalPanel.Size = new System.Drawing.Size(745, 64);
			this.TotalPanel.Subtitle = "sfgs";
			this.TotalPanel.TabIndex = 19;
			this.TotalPanel.TextColor = System.Drawing.Color.White;
			this.TotalPanel.Title = "Total";
			// 
			// PercentLabel
			// 
			this.PercentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.PercentLabel.AutoSize = true;
			this.PercentLabel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.PercentLabel.Location = new System.Drawing.Point(693, 39);
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
			this.TotalProgressbar.Size = new System.Drawing.Size(682, 19);
			this.TotalProgressbar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.TotalProgressbar.TabIndex = 6;
			// 
			// StatusViewHost
			// 
			this.StatusViewHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.StatusViewHost.Animate = true;
			this.StatusViewHost.AutoScroll = true;
			this.StatusViewHost.Location = new System.Drawing.Point(6, 85);
			this.StatusViewHost.Name = "StatusViewHost";
			this.StatusViewHost.PanelDistance = 0;
			this.StatusViewHost.Size = new System.Drawing.Size(747, 146);
			this.StatusViewHost.TabIndex = 2;
			// 
			// WipeTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.StatisicsPanel);
			this.Controls.Add(this.panelExHost1);
			this.Controls.Add(this.FooterPanel);
			this.Controls.Add(this.StatusViewHost);
			this.Name = "WipeTool";
			this.Size = new System.Drawing.Size(760, 267);
			this.StatisicsPanel.ResumeLayout(false);
			this.StatisicsPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.FooterPanel.ResumeLayout(false);
			this.FooterPanel.PerformLayout();
			this.panelExHost1.ResumeLayout(false);
			this.TotalPanel.ResumeLayout(false);
			this.TotalPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private SecureDeleteWinForms.PanelExHost StatusViewHost;
		private System.Windows.Forms.Timer StatusTimer;
		private System.Windows.Forms.Panel FooterPanel;
		private System.Windows.Forms.Panel StatisicsPanel;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label SpeedLabel;
		private System.Windows.Forms.Label WipedSlackLabel;
		private System.Windows.Forms.Label WipedLabel;
		private System.Windows.Forms.Label DurationLabel;
		private System.Windows.Forms.Label FailedLabel;
		private System.Windows.Forms.Label ErrorLabel;
		private System.Windows.Forms.LinkLabel SaveReportLabel;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.CheckBox AfterCheckbox;
		private PanelExHost panelExHost1;
		private PanelEx TotalPanel;
		private System.Windows.Forms.Label PercentLabel;
		private System.Windows.Forms.ProgressBar TotalProgressbar;



	}
}
