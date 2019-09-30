namespace Xr.RtManager.Pages.cms
{
    partial class MessageSettingEdit
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
            this.components = new System.ComponentModel.Container();
            this.teStardTime = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.meUpdateDesc = new DevExpress.XtraEditors.MemoEdit();
            this.butClose = new Xr.Common.Controls.ButtonControl();
            this.butAdd = new Xr.Common.Controls.ButtonControl();
            this.dcMessage = new Xr.Common.Controls.DataController(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.teStardTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.meUpdateDesc.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // teStardTime
            // 
            this.dcMessage.SetDataMember(this.teStardTime, "type");
            this.teStardTime.Location = new System.Drawing.Point(135, 43);
            this.teStardTime.Name = "teStardTime";
            this.teStardTime.Properties.AutoHeight = false;
            this.teStardTime.Size = new System.Drawing.Size(300, 28);
            this.teStardTime.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(64, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 114;
            this.label6.Text = "模板内容：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(64, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 113;
            this.label3.Text = "模板类型：";
            // 
            // meUpdateDesc
            // 
            this.dcMessage.SetDataMember(this.meUpdateDesc, "content");
            this.meUpdateDesc.Location = new System.Drawing.Point(135, 86);
            this.meUpdateDesc.Name = "meUpdateDesc";
            this.meUpdateDesc.Size = new System.Drawing.Size(300, 102);
            this.meUpdateDesc.TabIndex = 4;
            this.meUpdateDesc.UseOptimizedRendering = true;
            // 
            // butClose
            // 
            this.butClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.butClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.butClose.HoverBackColor = System.Drawing.Color.Empty;
            this.butClose.Location = new System.Drawing.Point(336, 205);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(75, 30);
            this.butClose.Style = Xr.Common.Controls.ButtonStyle.Return;
            this.butClose.TabIndex = 6;
            this.butClose.Text = "关闭";
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // butAdd
            // 
            this.butAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.butAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.butAdd.HoverBackColor = System.Drawing.Color.Empty;
            this.butAdd.Location = new System.Drawing.Point(227, 205);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(75, 30);
            this.butAdd.Style = Xr.Common.Controls.ButtonStyle.Save;
            this.butAdd.TabIndex = 5;
            this.butAdd.Text = "保存";
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // MessageSettingEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(540, 247);
            this.Controls.Add(this.meUpdateDesc);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.butAdd);
            this.Controls.Add(this.teStardTime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageSettingEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新增消息模板";
            this.Load += new System.EventHandler(this.MessageSettingEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.teStardTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.meUpdateDesc.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Xr.Common.Controls.DataController dcMessage;
        private Xr.Common.Controls.ButtonControl butClose;
        private Xr.Common.Controls.ButtonControl butAdd;
        private DevExpress.XtraEditors.TextEdit teStardTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.MemoEdit meUpdateDesc;
    }
}