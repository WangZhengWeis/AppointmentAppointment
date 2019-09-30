namespace Xr.RtManager
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tmHeartbeat = new System.Windows.Forms.Timer(this.components);
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup2 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup3 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem1 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem2 = new DevExpress.XtraNavBar.NavBarItem();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panelEnhanced1 = new Xr.Common.Controls.PanelEnhanced();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panMenuBar = new Xr.Common.Controls.PanelEx(this.components);
            this.panelEx1 = new Xr.Common.Controls.PanelEx(this.components);
            this.panelEx3 = new Xr.Common.Controls.PanelEx(this.components);
            this.panelEx2 = new Xr.Common.Controls.PanelEx(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labBottomRight = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labBottomLeft = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.关闭其他页签ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关闭全部页签ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelEnhanced1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.panel5.SuspendLayout();
            this.panMenuBar.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.panelEx2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tmHeartbeat
            // 
            this.tmHeartbeat.Interval = 43200000;
            this.tmHeartbeat.Tick += new System.EventHandler(this.tmHeartbeat_Tick);
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "navBarGroup1";
            this.navBarGroup1.Name = "navBarGroup1";
            // 
            // navBarGroup2
            // 
            this.navBarGroup2.Caption = "navBarGroup2";
            this.navBarGroup2.Name = "navBarGroup2";
            // 
            // navBarGroup3
            // 
            this.navBarGroup3.Caption = "navBarGroup3";
            this.navBarGroup3.Name = "navBarGroup3";
            // 
            // navBarItem1
            // 
            this.navBarItem1.Caption = "navBarItem1";
            this.navBarItem1.Name = "navBarItem1";
            // 
            // navBarItem2
            // 
            this.navBarItem2.Caption = "navBarItem2";
            this.navBarItem2.Name = "navBarItem2";
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(1, 1);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(843, 699);
            this.panel7.TabIndex = 0;
            // 
            // panelEnhanced1
            // 
            this.panelEnhanced1.BackgroundImage = global::Xr.RtManager.Properties.Resources.bg;
            this.panelEnhanced1.Controls.Add(this.xtraTabControl1);
            this.panelEnhanced1.Controls.Add(this.panel4);
            this.panelEnhanced1.Controls.Add(this.panel5);
            this.panelEnhanced1.Controls.Add(this.panel1);
            this.panelEnhanced1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEnhanced1.Location = new System.Drawing.Point(0, 0);
            this.panelEnhanced1.Name = "panelEnhanced1";
            this.panelEnhanced1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.panelEnhanced1.Size = new System.Drawing.Size(1008, 729);
            this.panelEnhanced1.TabIndex = 0;
            this.panelEnhanced1.SizeChanged += new System.EventHandler(this.panelEnhanced1_SizeChanged);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.xtraTabControl1.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xtraTabControl1.Appearance.Options.UseBackColor = true;
            this.xtraTabControl1.Appearance.Options.UseFont = true;
            this.xtraTabControl1.AppearancePage.Header.Font = new System.Drawing.Font("Tahoma", 10F);
            this.xtraTabControl1.AppearancePage.Header.Options.UseFont = true;
            this.xtraTabControl1.AppearancePage.HeaderActive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.xtraTabControl1.AppearancePage.HeaderActive.Options.UseForeColor = true;
            this.xtraTabControl1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders;
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(168, 3);
            this.xtraTabControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.Size = new System.Drawing.Size(840, 702);
            this.xtraTabControl1.TabIndex = 20;
            this.xtraTabControl1.CloseButtonClick += new System.EventHandler(this.xtraTabControl1_CloseButtonClick);
            this.xtraTabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.xtraTabControl_MouseDown);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(163, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(5, 702);
            this.panel4.TabIndex = 19;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Controls.Add(this.panMenuBar);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(160, 702);
            this.panel5.TabIndex = 17;
            // 
            // panMenuBar
            // 
            this.panMenuBar.BackColor = System.Drawing.Color.Transparent;
            this.panMenuBar.BorderColor = System.Drawing.Color.Black;
            this.panMenuBar.BorderSize = 1;
            this.panMenuBar.BorderStyleBottom = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panMenuBar.BorderStyleLeft = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panMenuBar.BorderStyleRight = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panMenuBar.BorderStyleTop = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panMenuBar.Controls.Add(this.panelEx1);
            this.panMenuBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMenuBar.Location = new System.Drawing.Point(0, 0);
            this.panMenuBar.Name = "panMenuBar";
            this.panMenuBar.Padding = new System.Windows.Forms.Padding(1);
            this.panMenuBar.Size = new System.Drawing.Size(160, 698);
            this.panMenuBar.TabIndex = 5;
            // 
            // panelEx1
            // 
            this.panelEx1.BorderColor = System.Drawing.Color.Black;
            this.panelEx1.BorderSize = 1;
            this.panelEx1.BorderStyleBottom = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx1.BorderStyleLeft = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx1.BorderStyleRight = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx1.BorderStyleTop = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx1.Controls.Add(this.panelEx3);
            this.panelEx1.Controls.Add(this.panelEx2);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(1, 1);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(158, 164);
            this.panelEx1.TabIndex = 0;
            this.panelEx1.Visible = false;
            // 
            // panelEx3
            // 
            this.panelEx3.BorderColor = System.Drawing.Color.Black;
            this.panelEx3.BorderSize = 1;
            this.panelEx3.BorderStyleBottom = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx3.BorderStyleLeft = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx3.BorderStyleRight = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx3.BorderStyleTop = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx3.Location = new System.Drawing.Point(0, 36);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(155, 109);
            this.panelEx3.TabIndex = 1;
            // 
            // panelEx2
            // 
            this.panelEx2.BorderColor = System.Drawing.Color.Black;
            this.panelEx2.BorderSize = 1;
            this.panelEx2.BorderStyleBottom = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx2.BorderStyleLeft = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx2.BorderStyleRight = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx2.BorderStyleTop = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.panelEx2.Controls.Add(this.label3);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx2.Location = new System.Drawing.Point(0, 0);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(158, 30);
            this.panelEx2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "label3";
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 698);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(160, 4);
            this.panel6.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 705);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1005, 24);
            this.panel1.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.labBottomRight, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labBottomLeft, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1005, 24);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labBottomRight
            // 
            this.labBottomRight.AutoSize = true;
            this.labBottomRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labBottomRight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labBottomRight.Location = new System.Drawing.Point(706, 0);
            this.labBottomRight.Name = "labBottomRight";
            this.labBottomRight.Size = new System.Drawing.Size(296, 24);
            this.labBottomRight.TabIndex = 2;
            this.labBottomRight.Text = "2018-10-10 10:10:10";
            this.labBottomRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(304, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(396, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "技术支持：广州湘润信息科技有限公司 ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labBottomLeft
            // 
            this.labBottomLeft.AutoSize = true;
            this.labBottomLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labBottomLeft.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labBottomLeft.Location = new System.Drawing.Point(3, 0);
            this.labBottomLeft.Name = "labBottomLeft";
            this.labBottomLeft.Size = new System.Drawing.Size(295, 24);
            this.labBottomLeft.TabIndex = 0;
            this.labBottomLeft.Text = "科室|姓名";
            this.labBottomLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关闭其他页签ToolStripMenuItem,
            this.关闭全部页签ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 48);
            // 
            // 关闭其他页签ToolStripMenuItem
            // 
            this.关闭其他页签ToolStripMenuItem.Name = "关闭其他页签ToolStripMenuItem";
            this.关闭其他页签ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.关闭其他页签ToolStripMenuItem.Text = "关闭其他页签";
            this.关闭其他页签ToolStripMenuItem.Click += new System.EventHandler(this.tsmCloseOther_Click);
            // 
            // 关闭全部页签ToolStripMenuItem
            // 
            this.关闭全部页签ToolStripMenuItem.Name = "关闭全部页签ToolStripMenuItem";
            this.关闭全部页签ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.关闭全部页签ToolStripMenuItem.Text = "关闭全部页签";
            this.关闭全部页签ToolStripMenuItem.Click += new System.EventHandler(this.tsmCloseAll_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.panelEnhanced1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "预约分诊系统";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.panelEnhanced1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panMenuBar.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            this.panelEx2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer tmHeartbeat;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup2;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup3;
        private DevExpress.XtraNavBar.NavBarItem navBarItem1;
        private DevExpress.XtraNavBar.NavBarItem navBarItem2;
        private System.Windows.Forms.Panel panel7;
        private Xr.Common.Controls.PanelEnhanced panelEnhanced1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel6;
        private Xr.Common.Controls.PanelEx panMenuBar;
        private Xr.Common.Controls.PanelEx panelEx1;
        private Xr.Common.Controls.PanelEx panelEx3;
        private Xr.Common.Controls.PanelEx panelEx2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labBottomRight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labBottomLeft;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 关闭其他页签ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关闭全部页签ToolStripMenuItem;

    }
}