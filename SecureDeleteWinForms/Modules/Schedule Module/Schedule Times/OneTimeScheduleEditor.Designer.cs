namespace SecureDeleteWinForms
{
	partial class OneTimeScheduleEditor
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
			this.groupBox1.SuspendLayout();
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
			this.groupBox1.Size = new System.Drawing.Size(496, 49);
			this.groupBox1.TabIndex = 7;
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
			// OneTimeScheduleEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "OneTimeScheduleEditor";
			this.Size = new System.Drawing.Size(502, 61);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DateTimePicker StartTimePicker;
		private System.Windows.Forms.DateTimePicker StartDatePicker;
	}
}
