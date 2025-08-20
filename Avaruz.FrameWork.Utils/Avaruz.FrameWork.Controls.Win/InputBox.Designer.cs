// ******************************************************************
//
//	If this code works it was written by:
//		Malcolm
//		MamSoft / Manniff Computers
//		© 2008 - 2008...
//
//	if not, I have no idea who wrote it.
//
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avaruz.FrameWork.Controls.Win
{
	public partial class InputBox
	{

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
            btnOK = new AvaruzButton();
            btnCancel = new AvaruzButton();
            txtInput = new System.Windows.Forms.TextBox();
            lblPrompt = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.BackColor = System.Drawing.Color.MediumSlateBlue;
            btnOK.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            btnOK.BorderColor = System.Drawing.Color.PaleVioletRed;
            btnOK.BorderRadius = 10;
            btnOK.BorderSize = 0;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOK.ForeColor = System.Drawing.Color.White;
            btnOK.Location = new System.Drawing.Point(147, 67);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(75, 30);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.TextColor = System.Drawing.Color.White;
            btnOK.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.BackColor = System.Drawing.Color.MediumSlateBlue;
            btnCancel.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            btnCancel.BorderColor = System.Drawing.Color.PaleVioletRed;
            btnCancel.BorderRadius = 10;
            btnCancel.BorderSize = 0;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancel.ForeColor = System.Drawing.Color.White;
            btnCancel.Location = new System.Drawing.Point(228, 67);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 30);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.TextColor = System.Drawing.Color.White;
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // txtInput
            // 
            txtInput.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtInput.Location = new System.Drawing.Point(10, 27);
            txtInput.Name = "txtInput";
            txtInput.Size = new System.Drawing.Size(288, 20);
            txtInput.TabIndex = 0;
            // 
            // lblPrompt
            // 
            lblPrompt.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblPrompt.Location = new System.Drawing.Point(10, 10);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new System.Drawing.Size(62, 14);
            lblPrompt.TabIndex = 2;
            lblPrompt.Text = "Enter data";
            // 
            // InputBox
            // 
            AcceptButton = btnOK;
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(310, 109);
            ControlBox = false;
            Controls.Add(lblPrompt);
            Controls.Add(txtInput);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InputBox";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "InputBox";
            Load += InputBox_Load;
            ResumeLayout(false);
            PerformLayout();

        }
        #endregion

        public System.Windows.Forms.TextBox txtInput;
		private AvaruzButton btnOK;
		private System.ComponentModel.IContainer components=null;
		private System.Windows.Forms.Label lblPrompt;
        private AvaruzButton btnCancel;
    }
}
