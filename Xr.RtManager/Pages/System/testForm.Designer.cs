namespace Xr.RtManager
{
    partial class TestForm
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
            this.menuTest1 = new Xr.Common.Controls.TreeMenuControl();
            this.SuspendLayout();
            // 
            // menuTest1
            // 
            this.menuTest1.AutoScroll = true;
            this.menuTest1.BorderColor = System.Drawing.Color.Black;
            this.menuTest1.BorderSize = 1;
            this.menuTest1.BorderStyleBottom = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.menuTest1.BorderStyleLeft = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.menuTest1.BorderStyleRight = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.menuTest1.BorderStyleTop = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.menuTest1.DataSource = null;
            this.menuTest1.DisplayMember = "name";
            this.menuTest1.EditValue = null;
            this.menuTest1.KeyFieldName = "id";
            this.menuTest1.Location = new System.Drawing.Point(425, 67);
            this.menuTest1.Name = "menuTest1";
            this.menuTest1.ParentFieldName = "parentId";
            this.menuTest1.Size = new System.Drawing.Size(262, 400);
            this.menuTest1.TabIndex = 0;
            this.menuTest1.UseZoom = false;
            this.menuTest1.ValueMember = "value";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.menuTest1);
            this.Name = "TestForm";
            this.Size = new System.Drawing.Size(1339, 481);
            this.Load += new System.EventHandler(this.LogForm_Load);
            this.Resize += new System.EventHandler(this.LogForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private Xr.Common.Controls.TreeMenuControl menuTest1;




    }
}
