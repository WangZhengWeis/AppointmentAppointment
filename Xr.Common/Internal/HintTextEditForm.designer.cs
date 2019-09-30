namespace Xr.Common.Internal
{
    partial class HintTextEditForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.meMessage = new DevExpress.XtraEditors.MemoEdit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.meMessage.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.meMessage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(299, 58);
            this.panel1.TabIndex = 1;
            // 
            // meMessage
            // 
            this.meMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.meMessage.Location = new System.Drawing.Point(0, 0);
            this.meMessage.Name = "meMessage";
            this.meMessage.Properties.ReadOnly = true;
            this.meMessage.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.meMessage.Size = new System.Drawing.Size(299, 58);
            this.meMessage.TabIndex = 1;
            this.meMessage.UseOptimizedRendering = true;
            this.meMessage.TextChanged += new System.EventHandler(this.meMessage_TextChanged);
            // 
            // HintTextEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(299, 58);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HintTextEditForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "HintTextBoxForm";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.HintMessageBoxForm_Deactivate);
            this.Shown += new System.EventHandler(this.HintMessageBoxForm_Shown);
            this.Resize += new System.EventHandler(this.HintMessageBoxForm_Resize);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.meMessage.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.MemoEdit meMessage;
    }
}