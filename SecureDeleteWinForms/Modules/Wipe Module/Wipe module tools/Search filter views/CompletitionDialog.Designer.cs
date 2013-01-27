namespace SecureDeleteWinForms.WipeTools
{
	partial class CompletitionDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompletitionDialog));
			this.SuggestionList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// SuggestionList
			// 
			this.SuggestionList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.SuggestionList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SuggestionList.FullRowSelect = true;
			this.SuggestionList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.SuggestionList.HideSelection = false;
			this.SuggestionList.Location = new System.Drawing.Point(0, 0);
			this.SuggestionList.MultiSelect = false;
			this.SuggestionList.Name = "SuggestionList";
			this.SuggestionList.Size = new System.Drawing.Size(222, 102);
			this.SuggestionList.SmallImageList = this.imageList1;
			this.SuggestionList.TabIndex = 0;
			this.SuggestionList.UseCompatibleStateImageBehavior = false;
			this.SuggestionList.View = System.Windows.Forms.View.Details;
			this.SuggestionList.DoubleClick += new System.EventHandler(this.SuggestionList_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Width = 180;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "variable.ico");
			this.imageList1.Images.SetKeyName(1, "logical.ico");
			// 
			// CompletitionDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(222, 102);
			this.Controls.Add(this.SuggestionList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CompletitionDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CompletitionDialog_KeyDown);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView SuggestionList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ImageList imageList1;
	}
}