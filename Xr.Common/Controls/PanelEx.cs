using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xr.Common.Controls
{
    /// <summary>
    /// 自定义边框颜色和大小Panel
    /// </summary>
    [ToolboxItem(true)]
    public partial class PanelEx : Panel
    {
        public PanelEx()
        {
            InitializeComponent();
        }

        public PanelEx(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private Color _BorderColor = Color.Black; 
        [Browsable(true), Description("边框颜色"), Category("自定义分组")]
        public Color BorderColor
        {
            get { return _BorderColor; }
            set
            {
                _BorderColor = value;
                this.Invalidate();
            }
        }

        private int _BorderSize = 1;

        [Browsable(true), Description("边框粗细"), Category("自定义分组")]
        public int BorderSize
        {
            get { return _BorderSize; }
            set 
            { 
                _BorderSize = value;
               this.Invalidate();
            }
        }

        private ButtonBorderStyle _BorderStyleTop = ButtonBorderStyle.Solid;

        [Browsable(true), Description("上边框样式"), Category("自定义分组")]
        public ButtonBorderStyle BorderStyleTop
        {
            get { return _BorderStyleTop; }
            set
            {
                _BorderStyleTop = value;
                this.Invalidate();
            }
        }

        private ButtonBorderStyle _BorderStyleRight = ButtonBorderStyle.Solid;

        [Browsable(true), Description("右边框样式"), Category("自定义分组")]
        public ButtonBorderStyle BorderStyleRight
        {
            get { return _BorderStyleRight; }
            set
            {
                _BorderStyleRight = value;
                this.Invalidate();
            }
        }

        private ButtonBorderStyle _BorderStyleBottom = ButtonBorderStyle.Solid;

        [Browsable(true), Description("下边框样式"), Category("自定义分组")]
        public ButtonBorderStyle BorderStyleBottom
        {
            get { return _BorderStyleBottom; }
            set
            {
                _BorderStyleBottom = value;
                this.Invalidate();
            }
        }

        private ButtonBorderStyle _BorderStyleLeft = ButtonBorderStyle.Solid;

        [Browsable(true), Description("左边框样式"), Category("自定义分组")]
        public ButtonBorderStyle BorderStyleLeft
        {
            get { return _BorderStyleLeft; }
            set
            {
                _BorderStyleLeft = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 重写OnPaint方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                            this.ClientRectangle,
                            this._BorderColor,
                            this._BorderSize,
                            this._BorderStyleLeft,
                            this._BorderColor,
                            this._BorderSize,
                            this._BorderStyleTop,
                            this._BorderColor,
                            this._BorderSize,
                            this._BorderStyleRight,
                            this._BorderColor,
                            this._BorderSize,
                            this._BorderStyleBottom);
        }
    }
}
