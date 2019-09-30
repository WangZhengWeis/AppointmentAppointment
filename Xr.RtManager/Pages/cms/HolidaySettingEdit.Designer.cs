namespace Xr.RtManager.Pages.cms
{
    partial class HolidaySettingEdit
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
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tecode = new DevExpress.XtraEditors.TextEdit();
            this.tename = new DevExpress.XtraEditors.TextEdit();
            this.teStardTime = new DevExpress.XtraEditors.TextEdit();
            this.teEndTime = new DevExpress.XtraEditors.TextEdit();
            this.radioGroup2 = new DevExpress.XtraEditors.RadioGroup();
            this.label5 = new System.Windows.Forms.Label();
            this.butClose = new Xr.Common.Controls.ButtonControl();
            this.butAdd = new Xr.Common.Controls.ButtonControl();
            this.dcHolday = new Xr.Common.Controls.DataController(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tecode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tename.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teStardTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(55, 167);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 107;
            this.label6.Text = "结束日期：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(55, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 106;
            this.label3.Text = "开始日期：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(55, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 105;
            this.label2.Text = "所属年份：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(41, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 104;
            this.label1.Text = "节假日名称：";
            // 
            // tecode
            // 
            this.dcHolday.SetDataMember(this.tecode, "year");
            this.tecode.Location = new System.Drawing.Point(126, 79);
            this.tecode.Name = "tecode";
            this.tecode.Properties.AutoHeight = false;
            this.tecode.Size = new System.Drawing.Size(300, 28);
            this.tecode.TabIndex = 2;
            // 
            // tename
            // 
            this.dcHolday.SetDataMember(this.tename, "name");
            this.tename.Location = new System.Drawing.Point(126, 36);
            this.tename.Name = "tename";
            this.tename.Properties.AutoHeight = false;
            this.tename.Size = new System.Drawing.Size(300, 28);
            this.tename.TabIndex = 1;
            // 
            // teStardTime
            // 
            this.dcHolday.SetDataMember(this.teStardTime, "beginDate");
            this.teStardTime.Location = new System.Drawing.Point(126, 122);
            this.teStardTime.Name = "teStardTime";
            this.teStardTime.Properties.AutoHeight = false;
            this.teStardTime.Size = new System.Drawing.Size(300, 28);
            this.teStardTime.TabIndex = 3;
            // 
            // teEndTime
            // 
            this.dcHolday.SetDataMember(this.teEndTime, "endDate");
            this.teEndTime.Location = new System.Drawing.Point(126, 163);
            this.teEndTime.Name = "teEndTime";
            this.teEndTime.Properties.AutoHeight = false;
            this.teEndTime.Size = new System.Drawing.Size(300, 28);
            this.teEndTime.TabIndex = 4;
            // 
            // radioGroup2
            // 
            this.dcHolday.SetDataMember(this.radioGroup2, "isUse");
            this.radioGroup2.EditValue = "1";
            this.radioGroup2.Location = new System.Drawing.Point(143, 202);
            this.radioGroup2.Name = "radioGroup2";
            this.radioGroup2.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioGroup2.Properties.Appearance.Options.UseFont = true;
            this.radioGroup2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.radioGroup2.Properties.Columns = 2;
            this.radioGroup2.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "是"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("0", "否")});
            this.radioGroup2.Size = new System.Drawing.Size(234, 30);
            this.radioGroup2.TabIndex = 109;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(55, 206);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 20);
            this.label5.TabIndex = 108;
            this.label5.Text = "是否启用：";
            // 
            // butClose
            // 
            this.butClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.butClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.butClose.HoverBackColor = System.Drawing.Color.Empty;
            this.butClose.Location = new System.Drawing.Point(327, 265);
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
            this.butAdd.Location = new System.Drawing.Point(218, 265);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(75, 30);
            this.butAdd.Style = Xr.Common.Controls.ButtonStyle.Save;
            this.butAdd.TabIndex = 5;
            this.butAdd.Text = "保存";
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // HolidaySettingEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(540, 317);
            this.Controls.Add(this.radioGroup2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.butAdd);
            this.Controls.Add(this.teEndTime);
            this.Controls.Add(this.teStardTime);
            this.Controls.Add(this.tename);
            this.Controls.Add(this.tecode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HolidaySettingEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加节假日";
            this.Load += new System.EventHandler(this.HolidaySettingEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tecode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tename.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teStardTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup2.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit tecode;
        private DevExpress.XtraEditors.TextEdit tename;
        private DevExpress.XtraEditors.TextEdit teStardTime;
        private DevExpress.XtraEditors.TextEdit teEndTime;
        private Xr.Common.Controls.ButtonControl butClose;
        private Xr.Common.Controls.ButtonControl butAdd;
        private Xr.Common.Controls.DataController dcHolday;
        private DevExpress.XtraEditors.RadioGroup radioGroup2;
        private System.Windows.Forms.Label label5;
    }
}