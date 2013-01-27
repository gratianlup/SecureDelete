namespace SecureDeleteWinForms
{
	partial class ActionEditorWindow
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
			this.panel3 = new System.Windows.Forms.Panel();
			this.CancelButton = new System.Windows.Forms.Button();
			this.InsertButton = new System.Windows.Forms.Button();
			this.ActionEditor = new SecureDeleteWinForms.ActionEditor();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(82)))), ((int)(((byte)(111)))));
			this.panel3.Controls.Add(this.CancelButton);
			this.panel3.Controls.Add(this.InsertButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 535);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(797, 30);
			this.panel3.TabIndex = 13;
			// 
			// CancelButton
			// 
			this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelButton.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelButton.Location = new System.Drawing.Point(718, 3);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(76, 24);
			this.CancelButton.TabIndex = 4;
			this.CancelButton.Text = "Close";
			this.CancelButton.UseVisualStyleBackColor = false;
			// 
			// InsertButton
			// 
			this.InsertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.InsertButton.BackColor = System.Drawing.SystemColors.Control;
			this.InsertButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.InsertButton.Image = global::SecureDeleteWinForms.Properties.Resources.save;
			this.InsertButton.Location = new System.Drawing.Point(640, 3);
			this.InsertButton.Name = "InsertButton";
			this.InsertButton.Size = new System.Drawing.Size(76, 24);
			this.InsertButton.TabIndex = 3;
			this.InsertButton.Text = "Save";
			this.InsertButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.InsertButton.UseVisualStyleBackColor = true;
			// 
			// ActionEditor
			// 
			this.ActionEditor.Actions = null;
			this.ActionEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ActionEditor.Location = new System.Drawing.Point(0, 0);
			this.ActionEditor.Name = "ActionEditor";
			this.ActionEditor.Options = null;
			this.ActionEditor.Size = new System.Drawing.Size(797, 535);
			this.ActionEditor.TabIndex = 0;
			// 
			// ActionEditorWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(797, 565);
			this.Controls.Add(this.ActionEditor);
			this.Controls.Add(this.panel3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ActionEditorWindow";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Action Editor";
			this.Load += new System.EventHandler(this.ActionEditorWindow_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ActionEditorWindow_FormClosing);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private ActionEditor ActionEditor;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Button InsertButton;
	}
}