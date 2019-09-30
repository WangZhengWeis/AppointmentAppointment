using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Drawing.Drawing2D;
using DevExpress.Utils.Editors;
using System.Drawing.Design;

namespace Xr.Common.Controls
{
    public partial class GroupBorderPanel : PanelControl 
    {
        /*
        public GroupBorderPanel()
        {
            InitializeComponent();
            borderPen=new Pen(Color.FromArgb(214, 214, 214));
            this.Paint += new PaintEventHandler(GroupBorderPanel_Paint);
        }
        private Pen borderPen;
        /// <summary>
        /// 获取或设置边框颜色
        /// </summary>
        [DefaultValue(typeof(Color), "214, 214, 214")]
        [Description("边框颜色")]
        public Color BorderColor
        {
            get { return borderPen.Color; }
            set
            {
                borderPen.Color = value;
                Invalidate();
            }
        }
        private void GroupBorderPanel_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.Clear(this.BackColor);
            //e.Graphics.DrawString(this.Text, this.Font, Brushes.Black, 10, 1);
            e.Graphics.DrawLine(borderPen, 1, 9, 8, 9);
            e.Graphics.DrawLine(borderPen, e.Graphics.MeasureString(this.Text, this.Font).Width + 8, 9, this.Width - 2, 9);
            e.Graphics.DrawLine(borderPen, 1, 9, 1, this.Height - 2);
            e.Graphics.DrawLine(borderPen, 1, this.Height - 2, this.Width - 2, this.Height - 2);
            e.Graphics.DrawLine(borderPen, this.Width - 2, 9, this.Width - 2, this.Height - 2);
        }

         */

        
        private int borderWidth;
        private BorderSides borderSides;
        private Pen borderPen;
        private Color fillColor1;
        private Color fillColor2;
        private LinearGradientMode gradientMode;
        private LinearGradientBrush fillBrush = null;
        private String groupText ;

        public GroupBorderPanel()
        {
            base.BorderStyle = BorderStyles.NoBorder;

            fillColor1 = Color.FromArgb(249, 249, 249);
            fillColor2 = Color.FromArgb(249, 249, 249);
            gradientMode = LinearGradientMode.Vertical;

            borderPen = new Pen(Color.FromArgb(214, 214, 214));

            RefreshFillBrush();

            this.DoubleBuffered = true;
            this.BorderWidth = 1;
            this.BorderSides = BorderSides.All;
            this.CornerRadius = new CornerRadius(0);
            this.Padding = new Padding(0, 20, 0, 0);
            this.Paint += new PaintEventHandler(BorderPanel_Paint);
        }

        /// <summary>
        /// 获取或设置需要显示边框的侧边
        /// </summary>
        [DefaultValue(BorderSides.All)]
        [Description("需要显示边框的侧边")]
        [Editor(typeof(AttributesEditor), typeof(UITypeEditor))]
        public BorderSides BorderSides
        {
            get { return borderSides; }
            set
            {
                borderSides = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取圆角半径
        /// </summary>        
        [Description("转角处圆角半径")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CornerRadius CornerRadius { get;  set; }

        /// <summary>
        /// 获取或设置边框颜色
        /// </summary>
        [DefaultValue(typeof(Color), "214, 214, 214")]
        [Description("边框颜色")]
        public Color BorderColor
        {
            get { return borderPen.Color; }
            set
            {
                borderPen.Color = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置边框宽度
        /// </summary>
        [DefaultValue(1)]
        [Description("边框宽度")]
        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value > 0 ? value : 1;
                borderPen.Width = borderWidth;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置填充色1
        /// </summary>
        [DefaultValue(typeof(Color), "249, 249, 249")]
        [Description("填充色1")]
        public Color FillColor1
        {
            get { return fillColor1; }
            set
            {
                fillColor1 = value;
                RefreshFillBrush();
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置填充色2
        /// </summary>
        [DefaultValue(typeof(Color), "249, 249, 249")]
        [Description("填充色2")]
        public Color FillColor2
        {
            get { return fillColor2; }
            set
            {
                fillColor2 = value;
                RefreshFillBrush();
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置填充方向
        /// </summary>
        [DefaultValue(LinearGradientMode.Vertical)]
        [Description("填充方向")]
        public LinearGradientMode GradientMode
        {
            get { return gradientMode; }
            set
            {
                gradientMode = value;
                RefreshFillBrush();
                Invalidate();
            }
        }
        /// <summary>
        /// 获取或设置分组文本
        /// </summary>
        [Description("分组文本")]
        public String GroupText
        {
            get { return groupText; }
            set
            {
                groupText = value;
            }
        }
        private Font font=new Font("微软雅黑",10f);
        /// <summary>
        /// 获取或设置分组文本样式
        /// </summary>
        [DefaultValue(typeof(Font), "微软雅黑,10pt")]
        [Description("分组文本样式")]
        public Font GroupTextFont 
        {
            get { return font; }
            set
            {
                font = value;
            }
        }
        /// <summary>
        /// 隐藏原控件中的对应属性
        /// </summary>
        [Browsable(false)]
        public new BorderStyles BorderStyle { get; set; }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RefreshFillBrush();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (borderPen != null) borderPen.Dispose();
            if (fillBrush != null) fillBrush.Dispose();
        }

        private void BorderPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //填充背景
            using (var path = CreateFillPath())
            {
                e.Graphics.FillPath(fillBrush, path);
            }


            var rect = new Rectangle(
                this.BorderWidth / 2,
                this.BorderWidth / 2,
                this.Width - (int)Math.Ceiling(this.BorderWidth / 2f),
                this.Height - (int)Math.Ceiling(this.BorderWidth / 2f));
            //绘制文字
            e.Graphics.DrawString(this.groupText, this.font, Brushes.Black, 15, 1);
            //绘制左上圆角
            if (this.CornerRadius.TopLeft > 0)
            {
                e.Graphics.DrawArc(borderPen, new Rectangle(rect.Left, rect.Top+9, 2 * this.CornerRadius.TopLeft, 2 * this.CornerRadius.TopLeft), 180, 90);
            }

            //绘制右上圆角
            if (this.CornerRadius.TopRight > 0)
            {
                e.Graphics.DrawArc(borderPen, new Rectangle(rect.Width - this.CornerRadius.TopRight * 2, 9, this.CornerRadius.TopRight * 2, this.CornerRadius.TopRight * 2), 270, 90);
            }

            //绘制右下圆角
            if (this.CornerRadius.BottomRight > 0)
            {
                e.Graphics.DrawArc(borderPen, new Rectangle(rect.Width - this.CornerRadius.BottomRight * 2, rect.Height - this.CornerRadius.BottomRight * 2, this.CornerRadius.BottomRight * 2, this.CornerRadius.BottomRight * 2), 0, 90);
            }

            //绘制左下圆角
            if (this.CornerRadius.BottomLeft > 0)
            {
                e.Graphics.DrawArc(borderPen, new Rectangle(0, rect.Height - this.CornerRadius.BottomLeft * 2, this.CornerRadius.BottomLeft * 2, this.CornerRadius.BottomLeft * 2), 90, 90);
            }

            //绘制上边框
            if (this.BorderSides.HasFlag(BorderSides.Top))
            {
                e.Graphics.DrawLine(borderPen, this.CornerRadius.TopLeft, rect.Top+9, 13, 9);
                e.Graphics.DrawLine(borderPen, e.Graphics.MeasureString(this.groupText, this.font).Width + 13, 9, rect.Width - this.CornerRadius.TopRight, rect.Top+9);

                //e.Graphics.DrawLine(borderPen, this.CornerRadius.TopLeft, rect.Top, rect.Width - this.CornerRadius.TopRight, rect.Top);
            }

            //绘制右边框
            if (this.BorderSides.HasFlag(BorderSides.Right))
            {
                e.Graphics.DrawLine(borderPen, rect.Width, this.CornerRadius.TopRight+9, rect.Width, rect.Height - this.CornerRadius.BottomRight);
            }

            //绘制下边框
            if (this.BorderSides.HasFlag(BorderSides.Bottom))
            {
                e.Graphics.DrawLine(borderPen, this.CornerRadius.BottomLeft, rect.Height, rect.Width - this.CornerRadius.BottomRight, rect.Height);
            }

            //绘制左边框
            if (this.BorderSides.HasFlag(BorderSides.Left))
            {
                e.Graphics.DrawLine(borderPen, rect.Left, this.CornerRadius.TopLeft+9, rect.Left, this.Height - this.CornerRadius.BottomLeft);
            }
        }

        private GraphicsPath CreateFillPath()
        {
            var path = new GraphicsPath();

            int left = 0;
            int top = 0;
            int width = this.Width;
            int height = this.Height;

            if (this.BorderSides.HasFlag(BorderSides.Left)) left += this.BorderWidth / 2;
            if (this.BorderSides.HasFlag(BorderSides.Top)) top += this.BorderWidth / 2;
            if (this.BorderSides.HasFlag(BorderSides.Right)) width -= (int)Math.Ceiling(this.BorderWidth / 2f);
            if (this.BorderSides.HasFlag(BorderSides.Bottom)) height -= (int)Math.Ceiling(this.BorderWidth / 2f);

            var rect = new Rectangle(left, top, width, height);

            /*
            var rect = new Rectangle(
                this.BorderWidth / 2,
                this.BorderWidth / 2,
                this.Width - (int)Math.Ceiling(this.BorderWidth / 2f),
                this.Height - (int)Math.Ceiling(this.BorderWidth / 2f));
            */

            //左上圆角
            if (this.CornerRadius.TopLeft > 0)
            {
                path.AddArc(new Rectangle(rect.Left, rect.Top, this.CornerRadius.TopLeft * 2, this.CornerRadius.TopLeft * 2), 180, 90);
            }

            //上边框
            path.AddLine(this.CornerRadius.TopLeft, 0, rect.Width - this.CornerRadius.TopLeft - this.CornerRadius.TopRight, 0);

            //右上圆角
            if (this.CornerRadius.TopRight > 0)
            {
                path.AddArc(new Rectangle(rect.Width - this.CornerRadius.TopRight * 2, 0, this.CornerRadius.TopRight * 2, this.CornerRadius.TopRight * 2), 270, 90);
            }

            //右边框
            path.AddLine(this.Width, this.CornerRadius.TopRight, rect.Width, rect.Height - this.CornerRadius.BottomRight);

            //右下圆角
            if (this.CornerRadius.BottomRight > 0)
            {
                path.AddArc(new Rectangle(rect.Width - this.CornerRadius.BottomRight * 2, rect.Height - this.CornerRadius.BottomRight * 2, this.CornerRadius.BottomRight * 2, this.CornerRadius.BottomRight * 2), 0, 90);
            }

            //下边框
            path.AddLine(rect.Width - this.CornerRadius.BottomRight, rect.Height, this.CornerRadius.BottomLeft, rect.Height);

            //左下圆角
            if (this.CornerRadius.BottomLeft > 0)
            {
                path.AddArc(new Rectangle(0, rect.Height - this.CornerRadius.BottomLeft * 2, this.CornerRadius.BottomLeft * 2, this.CornerRadius.BottomLeft * 2), 90, 90);
            }

            path.CloseFigure();

            return path;
        }

        protected void RefreshFillBrush()
        {
            if (fillBrush != null) fillBrush.Dispose();

            int width = this.Width > 0 ? this.Width : 1;
            int height = this.Height > 0 ? this.Height : 1;
            fillBrush = new LinearGradientBrush(new Rectangle(new Point(0, 0), new Size(width, height)), this.FillColor1, this.FillColor2, this.GradientMode);
        }
    }
}
