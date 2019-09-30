using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xr.Common.Controls;

namespace Xr.RtManager.Control
{
    /// <summary>
    /// 树状菜单栏控件(建议只用两集，多级的父级颜色没有处理)
    /// </summary>
    public partial class TextMenu : PanelEx
    {
        public TextMenu()
        {
            InitializeComponent();
            this.Width = 150;
            this.Height = 300;
            this.AutoScroll = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BorderSize = 1;
            this.BorderColor = borderColor;
            this.BorderStyleTop = ButtonBorderStyle.Solid;
            this.BorderStyleBottom = ButtonBorderStyle.Solid;
            this.BorderStyleRight = ButtonBorderStyle.Solid;
            this.BorderStyleLeft = ButtonBorderStyle.Solid;
        }

        ToolTip tp = new ToolTip();//文字显示工具
        //事件处理函数形式，用delegate定义
        public delegate void ItemClick(object sender, EventArgs e, object selectItem);
        //[Browsable(true)]
        //[EditorBrowsable(EditorBrowsableState.Always)]
        public event ItemClick MenuItemClick;

        private Color borderColor = Color.FromArgb(157, 160, 170);//边框颜色
        Font font = new Font("微软雅黑", 11);//菜单字体样式
        public int MenuItemHeight = 34; //菜单选项高度
        List<dynamic> dataSourceList = new List<dynamic>(); //存所有数据
        private PanelEx oldOneLevelPanel; //老的一级选中面板(其实就是当前选中的面板)
        private Label oldOneLevelLabel; //老的一级选中Label(其实就是当前选中的Label)
        private PanelEx oldPanel; //老的选中面板(其实就是当前选中的面板)
        private Label oldLabel; //老的选中Label(其实就是当前选中的Label)
        private int useLevel = 0; //用于判断目前选中的是一级的还是子级的  分一级子级贼坑 0：没有 1：一级 2：子级

        #region 属性

        /// <summary>
        /// 选中项对应的实体
        /// </summary>
        public object selectItem;
        /// <summary>
        /// 选中的显示值
        /// </summary>
        public String selectText;

        private bool useZoom = true;
        /// <summary>
        /// 是否使用缩放功能
        /// </summary>
        [Browsable(true), Description("是否使用缩放功能"), Category("自定义分组")]
        public bool UseZoom
        {
            get { return useZoom; }
            set
            {
                useZoom = value;
            }
        }

        private String keyFieldName = "id";
        /// <summary>
        /// 设置子集，默认为'id'
        /// </summary>
        [Browsable(true), Description("设置子集，默认为'id'"), Category("自定义分组")]
        public String KeyFieldName
        {
            get { return keyFieldName; }
            set
            {
                keyFieldName = value;
                //this.Invalidate();
            }
        }

        private String parentFieldName = "parentId";
        /// <summary>
        /// 设置父级项，默认为'parentId'
        /// </summary>
        [Browsable(true), Description("设置父级项，默认为'parentId'"), Category("自定义分组")]
        public String ParentFieldName
        {
            get { return parentFieldName; }
            set
            {
                parentFieldName = value;
            }
        }

        private String displayMember = "name";
        /// <summary>
        /// 显示的值
        /// </summary>
        [Browsable(true), Description("设置显示值，默认为'name'"), Category("自定义分组")]
        public String DisplayMember
        {
            get { return displayMember; }
            set
            {
                displayMember = value;
            }
        }

        private String valueMember = "value";
        /// <summary>
        /// 设置当前项，默认为'value'
        /// </summary>
        [Browsable(true), Description("设置当前项，默认为'value'"), Category("自定义分组")]
        public String ValueMember
        {
            get { return valueMember; }
            set
            {
                valueMember = value;
            }
        }

        /// <summary>
        /// 设置或获取选中项（只读）
        /// </summary>
        /// <param name="value"></param>
        public String EditValue
        {
            get
            {
                if (useLevel == 0) return null;
                else if(useLevel == 1) return oldOneLevelLabel.Tag.ToString();
                else if (useLevel == 2) return oldLabel.Tag.ToString();
                else return null;
            }
            //set
            //{
            //    //还原上一次点击的菜单的颜色
            //    if (oldPanel != null)
            //    {
            //        oldPanel.BackColor = Color.Transparent;
            //        oldLabel.ForeColor = Color.Black;
            //    }
            //    //保存当前菜单的数据
            //    foreach (dynamic menu in dataSourceList)
            //    {
            //        String itemValue = menu.GetType().GetProperty(valueMember).GetValue(menu, null);
            //        if (itemValue.Equals(value))
            //        {
            //            selectText = menu.GetType().GetProperty(displayMember).GetValue(menu, null);
            //            selectItem = menu;
            //            break;
            //        }
            //    }
            //    setEditValue(this, value);
            //}
        }

        #region 一级菜单背景、文字各种颜色设置(默认、点击、悬浮)
        private Color oneLevelBgColor = Color.Transparent;
        /// <summary>
        /// 一级菜单背景默认颜色
        /// </summary>
        [Browsable(true), Description("设置一级菜单背景默认颜色，默认为'Transparent'"), Category("自定义分组")]
        public Color OneLevelBgColor
        {
            get { return oneLevelBgColor; }
            set
            {
                oneLevelBgColor = value;
            }
        }

        private Color oneLevelTextColor = Color.Black;
        /// <summary>
        /// 一级菜单文字默认颜色
        /// </summary>
        [Browsable(true), Description("设置一级文字背景默认颜色，默认为'Black;'"), Category("自定义分组")]
        public Color OneLevelTextColor
        {
            get { return oneLevelTextColor; }
            set
            {
                oneLevelTextColor = value;
            }
        }

        private Color oneLevelBgClickColor = Color.FromArgb(57, 61, 73);
        /// <summary>
        /// 一级菜单点击色
        /// </summary>
        [Browsable(true), Description("设置一级菜单背景点击色，默认为'57, 61, 73'"), Category("自定义分组")]
        public Color OneLevelBgClickColor
        {
            get { return oneLevelBgClickColor; }
            set
            {
                oneLevelBgClickColor = value;
            }
        }

        private Color oneLevelTextClickColor = Color.White;
        /// <summary>
        /// 一级菜单文字点击色
        /// </summary>
        [Browsable(true), Description("设置一级菜单文字点击色，默认为'White'"), Category("自定义分组")]
        public Color OneLevelTextClickColor
        {
            get { return oneLevelTextClickColor; }
            set
            {
                oneLevelTextClickColor = value;
            }
        }

        private Color oneLevelBgSuspensionColor = Color.FromArgb(47, 64, 86);
        /// <summary>
        /// 一级菜单背景悬浮色
        /// </summary>
        [Browsable(true), Description("设置一级菜单背景悬浮色，默认为'47, 64, 86'"), Category("自定义分组")]
        public Color OneLevelBgSuspensionColor
        {
            get { return oneLevelBgSuspensionColor; }
            set
            {
                oneLevelBgSuspensionColor = value;
            }
        }

        private Color oneLevelTextSuspensionColor = Color.White;
        /// <summary>
        /// 一级菜单文字悬浮色
        /// </summary>
        [Browsable(true), Description("设置一级菜单文字悬浮色，默认为'White'"), Category("自定义分组")]
        public Color OneLevelTextSuspensionColor
        {
            get { return oneLevelTextSuspensionColor; }
            set
            {
                oneLevelTextSuspensionColor = value;
            }
        }
        #endregion

        #region 子级菜单(不包括第一级)背景、文字各种颜色设置(默认、点击、悬浮)
        private Color menuBgColor = Color.Transparent;
        /// <summary>
        /// 菜单背景默认颜色
        /// </summary>
        [Browsable(true), Description("设置菜单背景默认颜色，默认为'Transparent'"), Category("自定义分组")]
        public Color MenuBgColor
        {
            get { return menuBgColor; }
            set
            {
                menuBgColor = value;
            }
        }

        private Color menuTextColor = Color.Black;
        /// <summary>
        /// 菜单文字默认颜色
        /// </summary>
        [Browsable(true), Description("设置菜单文字默认颜色，默认为'Black'"), Category("自定义分组")]
        public Color MenuTextColor
        {
            get { return menuTextColor; }
            set
            {
                menuTextColor = value;
            }
        }

        private Color menuBgClickColor = Color.FromArgb(24, 166, 137);
        /// <summary>
        /// 菜单背景点击色(不包括一级菜单)
        /// </summary>
        [Browsable(true), Description("设置一级菜单背景点击色(不包括一级菜单)，默认为'24, 166, 137'"), Category("自定义分组")]
        public Color MenuBgClickColor
        {
            get { return menuBgClickColor; }
            set
            {
                 menuBgClickColor = value;
            }
        }

        private Color menuTextClickColor = Color.White;
        /// <summary>
        /// 菜单文字点击颜色
        /// </summary>
        [Browsable(true), Description("设置菜单文字点击颜色，默认为'White'"), Category("自定义分组")]
        public Color MenuTextClickColor
        {
            get { return menuTextClickColor; }
            set
            {
                menuTextClickColor = value;
            }
        }

        private Color menuBgSuspensionColor = Color.FromArgb(26, 179, 148);
        /// <summary>
        /// 菜单背景悬浮色(不包括一级菜单)
        /// </summary>
        [Browsable(true), Description("设置菜单背景悬浮色(不包括一级菜单)，默认为'26, 179, 148'"), Category("自定义分组")]
        public Color MenuBgSuspensionColor
        {
            get { return menuBgSuspensionColor; }
            set
            {
                menuBgSuspensionColor = value;
            }
        }

        private Color menuTextSuspensionColor = Color.Black;
        /// <summary>
        /// 菜单文字悬浮颜色
        /// </summary>
        [Browsable(true), Description("设置菜单文字悬浮颜色，默认为'Black'"), Category("自定义分组")]
        public Color MenuTextSuspensionColor
        {
            get { return menuTextSuspensionColor; }
            set
            {
                menuTextSuspensionColor = value;
            }
        }
        #endregion
        ///// <summary>
        ///// 设置选择项
        ///// </summary>
        ///// <param name="kj"></param>
        ///// <param name="value"></param>
        //private void setEditValue(System.Windows.Forms.Control kj, String value)
        //{
        //    foreach (System.Windows.Forms.Control control in kj.Controls)
        //    {
        //        PanelEx p1;
        //        if (control.Controls.Count == 1) p1 = (PanelEx)control.Controls[0]; //没有子集的时候只有只有一个子控件
        //        else p1 = (PanelEx)control.Controls[1];
        //        Label l1 = (Label)p1.Controls[0];
        //        if (l1.Tag.ToString().Equals(value))
        //        {
        //            p1.BackColor = Color.FromArgb(24, 166, 137);
        //            l1.ForeColor = Color.White;
        //            oldPanel = p1;
        //            oldLabel = l1;
        //            return;
        //        }
        //        if (control.Controls.Count == 2)//等于2就是有子集，有子集就调用自己
        //            setEditValue(control.Controls[0], value);
        //    }
        //}

        private object dataSource;
        /// <summary>
        /// 数据源
        /// </summary>
        public object DataSource
        {
            get { return dataSource; }
            set
            {
                if (((value != null) && !(value is IListSource)) && !(value is IEnumerable))
                {
                    throw new ArgumentException("Invalid_DataSource_Type");
                }
                if (value != dataSource)
                {
                    this.dataSource = value;
                    createUI();
                }
            }
        }

        #endregion

        int menuLevel = 1;//菜单级别

        /// <summary>
        /// 刷新数据源
        /// </summary>
        public void RefreshDataSource()
        {
            createUI();
        }

        /// <summary>
        /// 根据数据源生成界面
        /// </summary>
        private void createUI()
        {
            this.Controls.Clear();
            string type = dataSource.GetType().Name;
            switch (type)
            {
                case "DataSet":
                    DataSet ds = (DataSet)dataSource;
                    throw new ArgumentException("目前还不支持DataSet类型");
                //if (ds.Check())
                //{
                //    rp.DataSource = ds.Tables[0];
                //}
                //break;
                case "DataTable":
                    DataTable dt = (DataTable)dataSource;
                    throw new ArgumentException("目前还不支持DataTable类型");
                //if (dt.Rows.Count > 0)
                //{
                //    rp.DataSource = dt;
                //}
                //break;
                case "List`1":
                    //第一步：找出一级菜单列表
                    List<dynamic> dyList = new List<dynamic>();//存一级菜单
                    IEnumerable list = dataSource as IEnumerable;
                    foreach (var item in list)
                    {
                        dynamic temp = item;
                        dataSourceList.Add(item);
                        if (ifTopLevel(temp))
                        {
                            dyList.Add(temp);
                        }
                    }
                    //第二步：生成菜单控件
                    //遍历生成一级菜单
                    foreach (dynamic entity in dyList)
                    {
                        //选项，包括该选项的所有字菜单
                        PanelEx p1 = new PanelEx();
                        p1.BorderColor = borderColor;
                        p1.BorderStyleTop = ButtonBorderStyle.None;
                        p1.BorderStyleBottom = ButtonBorderStyle.None;
                        p1.BorderStyleRight = ButtonBorderStyle.None;
                        p1.BorderStyleLeft = ButtonBorderStyle.None;
                        p1.BackColor = Color.Transparent;
                        p1.AutoSize = true;
                        p1.Dock = DockStyle.Top;
                        //当前选项头
                        PanelEx p2 = new PanelEx();
                        p2.BackColor = oneLevelBgColor;
                        p2.BorderColor = borderColor;
                        p2.BorderStyleTop = ButtonBorderStyle.None;
                        p2.BorderStyleLeft = ButtonBorderStyle.None;
                        p2.BorderStyleRight = ButtonBorderStyle.None;
                        p2.BorderStyleBottom = ButtonBorderStyle.None;
                        p2.Margin = new Padding(0, 0, 0, 0);
                        p2.Padding = new Padding(0, 6, 0, 0);
                        p2.Height = MenuItemHeight;
                        p2.Dock = DockStyle.Top;

                        //当前选项文本
                        Label l2 = new Label();
                        l2.ForeColor = oneLevelTextColor;
                        l2.Text = entity.GetType().GetProperty(displayMember).GetValue(entity, null);//菜单显示的值
                        l2.Tag = entity.GetType().GetProperty(valueMember).GetValue(entity, null);//菜单对应的值
                        l2.Font = font;
                        l2.Dock = DockStyle.Left;
                        l2.MouseEnter += new EventHandler(OneLevelLabelMouseEnter);
                        l2.MouseLeave += new EventHandler(OneLevelLabelMouseLeave);
                        l2.Click += new EventHandler(OneLevelLabelMenuClicked);
                        p2.MouseEnter += new EventHandler(OneLevelPanelMouseEnter);
                        p2.MouseLeave += new EventHandler(OneLevelPanelMouseLeave);
                        p2.Click += new EventHandler(OneLevelPanelMenuClicked);
                        p2.Controls.Add(l2);
                        l2.BringToFront();
                        p1.Controls.Add(p2);

                        //创建下级菜单
                        menuLevel++;
                        String current = entity.GetType().GetProperty(keyFieldName).GetValue(entity, null);
                        createSubsetMenu(current, p1, p2);
                        menuLevel = 1;

                        this.Controls.Add(p1);
                        p1.BringToFront();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 创建下级菜单
        /// </summary>
        /// <param name="current">下级菜单的父节点</param>
        /// <param name="ParentPanel">下级菜单所在控件</param>
        /// <param name="ParentTop">下级菜单所在控件的头部</param>
        private void createSubsetMenu(String current, PanelEx ParentPanel, PanelEx ParentTop)
        {
            int currentMenuLevel = menuLevel;//记录当前菜单级别
            //找子集
            List<dynamic> menuList = new List<dynamic>();//存子集
            foreach (dynamic menu in dataSourceList)
            {
                String menuParent = menu.GetType().GetProperty(parentFieldName).GetValue(menu, null);
                if (menuParent != null && menuParent.Equals(current))
                {
                    menuList.Add(menu);
                }
            }
            if (menuList.Count <= 0)
            {
                Padding pd = ParentTop.Padding;
                int left = pd.Left;
                if (useZoom) left = pd.Left + 15;
                ParentTop.Padding = new Padding(left, pd.Top, pd.Right, pd.Bottom);
                return;
            }
            else if (menuList.Count > 0)
            {
                //子菜单面板
                PanelEx p1 = new PanelEx();
                p1.BorderStyleTop = ButtonBorderStyle.None;
                p1.BorderStyleBottom = ButtonBorderStyle.None;
                p1.BorderStyleLeft = ButtonBorderStyle.None;
                p1.BorderStyleRight = ButtonBorderStyle.None;
                p1.BorderColor = Color.FromArgb(157, 160, 170);
                p1.Margin = new Padding(0, 0, 0, 0);
                p1.Padding = new Padding(0, 0, 0, 0);
                p1.Visible = false;
                p1.AutoSize = true;
                p1.Dock = DockStyle.Top;

                //循环添加菜单
                foreach (dynamic menu in menuList)
                {
                    //子级菜单
                    PanelEx p2 = new PanelEx();//某个子集的面板
                    p2.BorderColor = borderColor;
                    p2.BorderStyleTop = ButtonBorderStyle.None;
                    p2.BorderStyleBottom = ButtonBorderStyle.None;
                    p2.BorderStyleRight = ButtonBorderStyle.None;
                    p2.BorderStyleLeft = ButtonBorderStyle.None;
                    p2.BackColor = Color.Transparent;
                    p2.AutoSize = true;
                    p2.Dock = DockStyle.Top;

                    PanelEx p3 = new PanelEx();//某个子集的选项头
                    p3.BackColor = oneLevelBgColor;
                    p3.BorderColor = borderColor;
                    p3.BorderStyleTop = ButtonBorderStyle.None;
                    p3.BorderStyleBottom = ButtonBorderStyle.None;
                    p3.BorderStyleLeft = ButtonBorderStyle.None;
                    p3.BorderStyleRight = ButtonBorderStyle.None;
                    p3.BackColor = Color.Transparent;
                    p3.Height = MenuItemHeight;
                    p3.Dock = DockStyle.Top;

                    p3.Padding = new Padding((menuLevel - 1) * 15, 6, 1, 1);

                    Label l2 = new Label();
                    l2.ForeColor = oneLevelTextColor;
                    l2.Font = font;
                    l2.Text = menu.GetType().GetProperty(displayMember).GetValue(menu, null);//菜单显示的值
                    l2.Tag = menu.GetType().GetProperty(valueMember).GetValue(menu, null);//菜单对应的值
                    l2.Dock = DockStyle.Left;
                    l2.MouseEnter += new EventHandler(LabelMouseEnter);
                    l2.MouseLeave += new EventHandler(LabelMouseLeave);
                    l2.Click += new EventHandler(LabelMenuClicked);
                    p3.MouseEnter += new EventHandler(PanelMouseEnter);
                    p3.MouseLeave += new EventHandler(PanelMouseLeave);
                    p3.Click += new EventHandler(PanelMenuClicked);
                    p3.Controls.Add(l2);
                    l2.BringToFront();
                    p2.Controls.Add(p3);
                    p1.Controls.Add(p2);
                    p2.BringToFront();

                    //创建下级菜单
                    menuLevel++;
                    current = menu.GetType().GetProperty(keyFieldName).GetValue(menu, null);
                    createSubsetMenu(current, p2, p3);
                    menuLevel = currentMenuLevel;
                }
                ParentPanel.Controls.Add(p1);
                p1.BringToFront();
            }
        }

        /// <summary>
        /// 判断是否是顶级元素
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private bool ifTopLevel(dynamic temp)
        {
            String parent = temp.GetType().GetProperty(parentFieldName).GetValue(temp, null);
            if (parent == null || parent.Trim().Length == 0)
            {
                return true;
            }
            IEnumerable list = dataSource as IEnumerable;
            foreach (var item in list)
            {
                dynamic entity = item;
                //听过反射效能很差(不理解)
                String key = entity.GetType().GetProperty(keyFieldName).GetValue(entity, null);
                if (key.Equals(parent))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取顶级元素
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private dynamic getTopLevel(dynamic temp)
        {
            String parent = temp.GetType().GetProperty(parentFieldName).GetValue(temp, null);
            if (parent == null || parent.Trim().Length == 0)
            {
                return temp;//本身就是顶级元素
            }
            IEnumerable list = dataSource as IEnumerable;
            foreach (var item in list)
            {
                dynamic entity = item;
                //听过反射效能很差(不理解)
                String key = entity.GetType().GetProperty(keyFieldName).GetValue(entity, null);
                if (key.Equals(parent))
                {
                    temp = getTopLevel(entity);
                    break;
                }
            }
            //遍历不到匹配的，本身就是顶级元素
            return temp;
        }

        /// <summary>
        /// 下拉收缩点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpansionContractionClicked(object sender, EventArgs e)
        {
            //下级菜单的显示隐藏
            Label l1 = (Label)sender;
            PanelEx p1 = (PanelEx)l1.Parent;
            PanelEx p2 = (PanelEx)p1.Parent;
            PanelEx p3 = (PanelEx)p2.Controls[0];//因为添加控件使用了BringToFront，所以顺序要反着算
            if (p3.Visible)
            {
                l1.Text = "+";
            }
            else
            {
                l1.Text = "-";
            }
            p3.Visible = !p3.Visible;
        }

        Color OneLevelBgMouseOriginally = Color.Transparent;//一级菜单背景原色
        Color OneLevelTextMouseOriginally = Color.Transparent;//一级菜文字单原色
        Color bgMouseOriginally = Color.Transparent;//菜单背景原色
        Color textMouseOriginally = Color.Transparent;//菜单文字原色

        #region 一级菜单相关事件(点击、悬浮、离开)

        /// <summary>
        /// 一级菜单鼠标悬停事件（label）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OneLevelLabelMouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            PanelEx selectionPanel = (PanelEx)label.Parent;
            //不等于悬浮色修改是为了尽量减少对界面的修改;不等于点击色修改，点击色不需要悬浮效果
            if (selectionPanel.BackColor != oneLevelBgSuspensionColor && selectionPanel.BackColor != oneLevelBgClickColor)
            {
                OneLevelBgMouseOriginally = selectionPanel.BackColor;
                OneLevelTextMouseOriginally = label.ForeColor;
                selectionPanel.BackColor = oneLevelBgSuspensionColor;
                label.ForeColor = oneLevelTextSuspensionColor;
            }
            tp.SetToolTip(label, label.Text.Replace("\r\n", ""));
        }

        /// <summary>
        /// 一级菜单鼠标离开事件（label）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OneLevelLabelMouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            PanelEx selectionPanel = (PanelEx)label.Parent;
            //不是选中的菜单 背景和文字就恢复原来的颜色
            if (selectionPanel.BackColor != oneLevelBgClickColor)
            {
                if (selectionPanel.BackColor != OneLevelBgMouseOriginally)
                    selectionPanel.BackColor = OneLevelBgMouseOriginally;
                if (label.ForeColor != OneLevelTextMouseOriginally)
                    label.ForeColor = OneLevelTextMouseOriginally;
            }
        }

        /// <summary>
        /// 一级菜单点击事件（label）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OneLevelLabelMenuClicked(object sender, EventArgs e)
        {
            Label selectionLabel = (Label)sender; //选中的Label
            PanelEx selectionPanel = (PanelEx)selectionLabel.Parent; //选中的面板

            useLevel = 1;
            //上一个选中的父级菜单颜色还原
            if (oldOneLevelPanel != null)
            {
                oldOneLevelPanel.BackColor = oneLevelBgColor;
                oldOneLevelLabel.ForeColor = oneLevelTextColor;
            }

            //上一个选中的子级菜单颜色还原
            if (oldPanel != null)
            {
                oldPanel.BackColor = menuBgColor;
                oldLabel.ForeColor = menuTextColor;
            }

            //隐藏之前的下拉面板
            if (oldOneLevelPanel != null)
            {
                PanelEx selectionOldPanel = (PanelEx)oldOneLevelPanel.Parent; //上一个选中的面板
                if (selectionOldPanel.Controls.Count > 1)
                {
                    PanelEx xlPan = (PanelEx)selectionOldPanel.Controls[0];//因为添加控件使用了BringToFront，所以顺序要反着算
                    xlPan.Visible = false;
                }
            }
            
            //修改选择的菜单颜色
            selectionPanel.BackColor = oneLevelBgClickColor;
            selectionLabel.ForeColor = oneLevelTextClickColor;

            //保存当前选中的数据
            oldOneLevelPanel = selectionPanel;
            oldOneLevelLabel = selectionLabel;
            foreach (dynamic menu in dataSourceList)
            {
                String itemValue = menu.GetType().GetProperty(valueMember).GetValue(menu, null);
                if (itemValue.Equals(selectionLabel.Tag.ToString()))
                {
                    selectText = menu.GetType().GetProperty(displayMember).GetValue(menu, null);
                    selectItem = menu;
                    break;
                }
            }

            //下级菜单的显示隐藏
            PanelEx menuPan = (PanelEx)selectionPanel.Parent;
            if (menuPan.Controls.Count > 1)
            {
                PanelEx xlPan = (PanelEx)menuPan.Controls[0];//因为添加控件使用了BringToFront，所以顺序要反着算
                xlPan.Visible = !xlPan.Visible;
            }

            //使用本控件的点击事件
            if (MenuItemClick != null)
            {
                MenuItemClick(sender, new EventArgs(), selectItem);
            }
        }

        /// <summary>
        /// 一级菜单鼠标悬停事件（panel）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OneLevelPanelMouseEnter(object sender, EventArgs e)
        {
            PanelEx selectionPanel = (PanelEx)sender;
            Label label = (Label)selectionPanel.Controls[0];
            //不等于悬浮色修改是为了尽量减少对界面的修改;不等于点击色修改，点击色不需要悬浮效果
            if (selectionPanel.BackColor != oneLevelBgSuspensionColor && selectionPanel.BackColor != oneLevelBgClickColor)
            {
                OneLevelBgMouseOriginally = selectionPanel.BackColor;
                OneLevelTextMouseOriginally = label.ForeColor;
                selectionPanel.BackColor = oneLevelBgSuspensionColor;
                label.ForeColor = oneLevelTextSuspensionColor;
            }
            tp.SetToolTip(label, label.Text.Replace("\r\n", ""));
        }

        /// <summary>
        /// 一级菜单鼠标离开事件（panel）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OneLevelPanelMouseLeave(object sender, EventArgs e)
        {
            PanelEx selectionPanel = (PanelEx)sender;
            Label label = (Label)selectionPanel.Controls[0];
            //不是选中的菜单背景和文字都恢复原来的颜色
            if (selectionPanel.BackColor != oneLevelBgClickColor)
            {
                if (selectionPanel.BackColor != OneLevelBgMouseOriginally)
                    selectionPanel.BackColor = OneLevelBgMouseOriginally;
                if (label.ForeColor != OneLevelTextMouseOriginally)
                    label.ForeColor = OneLevelTextMouseOriginally;
            }
        }

        /// <summary>
        /// 一级菜单点击事件（panel）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OneLevelPanelMenuClicked(object sender, EventArgs e)
        {
            PanelEx selectionPanel = (PanelEx)sender;//选中的Label
            Label selectionLabel = (Label)selectionPanel.Controls[0];//选中的面板

            useLevel = 1;
            //上一个选中的父级菜单颜色还原
            if (oldOneLevelPanel != null)
            {
                oldOneLevelPanel.BackColor = oneLevelBgColor;
                oldOneLevelLabel.ForeColor = oneLevelTextColor;
            }

            //上一个选中的子级菜单颜色还原
            if (oldPanel != null)
            {
                oldPanel.BackColor = menuBgColor;
                oldLabel.ForeColor = menuTextColor;
            }

            //隐藏之前的下拉面板
            if (oldOneLevelPanel != null)
            {
                PanelEx selectionOldPanel = (PanelEx)oldOneLevelPanel.Parent; //上一个选中的面板
                if (selectionOldPanel.Controls.Count > 1)
                {
                    PanelEx xlPan = (PanelEx)selectionOldPanel.Controls[0];//因为添加控件使用了BringToFront，所以顺序要反着算
                    xlPan.Visible = false;
                }
            }

            //修改选择的菜单颜色
            selectionPanel.BackColor = oneLevelBgClickColor;
            selectionLabel.ForeColor = oneLevelTextClickColor;

            //保存当前选中的数据
            oldOneLevelPanel = selectionPanel;
            oldOneLevelLabel = selectionLabel;
            foreach (dynamic menu in dataSourceList)
            {
                String itemValue = menu.GetType().GetProperty(valueMember).GetValue(menu, null);
                if (itemValue.Equals(selectionLabel.Tag.ToString()))
                {
                    selectText = menu.GetType().GetProperty(displayMember).GetValue(menu, null);
                    selectItem = menu;
                    break;
                }
            }

            //下级菜单的显示隐藏
            PanelEx menuPan = (PanelEx)selectionPanel.Parent;
            if (menuPan.Controls.Count > 1)
            {
                PanelEx xlPan = (PanelEx)menuPan.Controls[0];//因为添加控件使用了BringToFront，所以顺序要反着算
                xlPan.Visible = !xlPan.Visible;
            }

            //使用本控件的点击事件
            if (MenuItemClick != null)
            {
                MenuItemClick(sender, new EventArgs(), selectItem);
            }
        }


        #endregion

        #region 菜单相关事件(点击、悬浮、离开，不包括一级菜单)

        /// <summary>
        /// 菜单鼠标悬停事件（label）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LabelMouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            PanelEx selectionPanel = (PanelEx)label.Parent;
            if (selectionPanel.BackColor != menuBgSuspensionColor)//尽量减少对界面的修改
            {
                bgMouseOriginally = selectionPanel.BackColor;
                textMouseOriginally = label.ForeColor;
                selectionPanel.BackColor = menuBgSuspensionColor;
                label.ForeColor = menuTextSuspensionColor;
            }
            tp.SetToolTip(label, label.Text.Replace("\r\n", ""));
        }

        /// <summary>
        /// 菜单鼠标离开事件（label）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LabelMouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            PanelEx selectionPanel = (PanelEx)label.Parent;
            //不是选中的菜单,背景和文字就恢复原色
            if (selectionPanel.BackColor != Color.FromArgb(24, 166, 137))
            {
                if (selectionPanel.BackColor != bgMouseOriginally)
                    selectionPanel.BackColor = bgMouseOriginally;
                if (label.ForeColor != textMouseOriginally)
                    label.ForeColor = textMouseOriginally;
            }
        }

        /// <summary>
        /// 菜单鼠标悬停事件（panel）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PanelMouseEnter(object sender, EventArgs e)
        {
            PanelEx selectionPanel = (PanelEx)sender;
            Label label = (Label)selectionPanel.Controls[0];
            if (selectionPanel.BackColor != menuBgSuspensionColor)
            {
                bgMouseOriginally = selectionPanel.BackColor;
                textMouseOriginally = label.ForeColor;
                selectionPanel.BackColor = menuBgSuspensionColor;
                label.ForeColor = menuTextSuspensionColor;
            }
            tp.SetToolTip(label, label.Text.Replace("\r\n", ""));
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
            //不是选中的菜单,背景和文字就恢复原色
            if (selectionPanel.BackColor != Color.FromArgb(24, 166, 137))
            {
                if (selectionPanel.BackColor != bgMouseOriginally)
                    selectionPanel.BackColor = bgMouseOriginally;
                if (label.ForeColor != textMouseOriginally)
                    label.ForeColor = textMouseOriginally;
            }
        }

        /// <summary>
        /// 菜单点击事件（label）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LabelMenuClicked(object sender, EventArgs e)
        {
            Label selectionLabel = (Label)sender; //选中的Label
            PanelEx selectionPanel = (PanelEx)selectionLabel.Parent; //选中的面板

            //上一个选中的菜单颜色还原
            if (oldPanel != null)
            {
                oldPanel.BackColor = menuBgColor;
                oldLabel.ForeColor = menuTextColor;
            }

            //修改选择的菜单颜色
            selectionPanel.BackColor = menuBgClickColor;
            selectionLabel.ForeColor = menuTextColor;

            //保存当前选中的数据
            oldPanel = selectionPanel;
            oldLabel = selectionLabel;
            foreach (dynamic menu in dataSourceList)
            {
                String itemValue = menu.GetType().GetProperty(valueMember).GetValue(menu, null);
                if (itemValue.Equals(selectionLabel.Tag.ToString()))
                {
                    selectText = menu.GetType().GetProperty(displayMember).GetValue(menu, null);
                    selectItem = menu;
                    break;
                }
            }

            //下级菜单的显示隐藏
            PanelEx menuPan = (PanelEx)selectionPanel.Parent;
            if (menuPan.Controls.Count > 1)
            {
                PanelEx xlPan = (PanelEx)menuPan.Controls[0];//因为添加控件使用了BringToFront，所以顺序要反着算
                xlPan.Visible = !xlPan.Visible;
            }

            //使用本控件的点击事件
            if (MenuItemClick != null)
            {
                MenuItemClick(sender, new EventArgs(), selectItem);
            }
        }

        /// <summary>
        /// 菜单点击事件（panel）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PanelMenuClicked(object sender, EventArgs e)
        {
            PanelEx selectionPanel = (PanelEx)sender;//选中的Label
            Label selectionLabel = (Label)selectionPanel.Controls[0];//选中的面板

            //上一个选中的菜单颜色还原
            if (oldPanel != null)
            {
                oldPanel.BackColor = menuBgColor;
                oldLabel.ForeColor = menuTextColor;
            }
            //修改选择的菜单颜色
            selectionPanel.BackColor = menuBgClickColor;
            selectionLabel.ForeColor = menuTextColor;

            //保存当前选中的数据
            oldPanel = selectionPanel;
            oldLabel = selectionLabel;
            foreach (dynamic menu in dataSourceList)
            {
                String itemValue = menu.GetType().GetProperty(valueMember).GetValue(menu, null);
                if (itemValue.Equals(selectionLabel.Tag.ToString()))
                {
                    selectText = menu.GetType().GetProperty(displayMember).GetValue(menu, null);
                    selectItem = menu;
                    break;
                }
            }

            PanelEx menuPan = (PanelEx)selectionPanel.Parent;
            if (menuPan.Controls.Count > 1)
            {
                PanelEx xlPan = (PanelEx)menuPan.Controls[0];//因为添加控件使用了BringToFront，所以顺序要反着算
                xlPan.Visible = !xlPan.Visible;
            }

            //使用本控件的点击事件
            if (MenuItemClick != null)
            {
                MenuItemClick(sender, new EventArgs(), selectItem);
            }
        }
    
        #endregion
    }
}
