namespace Xr.RtManager
{
    partial class UpdateNoticeForm
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
            this.meUpdateDesc = new DevExpress.XtraEditors.MemoEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSeeMore = new Xr.Common.Controls.ButtonControl();
            this.btnClose = new Xr.Common.Controls.ButtonControl();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.meUpdateDesc.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // meUpdateDesc
            // 
            this.meUpdateDesc.EditValue = "";
            this.meUpdateDesc.Location = new System.Drawing.Point(20, 85);
            this.meUpdateDesc.Name = "meUpdateDesc";
            this.meUpdateDesc.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.meUpdateDesc.Properties.Appearance.Options.UseFont = true;
            this.meUpdateDesc.Properties.ReadOnly = true;
            this.meUpdateDesc.Size = new System.Drawing.Size(584, 349);
            this.meUpdateDesc.TabIndex = 95;
            this.meUpdateDesc.UseOptimizedRendering = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(18, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 96;
            this.label1.Text = "更新时间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(95, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 20);
            this.label2.TabIndex = 97;
            this.label2.Text = "2019-01-01 11:11:11";
            // 
            // btnSeeMore
            // 
            this.btnSeeMore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnSeeMore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnSeeMore.HoverBackColor = System.Drawing.Color.Empty;
            this.btnSeeMore.Location = new System.Drawing.Point(439, 445);
            this.btnSeeMore.Name = "btnSeeMore";
            this.btnSeeMore.SecondText = "";
            this.btnSeeMore.Size = new System.Drawing.Size(75, 30);
            this.btnSeeMore.Style = Xr.Common.Controls.ButtonStyle.Save;
            this.btnSeeMore.TabIndex = 98;
            this.btnSeeMore.Text = "查看更多";
            this.btnSeeMore.Click += new System.EventHandler(this.btnSeeMore_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnClose.HoverBackColor = System.Drawing.Color.Empty;
            this.btnClose.Location = new System.Drawing.Point(525, 445);
            this.btnClose.Name = "btnClose";
            this.btnClose.SecondText = "";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.Style = Xr.Common.Controls.ButtonStyle.Return;
            this.btnClose.TabIndex = 99;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(18, 450);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(359, 20);
            this.label3.TabIndex = 100;
            this.label3.Text = "在系统设置的更新公告模块可以查看所有版本的更新说明";
            // 
            // UpdateNoticeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 489);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSeeMore);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.meUpdateDesc);
            this.Name = "UpdateNoticeForm";
            this.Text = "更新公告V2.5.0";
            this.Load += new System.EventHandler(this.UpdateNoticeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.meUpdateDesc.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit meUpdateDesc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Xr.Common.Controls.ButtonControl btnSeeMore;
        private Xr.Common.Controls.ButtonControl btnClose;
        private System.Windows.Forms.Label label3;
    }
}