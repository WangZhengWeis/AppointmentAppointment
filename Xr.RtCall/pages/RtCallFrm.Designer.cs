namespace Xr.RtCall.pages
{
    partial class RtCallFrm
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
            this.components = new System.ComponentModel.Container();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.skinButNext = new CCWin.SkinControl.SkinButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.skinbutBig = new CCWin.SkinControl.SkinButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.skinbutBig);
            this.panelControl1.Controls.Add(this.skinButNext);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(615, 48);
            this.panelControl1.TabIndex = 0;
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 48);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(615, 452);
            this.panelControl2.TabIndex = 1;
            // 
            // skinButNext
            // 
            this.skinButNext.BackColor = System.Drawing.Color.Transparent;
            this.skinButNext.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(175)))), ((int)(((byte)(218)))));
            this.skinButNext.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(175)))), ((int)(((byte)(218)))));
            this.skinButNext.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButNext.DownBack = null;
            this.skinButNext.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButNext.ForeColor = System.Drawing.Color.White;
            this.skinButNext.IsDrawBorder = false;
            this.skinButNext.IsDrawGlass = false;
            this.skinButNext.IsEnabledDraw = false;
            this.skinButNext.Location = new System.Drawing.Point(11, 9);
            this.skinButNext.MouseBack = null;
            this.skinButNext.Name = "skinButNext";
            this.skinButNext.NormlBack = null;
            this.skinButNext.Radius = 4;
            this.skinButNext.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinButNext.Size = new System.Drawing.Size(75, 30);
            this.skinButNext.TabIndex = 0;
            this.skinButNext.Text = "下";
            this.skinButNext.UseVisualStyleBackColor = false;
            this.skinButNext.Click += new System.EventHandler(this.skinButNext_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(115, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "等待呼叫病人";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(295, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "[请稍候...]";
            // 
            // skinbutBig
            // 
            this.skinbutBig.BackColor = System.Drawing.Color.Transparent;
            this.skinbutBig.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(175)))), ((int)(((byte)(218)))));
            this.skinbutBig.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(175)))), ((int)(((byte)(218)))));
            this.skinbutBig.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinbutBig.DownBack = null;
            this.skinbutBig.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinbutBig.ForeColor = System.Drawing.Color.White;
            this.skinbutBig.IsDrawBorder = false;
            this.skinbutBig.IsDrawGlass = false;
            this.skinbutBig.IsEnabledDraw = false;
            this.skinbutBig.Location = new System.Drawing.Point(479, 8);
            this.skinbutBig.MouseBack = null;
            this.skinbutBig.Name = "skinbutBig";
            this.skinbutBig.NormlBack = null;
            this.skinbutBig.Radius = 4;
            this.skinbutBig.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinbutBig.Size = new System.Drawing.Size(75, 30);
            this.skinbutBig.TabIndex = 0;
            this.skinbutBig.Text = "最大化";
            this.skinbutBig.UseVisualStyleBackColor = false;
            this.skinbutBig.Click += new System.EventHandler(this.skinbutBig_Click);
            // 
            // RtCallFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "RtCallFrm";
            this.Size = new System.Drawing.Size(615, 500);
            //this.Resize += new System.EventHandler(this.RtCallFrm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private CCWin.SkinControl.SkinButton skinButNext;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private CCWin.SkinControl.SkinButton skinbutBig;

    }
}
