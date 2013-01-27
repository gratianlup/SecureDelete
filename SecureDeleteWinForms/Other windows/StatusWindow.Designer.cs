namespace SecureDeleteWinForms
{
	partial class StatusWindow
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusWindow));
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.StatusHost = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.ToolHeader = new System.Windows.Forms.Panel();
			this.ToolHeaderLabel = new System.Windows.Forms.Label();
			this.ToolCloseButton = new System.Windows.Forms.Button();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.panel3.SuspendLayout();
			this.ToolHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.White;
			this.panel3.Controls.Add(this.panel1);
			this.panel3.Controls.Add(this.linkLabel1);
			this.panel3.Controls.Add(this.StatusHost);
			this.panel3.Controls.Add(this.button1);
			this.panel3.Controls.Add(this.ToolHeader);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(355, 130);
			this.panel3.TabIndex = 6;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.Color.DarkGray;
			this.panel1.Location = new System.Drawing.Point(0, 101);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(356, 1);
			this.panel1.TabIndex = 13;
			// 
			// linkLabel1
			// 
			this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(5, 109);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(102, 13);
			this.linkLabel1.TabIndex = 12;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Show SecureDelete";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// StatusHost
			// 
			this.StatusHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.StatusHost.Location = new System.Drawing.Point(-1, 24);
			this.StatusHost.Name = "StatusHost";
			this.StatusHost.Size = new System.Drawing.Size(357, 74);
			this.StatusHost.TabIndex = 11;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.BackColor = System.Drawing.SystemColors.Control;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(277, 104);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 10;
			this.button1.Text = "Stop All";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// ToolHeader
			// 
			this.ToolHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ToolHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.ToolHeader.Controls.Add(this.ToolHeaderLabel);
			this.ToolHeader.Controls.Add(this.ToolCloseButton);
			this.ToolHeader.Location = new System.Drawing.Point(-1, 0);
			this.ToolHeader.Name = "ToolHeader";
			this.ToolHeader.Size = new System.Drawing.Size(355, 24);
			this.ToolHeader.TabIndex = 6;
			// 
			// ToolHeaderLabel
			// 
			this.ToolHeaderLabel.AutoSize = true;
			this.ToolHeaderLabel.ForeColor = System.Drawing.Color.White;
			this.ToolHeaderLabel.Location = new System.Drawing.Point(3, 5);
			this.ToolHeaderLabel.Name = "ToolHeaderLabel";
			this.ToolHeaderLabel.Size = new System.Drawing.Size(72, 13);
			this.ToolHeaderLabel.TabIndex = 1;
			this.ToolHeaderLabel.Text = "SecureDelete";
			// 
			// ToolCloseButton
			// 
			this.ToolCloseButton.AutoSize = true;
			this.ToolCloseButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ToolCloseButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.ToolCloseButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.ToolCloseButton.FlatAppearance.BorderSize = 0;
			this.ToolCloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ToolCloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.ToolCloseButton.ForeColor = System.Drawing.Color.White;
			this.ToolCloseButton.Location = new System.Drawing.Point(330, 0);
			this.ToolCloseButton.Name = "ToolCloseButton";
			this.ToolCloseButton.Size = new System.Drawing.Size(25, 24);
			this.ToolCloseButton.TabIndex = 0;
			this.ToolCloseButton.Text = "X";
			this.ToolCloseButton.UseVisualStyleBackColor = false;
			this.ToolCloseButton.Click += new System.EventHandler(this.ToolCloseButton_Click);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "safdgsd.ico");
			// 
			// StatusWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(355, 130);
			this.ControlBox = false;
			this.Controls.Add(this.panel3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StatusWindow";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.TopMost = true;
			this.VisibleChanged += new System.EventHandler(this.StatusWindow_VisibleChanged);
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.ToolHeader.ResumeLayout(false);
			this.ToolHeader.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel ToolHeader;
		private System.Windows.Forms.Label ToolHeaderLabel;
		private System.Windows.Forms.Button ToolCloseButton;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Panel StatusHost;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Panel panel1;
	}
}