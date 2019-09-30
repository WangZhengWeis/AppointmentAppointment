using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.Utils.Editors;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace Xr.Common.Controls
{
    /// <summary>
    /// 带边框和圆角的Panel
    /// </summary>
    [ToolboxItem(true)]
    public class BorderPanel : PanelControl
    {
        private int borderWidth;
        private BorderSides borderSides;
        private Pen borderPen;
        private Color fillColor1;
        private Color fillColor2;
        private LinearGradientMode gradientMode;
        private LinearGradientBrush fillBrush = null;

        public BorderPanel()
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

            //绘制左上圆角
            if (this.CornerRadius.TopLeft > 0)
            {
                e.Graphics.DrawArc(borderPen, new Rectangle(rect.Left, rect.Top, 2 * this.CornerRadius.TopLeft, 2 * this.CornerRadius.TopLeft), 180, 90);
            }

            //绘制右上圆角
            if (this.CornerRadius.TopRight > 0)
            {
                e.Graphics.DrawArc(borderPen, new Rectangle(rect.Width - this.CornerRadius.TopRight * 2, 0, this.CornerRadius.TopRight * 2, this.CornerRadius.TopRight * 2), 270, 90);
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
                e.Graphics.DrawLine(borderPen, this.CornerRadius.TopLeft, rect.Top, rect.Width - this.CornerRadius.TopRight, rect.Top);
            }

            //绘制右边框
            if (this.BorderSides.HasFlag(BorderSides.Right))
            {
                e.Graphics.DrawLine(borderPen, rect.Width, this.CornerRadius.TopRight, rect.Width, rect.Height - this.CornerRadius.BottomRight);
            }

            //绘制下边框
            if (this.BorderSides.HasFlag(BorderSides.Bottom))
            {
                e.Graphics.DrawLine(borderPen, this.CornerRadius.BottomLeft, rect.Height, rect.Width - this.CornerRadius.BottomRight, rect.Height);
            }

            //绘制左边框
            if (this.BorderSides.HasFlag(BorderSides.Left))
            {
                e.Graphics.DrawLine(borderPen, rect.Left, this.CornerRadius.TopLeft, rect.Left, this.Height - this.CornerRadius.BottomLeft);
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

    /// <summary>
    /// 表示需要显示边框的边缘
    /// </summary>
    [Flags]
    public enum BorderSides
    {
        /// <summary>
        /// 不显示边框
        /// </summary>
        None = 0,

        /// <summary>
        /// 四周均显示边框
        /// </summary>
        All = Left | Top | Right | Bottom,

        /// <summary>
        /// 左侧显示边框
        /// </summary>
        Left = 1,

        /// <summary>
        /// 上方显示边框
        /// </summary>
        Top = 2,

        /// <summary>
        /// 右侧显示边框
        /// </summary>
        Right = 4,

        /// <summary>
        /// 下方显示边框
        /// </summary>
        Bottom = 8
    }

    /// <summary>
    /// 表示圆角半径
    /// </summary>
    [Serializable]
    [Browsable(true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CornerRadius
    {
        private int all;

        public CornerRadius(int all)
        {
            this.All = all;
        }

        /// <summary>
        /// 获取或设置所有圆角半径
        /// </summary>
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.All)]
        public int All
        {
            get { return all; }
            set
            {
                this.all = value;
                this.TopLeft = value;
                this.TopRight = value;
                this.BottomLeft = value;
                this.BottomRight = value;
            }
        }

        /// <summary>
        /// 获取或设置左上角半径
        /// </summary>
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.All)]
        public int TopLeft { get; set; }

        /// <summary>
        /// 获取或设置右上角半径
        /// </summary>
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.All)]
        public int TopRight { get; set; }

        /// <summary>
        /// 获取或设置左下脚半径
        /// </summary>
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.All)]
        public int BottomLeft { get; set; }

        /// <summary>
        /// 获取或设置右下角半径
        /// </summary>
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.All)]
        public int BottomRight { get; set; }

        public override string ToString()
        {
            return
                TopLeft.ToString() + "," +
                TopRight.ToString() + "," +
                BottomLeft.ToString() + "," +
                BottomRight.ToString();
        }
    }
}
