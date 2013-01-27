namespace SecureDeleteWinForms
{
	partial class PowerShellObjectResults
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PowerShellObjectResults));
			this.panel3 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.MemberList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.MemberButton = new System.Windows.Forms.ToolStripButton();
			this.PropertyButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.SearchTextbox = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ActionLabel = new System.Windows.Forms.Label();
			this.PropertyList = new System.Windows.Forms.ListView();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.ParameterList = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.label3 = new System.Windows.Forms.Label();
			this.TypeLabel = new System.Windows.Forms.Label();
			this.MemberLabel = new System.Windows.Forms.Label();
			this.ObjectLabel = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel3.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.button1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 451);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(763, 30);
			this.panel3.TabIndex = 6;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.BackColor = System.Drawing.SystemColors.Control;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(684, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(76, 24);
			this.button1.TabIndex = 4;
			this.button1.Text = "Close";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer1.Panel1.Controls.Add(this.MemberList);
			this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
			this.splitContainer1.Panel1.Controls.Add(this.panel1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer1.Panel2.Controls.Add(this.PropertyList);
			this.splitContainer1.Panel2.Controls.Add(this.ParameterList);
			this.splitContainer1.Panel2.Controls.Add(this.label3);
			this.splitContainer1.Panel2.Controls.Add(this.TypeLabel);
			this.splitContainer1.Panel2.Controls.Add(this.MemberLabel);
			this.splitContainer1.Panel2.Controls.Add(this.ObjectLabel);
			this.splitContainer1.Panel2.Controls.Add(this.panel2);
			this.splitContainer1.Size = new System.Drawing.Size(763, 451);
			this.splitContainer1.SplitterDistance = 398;
			this.splitContainer1.SplitterWidth = 2;
			this.splitContainer1.TabIndex = 7;
			// 
			// MemberList
			// 
			this.MemberList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.MemberList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MemberList.FullRowSelect = true;
			this.MemberList.HideSelection = false;
			this.MemberList.Location = new System.Drawing.Point(0, 24);
			this.MemberList.Name = "MemberList";
			this.MemberList.Size = new System.Drawing.Size(398, 402);
			this.MemberList.SmallImageList = this.imageList1;
			this.MemberList.TabIndex = 5;
			this.MemberList.UseCompatibleStateImageBehavior = false;
			this.MemberList.View = System.Windows.Forms.View.Details;
			this.MemberList.SelectedIndexChanged += new System.EventHandler(this.MemberList_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Time";
			this.columnHeader1.Width = 80;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Object";
			this.columnHeader2.Width = 120;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Member";
			this.columnHeader3.Width = 150;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "logical.ico");
			this.imageList1.Images.SetKeyName(1, "variable.ico");
			this.imageList1.Images.SetKeyName(2, "local.ico");
			// 
			// toolStrip1
			// 
			this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
			this.toolStrip1.CanOverflow = false;
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MemberButton,
            this.PropertyButton,
            this.toolStripButton3,
            this.SearchTextbox,
            this.toolStripLabel1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 426);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 1, 0);
			this.toolStrip1.Size = new System.Drawing.Size(398, 25);
			this.toolStrip1.TabIndex = 6;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// MemberButton
			// 
			this.MemberButton.Checked = true;
			this.MemberButton.CheckOnClick = true;
			this.MemberButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.MemberButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.MemberButton.Image = global::SecureDeleteWinForms.Properties.Resources.method;
			this.MemberButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.MemberButton.Name = "MemberButton";
			this.MemberButton.Size = new System.Drawing.Size(23, 22);
			this.MemberButton.Text = "Show/Hide Methods";
			this.MemberButton.Click += new System.EventHandler(this.MemberButton_Click);
			// 
			// PropertyButton
			// 
			this.PropertyButton.Checked = true;
			this.PropertyButton.CheckOnClick = true;
			this.PropertyButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.PropertyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.PropertyButton.Image = global::SecureDeleteWinForms.Properties.Resources.property;
			this.PropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.PropertyButton.Name = "PropertyButton";
			this.PropertyButton.Size = new System.Drawing.Size(23, 22);
			this.PropertyButton.Text = "Show/Hide Properties";
			this.PropertyButton.Click += new System.EventHandler(this.PropertyButton_Click);
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.Image = global::SecureDeleteWinForms.Properties.Resources.delete_profile;
			this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton3.Text = "toolStripButton3";
			this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
			// 
			// SearchTextbox
			// 
			this.SearchTextbox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.SearchTextbox.BackColor = System.Drawing.Color.White;
			this.SearchTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SearchTextbox.Name = "SearchTextbox";
			this.SearchTextbox.Size = new System.Drawing.Size(150, 25);
			this.SearchTextbox.TextChanged += new System.EventHandler(this.SearchTextbox_TextChanged);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(42, 22);
			this.toolStripLabel1.Text = "Search";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel1.Controls.Add(this.ActionLabel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(398, 24);
			this.panel1.TabIndex = 4;
			// 
			// ActionLabel
			// 
			this.ActionLabel.AutoSize = true;
			this.ActionLabel.ForeColor = System.Drawing.Color.White;
			this.ActionLabel.Location = new System.Drawing.Point(3, 6);
			this.ActionLabel.Name = "ActionLabel";
			this.ActionLabel.Size = new System.Drawing.Size(110, 13);
			this.ActionLabel.TabIndex = 1;
			this.ActionLabel.Text = "Registered actions (0)";
			// 
			// PropertyList
			// 
			this.PropertyList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.PropertyList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7});
			this.PropertyList.FullRowSelect = true;
			this.PropertyList.Location = new System.Drawing.Point(6, 308);
			this.PropertyList.Name = "PropertyList";
			this.PropertyList.Size = new System.Drawing.Size(348, 137);
			this.PropertyList.SmallImageList = this.imageList1;
			this.PropertyList.TabIndex = 10;
			this.PropertyList.UseCompatibleStateImageBehavior = false;
			this.PropertyList.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Property Name";
			this.columnHeader6.Width = 120;
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Property Value";
			this.columnHeader7.Width = 220;
			// 
			// ParameterList
			// 
			this.ParameterList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ParameterList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
			this.ParameterList.FullRowSelect = true;
			this.ParameterList.HideSelection = false;
			this.ParameterList.Location = new System.Drawing.Point(6, 97);
			this.ParameterList.Name = "ParameterList";
			this.ParameterList.Size = new System.Drawing.Size(348, 212);
			this.ParameterList.SmallImageList = this.imageList1;
			this.ParameterList.TabIndex = 9;
			this.ParameterList.UseCompatibleStateImageBehavior = false;
			this.ParameterList.View = System.Windows.Forms.View.Details;
			this.ParameterList.SelectedIndexChanged += new System.EventHandler(this.ParameterList_SelectedIndexChanged);
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Name";
			this.columnHeader4.Width = 120;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Value";
			this.columnHeader5.Width = 220;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 81);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(63, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Parameters:";
			// 
			// TypeLabel
			// 
			this.TypeLabel.AutoSize = true;
			this.TypeLabel.Location = new System.Drawing.Point(6, 64);
			this.TypeLabel.Name = "TypeLabel";
			this.TypeLabel.Size = new System.Drawing.Size(34, 13);
			this.TypeLabel.TabIndex = 7;
			this.TypeLabel.Text = "Type:";
			// 
			// MemberLabel
			// 
			this.MemberLabel.AutoSize = true;
			this.MemberLabel.Location = new System.Drawing.Point(6, 47);
			this.MemberLabel.Name = "MemberLabel";
			this.MemberLabel.Size = new System.Drawing.Size(48, 13);
			this.MemberLabel.TabIndex = 6;
			this.MemberLabel.Text = "Member:";
			// 
			// ObjectLabel
			// 
			this.ObjectLabel.AutoSize = true;
			this.ObjectLabel.Location = new System.Drawing.Point(6, 30);
			this.ObjectLabel.Name = "ObjectLabel";
			this.ObjectLabel.Size = new System.Drawing.Size(41, 13);
			this.ObjectLabel.TabIndex = 5;
			this.ObjectLabel.Text = "Object:";
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(363, 24);
			this.panel2.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Member details";
			// 
			// PowerShellObjectResults
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(763, 481);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.panel3);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PowerShellObjectResults";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Exposed Objects Results";
			this.Load += new System.EventHandler(this.PowerShellObjectResults_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PowerShellObjectResults_FormClosing);
			this.panel3.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label ActionLabel;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView MemberList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.Label TypeLabel;
		private System.Windows.Forms.Label MemberLabel;
		private System.Windows.Forms.Label ObjectLabel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListView ParameterList;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ListView PropertyList;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton MemberButton;
		private System.Windows.Forms.ToolStripButton PropertyButton;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripTextBox SearchTextbox;
		private System.Windows.Forms.ToolStripButton toolStripButton3;
	}
}