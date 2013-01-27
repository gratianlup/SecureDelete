namespace SecureDeleteWinForms
{
	partial class AboutBox
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
			this.panel3 = new System.Windows.Forms.Panel();
			this.CancelButton = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.PluginSelector = new SecureDeleteWinForms.PanelSelectControl();
			this.AboutSelector = new SecureDeleteWinForms.PanelSelectControl();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.AboutPanel = new System.Windows.Forms.Panel();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.PluginPanel = new System.Windows.Forms.Panel();
			this.PluginList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.AboutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.PluginPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.CancelButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 375);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(559, 30);
			this.panel3.TabIndex = 28;
			// 
			// CancelButton
			// 
			this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelButton.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelButton.Location = new System.Drawing.Point(478, 3);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(76, 24);
			this.CancelButton.TabIndex = 4;
			this.CancelButton.Text = "Close";
			this.CancelButton.UseVisualStyleBackColor = false;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.White;
			this.panel2.Controls.Add(this.pictureBox3);
			this.panel2.Controls.Add(this.PluginSelector);
			this.panel2.Controls.Add(this.AboutSelector);
			this.panel2.Controls.Add(this.pictureBox2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(559, 91);
			this.panel2.TabIndex = 29;
			// 
			// pictureBox3
			// 
			this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Right;
			this.pictureBox3.Image = global::SecureDeleteWinForms.Properties.Resources.crash2;
			this.pictureBox3.Location = new System.Drawing.Point(371, 0);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(188, 91);
			this.pictureBox3.TabIndex = 18;
			this.pictureBox3.TabStop = false;
			this.pictureBox3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox3_MouseUp);
			// 
			// PluginSelector
			// 
			this.PluginSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.PluginSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.PluginSelector.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.PluginSelector.Location = new System.Drawing.Point(70, 70);
			this.PluginSelector.Name = "PluginSelector";
			this.PluginSelector.Reversed = false;
			this.PluginSelector.Selected = false;
			this.PluginSelector.SelectedColor = System.Drawing.Color.White;
			this.PluginSelector.SelectedTextColor = System.Drawing.Color.Black;
			this.PluginSelector.SelectorText = "Plugin info";
			this.PluginSelector.Size = new System.Drawing.Size(66, 21);
			this.PluginSelector.TabIndex = 31;
			this.PluginSelector.TextColor = System.Drawing.Color.White;
			this.PluginSelector.SelectedStateChanged += new System.EventHandler(this.PluginSelector_SelectedStateChanged);
			// 
			// AboutSelector
			// 
			this.AboutSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.AboutSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.AboutSelector.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.AboutSelector.Location = new System.Drawing.Point(17, 70);
			this.AboutSelector.Name = "AboutSelector";
			this.AboutSelector.Reversed = false;
			this.AboutSelector.Selected = false;
			this.AboutSelector.SelectedColor = System.Drawing.Color.White;
			this.AboutSelector.SelectedTextColor = System.Drawing.Color.Black;
			this.AboutSelector.SelectorText = "About";
			this.AboutSelector.Size = new System.Drawing.Size(49, 21);
			this.AboutSelector.TabIndex = 30;
			this.AboutSelector.TextColor = System.Drawing.Color.White;
			this.AboutSelector.SelectedStateChanged += new System.EventHandler(this.AboutSelector_SelectedStateChanged);
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::SecureDeleteWinForms.Properties.Resources.SecureDelete_about3;
			this.pictureBox2.Location = new System.Drawing.Point(0, 1);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(347, 92);
			this.pictureBox2.TabIndex = 17;
			this.pictureBox2.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Black;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 91);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(559, 1);
			this.panel1.TabIndex = 30;
			// 
			// AboutPanel
			// 
			this.AboutPanel.BackColor = System.Drawing.Color.White;
			this.AboutPanel.Controls.Add(this.pictureBox4);
			this.AboutPanel.Controls.Add(this.label3);
			this.AboutPanel.Controls.Add(this.label2);
			this.AboutPanel.Controls.Add(this.label1);
			this.AboutPanel.Controls.Add(this.pictureBox1);
			this.AboutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AboutPanel.Location = new System.Drawing.Point(0, 92);
			this.AboutPanel.Name = "AboutPanel";
			this.AboutPanel.Size = new System.Drawing.Size(559, 283);
			this.AboutPanel.TabIndex = 31;
			// 
			// pictureBox4
			// 
			this.pictureBox4.Image = global::SecureDeleteWinForms.Properties.Resources.logo_sd_copy;
			this.pictureBox4.Location = new System.Drawing.Point(15, 12);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(50, 50);
			this.pictureBox4.TabIndex = 4;
			this.pictureBox4.TabStop = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(70, 45);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(151, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Copyright (c) 2008 Lup Gratian";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(70, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Version 0.8";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(70, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "SecureDelete";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.Image = global::SecureDeleteWinForms.Properties.Resources.Untitled_31;
			this.pictureBox1.Location = new System.Drawing.Point(0, 72);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(559, 213);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// PluginPanel
			// 
			this.PluginPanel.BackColor = System.Drawing.Color.White;
			this.PluginPanel.Controls.Add(this.PluginList);
			this.PluginPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PluginPanel.Location = new System.Drawing.Point(0, 92);
			this.PluginPanel.Name = "PluginPanel";
			this.PluginPanel.Size = new System.Drawing.Size(559, 283);
			this.PluginPanel.TabIndex = 32;
			// 
			// PluginList
			// 
			this.PluginList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.PluginList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.PluginList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PluginList.FullRowSelect = true;
			this.PluginList.Location = new System.Drawing.Point(0, 0);
			this.PluginList.Name = "PluginList";
			this.PluginList.Size = new System.Drawing.Size(559, 283);
			this.PluginList.TabIndex = 0;
			this.PluginList.UseCompatibleStateImageBehavior = false;
			this.PluginList.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Assembly";
			this.columnHeader1.Width = 200;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Version";
			this.columnHeader2.Width = 170;
			// 
			// AboutBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(559, 405);
			this.ControlBox = false;
			this.Controls.Add(this.AboutPanel);
			this.Controls.Add(this.PluginPanel);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(570, 420);
			this.Name = "AboutBox";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.AboutBox_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AboutBox_KeyDown);
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.AboutPanel.ResumeLayout(false);
			this.AboutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.PluginPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox pictureBox3;
		private PanelSelectControl AboutSelector;
		private PanelSelectControl PluginSelector;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel AboutPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel PluginPanel;
		private System.Windows.Forms.ListView PluginList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox4;

	}
}