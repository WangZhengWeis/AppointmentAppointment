using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Runtime.Remoting;
using System.Configuration;
using Xr.Common;
using DevExpress.XtraNavBar;
using Xr.Http;

namespace Xr.RtManager
{
    public partial class MainFormTwo : Form
    {
        public MainFormTwo()
        {
            InitializeComponent();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        private string dialogText = "正在加载中，请稍候...";

        public delegate void ShowDatatableDelegate(XtraTabPage page, String ucName);

        private void MainForm_Load(object sender, EventArgs e)
        {
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            //LoadInterface("tsmiE1", "医保处理");
            //Common.ShowProcessing(dialogText, this, (obj) =>
            //{
            //    Thread.Sleep(2000); 
            //}, null);
            userName.Text = AppContext.Session.name;
            this.timer1.Start();

            tmHeartbeat.Enabled = true;

            //获取父级为1的菜单
            List<MenuEntity> menuList = new List<MenuEntity>();
            for (int i = 0; i < AppContext.Session.menuList.Count(); i++)
            {
                MenuEntity menu = AppContext.Session.menuList[i];
                if (menu.parentId.Equals("1"))
                {
                    menuList.Add(menu);
                }
            }
            //排序菜单
            menuList = menuList.OrderBy(x => x.sort).ToList();
            //循环添加菜单
            //navBarControl1.Groups.Clear();
            foreach(MenuEntity menu in menuList){
                AddContextMenu(menu.id, menu.name);
            }
        }

        //添加菜单
        private void AddContextMenu(String menuId, String Caption)
        {
            NavBarGroup nbGroup = new NavBarGroup();
            nbGroup.Name = "nbGroup"+menuId;
            nbGroup.Caption = Caption;
            nbGroup.SmallImageIndex = -1;
            nbGroup.LargeImageIndex = -1;
            //添加到导航栏所有分组集合
            navBarControl1.Groups.Add(nbGroup);

            addSubmenu(menuId, Caption, nbGroup);
        }


        private void addSubmenu(String parent, String Caption, NavBarGroup nbGroup1)
        {
            //获取某菜单的下一级所有菜单
            List<MenuEntity> menuList = new List<MenuEntity>();
            foreach (MenuEntity menu in AppContext.Session.menuList)
            {
                if (menu.parentId.Equals(parent))
                {
                    menuList.Add(menu);
                }
            }
            //排序菜单
            menuList = menuList.OrderBy(x => x.sort).ToList();
            //循环添加菜单
            foreach (MenuEntity menu in menuList)
            {
                NavBarItem nbItem1 = new NavBarItem();
                nbItem1.Name = "nbItem" + menu.id;
                nbItem1.Caption = menu.name;
                nbItem1.Tag = menu.href;
                nbItem1.SmallImageIndex = -1;
                nbItem1.LargeImageIndex = -1;
                nbItem1.LinkClicked += Item_Click;
                //添加到导航栏所有子项目集合
                navBarControl1.Items.Add(nbItem1);


                //添加子项目至某一分组
                nbGroup1.ItemLinks.AddRange(new NavBarItemLink[] {
                    new NavBarItemLink(nbItem1)
                });
            }
        }


        /// <summary>
        /// 添加子菜单
        /// </summary>
        /// <param name="text">要显示的文字，如果为 - 则显示为分割线</param>
        /// <param name="tag">用作指向页面</param>
        /// <param name="cms">要添加到的子菜单集合</param>
        /// <param name="callback">点击时触发的事件</param>
        /// <returns>生成的子菜单，如果为分隔条则返回null</returns>

        //ToolStripMenuItem AddContextMenu(String id, string text, string tag, ToolStripItemCollection cms, EventHandler callback)
        //{
        //    if (text == "-")
        //    {
        //        ToolStripSeparator tsp = new ToolStripSeparator();
        //        cms.Add(tsp);
        //        return null;
        //    }
        //    else if (!string.IsNullOrEmpty(text))
        //    {
        //        ToolStripMenuItem tsmi = new ToolStripMenuItem(text);
        //        tsmi.Name = "tsmi"+id;
        //        tsmi.Tag = tag;
        //        if (callback != null) tsmi.Click += callback;
        //        cms.Add(tsmi);

        //        return tsmi;
        //    }

        //    return null;
        //}

        /// <summary>
        /// 菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MenuClicked(object sender, EventArgs e)
        {
            //以下主要是动态生成事件并打开窗体

            //((sender as ToolStripMenuItem).Tag)强制转换

            //ObjectHandle t = Activator.CreateInstance("WinForms", "WinForms.Form2");
            //Form f = (Form)t.Unwrap();
            //f.ShowDialog();
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item.Tag != "" && item.Tag != null)
            {
                int i = GetTabName(item.Name);
                if (i == -1)
                {
                    System.Type tab = System.Type.GetType("Xr.RtManager." + item.Tag);
                    UserControl uc = (UserControl)Activator.CreateInstance(tab);
                    AaddUserControl(uc, item.Name, item.Text);
                }
                else
                {
                    xtraTabControl1.SelectedTabPageIndex = i;
                }
            }
        }

        void Item_Click(object sender, EventArgs e)
        {
            NavBarItem item = (NavBarItem)sender;
            if (item.Tag != "" && item.Tag != null)
            {
                int i = GetTabName(item.Name);
                if (i == -1)
                {
                    System.Type tab = System.Type.GetType("Xr.RtManager." + item.Tag);
                    UserControl uc = (UserControl)Activator.CreateInstance(tab);
                    AaddUserControl(uc, item.Name, item.Caption);
                }
                else
                {
                    xtraTabControl1.SelectedTabPageIndex = i;
                }
            }
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
                XtraTabPage page = new XtraTabPage();
                Xuser.Parent = this;
                page.Name = name;   //控件标示  
                page.Text = caption;  //显示标题  
                Xuser.Dock = System.Windows.Forms.DockStyle.Fill;  //dock属性 全屏撑大  
                page.Controls.Add(Xuser);

                xtraTabControl1.TabPages.Add(page);
                xtraTabControl1.SelectedTabPage = page;  //首页显示  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>  
        /// 遍历打开的窗口  
        /// </summary>  
        /// <param name="value">name值</param>  
        /// <returns></returns>  
        public int GetTabName(string value)
        {
            int count = -1;
            for (int i = 0; i < xtraTabControl1.TabPages.Count; i++)
            {
                if (xtraTabControl1.TabPages[i].Name == value)
                {
                    return i;
                }
            }
            return count;
        }

        /// <summary>
        /// 子界面添加新的tab界面的方法（给子界面用的）
        /// </summary>
        /// <param name="name">控件唯一的 name 属性（类名）</param>
        /// <param name="caption">显示标题 caption</param>
        /// <param name="navigationData">参数</param>
        public void JumpInterface(string name, string caption, object navigationData)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data = (Dictionary<String, Object>)navigationData;
            //this._pkPv = data["pkPv"].ToString();

            if (name.Equals("RoleDistributionForm"))
            {
                RoleDistributionForm tab = new RoleDistributionForm();
                tab.id = data["id"].ToString();
                AaddUserControl(tab, name, caption);
            }
        }

        /// <summary>
        /// 关闭当前页，给子页面用的
        /// </summary>
        public void CloseTab()
        {
            string name = this.xtraTabControl1.SelectedTabPage.Text;//得到关闭的选项卡的text
            foreach (XtraTabPage page in xtraTabControl1.TabPages)//遍历得到和关闭的选项卡一样的Text
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
            this.Close();
        }

        /// <summary>
        /// 调用已打开的的方法(提供给子界面用的)
        /// </summary>
        /// <param name="name"></param>
        public void refreshInterface(String name)
        {
            //foreach (XtraTabPage page in xtraTabControl1.TabPages)//遍历得到和关闭的选项卡一样的Text
            //{
            //    if (page.Text == name)
            //    {
            //        if (name.Equals("入院登记"))
            //        {
            //            DaiRuYuanLieBiao tab = (DaiRuYuanLieBiao)page.Controls[0];
            //            tab.skinButton1_Click(null, null);
            //        }
            //    }
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.SysTime.Text = System.DateTime.Now.ToString();
        }

        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs c = (DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs)e;
            DevExpress.XtraTab.XtraTabPage page = (DevExpress.XtraTab.XtraTabPage)c.PrevPage;
            this.xtraTabControl1.TabPages.Remove(page);
        }

        private void tmHeartbeat_Tick(object sender, EventArgs e)
        {
            tmHeartbeat.Enabled = false;
            String url = AppContext.AppConfig.serverUrl + "getCurrentDate";
            String data = HttpClass.httpPost(url);
            if (!data.Equals("远程服务器返回错误: (404) 未找到。"))
            {
                tmHeartbeat.Enabled = true;
            }
            else
            {
                this.FormClosing -= MainForm_FormClosing;

                if (MessageBoxUtils.Show(data, MessageBoxButtons.OKCancel, new[] { "重新登录", "退出系统" }) == DialogResult.OK)
                {
                    Application.Restart();
                }
                else
                {
                    Close();
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = MessageBoxUtils.Show("是否确定需要退出当前系统？", MessageBoxButtons.YesNo) == DialogResult.No;
        }
    }
}
