namespace SecureDeleteWinForms.Modules
{
	partial class ReportModule
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportModule));
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Default");
			this.MenuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ViewMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.xMLToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.textFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.hTMLToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deselectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.removeSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeSelectedToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.removeAllToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.removeAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.removeSelectedToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.removeAllToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SearchMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.ReportList = new System.Windows.Forms.ListView();
			this.Date = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.ViewButton = new System.Windows.Forms.ToolStripButton();
			this.RemoveButton = new System.Windows.Forms.ToolStripSplitButton();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ExportButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.textFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.xMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.SearchButton = new System.Windows.Forms.ToolStripButton();
			this.ToolHost = new System.Windows.Forms.Panel();
			this.ToolHeader = new System.Windows.Forms.Panel();
			this.ToolCloseButton = new System.Windows.Forms.Button();
			this.ToolHeaderIcon = new System.Windows.Forms.PictureBox();
			this.ToolHeaderLabel = new System.Windows.Forms.Label();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.ReportCategories = new System.Windows.Forms.TreeView();
			this.imageList2 = new System.Windows.Forms.ImageList(this.components);
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.MenuStrip.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.ToolHeader.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ToolHeaderIcon)).BeginInit();
			this.toolStrip2.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// MenuStrip
			// 
			this.MenuStrip.BackColor = System.Drawing.SystemColors.Control;
			this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.MenuStrip.Location = new System.Drawing.Point(0, 0);
			this.MenuStrip.Name = "MenuStrip";
			this.MenuStrip.Size = new System.Drawing.Size(759, 24);
			this.MenuStrip.TabIndex = 1;
			this.MenuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewMenu,
            this.exportToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// ViewMenu
			// 
			this.ViewMenu.Image = global::SecureDeleteWinForms.Properties.Resources.creion32;
			this.ViewMenu.Name = "ViewMenu";
			this.ViewMenu.Size = new System.Drawing.Size(137, 22);
			this.ViewMenu.Text = "View Report";
			this.ViewMenu.Click += new System.EventHandler(this.viewReportToolStripMenuItem_Click);
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xMLToolStripMenuItem1,
            this.textFileToolStripMenuItem1,
            this.hTMLToolStripMenuItem1});
			this.exportToolStripMenuItem.Image = global::SecureDeleteWinForms.Properties.Resources.export_profile3;
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
			this.exportToolStripMenuItem.Text = "Export";
			// 
			// xMLToolStripMenuItem1
			// 
			this.xMLToolStripMenuItem1.Name = "xMLToolStripMenuItem1";
			this.xMLToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
			this.xMLToolStripMenuItem1.Text = "XML";
			// 
			// textFileToolStripMenuItem1
			// 
			this.textFileToolStripMenuItem1.Name = "textFileToolStripMenuItem1";
			this.textFileToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
			this.textFileToolStripMenuItem1.Text = "Text File";
			// 
			// hTMLToolStripMenuItem1
			// 
			this.hTMLToolStripMenuItem1.Name = "hTMLToolStripMenuItem1";
			this.hTMLToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
			this.hTMLToolStripMenuItem1.Text = "HTML";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(134, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click_1);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.deselectAllToolStripMenuItem,
            this.toolStripSeparator2,
            this.removeSelectedToolStripMenuItem,
            this.removeAllToolStripMenuItem1});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
			this.selectAllToolStripMenuItem.Text = "Select All";
			// 
			// deselectAllToolStripMenuItem
			// 
			this.deselectAllToolStripMenuItem.Name = "deselectAllToolStripMenuItem";
			this.deselectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.A)));
			this.deselectAllToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
			this.deselectAllToolStripMenuItem.Text = "Deselect All";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(206, 6);
			// 
			// removeSelectedToolStripMenuItem
			// 
			this.removeSelectedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSelectedToolStripMenuItem1,
            this.removeAllToolStripMenuItem2});
			this.removeSelectedToolStripMenuItem.Name = "removeSelectedToolStripMenuItem";
			this.removeSelectedToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
			this.removeSelectedToolStripMenuItem.Text = "Sessions";
			// 
			// removeSelectedToolStripMenuItem1
			// 
			this.removeSelectedToolStripMenuItem1.Name = "removeSelectedToolStripMenuItem1";
			this.removeSelectedToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
			this.removeSelectedToolStripMenuItem1.Text = "Remove Selected";
			// 
			// removeAllToolStripMenuItem2
			// 
			this.removeAllToolStripMenuItem2.Name = "removeAllToolStripMenuItem2";
			this.removeAllToolStripMenuItem2.Size = new System.Drawing.Size(164, 22);
			this.removeAllToolStripMenuItem2.Text = "Remove All";
			// 
			// removeAllToolStripMenuItem1
			// 
			this.removeAllToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSelectedToolStripMenuItem2,
            this.removeAllToolStripMenuItem3});
			this.removeAllToolStripMenuItem1.Name = "removeAllToolStripMenuItem1";
			this.removeAllToolStripMenuItem1.Size = new System.Drawing.Size(209, 22);
			this.removeAllToolStripMenuItem1.Text = "Reports";
			// 
			// removeSelectedToolStripMenuItem2
			// 
			this.removeSelectedToolStripMenuItem2.Name = "removeSelectedToolStripMenuItem2";
			this.removeSelectedToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.removeSelectedToolStripMenuItem2.Size = new System.Drawing.Size(188, 22);
			this.removeSelectedToolStripMenuItem2.Text = "Remove Selected";
			// 
			// removeAllToolStripMenuItem3
			// 
			this.removeAllToolStripMenuItem3.Name = "removeAllToolStripMenuItem3";
			this.removeAllToolStripMenuItem3.Size = new System.Drawing.Size(188, 22);
			this.removeAllToolStripMenuItem3.Text = "Remove All";
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SearchMenu,
            this.toolStripSeparator5,
            this.optionsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// SearchMenu
			// 
			this.SearchMenu.Checked = true;
			this.SearchMenu.CheckState = System.Windows.Forms.CheckState.Checked;
			this.SearchMenu.Image = global::SecureDeleteWinForms.Properties.Resources.Project3;
			this.SearchMenu.Name = "SearchMenu";
			this.SearchMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.SearchMenu.Size = new System.Drawing.Size(192, 22);
			this.SearchMenu.Text = "Search Reports";
			this.SearchMenu.Click += new System.EventHandler(this.searchReportsToolStripMenuItem_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(189, 6);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
			this.optionsToolStripMenuItem.Text = "Options";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click_1);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator4,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// contentsToolStripMenuItem
			// 
			this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
			this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.contentsToolStripMenuItem.Text = "Contents";
			// 
			// indexToolStripMenuItem
			// 
			this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
			this.indexToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.indexToolStripMenuItem.Text = "Index";
			// 
			// searchToolStripMenuItem
			// 
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.searchToolStripMenuItem.Text = "Search";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(119, 6);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click_1);
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.ReportList);
			this.splitContainer2.Panel1.Controls.Add(this.toolStrip1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.ToolHost);
			this.splitContainer2.Panel2.Controls.Add(this.ToolHeader);
			this.splitContainer2.Size = new System.Drawing.Size(551, 445);
			this.splitContainer2.SplitterDistance = 123;
			this.splitContainer2.SplitterWidth = 2;
			this.splitContainer2.TabIndex = 0;
			// 
			// ReportList
			// 
			this.ReportList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ReportList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Date,
            this.columnHeader1,
            this.columnHeader2});
			this.ReportList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReportList.FullRowSelect = true;
			this.ReportList.HideSelection = false;
			this.ReportList.Location = new System.Drawing.Point(0, 31);
			this.ReportList.Name = "ReportList";
			this.ReportList.Size = new System.Drawing.Size(551, 92);
			this.ReportList.SmallImageList = this.imageList1;
			this.ReportList.TabIndex = 3;
			this.ReportList.UseCompatibleStateImageBehavior = false;
			this.ReportList.View = System.Windows.Forms.View.Details;
			this.ReportList.SelectedIndexChanged += new System.EventHandler(this.ReportList_SelectedIndexChanged);
			this.ReportList.DoubleClick += new System.EventHandler(this.ReportList_DoubleClick);
			this.ReportList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ReportList_ColumnClick);
			this.ReportList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ReportList_KeyDown);
			// 
			// Date
			// 
			this.Date.Text = "Date";
			this.Date.Width = 180;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Failed";
			this.columnHeader1.Width = 120;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Errors";
			this.columnHeader2.Width = 120;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "up.png");
			this.imageList1.Images.SetKeyName(1, "down.png");
			this.imageList1.Images.SetKeyName(2, "report.png");
			this.imageList1.Images.SetKeyName(3, "report_delete.png");
			// 
			// toolStrip1
			// 
			this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
			this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewButton,
            this.RemoveButton,
            this.ExportButton,
            this.toolStripSeparator1,
            this.SearchButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.toolStrip1.Size = new System.Drawing.Size(551, 31);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// ViewButton
			// 
			this.ViewButton.Enabled = false;
			this.ViewButton.Image = global::SecureDeleteWinForms.Properties.Resources.creion32;
			this.ViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ViewButton.Name = "ViewButton";
			this.ViewButton.Size = new System.Drawing.Size(60, 28);
			this.ViewButton.Text = "View";
			this.ViewButton.Click += new System.EventHandler(this.toolStripButton2_Click);
			// 
			// RemoveButton
			// 
			this.RemoveButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.removeAllToolStripMenuItem});
			this.RemoveButton.Image = global::SecureDeleteWinForms.Properties.Resources.delete;
			this.RemoveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RemoveButton.Name = "RemoveButton";
			this.RemoveButton.Size = new System.Drawing.Size(90, 28);
			this.RemoveButton.Text = "Remove";
			this.RemoveButton.ToolTipText = "Remove items";
			this.RemoveButton.ButtonClick += new System.EventHandler(this.RemoveButton_ButtonClick);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.removeToolStripMenuItem.Text = "Remove Selected";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
			// 
			// removeAllToolStripMenuItem
			// 
			this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
			this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.removeAllToolStripMenuItem.Text = "Remove All";
			this.removeAllToolStripMenuItem.Click += new System.EventHandler(this.removeAllToolStripMenuItem_Click);
			// 
			// ExportButton
			// 
			this.ExportButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textFileToolStripMenuItem,
            this.xMLToolStripMenuItem,
            this.hTMLToolStripMenuItem});
			this.ExportButton.Enabled = false;
			this.ExportButton.Image = global::SecureDeleteWinForms.Properties.Resources.load_profile;
			this.ExportButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ExportButton.Name = "ExportButton";
			this.ExportButton.Size = new System.Drawing.Size(77, 28);
			this.ExportButton.Text = "Export";
			// 
			// textFileToolStripMenuItem
			// 
			this.textFileToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.textFileToolStripMenuItem.Name = "textFileToolStripMenuItem";
			this.textFileToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.textFileToolStripMenuItem.Text = "Text File";
			this.textFileToolStripMenuItem.Click += new System.EventHandler(this.textFileToolStripMenuItem_Click);
			// 
			// xMLToolStripMenuItem
			// 
			this.xMLToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.xMLToolStripMenuItem.Name = "xMLToolStripMenuItem";
			this.xMLToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.xMLToolStripMenuItem.Text = "XML";
			this.xMLToolStripMenuItem.Click += new System.EventHandler(this.xMLToolStripMenuItem_Click);
			// 
			// hTMLToolStripMenuItem
			// 
			this.hTMLToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.hTMLToolStripMenuItem.Name = "hTMLToolStripMenuItem";
			this.hTMLToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.hTMLToolStripMenuItem.Text = "HTML";
			this.hTMLToolStripMenuItem.Click += new System.EventHandler(this.hTMLToolStripMenuItem_Click_1);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
			// 
			// SearchButton
			// 
			this.SearchButton.Image = global::SecureDeleteWinForms.Properties.Resources.Project3;
			this.SearchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.SearchButton.Name = "SearchButton";
			this.SearchButton.Size = new System.Drawing.Size(70, 28);
			this.SearchButton.Text = "Search";
			this.SearchButton.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// ToolHost
			// 
			this.ToolHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ToolHost.Location = new System.Drawing.Point(0, 24);
			this.ToolHost.Name = "ToolHost";
			this.ToolHost.Size = new System.Drawing.Size(551, 296);
			this.ToolHost.TabIndex = 3;
			// 
			// ToolHeader
			// 
			this.ToolHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.ToolHeader.Controls.Add(this.ToolCloseButton);
			this.ToolHeader.Controls.Add(this.ToolHeaderIcon);
			this.ToolHeader.Controls.Add(this.ToolHeaderLabel);
			this.ToolHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolHeader.Location = new System.Drawing.Point(0, 0);
			this.ToolHeader.Name = "ToolHeader";
			this.ToolHeader.Size = new System.Drawing.Size(551, 24);
			this.ToolHeader.TabIndex = 2;
			// 
			// ToolCloseButton
			// 
			this.ToolCloseButton.AutoSize = true;
			this.ToolCloseButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.ToolCloseButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.ToolCloseButton.FlatAppearance.BorderSize = 0;
			this.ToolCloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ToolCloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.ToolCloseButton.ForeColor = System.Drawing.Color.White;
			this.ToolCloseButton.Location = new System.Drawing.Point(526, 0);
			this.ToolCloseButton.Name = "ToolCloseButton";
			this.ToolCloseButton.Size = new System.Drawing.Size(25, 24);
			this.ToolCloseButton.TabIndex = 3;
			this.ToolCloseButton.Text = "X";
			this.ToolCloseButton.UseVisualStyleBackColor = false;
			this.ToolCloseButton.Click += new System.EventHandler(this.ToolCloseButton_Click_1);
			// 
			// ToolHeaderIcon
			// 
			this.ToolHeaderIcon.Image = global::SecureDeleteWinForms.Properties.Resources.file;
			this.ToolHeaderIcon.Location = new System.Drawing.Point(4, 4);
			this.ToolHeaderIcon.Name = "ToolHeaderIcon";
			this.ToolHeaderIcon.Size = new System.Drawing.Size(16, 16);
			this.ToolHeaderIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.ToolHeaderIcon.TabIndex = 2;
			this.ToolHeaderIcon.TabStop = false;
			// 
			// ToolHeaderLabel
			// 
			this.ToolHeaderLabel.AutoSize = true;
			this.ToolHeaderLabel.ForeColor = System.Drawing.Color.White;
			this.ToolHeaderLabel.Location = new System.Drawing.Point(21, 5);
			this.ToolHeaderLabel.Name = "ToolHeaderLabel";
			this.ToolHeaderLabel.Size = new System.Drawing.Size(74, 13);
			this.ToolHeaderLabel.TabIndex = 1;
			this.ToolHeaderLabel.Text = "Report Details";
			// 
			// toolStrip2
			// 
			this.toolStrip2.BackColor = System.Drawing.SystemColors.Control;
			this.toolStrip2.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
			this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1});
			this.toolStrip2.Location = new System.Drawing.Point(0, 0);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.toolStrip2.Size = new System.Drawing.Size(206, 31);
			this.toolStrip2.TabIndex = 3;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
			this.toolStripSplitButton1.Image = global::SecureDeleteWinForms.Properties.Resources.delete;
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size(90, 28);
			this.toolStripSplitButton1.Text = "Remove";
			this.toolStripSplitButton1.ToolTipText = "Remove items";
			this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
			this.toolStripMenuItem1.Text = "Remove Selected";
			this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(164, 22);
			this.toolStripMenuItem2.Text = "Remove All";
			this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
			// 
			// ReportCategories
			// 
			this.ReportCategories.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ReportCategories.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReportCategories.FullRowSelect = true;
			this.ReportCategories.HideSelection = false;
			this.ReportCategories.ImageIndex = 0;
			this.ReportCategories.ImageList = this.imageList2;
			this.ReportCategories.Location = new System.Drawing.Point(0, 31);
			this.ReportCategories.Name = "ReportCategories";
			treeNode1.Name = "Node0";
			treeNode1.Text = "Default";
			this.ReportCategories.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
			this.ReportCategories.SelectedImageIndex = 0;
			this.ReportCategories.Size = new System.Drawing.Size(206, 414);
			this.ReportCategories.TabIndex = 0;
			this.ReportCategories.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ReportCategories_AfterSelect);
			// 
			// imageList2
			// 
			this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
			this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList2.Images.SetKeyName(0, "bullet_blue.png");
			this.imageList2.Images.SetKeyName(1, "date.png");
			this.imageList2.Images.SetKeyName(2, "date_error.png");
			// 
			// splitContainer1
			// 
			this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.ReportCategories);
			this.splitContainer1.Panel1.Controls.Add(this.toolStrip2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(759, 445);
			this.splitContainer1.SplitterDistance = 206;
			this.splitContainer1.SplitterWidth = 2;
			this.splitContainer1.TabIndex = 0;
			// 
			// ReportModule
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.MenuStrip);
			this.Name = "ReportModule";
			this.Size = new System.Drawing.Size(759, 469);
			this.MenuStrip.ResumeLayout(false);
			this.MenuStrip.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ToolHeader.ResumeLayout(false);
			this.ToolHeader.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ToolHeaderIcon)).EndInit();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip MenuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem xMLToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem textFileToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deselectAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem removeSelectedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeSelectedToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem removeSelectedToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem hTMLToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem SearchMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem ViewMenu;
		private System.Windows.Forms.SplitContainer splitContainer2;
		public System.Windows.Forms.ListView ReportList;
		private System.Windows.Forms.ColumnHeader Date;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton ViewButton;
		private System.Windows.Forms.ToolStripSplitButton RemoveButton;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripDropDownButton ExportButton;
		private System.Windows.Forms.ToolStripMenuItem textFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem xMLToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hTMLToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton SearchButton;
		private System.Windows.Forms.Panel ToolHost;
		private System.Windows.Forms.Panel ToolHeader;
		private System.Windows.Forms.Button ToolCloseButton;
		private System.Windows.Forms.PictureBox ToolHeaderIcon;
		private System.Windows.Forms.Label ToolHeaderLabel;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.TreeView ReportCategories;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ImageList imageList2;

	}
}
