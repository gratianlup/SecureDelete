namespace SecureDeleteWinForms
{
	partial class ScheduleOptions
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
			this.SelectorPanel = new System.Windows.Forms.Panel();
			this.OptionsSelector = new SecureDeleteWinForms.PanelSelectControl();
			this.AfterSelector = new SecureDeleteWinForms.PanelSelectControl();
			this.BeforeSelector = new SecureDeleteWinForms.PanelSelectControl();
			this.ScheduleSelector = new SecureDeleteWinForms.PanelSelectControl();
			this.GeneralSelector = new SecureDeleteWinForms.PanelSelectControl();
			this.panel3 = new System.Windows.Forms.Panel();
			this.CloseButton = new System.Windows.Forms.Button();
			this.SaveButton = new System.Windows.Forms.Button();
			this.GeneralPanel = new System.Windows.Forms.Panel();
			this.EnabledCheckbox = new System.Windows.Forms.CheckBox();
			this.SaveReportsCheckbox = new System.Windows.Forms.CheckBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.DescriptionTextbox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.NameTextbox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SchedulePanel = new System.Windows.Forms.Panel();
			this.pictureBox6 = new System.Windows.Forms.PictureBox();
			this.pictureBox5 = new System.Windows.Forms.PictureBox();
			this.radioButton6 = new System.Windows.Forms.RadioButton();
			this.radioButton5 = new System.Windows.Forms.RadioButton();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.radioButton4 = new System.Windows.Forms.RadioButton();
			this.ScheduleControlHost = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.BeforePanel = new System.Windows.Forms.Panel();
			this.BeforeActionEditor = new SecureDeleteWinForms.ActionEditor();
			this.AfterPanel = new System.Windows.Forms.Panel();
			this.AfterActionEditor = new SecureDeleteWinForms.ActionEditor();
			this.ErrorTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.OptionsPanel = new System.Windows.Forms.Panel();
			this.OptionsHost = new SecureDeleteWinForms.PanelExHost();
			this.panelEx1 = new SecureDeleteWinForms.PanelEx();
			this.WipeOptionsEditor = new SecureDeleteWinForms.Options.WipeOptionsEditor();
			this.panelEx2 = new SecureDeleteWinForms.PanelEx();
			this.RandomOptionsEditor = new SecureDeleteWinForms.Options.RandomOptionsEditor();
			this.panelEx3 = new SecureDeleteWinForms.PanelEx();
			this.logOptionsEditor1 = new SecureDeleteWinForms.Options.LogOptionsEditor();
			this.DefaultOptionsRadioButton = new System.Windows.Forms.RadioButton();
			this.CustomOptionsRadioButton = new System.Windows.Forms.RadioButton();
			this.SelectorPanel.SuspendLayout();
			this.panel3.SuspendLayout();
			this.GeneralPanel.SuspendLayout();
			this.SchedulePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.BeforePanel.SuspendLayout();
			this.AfterPanel.SuspendLayout();
			this.OptionsPanel.SuspendLayout();
			this.OptionsHost.SuspendLayout();
			this.panelEx1.SuspendLayout();
			this.panelEx2.SuspendLayout();
			this.panelEx3.SuspendLayout();
			this.SuspendLayout();
			// 
			// SelectorPanel
			// 
			this.SelectorPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.SelectorPanel.Controls.Add(this.OptionsSelector);
			this.SelectorPanel.Controls.Add(this.AfterSelector);
			this.SelectorPanel.Controls.Add(this.BeforeSelector);
			this.SelectorPanel.Controls.Add(this.ScheduleSelector);
			this.SelectorPanel.Controls.Add(this.GeneralSelector);
			this.SelectorPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.SelectorPanel.Location = new System.Drawing.Point(0, 0);
			this.SelectorPanel.Name = "SelectorPanel";
			this.SelectorPanel.Size = new System.Drawing.Size(769, 24);
			this.SelectorPanel.TabIndex = 3;
			// 
			// OptionsSelector
			// 
			this.OptionsSelector.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.OptionsSelector.Location = new System.Drawing.Point(127, 3);
			this.OptionsSelector.Name = "OptionsSelector";
			this.OptionsSelector.Reversed = false;
			this.OptionsSelector.Selected = false;
			this.OptionsSelector.SelectedColor = System.Drawing.SystemColors.Control;
			this.OptionsSelector.SelectedTextColor = System.Drawing.SystemColors.ControlText;
			this.OptionsSelector.SelectorText = "Wipe Options";
			this.OptionsSelector.Size = new System.Drawing.Size(80, 21);
			this.OptionsSelector.TabIndex = 4;
			this.OptionsSelector.TextColor = System.Drawing.Color.White;
			this.OptionsSelector.SelectedStateChanged += new System.EventHandler(this.OptionsSelector_SelectedStateChanged);
			// 
			// AfterSelector
			// 
			this.AfterSelector.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.AfterSelector.Location = new System.Drawing.Point(328, 3);
			this.AfterSelector.Name = "AfterSelector";
			this.AfterSelector.Reversed = false;
			this.AfterSelector.Selected = false;
			this.AfterSelector.SelectedColor = System.Drawing.SystemColors.Control;
			this.AfterSelector.SelectedTextColor = System.Drawing.SystemColors.ControlText;
			this.AfterSelector.SelectorText = "After Wipe Actions";
			this.AfterSelector.Size = new System.Drawing.Size(105, 21);
			this.AfterSelector.TabIndex = 3;
			this.AfterSelector.TextColor = System.Drawing.Color.White;
			this.AfterSelector.SelectedStateChanged += new System.EventHandler(this.AfterSelector_SelectedStateChanged);
			// 
			// BeforeSelector
			// 
			this.BeforeSelector.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.BeforeSelector.Location = new System.Drawing.Point(210, 3);
			this.BeforeSelector.Name = "BeforeSelector";
			this.BeforeSelector.Reversed = false;
			this.BeforeSelector.Selected = false;
			this.BeforeSelector.SelectedColor = System.Drawing.SystemColors.Control;
			this.BeforeSelector.SelectedTextColor = System.Drawing.SystemColors.ControlText;
			this.BeforeSelector.SelectorText = "Before Wipe Actions";
			this.BeforeSelector.Size = new System.Drawing.Size(115, 21);
			this.BeforeSelector.TabIndex = 2;
			this.BeforeSelector.TextColor = System.Drawing.Color.White;
			this.BeforeSelector.SelectedStateChanged += new System.EventHandler(this.BeforeSelector_SelectedStateChanged);
			// 
			// ScheduleSelector
			// 
			this.ScheduleSelector.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.ScheduleSelector.Location = new System.Drawing.Point(61, 3);
			this.ScheduleSelector.Name = "ScheduleSelector";
			this.ScheduleSelector.Reversed = false;
			this.ScheduleSelector.Selected = false;
			this.ScheduleSelector.SelectedColor = System.Drawing.SystemColors.Control;
			this.ScheduleSelector.SelectedTextColor = System.Drawing.SystemColors.ControlText;
			this.ScheduleSelector.SelectorText = "Schedule";
			this.ScheduleSelector.Size = new System.Drawing.Size(63, 21);
			this.ScheduleSelector.TabIndex = 1;
			this.ScheduleSelector.TextColor = System.Drawing.Color.White;
			this.ScheduleSelector.SelectedStateChanged += new System.EventHandler(this.ScheduleSelector_SelectedStateChanged);
			// 
			// GeneralSelector
			// 
			this.GeneralSelector.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.GeneralSelector.Location = new System.Drawing.Point(2, 3);
			this.GeneralSelector.Name = "GeneralSelector";
			this.GeneralSelector.Reversed = false;
			this.GeneralSelector.Selected = false;
			this.GeneralSelector.SelectedColor = System.Drawing.SystemColors.Control;
			this.GeneralSelector.SelectedTextColor = System.Drawing.SystemColors.ControlText;
			this.GeneralSelector.SelectorText = "General";
			this.GeneralSelector.Size = new System.Drawing.Size(55, 21);
			this.GeneralSelector.TabIndex = 0;
			this.GeneralSelector.TextColor = System.Drawing.Color.White;
			this.GeneralSelector.SelectedStateChanged += new System.EventHandler(this.WipeModuleSelector_SelectedStateChanged);
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.CloseButton);
			this.panel3.Controls.Add(this.SaveButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 554);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(769, 30);
			this.panel3.TabIndex = 11;
			// 
			// CloseButton
			// 
			this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CloseButton.BackColor = System.Drawing.SystemColors.Control;
			this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CloseButton.Location = new System.Drawing.Point(688, 4);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(75, 23);
			this.CloseButton.TabIndex = 1;
			this.CloseButton.Text = "Close";
			this.CloseButton.UseVisualStyleBackColor = true;
			// 
			// SaveButton
			// 
			this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SaveButton.BackColor = System.Drawing.SystemColors.Control;
			this.SaveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.SaveButton.Location = new System.Drawing.Point(611, 4);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(75, 23);
			this.SaveButton.TabIndex = 0;
			this.SaveButton.Text = "Save";
			this.SaveButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.SaveButton.UseVisualStyleBackColor = true;
			// 
			// GeneralPanel
			// 
			this.GeneralPanel.BackColor = System.Drawing.SystemColors.Control;
			this.GeneralPanel.Controls.Add(this.EnabledCheckbox);
			this.GeneralPanel.Controls.Add(this.SaveReportsCheckbox);
			this.GeneralPanel.Controls.Add(this.panel2);
			this.GeneralPanel.Controls.Add(this.DescriptionTextbox);
			this.GeneralPanel.Controls.Add(this.label2);
			this.GeneralPanel.Controls.Add(this.NameTextbox);
			this.GeneralPanel.Controls.Add(this.label1);
			this.GeneralPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GeneralPanel.Location = new System.Drawing.Point(0, 24);
			this.GeneralPanel.Name = "GeneralPanel";
			this.GeneralPanel.Size = new System.Drawing.Size(769, 530);
			this.GeneralPanel.TabIndex = 12;
			// 
			// EnabledCheckbox
			// 
			this.EnabledCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.EnabledCheckbox.AutoSize = true;
			this.EnabledCheckbox.Checked = true;
			this.EnabledCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.EnabledCheckbox.Location = new System.Drawing.Point(15, 464);
			this.EnabledCheckbox.Name = "EnabledCheckbox";
			this.EnabledCheckbox.Size = new System.Drawing.Size(65, 17);
			this.EnabledCheckbox.TabIndex = 6;
			this.EnabledCheckbox.Text = "Enabled";
			this.EnabledCheckbox.UseVisualStyleBackColor = true;
			this.EnabledCheckbox.CheckedChanged += new System.EventHandler(this.EnabledCheckbox_CheckedChanged);
			// 
			// SaveReportsCheckbox
			// 
			this.SaveReportsCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.SaveReportsCheckbox.AutoSize = true;
			this.SaveReportsCheckbox.Location = new System.Drawing.Point(15, 482);
			this.SaveReportsCheckbox.Name = "SaveReportsCheckbox";
			this.SaveReportsCheckbox.Size = new System.Drawing.Size(86, 17);
			this.SaveReportsCheckbox.TabIndex = 5;
			this.SaveReportsCheckbox.Text = "Save reports";
			this.SaveReportsCheckbox.UseVisualStyleBackColor = true;
			this.SaveReportsCheckbox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel2.Location = new System.Drawing.Point(12, 451);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(745, 1);
			this.panel2.TabIndex = 4;
			// 
			// DescriptionTextbox
			// 
			this.DescriptionTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.DescriptionTextbox.BackColor = System.Drawing.SystemColors.Window;
			this.DescriptionTextbox.Location = new System.Drawing.Point(78, 43);
			this.DescriptionTextbox.Multiline = true;
			this.DescriptionTextbox.Name = "DescriptionTextbox";
			this.DescriptionTextbox.Size = new System.Drawing.Size(679, 394);
			this.DescriptionTextbox.TabIndex = 3;
			this.DescriptionTextbox.TextChanged += new System.EventHandler(this.DescriptionTextbox_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Description";
			// 
			// NameTextbox
			// 
			this.NameTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.NameTextbox.Location = new System.Drawing.Point(78, 11);
			this.NameTextbox.Name = "NameTextbox";
			this.NameTextbox.Size = new System.Drawing.Size(679, 20);
			this.NameTextbox.TabIndex = 1;
			this.NameTextbox.Validating += new System.ComponentModel.CancelEventHandler(this.NameTextbox_Validating);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			// 
			// SchedulePanel
			// 
			this.SchedulePanel.BackColor = System.Drawing.SystemColors.Control;
			this.SchedulePanel.Controls.Add(this.pictureBox6);
			this.SchedulePanel.Controls.Add(this.pictureBox5);
			this.SchedulePanel.Controls.Add(this.radioButton6);
			this.SchedulePanel.Controls.Add(this.radioButton5);
			this.SchedulePanel.Controls.Add(this.pictureBox4);
			this.SchedulePanel.Controls.Add(this.pictureBox3);
			this.SchedulePanel.Controls.Add(this.pictureBox2);
			this.SchedulePanel.Controls.Add(this.pictureBox1);
			this.SchedulePanel.Controls.Add(this.radioButton4);
			this.SchedulePanel.Controls.Add(this.ScheduleControlHost);
			this.SchedulePanel.Controls.Add(this.panel1);
			this.SchedulePanel.Controls.Add(this.radioButton3);
			this.SchedulePanel.Controls.Add(this.radioButton2);
			this.SchedulePanel.Controls.Add(this.radioButton1);
			this.SchedulePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SchedulePanel.Location = new System.Drawing.Point(0, 24);
			this.SchedulePanel.Name = "SchedulePanel";
			this.SchedulePanel.Size = new System.Drawing.Size(769, 530);
			this.SchedulePanel.TabIndex = 13;
			// 
			// pictureBox6
			// 
			this.pictureBox6.Image = global::SecureDeleteWinForms.Properties.Resources.application_double;
			this.pictureBox6.Location = new System.Drawing.Point(9, 124);
			this.pictureBox6.Name = "pictureBox6";
			this.pictureBox6.Size = new System.Drawing.Size(16, 16);
			this.pictureBox6.TabIndex = 14;
			this.pictureBox6.TabStop = false;
			// 
			// pictureBox5
			// 
			this.pictureBox5.Image = global::SecureDeleteWinForms.Properties.Resources.cup;
			this.pictureBox5.Location = new System.Drawing.Point(8, 102);
			this.pictureBox5.Name = "pictureBox5";
			this.pictureBox5.Size = new System.Drawing.Size(16, 16);
			this.pictureBox5.TabIndex = 13;
			this.pictureBox5.TabStop = false;
			// 
			// radioButton6
			// 
			this.radioButton6.AutoSize = true;
			this.radioButton6.Enabled = false;
			this.radioButton6.Location = new System.Drawing.Point(31, 122);
			this.radioButton6.Name = "radioButton6";
			this.radioButton6.Size = new System.Drawing.Size(77, 17);
			this.radioButton6.TabIndex = 12;
			this.radioButton6.TabStop = true;
			this.radioButton6.Text = "Application";
			this.radioButton6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.radioButton6.UseVisualStyleBackColor = true;
			// 
			// radioButton5
			// 
			this.radioButton5.AutoSize = true;
			this.radioButton5.Enabled = false;
			this.radioButton5.Location = new System.Drawing.Point(31, 101);
			this.radioButton5.Name = "radioButton5";
			this.radioButton5.Size = new System.Drawing.Size(42, 17);
			this.radioButton5.TabIndex = 11;
			this.radioButton5.TabStop = true;
			this.radioButton5.Text = "Idle";
			this.radioButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.radioButton5.UseVisualStyleBackColor = true;
			// 
			// pictureBox4
			// 
			this.pictureBox4.Image = global::SecureDeleteWinForms.Properties.Resources.calendar;
			this.pictureBox4.Location = new System.Drawing.Point(8, 34);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(16, 16);
			this.pictureBox4.TabIndex = 10;
			this.pictureBox4.TabStop = false;
			// 
			// pictureBox3
			// 
			this.pictureBox3.Image = global::SecureDeleteWinForms.Properties.Resources.calendar_view_day;
			this.pictureBox3.Location = new System.Drawing.Point(9, 11);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(16, 16);
			this.pictureBox3.TabIndex = 9;
			this.pictureBox3.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::SecureDeleteWinForms.Properties.Resources.calendar_view_week;
			this.pictureBox2.Location = new System.Drawing.Point(8, 57);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(16, 16);
			this.pictureBox2.TabIndex = 8;
			this.pictureBox2.TabStop = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::SecureDeleteWinForms.Properties.Resources.calendar_view_month;
			this.pictureBox1.Location = new System.Drawing.Point(8, 80);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(16, 16);
			this.pictureBox1.TabIndex = 7;
			this.pictureBox1.TabStop = false;
			// 
			// radioButton4
			// 
			this.radioButton4.AutoSize = true;
			this.radioButton4.Location = new System.Drawing.Point(32, 11);
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.Size = new System.Drawing.Size(67, 17);
			this.radioButton4.TabIndex = 6;
			this.radioButton4.TabStop = true;
			this.radioButton4.Text = "One time";
			this.radioButton4.UseVisualStyleBackColor = true;
			this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
			// 
			// ScheduleControlHost
			// 
			this.ScheduleControlHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ScheduleControlHost.Location = new System.Drawing.Point(136, 12);
			this.ScheduleControlHost.Name = "ScheduleControlHost";
			this.ScheduleControlHost.Size = new System.Drawing.Size(621, 508);
			this.ScheduleControlHost.TabIndex = 5;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel1.Location = new System.Drawing.Point(128, 11);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1, 511);
			this.panel1.TabIndex = 4;
			// 
			// radioButton3
			// 
			this.radioButton3.AutoSize = true;
			this.radioButton3.Location = new System.Drawing.Point(32, 80);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(62, 17);
			this.radioButton3.TabIndex = 3;
			this.radioButton3.TabStop = true;
			this.radioButton3.Text = "Monthly";
			this.radioButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.radioButton3.UseVisualStyleBackColor = true;
			this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
			// 
			// radioButton2
			// 
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new System.Drawing.Point(32, 57);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(61, 17);
			this.radioButton2.TabIndex = 2;
			this.radioButton2.TabStop = true;
			this.radioButton2.Text = "Weekly";
			this.radioButton2.UseVisualStyleBackColor = true;
			this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Location = new System.Drawing.Point(32, 34);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(48, 17);
			this.radioButton1.TabIndex = 1;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "Daily";
			this.radioButton1.UseVisualStyleBackColor = true;
			this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
			// 
			// BeforePanel
			// 
			this.BeforePanel.BackColor = System.Drawing.SystemColors.Control;
			this.BeforePanel.Controls.Add(this.BeforeActionEditor);
			this.BeforePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BeforePanel.Location = new System.Drawing.Point(0, 24);
			this.BeforePanel.Name = "BeforePanel";
			this.BeforePanel.Size = new System.Drawing.Size(769, 530);
			this.BeforePanel.TabIndex = 14;
			// 
			// BeforeActionEditor
			// 
			this.BeforeActionEditor.Actions = null;
			this.BeforeActionEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BeforeActionEditor.Location = new System.Drawing.Point(0, 0);
			this.BeforeActionEditor.Name = "BeforeActionEditor";
			this.BeforeActionEditor.Options = null;
			this.BeforeActionEditor.Size = new System.Drawing.Size(769, 530);
			this.BeforeActionEditor.TabIndex = 0;
			// 
			// AfterPanel
			// 
			this.AfterPanel.BackColor = System.Drawing.SystemColors.Control;
			this.AfterPanel.Controls.Add(this.AfterActionEditor);
			this.AfterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AfterPanel.Location = new System.Drawing.Point(0, 24);
			this.AfterPanel.Name = "AfterPanel";
			this.AfterPanel.Size = new System.Drawing.Size(769, 530);
			this.AfterPanel.TabIndex = 15;
			// 
			// AfterActionEditor
			// 
			this.AfterActionEditor.Actions = null;
			this.AfterActionEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AfterActionEditor.Location = new System.Drawing.Point(0, 0);
			this.AfterActionEditor.Name = "AfterActionEditor";
			this.AfterActionEditor.Options = null;
			this.AfterActionEditor.Size = new System.Drawing.Size(769, 530);
			this.AfterActionEditor.TabIndex = 0;
			// 
			// ErrorTooltip
			// 
			this.ErrorTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
			this.ErrorTooltip.ToolTipTitle = "Message";
			// 
			// OptionsPanel
			// 
			this.OptionsPanel.BackColor = System.Drawing.SystemColors.Control;
			this.OptionsPanel.Controls.Add(this.OptionsHost);
			this.OptionsPanel.Controls.Add(this.DefaultOptionsRadioButton);
			this.OptionsPanel.Controls.Add(this.CustomOptionsRadioButton);
			this.OptionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OptionsPanel.Location = new System.Drawing.Point(0, 24);
			this.OptionsPanel.Name = "OptionsPanel";
			this.OptionsPanel.Size = new System.Drawing.Size(769, 530);
			this.OptionsPanel.TabIndex = 16;
			// 
			// OptionsHost
			// 
			this.OptionsHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.OptionsHost.Animate = true;
			this.OptionsHost.AutoScroll = true;
			this.OptionsHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.OptionsHost.Controls.Add(this.panelEx1);
			this.OptionsHost.Controls.Add(this.panelEx2);
			this.OptionsHost.Controls.Add(this.panelEx3);
			this.OptionsHost.Location = new System.Drawing.Point(12, 53);
			this.OptionsHost.Name = "OptionsHost";
			this.OptionsHost.Padding = new System.Windows.Forms.Padding(2);
			this.OptionsHost.PanelDistance = 6;
			this.OptionsHost.Size = new System.Drawing.Size(745, 467);
			this.OptionsHost.TabIndex = 8;
			// 
			// panelEx1
			// 
			this.panelEx1.AllowCollapse = true;
			this.panelEx1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelEx1.Collapsed = false;
			this.panelEx1.CollapsedSize = 24;
			this.panelEx1.Controls.Add(this.WipeOptionsEditor);
			this.panelEx1.ExpandedSize = 415;
			this.panelEx1.GradientColor1 = System.Drawing.Color.White;
			this.panelEx1.GradientColor2 = System.Drawing.SystemColors.Control;
			this.panelEx1.Location = new System.Drawing.Point(3, 3);
			this.panelEx1.Name = "panelEx1";
			this.panelEx1.Size = new System.Drawing.Size(720, 415);
			this.panelEx1.Subtitle = null;
			this.panelEx1.TabIndex = 0;
			this.panelEx1.TextColor = System.Drawing.SystemColors.WindowText;
			this.panelEx1.Title = "General";
			// 
			// WipeOptionsEditor
			// 
			this.WipeOptionsEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.WipeOptionsEditor.Location = new System.Drawing.Point(-1, 25);
			this.WipeOptionsEditor.MethodManager = null;
			this.WipeOptionsEditor.Name = "WipeOptionsEditor";
			this.WipeOptionsEditor.Options = null;
			this.WipeOptionsEditor.Size = new System.Drawing.Size(721, 383);
			this.WipeOptionsEditor.TabIndex = 1;
			// 
			// panelEx2
			// 
			this.panelEx2.AllowCollapse = true;
			this.panelEx2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelEx2.Collapsed = false;
			this.panelEx2.CollapsedSize = 24;
			this.panelEx2.Controls.Add(this.RandomOptionsEditor);
			this.panelEx2.ExpandedSize = 244;
			this.panelEx2.GradientColor1 = System.Drawing.Color.White;
			this.panelEx2.GradientColor2 = System.Drawing.SystemColors.Control;
			this.panelEx2.Location = new System.Drawing.Point(3, 425);
			this.panelEx2.Name = "panelEx2";
			this.panelEx2.Size = new System.Drawing.Size(720, 244);
			this.panelEx2.Subtitle = null;
			this.panelEx2.TabIndex = 1;
			this.panelEx2.TextColor = System.Drawing.SystemColors.WindowText;
			this.panelEx2.Title = "Random";
			// 
			// RandomOptionsEditor
			// 
			this.RandomOptionsEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.RandomOptionsEditor.Location = new System.Drawing.Point(0, 25);
			this.RandomOptionsEditor.Name = "RandomOptionsEditor";
			this.RandomOptionsEditor.Options = null;
			this.RandomOptionsEditor.Size = new System.Drawing.Size(718, 222);
			this.RandomOptionsEditor.TabIndex = 2;
			// 
			// panelEx3
			// 
			this.panelEx3.AllowCollapse = true;
			this.panelEx3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelEx3.Collapsed = false;
			this.panelEx3.CollapsedSize = 24;
			this.panelEx3.Controls.Add(this.logOptionsEditor1);
			this.panelEx3.ExpandedSize = 160;
			this.panelEx3.GradientColor1 = System.Drawing.Color.White;
			this.panelEx3.GradientColor2 = System.Drawing.SystemColors.Control;
			this.panelEx3.Location = new System.Drawing.Point(3, 676);
			this.panelEx3.Name = "panelEx3";
			this.panelEx3.Size = new System.Drawing.Size(720, 160);
			this.panelEx3.Subtitle = null;
			this.panelEx3.TabIndex = 2;
			this.panelEx3.TextColor = System.Drawing.SystemColors.WindowText;
			this.panelEx3.Title = "Log file";
			// 
			// logOptionsEditor1
			// 
			this.logOptionsEditor1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.logOptionsEditor1.Location = new System.Drawing.Point(0, 25);
			this.logOptionsEditor1.Name = "logOptionsEditor1";
			this.logOptionsEditor1.Size = new System.Drawing.Size(720, 137);
			this.logOptionsEditor1.TabIndex = 0;
			// 
			// DefaultOptionsRadioButton
			// 
			this.DefaultOptionsRadioButton.AutoSize = true;
			this.DefaultOptionsRadioButton.Location = new System.Drawing.Point(12, 11);
			this.DefaultOptionsRadioButton.Name = "DefaultOptionsRadioButton";
			this.DefaultOptionsRadioButton.Size = new System.Drawing.Size(116, 17);
			this.DefaultOptionsRadioButton.TabIndex = 6;
			this.DefaultOptionsRadioButton.TabStop = true;
			this.DefaultOptionsRadioButton.Text = "Use default options";
			this.DefaultOptionsRadioButton.UseVisualStyleBackColor = true;
			this.DefaultOptionsRadioButton.CheckedChanged += new System.EventHandler(this.DefaultOptionsRadioButton_CheckedChanged);
			// 
			// CustomOptionsRadioButton
			// 
			this.CustomOptionsRadioButton.AutoSize = true;
			this.CustomOptionsRadioButton.Location = new System.Drawing.Point(12, 30);
			this.CustomOptionsRadioButton.Name = "CustomOptionsRadioButton";
			this.CustomOptionsRadioButton.Size = new System.Drawing.Size(118, 17);
			this.CustomOptionsRadioButton.TabIndex = 1;
			this.CustomOptionsRadioButton.TabStop = true;
			this.CustomOptionsRadioButton.Text = "Use custom options";
			this.CustomOptionsRadioButton.UseVisualStyleBackColor = true;
			this.CustomOptionsRadioButton.CheckedChanged += new System.EventHandler(this.CustomOptionsRadioButton_CheckedChanged);
			// 
			// ScheduleOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(769, 584);
			this.Controls.Add(this.GeneralPanel);
			this.Controls.Add(this.SchedulePanel);
			this.Controls.Add(this.OptionsPanel);
			this.Controls.Add(this.AfterPanel);
			this.Controls.Add(this.BeforePanel);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.SelectorPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ScheduleOptions";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Scheduled Wipe Options";
			this.Load += new System.EventHandler(this.ScheduleOptions_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScheduleOptions_FormClosing);
			this.SelectorPanel.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.GeneralPanel.ResumeLayout(false);
			this.GeneralPanel.PerformLayout();
			this.SchedulePanel.ResumeLayout(false);
			this.SchedulePanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.BeforePanel.ResumeLayout(false);
			this.AfterPanel.ResumeLayout(false);
			this.OptionsPanel.ResumeLayout(false);
			this.OptionsPanel.PerformLayout();
			this.OptionsHost.ResumeLayout(false);
			this.panelEx1.ResumeLayout(false);
			this.panelEx2.ResumeLayout(false);
			this.panelEx3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel SelectorPanel;
		private PanelSelectControl BeforeSelector;
		private PanelSelectControl ScheduleSelector;
		private PanelSelectControl GeneralSelector;
		private PanelSelectControl AfterSelector;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.Panel GeneralPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox NameTextbox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox DescriptionTextbox;
		private System.Windows.Forms.Panel SchedulePanel;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel ScheduleControlHost;
		private System.Windows.Forms.Panel BeforePanel;
		private ActionEditor BeforeActionEditor;
		private System.Windows.Forms.Panel AfterPanel;
		private ActionEditor AfterActionEditor;
		private System.Windows.Forms.RadioButton radioButton4;
		private System.Windows.Forms.ToolTip ErrorTooltip;
		private System.Windows.Forms.Panel OptionsPanel;
		private System.Windows.Forms.RadioButton DefaultOptionsRadioButton;
		private System.Windows.Forms.RadioButton CustomOptionsRadioButton;
		private PanelSelectControl OptionsSelector;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.CheckBox SaveReportsCheckbox;
		private System.Windows.Forms.CheckBox EnabledCheckbox;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.PictureBox pictureBox4;
		private PanelExHost OptionsHost;
		private PanelEx panelEx1;
		private SecureDeleteWinForms.Options.WipeOptionsEditor WipeOptionsEditor;
		private PanelEx panelEx2;
		private SecureDeleteWinForms.Options.RandomOptionsEditor RandomOptionsEditor;
		private PanelEx panelEx3;
		private SecureDeleteWinForms.Options.LogOptionsEditor logOptionsEditor1;
		private System.Windows.Forms.PictureBox pictureBox5;
		private System.Windows.Forms.RadioButton radioButton6;
		private System.Windows.Forms.RadioButton radioButton5;
		private System.Windows.Forms.PictureBox pictureBox6;
	}
}