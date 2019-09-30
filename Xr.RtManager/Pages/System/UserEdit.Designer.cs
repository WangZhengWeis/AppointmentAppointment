namespace Xr.RtManager
{
    partial class UserEdit
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
            this.dcUser = new Xr.Common.Controls.DataController(this.components);
            this.teLoginName = new DevExpress.XtraEditors.TextEdit();
            this.tePassword = new DevExpress.XtraEditors.TextEdit();
            this.teName = new DevExpress.XtraEditors.TextEdit();
            this.textEdit6 = new DevExpress.XtraEditors.TextEdit();
            this.tePassword2 = new DevExpress.XtraEditors.TextEdit();
            this.teEmail = new DevExpress.XtraEditors.TextEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.radioGroup2 = new DevExpress.XtraEditors.RadioGroup();
            this.treeDept = new DevExpress.XtraEditors.TreeListLookUpEdit();
            this.treeList2 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn9 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn10 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.textEdit7 = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnUpload = new Xr.Common.Controls.ButtonControl();
            this.btnSelect = new Xr.Common.Controls.ButtonControl();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnSave = new Xr.Common.Controls.ButtonControl();
            this.btnCancel = new Xr.Common.Controls.ButtonControl();
            this.panelEx1 = new Xr.Common.Controls.PanelEx(this.components);
            this.richEditor1 = new Xr.Common.Controls.RichEditor();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.label15 = new System.Windows.Forms.Label();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListRole = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            ((System.ComponentModel.ISupportInitialize)(this.teLoginName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit6.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePassword2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeDept.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit7.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListRole)).BeginInit();
            this.SuspendLayout();
            // 
            // teLoginName
            // 
            this.dcUser.SetDataMember(this.teLoginName, "loginName");
            this.teLoginName.Location = new System.Drawing.Point(491, 71);
            this.teLoginName.Name = "teLoginName";
            this.teLoginName.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teLoginName.Properties.Appearance.Options.UseFont = true;
            this.teLoginName.Properties.AutoHeight = false;
            this.teLoginName.Size = new System.Drawing.Size(232, 29);
            this.teLoginName.TabIndex = 74;
            // 
            // tePassword
            // 
            this.dcUser.SetDataMember(this.tePassword, "password");
            this.tePassword.Location = new System.Drawing.Point(131, 116);
            this.tePassword.Name = "tePassword";
            this.tePassword.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tePassword.Properties.Appearance.Options.UseFont = true;
            this.tePassword.Properties.AutoHeight = false;
            this.tePassword.Properties.PasswordChar = '*';
            this.tePassword.Size = new System.Drawing.Size(232, 29);
            this.tePassword.TabIndex = 75;
            // 
            // teName
            // 
            this.dcUser.SetDataMember(this.teName, "name");
            this.teName.Location = new System.Drawing.Point(131, 161);
            this.teName.Name = "teName";
            this.teName.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teName.Properties.Appearance.Options.UseFont = true;
            this.teName.Properties.AutoHeight = false;
            this.teName.Size = new System.Drawing.Size(232, 29);
            this.teName.TabIndex = 76;
            // 
            // textEdit6
            // 
            this.dcUser.SetDataMember(this.textEdit6, "phone");
            this.textEdit6.Location = new System.Drawing.Point(131, 206);
            this.textEdit6.Name = "textEdit6";
            this.textEdit6.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEdit6.Properties.Appearance.Options.UseFont = true;
            this.textEdit6.Properties.AutoHeight = false;
            this.textEdit6.Size = new System.Drawing.Size(232, 29);
            this.textEdit6.TabIndex = 77;
            // 
            // tePassword2
            // 
            this.dcUser.SetDataMember(this.tePassword2, "newPassword");
            this.tePassword2.Location = new System.Drawing.Point(491, 116);
            this.tePassword2.Name = "tePassword2";
            this.tePassword2.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tePassword2.Properties.Appearance.Options.UseFont = true;
            this.tePassword2.Properties.AutoHeight = false;
            this.tePassword2.Properties.PasswordChar = '*';
            this.tePassword2.Size = new System.Drawing.Size(233, 29);
            this.tePassword2.TabIndex = 83;
            // 
            // teEmail
            // 
            this.dcUser.SetDataMember(this.teEmail, "email");
            this.teEmail.Location = new System.Drawing.Point(491, 161);
            this.teEmail.Name = "teEmail";
            this.teEmail.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teEmail.Properties.Appearance.Options.UseFont = true;
            this.teEmail.Properties.AutoHeight = false;
            this.teEmail.Size = new System.Drawing.Size(233, 29);
            this.teEmail.TabIndex = 84;
            // 
            // radioGroup1
            // 
            this.dcUser.SetDataMember(this.radioGroup1, "userType");
            this.radioGroup1.EditValue = "0";
            this.radioGroup1.Location = new System.Drawing.Point(129, 71);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioGroup1.Properties.Appearance.Options.UseFont = true;
            this.radioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.radioGroup1.Properties.Columns = 2;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("0", "普通用户"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "管理员")});
            this.radioGroup1.Size = new System.Drawing.Size(234, 30);
            this.radioGroup1.TabIndex = 93;
            // 
            // radioGroup2
            // 
            this.dcUser.SetDataMember(this.radioGroup2, "isResetPwd");
            this.radioGroup2.EditValue = "0";
            this.radioGroup2.Location = new System.Drawing.Point(131, 251);
            this.radioGroup2.Name = "radioGroup2";
            this.radioGroup2.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioGroup2.Properties.Appearance.Options.UseFont = true;
            this.radioGroup2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.radioGroup2.Properties.Columns = 2;
            this.radioGroup2.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "是"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("0", "否")});
            this.radioGroup2.Size = new System.Drawing.Size(234, 30);
            this.radioGroup2.TabIndex = 95;
            // 
            // treeDept
            // 
            this.dcUser.SetDataMember(this.treeDept, "deptIds");
            this.treeDept.EditValue = "";
            this.treeDept.Location = new System.Drawing.Point(131, 341);
            this.treeDept.Name = "treeDept";
            this.treeDept.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeDept.Properties.Appearance.Options.UseFont = true;
            this.treeDept.Properties.AutoHeight = false;
            this.treeDept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.treeDept.Properties.NullText = "";
            this.treeDept.Properties.PopupFormSize = new System.Drawing.Size(590, 0);
            this.treeDept.Properties.TreeList = this.treeList2;
            this.treeDept.Size = new System.Drawing.Size(594, 26);
            this.treeDept.TabIndex = 112;
            this.treeDept.Visible = false;
            this.treeDept.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.treeDept_CustomDisplayText);
            // 
            // treeList2
            // 
            this.treeList2.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn9,
            this.treeListColumn10});
            this.treeList2.Location = new System.Drawing.Point(190, 222);
            this.treeList2.Name = "treeList2";
            this.treeList2.OptionsBehavior.EnableFiltering = true;
            this.treeList2.OptionsView.AllowHtmlDrawHeaders = true;
            this.treeList2.OptionsView.ShowCheckBoxes = true;
            this.treeList2.OptionsView.ShowIndentAsRowStyle = true;
            this.treeList2.RowHeight = 30;
            this.treeList2.Size = new System.Drawing.Size(400, 200);
            this.treeList2.TabIndex = 0;
            // 
            // treeListColumn9
            // 
            this.treeListColumn9.AppearanceCell.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.treeListColumn9.AppearanceCell.Options.UseFont = true;
            this.treeListColumn9.AppearanceHeader.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.treeListColumn9.AppearanceHeader.Options.UseFont = true;
            this.treeListColumn9.Caption = "科室";
            this.treeListColumn9.FieldName = "name";
            this.treeListColumn9.MinWidth = 32;
            this.treeListColumn9.Name = "treeListColumn9";
            this.treeListColumn9.Visible = true;
            this.treeListColumn9.VisibleIndex = 0;
            // 
            // treeListColumn10
            // 
            this.treeListColumn10.Caption = "id";
            this.treeListColumn10.FieldName = "id";
            this.treeListColumn10.Name = "treeListColumn10";
            // 
            // textEdit7
            // 
            this.textEdit7.Location = new System.Drawing.Point(491, 206);
            this.textEdit7.Name = "textEdit7";
            this.textEdit7.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEdit7.Properties.Appearance.Options.UseFont = true;
            this.textEdit7.Properties.AutoHeight = false;
            this.textEdit7.Size = new System.Drawing.Size(234, 29);
            this.textEdit7.TabIndex = 85;
            this.textEdit7.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(65, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 39;
            this.label3.Text = "头像：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(416, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 20);
            this.label4.TabIndex = 41;
            this.label4.Text = "登录名：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(67, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 20);
            this.label5.TabIndex = 42;
            this.label5.Text = "密码：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(67, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 20);
            this.label6.TabIndex = 43;
            this.label6.Text = "姓名：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(67, 209);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 20);
            this.label9.TabIndex = 45;
            this.label9.Text = "固话：";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(70, 731);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 20);
            this.label11.TabIndex = 47;
            this.label11.Text = "备注：";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label11.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(39, 299);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 20);
            this.label12.TabIndex = 48;
            this.label12.Text = "用户角色：";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(402, 119);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 20);
            this.label7.TabIndex = 80;
            this.label7.Text = "确认密码：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(429, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 20);
            this.label8.TabIndex = 81;
            this.label8.Text = "邮箱：";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(429, 209);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 20);
            this.label10.TabIndex = 46;
            this.label10.Text = "手机：";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label10.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(129, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 89;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnUpload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnUpload.HoverBackColor = System.Drawing.Color.Empty;
            this.btnUpload.Location = new System.Drawing.Point(335, 30);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.SecondText = "";
            this.btnUpload.Size = new System.Drawing.Size(75, 30);
            this.btnUpload.Style = Xr.Common.Controls.ButtonStyle.Save;
            this.btnUpload.TabIndex = 90;
            this.btnUpload.Text = "上传";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnSelect.HoverBackColor = System.Drawing.Color.Empty;
            this.btnSelect.Location = new System.Drawing.Point(238, 30);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.SecondText = "";
            this.btnSelect.Size = new System.Drawing.Size(75, 30);
            this.btnSelect.Style = Xr.Common.Controls.ButtonStyle.Query;
            this.btnSelect.TabIndex = 91;
            this.btnSelect.Text = "选择";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(41, 74);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 20);
            this.label13.TabIndex = 92;
            this.label13.Text = "用户类型：";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(14, 254);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(107, 20);
            this.label14.TabIndex = 94;
            this.label14.Text = "更换周期限制：";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnSave.HoverBackColor = System.Drawing.Color.Empty;
            this.btnSave.Location = new System.Drawing.Point(540, 632);
            this.btnSave.Name = "btnSave";
            this.btnSave.SecondText = "";
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
            this.btnCancel.Location = new System.Drawing.Point(648, 632);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.SecondText = "";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.Style = Xr.Common.Controls.ButtonStyle.Return;
            this.btnCancel.TabIndex = 99;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(196)))), ((int)(((byte)(203)))));
            this.panelEx1.BorderSize = 1;
            this.panelEx1.BorderStyleBottom = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx1.BorderStyleLeft = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx1.BorderStyleRight = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx1.BorderStyleTop = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx1.Controls.Add(this.richEditor1);
            this.panelEx1.Location = new System.Drawing.Point(136, 730);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx1.Size = new System.Drawing.Size(593, 150);
            this.panelEx1.TabIndex = 101;
            this.panelEx1.Visible = false;
            // 
            // richEditor1
            // 
            this.richEditor1.BorderColor = System.Drawing.Color.Black;
            this.richEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditor1.HeightLightBolorColor = System.Drawing.Color.Black;
            this.richEditor1.ImagUploadUrl = null;
            this.richEditor1.Location = new System.Drawing.Point(1, 1);
            this.richEditor1.Name = "richEditor1";
            this.richEditor1.Size = new System.Drawing.Size(591, 148);
            this.richEditor1.TabIndex = 101;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(123, 117);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(7, 14);
            this.labelControl1.TabIndex = 102;
            this.labelControl1.Text = "*";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(25, 461);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(93, 20);
            this.label15.TabIndex = 103;
            this.label15.Text = "可操作科室：";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(123, 466);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(7, 14);
            this.labelControl2.TabIndex = 113;
            this.labelControl2.Text = "*";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(123, 301);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(7, 14);
            this.labelControl3.TabIndex = 114;
            this.labelControl3.Text = "*";
            // 
            // treeList1
            // 
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn2});
            this.treeList1.Location = new System.Drawing.Point(131, 461);
            this.treeList1.Name = "treeList1";
            this.treeList1.Size = new System.Drawing.Size(594, 150);
            this.treeList1.TabIndex = 115;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "科室列表";
            this.treeListColumn2.FieldName = "name";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 0;
            // 
            // treeListRole
            // 
            this.treeListRole.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeListRole.Location = new System.Drawing.Point(131, 296);
            this.treeListRole.Name = "treeListRole";
            this.treeListRole.Size = new System.Drawing.Size(594, 150);
            this.treeListRole.TabIndex = 116;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "角色列表";
            this.treeListColumn1.FieldName = "name";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // UserEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(781, 683);
            this.Controls.Add(this.treeListRole);
            this.Controls.Add(this.treeList1);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.treeDept);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.radioGroup2);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textEdit7);
            this.Controls.Add(this.teEmail);
            this.Controls.Add(this.tePassword2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textEdit6);
            this.Controls.Add(this.teName);
            this.Controls.Add(this.tePassword);
            this.Controls.Add(this.teLoginName);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户添加";
            this.Load += new System.EventHandler(this.UserEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.teLoginName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit6.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePassword2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeDept.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit7.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListRole)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Xr.Common.Controls.DataController dcUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private DevExpress.XtraEditors.TextEdit teLoginName;
        private DevExpress.XtraEditors.TextEdit tePassword;
        private DevExpress.XtraEditors.TextEdit teName;
        private DevExpress.XtraEditors.TextEdit textEdit6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.TextEdit tePassword2;
        private DevExpress.XtraEditors.TextEdit teEmail;
        private DevExpress.XtraEditors.TextEdit textEdit7;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Xr.Common.Controls.ButtonControl btnUpload;
        private Xr.Common.Controls.ButtonControl btnSelect;
        private System.Windows.Forms.Label label13;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.RadioGroup radioGroup2;
        private System.Windows.Forms.Label label14;
        private Xr.Common.Controls.ButtonControl btnSave;
        private Xr.Common.Controls.ButtonControl btnCancel;
        private Xr.Common.Controls.PanelEx panelEx1;
        private Xr.Common.Controls.RichEditor richEditor1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Label label15;
        private DevExpress.XtraEditors.TreeListLookUpEdit treeDept;
        private DevExpress.XtraTreeList.TreeList treeList2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn9;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn10;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.TreeList treeListRole;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
    }
}
