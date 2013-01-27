namespace SecureDeleteWinForms
{
	partial class DailyScheduleEditor
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.StartTimePicker = new System.Windows.Forms.DateTimePicker();
			this.StartDatePicker = new System.Windows.Forms.DateTimePicker();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.EndTimeCheckbox = new System.Windows.Forms.CheckBox();
			this.EndTimePicker = new System.Windows.Forms.DateTimePicker();
			this.EndDatePicker = new System.Windows.Forms.DateTimePicker();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.RecurenceUpDown = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RecurenceUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.StartTimePicker);
			this.groupBox1.Controls.Add(this.StartDatePicker);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(468, 49);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Start Time";
			// 
			// StartTimePicker
			// 
			this.StartTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.StartTimePicker.Location = new System.Drawing.Point(135, 19);
			this.StartTimePicker.Name = "StartTimePicker";
			this.StartTimePicker.Size = new System.Drawing.Size(103, 20);
			this.StartTimePicker.TabIndex = 1;
			this.StartTimePicker.ValueChanged += new System.EventHandler(this.StartTimePicker_ValueChanged);
			// 
			// StartDatePicker
			// 
			this.StartDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.StartDatePicker.Location = new System.Drawing.Point(6, 19);
			this.StartDatePicker.Name = "StartDatePicker";
			this.StartDatePicker.Size = new System.Drawing.Size(123, 20);
			this.StartDatePicker.TabIndex = 0;
			this.StartDatePicker.ValueChanged += new System.EventHandler(this.StartDatePicker_ValueChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.EndTimeCheckbox);
			this.groupBox2.Controls.Add(this.EndTimePicker);
			this.groupBox2.Controls.Add(this.EndDatePicker);
			this.groupBox2.Location = new System.Drawing.Point(3, 58);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(468, 70);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Expire Time";
			// 
			// EndTimeCheckbox
			// 
			this.EndTimeCheckbox.AutoSize = true;
			this.EndTimeCheckbox.Location = new System.Drawing.Point(6, 19);
			this.EndTimeCheckbox.Name = "EndTimeCheckbox";
			this.EndTimeCheckbox.Size = new System.Drawing.Size(65, 17);
			this.EndTimeCheckbox.TabIndex = 2;
			this.EndTimeCheckbox.Text = "Enabled";
			this.EndTimeCheckbox.UseVisualStyleBackColor = true;
			this.EndTimeCheckbox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// EndTimePicker
			// 
			this.EndTimePicker.Enabled = false;
			this.EndTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.EndTimePicker.Location = new System.Drawing.Point(135, 42);
			this.EndTimePicker.Name = "EndTimePicker";
			this.EndTimePicker.Size = new System.Drawing.Size(103, 20);
			this.EndTimePicker.TabIndex = 1;
			this.EndTimePicker.ValueChanged += new System.EventHandler(this.EndTimePicker_ValueChanged);
			// 
			// EndDatePicker
			// 
			this.EndDatePicker.Enabled = false;
			this.EndDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.EndDatePicker.Location = new System.Drawing.Point(6, 42);
			this.EndDatePicker.Name = "EndDatePicker";
			this.EndDatePicker.Size = new System.Drawing.Size(123, 20);
			this.EndDatePicker.TabIndex = 0;
			this.EndDatePicker.ValueChanged += new System.EventHandler(this.EndDatePicker_ValueChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.RecurenceUpDown);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Location = new System.Drawing.Point(3, 134);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(468, 52);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Recurence";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(148, 23);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(29, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "days";
			// 
			// RecurenceUpDown
			// 
			this.RecurenceUpDown.Location = new System.Drawing.Point(77, 21);
			this.RecurenceUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.RecurenceUpDown.Name = "RecurenceUpDown";
			this.RecurenceUpDown.Size = new System.Drawing.Size(65, 20);
			this.RecurenceUpDown.TabIndex = 4;
			this.RecurenceUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.RecurenceUpDown.ValueChanged += new System.EventHandler(this.RecurenceUpDown_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Recur every";
			// 
			// DailyScheduleEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "DailyScheduleEditor";
			this.Size = new System.Drawing.Size(474, 210);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.RecurenceUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DateTimePicker StartDatePicker;
		private System.Windows.Forms.DateTimePicker StartTimePicker;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.DateTimePicker EndTimePicker;
		private System.Windows.Forms.DateTimePicker EndDatePicker;
		private System.Windows.Forms.CheckBox EndTimeCheckbox;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown RecurenceUpDown;
	}
}
