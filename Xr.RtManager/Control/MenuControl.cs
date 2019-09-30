using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xr.Common;
using Xr.Common.Controls;
using Xr.RtManager.Pages.cms;

namespace Xr.RtManager.Control
{
    public partial class MenuControl : UserControl
    {
        public MenuControl()
        {
            InitializeComponent();
        }

        private List<Item> dataSource { get; set; }
        public bool isSort;//是否进行排序
        public Color borderColor = Color.FromArgb(157, 160, 170);
        public Font font = new Font("微软雅黑", 12);//菜单字体样式
        public   int MenuItemHeight = 34; //菜单选项高度

        //事件处理函数形式，用delegate定义
        public delegate void ItemClick(object sender, EventArgs e);
        //[Browsable(true)]
        //[EditorBrowsable(EditorBrowsableState.Always)]
        public  event ItemClick MenuItemClick;

        public void setDataSource(List<Item> menuList)
        {
            dataSource = menuList;
            if (isSort)
            {
                //排序菜单
                menuList = menuList.OrderBy(x => x.sort).ToList();
            }

            //循环添加菜单
            foreach (Item item in menuList)
            {
                PanelEx itemPanel = new PanelEx();
                itemPanel.BorderColor = borderColor;
                itemPanel.BorderStyleTop = ButtonBorderStyle.None;
                itemPanel.BorderStyleBottom = ButtonBorderStyle.None;
                itemPanel.BorderStyleLeft = ButtonBorderStyle.None;
                itemPanel.BackColor = Color.Transparent;
                itemPanel.Height = MenuItemHeight;
                itemPanel.Dock = DockStyle.Top;
                itemPanel.Padding = new Padding(20, 6, 1, 1);
                Label label = new Label();
                label.BackColor = Color.Transparent;
                label.Font = font;
                label.Name = "lab" + item.value;
                label.Text = item.name;
                label.Tag = item.value;
                label.Dock = DockStyle.Fill;
                label.Click += new EventHandler(MenuClicked);
                label.MouseEnter += new EventHandler(TwoLevelMouseEnter);
                label.MouseLeave += new EventHandler(TwoLevelMouseLeave);
                itemPanel.Controls.Add(label);
                panelEx1.Controls.Add(itemPanel);
                panelEx1.BringToFront();
            }
        }

        /// <summary>
        /// 菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MenuClicked(object sender, EventArgs e)
        {
            Label selectionLabel = (Label)sender;
            PanelEx selectionPanel = (PanelEx)selectionLabel.Parent;
            PanelEx allPanel = (PanelEx)selectionPanel.Parent;
            //当前面板下所有二级菜单背景改为透明,文字颜色改为黑色
            foreach (System.Windows.Forms.Control control in allPanel.Controls)
            {
                PanelEx panel = (PanelEx)control;
                Label label = (Label)panel.Controls[0];
                control.BackColor = Color.Transparent;
                label.ForeColor = Color.Black;
            }
            //修改选择的二级菜单背景色
            selectionPanel.BackColor = Color.FromArgb(24, 166, 137);
            selectionLabel.ForeColor = Color.White;
            if (MenuItemClick != null)
                MenuItemClick(sender, new EventArgs());
        }

        Color MouseOriginally = Color.Transparent;//菜单原色

        /// <summary>
        /// 二级菜单鼠标悬停事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TwoLevelMouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            PanelEx selectionPanel = (PanelEx)label.Parent;
            MouseOriginally = selectionPanel.BackColor;
            selectionPanel.BackColor = Color.FromArgb(26, 179, 148);
            label.ForeColor = Color.White;
        }

        /// <summary>
        /// 二级菜单鼠标离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TwoLevelMouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            PanelEx selectionPanel = (PanelEx)label.Parent;
            if (selectionPanel.BackColor != Color.FromArgb(24, 166, 137))
            {
                selectionPanel.BackColor =MouseOriginally;
            }
            if (selectionPanel.BackColor != Color.FromArgb(24, 166, 137)
                && selectionPanel.BackColor != Color.FromArgb(26, 179, 148))
            {
                label.ForeColor = Color.Black;
            }
        }
    }
    public class Item
    {
        public String value { get; set; }
        public String name { get; set; }
        public String sort { get; set; }
    }
}
