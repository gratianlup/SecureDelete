namespace SecureDeleteMMC
{
	partial class HostControl
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
		public void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HostControl));

			if (Interface == null)
			{
				Interface = new SecureDeleteWinForms.MainForm();
				// 
				// Interface
				// 
				Interface.Dock = System.Windows.Forms.DockStyle.Fill;
				Interface.Location = new System.Drawing.Point(0, 0);
				Interface.Minimal = true;
				Interface.Name = "Interface";
				Interface.Size = new System.Drawing.Size(666, 484);
				Interface.TabIndex = 0;
			}

			// 
			// HostControl
			// 
			SuspendLayout();
			AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

			if (Interface.Parent != null)
			{
				Interface.Parent.Controls.Clear();
			}

			Controls.Add(Interface);

			Name = "HostControl";
			Size = new System.Drawing.Size(666, 484);
			ResumeLayout(false);
		}

		#endregion

		private static SecureDeleteWinForms.MainForm Interface;
	}
}
