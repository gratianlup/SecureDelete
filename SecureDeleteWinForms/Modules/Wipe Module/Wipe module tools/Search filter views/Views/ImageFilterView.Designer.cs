namespace SecureDeleteWinForms
{
	partial class ImageFilterView
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
			this.StatusIndicator = new System.Windows.Forms.Panel();
			this.ErrorTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.label3 = new System.Windows.Forms.Label();
			this.NameTextbox = new System.Windows.Forms.TextBox();
			this.FilterType = new System.Windows.Forms.ComboBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.TextPanel = new System.Windows.Forms.Panel();
			this.RegexCheckbox = new System.Windows.Forms.CheckBox();
			this.MatchCaseCheckbox = new System.Windows.Forms.CheckBox();
			this.TextFilterValue = new System.Windows.Forms.TextBox();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.DatePanel = new System.Windows.Forms.Panel();
			this.DateValue = new System.Windows.Forms.DateTimePicker();
			this.DateImplicationCombobox = new System.Windows.Forms.ComboBox();
			this.NumberPanel = new System.Windows.Forms.Panel();
			this.NumberFilterLabel = new System.Windows.Forms.Label();
			this.SizeValue = new System.Windows.Forms.TextBox();
			this.SizeImplicationCombobox = new System.Windows.Forms.ComboBox();
			this.ProgramPanel = new System.Windows.Forms.Panel();
			this.ProgramCombobox = new System.Windows.Forms.ComboBox();
			this.ExposureTimePanel = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.DenominatorTextbox = new System.Windows.Forms.TextBox();
			this.ExposureTimeLabel = new System.Windows.Forms.Label();
			this.NumeratorTextbox = new System.Windows.Forms.TextBox();
			this.ExposureTimeSizeCombobox = new System.Windows.Forms.ComboBox();
			this.FlashPanel = new System.Windows.Forms.Panel();
			this.FlashLabel = new System.Windows.Forms.Label();
			this.MeteringPanel = new System.Windows.Forms.Panel();
			this.MeteringComboBox = new System.Windows.Forms.ComboBox();
			this.OrientationPanel = new System.Windows.Forms.Panel();
			this.OrientationCombobox = new System.Windows.Forms.ComboBox();
			this.RatingPanel = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.RatingCombobox = new System.Windows.Forms.ComboBox();
			this.StarLabel = new System.Windows.Forms.Label();
			this.Star5 = new System.Windows.Forms.PictureBox();
			this.Star3 = new System.Windows.Forms.PictureBox();
			this.Star2 = new System.Windows.Forms.PictureBox();
			this.Star4 = new System.Windows.Forms.PictureBox();
			this.Star1 = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.TagsPanel = new System.Windows.Forms.Panel();
			this.TagList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.AddTagButton = new System.Windows.Forms.Button();
			this.RemoveTagButton = new System.Windows.Forms.Button();
			this.YesNoCombobox = new System.Windows.Forms.ComboBox();
			this.TagsPanel2 = new System.Windows.Forms.Panel();
			this.TagLabel2 = new System.Windows.Forms.Label();
			this.TagImplicationCombobox = new System.Windows.Forms.ComboBox();
			this.TagLabel1 = new System.Windows.Forms.Label();
			this.TextPanel.SuspendLayout();
			this.DatePanel.SuspendLayout();
			this.NumberPanel.SuspendLayout();
			this.ProgramPanel.SuspendLayout();
			this.ExposureTimePanel.SuspendLayout();
			this.FlashPanel.SuspendLayout();
			this.MeteringPanel.SuspendLayout();
			this.OrientationPanel.SuspendLayout();
			this.RatingPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Star5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Star3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Star2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Star4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Star1)).BeginInit();
			this.TagsPanel.SuspendLayout();
			this.TagsPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// StatusIndicator
			// 
			this.StatusIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.StatusIndicator.BackColor = System.Drawing.Color.Lime;
			this.StatusIndicator.Location = new System.Drawing.Point(0, 0);
			this.StatusIndicator.Name = "StatusIndicator";
			this.StatusIndicator.Size = new System.Drawing.Size(2, 159);
			this.StatusIndicator.TabIndex = 31;
			// 
			// ErrorTooltip
			// 
			this.ErrorTooltip.StripAmpersands = true;
			this.ErrorTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
			this.ErrorTooltip.ToolTipTitle = "Message";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(698, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 13);
			this.label3.TabIndex = 29;
			this.label3.Text = "Name";
			// 
			// NameTextbox
			// 
			this.NameTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.NameTextbox.Location = new System.Drawing.Point(739, 7);
			this.NameTextbox.Name = "NameTextbox";
			this.NameTextbox.Size = new System.Drawing.Size(100, 20);
			this.NameTextbox.TabIndex = 30;
			this.NameTextbox.TextChanged += new System.EventHandler(this.NameTextbox_TextChanged);
			// 
			// FilterType
			// 
			this.FilterType.BackColor = System.Drawing.SystemColors.Window;
			this.FilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FilterType.FormattingEnabled = true;
			this.FilterType.Items.AddRange(new object[] {
            "Author",
            "Camera Maker",
            "Camera Model",
            "Copyright",
            "Date Taken",
            "Exposure Bias",
            "Exposure Program",
            "Exposure Time",
            "Flash Fired",
            "F Number",
            "Focal Length",
            "ISO",
            "Metering Mode",
            "Orientation",
            "Rating",
            "Software",
            "Tag list",
            "Title"});
			this.FilterType.Location = new System.Drawing.Point(82, 6);
			this.FilterType.Name = "FilterType";
			this.FilterType.Size = new System.Drawing.Size(129, 21);
			this.FilterType.TabIndex = 24;
			this.FilterType.SelectedIndexChanged += new System.EventHandler(this.FilterType_SelectedIndexChanged);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.checkBox1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
			this.checkBox1.ForeColor = System.Drawing.Color.White;
			this.checkBox1.Location = new System.Drawing.Point(8, 9);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(65, 17);
			this.checkBox1.TabIndex = 22;
			this.checkBox1.Text = "Enabled";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// TextPanel
			// 
			this.TextPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.TextPanel.Controls.Add(this.RegexCheckbox);
			this.TextPanel.Controls.Add(this.MatchCaseCheckbox);
			this.TextPanel.Controls.Add(this.TextFilterValue);
			this.TextPanel.Location = new System.Drawing.Point(286, 0);
			this.TextPanel.Name = "TextPanel";
			this.TextPanel.Size = new System.Drawing.Size(406, 34);
			this.TextPanel.TabIndex = 32;
			this.TextPanel.Visible = false;
			// 
			// RegexCheckbox
			// 
			this.RegexCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.RegexCheckbox.AutoSize = true;
			this.RegexCheckbox.ForeColor = System.Drawing.Color.White;
			this.RegexCheckbox.Location = new System.Drawing.Point(290, 9);
			this.RegexCheckbox.Name = "RegexCheckbox";
			this.RegexCheckbox.Size = new System.Drawing.Size(116, 17);
			this.RegexCheckbox.TabIndex = 2;
			this.RegexCheckbox.Text = "Regular expression";
			this.RegexCheckbox.UseVisualStyleBackColor = true;
			this.RegexCheckbox.CheckedChanged += new System.EventHandler(this.RegexCheckbox_CheckedChanged);
			// 
			// MatchCaseCheckbox
			// 
			this.MatchCaseCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.MatchCaseCheckbox.AutoSize = true;
			this.MatchCaseCheckbox.ForeColor = System.Drawing.Color.White;
			this.MatchCaseCheckbox.Location = new System.Drawing.Point(208, 9);
			this.MatchCaseCheckbox.Name = "MatchCaseCheckbox";
			this.MatchCaseCheckbox.Size = new System.Drawing.Size(82, 17);
			this.MatchCaseCheckbox.TabIndex = 1;
			this.MatchCaseCheckbox.Text = "Match case";
			this.MatchCaseCheckbox.UseVisualStyleBackColor = true;
			this.MatchCaseCheckbox.CheckedChanged += new System.EventHandler(this.MatchCaseCheckbox_CheckedChanged);
			// 
			// TextFilterValue
			// 
			this.TextFilterValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.TextFilterValue.Location = new System.Drawing.Point(0, 7);
			this.TextFilterValue.Name = "TextFilterValue";
			this.TextFilterValue.Size = new System.Drawing.Size(202, 20);
			this.TextFilterValue.TabIndex = 0;
			this.TextFilterValue.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// comboBox2
			// 
			this.comboBox2.BackColor = System.Drawing.SystemColors.Window;
			this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Items.AddRange(new object[] {
            "Is",
            "Is Not"});
			this.comboBox2.Location = new System.Drawing.Point(217, 6);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(63, 21);
			this.comboBox2.TabIndex = 25;
			// 
			// DatePanel
			// 
			this.DatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.DatePanel.Controls.Add(this.DateValue);
			this.DatePanel.Controls.Add(this.DateImplicationCombobox);
			this.DatePanel.Location = new System.Drawing.Point(286, 0);
			this.DatePanel.Name = "DatePanel";
			this.DatePanel.Size = new System.Drawing.Size(406, 34);
			this.DatePanel.TabIndex = 33;
			this.DatePanel.Visible = false;
			// 
			// DateValue
			// 
			this.DateValue.Location = new System.Drawing.Point(113, 7);
			this.DateValue.Name = "DateValue";
			this.DateValue.Size = new System.Drawing.Size(200, 20);
			this.DateValue.TabIndex = 18;
			this.DateValue.ValueChanged += new System.EventHandler(this.DateValue_ValueChanged);
			// 
			// DateImplicationCombobox
			// 
			this.DateImplicationCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DateImplicationCombobox.FormattingEnabled = true;
			this.DateImplicationCombobox.Items.AddRange(new object[] {
            "Newer or from",
            "From",
            "Older or from"});
			this.DateImplicationCombobox.Location = new System.Drawing.Point(0, 6);
			this.DateImplicationCombobox.Name = "DateImplicationCombobox";
			this.DateImplicationCombobox.Size = new System.Drawing.Size(107, 21);
			this.DateImplicationCombobox.TabIndex = 17;
			this.DateImplicationCombobox.SelectedIndexChanged += new System.EventHandler(this.DateImplicationCombobox_SelectedIndexChanged);
			// 
			// NumberPanel
			// 
			this.NumberPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.NumberPanel.Controls.Add(this.NumberFilterLabel);
			this.NumberPanel.Controls.Add(this.SizeValue);
			this.NumberPanel.Controls.Add(this.SizeImplicationCombobox);
			this.NumberPanel.Location = new System.Drawing.Point(286, 0);
			this.NumberPanel.Name = "NumberPanel";
			this.NumberPanel.Size = new System.Drawing.Size(406, 34);
			this.NumberPanel.TabIndex = 34;
			this.NumberPanel.Visible = false;
			// 
			// NumberFilterLabel
			// 
			this.NumberFilterLabel.AutoSize = true;
			this.NumberFilterLabel.ForeColor = System.Drawing.Color.White;
			this.NumberFilterLabel.Location = new System.Drawing.Point(274, 10);
			this.NumberFilterLabel.Name = "NumberFilterLabel";
			this.NumberFilterLabel.Size = new System.Drawing.Size(62, 13);
			this.NumberFilterLabel.TabIndex = 19;
			this.NumberFilterLabel.Text = "Sample text";
			// 
			// SizeValue
			// 
			this.SizeValue.Location = new System.Drawing.Point(111, 6);
			this.SizeValue.Name = "SizeValue";
			this.SizeValue.Size = new System.Drawing.Size(160, 20);
			this.SizeValue.TabIndex = 18;
			this.SizeValue.TextChanged += new System.EventHandler(this.SizeValue_TextChanged);
			this.SizeValue.Validating += new System.ComponentModel.CancelEventHandler(this.SizeValue_Validating);
			// 
			// SizeImplicationCombobox
			// 
			this.SizeImplicationCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SizeImplicationCombobox.FormattingEnabled = true;
			this.SizeImplicationCombobox.Items.AddRange(new object[] {
            "Smaller than",
            "Equal to",
            "Greater than"});
			this.SizeImplicationCombobox.Location = new System.Drawing.Point(0, 6);
			this.SizeImplicationCombobox.Name = "SizeImplicationCombobox";
			this.SizeImplicationCombobox.Size = new System.Drawing.Size(107, 21);
			this.SizeImplicationCombobox.TabIndex = 17;
			this.SizeImplicationCombobox.SelectedIndexChanged += new System.EventHandler(this.SizeImplicationCombobox_SelectedIndexChanged);
			// 
			// ProgramPanel
			// 
			this.ProgramPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ProgramPanel.Controls.Add(this.ProgramCombobox);
			this.ProgramPanel.Location = new System.Drawing.Point(286, 0);
			this.ProgramPanel.Name = "ProgramPanel";
			this.ProgramPanel.Size = new System.Drawing.Size(406, 34);
			this.ProgramPanel.TabIndex = 35;
			this.ProgramPanel.Visible = false;
			// 
			// ProgramCombobox
			// 
			this.ProgramCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ProgramCombobox.FormattingEnabled = true;
			this.ProgramCombobox.Items.AddRange(new object[] {
            "Action Program",
            "Aperature Priority",
            "Creative Program",
            "Landscape Mode",
            "Manual",
            "Not Defined",
            "Portait Mode",
            "Program",
            "Shutter Priority"});
			this.ProgramCombobox.Location = new System.Drawing.Point(0, 6);
			this.ProgramCombobox.Name = "ProgramCombobox";
			this.ProgramCombobox.Size = new System.Drawing.Size(202, 21);
			this.ProgramCombobox.TabIndex = 17;
			this.ProgramCombobox.SelectedIndexChanged += new System.EventHandler(this.ProgramCombobox_SelectedIndexChanged);
			// 
			// ExposureTimePanel
			// 
			this.ExposureTimePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ExposureTimePanel.Controls.Add(this.label2);
			this.ExposureTimePanel.Controls.Add(this.DenominatorTextbox);
			this.ExposureTimePanel.Controls.Add(this.ExposureTimeLabel);
			this.ExposureTimePanel.Controls.Add(this.NumeratorTextbox);
			this.ExposureTimePanel.Controls.Add(this.ExposureTimeSizeCombobox);
			this.ExposureTimePanel.Location = new System.Drawing.Point(286, 0);
			this.ExposureTimePanel.Name = "ExposureTimePanel";
			this.ExposureTimePanel.Size = new System.Drawing.Size(406, 34);
			this.ExposureTimePanel.TabIndex = 36;
			this.ExposureTimePanel.Visible = false;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(160, 11);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(12, 13);
			this.label2.TabIndex = 21;
			this.label2.Text = "/";
			// 
			// DenominatorTextbox
			// 
			this.DenominatorTextbox.Location = new System.Drawing.Point(174, 6);
			this.DenominatorTextbox.Name = "DenominatorTextbox";
			this.DenominatorTextbox.Size = new System.Drawing.Size(57, 20);
			this.DenominatorTextbox.TabIndex = 20;
			this.DenominatorTextbox.TextChanged += new System.EventHandler(this.DenominatorTextbox_TextChanged);
			this.DenominatorTextbox.Validating += new System.ComponentModel.CancelEventHandler(this.DenominatorTextbox_Validating);
			// 
			// ExposureTimeLabel
			// 
			this.ExposureTimeLabel.AutoSize = true;
			this.ExposureTimeLabel.ForeColor = System.Drawing.Color.White;
			this.ExposureTimeLabel.Location = new System.Drawing.Point(234, 10);
			this.ExposureTimeLabel.Name = "ExposureTimeLabel";
			this.ExposureTimeLabel.Size = new System.Drawing.Size(62, 13);
			this.ExposureTimeLabel.TabIndex = 19;
			this.ExposureTimeLabel.Text = "(0) seconds";
			// 
			// NumeratorTextbox
			// 
			this.NumeratorTextbox.Location = new System.Drawing.Point(111, 6);
			this.NumeratorTextbox.Name = "NumeratorTextbox";
			this.NumeratorTextbox.Size = new System.Drawing.Size(48, 20);
			this.NumeratorTextbox.TabIndex = 18;
			this.NumeratorTextbox.TextChanged += new System.EventHandler(this.NumeratorTextbox_TextChanged);
			this.NumeratorTextbox.Validating += new System.ComponentModel.CancelEventHandler(this.NumeratorTextbox_Validating);
			// 
			// ExposureTimeSizeCombobox
			// 
			this.ExposureTimeSizeCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ExposureTimeSizeCombobox.FormattingEnabled = true;
			this.ExposureTimeSizeCombobox.Items.AddRange(new object[] {
            "Smaller than",
            "Equal to",
            "Greater than"});
			this.ExposureTimeSizeCombobox.Location = new System.Drawing.Point(0, 6);
			this.ExposureTimeSizeCombobox.Name = "ExposureTimeSizeCombobox";
			this.ExposureTimeSizeCombobox.Size = new System.Drawing.Size(107, 21);
			this.ExposureTimeSizeCombobox.TabIndex = 17;
			// 
			// FlashPanel
			// 
			this.FlashPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.FlashPanel.Controls.Add(this.FlashLabel);
			this.FlashPanel.Location = new System.Drawing.Point(286, 0);
			this.FlashPanel.Name = "FlashPanel";
			this.FlashPanel.Size = new System.Drawing.Size(406, 34);
			this.FlashPanel.TabIndex = 37;
			this.FlashPanel.Visible = false;
			// 
			// FlashLabel
			// 
			this.FlashLabel.AutoSize = true;
			this.FlashLabel.ForeColor = System.Drawing.Color.White;
			this.FlashLabel.Location = new System.Drawing.Point(0, 10);
			this.FlashLabel.Name = "FlashLabel";
			this.FlashLabel.Size = new System.Drawing.Size(25, 13);
			this.FlashLabel.TabIndex = 19;
			this.FlashLabel.Text = "true";
			// 
			// MeteringPanel
			// 
			this.MeteringPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.MeteringPanel.Controls.Add(this.MeteringComboBox);
			this.MeteringPanel.Location = new System.Drawing.Point(286, 0);
			this.MeteringPanel.Name = "MeteringPanel";
			this.MeteringPanel.Size = new System.Drawing.Size(406, 34);
			this.MeteringPanel.TabIndex = 38;
			this.MeteringPanel.Visible = false;
			// 
			// MeteringComboBox
			// 
			this.MeteringComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MeteringComboBox.FormattingEnabled = true;
			this.MeteringComboBox.Items.AddRange(new object[] {
            "Average",
            "Center Weighted Average",
            "Matrix",
            "Multi Spot",
            "Partial",
            "Spot",
            "Unknown"});
			this.MeteringComboBox.Location = new System.Drawing.Point(0, 6);
			this.MeteringComboBox.Name = "MeteringComboBox";
			this.MeteringComboBox.Size = new System.Drawing.Size(202, 21);
			this.MeteringComboBox.TabIndex = 17;
			this.MeteringComboBox.SelectedIndexChanged += new System.EventHandler(this.MeteringComboBox_SelectedIndexChanged);
			// 
			// OrientationPanel
			// 
			this.OrientationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.OrientationPanel.Controls.Add(this.OrientationCombobox);
			this.OrientationPanel.Location = new System.Drawing.Point(286, 0);
			this.OrientationPanel.Name = "OrientationPanel";
			this.OrientationPanel.Size = new System.Drawing.Size(406, 34);
			this.OrientationPanel.TabIndex = 39;
			this.OrientationPanel.Visible = false;
			// 
			// OrientationCombobox
			// 
			this.OrientationCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.OrientationCombobox.FormattingEnabled = true;
			this.OrientationCombobox.Items.AddRange(new object[] {
            "Correct",
            "Rotate Left",
            "Rotate Right"});
			this.OrientationCombobox.Location = new System.Drawing.Point(0, 6);
			this.OrientationCombobox.Name = "OrientationCombobox";
			this.OrientationCombobox.Size = new System.Drawing.Size(133, 21);
			this.OrientationCombobox.TabIndex = 17;
			this.OrientationCombobox.SelectedIndexChanged += new System.EventHandler(this.OrientationCombobox_SelectedIndexChanged);
			// 
			// RatingPanel
			// 
			this.RatingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.RatingPanel.Controls.Add(this.panel1);
			this.RatingPanel.Controls.Add(this.RatingCombobox);
			this.RatingPanel.Controls.Add(this.StarLabel);
			this.RatingPanel.Controls.Add(this.Star5);
			this.RatingPanel.Controls.Add(this.Star3);
			this.RatingPanel.Controls.Add(this.Star2);
			this.RatingPanel.Controls.Add(this.Star4);
			this.RatingPanel.Controls.Add(this.Star1);
			this.RatingPanel.Location = new System.Drawing.Point(286, 0);
			this.RatingPanel.Name = "RatingPanel";
			this.RatingPanel.Size = new System.Drawing.Size(406, 34);
			this.RatingPanel.TabIndex = 40;
			this.RatingPanel.Visible = false;
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(108, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(10, 35);
			this.panel1.TabIndex = 19;
			this.panel1.MouseLeave += new System.EventHandler(this.panel1_MouseLeave);
			this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
			this.panel1.MouseEnter += new System.EventHandler(this.panel1_MouseEnter);
			// 
			// RatingCombobox
			// 
			this.RatingCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RatingCombobox.FormattingEnabled = true;
			this.RatingCombobox.Items.AddRange(new object[] {
            "Smaller than",
            "Equal to",
            "Greater than"});
			this.RatingCombobox.Location = new System.Drawing.Point(0, 6);
			this.RatingCombobox.Name = "RatingCombobox";
			this.RatingCombobox.Size = new System.Drawing.Size(107, 21);
			this.RatingCombobox.TabIndex = 18;
			this.RatingCombobox.SelectedIndexChanged += new System.EventHandler(this.RatingCombobox_SelectedIndexChanged);
			// 
			// StarLabel
			// 
			this.StarLabel.AutoSize = true;
			this.StarLabel.ForeColor = System.Drawing.Color.White;
			this.StarLabel.Location = new System.Drawing.Point(215, 9);
			this.StarLabel.Name = "StarLabel";
			this.StarLabel.Size = new System.Drawing.Size(41, 13);
			this.StarLabel.TabIndex = 5;
			this.StarLabel.Text = "0 starts";
			// 
			// Star5
			// 
			this.Star5.Image = global::SecureDeleteWinForms.Properties.Resources.star_disabled;
			this.Star5.Location = new System.Drawing.Point(192, 8);
			this.Star5.Name = "Star5";
			this.Star5.Size = new System.Drawing.Size(20, 16);
			this.Star5.TabIndex = 4;
			this.Star5.TabStop = false;
			this.Star5.MouseLeave += new System.EventHandler(this.Star5_MouseLeave);
			this.Star5.Click += new System.EventHandler(this.Star5_Click);
			this.Star5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Star5_MouseUp);
			this.Star5.MouseEnter += new System.EventHandler(this.pictureBox5_MouseEnter);
			// 
			// Star3
			// 
			this.Star3.Image = global::SecureDeleteWinForms.Properties.Resources.star;
			this.Star3.Location = new System.Drawing.Point(155, 8);
			this.Star3.Name = "Star3";
			this.Star3.Size = new System.Drawing.Size(20, 16);
			this.Star3.TabIndex = 3;
			this.Star3.TabStop = false;
			this.Star3.MouseLeave += new System.EventHandler(this.Star3_MouseLeave);
			this.Star3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Star3_MouseUp);
			this.Star3.MouseEnter += new System.EventHandler(this.Star3_MouseEnter);
			// 
			// Star2
			// 
			this.Star2.Image = global::SecureDeleteWinForms.Properties.Resources.star;
			this.Star2.Location = new System.Drawing.Point(136, 8);
			this.Star2.Name = "Star2";
			this.Star2.Size = new System.Drawing.Size(20, 16);
			this.Star2.TabIndex = 2;
			this.Star2.TabStop = false;
			this.Star2.MouseLeave += new System.EventHandler(this.Star2_MouseLeave);
			this.Star2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Star2_MouseUp);
			this.Star2.MouseEnter += new System.EventHandler(this.Start2_MouseEnter);
			// 
			// Star4
			// 
			this.Star4.Image = global::SecureDeleteWinForms.Properties.Resources.star_disabled;
			this.Star4.Location = new System.Drawing.Point(174, 8);
			this.Star4.Name = "Star4";
			this.Star4.Size = new System.Drawing.Size(20, 16);
			this.Star4.TabIndex = 1;
			this.Star4.TabStop = false;
			this.Star4.MouseLeave += new System.EventHandler(this.Star4_MouseLeave);
			this.Star4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Star4_MouseUp);
			this.Star4.MouseEnter += new System.EventHandler(this.Star4_MouseEnter);
			// 
			// Star1
			// 
			this.Star1.Image = global::SecureDeleteWinForms.Properties.Resources.star;
			this.Star1.Location = new System.Drawing.Point(117, 8);
			this.Star1.Name = "Star1";
			this.Star1.Size = new System.Drawing.Size(20, 16);
			this.Star1.TabIndex = 0;
			this.Star1.TabStop = false;
			this.Star1.MouseLeave += new System.EventHandler(this.Star1_MouseLeave);
			this.Star1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Star1_MouseUp);
			this.Star1.MouseEnter += new System.EventHandler(this.Star1_MouseEnter);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Image = global::SecureDeleteWinForms.Properties.Resources.delete_profile;
			this.button1.Location = new System.Drawing.Point(846, -1);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(23, 163);
			this.button1.TabIndex = 28;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// TagsPanel
			// 
			this.TagsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.TagsPanel.BackColor = System.Drawing.Color.White;
			this.TagsPanel.Controls.Add(this.TagList);
			this.TagsPanel.Location = new System.Drawing.Point(8, 33);
			this.TagsPanel.Name = "TagsPanel";
			this.TagsPanel.Size = new System.Drawing.Size(725, 123);
			this.TagsPanel.TabIndex = 41;
			this.TagsPanel.Visible = false;
			// 
			// TagList
			// 
			this.TagList.CheckBoxes = true;
			this.TagList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.TagList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TagList.GridLines = true;
			this.TagList.LabelEdit = true;
			this.TagList.Location = new System.Drawing.Point(0, 0);
			this.TagList.Name = "TagList";
			this.TagList.Size = new System.Drawing.Size(725, 123);
			this.TagList.TabIndex = 0;
			this.TagList.UseCompatibleStateImageBehavior = false;
			this.TagList.View = System.Windows.Forms.View.Details;
			this.TagList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.TagList_ItemChecked);
			this.TagList.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.TagList_AfterLabelEdit);
			this.TagList.SelectedIndexChanged += new System.EventHandler(this.TagList_SelectedIndexChanged);
			this.TagList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TagList_MouseUp);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Tag";
			this.columnHeader1.Width = 289;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Match case";
			this.columnHeader2.Width = 80;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Regular expression";
			this.columnHeader3.Width = 115;
			// 
			// AddTagButton
			// 
			this.AddTagButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.AddTagButton.Image = global::SecureDeleteWinForms.Properties.Resources.add_profile;
			this.AddTagButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.AddTagButton.Location = new System.Drawing.Point(739, 33);
			this.AddTagButton.Name = "AddTagButton";
			this.AddTagButton.Size = new System.Drawing.Size(100, 23);
			this.AddTagButton.TabIndex = 42;
			this.AddTagButton.Text = "Add tag";
			this.AddTagButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.AddTagButton.UseVisualStyleBackColor = true;
			this.AddTagButton.Click += new System.EventHandler(this.button2_Click);
			// 
			// RemoveTagButton
			// 
			this.RemoveTagButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.RemoveTagButton.Enabled = false;
			this.RemoveTagButton.Image = global::SecureDeleteWinForms.Properties.Resources.delete_profile;
			this.RemoveTagButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.RemoveTagButton.Location = new System.Drawing.Point(739, 59);
			this.RemoveTagButton.Name = "RemoveTagButton";
			this.RemoveTagButton.Size = new System.Drawing.Size(100, 23);
			this.RemoveTagButton.TabIndex = 43;
			this.RemoveTagButton.Text = "Remove tags";
			this.RemoveTagButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.RemoveTagButton.UseVisualStyleBackColor = true;
			this.RemoveTagButton.Click += new System.EventHandler(this.button3_Click);
			// 
			// YesNoCombobox
			// 
			this.YesNoCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.YesNoCombobox.FormattingEnabled = true;
			this.YesNoCombobox.Items.AddRange(new object[] {
            "Yes",
            "No"});
			this.YesNoCombobox.Location = new System.Drawing.Point(739, 109);
			this.YesNoCombobox.Name = "YesNoCombobox";
			this.YesNoCombobox.Size = new System.Drawing.Size(87, 21);
			this.YesNoCombobox.TabIndex = 44;
			this.YesNoCombobox.Visible = false;
			this.YesNoCombobox.SelectedIndexChanged += new System.EventHandler(this.YesNoCombobox_SelectedIndexChanged);
			this.YesNoCombobox.Leave += new System.EventHandler(this.YesNoCombobox_Leave);
			// 
			// TagsPanel2
			// 
			this.TagsPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.TagsPanel2.Controls.Add(this.TagLabel2);
			this.TagsPanel2.Controls.Add(this.TagImplicationCombobox);
			this.TagsPanel2.Controls.Add(this.TagLabel1);
			this.TagsPanel2.Location = new System.Drawing.Point(286, 0);
			this.TagsPanel2.Name = "TagsPanel2";
			this.TagsPanel2.Size = new System.Drawing.Size(406, 34);
			this.TagsPanel2.TabIndex = 45;
			this.TagsPanel2.Visible = false;
			// 
			// TagLabel2
			// 
			this.TagLabel2.AutoSize = true;
			this.TagLabel2.ForeColor = System.Drawing.Color.White;
			this.TagLabel2.Location = new System.Drawing.Point(153, 10);
			this.TagLabel2.Name = "TagLabel2";
			this.TagLabel2.Size = new System.Drawing.Size(99, 13);
			this.TagLabel2.TabIndex = 27;
			this.TagLabel2.Text = "of the folowing tags";
			// 
			// TagImplicationCombobox
			// 
			this.TagImplicationCombobox.BackColor = System.Drawing.SystemColors.Window;
			this.TagImplicationCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TagImplicationCombobox.FormattingEnabled = true;
			this.TagImplicationCombobox.Items.AddRange(new object[] {
            "All",
            "Some"});
			this.TagImplicationCombobox.Location = new System.Drawing.Point(71, 6);
			this.TagImplicationCombobox.Name = "TagImplicationCombobox";
			this.TagImplicationCombobox.Size = new System.Drawing.Size(76, 21);
			this.TagImplicationCombobox.TabIndex = 26;
			this.TagImplicationCombobox.SelectedIndexChanged += new System.EventHandler(this.TagImplicationCombobox_SelectedIndexChanged);
			// 
			// TagLabel1
			// 
			this.TagLabel1.AutoSize = true;
			this.TagLabel1.ForeColor = System.Drawing.Color.White;
			this.TagLabel1.Location = new System.Drawing.Point(-1, 10);
			this.TagLabel1.Name = "TagLabel1";
			this.TagLabel1.Size = new System.Drawing.Size(68, 13);
			this.TagLabel1.TabIndex = 0;
			this.TagLabel1.Text = "composed of";
			// 
			// ImageFilterView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.TagsPanel2);
			this.Controls.Add(this.YesNoCombobox);
			this.Controls.Add(this.RemoveTagButton);
			this.Controls.Add(this.AddTagButton);
			this.Controls.Add(this.TagsPanel);
			this.Controls.Add(this.RatingPanel);
			this.Controls.Add(this.NumberPanel);
			this.Controls.Add(this.OrientationPanel);
			this.Controls.Add(this.MeteringPanel);
			this.Controls.Add(this.FlashPanel);
			this.Controls.Add(this.ExposureTimePanel);
			this.Controls.Add(this.ProgramPanel);
			this.Controls.Add(this.TextPanel);
			this.Controls.Add(this.DatePanel);
			this.Controls.Add(this.StatusIndicator);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.NameTextbox);
			this.Controls.Add(this.comboBox2);
			this.Controls.Add(this.FilterType);
			this.Controls.Add(this.checkBox1);
			this.Name = "ImageFilterView";
			this.Size = new System.Drawing.Size(868, 159);
			this.TextPanel.ResumeLayout(false);
			this.TextPanel.PerformLayout();
			this.DatePanel.ResumeLayout(false);
			this.NumberPanel.ResumeLayout(false);
			this.NumberPanel.PerformLayout();
			this.ProgramPanel.ResumeLayout(false);
			this.ExposureTimePanel.ResumeLayout(false);
			this.ExposureTimePanel.PerformLayout();
			this.FlashPanel.ResumeLayout(false);
			this.FlashPanel.PerformLayout();
			this.MeteringPanel.ResumeLayout(false);
			this.OrientationPanel.ResumeLayout(false);
			this.RatingPanel.ResumeLayout(false);
			this.RatingPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Star5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Star3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Star2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Star4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Star1)).EndInit();
			this.TagsPanel.ResumeLayout(false);
			this.TagsPanel2.ResumeLayout(false);
			this.TagsPanel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel StatusIndicator;
		private System.Windows.Forms.ToolTip ErrorTooltip;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox NameTextbox;
		private System.Windows.Forms.ComboBox FilterType;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Panel TextPanel;
		private System.Windows.Forms.TextBox TextFilterValue;
		private System.Windows.Forms.CheckBox MatchCaseCheckbox;
		private System.Windows.Forms.CheckBox RegexCheckbox;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.Panel DatePanel;
		private System.Windows.Forms.DateTimePicker DateValue;
		private System.Windows.Forms.ComboBox DateImplicationCombobox;
		private System.Windows.Forms.Panel NumberPanel;
		private System.Windows.Forms.ComboBox SizeImplicationCombobox;
		private System.Windows.Forms.TextBox SizeValue;
		private System.Windows.Forms.Label NumberFilterLabel;
		private System.Windows.Forms.Panel ProgramPanel;
		private System.Windows.Forms.ComboBox ProgramCombobox;
		private System.Windows.Forms.Panel ExposureTimePanel;
		private System.Windows.Forms.Label ExposureTimeLabel;
		private System.Windows.Forms.TextBox NumeratorTextbox;
		private System.Windows.Forms.ComboBox ExposureTimeSizeCombobox;
		private System.Windows.Forms.TextBox DenominatorTextbox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel FlashPanel;
		private System.Windows.Forms.Label FlashLabel;
		private System.Windows.Forms.Panel MeteringPanel;
		private System.Windows.Forms.ComboBox MeteringComboBox;
		private System.Windows.Forms.Panel OrientationPanel;
		private System.Windows.Forms.ComboBox OrientationCombobox;
		private System.Windows.Forms.Panel RatingPanel;
		private System.Windows.Forms.PictureBox Star1;
		private System.Windows.Forms.PictureBox Star4;
		private System.Windows.Forms.PictureBox Star5;
		private System.Windows.Forms.PictureBox Star3;
		private System.Windows.Forms.PictureBox Star2;
		private System.Windows.Forms.Label StarLabel;
		private System.Windows.Forms.ComboBox RatingCombobox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel TagsPanel;
		private System.Windows.Forms.Button AddTagButton;
		private System.Windows.Forms.Button RemoveTagButton;
		private System.Windows.Forms.ListView TagList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ComboBox YesNoCombobox;
		private System.Windows.Forms.Panel TagsPanel2;
		private System.Windows.Forms.ComboBox TagImplicationCombobox;
		private System.Windows.Forms.Label TagLabel1;
		private System.Windows.Forms.Label TagLabel2;
	}
}
