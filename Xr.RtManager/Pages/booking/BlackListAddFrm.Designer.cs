namespace Xr.RtManager
{
    partial class BlackListAddFrm
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
            this.btnSave = new Xr.Common.Controls.ButtonControl();
            this.btnCancel = new Xr.Common.Controls.ButtonControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel15 = new System.Windows.Forms.Panel();
            this.rbt_sexF = new System.Windows.Forms.RadioButton();
            this.panel16 = new System.Windows.Forms.Panel();
            this.rbt_sexM = new System.Windows.Forms.RadioButton();
            this.txt_breakTimes = new System.Windows.Forms.TextBox();
            this.txt_phone = new System.Windows.Forms.TextBox();
            this.txt_patientId = new System.Windows.Forms.TextBox();
            this.panel19 = new System.Windows.Forms.Panel();
            this.rbt_isUseF = new System.Windows.Forms.RadioButton();
            this.panel20 = new System.Windows.Forms.Panel();
            this.rbt_isUseT = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_patientName = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel15.SuspendLayout();
            this.panel19.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnSave.HoverBackColor = System.Drawing.Color.Empty;
            this.btnSave.Location = new System.Drawing.Point(240, 158);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.Style = Xr.Common.Controls.ButtonStyle.Save;
            this.btnSave.TabIndex = 98;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnCancel.HoverBackColor = System.Drawing.Color.Empty;
            this.btnCancel.Location = new System.Drawing.Point(321, 158);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.Style = Xr.Common.Controls.ButtonStyle.Return;
            this.btnCancel.TabIndex = 99;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel15, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txt_breakTimes, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.txt_phone, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txt_patientId, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel19, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.label10, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label19, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label15, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txt_patientName, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(393, 140);
            this.tableLayoutPanel1.TabIndex = 100;
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.rbt_sexF);
            this.panel15.Controls.Add(this.panel16);
            this.panel15.Controls.Add(this.rbt_sexM);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel15.Location = new System.Drawing.Point(63, 73);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(125, 29);
            this.panel15.TabIndex = 192;
            // 
            // rbt_sexF
            // 
            this.rbt_sexF.AutoSize = true;
            this.rbt_sexF.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbt_sexF.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.rbt_sexF.Location = new System.Drawing.Point(59, 0);
            this.rbt_sexF.Name = "rbt_sexF";
            this.rbt_sexF.Size = new System.Drawing.Size(41, 29);
            this.rbt_sexF.TabIndex = 1;
            this.rbt_sexF.Text = "女";
            this.rbt_sexF.UseVisualStyleBackColor = true;
            // 
            // panel16
            // 
            this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel16.Location = new System.Drawing.Point(41, 0);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(18, 29);
            this.panel16.TabIndex = 2;
            // 
            // rbt_sexM
            // 
            this.rbt_sexM.AutoSize = true;
            this.rbt_sexM.Checked = true;
            this.rbt_sexM.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbt_sexM.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.rbt_sexM.Location = new System.Drawing.Point(0, 0);
            this.rbt_sexM.Name = "rbt_sexM";
            this.rbt_sexM.Size = new System.Drawing.Size(41, 29);
            this.rbt_sexM.TabIndex = 0;
            this.rbt_sexM.TabStop = true;
            this.rbt_sexM.Text = "男";
            this.rbt_sexM.UseVisualStyleBackColor = true;
            // 
            // txt_breakTimes
            // 
            this.txt_breakTimes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_breakTimes.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txt_breakTimes.Location = new System.Drawing.Point(264, 40);
            this.txt_breakTimes.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_breakTimes.Multiline = true;
            this.txt_breakTimes.Name = "txt_breakTimes";
            this.txt_breakTimes.Size = new System.Drawing.Size(126, 27);
            this.txt_breakTimes.TabIndex = 191;
            // 
            // txt_phone
            // 
            this.txt_phone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_phone.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txt_phone.Location = new System.Drawing.Point(63, 40);
            this.txt_phone.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_phone.Multiline = true;
            this.txt_phone.Name = "txt_phone";
            this.txt_phone.Size = new System.Drawing.Size(125, 27);
            this.txt_phone.TabIndex = 190;
            // 
            // txt_patientId
            // 
            this.txt_patientId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_patientId.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txt_patientId.Location = new System.Drawing.Point(63, 5);
            this.txt_patientId.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_patientId.Multiline = true;
            this.txt_patientId.Name = "txt_patientId";
            this.txt_patientId.Size = new System.Drawing.Size(125, 27);
            this.txt_patientId.TabIndex = 188;
            // 
            // panel19
            // 
            this.panel19.Controls.Add(this.rbt_isUseF);
            this.panel19.Controls.Add(this.panel20);
            this.panel19.Controls.Add(this.rbt_isUseT);
            this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel19.Location = new System.Drawing.Point(264, 73);
            this.panel19.Name = "panel19";
            this.panel19.Size = new System.Drawing.Size(126, 29);
            this.panel19.TabIndex = 186;
            // 
            // rbt_isUseF
            // 
            this.rbt_isUseF.AutoSize = true;
            this.rbt_isUseF.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbt_isUseF.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.rbt_isUseF.Location = new System.Drawing.Point(71, 0);
            this.rbt_isUseF.Name = "rbt_isUseF";
            this.rbt_isUseF.Size = new System.Drawing.Size(41, 29);
            this.rbt_isUseF.TabIndex = 1;
            this.rbt_isUseF.Text = "否";
            this.rbt_isUseF.UseVisualStyleBackColor = true;
            // 
            // panel20
            // 
            this.panel20.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel20.Location = new System.Drawing.Point(41, 0);
            this.panel20.Name = "panel20";
            this.panel20.Size = new System.Drawing.Size(30, 29);
            this.panel20.TabIndex = 2;
            // 
            // rbt_isUseT
            // 
            this.rbt_isUseT.AutoSize = true;
            this.rbt_isUseT.Checked = true;
            this.rbt_isUseT.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbt_isUseT.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.rbt_isUseT.Location = new System.Drawing.Point(0, 0);
            this.rbt_isUseT.Name = "rbt_isUseT";
            this.rbt_isUseT.Size = new System.Drawing.Size(41, 29);
            this.rbt_isUseT.TabIndex = 0;
            this.rbt_isUseT.TabStop = true;
            this.rbt_isUseT.Text = "是";
            this.rbt_isUseT.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Right;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(196, 73);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 29);
            this.label10.TabIndex = 106;
            this.label10.Text = "是否启用";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(5, 73);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 29);
            this.label4.TabIndex = 102;
            this.label4.Text = "性别";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.Dock = System.Windows.Forms.DockStyle.Right;
            this.label19.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(5, 38);
            this.label19.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(55, 29);
            this.label19.TabIndex = 99;
            this.label19.Text = "电话";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.Dock = System.Windows.Forms.DockStyle.Right;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(196, 38);
            this.label15.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 29);
            this.label15.TabIndex = 95;
            this.label15.Text = "爽约次数";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Right;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(196, 3);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 29);
            this.label7.TabIndex = 87;
            this.label7.Text = "患者姓名";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(5, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 29);
            this.label1.TabIndex = 85;
            this.label1.Text = "患者ID";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_patientName
            // 
            this.txt_patientName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_patientName.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txt_patientName.Location = new System.Drawing.Point(264, 5);
            this.txt_patientName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txt_patientName.Multiline = true;
            this.txt_patientName.Name = "txt_patientName";
            this.txt_patientName.Size = new System.Drawing.Size(126, 27);
            this.txt_patientName.TabIndex = 187;
            // 
            // BlackListAddFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(414, 200);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BlackListAddFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加黑名单";
            this.Load += new System.EventHandler(this.UserEdit_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.panel19.ResumeLayout(false);
            this.panel19.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Xr.Common.Controls.ButtonControl btnSave;
        private Xr.Common.Controls.ButtonControl btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.RadioButton rbt_sexF;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.RadioButton rbt_sexM;
        private System.Windows.Forms.TextBox txt_breakTimes;
        private System.Windows.Forms.TextBox txt_phone;
        private System.Windows.Forms.TextBox txt_patientId;
        private System.Windows.Forms.Panel panel19;
        private System.Windows.Forms.RadioButton rbt_isUseF;
        private System.Windows.Forms.Panel panel20;
        private System.Windows.Forms.RadioButton rbt_isUseT;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_patientName;
    }
}
