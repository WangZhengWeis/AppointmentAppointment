using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using XiangRun.Properties;

namespace XiangRun.Internal
{
    internal partial class HintTextBoxForm : Form
    {
        private const int Radius = 5;       //窗体圆角半角

        public bool flag = false; //是否自适应大小

        public HintTextBoxForm()
        {
            InitializeComponent();
        }

        public string Message
        {
            get { return lblMessage.Text; }
            set { lblMessage.Text = value; }
        }

        public int DurationSeconds
        {
            get { return (int)timer1.Interval / 1000; }
            set { timer1.Interval = value * 1000; }
        }

        public HintMessageBoxIcon MessageBoxIcon { get; set; }

        public bool KeepAliveOnOuterClick { get; set; }

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

        private void SetRegion()
        {
            using (var path = CreateGraphicsPath(this.ClientRectangle))
            {
                this.Region = new Region(path);
            }
        }

        private void HintMessageBoxForm_Shown(object sender, EventArgs e)
        {
            //switch (this.MessageBoxIcon)
            //{
            //    case HintMessageBoxIcon.Error:
            //        pbIcon.Image = Resources.HintMessageError;
            //        break;

            //    default:
            //        pbIcon.Image = Resources.HintMessageSuccess;
            //        break;
            //}

            timer1.Enabled = true;
        }

        private void HintMessageBoxForm_Resize(object sender, EventArgs e)
        {
            SetRegion();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Close();
        }

        private void HintMessageBoxForm_Deactivate(object sender, EventArgs e)
        {
            if (!this.KeepAliveOnOuterClick) Close();
        }

        /// <summary>
        /// 根据文字大小修改lblMessage和窗口的宽度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblMessage_TextChanged(object sender, EventArgs e)
        {
            if (flag)
            {
                Graphics graphics = CreateGraphics();
                String[] strArr = lblMessage.Text.Split(new string[] { "\n" }, StringSplitOptions.None);
                float lblMsgWidth = 220;//记录最大的宽度,最小为220
                int fontHeight = 0;
                for (int i = 0; i < strArr.Length; i++)
                {
                    SizeF sizeF = graphics.MeasureString(strArr[i], lblMessage.Font);
                    if (i == 0)
                    {
                        fontHeight = (int)sizeF.Height;
                        if(sizeF.Width > 220)
                            lblMsgWidth = sizeF.Width;
                    }
                    else
                    {
                        if (sizeF.Width > lblMsgWidth)
                        {
                            lblMsgWidth = sizeF.Width;
                        }
                    }
                }
                int msgWidth = (int)lblMsgWidth;
                int addWidth = msgWidth - lblMessage.Width;
                this.Width = this.Width + addWidth;
                lblMessage.Width = msgWidth;
                this.Height = 16 + strArr.Count() * fontHeight;
                lblMessage.Height = strArr.Count() * fontHeight;
            }
        }
    }
}
