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
using DevExpress.XtraEditors;

namespace Xr.Common.Controls
{
    public partial class MenuControl : UserControl
    {
        public MenuControl()
        {
            InitializeComponent();
            this.panelEx1.HorizontalScroll.Visible = true;
        }

        public String itemName { get; set; }
        public String itemText { get; set; }
        public String itemTag { get; set; }

        private List<Item> dataSource { get; set; }
        public bool isSort;//是否进行排序
        public Color borderColor = Color.FromArgb(157, 160, 170);
        public Font font = new Font("微软雅黑", 10);//菜单字体样式
        public   int MenuItemHeight = 34; //菜单选项高度

        //事件处理函数形式，用delegate定义
        public delegate void ItemClick(object sender, EventArgs e);
        //[Browsable(true)]
        //[EditorBrowsable(EditorBrowsableState.Always)]
        public  event ItemClick MenuItemClick;

        public void setDataSource(List<Item> menuList)
        {
            panelEx1.Controls.Clear();
            labMeasure.Font = font;
            dataSource = menuList;
            if (isSort)
            {
                //排序菜单
                menuList = menuList.OrderBy(x => x.sort).ToList();
            }

            Graphics graphics = CreateGraphics();
            //循环添加菜单
            for (int i = menuList.Count-1; i >= 0; i--)
            {
                Item item = menuList[i];
                PanelEx itemPanel = new PanelEx();
                itemPanel.BorderColor = borderColor;
                itemPanel.BorderStyleTop = ButtonBorderStyle.None;
                itemPanel.BorderStyleBottom = ButtonBorderStyle.None;
                itemPanel.BorderStyleLeft = ButtonBorderStyle.None;

                if (item.spcialBColor != null && item.spcialBColor == "Red")
                {
                    itemPanel.Tag = "Color#RED#";
                    itemPanel.BackColor = Color.FromArgb(180, 219, 112, 147);// Color.Transparent;
                }
                else
                {
                    itemPanel.BackColor = Color.Transparent;
                }
                itemPanel.Height = MenuItemHeight;
                itemPanel.Dock = DockStyle.Top;
                if (item.parentId != null && item.parentId.Length != 0)
                    itemPanel.Padding = new Padding(20, 6, 10, 1);
                else
                    itemPanel.Padding = new Padding(10, 6, 10, 1);
                Label label = new Label();
                label.BackColor = Color.Transparent;
                label.Font = font;
                label.Name = item.value;
                label.Tag = item.tag;
                label.Dock = DockStyle.Fill;
                //label.AutoSize = false;
                label.Text = item.name;
                if (item.Enable)//可设置Item为无效从而不响应事件
                {
                    label.Click += new EventHandler(MenuClicked);
                    label.MouseEnter += new EventHandler(TwoLevelMouseEnter);
                    label.MouseLeave += new EventHandler(TwoLevelMouseLeave);

                    itemPanel.Click += new EventHandler(PanelMenuClicked);
                    itemPanel.MouseEnter += new EventHandler(PanelMouseEnter);
                    itemPanel.MouseLeave += new EventHandler(PanelMouseLeave);
                }
                itemPanel.Controls.Add(label);
                panelEx1.Controls.Add(itemPanel);
                //panelEx1.BringToFront();
                //String name = ""; //重新组织的字符串
                //float currentLineWidth = 0f; //当前行文字宽度
                //float rowWidht = this.Width - 20; //行宽度，不能取label的，不知道什么原因，取label比实际的小
                //int row = 0; //多加的行数
                //int textHeight = 0; //字的高度
                //labMeasure.Text = item.name;
                //for (int i = 0; i < item.name.Length; i++)
                //{
                //    labMeasure.Text = item.name.Substring(i, 1);
                //    textHeight = (int)labMeasure.Height;
                //    currentLineWidth += labMeasure.Width-9; //label左右内边距加起来为9
                //    if (currentLineWidth+9 < rowWidht || currentLineWidth+9 == rowWidht)//比较要加上边距
                //    {
                //        name += item.name.Substring(i, 1);
                //    }
                //    else
                //    {
                //        name += "\r\n" + item.name.Substring(i, 1);
                //        currentLineWidth = 0f;
                //        row += 1; 
                //    }
                //}
                //label.Text = name;
                //itemPanel.Height = MenuItemHeight + textHeight * row;                
            }
        }
        #region 更改上面的添加List<string>类型的数据
        private List<string> dataSources { get; set; }
        public void setDataSources(List<string> menuList, bool BorderColor, string spcialBColor)
        {
            labMeasure.Font = font;
            dataSources = menuList;
            //if (isSort)
            //{
            //    //排序菜单
            //    menuList = menuList.OrderBy(x => x.sort).ToList();
            //}
            if (menuList==null)
            {
                panelEx1.Controls.Clear();
                return;
            }
            Graphics graphics = CreateGraphics();
            panelEx1.Controls.Clear();//先清空一遍 防止重复添加
            //循环添加菜单
            foreach (string item in menuList)
            {
                PanelEx itemPanel = new PanelEx();
                itemPanel.Controls.Clear();
                itemPanel.BorderColor = this.borderColor;
                itemPanel.BorderStyleTop = ButtonBorderStyle.None;
                itemPanel.BorderStyleBottom = ButtonBorderStyle.None;
                itemPanel.BorderStyleLeft = ButtonBorderStyle.None;
                if (spcialBColor != null && spcialBColor == "Red")
                {
                    itemPanel.Tag = "Color#RED#";
                    itemPanel.BackColor = Color.FromArgb(180, 219, 112, 147);// Color.Transparent;
                }
                else
                {
                    itemPanel.BackColor = Color.Transparent;
                }
                itemPanel.Height = MenuItemHeight;
                itemPanel.Dock = DockStyle.Top;
                itemPanel.Padding = new Padding(10, 6, 10, 1);
                Label label = new Label();
                label.BackColor = Color.Transparent;
                label.Font = font;
                //label.Name = item.value;
                //label.Tag = item.tag;
                label.Dock = DockStyle.Fill;
                //label.AutoSize = false;
                label.Text = item.ToString();
                label.Click += new EventHandler(MenuClicked);
                label.MouseEnter += new EventHandler(TwoLevelMouseEnter);
                label.MouseLeave += new EventHandler(TwoLevelMouseLeave);
                itemPanel.Controls.Add(label);
                panelEx1.Controls.Add(itemPanel);
                panelEx1.BringToFront();
                if (BorderColor)
                {
                    this.BorderStyle = BorderStyle.None;
                    panelEx1.BorderStyleBottom = ButtonBorderStyle.None;
                    panelEx1.BorderStyleLeft = ButtonBorderStyle.None;
                    panelEx1.BorderStyleRight = ButtonBorderStyle.None; 
                    panelEx1.BorderStyleTop = ButtonBorderStyle.None; 
                }              
            }
        }
        #endregion 
        
        /// <summary>
        /// 设置当前选中项
        /// </summary>
        /// <param name="value"></param>
        public void EditValue(String value)
        {
            foreach (PanelEx panel in panelEx1.Controls)
            {
                Label label = (Label)panel.Controls[0];
                if (label.Name == value)
                {
                    MenuClicked(label, null);
                    return;
                }
            }
        }

        /// <summary>
        /// 菜单点击事件（panel）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PanelMenuClicked(object sender, EventArgs e)
        {
            PanelEx selectionPanel = (PanelEx)sender;
            Label selectionLabel = (Label)selectionPanel.Controls[0];
            PanelEx allPanel = (PanelEx)selectionPanel.Parent;
            //当前面板下所有二级菜单背景改为透明,文字颜色改为黑色
            foreach (System.Windows.Forms.Control control in allPanel.Controls)
            {
                PanelEx panel = (PanelEx)control;
                Label label = (Label)panel.Controls[0];
                if (panel.Tag!=null&&panel.Tag.ToString().Contains("Color#RED#"))
                {
                    control.BackColor = Color.FromArgb(180, 219, 112, 147);//Color.Transparent;
                }
                else
                {
                    control.BackColor = Color.Transparent;
                }
                label.ForeColor = Color.Black;
            }
            //修改选择的二级菜单背景色
            selectionPanel.BackColor = Color.FromArgb(24, 166, 137);
            selectionLabel.ForeColor = Color.White;
            itemName = selectionLabel.Name;
            itemText = selectionLabel.Text;
            if (selectionLabel.Tag != null)
            {
                itemTag = selectionLabel.Tag.ToString();
            }
            if (MenuItemClick != null)
                MenuItemClick(sender, new EventArgs());
        }
        
        /// <summary>
        /// 菜单点击事件（label）
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
                if (panel.Tag != null && panel.Tag.ToString().Contains("Color#RED#"))
                {
                    control.BackColor = Color.FromArgb(180, 219, 112, 147);//Color.Transparent;
                }
                else
                {
                    control.BackColor = Color.Transparent;
                }
                label.ForeColor = Color.Black;
            }
            //修改选择的二级菜单背景色
            selectionPanel.BackColor = Color.FromArgb(24, 166, 137);
            selectionLabel.ForeColor = Color.White;
            itemName = selectionLabel.Name;
            itemText = selectionLabel.Text;
            if (selectionLabel.Tag != null)
            {
                itemTag = selectionLabel.Tag.ToString();
            }
            if (MenuItemClick != null)
                MenuItemClick(sender, new EventArgs());
        }

        Color MouseOriginally = Color.Transparent;//菜单原色

        /// <summary>
        /// 菜单鼠标悬停事件（panel）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PanelMouseEnter(object sender, EventArgs e)
        {
            PanelEx selectionPanel = (PanelEx)sender;
            Label label = (Label)selectionPanel.Controls[0];
            MouseOriginally = selectionPanel.BackColor;
            selectionPanel.BackColor = Color.FromArgb(26, 179, 148);
            label.ForeColor = Color.White;
            toolTip1.SetToolTip(label, label.Text.Replace("\r\n", ""));
        }

        /// <summary>
        /// 菜单鼠标悬停事件（label）
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
            toolTip1.SetToolTip(label, label.Text.Replace("\r\n", ""));
        }

        /// <summary>
        /// 菜单鼠标离开事件（panel）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PanelMouseLeave(object sender, EventArgs e)
        {
            PanelEx selectionPanel = (PanelEx)sender;
            Label label = (Label)selectionPanel.Controls[0];
            if (selectionPanel.BackColor != Color.FromArgb(24, 166, 137))
            {
                selectionPanel.BackColor = MouseOriginally;
            }
            if (selectionPanel.BackColor != Color.FromArgb(24, 166, 137)
                && selectionPanel.BackColor != Color.FromArgb(26, 179, 148))
            {
                label.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// 菜单鼠标离开事件（label）
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
        private bool enble = true;
        public bool Enable
        {
            get { return enble; }
            set { enble = value; }
        }
        public String value { get; set; }
        public String name { get; set; }
        public String sort { get; set; }
        public String tag { get; set; }
        public String parentId { get; set; }
        public String spcialBColor { get; set; }
    }
}
