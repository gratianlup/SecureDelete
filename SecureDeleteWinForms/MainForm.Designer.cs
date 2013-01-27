namespace SecureDeleteWinForms
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deselectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.inverseSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TaskProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.StatusBar = new System.Windows.Forms.StatusStrip();
			this.TaskStopList = new System.Windows.Forms.ToolStripSplitButton();
			this.stopAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.TaskList = new System.Windows.Forms.ToolStripDropDownButton();
			this.ModuleStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.wipeMethodsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.wipeMethodsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.ModuleHost = new System.Windows.Forms.Panel();
			this.ModulePanel = new System.Windows.Forms.Panel();
			this.ReportsModuleSelector = new SecureDeleteWinForms.PanelSelectControl();
			this.ScheduleModuleSelector = new SecureDeleteWinForms.PanelSelectControl();
			this.WipeModuleSelector = new SecureDeleteWinForms.PanelSelectControl();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.StatusBar.SuspendLayout();
			this.ModulePanel.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
			this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.newToolStripMenuItem.Text = "&New";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
			this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.openToolStripMenuItem.Text = "&Open";
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
			this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
			// 
			// printToolStripMenuItem
			// 
			this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
			this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.printToolStripMenuItem.Name = "printToolStripMenuItem";
			this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.printToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.printToolStripMenuItem.Text = "&Print";
			// 
			// printPreviewToolStripMenuItem
			// 
			this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripMenuItem.Image")));
			this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
			this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem,
            this.deselectAllToolStripMenuItem,
            this.inverseSelectionToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
			this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.cutToolStripMenuItem.Text = "Cu&t";
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
			this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.copyToolStripMenuItem.Text = "&Copy";
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
			this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.pasteToolStripMenuItem.Text = "&Paste";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(159, 6);
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.selectAllToolStripMenuItem.Text = "Select &All";
			// 
			// deselectAllToolStripMenuItem
			// 
			this.deselectAllToolStripMenuItem.Name = "deselectAllToolStripMenuItem";
			this.deselectAllToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.deselectAllToolStripMenuItem.Text = "Deselect All";
			// 
			// inverseSelectionToolStripMenuItem
			// 
			this.inverseSelectionToolStripMenuItem.Name = "inverseSelectionToolStripMenuItem";
			this.inverseSelectionToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.inverseSelectionToolStripMenuItem.Text = "Inverse Selection";
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// customizeToolStripMenuItem
			// 
			this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
			this.customizeToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.customizeToolStripMenuItem.Text = "&Customize";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.optionsToolStripMenuItem.Text = "&Options";
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
			this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.contentsToolStripMenuItem.Text = "&Contents";
			// 
			// indexToolStripMenuItem
			// 
			this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
			this.indexToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.indexToolStripMenuItem.Text = "&Index";
			// 
			// searchToolStripMenuItem
			// 
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.searchToolStripMenuItem.Text = "&Search";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.aboutToolStripMenuItem.Text = "&About...";
			// 
			// TaskProgress
			// 
			this.TaskProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.TaskProgress.Name = "TaskProgress";
			this.TaskProgress.Size = new System.Drawing.Size(100, 16);
			this.TaskProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.TaskProgress.Visible = false;
			// 
			// StatusBar
			// 
			this.StatusBar.BackColor = System.Drawing.SystemColors.Control;
			this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TaskStopList,
            this.TaskProgress,
            this.TaskList,
            this.ModuleStatusLabel});
			this.StatusBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.StatusBar.Location = new System.Drawing.Point(0, 518);
			this.StatusBar.Name = "StatusBar";
			this.StatusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
			this.StatusBar.Size = new System.Drawing.Size(929, 22);
			this.StatusBar.TabIndex = 1;
			this.StatusBar.Text = "Tools";
			// 
			// TaskStopList
			// 
			this.TaskStopList.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.TaskStopList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TaskStopList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stopAllToolStripMenuItem,
            this.toolStripSeparator3});
			this.TaskStopList.Image = global::SecureDeleteWinForms.Properties.Resources.safdgsd;
			this.TaskStopList.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TaskStopList.Name = "TaskStopList";
			this.TaskStopList.Size = new System.Drawing.Size(32, 20);
			this.TaskStopList.Text = "toolStripSplitButton1";
			this.TaskStopList.Visible = false;
			this.TaskStopList.ButtonClick += new System.EventHandler(this.TaskStopList_ButtonClick);
			// 
			// stopAllToolStripMenuItem
			// 
			this.stopAllToolStripMenuItem.Name = "stopAllToolStripMenuItem";
			this.stopAllToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
			this.stopAllToolStripMenuItem.Text = "Stop All";
			this.stopAllToolStripMenuItem.Click += new System.EventHandler(this.stopAllToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(112, 6);
			// 
			// TaskList
			// 
			this.TaskList.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.TaskList.Image = global::SecureDeleteWinForms.Properties.Resources.Project3;
			this.TaskList.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TaskList.Name = "TaskList";
			this.TaskList.Size = new System.Drawing.Size(106, 20);
			this.TaskList.Text = "Search Status";
			this.TaskList.Visible = false;
			// 
			// ModuleStatusLabel
			// 
			this.ModuleStatusLabel.Name = "ModuleStatusLabel";
			this.ModuleStatusLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// wipeMethodsToolStripMenuItem
			// 
			this.wipeMethodsToolStripMenuItem.Name = "wipeMethodsToolStripMenuItem";
			this.wipeMethodsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.wipeMethodsToolStripMenuItem.Text = "WipeMethods";
			// 
			// wipeMethodsToolStripMenuItem1
			// 
			this.wipeMethodsToolStripMenuItem1.Name = "wipeMethodsToolStripMenuItem1";
			this.wipeMethodsToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
			this.wipeMethodsToolStripMenuItem1.Text = "Wipe Methods";
			// 
			// ModuleHost
			// 
			this.ModuleHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ModuleHost.Location = new System.Drawing.Point(0, 24);
			this.ModuleHost.Name = "ModuleHost";
			this.ModuleHost.Size = new System.Drawing.Size(929, 494);
			this.ModuleHost.TabIndex = 3;
			// 
			// ModulePanel
			// 
			this.ModulePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.ModulePanel.Controls.Add(this.ReportsModuleSelector);
			this.ModulePanel.Controls.Add(this.ScheduleModuleSelector);
			this.ModulePanel.Controls.Add(this.WipeModuleSelector);
			this.ModulePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.ModulePanel.Location = new System.Drawing.Point(0, 0);
			this.ModulePanel.Name = "ModulePanel";
			this.ModulePanel.Size = new System.Drawing.Size(929, 24);
			this.ModulePanel.TabIndex = 2;
			// 
			// ReportsModuleSelector
			// 
			this.ReportsModuleSelector.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.ReportsModuleSelector.Location = new System.Drawing.Point(115, 3);
			this.ReportsModuleSelector.Name = "ReportsModuleSelector";
			this.ReportsModuleSelector.Reversed = false;
			this.ReportsModuleSelector.Selected = false;
			this.ReportsModuleSelector.SelectedColor = System.Drawing.SystemColors.Control;
			this.ReportsModuleSelector.SelectedTextColor = System.Drawing.SystemColors.ControlText;
			this.ReportsModuleSelector.SelectorText = "Reports";
			this.ReportsModuleSelector.Size = new System.Drawing.Size(54, 21);
			this.ReportsModuleSelector.TabIndex = 2;
			this.ReportsModuleSelector.TextColor = System.Drawing.Color.White;
			this.ReportsModuleSelector.Click += new System.EventHandler(this.ReportsModuleSelector_Click_1);
			// 
			// ScheduleModuleSelector
			// 
			this.ScheduleModuleSelector.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.ScheduleModuleSelector.Location = new System.Drawing.Point(48, 3);
			this.ScheduleModuleSelector.Name = "ScheduleModuleSelector";
			this.ScheduleModuleSelector.Reversed = false;
			this.ScheduleModuleSelector.Selected = false;
			this.ScheduleModuleSelector.SelectedColor = System.Drawing.SystemColors.Control;
			this.ScheduleModuleSelector.SelectedTextColor = System.Drawing.SystemColors.ControlText;
			this.ScheduleModuleSelector.SelectorText = "Schedule";
			this.ScheduleModuleSelector.Size = new System.Drawing.Size(63, 21);
			this.ScheduleModuleSelector.TabIndex = 1;
			this.ScheduleModuleSelector.TextColor = System.Drawing.Color.White;
			this.ScheduleModuleSelector.Click += new System.EventHandler(this.ScheduleModuleSelector_Click);
			// 
			// WipeModuleSelector
			// 
			this.WipeModuleSelector.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.WipeModuleSelector.Location = new System.Drawing.Point(2, 3);
			this.WipeModuleSelector.Name = "WipeModuleSelector";
			this.WipeModuleSelector.Reversed = false;
			this.WipeModuleSelector.Selected = false;
			this.WipeModuleSelector.SelectedColor = System.Drawing.SystemColors.Control;
			this.WipeModuleSelector.SelectedTextColor = System.Drawing.SystemColors.ControlText;
			this.WipeModuleSelector.SelectorText = "Wipe";
			this.WipeModuleSelector.Size = new System.Drawing.Size(42, 21);
			this.WipeModuleSelector.TabIndex = 0;
			this.WipeModuleSelector.TextColor = System.Drawing.Color.White;
			this.WipeModuleSelector.Click += new System.EventHandler(this.WipeModuleSelector_Click);
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "SecureDelete";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseMove);
			this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.toolStripSeparator6,
            this.closeToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(104, 54);
			// 
			// showToolStripMenuItem
			// 
			this.showToolStripMenuItem.Name = "showToolStripMenuItem";
			this.showToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.showToolStripMenuItem.Text = "Show";
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(100, 6);
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.closeToolStripMenuItem.Text = "Close";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ModuleHost);
			this.Controls.Add(this.ModulePanel);
			this.Controls.Add(this.StatusBar);
			this.Name = "MainForm";
			this.Size = new System.Drawing.Size(929, 540);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.StatusBar.ResumeLayout(false);
			this.StatusBar.PerformLayout();
			this.ModulePanel.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deselectAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem inverseSelectionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripProgressBar TaskProgress;
		private System.Windows.Forms.StatusStrip StatusBar;
		private System.Windows.Forms.ToolStripSplitButton TaskStopList;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem stopAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripDropDownButton TaskList;
		private System.Windows.Forms.ToolStripMenuItem wipeMethodsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem wipeMethodsToolStripMenuItem1;
		private System.Windows.Forms.ToolStripStatusLabel ModuleStatusLabel;
		private System.Windows.Forms.Panel ModuleHost;
		private System.Windows.Forms.Panel ModulePanel;
		private PanelSelectControl ReportsModuleSelector;
		private PanelSelectControl ScheduleModuleSelector;
		private PanelSelectControl WipeModuleSelector;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
	}
}

