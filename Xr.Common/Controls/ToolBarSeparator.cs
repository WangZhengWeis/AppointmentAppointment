using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xr.Common.Controls
{
    /// <summary>
    /// 表示工具栏上的一个分隔符
    /// </summary>
    public class ToolBarSeparator : UserControl
    {
        private Pen pen;

        public ToolBarSeparator()
        {
            this.Width = 9;
            this.Dock = DockStyle.Left;
            this.ForeColor = Color.FromArgb(206, 223, 234);
        }

        [DefaultValue(DockStyle.Left)]
        public override DockStyle Dock
        {
            get { return base.Dock; }
            set { base.Dock = value; }
        }

        [DefaultValue(typeof(Color), "206, 223, 234")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);

            pen = null;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //在控件中间位置绘制一个竖线            
            if (pen == null) pen = new Pen(this.ForeColor, 1);
            e.Graphics.DrawLine(pen, (this.Width - pen.Width) / 2, 7, (this.Width - pen.Width) / 2, this.Height - 7);
        }
    }
}
