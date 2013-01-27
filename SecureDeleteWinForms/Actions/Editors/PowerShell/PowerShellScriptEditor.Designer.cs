namespace SecureDeleteWinForms
{
	partial class PowerShellScriptEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PowerShellScriptEditor));
			this.EditorPanel = new System.Windows.Forms.Panel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.ScriptEditor = new System.Windows.Forms.RichTextBox();
			this.ResultsBox = new System.Windows.Forms.RichTextBox();
			this.Browser = new System.Windows.Forms.WebBrowser();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.ExposedResultsButton = new System.Windows.Forms.ToolStripSplitButton();
			this.StateLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.EditorToolbar = new System.Windows.Forms.ToolStrip();
			this.NewButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.OpenButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.RunButton = new System.Windows.Forms.ToolStripButton();
			this.StopButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.ExposedButton = new System.Windows.Forms.ToolStripButton();
			this.SizeCombobox = new System.Windows.Forms.ToolStripComboBox();
			this.FontCombobox = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.CancelButton = new System.Windows.Forms.Button();
			this.InsertButton = new System.Windows.Forms.Button();
			this.EditorPanel.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.EditorToolbar.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// EditorPanel
			// 
			this.EditorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.EditorPanel.Controls.Add(this.splitContainer1);
			this.EditorPanel.Controls.Add(this.EditorToolbar);
			this.EditorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EditorPanel.Location = new System.Drawing.Point(0, 0);
			this.EditorPanel.Name = "EditorPanel";
			this.EditorPanel.Size = new System.Drawing.Size(754, 486);
			this.EditorPanel.TabIndex = 27;
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
			this.splitContainer1.Panel1.Controls.Add(this.ScriptEditor);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.ResultsBox);
			this.splitContainer1.Panel2.Controls.Add(this.Browser);
			this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
			this.splitContainer1.Size = new System.Drawing.Size(752, 459);
			this.splitContainer1.SplitterDistance = 305;
			this.splitContainer1.SplitterWidth = 2;
			this.splitContainer1.TabIndex = 3;
			// 
			// ScriptEditor
			// 
			this.ScriptEditor.BackColor = System.Drawing.SystemColors.Window;
			this.ScriptEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ScriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ScriptEditor.Location = new System.Drawing.Point(0, 0);
			this.ScriptEditor.Name = "ScriptEditor";
			this.ScriptEditor.Size = new System.Drawing.Size(752, 305);
			this.ScriptEditor.TabIndex = 1;
			this.ScriptEditor.Text = "";
			this.ScriptEditor.TextChanged += new System.EventHandler(this.ScriptEditor_TextChanged);
			// 
			// ResultsBox
			// 
			this.ResultsBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ResultsBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ResultsBox.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.ResultsBox.ForeColor = System.Drawing.SystemColors.WindowText;
			this.ResultsBox.Location = new System.Drawing.Point(0, 22);
			this.ResultsBox.Name = "ResultsBox";
			this.ResultsBox.ReadOnly = true;
			this.ResultsBox.Size = new System.Drawing.Size(752, 130);
			this.ResultsBox.TabIndex = 0;
			this.ResultsBox.Text = "Script results";
			this.ResultsBox.WordWrap = false;
			// 
			// Browser
			// 
			this.Browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Browser.Location = new System.Drawing.Point(0, 22);
			this.Browser.MinimumSize = new System.Drawing.Size(20, 20);
			this.Browser.Name = "Browser";
			this.Browser.Size = new System.Drawing.Size(752, 130);
			this.Browser.TabIndex = 1;
			this.Browser.Visible = false;
			// 
			// statusStrip1
			// 
			this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Top;
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.ExposedResultsButton,
            this.StateLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 0);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
			this.statusStrip1.Size = new System.Drawing.Size(752, 22);
			this.statusStrip1.SizingGrip = false;
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(44, 17);
			this.toolStripStatusLabel1.Text = "Results";
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(693, 17);
			this.toolStripStatusLabel2.Spring = true;
			// 
			// ExposedResultsButton
			// 
			this.ExposedResultsButton.DropDownButtonWidth = 0;
			this.ExposedResultsButton.ForeColor = System.Drawing.Color.White;
			this.ExposedResultsButton.Image = global::SecureDeleteWinForms.Properties.Resources.Untitled;
			this.ExposedResultsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ExposedResultsButton.Name = "ExposedResultsButton";
			this.ExposedResultsButton.Size = new System.Drawing.Size(154, 20);
			this.ExposedResultsButton.Text = "Exposed Objects Results";
			this.ExposedResultsButton.Visible = false;
			this.ExposedResultsButton.MouseLeave += new System.EventHandler(this.ExposedResultsButton_MouseLeave);
			this.ExposedResultsButton.Click += new System.EventHandler(this.ExposedResultsButton_Click);
			this.ExposedResultsButton.MouseEnter += new System.EventHandler(this.ExposedResultsButton_MouseEnter);
			// 
			// StateLabel
			// 
			this.StateLabel.ForeColor = System.Drawing.Color.White;
			this.StateLabel.Image = global::SecureDeleteWinForms.Properties.Resources.powershell_small;
			this.StateLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.StateLabel.Name = "StateLabel";
			this.StateLabel.Size = new System.Drawing.Size(98, 17);
			this.StateLabel.Text = "Script running";
			this.StateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.StateLabel.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
			this.StateLabel.Visible = false;
			// 
			// EditorToolbar
			// 
			this.EditorToolbar.BackColor = System.Drawing.SystemColors.Control;
			this.EditorToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.EditorToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewButton,
            this.toolStripSeparator1,
            this.OpenButton,
            this.toolStripButton3,
            this.toolStripSeparator2,
            this.RunButton,
            this.StopButton,
            this.toolStripSeparator3,
            this.ExposedButton,
            this.SizeCombobox,
            this.FontCombobox,
            this.toolStripButton4,
            this.toolStripLabel1});
			this.EditorToolbar.Location = new System.Drawing.Point(0, 0);
			this.EditorToolbar.Name = "EditorToolbar";
			this.EditorToolbar.Padding = new System.Windows.Forms.Padding(3, 0, 1, 0);
			this.EditorToolbar.Size = new System.Drawing.Size(752, 25);
			this.EditorToolbar.TabIndex = 2;
			this.EditorToolbar.Text = "toolStrip1";
			// 
			// NewButton
			// 
			this.NewButton.Image = global::SecureDeleteWinForms.Properties.Resources.DocumentHS;
			this.NewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.NewButton.Name = "NewButton";
			this.NewButton.Size = new System.Drawing.Size(51, 22);
			this.NewButton.Text = "New";
			this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// OpenButton
			// 
			this.OpenButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.OpenButton.Image = global::SecureDeleteWinForms.Properties.Resources.openHS;
			this.OpenButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.OpenButton.Name = "OpenButton";
			this.OpenButton.Size = new System.Drawing.Size(23, 22);
			this.OpenButton.Text = "Open";
			this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.Image = global::SecureDeleteWinForms.Properties.Resources.save;
			this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton3.Text = "Save";
			this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// RunButton
			// 
			this.RunButton.Enabled = false;
			this.RunButton.Image = global::SecureDeleteWinForms.Properties.Resources.dfsd;
			this.RunButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RunButton.Name = "RunButton";
			this.RunButton.Size = new System.Drawing.Size(81, 22);
			this.RunButton.Text = "Run Script";
			this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
			// 
			// StopButton
			// 
			this.StopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.StopButton.Enabled = false;
			this.StopButton.Image = global::SecureDeleteWinForms.Properties.Resources.safdgsd;
			this.StopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.StopButton.Name = "StopButton";
			this.StopButton.Size = new System.Drawing.Size(23, 22);
			this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// ExposedButton
			// 
			this.ExposedButton.Image = global::SecureDeleteWinForms.Properties.Resources.class1;
			this.ExposedButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ExposedButton.Name = "ExposedButton";
			this.ExposedButton.Size = new System.Drawing.Size(113, 22);
			this.ExposedButton.Text = "Exposed Objects";
			this.ExposedButton.Click += new System.EventHandler(this.ExposedButton_Click);
			// 
			// SizeCombobox
			// 
			this.SizeCombobox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.SizeCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SizeCombobox.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24"});
			this.SizeCombobox.Name = "SizeCombobox";
			this.SizeCombobox.Size = new System.Drawing.Size(75, 25);
			this.SizeCombobox.SelectedIndexChanged += new System.EventHandler(this.SizeCombobox_SelectedIndexChanged);
			// 
			// FontCombobox
			// 
			this.FontCombobox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.FontCombobox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this.FontCombobox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.FontCombobox.Name = "FontCombobox";
			this.FontCombobox.Size = new System.Drawing.Size(150, 25);
			this.FontCombobox.SelectedIndexChanged += new System.EventHandler(this.FontCombobox_SelectedIndexChanged);
			// 
			// toolStripButton4
			// 
			this.toolStripButton4.Image = global::SecureDeleteWinForms.Properties.Resources.powershell_small;
			this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.Size = new System.Drawing.Size(109, 22);
			this.toolStripButton4.Text = "PowerShell Info";
			this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(31, 22);
			this.toolStripLabel1.Text = "Font";
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.CancelButton);
			this.panel3.Controls.Add(this.InsertButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 486);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(754, 30);
			this.panel3.TabIndex = 28;
			// 
			// CancelButton
			// 
			this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelButton.BackColor = System.Drawing.Color.AliceBlue;
			this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelButton.Location = new System.Drawing.Point(675, 3);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(76, 24);
			this.CancelButton.TabIndex = 4;
			this.CancelButton.Text = "Close";
			this.CancelButton.UseVisualStyleBackColor = true;
			// 
			// InsertButton
			// 
			this.InsertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.InsertButton.BackColor = System.Drawing.SystemColors.Control;
			this.InsertButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.InsertButton.Image = global::SecureDeleteWinForms.Properties.Resources.save;
			this.InsertButton.Location = new System.Drawing.Point(597, 3);
			this.InsertButton.Name = "InsertButton";
			this.InsertButton.Size = new System.Drawing.Size(76, 24);
			this.InsertButton.TabIndex = 3;
			this.InsertButton.Text = "Save";
			this.InsertButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.InsertButton.UseVisualStyleBackColor = true;
			// 
			// PowerShellScriptEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(754, 516);
			this.Controls.Add(this.EditorPanel);
			this.Controls.Add(this.panel3);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "PowerShellScriptEditor";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "PowerShell Script Editor";
			this.Load += new System.EventHandler(this.PowerShellScriptEditor_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PowerShellScriptEditor_FormClosing);
			this.EditorPanel.ResumeLayout(false);
			this.EditorPanel.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.EditorToolbar.ResumeLayout(false);
			this.EditorToolbar.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel EditorPanel;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.RichTextBox ScriptEditor;
		private System.Windows.Forms.RichTextBox ResultsBox;
		private System.Windows.Forms.WebBrowser Browser;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.ToolStripSplitButton ExposedResultsButton;
		private System.Windows.Forms.ToolStripStatusLabel StateLabel;
		private System.Windows.Forms.ToolStrip EditorToolbar;
		private System.Windows.Forms.ToolStripButton NewButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton OpenButton;
		private System.Windows.Forms.ToolStripButton toolStripButton3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton RunButton;
		private System.Windows.Forms.ToolStripButton StopButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton ExposedButton;
		private System.Windows.Forms.ToolStripComboBox SizeCombobox;
		private System.Windows.Forms.ToolStripComboBox FontCombobox;
		private System.Windows.Forms.ToolStripButton toolStripButton4;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Button InsertButton;
	}
}