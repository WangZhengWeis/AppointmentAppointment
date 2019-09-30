namespace Xr.RtManager
{
    partial class MenuEdit
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
            this.teHref = new DevExpress.XtraEditors.TextEdit();
            this.tePermission = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.treeParent = new DevExpress.XtraEditors.TreeListLookUpEdit();
            this.treeListLookUpEdit1TreeList = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dcMenu = new Xr.Common.Controls.DataController(this.components);
            this.teTarget = new DevExpress.XtraEditors.TextEdit();
            this.teName = new DevExpress.XtraEditors.TextEdit();
            this.rgIsShow = new DevExpress.XtraEditors.RadioGroup();
            this.teSort = new DevExpress.XtraEditors.TextEdit();
            this.btnSave = new Xr.Common.Controls.ButtonControl();
            this.btnCancel = new Xr.Common.Controls.ButtonControl();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.teHref.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePermission.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeParent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListLookUpEdit1TreeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teTarget.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgIsShow.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSort.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // teHref
            // 
            this.dcMenu.SetDataMember(this.teHref, "href");
            this.teHref.Location = new System.Drawing.Point(129, 71);
            this.teHref.Name = "teHref";
            this.teHref.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teHref.Properties.Appearance.Options.UseFont = true;
            this.teHref.Properties.AutoHeight = false;
            this.teHref.Size = new System.Drawing.Size(232, 29);
            this.teHref.TabIndex = 3;
            // 
            // tePermission
            // 
            this.dcMenu.SetDataMember(this.tePermission, "permission");
            this.tePermission.Location = new System.Drawing.Point(129, 161);
            this.tePermission.Name = "tePermission";
            this.tePermission.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tePermission.Properties.Appearance.Options.UseFont = true;
            this.tePermission.Properties.AutoHeight = false;
            this.tePermission.Size = new System.Drawing.Size(232, 29);
            this.tePermission.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(39, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 36;
            this.label1.Text = "上级菜单：";
            // 
            // treeParent
            // 
            this.dcMenu.SetDataMember(this.treeParent, "parentId");
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(65, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 20);
            this.label4.TabIndex = 39;
            this.label4.Text = "链接：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(65, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 20);
            this.label5.TabIndex = 40;
            this.label5.Text = "图标：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(39, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 41;
            this.label6.Text = "权限标识：";
            // 
            // teTarget
            // 
            this.dcMenu.SetDataMember(this.teTarget, "target");
            this.teTarget.Location = new System.Drawing.Point(487, 71);
            this.teTarget.Name = "teTarget";
            this.teTarget.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teTarget.Properties.Appearance.Options.UseFont = true;
            this.teTarget.Properties.AutoHeight = false;
            this.teTarget.Size = new System.Drawing.Size(233, 29);
            this.teTarget.TabIndex = 4;
            // 
            // teName
            // 
            this.dcMenu.SetDataMember(this.teName, "name");
            this.teName.Location = new System.Drawing.Point(486, 25);
            this.teName.Name = "teName";
            this.teName.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teName.Properties.Appearance.Options.UseFont = true;
            this.teName.Properties.AutoHeight = false;
            this.teName.Size = new System.Drawing.Size(233, 29);
            this.teName.TabIndex = 49;
            // 
            // rgIsShow
            // 
            this.dcMenu.SetDataMember(this.rgIsShow, "isShow");
            this.rgIsShow.EditValue = "1";
            this.rgIsShow.Location = new System.Drawing.Point(487, 161);
            this.rgIsShow.Name = "rgIsShow";
            this.rgIsShow.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rgIsShow.Properties.Appearance.Options.UseFont = true;
            this.rgIsShow.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rgIsShow.Properties.Columns = 2;
            this.rgIsShow.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "显示"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("0", "隐藏")});
            this.rgIsShow.Size = new System.Drawing.Size(135, 31);
            this.rgIsShow.TabIndex = 52;
            // 
            // teSort
            // 
            this.dcMenu.SetDataMember(this.teSort, "sort");
            this.teSort.Location = new System.Drawing.Point(486, 116);
            this.teSort.Name = "teSort";
            this.teSort.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teSort.Properties.Appearance.Options.UseFont = true;
            this.teSort.Properties.AutoHeight = false;
            this.teSort.Properties.Mask.EditMask = "[0-9]*";
            this.teSort.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.teSort.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.teSort.Size = new System.Drawing.Size(233, 29);
            this.teSort.TabIndex = 53;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.btnSave.HoverBackColor = System.Drawing.Color.Empty;
            this.btnSave.Location = new System.Drawing.Point(534, 222);
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
            this.btnCancel.Location = new System.Drawing.Point(643, 222);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.Style = Xr.Common.Controls.ButtonStyle.Return;
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(426, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 48;
            this.label3.Text = "目标：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(426, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 37;
            this.label2.Text = "名称：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(426, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 20);
            this.label8.TabIndex = 43;
            this.label8.Text = "可见：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(426, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 20);
            this.label7.TabIndex = 42;
            this.label7.Text = "排序：";
            // 
            // MenuEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(781, 267);
            this.Controls.Add(this.teSort);
            this.Controls.Add(this.rgIsShow);
            this.Controls.Add(this.teName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tePermission);
            this.Controls.Add(this.teTarget);
            this.Controls.Add(this.teHref);
            this.Controls.Add(this.treeParent);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MenuEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "菜单添加";
            this.Load += new System.EventHandler(this.MenuEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.teHref.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePermission.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeParent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListLookUpEdit1TreeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teTarget.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgIsShow.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSort.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit tePermission;
        private DevExpress.XtraEditors.TextEdit teHref;
        private DevExpress.XtraEditors.TreeListLookUpEdit treeParent;
        private DevExpress.XtraTreeList.TreeList treeListLookUpEdit1TreeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private System.Windows.Forms.Label label1;
        private Xr.Common.Controls.DataController dcMenu;
        private Xr.Common.Controls.ButtonControl btnSave;
        private Xr.Common.Controls.ButtonControl btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit teTarget;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit teName;
        private DevExpress.XtraEditors.RadioGroup rgIsShow;
        private DevExpress.XtraEditors.TextEdit teSort;
    }
}
