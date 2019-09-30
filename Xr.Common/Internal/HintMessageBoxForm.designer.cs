namespace Xr.Common.Internal
{
    partial class HintMessageBoxForm
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
            this.timer1 = new System.Windows.Forms.Timer();
            this.borderPanel1 = new Xr.Common.Controls.BorderPanel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.borderPanel1)).BeginInit();
            this.borderPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // borderPanel1
            // 
            this.borderPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(180)))), ((int)(((byte)(192)))));
            this.borderPanel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.borderPanel1.Controls.Add(this.lblMessage);
            this.borderPanel1.Controls.Add(this.pbIcon);
            this.borderPanel1.CornerRadius.All = 5;
            this.borderPanel1.CornerRadius.BottomLeft = 5;
            this.borderPanel1.CornerRadius.BottomRight = 5;
            this.borderPanel1.CornerRadius.TopLeft = 5;
            this.borderPanel1.CornerRadius.TopRight = 5;
            this.borderPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel1.FillColor1 = System.Drawing.Color.White;
            this.borderPanel1.FillColor2 = System.Drawing.Color.White;
            this.borderPanel1.Location = new System.Drawing.Point(1, 1);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.borderPanel1.Size = new System.Drawing.Size(297, 55);
            this.borderPanel1.TabIndex = 0;
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.White;
            this.lblMessage.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
            this.lblMessage.Location = new System.Drawing.Point(63, 8);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(220, 39);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "label1";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMessage.TextChanged += new System.EventHandler(this.lblMessage_TextChanged);
            // 
            // pbIcon
            // 
            this.pbIcon.BackColor = System.Drawing.Color.Transparent;
            this.pbIcon.Image = global::Xr.Common.Properties.Resources.HintMessageSuccess;
            this.pbIcon.Location = new System.Drawing.Point(16, 11);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(32, 32);
            this.pbIcon.TabIndex = 0;
            this.pbIcon.TabStop = false;
            // 
            // HintMessageBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.ClientSize = new System.Drawing.Size(299, 58);
            this.Controls.Add(this.borderPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HintMessageBoxForm";
            this.Padding = new System.Windows.Forms.Padding(1, 1, 1, 2);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HintMessageBoxForm";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.HintMessageBoxForm_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HintMessageBoxForm_FormClosed);
            this.Shown += new System.EventHandler(this.HintMessageBoxForm_Shown);
            this.Resize += new System.EventHandler(this.HintMessageBoxForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.borderPanel1)).EndInit();
            this.borderPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Xr.Common.Controls.BorderPanel borderPanel1;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.PictureBox pbIcon;
    }
}