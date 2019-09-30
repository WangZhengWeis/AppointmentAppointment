using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Threading;
using Xr.Common;
using Xr.Common.Controls;
using Xr.Http;

namespace Xr.RtManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            cmd = new Xr.Common.Controls.OpaqueCommand(this);
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            panMenuBar.BorderColor = borderColor;
        }
        private Xr.Common.Controls.OpaqueCommand cmd;
        private Xr.Common.Controls.OpaqueCommand cmd2;
        private Color borderColor = Color.FromArgb(157, 160, 170);

        public int openT6 = -1;

        private void MainForm_Load(object sender, EventArgs e)
        {
            AppContext.Session.openStatus = false;
            cmd.ShowOpaqueLayer(0f);
            var deptName = "无";
            if (AppContext.Session.deptList.Count > 0)
            {
                deptName = AppContext.Session.deptList[0].name;
            }
            labBottomLeft.Text = deptName + " | " + AppContext.Session.name + " | " + System.DateTime.Now.ToString();
            timer1.Start();

            tmHeartbeat.Enabled = true;

            var menuList = new List<MenuEntity>();
            for (var i = 0; i < AppContext.Session.menuList.Count(); i++)
            {
                var menu = AppContext.Session.menuList[i];
                if (menu.parentId.Equals("1"))
                {
                    menuList.Add(menu);
                }
            }
            menuList = menuList.OrderBy(x => x.sort).ToList();
            panMenuBar.Controls.Clear();
            foreach (MenuEntity menu in menuList)
            {
                AddContextMenu(menu.id, menu.name, menu.href, panMenuBar);
            }
            cmd.ShowOpaqueLayer(0f, "初始化读卡器...");
            DoWorkAsync(500, (o) =>
            {
                openT6 = HardwareInitialClass.OpenDevice();
                if (openT6 != 0)
                {
                    Xr.Log4net.LogHelper.Info("社保读卡器初始化失败:");
                }
                else
                {
                    Xr.Log4net.LogHelper.Info("社保读卡器初始化成功");
                }
                return null;

            }, null, (data) =>
            {
                var form = new WelcomeForm();
                AaddUserControl(form, "Welcome", "欢迎页");
                cmd.HideOpaqueLayer();
            });
        }

        private void AddContextMenu(String menuId, String Caption, String tag, Panel parentPanel)
        {
            var font = new Font("微软雅黑", 11);
            var MenuItemHeight = 34;

            var panel21 = new PanelEx();
            panel21.BorderColor = borderColor;
            panel21.BorderStyleTop = ButtonBorderStyle.None;
            panel21.BorderStyleBottom = ButtonBorderStyle.None;
            panel21.BorderStyleRight = ButtonBorderStyle.None;
            panel21.BorderStyleLeft = ButtonBorderStyle.None;
            panel21.Name = "panel" + menuId;
            panel21.BackColor = Color.Transparent;
            panel21.AutoSize = true;
            panel21.Dock = DockStyle.Top;
            var panel22 = new PanelEx();
            panel22.BorderColor = borderColor;
            panel22.BorderStyleTop = ButtonBorderStyle.None;
            panel22.BorderStyleLeft = ButtonBorderStyle.None;
            panel22.BorderStyleRight = ButtonBorderStyle.None;
            panel22.Margin = new Padding(0, 0, 0, 0);
            panel22.Padding = new Padding(0, 6, 0, 0);
            panel22.Height = MenuItemHeight;
            panel22.Dock = DockStyle.Top;
            var label21 = new Label();
            label21.Name = "lab" + menuId;
            label21.Text = Caption;
            label21.Tag = tag;
            label21.Font = font;
            label21.Dock = DockStyle.Fill;
            panel22.Controls.Add(label21);
            panel21.Controls.Add(panel22);
            var menuList = new List<MenuEntity>();
            foreach (MenuEntity menu in AppContext.Session.menuList)
            {
                if (menu.parentId.Equals(menuId))
                {
                    menuList.Add(menu);
                }
            }
            if (menuList.Count > 0)
            {
                var panel23 = new PanelEx();
                panel23.BorderStyleLeft = ButtonBorderStyle.None;
                panel23.BorderStyleRight = ButtonBorderStyle.None;
                panel23.BorderColor = Color.FromArgb(157, 160, 170);
                panel23.Margin = new Padding(0, 0, 0, 0);
                panel23.Padding = new Padding(0, 1, 0, 0);
                panel23.Visible = false;
                panel23.AutoSize = true;
                panel23.Dock = DockStyle.Top;
                menuList = menuList.OrderBy(x => x.sort).ToList();
                foreach (MenuEntity menu in menuList)
                {
                    var panel25 = new PanelEx();
                    panel25.BorderColor = borderColor;
                    panel25.BorderStyleTop = ButtonBorderStyle.None;
                    panel25.BorderStyleBottom = ButtonBorderStyle.None;
                    panel25.BorderStyleLeft = ButtonBorderStyle.None;
                    panel25.BorderStyleRight = ButtonBorderStyle.None;
                    panel25.BackColor = Color.Transparent;
                    panel25.Height = MenuItemHeight;
                    panel25.Dock = DockStyle.Top;
                    panel25.Padding = new Padding(20, 6, 1, 1);
                    var label22 = new Label();
                    label22.BackColor = Color.Transparent;
                    label22.Font = font;
                    label22.Name = "lab" + menu.id;
                    label22.Text = menu.name;
                    label22.Tag = menu.href;
                    label22.Dock = DockStyle.Fill;
                    label22.Click += new EventHandler(TwoLevelMenuClicked);
                    label22.MouseEnter += new EventHandler(TwoLevelMouseEnter);
                    label22.MouseLeave += new EventHandler(TwoLevelMouseLeave);
                    panel25.Controls.Add(label22);
                    panel23.Controls.Add(panel25);
                    panel25.BringToFront();
                }
                panel21.Controls.Add(panel23);
                panel23.BringToFront();
            }
            label21.Click += new EventHandler(OneLevelMenuClicked);
            label21.MouseEnter += new EventHandler(OneLevelMouseEnter);
            label21.MouseLeave += new EventHandler(OneLevelMouseLeave);
            parentPanel.Controls.Add(panel21);
            panel21.BringToFront();
        }

        private String currentMenuName = string.Empty;

        /// <summary>
        /// 一级菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OneLevelMenuClicked(object sender, EventArgs e)
        {
            var item = (Label)sender;
            if (currentMenuName.Equals(item.Name))
            {
                return;
            }
            currentMenuName = item.Name;
            var selectionPanelTop = (PanelEx)item.Parent;
            var selectionPanel = (PanelEx)selectionPanelTop.Parent;
            for (var i = panMenuBar.Controls.Count - 1; i >= 0; i--)
            {
                var control = panMenuBar.Controls[i];
                var panelTop = new PanelEx();
                if (control.Controls.Count == 2)
                {
                    panelTop = (PanelEx)control.Controls[1];
                    var panel = (PanelEx)control.Controls[0];
                    panel.Visible = false;
                }
                else
                {
                    panelTop = (PanelEx)control.Controls[0];
                }
                panelTop.BackColor = Color.Transparent;
                var label = (Label)panelTop.Controls[0];
                label.ForeColor = Color.Black;
            }
            if (selectionPanel.Controls.Count == 2)
            {
                var panel = (PanelEx)selectionPanel.Controls[0];
                foreach (System.Windows.Forms.Control control in panel.Controls)
                {
                    var itemPanel = (PanelEx)control;
                    var itemLabel = (Label)itemPanel.Controls[0];
                    control.BackColor = Color.Transparent;
                    itemLabel.ForeColor = Color.Black;
                }
                panel.Visible = true;
            }

            if (selectionPanelTop.BackColor == Color.Transparent)
            {
                selectionPanelTop.BackColor = Color.FromArgb(57, 61, 73);
                item.ForeColor = Color.White;
            }
            else
            {
                selectionPanelTop.BackColor = Color.Transparent;
                item.ForeColor = Color.Black;
            }
        }

        private String[] param = new String[] { null, null, null };
        private Color OneLevelMouseOriginally = Color.Transparent;

        /// <summary>
        /// 二级菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwoLevelMenuClicked(object sender, EventArgs e)
        {
            var selectionLabel = (Label)sender;
            var selectionPanel = (PanelEx)selectionLabel.Parent;
            var allPanel = (PanelEx)selectionPanel.Parent;
            foreach (System.Windows.Forms.Control control in allPanel.Controls)
            {
                var panel = (PanelEx)control;
                var label = (Label)panel.Controls[0];
                control.BackColor = Color.Transparent;
                label.ForeColor = Color.Black;
            }
            selectionPanel.BackColor = Color.FromArgb(24, 166, 137);
            selectionLabel.ForeColor = Color.White;
            DoWorkAsync(200, (o) =>
            {
                return null;

            }, null, (data) =>
            {
                if (selectionLabel.Tag != null && selectionLabel.Tag.ToString().Length > 0)
                {
                    param[0] = selectionLabel.Tag.ToString();
                    param[1] = selectionLabel.Name;
                    param[2] = selectionLabel.Text;
                }
                else
                {
                    param[0] = null;
                    param[1] = null;
                    param[2] = null;
                }

                var href = param[0];
                var id = param[1];
                var name = param[2];
                if (href != string.Empty && href != null)
                {
                    var i = GetTabName(id);
                    if (i == -1)
                    {
                        var tab = System.Type.GetType("Xr.RtManager." + href);
                        AaddUserControl(tab, id, name);
                    }
                    else
                    {
                        xtraTabControl1.SelectedTabPageIndex = i;
                    }
                }
            });
        }

        /// <summary>
        /// 一级菜单鼠标悬停事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OneLevelMouseEnter(object sender, EventArgs e)
        {
            var label = (Label)sender;
            var selectionPanel = (PanelEx)label.Parent;
            OneLevelMouseOriginally = selectionPanel.BackColor;
            selectionPanel.BackColor = Color.FromArgb(47, 64, 86);
            label.ForeColor = Color.White;
            toolTip1.SetToolTip(label, label.Text);
        }

        /// <summary>
        /// 一级菜单鼠标离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OneLevelMouseLeave(object sender, EventArgs e)
        {
            var label = (Label)sender;
            var selectionPanel = (PanelEx)label.Parent;
            if (selectionPanel.BackColor != Color.FromArgb(57, 61, 73))
            {
                selectionPanel.BackColor = OneLevelMouseOriginally;
            }
            if (selectionPanel.BackColor != Color.FromArgb(47, 64, 86)
                && selectionPanel.BackColor != Color.FromArgb(57, 61, 73))
            {
                label.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// 二级菜单鼠标悬停事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwoLevelMouseEnter(object sender, EventArgs e)
        {
            var label = (Label)sender;
            var selectionPanel = (PanelEx)label.Parent;
            OneLevelMouseOriginally = selectionPanel.BackColor;
            selectionPanel.BackColor = Color.FromArgb(26, 179, 148);
            label.ForeColor = Color.White;
            toolTip1.SetToolTip(label, label.Text);
        }

        /// <summary>
        /// 二级菜单鼠标离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwoLevelMouseLeave(object sender, EventArgs e)
        {
            var label = (Label)sender;
            var selectionPanel = (PanelEx)label.Parent;
            if (selectionPanel.BackColor != Color.FromArgb(24, 166, 137))
            {
                selectionPanel.BackColor = OneLevelMouseOriginally;
            }
            if (selectionPanel.BackColor != Color.FromArgb(24, 166, 137)
                && selectionPanel.BackColor != Color.FromArgb(26, 179, 148))
            {
                label.ForeColor = Color.Black;
            }
        }

        /// <summary>  
        /// 遍历打开的窗口  
        /// </summary>
        /// <param name="value">name值</param>
        /// <returns></returns>
        public int GetTabName(string value)
        {
            var count = -1;
            for (var i = 0; i < xtraTabControl1.TabPages.Count; i++)
            {
                if (xtraTabControl1.TabPages[i].Name == value)
                {
                    return i;
                }
            }
            return count;
        }

        public delegate void ShowDatatableDelegate(XtraTabPage page, UserControl Xuser);
        private void showPage(XtraTabPage page, UserControl Xuser)
        {
            Xuser.BackColor = Color.Transparent;
            var panelE = new PanelEnhanced();
            panelE.BackgroundImage = Properties.Resources.bg2;
            panelE.Dock = DockStyle.Fill;
            panelE.Controls.Add(Xuser);
            page.Controls.Add(panelE);
            xtraTabControl1.TabPages.Add(page);
            xtraTabControl1.SelectedTabPage.ResetBackColor();
            xtraTabControl1.SelectedTabPage.BackColor = Color.Transparent;
            xtraTabControl1.SelectedTabPage = page;
        }

        public delegate void ShowDatatableTypeDelegate(XtraTabPage page, System.Type type );
        private void showPage(XtraTabPage page, System.Type type)
        {
            var panelE = new PanelEnhanced();
            panelE.BackgroundImage = Properties.Resources.bg2;
            panelE.Dock = DockStyle.Fill;
            page.Controls.Add(panelE);
            cmd2 = new Xr.Common.Controls.OpaqueCommand(page);
            cmd2.ShowOpaqueLayer(0f);
            DoWorkAsync(100, (o) =>
            {
                return null;

            }, null, (data) =>
            {
                var uc = (UserControl)Activator.CreateInstance(type);
                uc.BackColor = Color.Transparent;
                setUcUI(null, uc);
                panelE.Controls.Add(uc);
                cmd2.HideOpaqueLayer();
            });
        }
        private void setUcUI(XtraTabPage page, UserControl Xuser)
        {
            Xuser.Parent = this;
            Xuser.Dock = DockStyle.Fill;
        }
        /// <summary>  
        /// 添加到Tab控件里  
        /// </summary>
        /// <param name="Xuser">要添加的用户控件实例</param>
        /// <param name="name"> 控件唯一的 name 属性</param>
        /// <param name="caption">显示标题 caption</param>
        private void AaddUserControl(UserControl Xuser, string name, string caption)
        {
            try
            {
                Invoke(new ShowDatatableDelegate(setUcUI), new object[] { null, Xuser });
                var page = new XtraTabPage();
                page.BackColor = Color.Transparent;
                page.Name = name;
                page.Text = caption;
                Invoke(new ShowDatatableDelegate(showPage), new object[] { page, Xuser });
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
            }
        }
        /// <summary>  
        /// 添加到Tab控件里  
        /// </summary>
        /// <param name="type">要添加的用户控件实例</param>
        /// <param name="name"> 控件唯一的 name 属性</param>
        /// <param name="caption">显示标题 caption</param>
        private void AaddUserControl(System.Type type, string name, string caption)
        {
            try
            {
                var page = new XtraTabPage();
                page.BackColor = Color.Transparent;
                page.Name = name;
                page.Text = caption;

                xtraTabControl1.TabPages.Add(page);
                xtraTabControl1.SelectedTabPage.ResetBackColor();
                xtraTabControl1.SelectedTabPage.BackColor = Color.Transparent;
                xtraTabControl1.SelectedTabPage = page;

                AppContext.Session.waitControl = page;
                Invoke(new ShowDatatableTypeDelegate(showPage), new object[] { page, type });
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
            }
        }
        /// <summary>
        /// 子界面添加新的tab界面的方法（给子界面用的）
        /// </summary>
        /// <param name="name">控件唯一的 name 属性（类名）</param>
        /// <param name="caption">显示标题 caption</param>
        /// <param name="navigationData">参数</param>
        public void JumpInterface(string name, string caption, object navigationData)
        {
            var data = new Dictionary<string, object>();
            data = (Dictionary<String, Object>)navigationData;

            if (name.Equals("RoleDistributionForm"))
            {
                var tab = new RoleDistributionForm();
                tab.id = data["id"].ToString();
                AaddUserControl(tab, name, caption);
            }
            else if (name.Equals("ClientVersionQueryForm"))
            {
                var tab = System.Type.GetType("Xr.RtManager.ClientVersionQueryForm");
                AaddUserControl(tab, name, caption);
            }
        }

        /// <summary>
        /// 关闭当前页，给子页面用的
        /// </summary>
        public void CloseTab()
        {
            var name = xtraTabControl1.SelectedTabPage.Text;
            foreach (XtraTabPage page in xtraTabControl1.TabPages)
            {
                if (page.Text == name)
                {
                    xtraTabControl1.TabPages.Remove(page);
                    page.Dispose();
                    return;
                }
            }
        }

        public void ExitProgram()
        {
            Close();
        }

        /// <summary>
        /// 调用已打开的的方法(提供给子界面用的)
        /// </summary>
        /// <param name="name"></param>
        public void refreshInterface(String name)
        {
        }

        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            var c = (DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs)e;
            var page = (DevExpress.XtraTab.XtraTabPage)c.PrevPage;
            xtraTabControl1.TabPages.Remove(page);
        }

        /// <summary>
        /// 背景图片随窗体的大小而改变大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelEnhanced1_SizeChanged(object sender, EventArgs e)
        {
            loadBackImage();
        }

        private void loadBackImage()
        {
            var bit = new Bitmap(Width, Height);
            var g = Graphics.FromImage(bit);
            g.DrawImage(Properties.Resources.bg, new Rectangle(0, 0, bit.Width, bit.Height), 0, 0, Properties.Resources.bg1.Width, Properties.Resources.bg1.Height, GraphicsUnit.Pixel);
            panelEnhanced1.BackgroundImage = bit;
            g.Dispose();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBoxUtils.Show("你确定退出系统吗?", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, this) == DialogResult.OK)
            {
                var url = AppContext.AppConfig.serverUrl + "logout";
                var data = HttpClass.httpPost(url);
                var objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    try
                    {
                        //退出社保读卡器
                        openT6 = HardwareInitialClass.CloseDevice();
                        if (openT6 != 0)
                        {
                            Xr.Log4net.LogHelper.Info("社保读卡器退出失败:");
                        }
                        else
                        {
                            Xr.Log4net.LogHelper.Info("社保读卡器退出成功");
                        }
                    }
                    catch
                    { }
                    AppContext.Unload();
                    e.Cancel = false;
                }
                else
                {
                    MessageBoxUtils.Show("会话登出失败:" + objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labBottomRight.Text = System.DateTime.Now.ToString();
        }
        private void xtraTabControl_MouseDown(object sender, MouseEventArgs e)
        {
            var hinfo = xtraTabControl1.CalcHitInfo(new Point(e.X, e.Y));
            if (e.Button == MouseButtons.Right && hinfo.Page != null)
            {
                contextMenuStrip1.Show(xtraTabControl1, new Point(e.X, e.Y));
            }
        }

        private void xtraTabControl_MouseUp(object sender, MouseEventArgs e)
        {
        }
        /// <summary>
        /// 关闭当前页签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmCloseCurrent_Click(object sender, EventArgs e)
        {
            var name = xtraTabControl1.SelectedTabPage.Text;
            foreach (XtraTabPage page in xtraTabControl1.TabPages)
            {
                if (page.Text == name)
                {
                    xtraTabControl1.TabPages.Remove(page);
                    page.Dispose();
                    return;
                }
            }
        }
        /// <summary>
        /// 关闭其他页签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmCloseOther_Click(object sender, EventArgs e)
        {
            var index = xtraTabControl1.SelectedTabPageIndex;
            for (var i = xtraTabControl1.TabPages.Count - 1; i >= 0; i--)
            {
                if (i != index)
                {
                    xtraTabControl1.TabPages.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// 关闭全部页签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmCloseAll_Click(object sender, EventArgs e)
        {
            xtraTabControl1.TabPages.Clear();
        }

        /// <summary>
        /// 检查会话 12个小时一次(43200000秒)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmHeartbeat_Tick(object sender, EventArgs e)
        {
            tmHeartbeat.Enabled = false;
            var url = AppContext.AppConfig.serverUrl + "sys/sysUser/getCurrentDate";
            DoWorkAsync(0, (o) =>
            {
                var data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                var objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    tmHeartbeat.Enabled = true;
                }
                else
                {
                    if (MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OKCancel, new [] { "重新登录", "退出系统" }, this) == DialogResult.OK)
                    {
                        Application.Restart();
                    }
                    else
                    {
                        Close();
                    }
                }
            });
        }

        /// <summary>
        /// 多线程异步后台处理某些耗时的数据，不会卡死界面
        /// </summary>
        /// <param name="time">线程延迟多少</param>
        /// <param name="workFunc">Func委托，包装耗时处理（不含UI界面处理），示例：(o)=>{ 具体耗时逻辑; return 处理的结果数据 }</param>
        /// <param name="funcArg">Func委托参数，用于跨线程传递给耗时处理逻辑所需要的对象，示例：String对象、JObject对象或DataTable等任何一个值</param>
        /// <param name="workCompleted">Action委托，包装耗时处理完成后，下步操作（一般是更新界面的数据或UI控件），示列：(r)=>{ datagirdview1.DataSource=r; }</param>
        protected void DoWorkAsync(int time, Func<object, object> workFunc, object funcArg = null, Action<object> workCompleted = null)
        {
            var bgWorkder = new BackgroundWorker();


            bgWorkder.WorkerReportsProgress = true;
            bgWorkder.ProgressChanged += (s, arg) =>
            {
                if (arg.ProgressPercentage > 1)
                {
                    return;
                }
            };

            bgWorkder.RunWorkerCompleted += (s, arg) =>
            {

                try
                {
                    bgWorkder.Dispose();

                    if (workCompleted != null)
                    {
                        workCompleted(arg.Result);
                    }
                }
                catch (Exception ex)
                {
                    cmd.HideOpaqueLayer();
                    if (ex.InnerException == null)
                    {
                        throw new Exception(ex.Message);
                    }
                    else
                    {
                        throw new Exception(ex.InnerException.Message);
                    }
                }
            };

            bgWorkder.DoWork += (s, arg) =>
            {
                bgWorkder.ReportProgress(1);
                var result = workFunc(arg.Argument);
                arg.Result = result;
                bgWorkder.ReportProgress(100);
                Thread.Sleep(time);
            };

            bgWorkder.RunWorkerAsync(funcArg);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(this);
            cmd.rectDisplay = DisplayRectangle;
        }
    }
}
