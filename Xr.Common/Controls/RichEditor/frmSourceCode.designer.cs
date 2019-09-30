using System.Windows.Forms;
namespace Xr.Common.Controls
{
    partial class frmSourceCode
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
            this.txt_SourceCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt_SourceCode
            // 
            this.txt_SourceCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_SourceCode.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_SourceCode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_SourceCode.Location = new System.Drawing.Point(0, 0);
            this.txt_SourceCode.Multiline = true;
            this.txt_SourceCode.Name = "txt_SourceCode";
            this.txt_SourceCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_SourceCode.Size = new System.Drawing.Size(692, 425);
            this.txt_SourceCode.TabIndex = 0;
            // 
            // frmSourceCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 425);
            this.Controls.Add(this.txt_SourceCode);
            this.Location = new System.Drawing.Point(6, 392);
            this.Name = "frmSourceCode";
            this.Text = "源码预览";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txt_SourceCode;

    }
}