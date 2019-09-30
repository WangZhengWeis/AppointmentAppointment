namespace Xr.Common.Controls
{
    partial class PageControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelControl1 = new System.Windows.Forms.Label();
            this.teJumpPage = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.tePageSize = new DevExpress.XtraEditors.TextEdit();
            this.btnJump = new Xr.Common.Controls.ButtonControl();
            this.btnLast = new Xr.Common.Controls.ButtonControl();
            this.btnNext = new Xr.Common.Controls.ButtonControl();
            this.btnPre = new Xr.Common.Controls.ButtonControl();
            this.btnFirst = new Xr.Common.Controls.ButtonControl();
            ((System.ComponentModel.ISupportInitialize)(this.teJumpPage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePageSize.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.AutoSize = true;
            this.labelControl1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl1.Location = new System.Drawing.Point(433, 7);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(103, 20);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "共1页，共10条";
            // 
            // teJumpPage
            // 
            this.teJumpPage.EditValue = "1";
            this.teJumpPage.Location = new System.Drawing.Point(280, 5);
            this.teJumpPage.Name = "teJumpPage";
            this.teJumpPage.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teJumpPage.Properties.Appearance.Options.UseFont = true;
            this.teJumpPage.Properties.Appearance.Options.UseTextOptions = true;
            this.teJumpPage.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.teJumpPage.Properties.AutoHeight = false;
            this.teJumpPage.Properties.Mask.EditMask = "[0-9]*";
            this.teJumpPage.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.teJumpPage.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.teJumpPage.Size = new System.Drawing.Size(30, 25);
            this.teJumpPage.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(312, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "/";
            // 
            // tePageSize
            // 
            this.tePageSize.EditValue = "10";
            this.tePageSize.Location = new System.Drawing.Point(330, 5);
            this.tePageSize.Name = "tePageSize";
            this.tePageSize.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tePageSize.Properties.Appearance.Options.UseFont = true;
            this.tePageSize.Properties.Appearance.Options.UseTextOptions = true;
            this.tePageSize.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tePageSize.Properties.AutoHeight = false;
            this.tePageSize.Properties.Mask.EditMask = "[0-9]*";
            this.tePageSize.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.tePageSize.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.tePageSize.Size = new System.Drawing.Size(31, 25);
            this.tePageSize.TabIndex = 16;
            // 
            // btnJump
            // 
            this.btnJump.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnJump.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnJump.HoverBackColor = System.Drawing.Color.Empty;
            this.btnJump.Location = new System.Drawing.Point(370, 5);
            this.btnJump.Name = "btnJump";
            this.btnJump.Size = new System.Drawing.Size(60, 25);
            this.btnJump.Style = Xr.Common.Controls.ButtonStyle.Query;
            this.btnJump.TabIndex = 11;
            this.btnJump.Text = "跳转";
            this.btnJump.Click += new System.EventHandler(this.btnJump_Click);
            // 
            // btnLast
            // 
            this.btnLast.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnLast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnLast.HoverBackColor = System.Drawing.Color.Empty;
            this.btnLast.Location = new System.Drawing.Point(210, 5);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(60, 25);
            this.btnLast.Style = Xr.Common.Controls.ButtonStyle.Query;
            this.btnLast.TabIndex = 10;
            this.btnLast.Text = "末页";
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnNext.HoverBackColor = System.Drawing.Color.Empty;
            this.btnNext.Location = new System.Drawing.Point(140, 5);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(60, 25);
            this.btnNext.Style = Xr.Common.Controls.ButtonStyle.Query;
            this.btnNext.TabIndex = 9;
            this.btnNext.Text = "下一页";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPre
            // 
            this.btnPre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnPre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnPre.HoverBackColor = System.Drawing.Color.Empty;
            this.btnPre.Location = new System.Drawing.Point(70, 5);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(60, 25);
            this.btnPre.Style = Xr.Common.Controls.ButtonStyle.Query;
            this.btnPre.TabIndex = 8;
            this.btnPre.Text = "上一页";
            this.btnPre.Click += new System.EventHandler(this.btnPre_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnFirst.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnFirst.HoverBackColor = System.Drawing.Color.Empty;
            this.btnFirst.Location = new System.Drawing.Point(0, 5);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(60, 25);
            this.btnFirst.Style = Xr.Common.Controls.ButtonStyle.Query;
            this.btnFirst.TabIndex = 7;
            this.btnFirst.Text = "首页";
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // PageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tePageSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.teJumpPage);
            this.Controls.Add(this.btnJump);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPre);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.labelControl1);
            this.Name = "PageControl";
            this.Size = new System.Drawing.Size(842, 35);
            ((System.ComponentModel.ISupportInitialize)(this.teJumpPage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePageSize.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelControl1;
        private ButtonControl btnFirst;
        private ButtonControl btnPre;
        private ButtonControl btnNext;
        private ButtonControl btnLast;
        private ButtonControl btnJump;
        private DevExpress.XtraEditors.TextEdit teJumpPage;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit tePageSize;
    }
}
