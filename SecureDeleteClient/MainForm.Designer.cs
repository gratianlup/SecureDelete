namespace SecureDelete
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.Interface = new SecureDeleteWinForms.MainForm();
			this.SuspendLayout();
			// 
			// Interface
			// 
			this.Interface.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Interface.Location = new System.Drawing.Point(0, 0);
			this.Interface.Minimal = false;
			this.Interface.Name = "Interface";
			this.Interface.Options = ((SecureDelete.SDOptions)(resources.GetObject("Interface.Options")));
			this.Interface.Size = new System.Drawing.Size(825, 626);
			this.Interface.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(825, 626);
			this.Controls.Add(this.Interface);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "SecureDelete";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private SecureDeleteWinForms.MainForm Interface;
	}
}

