namespace Xr.Common.Internal
{
    partial class MessageBoxForm
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
            this.pcButtons = new DevExpress.XtraEditors.PanelControl();
            this.pcContent = new DevExpress.XtraEditors.PanelControl();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.lcMessage = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pcButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcContent)).BeginInit();
            this.pcContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pcButtons
            // 
            this.pcButtons.Appearance.BackColor = System.Drawing.Color.White;
            this.pcButtons.Appearance.Options.UseBackColor = true;
            this.pcButtons.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pcButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pcButtons.Location = new System.Drawing.Point(2, 198);
            this.pcButtons.Name = "pcButtons";
            this.pcButtons.Size = new System.Drawing.Size(546, 56);
            this.pcButtons.TabIndex = 2;
            // 
            // pcContent
            // 
            this.pcContent.Appearance.BackColor = System.Drawing.Color.White;
            this.pcContent.Appearance.Options.UseBackColor = true;
            this.pcContent.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pcContent.Controls.Add(this.pbIcon);
            this.pcContent.Controls.Add(this.lcMessage);
            this.pcContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcContent.Location = new System.Drawing.Point(2, 32);
            this.pcContent.Name = "pcContent";
            this.pcContent.Size = new System.Drawing.Size(546, 166);
            this.pcContent.TabIndex = 5;
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new System.Drawing.Point(15, 15);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(46, 45);
            this.pbIcon.TabIndex = 2;
            this.pbIcon.TabStop = false;
            // 
            // lcMessage
            // 
            this.lcMessage.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lcMessage.Location = new System.Drawing.Point(84, 15);
            this.lcMessage.Name = "lcMessage";
            this.lcMessage.Size = new System.Drawing.Size(64, 19);
            this.lcMessage.TabIndex = 3;
            this.lcMessage.Text = "lcMessage";
            // 
            // MessageBoxForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(550, 257);
            this.ControlBox = false;
            this.Controls.Add(this.pcContent);
            this.Controls.Add(this.pcButtons);
            this.KeyPreview = true;
            this.Name = "MessageBoxForm";
            this.Tag = "bdw";
            this.Text = "MessageBoxForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MessageBoxForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pcButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcContent)).EndInit();
            this.pcContent.ResumeLayout(false);
            this.pcContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pcButtons;
        private DevExpress.XtraEditors.PanelControl pcContent;
        private System.Windows.Forms.PictureBox pbIcon;
        private DevExpress.XtraEditors.LabelControl lcMessage;

    }
}