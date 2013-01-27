namespace SecureDeleteWinForms
{
	partial class ScheduleHistoryTool
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
			SecureDeleteWinForms.VisualList visualList1 = new SecureDeleteWinForms.VisualList();
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "dfgsdf"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "asdfasdgsdg", System.Drawing.SystemColors.HotTrack, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238))))}, -1);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScheduleHistoryTool));
			this.Visualizer = new SecureDeleteWinForms.HistoryVisualizer();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.OkButton = new System.Windows.Forms.ToolStripButton();
			this.ErrorButton = new System.Windows.Forms.ToolStripButton();
			this.FailedButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.DetailsButton = new System.Windows.Forms.ToolStripButton();
			this.HistogramButton = new System.Windows.Forms.ToolStripButton();
			this.SizeLabel = new System.Windows.Forms.ToolStripLabel();
			this.SizeValue = new System.Windows.Forms.ToolStripComboBox();
			this.ItemLabel = new System.Windows.Forms.ToolStripLabel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.List = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.toolStrip1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// Visualizer
			// 
			this.Visualizer.BackgroundColor = null;
			this.Visualizer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Visualizer.DurationHistogramColor = null;
			this.Visualizer.ErrorColor1 = null;
			this.Visualizer.ErrorColor2 = null;
			this.Visualizer.FailedColor1 = null;
			this.Visualizer.FailedColor2 = null;
			this.Visualizer.Location = new System.Drawing.Point(0, 0);
			this.Visualizer.Name = "Visualizer";
			this.Visualizer.NormalColor1 = null;
			this.Visualizer.NormalColor2 = null;
			this.Visualizer.PointColor = null;
			this.Visualizer.PointRadius = 0;
			this.Visualizer.SelectionColor = null;
			this.Visualizer.ShowDurationHistogram = false;
			this.Visualizer.ShowStartTime = false;
			this.Visualizer.Size = new System.Drawing.Size(798, 60);
			this.Visualizer.SuspendUpdate = false;
			this.Visualizer.TabIndex = 0;
			this.Visualizer.TextColor = null;
			visualList1.Parent = this.Visualizer;
			this.Visualizer.Visuals = visualList1;
			this.Visualizer.VisualWidth = 0;
			this.Visualizer.OnSelectionChanged += new SecureDeleteWinForms.HistoryVisualizer.SelectedVisualDelegate(this.Visualizer_OnSelectionChanged);
			// 
			// toolStrip1
			// 
			this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.OkButton,
            this.ErrorButton,
            this.FailedButton,
            this.toolStripSeparator1,
            this.DetailsButton,
            this.HistogramButton,
            this.SizeLabel,
            this.SizeValue,
            this.ItemLabel});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
			this.toolStrip1.Size = new System.Drawing.Size(798, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::SecureDeleteWinForms.Properties.Resources.RepeatHS;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripButton1.ToolTipText = "Reload";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click_2);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.Image = global::SecureDeleteWinForms.Properties.Resources.delete1;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(54, 22);
			this.toolStripButton2.Text = "Clear";
			this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(36, 22);
			this.toolStripLabel1.Text = "Show";
			// 
			// OkButton
			// 
			this.OkButton.Checked = true;
			this.OkButton.CheckOnClick = true;
			this.OkButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.OkButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.OkButton.Image = global::SecureDeleteWinForms.Properties.Resources.OK;
			this.OkButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(23, 22);
			this.OkButton.Text = "toolStripButton3";
			this.OkButton.ToolTipText = "Completed";
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			// 
			// ErrorButton
			// 
			this.ErrorButton.Checked = true;
			this.ErrorButton.CheckOnClick = true;
			this.ErrorButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ErrorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ErrorButton.Image = global::SecureDeleteWinForms.Properties.Resources.warning1;
			this.ErrorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ErrorButton.Name = "ErrorButton";
			this.ErrorButton.Size = new System.Drawing.Size(23, 22);
			this.ErrorButton.Text = "toolStripButton2";
			this.ErrorButton.ToolTipText = "Completed with Errors";
			this.ErrorButton.Click += new System.EventHandler(this.ErrorButton_Click);
			// 
			// FailedButton
			// 
			this.FailedButton.Checked = true;
			this.FailedButton.CheckOnClick = true;
			this.FailedButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.FailedButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.FailedButton.Image = global::SecureDeleteWinForms.Properties.Resources.error;
			this.FailedButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.FailedButton.Name = "FailedButton";
			this.FailedButton.Size = new System.Drawing.Size(23, 22);
			this.FailedButton.Text = "toolStripButton1";
			this.FailedButton.ToolTipText = "Failed";
			this.FailedButton.Click += new System.EventHandler(this.FailedButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// DetailsButton
			// 
			this.DetailsButton.Checked = true;
			this.DetailsButton.CheckOnClick = true;
			this.DetailsButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.DetailsButton.Image = global::SecureDeleteWinForms.Properties.Resources.top_panel;
			this.DetailsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.DetailsButton.Name = "DetailsButton";
			this.DetailsButton.Size = new System.Drawing.Size(94, 22);
			this.DetailsButton.Text = "Details Panel";
			this.DetailsButton.Click += new System.EventHandler(this.DetailsButton_Click);
			// 
			// HistogramButton
			// 
			this.HistogramButton.Checked = true;
			this.HistogramButton.CheckOnClick = true;
			this.HistogramButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.HistogramButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.HistogramButton.Image = global::SecureDeleteWinForms.Properties.Resources.performance;
			this.HistogramButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.HistogramButton.Name = "HistogramButton";
			this.HistogramButton.Size = new System.Drawing.Size(23, 22);
			this.HistogramButton.Text = "Histogram";
			this.HistogramButton.ToolTipText = "Duration Histogram";
			this.HistogramButton.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// SizeLabel
			// 
			this.SizeLabel.Name = "SizeLabel";
			this.SizeLabel.Size = new System.Drawing.Size(27, 22);
			this.SizeLabel.Text = "Size";
			// 
			// SizeValue
			// 
			this.SizeValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SizeValue.Items.AddRange(new object[] {
            "Small",
            "Medium",
            "Large"});
			this.SizeValue.Name = "SizeValue";
			this.SizeValue.Size = new System.Drawing.Size(121, 25);
			this.SizeValue.SelectedIndexChanged += new System.EventHandler(this.SizeValue_SelectedIndexChanged);
			// 
			// ItemLabel
			// 
			this.ItemLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.ItemLabel.Name = "ItemLabel";
			this.ItemLabel.Size = new System.Drawing.Size(48, 22);
			this.ItemLabel.Text = "Items: 0";
			// 
			// splitContainer1
			// 
			this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 25);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.Visualizer);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.List);
			this.splitContainer1.Size = new System.Drawing.Size(798, 419);
			this.splitContainer1.SplitterDistance = 60;
			this.splitContainer1.SplitterWidth = 1;
			this.splitContainer1.TabIndex = 1;
			// 
			// List
			// 
			this.List.BackColor = System.Drawing.SystemColors.Window;
			this.List.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader6,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
			this.List.Dock = System.Windows.Forms.DockStyle.Fill;
			this.List.FullRowSelect = true;
			this.List.HideSelection = false;
			listViewItem1.UseItemStyleForSubItems = false;
			this.List.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.List.Location = new System.Drawing.Point(0, 0);
			this.List.MultiSelect = false;
			this.List.Name = "List";
			this.List.Size = new System.Drawing.Size(798, 358);
			this.List.SmallImageList = this.imageList1;
			this.List.TabIndex = 3;
			this.List.UseCompatibleStateImageBehavior = false;
			this.List.View = System.Windows.Forms.View.Details;
			this.List.SelectedIndexChanged += new System.EventHandler(this.List_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Date";
			this.columnHeader1.Width = 125;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Status";
			this.columnHeader2.Width = 239;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Duration";
			this.columnHeader6.Width = 102;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Failed Objects";
			this.columnHeader3.Width = 105;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Errors";
			this.columnHeader4.Width = 77;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Report";
			this.columnHeader5.Width = 78;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "warning.png");
			this.imageList1.Images.SetKeyName(1, "error.png");
			// 
			// ScheduleHistoryTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "ScheduleHistoryTool";
			this.Size = new System.Drawing.Size(798, 444);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStripButton FailedButton;
		private HistoryVisualizer Visualizer;
		private System.Windows.Forms.ListView List;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripButton ErrorButton;
		private System.Windows.Forms.ToolStripButton OkButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton DetailsButton;
		private System.Windows.Forms.ToolStripLabel SizeLabel;
		private System.Windows.Forms.ToolStripComboBox SizeValue;
		private System.Windows.Forms.ToolStripLabel ItemLabel;
		private System.Windows.Forms.ToolStripButton HistogramButton;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.ColumnHeader columnHeader6;


	}
}
