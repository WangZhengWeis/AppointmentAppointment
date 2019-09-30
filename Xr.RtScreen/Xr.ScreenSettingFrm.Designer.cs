namespace Xr.RtScreen
{
    partial class SettingFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingFrm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.Postoperative = new DevExpress.XtraEditors.RadioGroup();
            this.treeHostile = new DevExpress.XtraEditors.TreeListLookUpEdit();
            this.treeList3 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeClinc = new DevExpress.XtraEditors.TreeListLookUpEdit();
            this.treeList2 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeKeshi = new DevExpress.XtraEditors.TreeListLookUpEdit();
            this.treeListLookUpEdit1TreeList = new DevExpress.XtraTreeList.TreeList();
            this.treeListColunm1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.buttonControl1 = new Xr.Common.Controls.ButtonControl();
            this.buttonControl2 = new Xr.Common.Controls.ButtonControl();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Postoperative.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeHostile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeClinc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeKeshi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListLookUpEdit1TreeList)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Postoperative);
            this.panel1.Controls.Add(this.treeHostile);
            this.panel1.Controls.Add(this.treeClinc);
            this.panel1.Controls.Add(this.treeKeshi);
            this.panel1.Controls.Add(this.buttonControl1);
            this.panel1.Controls.Add(this.buttonControl2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(456, 314);
            this.panel1.TabIndex = 127;
            // 
            // Postoperative
            // 
            this.Postoperative.EditValue = "1";
            this.Postoperative.Location = new System.Drawing.Point(121, 109);
            this.Postoperative.Margin = new System.Windows.Forms.Padding(1);
            this.Postoperative.Name = "Postoperative";
            this.Postoperative.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Postoperative.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Postoperative.Properties.Appearance.Options.UseBackColor = true;
            this.Postoperative.Properties.Appearance.Options.UseFont = true;
            this.Postoperative.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.Postoperative.Properties.Columns = 4;
            this.Postoperative.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "大屏"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "科室屏"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("3", "医生屏"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("4", "科室医生")});
            this.Postoperative.Properties.SelectedIndexChanged += new System.EventHandler(this.Postoperative_Properties_SelectedIndexChanged);
            this.Postoperative.Size = new System.Drawing.Size(325, 35);
            this.Postoperative.TabIndex = 184;
            // 
            // treeHostile
            // 
            this.treeHostile.EditValue = "全部";
            this.treeHostile.Location = new System.Drawing.Point(121, 19);
            this.treeHostile.Name = "treeHostile";
            this.treeHostile.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeHostile.Properties.Appearance.Options.UseFont = true;
            this.treeHostile.Properties.AutoHeight = false;
            this.treeHostile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.treeHostile.Properties.NullText = "全部";
            this.treeHostile.Properties.PopupFormMinSize = new System.Drawing.Size(30, 0);
            this.treeHostile.Properties.PopupFormSize = new System.Drawing.Size(160, 0);
            this.treeHostile.Properties.TreeList = this.treeList3;
            this.treeHostile.Size = new System.Drawing.Size(323, 27);
            this.treeHostile.TabIndex = 127;
            this.treeHostile.EditValueChanged += new System.EventHandler(this.treeHostile_EditValueChanged);
            // 
            // treeList3
            // 
            this.treeList3.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn2});
            this.treeList3.Location = new System.Drawing.Point(-44, 25);
            this.treeList3.Name = "treeList3";
            this.treeList3.OptionsBehavior.EnableFiltering = true;
            this.treeList3.OptionsView.ShowIndentAsRowStyle = true;
            this.treeList3.Size = new System.Drawing.Size(400, 200);
            this.treeList3.TabIndex = 0;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.AppearanceCell.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeListColumn2.AppearanceCell.Options.UseFont = true;
            this.treeListColumn2.AppearanceHeader.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeListColumn2.AppearanceHeader.Options.UseFont = true;
            this.treeListColumn2.Caption = "医院列表";
            this.treeListColumn2.FieldName = "name";
            this.treeListColumn2.MinWidth = 30;
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 0;
            // 
            // treeClinc
            // 
            this.treeClinc.EditValue = "";
            this.treeClinc.Location = new System.Drawing.Point(121, 156);
            this.treeClinc.Name = "treeClinc";
            this.treeClinc.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeClinc.Properties.Appearance.Options.UseFont = true;
            this.treeClinc.Properties.AutoHeight = false;
            this.treeClinc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.treeClinc.Properties.NullText = "";
            this.treeClinc.Properties.PopupFormMinSize = new System.Drawing.Size(30, 0);
            this.treeClinc.Properties.PopupFormSize = new System.Drawing.Size(160, 0);
            this.treeClinc.Properties.TreeList = this.treeList2;
            this.treeClinc.Size = new System.Drawing.Size(323, 25);
            this.treeClinc.TabIndex = 126;
            this.treeClinc.Visible = false;
            // 
            // treeList2
            // 
            this.treeList2.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeList2.Location = new System.Drawing.Point(-44, 25);
            this.treeList2.Name = "treeList2";
            this.treeList2.OptionsBehavior.EnableFiltering = true;
            this.treeList2.OptionsView.ShowIndentAsRowStyle = true;
            this.treeList2.Size = new System.Drawing.Size(400, 200);
            this.treeList2.TabIndex = 0;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.AppearanceCell.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeListColumn1.AppearanceCell.Options.UseFont = true;
            this.treeListColumn1.AppearanceHeader.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeListColumn1.AppearanceHeader.Options.UseFont = true;
            this.treeListColumn1.Caption = "诊室列表";
            this.treeListColumn1.FieldName = "name";
            this.treeListColumn1.MinWidth = 30;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // treeKeshi
            // 
            this.treeKeshi.EditValue = "";
            this.treeKeshi.Location = new System.Drawing.Point(121, 71);
            this.treeKeshi.Name = "treeKeshi";
            this.treeKeshi.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeKeshi.Properties.Appearance.Options.UseFont = true;
            this.treeKeshi.Properties.AutoHeight = false;
            this.treeKeshi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.treeKeshi.Properties.NullText = "";
            this.treeKeshi.Properties.PopupFormMinSize = new System.Drawing.Size(30, 0);
            this.treeKeshi.Properties.PopupFormSize = new System.Drawing.Size(160, 0);
            this.treeKeshi.Properties.TreeList = this.treeListLookUpEdit1TreeList;
            this.treeKeshi.Size = new System.Drawing.Size(323, 27);
            this.treeKeshi.TabIndex = 125;
            this.treeKeshi.EditValueChanged += new System.EventHandler(this.treeKeshi_EditValueChanged);
            this.treeKeshi.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.txtDoctors_CustomDisplayText);
            // 
            // treeListLookUpEdit1TreeList
            // 
            this.treeListLookUpEdit1TreeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColunm1});
            this.treeListLookUpEdit1TreeList.Location = new System.Drawing.Point(-44, 25);
            this.treeListLookUpEdit1TreeList.Name = "treeListLookUpEdit1TreeList";
            this.treeListLookUpEdit1TreeList.OptionsBehavior.EnableFiltering = true;
            this.treeListLookUpEdit1TreeList.OptionsView.AllowHtmlDrawHeaders = true;
            this.treeListLookUpEdit1TreeList.OptionsView.ShowCheckBoxes = true;
            this.treeListLookUpEdit1TreeList.OptionsView.ShowIndentAsRowStyle = true;
            this.treeListLookUpEdit1TreeList.Size = new System.Drawing.Size(400, 200);
            this.treeListLookUpEdit1TreeList.TabIndex = 0;
            // 
            // treeListColunm1
            // 
            this.treeListColunm1.AppearanceCell.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeListColunm1.AppearanceCell.Options.UseFont = true;
            this.treeListColunm1.AppearanceHeader.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeListColunm1.AppearanceHeader.Options.UseFont = true;
            this.treeListColunm1.Caption = "科室列表";
            this.treeListColunm1.FieldName = "name";
            this.treeListColunm1.MinWidth = 50;
            this.treeListColunm1.Name = "treeListColunm1";
            this.treeListColunm1.Visible = true;
            this.treeListColunm1.VisibleIndex = 0;
            // 
            // buttonControl1
            // 
            this.buttonControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.buttonControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.buttonControl1.HoverBackColor = System.Drawing.Color.Empty;
            this.buttonControl1.Location = new System.Drawing.Point(191, 208);
            this.buttonControl1.Name = "buttonControl1";
            this.buttonControl1.SecondText = "";
            this.buttonControl1.Size = new System.Drawing.Size(75, 30);
            this.buttonControl1.Style = Xr.Common.Controls.ButtonStyle.Save;
            this.buttonControl1.TabIndex = 2;
            this.buttonControl1.Text = "确定";
            this.buttonControl1.Click += new System.EventHandler(this.buttonControl1_Click);
            // 
            // buttonControl2
            // 
            this.buttonControl2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.buttonControl2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(131)))), ((int)(((byte)(113)))));
            this.buttonControl2.HoverBackColor = System.Drawing.Color.Empty;
            this.buttonControl2.Location = new System.Drawing.Point(272, 208);
            this.buttonControl2.Name = "buttonControl2";
            this.buttonControl2.SecondText = "";
            this.buttonControl2.Size = new System.Drawing.Size(75, 30);
            this.buttonControl2.Style = Xr.Common.Controls.ButtonStyle.Return;
            this.buttonControl2.TabIndex = 3;
            this.buttonControl2.Text = "取消";
            this.buttonControl2.Click += new System.EventHandler(this.buttonControl2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(34, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "医院名称：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(34, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "科室名称：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(34, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 21);
            this.label5.TabIndex = 1;
            this.label5.Text = "诊室名称：";
            this.label5.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(34, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "启动屏幕：";
            // 
            // SettingFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 314);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "初始设置";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Postoperative.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeHostile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeClinc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeKeshi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListLookUpEdit1TreeList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Common.Controls.ButtonControl buttonControl1;
        private Common.Controls.ButtonControl buttonControl2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TreeListLookUpEdit treeKeshi;
        private DevExpress.XtraTreeList.TreeList treeListLookUpEdit1TreeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColunm1;
        private DevExpress.XtraEditors.TreeListLookUpEdit treeClinc;
        private DevExpress.XtraTreeList.TreeList treeList2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraEditors.TreeListLookUpEdit treeHostile;
        private DevExpress.XtraTreeList.TreeList treeList3;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.RadioGroup Postoperative;
    }
}