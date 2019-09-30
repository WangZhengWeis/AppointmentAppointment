namespace Xr.RtManager
{
    partial class OfficeEdit
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.teName = new DevExpress.XtraEditors.TextEdit();
            this.teCode = new DevExpress.XtraEditors.TextEdit();
            this.teAddress = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.treeParent = new DevExpress.XtraEditors.TreeListLookUpEdit();
            this.treeListLookUpEdit1TreeList = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeArea = new DevExpress.XtraEditors.TreeListLookUpEdit();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.teMaster = new DevExpress.XtraEditors.TextEdit();
            this.teZipCode = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dcOffice = new Xr.Common.Controls.DataController(this.components);
            this.tePhone = new DevExpress.XtraEditors.TextEdit();
            this.teFax = new DevExpress.XtraEditors.TextEdit();
            this.teEmail = new DevExpress.XtraEditors.TextEdit();
            this.lueGrade = new DevExpress.XtraEditors.LookUpEdit();
            this.lueType = new DevExpress.XtraEditors.LookUpEdit();
            this.meRemarks = new DevExpress.XtraEditors.MemoEdit();
            this.btnSave = new Xr.Common.Controls.ButtonControl();
            this.btnCancel = new Xr.Common.Controls.ButtonControl();
            ((System.ComponentModel.ISupportInitialize)(this.teName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeParent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListLookUpEdit1TreeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teMaster.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teZipCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teFax.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueGrade.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.meRemarks.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // teName
            // 
            this.dcOffice.SetDataMember(this.teName, "name");
            this.teName.Location = new System.Drawing.Point(129, 71);
            this.teName.Name = "teName";
            this.teName.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teName.Properties.Appearance.Options.UseFont = true;
            this.teName.Properties.AutoHeight = false;
            this.teName.Size = new System.Drawing.Size(232, 29);
            this.teName.TabIndex = 3;
            // 
            // teCode
            // 
            this.dcOffice.SetDataMember(this.teCode, "code");
            this.teCode.Location = new System.Drawing.Point(487, 71);
            this.teCode.Name = "teCode";
            this.teCode.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teCode.Properties.Appearance.Options.UseFont = true;
            this.teCode.Properties.AutoHeight = false;
            this.teCode.Size = new System.Drawing.Size(233, 29);
            this.teCode.TabIndex = 4;
            // 
            // teAddress
            // 
            this.dcOffice.SetDataMember(this.teAddress, "address");
            this.teAddress.Location = new System.Drawing.Point(129, 161);
            this.teAddress.Name = "teAddress";
            this.teAddress.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teAddress.Properties.Appearance.Options.UseFont = true;
            this.teAddress.Properties.AutoHeight = false;
            this.teAddress.Size = new System.Drawing.Size(232, 29);
            this.teAddress.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(39, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 36;
            this.label1.Text = "上级单位：";
            // 
            // treeParent
            // 
            this.dcOffice.SetDataMember(this.treeParent, "parentId");
            this.treeParent.EditValue = "";
            this.treeParent.Location = new System.Drawing.Point(129, 25);
            this.treeParent.Name = "treeParent";
            this.treeParent.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeParent.Properties.Appearance.Options.UseFont = true;
            this.treeParent.Properties.AutoHeight = false;
            this.treeParent.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.treeParent.Properties.PopupFormSize = new System.Drawing.Size(232, 0);
            this.treeParent.Properties.TreeList = this.treeListLookUpEdit1TreeList;
            this.treeParent.Size = new System.Drawing.Size(232, 29);
            this.treeParent.TabIndex = 1;
            // 
            // treeListLookUpEdit1TreeList
            // 
            this.treeListLookUpEdit1TreeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeListLookUpEdit1TreeList.Location = new System.Drawing.Point(-113, -81);
            this.treeListLookUpEdit1TreeList.Name = "treeListLookUpEdit1TreeList";
            this.treeListLookUpEdit1TreeList.OptionsBehavior.EnableFiltering = true;
            this.treeListLookUpEdit1TreeList.OptionsView.AllowHtmlDrawHeaders = true;
            this.treeListLookUpEdit1TreeList.OptionsView.ShowIndentAsRowStyle = true;
            this.treeListLookUpEdit1TreeList.RowHeight = 30;
            this.treeListLookUpEdit1TreeList.Size = new System.Drawing.Size(400, 200);
            this.treeListLookUpEdit1TreeList.TabIndex = 0;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.AppearanceCell.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.treeListColumn1.AppearanceCell.Options.UseFont = true;
            this.treeListColumn1.AppearanceHeader.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.treeListColumn1.AppearanceHeader.Options.UseFont = true;
            this.treeListColumn1.Caption = "公司";
            this.treeListColumn1.FieldName = "name";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // treeArea
            // 
            this.dcOffice.SetDataMember(this.treeArea, "areaId");
            this.treeArea.EditValue = "";
            this.treeArea.Location = new System.Drawing.Point(487, 25);
            this.treeArea.Name = "treeArea";
            this.treeArea.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeArea.Properties.Appearance.Options.UseFont = true;
            this.treeArea.Properties.AutoHeight = false;
            this.treeArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.treeArea.Properties.PopupFormSize = new System.Drawing.Size(232, 0);
            this.treeArea.Properties.TreeList = this.treeList1;
            this.treeArea.Size = new System.Drawing.Size(233, 29);
            this.treeArea.TabIndex = 2;
            // 
            // treeList1
            // 
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn2});
            this.treeList1.Location = new System.Drawing.Point(-113, -81);
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsBehavior.EnableFiltering = true;
            this.treeList1.OptionsView.AllowHtmlDrawHeaders = true;
            this.treeList1.OptionsView.ShowIndentAsRowStyle = true;
            this.treeList1.RowHeight = 30;
            this.treeList1.Size = new System.Drawing.Size(400, 200);
            this.treeList1.TabIndex = 0;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.AppearanceCell.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.treeListColumn2.AppearanceCell.Options.UseFont = true;
            this.treeListColumn2.AppearanceHeader.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.treeListColumn2.AppearanceHeader.Options.UseFont = true;
            this.treeListColumn2.Caption = "机构";
            this.treeListColumn2.FieldName = "name";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 0;
            // 
            // teMaster
            // 
            this.dcOffice.SetDataMember(this.teMaster, "master");
            this.teMaster.Location = new System.Drawing.Point(129, 206);
            this.teMaster.Name = "teMaster";
            this.teMaster.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teMaster.Properties.Appearance.Options.UseFont = true;
            this.teMaster.Properties.AutoHeight = false;
            this.teMaster.Size = new System.Drawing.Size(233, 29);
            this.teMaster.TabIndex = 9;
            // 
            // teZipCode
            // 
            this.dcOffice.SetDataMember(this.teZipCode, "zipCode");
            this.teZipCode.Location = new System.Drawing.Point(487, 161);
            this.teZipCode.Name = "teZipCode";
            this.teZipCode.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teZipCode.Properties.Appearance.Options.UseFont = true;
            this.teZipCode.Properties.AutoHeight = false;
            this.teZipCode.Size = new System.Drawing.Size(233, 29);
            this.teZipCode.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(400, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 37;
            this.label2.Text = "归属区域：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(39, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 20);
            this.label4.TabIndex = 39;
            this.label4.Text = "单位名称：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(39, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 20);
            this.label5.TabIndex = 40;
            this.label5.Text = "单位类型：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(39, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 41;
            this.label6.Text = "联系地址：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(400, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 20);
            this.label7.TabIndex = 42;
            this.label7.Text = "单位级别：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(400, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 20);
            this.label8.TabIndex = 43;
            this.label8.Text = "邮政编码：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(52, 210);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 20);
            this.label9.TabIndex = 44;
            this.label9.Text = "负责人：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(426, 210);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 20);
            this.label10.TabIndex = 45;
            this.label10.Text = "电话：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(65, 255);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 20);
            this.label11.TabIndex = 46;
            this.label11.Text = "传真：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(65, 300);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 20);
            this.label12.TabIndex = 47;
            this.label12.Text = "备注：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(426, 255);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(51, 20);
            this.label14.TabIndex = 52;
            this.label14.Text = "邮箱：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(400, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 48;
            this.label3.Text = "单位编码：";
            // 
            // tePhone
            // 
            this.dcOffice.SetDataMember(this.tePhone, "phone");
            this.tePhone.Location = new System.Drawing.Point(487, 206);
            this.tePhone.Name = "tePhone";
            this.tePhone.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tePhone.Properties.Appearance.Options.UseFont = true;
            this.tePhone.Properties.AutoHeight = false;
            this.tePhone.Size = new System.Drawing.Size(233, 29);
            this.tePhone.TabIndex = 10;
            // 
            // teFax
            // 
            this.dcOffice.SetDataMember(this.teFax, "fax");
            this.teFax.Location = new System.Drawing.Point(129, 251);
            this.teFax.Name = "teFax";
            this.teFax.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teFax.Properties.Appearance.Options.UseFont = true;
            this.teFax.Properties.AutoHeight = false;
            this.teFax.Size = new System.Drawing.Size(233, 29);
            this.teFax.TabIndex = 11;
            // 
            // teEmail
            // 
            this.dcOffice.SetDataMember(this.teEmail, "email");
            this.teEmail.Location = new System.Drawing.Point(487, 251);
            this.teEmail.Name = "teEmail";
            this.teEmail.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teEmail.Properties.Appearance.Options.UseFont = true;
            this.teEmail.Properties.AutoHeight = false;
            this.teEmail.Size = new System.Drawing.Size(233, 29);
            this.teEmail.TabIndex = 12;
            // 
            // lueGrade
            // 
            this.dcOffice.SetDataMember(this.lueGrade, "grade");
            this.lueGrade.Location = new System.Drawing.Point(487, 116);
            this.lueGrade.Name = "lueGrade";
            this.lueGrade.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueGrade.Properties.Appearance.Options.UseFont = true;
            this.lueGrade.Properties.AppearanceDisabled.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lueGrade.Properties.AppearanceDisabled.Options.UseFont = true;
            this.lueGrade.Properties.AppearanceDropDown.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lueGrade.Properties.AppearanceDropDown.Options.UseFont = true;
            this.lueGrade.Properties.AppearanceDropDownHeader.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lueGrade.Properties.AppearanceDropDownHeader.Options.UseFont = true;
            this.lueGrade.Properties.AppearanceFocused.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lueGrade.Properties.AppearanceFocused.Options.UseFont = true;
            this.lueGrade.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueGrade.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.lueGrade.Properties.AutoHeight = false;
            this.lueGrade.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueGrade.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("code", "键值", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("name", "级别")});
            this.lueGrade.Properties.NullText = "";
            this.lueGrade.Size = new System.Drawing.Size(232, 29);
            this.lueGrade.TabIndex = 6;
            // 
            // lueType
            // 
            this.dcOffice.SetDataMember(this.lueType, "type");
            this.lueType.Location = new System.Drawing.Point(129, 116);
            this.lueType.Name = "lueType";
            this.lueType.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueType.Properties.Appearance.Options.UseFont = true;
            this.lueType.Properties.AppearanceDisabled.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueType.Properties.AppearanceDisabled.Options.UseFont = true;
            this.lueType.Properties.AppearanceDropDown.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueType.Properties.AppearanceDropDown.Options.UseFont = true;
            this.lueType.Properties.AppearanceDropDownHeader.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueType.Properties.AppearanceDropDownHeader.Options.UseFont = true;
            this.lueType.Properties.AppearanceFocused.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueType.Properties.AppearanceFocused.Options.UseFont = true;
            this.lueType.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueType.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.lueType.Properties.AutoHeight = false;
            serializableAppearanceObject1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            serializableAppearanceObject1.Options.UseFont = true;
            this.lueType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.lueType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("code", "键值", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("name", "类型")});
            this.lueType.Properties.NullText = "";
            this.lueType.Size = new System.Drawing.Size(232, 29);
            this.lueType.TabIndex = 5;
            // 
            // meRemarks
            // 
            this.dcOffice.SetDataMember(this.meRemarks, "remarks");
            this.meRemarks.Location = new System.Drawing.Point(129, 298);
            this.meRemarks.Name = "meRemarks";
            this.meRemarks.Size = new System.Drawing.Size(234, 78);
            this.meRemarks.TabIndex = 13;
            this.meRemarks.UseOptimizedRendering = true;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnSave.HoverBackColor = System.Drawing.Color.Empty;
            this.btnSave.Location = new System.Drawing.Point(533, 415);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.Style = Xr.Common.Controls.ButtonStyle.Save;
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnCancel.HoverBackColor = System.Drawing.Color.Empty;
            this.btnCancel.Location = new System.Drawing.Point(642, 415);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.Style = Xr.Common.Controls.ButtonStyle.Return;
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // OfficeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(781, 481);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.meRemarks);
            this.Controls.Add(this.lueType);
            this.Controls.Add(this.lueGrade);
            this.Controls.Add(this.teEmail);
            this.Controls.Add(this.teFax);
            this.Controls.Add(this.tePhone);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.teZipCode);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.teMaster);
            this.Controls.Add(this.teAddress);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.teCode);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.teName);
            this.Controls.Add(this.treeArea);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.treeParent);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Name = "OfficeEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "机构添加";
            this.Load += new System.EventHandler(this.OfficeEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.teName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeParent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListLookUpEdit1TreeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teMaster.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teZipCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teFax.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueGrade.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.meRemarks.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit teZipCode;
        private DevExpress.XtraEditors.TextEdit teMaster;
        private DevExpress.XtraEditors.TextEdit teAddress;
        private DevExpress.XtraEditors.TextEdit teCode;
        private DevExpress.XtraEditors.TextEdit teName;
        private DevExpress.XtraEditors.TreeListLookUpEdit treeArea;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraEditors.TreeListLookUpEdit treeParent;
        private DevExpress.XtraTreeList.TreeList treeListLookUpEdit1TreeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private System.Windows.Forms.Label label1;
        private Xr.Common.Controls.DataController dcOffice;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit tePhone;
        private DevExpress.XtraEditors.TextEdit teFax;
        private DevExpress.XtraEditors.TextEdit teEmail;
        private DevExpress.XtraEditors.LookUpEdit lueGrade;
        private DevExpress.XtraEditors.LookUpEdit lueType;
        private DevExpress.XtraEditors.MemoEdit meRemarks;
        private Xr.Common.Controls.ButtonControl btnSave;
        private Xr.Common.Controls.ButtonControl btnCancel;
    }
}
