using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Xr.Common.Controls;
using Xr.Common.Properties;
using Xr.Common.Utils;

namespace Xr.Common
{
    /// <summary>
    /// 表示NHIS窗体基类
    /// </summary>
    public class BaseForm : Form
    {
        private const int Radius = 5;            //窗体圆角半角
        private const int TitleHeight = 30;      //标题栏高度
        private const int TitleButtonWith = 30;  //标题栏按钮宽度

        //边框颜色
        private static readonly Pen[] BorderPens = new Pen[] {
            //new Pen(Color.FromArgb(226, 228, 230)),
            new Pen(Color.FromArgb(193, 198, 201)),            
            new Pen(Color.FromArgb(42, 131, 113))
            //new Pen(Color.FromArgb(193, 198, 201))
        };

        private static readonly Brush cornerBrush1 = new SolidBrush(BorderPens[BorderPens.Length - 2].Color);    //边框圆角颜色1
        private static readonly Brush cornerBrush2 = new SolidBrush(BorderPens[BorderPens.Length - 1].Color);    //边框圆角颜色2
        private static readonly Brush CloseButtonHoverBrush = new SolidBrush(Color.FromArgb(231, 0, 0));         //关闭按钮悬停颜色
        private static readonly Brush TitleBrush = new SolidBrush(Color.FromArgb(42, 131, 113));                 //标题栏颜色 
        //private static readonly Brush TitleBrush = new SolidBrush(Color.FromArgb(159, 186, 214));                 //标题栏颜色 
        private static readonly Font TitleFont = new Font("微软雅黑", 13, FontStyle.Bold, GraphicsUnit.Pixel);   //标题栏字体

        private Rectangle closeButtonRect = Rectangle.Empty;  //关闭按钮所在的区域
        private Point mouseLocation = Point.Empty;
        private bool mouseLeftButtonDown = false;
        private Point mouseLeftButtonDownLocation;
        private HotkeysManager hotkeysManager;
        //private DictLookUpEditManager lookUpEditManager;

        public BaseForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            base.Padding = new Padding(BorderPens.Length, BorderPens.Length + TitleHeight, BorderPens.Length, BorderPens.Length + 1);

            hotkeysManager = new HotkeysManager();
            //lookUpEditManager = new DictLookUpEditManager();
        }

        [Description("将弹出窗体设置到软件工作区域（不包括软件头部区域，其高度为70）的中间进行显示"), Category("自定义属性")]
        private bool sfjz
        {
            get
            {
                return sfjz;
            }
            set
            {
                sfjz = value;
            }
        }

        //隐藏这个属性，防止继承窗体中修改这个属性造成界面混乱
        [Browsable(false)]
        public new Padding Padding
        {
            get { return base.Padding; }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            //lookUpEditManager.Add(e.Control);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!this.DesignMode)
            {
                //if (AppContext.Session.IsLogin && !AppContext.Parameter.DisplayGridViewContextMenu)
                //{
                //    //屏蔽弹出窗口中所有表格的列头菜单
                //    Type typeDevGridView = typeof(DevExpress.XtraGrid.Views.Grid.GridView);
                //    foreach (var clientModuleField in this.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic))
                //    {
                //        if (clientModuleField.FieldType == typeDevGridView)
                //        {
                //            DevExpress.XtraGrid.Views.Grid.GridView view = clientModuleField.GetValue(this) as DevExpress.XtraGrid.Views.Grid.GridView;
                //            if (view != null)
                //            {
                //                //屏蔽数据过滤菜单
                //                view.OptionsCustomization.AllowFilter = false;
                //                //屏蔽右键自定义排序菜单
                //                view.OptionsMenu.EnableColumnMenu = false;
                //                clientModuleField.SetValue(this, view);
                //            }
                //        }
                //    }
                //}

                hotkeysManager.Register(this);
                //this.KeyPreview = hotkeysManager.Count > 0;  //这种写法，如果窗体的KeyPreview为true，可能会被改成false，造成程序错误
                if (hotkeysManager.Count > 0) this.KeyPreview = true;

                if (this.Tag==null || !this.Tag.Equals("bdw"))
                {
                    //将弹出窗体设置到软件工作区域（不包括软件头部区域，其高度为70）的中间进行显示
                    var workingArea = Screen.GetWorkingArea(this);
                    this.Left = workingArea.Left + (workingArea.Width - this.Width) / 2;
                    this.Top = workingArea.Top + (workingArea.Height - 70 - this.Height) / 2 + 70;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (!this.DesignMode)
            {
                var suppressKeyPress = false;
                if (hotkeysManager.Handle(e.KeyData, ref suppressKeyPress))
                {
                    e.SuppressKeyPress = suppressKeyPress;
                }
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (closeButtonRect.Contains(mouseLocation))
            {
                Close();
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate(new Rectangle(0, 0, this.Width, TitleHeight));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //using (var path = CreateGraphicsPath(this.ClientRectangle))
            //{
            //    this.Region = new Region(path);
            //}

            if (this.ControlBox)
            {
                closeButtonRect = new Rectangle(this.Width - BorderPens.Length - TitleButtonWith, BorderPens.Length, TitleButtonWith, TitleHeight);
            }
            else
            {
                closeButtonRect = Rectangle.Empty;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            //lookUpEditManager.Initialize();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            //在标题栏上点击，认为鼠标左键按下
            if (e.Button == MouseButtons.Left && new Rectangle(0, 0, this.Width, TitleHeight).Contains(e.Location))
            {
                mouseLeftButtonDown = true;
                mouseLeftButtonDownLocation = e.Location;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (mouseLeftButtonDown)
            {
                this.Left = this.Left + e.X - mouseLeftButtonDownLocation.X;
                this.Top = this.Top + e.Y - mouseLeftButtonDownLocation.Y;
            }
            else
            {
                if (closeButtonRect.Contains(mouseLocation) != closeButtonRect.Contains(e.Location))
                {
                    Invalidate(closeButtonRect);
                }

                mouseLocation = e.Location;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseLeftButtonDown = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            //绘制窗体边框
            for (var i = 0; i < BorderPens.Length; i++)
            {
                e.Graphics.DrawRectangle(BorderPens[i], rect);
                rect.Inflate(-1, -1);
            }

            //填充标题栏            
            e.Graphics.FillRectangle(TitleBrush, rect.Left, rect.Top, rect.Width + 1, TitleHeight);

            //绘制窗体圆角边框
            //DrawCorners(e.Graphics, new Rectangle(rect.Left - 1, rect.Top - 1, 2, 2));

            //绘制标题
            var textSize = e.Graphics.MeasureString(this.Text, TitleFont);
            e.Graphics.DrawString(this.Text, TitleFont, Brushes.White, (this.Width - (int)textSize.Width) / 2, BorderPens.Length + (TitleHeight - (int)textSize.Height) / 2);

            //绘制关闭按钮
            if (this.ControlBox)
            {
                if (closeButtonRect.Contains(mouseLocation))
                {
                    //DrawImage(e.Graphics, Resources.BaseFormCloseHover, closeButtonRect, CloseButtonHoverBrush);
                }
                else
                {
                    //DrawImage(e.Graphics, Resources.BaseFormClose, closeButtonRect, TitleBrush);
                }
            }
        }

        /// <summary>
        /// 绘制窗体圆角，绘制方法为：绘制一个2*2矩形，使用倒数第二层边框色填充，再将靠内侧的一个像素使用最后一层边框色填充
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="rect"></param>
        private void DrawCorners(Graphics graphics, Rectangle rect)
        {
            //左上角
            graphics.FillRectangle(cornerBrush1, rect);
            graphics.FillRectangle(cornerBrush2, rect.Left + 1, rect.Top + 1, 1, 1);

            //右上角
            rect.Offset(this.Width - 2 * BorderPens.Length, 0);
            graphics.FillRectangle(cornerBrush1, rect);
            graphics.FillRectangle(cornerBrush2, rect.Left, rect.Top + 1, 1, 1);

            //右下角
            rect.Offset(0, this.Height - 2 * BorderPens.Length);
            graphics.FillRectangle(cornerBrush1, rect);
            graphics.FillRectangle(cornerBrush2, rect.Left, rect.Top, 1, 1);

            //左下角
            rect.Offset(-(this.Width - 2 * BorderPens.Length), 0);
            graphics.FillRectangle(cornerBrush1, rect);
            graphics.FillRectangle(cornerBrush2, rect.Left + 1, rect.Top, 1, 1);
        }

        private void DrawImage(Graphics graphics, Image image, Rectangle rect, Brush background)
        {
            graphics.FillRectangle(background, rect);
            graphics.DrawImage(image, rect.Left + (rect.Width - image.Width) / 2, rect.Top + (rect.Height - image.Height) / 2);
        }

        private GraphicsPath CreateGraphicsPath(Rectangle rect)
        {
            var path = new GraphicsPath();

            //上
            path.AddArc(rect.Left, rect.Top, 2 * Radius, 2 * Radius, 180, 90);
            path.AddLine(Radius, 0, this.Width - 2 * Radius, 0);

            //右
            path.AddArc(rect.Width - 2 * Radius, rect.Top, 2 * Radius, 2 * Radius, 270, 90);
            path.AddLine(rect.Width, Radius, rect.Width, rect.Height - Radius);

            //下
            path.AddArc(rect.Width - 2 * Radius, rect.Height - 2 * Radius, 2 * Radius, 2 * Radius, 0, 90);
            path.AddLine(rect.Width - Radius, rect.Height, Radius, rect.Height);

            path.AddArc(rect.Left, rect.Height - 2 * Radius, 2 * Radius, 2 * Radius, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
