namespace SecureDeleteWinForms
{
	partial class PanelSelectControl
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
			this.MainPicture = new System.Windows.Forms.PictureBox();
			this.RightPicture = new System.Windows.Forms.PictureBox();
			this.LeftPicture = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.MainPicture)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.RightPicture)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LeftPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// MainPicture
			// 
			this.MainPicture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.MainPicture.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPicture.Image = global::SecureDeleteWinForms.SDResources.moduleSelector;
			this.MainPicture.Location = new System.Drawing.Point(2, 0);
			this.MainPicture.Name = "MainPicture";
			this.MainPicture.Size = new System.Drawing.Size(709, 163);
			this.MainPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.MainPicture.TabIndex = 2;
			this.MainPicture.TabStop = false;
			this.MainPicture.Click += new System.EventHandler(this.MainPicture_Click);
			// 
			// RightPicture
			// 
			this.RightPicture.Dock = System.Windows.Forms.DockStyle.Right;
			this.RightPicture.Image = global::SecureDeleteWinForms.SDResources.moduleSelectorRight;
			this.RightPicture.Location = new System.Drawing.Point(711, 0);
			this.RightPicture.Name = "RightPicture";
			this.RightPicture.Size = new System.Drawing.Size(2, 163);
			this.RightPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.RightPicture.TabIndex = 1;
			this.RightPicture.TabStop = false;
			this.RightPicture.Click += new System.EventHandler(this.RightPicture_Click);
			// 
			// LeftPicture
			// 
			this.LeftPicture.Dock = System.Windows.Forms.DockStyle.Left;
			this.LeftPicture.Image = global::SecureDeleteWinForms.SDResources.moduleSelectorLeft;
			this.LeftPicture.Location = new System.Drawing.Point(0, 0);
			this.LeftPicture.Name = "LeftPicture";
			this.LeftPicture.Size = new System.Drawing.Size(2, 163);
			this.LeftPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.LeftPicture.TabIndex = 0;
			this.LeftPicture.TabStop = false;
			this.LeftPicture.Click += new System.EventHandler(this.LeftPicture_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(157)))));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(5, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "label1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// PanelSelectControl
			// 
			this.Controls.Add(this.label1);
			this.Controls.Add(this.MainPicture);
			this.Controls.Add(this.RightPicture);
			this.Controls.Add(this.LeftPicture);
			this.Name = "PanelSelectControl";
			this.Size = new System.Drawing.Size(713, 163);
			this.Load += new System.EventHandler(this.ModuleSelectControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.MainPicture)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.RightPicture)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LeftPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox LeftPicture;
		private System.Windows.Forms.PictureBox RightPicture;
		private System.Windows.Forms.PictureBox MainPicture;
		private System.Windows.Forms.Label label1;
	}
}
