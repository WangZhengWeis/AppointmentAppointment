using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Xr.Common.Common;

namespace Xr.Common.Controls
{
    /// <summary>
    /// 表示工具栏上的按钮
    /// </summary>
    [DefaultEvent("Click")]
    public class ToolBarButton : UserControl, IButtonControl, ISupportHotkeys
    {
        private const int ControlSpacing = 10;          //控件间隔
        private const int DropDownMenuFlagSpacing = 3;  //下拉菜单按钮标记与文字之间的间隔
        private const int DropDownMenuFlagWidth = 6;    //下拉菜单按钮标记宽度
        private const int DropDownMenuFlagHeight = 4;   //下拉菜单按钮标记高度
        private readonly Color HoverColor = Color.FromArgb(42, 131, 113);

        private PictureBox pbIcon;
        private Image image;
        private Image hoverImage;
        private Image disabledImage;
        private ContextMenuStrip dropDownMenu;
        private bool hovering = false;

        public ToolBarButton()
        {
            pbIcon = new PictureBox();
            pbIcon.Parent = this;
            pbIcon.Size = new Size(16, 16);
            pbIcon.Enabled = false;

            this.BackColor = Color.Transparent;
            this.Dock = DockStyle.Left;
            this.Text = "";
            this.Font = new Font("微软雅黑", 14, GraphicsUnit.Pixel);
            this.Width = 52;
            this.ShowHotKeys = true;
        }

        [DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [DefaultValue(DockStyle.Left)]
        public override DockStyle Dock
        {
            get { return base.Dock; }
            set { base.Dock = value; }
        }

        /// <summary>
        /// 获取或设置与按钮相关联的文本
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        [DefaultValue(typeof(Font), "微软雅黑,14px")]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }
        private bool mShowHotKeys;

        [DefaultValue(true)]
        public bool ShowHotKeys
        {
            get{
                return mShowHotKeys;
            }
            set {
                mShowHotKeys = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置按钮快捷键
        /// </summary>
        [DefaultValue(Keys.None)]
        [Description("按钮快捷键")]
        public Keys Hotkeys { get; set; }

        /// <summary>
        /// 获取或设置单击按钮时返回给父窗体的值
        /// </summary>
        [DefaultValue(DialogResult.None)]
        [Description("单击按钮时返回给父窗体的值")]
        public DialogResult DialogResult { get; set; }

        [DefaultValue(null)]
        [Description("获取或设置按钮点击时的下拉菜单")]
        public ContextMenuStrip DropDownMenu
        {
            get { return dropDownMenu; }
            set
            {
                dropDownMenu = value;
                UpdateLayout();
                Invalidate();
            }
        }

        public void NotifyDefault(bool value)
        {

        }

        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }

        protected override void OnClick(EventArgs e)
        {
            var form = base.FindForm();
            if (form != null)
            {
                form.DialogResult = this.DialogResult;
            }

            if (dropDownMenu != null)
            {
                dropDownMenu.Show(this, new Point(0, this.Height));
            }

            base.OnClick(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            //确定图片位置
            if (!this.IsDesignMode())
            {
                //var imageFileName = Path.Combine(AppContext.Directories.Images, string.Format("Button-{0}.png", GetImageName()));
                //if (File.Exists(imageFileName)) image = new Bitmap(imageFileName);

                //imageFileName = Path.Combine(AppContext.Directories.Images, string.Format("Button-{0}-Hover.png", GetImageName()));
                //if (File.Exists(imageFileName)) hoverImage = new Bitmap(imageFileName);

                //imageFileName = Path.Combine(AppContext.Directories.Images, string.Format("Button-{0}-Disabled.png", GetImageName()));
                //if (File.Exists(imageFileName)) disabledImage = new Bitmap(imageFileName);

                //if (hoverImage == null) hoverImage = image;
                //if (disabledImage == null) disabledImage = image;
            }

            if (image != null)
            {
                pbIcon.Size = image.Size;
            }

            pbIcon.Image = image;

            UpdateLayout();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            RefreshIcon();
            Invalidate();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateLayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Brush textBrush = null;

            if (this.Enabled)
            {
                textBrush = hovering ? Brushes.White : Brushes.Black;
            }
            else
            {
                textBrush = SystemBrushes.GrayText;
            }
           
            //绘制文字
            var textSize = e.Graphics.MeasureString(DisplayText, this.Font);
            e.Graphics.DrawString(DisplayText, this.Font,
                textBrush,
                pbIcon.Right + ControlSpacing,
                (this.Height - textSize.Height) / 2);

            //绘制下拉菜单标记（在指定区域绘制一个倒三角形）
            if (dropDownMenu != null)
            {
                using (var triangle = new GraphicsPath())
                {
                    //计算绘制区域的左上角坐标
                    var x = this.Width - ControlSpacing - DropDownMenuFlagWidth;
                    var y = (this.Height - DropDownMenuFlagHeight) / 2;

                    //计算三个顶点的坐标
                    triangle.AddLines(new[]
                    {
                        new Point(x, y),
                        new Point(x + DropDownMenuFlagWidth, y),
                        new Point(x + DropDownMenuFlagWidth / 2, y + DropDownMenuFlagHeight)
                    });

                    triangle.CloseFigure();

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(textBrush, triangle);
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (this.DesignMode) return;

            hovering = true;
            this.BackColor = HoverColor;

            RefreshIcon();
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (this.DesignMode) return;

            hovering = false;
            this.BackColor = Color.Transparent;

            RefreshIcon();
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateLayout();
        }

        /// <summary>
        /// 获取图标名称
        /// </summary>
        /// <returns></returns>
        private string GetImageName()
        {
            var imageName = this.Text;

            //包含括号，取括号前面的部分
            var index = imageName.IndexOf('(');
            if (index != -1) imageName = imageName.Substring(0, index);

            return imageName.Replace("/", string.Empty).Replace("\\", string.Empty);
        }
        /// <summary>
        /// 按钮显示的文本信息
        /// </summary>
        string DisplayText
        {
            get
            {
                var showText = this.Text;
                if (ShowHotKeys)
                {
                    if (Hotkeys != Keys.None)
                    {
                        showText = showText + "[" + Hotkeys.ToString() + "]";
                    }
                }
                return showText;
            }
        }
        private void UpdateLayout()
        {
            pbIcon.Left = ControlSpacing;
            pbIcon.Top = (this.Height - pbIcon.Height) / 2;

            using (var graphics = this.CreateGraphics())
            {
                var textSize = graphics.MeasureString(DisplayText, this.Font);
                var width = pbIcon.Width + (int)textSize.Width + 3 * ControlSpacing;  //图标宽度+文字宽度+左边距+右边距+图标与文字间距
                if (dropDownMenu != null) width = width + DropDownMenuFlagSpacing + DropDownMenuFlagWidth;
                if (this.Dock == DockStyle.None || this.Dock == DockStyle.Left || this.Dock == DockStyle.Right)
                {
                    this.Width = width;
                }
            }
        }

        /// <summary>
        /// 刷新按钮图标
        /// </summary>
        private void RefreshIcon()
        {
            if (this.Enabled)
            {
                pbIcon.Image = hovering ? hoverImage : image;
            }
            else
            {
                pbIcon.Image = disabledImage;
            }
        }
    }
}
