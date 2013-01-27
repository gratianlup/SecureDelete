namespace SecureDeleteWinForms
{
	partial class StatusWindowElement
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusWindowElement));
			this.StopButton = new System.Windows.Forms.Button();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.ProgressBar = new System.Windows.Forms.ProgressBar();
			this.StatusLabelInternal = new System.Windows.Forms.Label();
			this.StatusIconInternal = new System.Windows.Forms.PictureBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.StatusIconInternal)).BeginInit();
			this.SuspendLayout();
			// 
			// StopButton
			// 
			this.StopButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.StopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.StopButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.StopButton.ImageKey = "safdgsd.ico";
			this.StopButton.ImageList = this.imageList1;
			this.StopButton.Location = new System.Drawing.Point(284, 1);
			this.StopButton.Name = "StopButton";
			this.StopButton.Size = new System.Drawing.Size(24, 22);
			this.StopButton.TabIndex = 8;
			this.StopButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.StopButton.UseVisualStyleBackColor = true;
			this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "safdgsd.ico");
			// 
			// ProgressBar
			// 
			this.ProgressBar.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.ProgressBar.Location = new System.Drawing.Point(137, 3);
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(145, 18);
			this.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.ProgressBar.TabIndex = 7;
			this.ProgressBar.Value = 33;
			// 
			// StatusLabelInternal
			// 
			this.StatusLabelInternal.AutoSize = true;
			this.StatusLabelInternal.Location = new System.Drawing.Point(23, 5);
			this.StatusLabelInternal.Name = "StatusLabelInternal";
			this.StatusLabelInternal.Size = new System.Drawing.Size(65, 13);
			this.StatusLabelInternal.TabIndex = 6;
			this.StatusLabelInternal.Text = "Wipe Status";
			// 
			// StatusIconInternal
			// 
			this.StatusIconInternal.Image = global::SecureDeleteWinForms.Properties.Resources.warning;
			this.StatusIconInternal.Location = new System.Drawing.Point(3, 3);
			this.StatusIconInternal.Name = "StatusIconInternal";
			this.StatusIconInternal.Size = new System.Drawing.Size(16, 16);
			this.StatusIconInternal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.StatusIconInternal.TabIndex = 5;
			this.StatusIconInternal.TabStop = false;
			// 
			// StatusWindowElement
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.StopButton);
			this.Controls.Add(this.ProgressBar);
			this.Controls.Add(this.StatusLabelInternal);
			this.Controls.Add(this.StatusIconInternal);
			this.Name = "StatusWindowElement";
			this.Size = new System.Drawing.Size(310, 23);
			((System.ComponentModel.ISupportInitialize)(this.StatusIconInternal)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ProgressBar ProgressBar;
		private System.Windows.Forms.Label StatusLabelInternal;
		private System.Windows.Forms.PictureBox StatusIconInternal;
		private System.Windows.Forms.Button StopButton;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}
