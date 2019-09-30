namespace Xr.RtManager.Pages.scheduling
{
    partial class RemarksEdit
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
            this.label1 = new System.Windows.Forms.Label();
            this.buttonControl1 = new Xr.Common.Controls.ButtonControl();
            this.buttonControl2 = new Xr.Common.Controls.ButtonControl();
            this.dcClientVersion = new Xr.Common.Controls.DataController(this.components);
            this.meRemarks = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.meRemarks.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(64, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "备注：";
            // 
            // buttonControl1
            // 
            this.buttonControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.buttonControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.buttonControl1.HoverBackColor = System.Drawing.Color.Empty;
            this.buttonControl1.Location = new System.Drawing.Point(394, 162);
            this.buttonControl1.Name = "buttonControl1";
            this.buttonControl1.Size = new System.Drawing.Size(75, 30);
            this.buttonControl1.Style = Xr.Common.Controls.ButtonStyle.Return;
            this.buttonControl1.TabIndex = 17;
            this.buttonControl1.Text = "关闭";
            this.buttonControl1.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // buttonControl2
            // 
            this.buttonControl2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.buttonControl2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.buttonControl2.HoverBackColor = System.Drawing.Color.Empty;
            this.buttonControl2.Location = new System.Drawing.Point(285, 162);
            this.buttonControl2.Name = "buttonControl2";
            this.buttonControl2.Size = new System.Drawing.Size(75, 30);
            this.buttonControl2.Style = Xr.Common.Controls.ButtonStyle.Save;
            this.buttonControl2.TabIndex = 16;
            this.buttonControl2.Text = "保存";
            this.buttonControl2.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // meRemarks
            // 
            this.meRemarks.Location = new System.Drawing.Point(133, 39);
            this.meRemarks.Name = "meRemarks";
            this.meRemarks.Size = new System.Drawing.Size(337, 96);
            this.meRemarks.TabIndex = 18;
            this.meRemarks.UseOptimizedRendering = true;
            // 
            // RemarksEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(540, 218);
            this.Controls.Add(this.meRemarks);
            this.Controls.Add(this.buttonControl1);
            this.Controls.Add(this.buttonControl2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RemarksEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "批量修改备注";
            ((System.ComponentModel.ISupportInitialize)(this.meRemarks.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Xr.Common.Controls.DataController dcClientVersion;
        private System.Windows.Forms.Label label1;
        private Xr.Common.Controls.ButtonControl buttonControl1;
        private Xr.Common.Controls.ButtonControl buttonControl2;
        private DevExpress.XtraEditors.MemoEdit meRemarks;
    }
}
