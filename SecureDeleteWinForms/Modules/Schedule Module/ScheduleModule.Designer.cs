namespace SecureDeleteWinForms.Modules
{
	partial class ScheduleModule
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
			SecureDeleteWinForms.Modules.ModuleActionManager moduleActionManager1 = new SecureDeleteWinForms.Modules.ModuleActionManager();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScheduleModule));
			this.MenuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MainContainer = new System.Windows.Forms.SplitContainer();
			this.TaskList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.RemoveButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.EditButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.StartButton = new System.Windows.Forms.ToolStripButton();
			this.StopButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.ImportButton = new System.Windows.Forms.ToolStripButton();
			this.ExportButton = new System.Windows.Forms.ToolStripButton();
			this.WipeItems = new SecureDeleteWinForms.Modules.WipeModule();
			this.HistoryTool = new SecureDeleteWinForms.ScheduleHistoryTool();
			this.ModulePanel = new System.Windows.Forms.Panel();
			this.HistorySelector = new SecureDeleteWinForms.PanelSelectControl();
			this.WipeSelector = new SecureDeleteWinForms.PanelSelectControl();
			this.StatusTimer = new System.Windows.Forms.Timer(this.components);
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.importExportSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editSelectedItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.removeSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.forceStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuStrip.SuspendLayout();
			this.MainContainer.Panel1.SuspendLayout();
			this.MainContainer.Panel2.SuspendLayout();
			this.MainContainer.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.ModulePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// MenuStrip
			// 
			this.MenuStrip.BackColor = System.Drawing.SystemColors.Control;
			this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolStripMenuItem1,
            this.helpToolStripMenuItem});
			this.MenuStrip.Location = new System.Drawing.Point(0, 0);
			this.MenuStrip.Name = "MenuStrip";
			this.MenuStrip.Size = new System.Drawing.Size(741, 24);
			this.MenuStrip.TabIndex = 0;
			this.MenuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// contentsToolStripMenuItem
			// 
			this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
			this.contentsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.contentsToolStripMenuItem.Text = "&Contents";
			// 
			// indexToolStripMenuItem
			// 
			this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
			this.indexToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.indexToolStripMenuItem.Text = "&Index";
			// 
			// searchToolStripMenuItem
			// 
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.searchToolStripMenuItem.Text = "&Search";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(149, 6);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.aboutToolStripMenuItem.Text = "&About...";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// MainContainer
			// 
			this.MainContainer.BackColor = System.Drawing.SystemColors.ControlDark;
			this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.MainContainer.Location = new System.Drawing.Point(0, 24);
			this.MainContainer.Name = "MainContainer";
			this.MainContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// MainContainer.Panel1
			// 
			this.MainContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.MainContainer.Panel1.Controls.Add(this.TaskList);
			this.MainContainer.Panel1.Controls.Add(this.toolStrip1);
			// 
			// MainContainer.Panel2
			// 
			this.MainContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
			this.MainContainer.Panel2.Controls.Add(this.WipeItems);
			this.MainContainer.Panel2.Controls.Add(this.HistoryTool);
			this.MainContainer.Panel2.Controls.Add(this.ModulePanel);
			this.MainContainer.Size = new System.Drawing.Size(741, 471);
			this.MainContainer.SplitterDistance = 234;
			this.MainContainer.SplitterWidth = 1;
			this.MainContainer.TabIndex = 1;
			// 
			// TaskList
			// 
			this.TaskList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.TaskList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TaskList.FullRowSelect = true;
			this.TaskList.HideSelection = false;
			this.TaskList.Location = new System.Drawing.Point(0, 31);
			this.TaskList.MultiSelect = false;
			this.TaskList.Name = "TaskList";
			this.TaskList.Size = new System.Drawing.Size(741, 203);
			this.TaskList.TabIndex = 4;
			this.TaskList.UseCompatibleStateImageBehavior = false;
			this.TaskList.View = System.Windows.Forms.View.Details;
			this.TaskList.SelectedIndexChanged += new System.EventHandler(this.TaskList_SelectedIndexChanged);
			this.TaskList.DoubleClick += new System.EventHandler(this.TaskList_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 200;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Starting Time";
			this.columnHeader2.Width = 140;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Status";
			this.columnHeader3.Width = 350;
			// 
			// toolStrip1
			// 
			this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
			this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.RemoveButton,
            this.toolStripSeparator8,
            this.EditButton,
            this.toolStripSeparator6,
            this.StartButton,
            this.StopButton,
            this.toolStripSeparator7,
            this.ImportButton,
            this.ExportButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.toolStrip1.Size = new System.Drawing.Size(741, 31);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.Image = global::SecureDeleteWinForms.Properties.Resources.add2;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(84, 28);
			this.toolStripButton1.Text = "Add Task";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// RemoveButton
			// 
			this.RemoveButton.Enabled = false;
			this.RemoveButton.Image = global::SecureDeleteWinForms.Properties.Resources.delete;
			this.RemoveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RemoveButton.Name = "RemoveButton";
			this.RemoveButton.Size = new System.Drawing.Size(78, 28);
			this.RemoveButton.Text = "Remove";
			this.RemoveButton.ToolTipText = "Remove items";
			this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(6, 31);
			// 
			// EditButton
			// 
			this.EditButton.Enabled = false;
			this.EditButton.Image = global::SecureDeleteWinForms.Properties.Resources.modify;
			this.EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.EditButton.Name = "EditButton";
			this.EditButton.Size = new System.Drawing.Size(104, 28);
			this.EditButton.Text = "Task Options";
			this.EditButton.Click += new System.EventHandler(this.toolStripButton2_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 31);
			// 
			// StartButton
			// 
			this.StartButton.Enabled = false;
			this.StartButton.Image = global::SecureDeleteWinForms.Properties.Resources.dfsd;
			this.StartButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.StartButton.Name = "StartButton";
			this.StartButton.Size = new System.Drawing.Size(91, 28);
			this.StartButton.Text = "Force Start";
			this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
			// 
			// StopButton
			// 
			this.StopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.StopButton.Enabled = false;
			this.StopButton.Image = global::SecureDeleteWinForms.Properties.Resources.safdgsd;
			this.StopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.StopButton.Name = "StopButton";
			this.StopButton.Size = new System.Drawing.Size(28, 28);
			this.StopButton.Text = "toolStripButton4";
			this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 31);
			// 
			// ImportButton
			// 
			this.ImportButton.Image = global::SecureDeleteWinForms.Properties.Resources.import;
			this.ImportButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ImportButton.Name = "ImportButton";
			this.ImportButton.Size = new System.Drawing.Size(71, 28);
			this.ImportButton.Text = "Import";
			this.ImportButton.Click += new System.EventHandler(this.toolStripButton3_Click);
			// 
			// ExportButton
			// 
			this.ExportButton.Enabled = false;
			this.ExportButton.Image = global::SecureDeleteWinForms.Properties.Resources.export_profile3;
			this.ExportButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ExportButton.Name = "ExportButton";
			this.ExportButton.Size = new System.Drawing.Size(68, 28);
			this.ExportButton.Text = "Export";
			this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
			// 
			// WipeItems
			// 
			this.WipeItems.ActionManager = moduleActionManager1;
			this.WipeItems.Async = true;
			this.WipeItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WipeItems.Location = new System.Drawing.Point(0, 24);
			this.WipeItems.Name = "WipeItems";
			this.WipeItems.Options = null;
			this.WipeItems.ParentControl = null;
			this.WipeItems.ScheduleMode = true;
			this.WipeItems.Size = new System.Drawing.Size(741, 212);
			this.WipeItems.TabIndex = 5;
			// 
			// HistoryTool
			// 
			this.HistoryTool.Dock = System.Windows.Forms.DockStyle.Fill;
			this.HistoryTool.Location = new System.Drawing.Point(0, 24);
			this.HistoryTool.Manager = null;
			this.HistoryTool.Name = "HistoryTool";
			this.HistoryTool.Size = new System.Drawing.Size(741, 212);
			this.HistoryTool.TabIndex = 4;
			this.HistoryTool.Task = null;
			this.HistoryTool.Visible = false;
			// 
			// ModulePanel
			// 
			this.ModulePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.ModulePanel.Controls.Add(this.HistorySelector);
			this.ModulePanel.Controls.Add(this.WipeSelector);
			this.ModulePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.ModulePanel.Location = new System.Drawing.Point(0, 0);
			this.ModulePanel.Name = "ModulePanel";
			this.ModulePanel.Size = new System.Drawing.Size(741, 24);
			this.ModulePanel.TabIndex = 3;
			// 
			// HistorySelector
			// 
			this.HistorySelector.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.HistorySelector.Location = new System.Drawing.Point(73, 3);
			this.HistorySelector.Name = "HistorySelector";
			this.HistorySelector.Reversed = false;
			this.HistorySelector.Selected = false;
			this.HistorySelector.SelectedColor = System.Drawing.SystemColors.Control;
			this.HistorySelector.SelectedTextColor = System.Drawing.SystemColors.WindowText;
			this.HistorySelector.SelectorText = "History";
			this.HistorySelector.Size = new System.Drawing.Size(49, 21);
			this.HistorySelector.TabIndex = 1;
			this.HistorySelector.TextColor = System.Drawing.Color.White;
			this.HistorySelector.SelectedStateChanged += new System.EventHandler(this.ScheduleModuleSelector_SelectedStateChanged);
			// 
			// WipeSelector
			// 
			this.WipeSelector.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.WipeSelector.Location = new System.Drawing.Point(2, 3);
			this.WipeSelector.Name = "WipeSelector";
			this.WipeSelector.Reversed = false;
			this.WipeSelector.Selected = false;
			this.WipeSelector.SelectedColor = System.Drawing.SystemColors.Control;
			this.WipeSelector.SelectedTextColor = System.Drawing.SystemColors.WindowText;
			this.WipeSelector.SelectorText = "Wipe Items";
			this.WipeSelector.Size = new System.Drawing.Size(68, 21);
			this.WipeSelector.TabIndex = 0;
			this.WipeSelector.TextColor = System.Drawing.Color.White;
			this.WipeSelector.SelectedStateChanged += new System.EventHandler(this.WipeSelector_SelectedStateChanged);
			// 
			// StatusTimer
			// 
			this.StatusTimer.Enabled = true;
			this.StatusTimer.Interval = 1000;
			this.StatusTimer.Tick += new System.EventHandler(this.StatusTimer_Tick);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "Flag_greenHS.png");
			this.imageList1.Images.SetKeyName(1, "Flag_redHS.png");
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importExportSettingsToolStripMenuItem,
            this.toolStripMenuItem2});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(48, 20);
			this.toolStripMenuItem1.Text = "&Tools";
			// 
			// importExportSettingsToolStripMenuItem
			// 
			this.importExportSettingsToolStripMenuItem.Name = "importExportSettingsToolStripMenuItem";
			this.importExportSettingsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.importExportSettingsToolStripMenuItem.Text = "Import/Export Settings";
			this.importExportSettingsToolStripMenuItem.Click += new System.EventHandler(this.importExportSettingsToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(193, 22);
			this.toolStripMenuItem2.Text = "&Options";
			this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editSelectedItemToolStripMenuItem,
            this.toolStripSeparator3,
            this.forceStartToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.toolStripSeparator4,
            this.removeSelectedToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// editSelectedItemToolStripMenuItem
			// 
			this.editSelectedItemToolStripMenuItem.Enabled = false;
			this.editSelectedItemToolStripMenuItem.Image = global::SecureDeleteWinForms.Properties.Resources.creion32;
			this.editSelectedItemToolStripMenuItem.Name = "editSelectedItemToolStripMenuItem";
			this.editSelectedItemToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.editSelectedItemToolStripMenuItem.Text = "Edit Options";
			this.editSelectedItemToolStripMenuItem.Click += new System.EventHandler(this.editSelectedItemToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
			// 
			// removeSelectedToolStripMenuItem
			// 
			this.removeSelectedToolStripMenuItem.Enabled = false;
			this.removeSelectedToolStripMenuItem.Image = global::SecureDeleteWinForms.Properties.Resources.delete_profile;
			this.removeSelectedToolStripMenuItem.Name = "removeSelectedToolStripMenuItem";
			this.removeSelectedToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.removeSelectedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.removeSelectedToolStripMenuItem.Text = "Remove";
			this.removeSelectedToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedToolStripMenuItem_Click);
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Image = global::SecureDeleteWinForms.Properties.Resources.add_profile;
			this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.newToolStripMenuItem.Text = "&Add Task";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// forceStartToolStripMenuItem
			// 
			this.forceStartToolStripMenuItem.Image = global::SecureDeleteWinForms.Properties.Resources.dfsd;
			this.forceStartToolStripMenuItem.Name = "forceStartToolStripMenuItem";
			this.forceStartToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.forceStartToolStripMenuItem.Text = "Force Start";
			this.forceStartToolStripMenuItem.Click += new System.EventHandler(this.forceStartToolStripMenuItem_Click);
			// 
			// stopToolStripMenuItem
			// 
			this.stopToolStripMenuItem.Image = global::SecureDeleteWinForms.Properties.Resources.safdgsd;
			this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
			this.stopToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.stopToolStripMenuItem.Text = "Stop";
			this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Image = global::SecureDeleteWinForms.Properties.Resources.import;
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
			this.toolStripMenuItem3.Text = "Import Tasks";
			this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Image = global::SecureDeleteWinForms.Properties.Resources.export_profile3;
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
			this.toolStripMenuItem4.Text = "Export Tasks";
			this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
			// 
			// ScheduleModule
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MainContainer);
			this.Controls.Add(this.MenuStrip);
			this.Name = "ScheduleModule";
			this.Size = new System.Drawing.Size(741, 495);
			this.MenuStrip.ResumeLayout(false);
			this.MenuStrip.PerformLayout();
			this.MainContainer.Panel1.ResumeLayout(false);
			this.MainContainer.Panel1.PerformLayout();
			this.MainContainer.Panel2.ResumeLayout(false);
			this.MainContainer.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ModulePanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip MenuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.SplitContainer MainContainer;
		private System.Windows.Forms.ListView TaskList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.Timer StatusTimer;
		private System.Windows.Forms.Panel ModulePanel;
		private PanelSelectControl HistorySelector;
		private PanelSelectControl WipeSelector;
		private ScheduleHistoryTool HistoryTool;
		private WipeModule WipeItems;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripButton RemoveButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripButton EditButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripButton StartButton;
		private System.Windows.Forms.ToolStripButton StopButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripButton ImportButton;
		private System.Windows.Forms.ToolStripButton ExportButton;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem importExportSettingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editSelectedItemToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem removeSelectedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem forceStartToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
	}
}
