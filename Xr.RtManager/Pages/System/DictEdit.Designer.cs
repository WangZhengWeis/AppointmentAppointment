namespace Xr.RtManager
{
    partial class DictEdit
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
            this.teValue = new DevExpress.XtraEditors.TextEdit();
            this.teLabel = new DevExpress.XtraEditors.TextEdit();
            this.textType = new DevExpress.XtraEditors.TextEdit();
            this.textDescription = new DevExpress.XtraEditors.TextEdit();
            this.textSort = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dcDict = new Xr.Common.Controls.DataController(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonControl1 = new Xr.Common.Controls.ButtonControl();
            this.buttonControl2 = new Xr.Common.Controls.ButtonControl();
            ((System.ComponentModel.ISupportInitialize)(this.teValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teLabel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textSort.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // teValue
            // 
            this.dcDict.SetDataMember(this.teValue, "value");
            this.teValue.Location = new System.Drawing.Point(99, 25);
            this.teValue.Name = "teValue";
            this.teValue.Properties.AutoHeight = false;
            this.teValue.Size = new System.Drawing.Size(300, 28);
            this.teValue.TabIndex = 1;
            // 
            // teLabel
            // 
            this.dcDict.SetDataMember(this.teLabel, "label");
            this.teLabel.Location = new System.Drawing.Point(99, 65);
            this.teLabel.Name = "teLabel";
            this.teLabel.Properties.AutoHeight = false;
            this.teLabel.Size = new System.Drawing.Size(300, 28);
            this.teLabel.TabIndex = 3;
            // 
            // textType
            // 
            this.dcDict.SetDataMember(this.textType, "type");
            this.textType.Location = new System.Drawing.Point(99, 105);
            this.textType.Name = "textType";
            this.textType.Properties.AutoHeight = false;
            this.textType.Size = new System.Drawing.Size(300, 28);
            this.textType.TabIndex = 5;
            // 
            // textDescription
            // 
            this.dcDict.SetDataMember(this.textDescription, "description");
            this.textDescription.Location = new System.Drawing.Point(99, 145);
            this.textDescription.Name = "textDescription";
            this.textDescription.Properties.AutoHeight = false;
            this.textDescription.Size = new System.Drawing.Size(300, 28);
            this.textDescription.TabIndex = 7;
            // 
            // textSort
            // 
            this.dcDict.SetDataMember(this.textSort, "sort");
            this.textSort.Location = new System.Drawing.Point(99, 185);
            this.textSort.Name = "textSort";
            this.textSort.Properties.AutoHeight = false;
            this.textSort.Size = new System.Drawing.Size(300, 28);
            this.textSort.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(39, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "排序：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(39, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "描述：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(39, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "键值：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(39, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "标签：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(39, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "类型：";
            // 
            // buttonControl1
            // 
            this.buttonControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.buttonControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.buttonControl1.HoverBackColor = System.Drawing.Color.Empty;
            this.buttonControl1.Location = new System.Drawing.Point(322, 246);
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
            this.buttonControl2.Location = new System.Drawing.Point(213, 246);
            this.buttonControl2.Name = "buttonControl2";
            this.buttonControl2.Size = new System.Drawing.Size(75, 30);
            this.buttonControl2.Style = Xr.Common.Controls.ButtonStyle.Save;
            this.buttonControl2.TabIndex = 16;
            this.buttonControl2.Text = "保存";
            this.buttonControl2.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // DictEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(461, 304);
            this.Controls.Add(this.buttonControl1);
            this.Controls.Add(this.buttonControl2);
            this.Controls.Add(this.textSort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textDescription);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.teLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.teValue);
            this.Controls.Add(this.label4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DictEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "字典添加";
            this.Load += new System.EventHandler(this.DictEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.teValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teLabel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textSort.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Xr.Common.Controls.DataController dcDict;
        private DevExpress.XtraEditors.TextEdit textSort;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit textDescription;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit textType;
        private DevExpress.XtraEditors.TextEdit teLabel;
        private DevExpress.XtraEditors.TextEdit teValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Xr.Common.Controls.ButtonControl buttonControl1;
        private Xr.Common.Controls.ButtonControl buttonControl2;
    }
}
