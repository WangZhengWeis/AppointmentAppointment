using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xr.Common.Controls
{
    public class ButtonControl : Control, IButtonControl, ISupportHotkeys
    {
        private static readonly ButtonAppearence GreenButtonAppearence = new ButtonAppearence(ButtonStyle.Green);
        private static readonly ButtonAppearence GrayButtonAppearence = new ButtonAppearence(ButtonStyle.Gray);
        private static readonly ButtonAppearence QueryButtonAppearence = new ButtonAppearence(ButtonStyle.Query);
        private static readonly ButtonAppearence SaveButtonAppearence = new ButtonAppearence(ButtonStyle.Save);
        private static readonly ButtonAppearence DelButtonAppearence = new ButtonAppearence(ButtonStyle.Del);
        private static readonly ButtonAppearence Calendar_dayButtonAppearence = new ButtonAppearence(ButtonStyle.Calendar_day);
        private static readonly ButtonAppearence Calendar_weekendButtonAppearence = new ButtonAppearence(ButtonStyle.Calendar_weekend);
        private static readonly ButtonAppearence Calendar_todayButtonAppearence = new ButtonAppearence(ButtonStyle.Calendar_today);
        private static readonly ButtonAppearence ReturnButtonAppearence = new ButtonAppearence(ButtonStyle.Return);
        private static readonly ButtonAppearence OKButtonAppearence = new ButtonAppearence(ButtonStyle.OK);
        private static readonly ButtonAppearence FF22A7ButtonAppearence = new ButtonAppearence(ButtonStyle.FF22A7);
        private static readonly ButtonAppearence FF5722ButtonAppearence = new ButtonAppearence(ButtonStyle.FF5722);

        private bool hovering;
        private ButtonStyle style;
        private ButtonAppearence buttonAppearence;

        public ButtonControl()
        {
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.ForeColor = Color.FromArgb(42, 131, 113);
            this.Font = new Font("微软雅黑", 14, GraphicsUnit.Pixel);
            this.Style = ButtonStyle.Gray;
            this.DialogResult = DialogResult.None;
            this.Height = 30;
            this.Width = 75;
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

        [DefaultValue(typeof(Color), "236, 236, 236")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [DefaultValue(typeof(Color), "66, 66, 66")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [DefaultValue(typeof(Font), "微软雅黑,14px")]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        private String secondText=String.Empty;
        public  String SecondText
        {
            get { return secondText; }
            set { secondText = value; }
        }
        /// <summary>
        /// 获取或设置鼠标悬停时的背景色
        /// </summary>
        [DefaultValue(typeof(Color), "236, 236, 236")]
        public Color HoverBackColor { get; set; }

        /// <summary>
        /// 获取或设置按钮风格
        /// </summary>
        [DefaultValue(ButtonStyle.Gray)]
        public ButtonStyle Style
        {
            get { return style; }
            set
            {
                style = value;
                if (style == ButtonStyle.Green)
                {
                    buttonAppearence = GreenButtonAppearence;
                }
                else if (style == ButtonStyle.Gray)
                {
                    buttonAppearence = GrayButtonAppearence;
                }
                else if (style == ButtonStyle.Query)
                {
                    buttonAppearence = QueryButtonAppearence;
                }
                else if (style == ButtonStyle.Save)
                {
                    buttonAppearence = SaveButtonAppearence;
                }
                else if (style == ButtonStyle.Del)
                {
                    buttonAppearence = DelButtonAppearence;
                }
                else if (style == ButtonStyle.Return)
                {
                    buttonAppearence = ReturnButtonAppearence;
                }
                else if (style == ButtonStyle.Calendar_day)
                {
                    buttonAppearence = Calendar_dayButtonAppearence;
                }
                else if (style == ButtonStyle.Calendar_weekend)
                {
                    buttonAppearence = Calendar_weekendButtonAppearence;
                }
                else if (style == ButtonStyle.Calendar_today)
                {
                    buttonAppearence = Calendar_todayButtonAppearence;
                }
                else if (style == ButtonStyle.OK)
                {
                    buttonAppearence = OKButtonAppearence;
                }
                else if (style == ButtonStyle.FF22A7)
                {
                    buttonAppearence = FF22A7ButtonAppearence;
                }
                else if (style == ButtonStyle.FF5722)
                {
                    buttonAppearence = FF5722ButtonAppearence;
                }
                else
                {
                    buttonAppearence = GrayButtonAppearence;
                }
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
            this.Focus();
            var form = base.FindForm();
            if (form != null)
            {
                form.DialogResult = this.DialogResult;
            }
            base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //确定背景色
            var backgroundBrush = buttonAppearence.Background;
            if (hovering) backgroundBrush = buttonAppearence.HoveringBackground;
            if (!this.Enabled) backgroundBrush = buttonAppearence.DisabledBackground;
            if (this.Focused) backgroundBrush = buttonAppearence.FocusedBackground;

            //确定文字颜色
            var textBrush = buttonAppearence.TextBrush;
            if (hovering) textBrush = buttonAppearence.HoveringTextBrush;
            if (!this.Enabled) textBrush = buttonAppearence.DisabledTextBrush;
            
            //绘制背景
            e.Graphics.FillRectangle(backgroundBrush, e.ClipRectangle);

            //绘制边框
            if (this.Enabled) 
                e.Graphics.DrawRectangle(buttonAppearence.BorderBrush, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);

            //绘制获得焦点时的样式
            if (this.Focused)
            {
                e.Graphics.DrawRectangle(buttonAppearence.FocusCuesBrush1, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
                e.Graphics.DrawRectangle(buttonAppearence.FocusCuesBrush2, e.ClipRectangle.Left + 1, e.ClipRectangle.Top + 1, e.ClipRectangle.Width - 3, e.ClipRectangle.Height - 3);
            }

            //绘制文字
            var textSize = e.Graphics.MeasureString(this.Text, this.Font);
            if (secondText == String.Empty)
            {
                e.Graphics.DrawString(this.Text, this.Font, textBrush, (this.Width - (int)textSize.Width) / 2, (this.Height - (int)textSize.Height) / 2);
            }
            else
            {
                Font secondFont = new Font("黑体", 8f);
                var secondtextSize = e.Graphics.MeasureString(secondText, this.Font);
                e.Graphics.DrawString(this.Text, this.Font, textBrush, (this.Width - (int)textSize.Width) / 2+8, (this.Height - (int)textSize.Height) / 2+5);
                e.Graphics.DrawString(secondText, secondFont, textBrush, 0, 0);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            hovering = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            hovering = false;
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        /// <summary>
        /// 按钮外观定义
        /// </summary>
        private class ButtonAppearence
        {
            internal ButtonAppearence(ButtonStyle style)
            {
                switch (style)
                {
                    case ButtonStyle.Green:
                        this.BorderBrush = new Pen(Color.FromArgb(42, 131, 113), 1);
                        this.FocusCuesBrush1 = new Pen(Color.FromArgb(92, 209, 185), 1);
                        this.FocusCuesBrush2 = new Pen(Color.White, 1) { DashPattern = new[] { 2f, 3f } };
                        this.TextBrush = new SolidBrush(Color.White);
                        this.DisabledTextBrush = new SolidBrush(Color.White);
                        this.HoveringTextBrush = new SolidBrush(Color.White);
                        this.Background = new SolidBrush(Color.FromArgb(42, 131, 113));
                        this.DisabledBackground = new SolidBrush(Color.FromArgb(217, 217, 217));
                        this.HoveringBackground = new SolidBrush(Color.FromArgb(51, 144, 125));
                        this.FocusedBackground = new SolidBrush(Color.FromArgb(42, 131, 113));
                        break;

                    case ButtonStyle.Gray:
                        this.BorderBrush = new Pen(Color.FromArgb(42, 131, 113), 1);
                        this.FocusCuesBrush1 = new Pen(Color.FromArgb(42, 131, 113), 1);
                        this.FocusCuesBrush2 = new Pen(Color.FromArgb(42, 131, 113), 1) { DashPattern = new[] { 2f, 3f } };
                        this.TextBrush = new SolidBrush(Color.FromArgb(42, 131, 113));
                        this.DisabledTextBrush = new SolidBrush(Color.White);
                        this.HoveringTextBrush = new SolidBrush(Color.White);
                        this.Background = new SolidBrush(Color.White);
                        this.DisabledBackground = new SolidBrush(Color.FromArgb(217, 217, 217));
                        this.HoveringBackground = new SolidBrush(Color.FromArgb(42, 131, 113));
                        this.FocusedBackground = new SolidBrush(Color.FromArgb(42, 131, 113));
                        break;
  
                    case ButtonStyle.Query:
                        this.BorderBrush = new Pen(Color.FromArgb(1, 170, 237), 0);
                        this.FocusCuesBrush1 = new Pen(Color.FromArgb(1, 150, 237), 0);
                        this.FocusCuesBrush2 = new Pen(Color.FromArgb(1, 150, 237), 1);
                        this.TextBrush = new SolidBrush(Color.White);
                        this.DisabledTextBrush = new SolidBrush(Color.FromArgb(85, 85, 85));
                        this.HoveringTextBrush = new SolidBrush(Color.White);
                        this.Background = new SolidBrush(Color.FromArgb(1, 170, 237));
                        this.DisabledBackground = new SolidBrush(Color.FromArgb(217, 217, 217));
                        this.HoveringBackground = new SolidBrush(Color.FromArgb(1, 190, 237));
                        this.FocusedBackground = new SolidBrush(Color.FromArgb(1, 150, 237));
                        break;
                    case ButtonStyle.Save:
                        this.BorderBrush = new Pen(Color.FromArgb(0, 150, 136), 0);
                        this.FocusCuesBrush1 = new Pen(Color.FromArgb(0, 130, 136), 0);
                        this.FocusCuesBrush2 = new Pen(Color.FromArgb(0, 130, 136), 1);
                        this.TextBrush = new SolidBrush(Color.White);
                        this.DisabledTextBrush = new SolidBrush(Color.FromArgb(85, 85, 85));
                        this.HoveringTextBrush = new SolidBrush(Color.White);
                        this.Background = new SolidBrush(Color.FromArgb(0, 150, 136));
                        this.DisabledBackground = new SolidBrush(Color.FromArgb(217, 217, 217));
                        this.HoveringBackground = new SolidBrush(Color.FromArgb(0, 170, 136));
                        this.FocusedBackground = new SolidBrush(Color.FromArgb(0, 130, 136));
                        break;
                    case ButtonStyle.Del:
                        this.BorderBrush = new Pen(Color.FromArgb(255, 87, 34), 0);
                        this.FocusCuesBrush1 = new Pen(Color.FromArgb(255, 64, 34), 0);
                        this.FocusCuesBrush2 = new Pen(Color.FromArgb(255, 64, 34), 1);
                        this.TextBrush = new SolidBrush(Color.White);
                        this.DisabledTextBrush = new SolidBrush(Color.FromArgb(85, 85, 85));
                        this.HoveringTextBrush = new SolidBrush(Color.White);
                        this.Background = new SolidBrush(Color.FromArgb(255, 87, 34));
                        this.DisabledBackground = new SolidBrush(Color.FromArgb(217, 217, 217));
                        this.HoveringBackground = new SolidBrush(Color.FromArgb(255, 104, 34));
                        this.FocusedBackground = new SolidBrush(Color.FromArgb(255, 64, 34));
                        break;
                    case ButtonStyle.Calendar_day:
                        this.BorderBrush = new Pen(Color.FromArgb(1, 170, 237), 0);
                        this.FocusCuesBrush1 = new Pen(Color.FromArgb(1, 150, 237), 0);
                        this.FocusCuesBrush2 = new Pen(Color.FromArgb(1, 150, 237), 1);
                        this.TextBrush = new SolidBrush(Color.White);
                        this.DisabledTextBrush = new SolidBrush(Color.FromArgb(85, 85, 85));
                        this.HoveringTextBrush = new SolidBrush(Color.White);
                        this.Background = new SolidBrush(Color.FromArgb(1, 170, 237));
                        this.DisabledBackground = new SolidBrush(Color.FromArgb(217, 217, 217));
                        this.HoveringBackground = new SolidBrush(Color.FromArgb(1, 190, 237));
                        this.FocusedBackground = new SolidBrush(Color.FromArgb(1, 150, 237));
                        break;
                    case ButtonStyle.Calendar_weekend:
                        this.BorderBrush = new Pen(Color.FromArgb(1, 170, 237), 0);
                        this.FocusCuesBrush1 = new Pen(Color.FromArgb(1, 150, 237), 0);
                        this.FocusCuesBrush2 = new Pen(Color.FromArgb(1, 150, 237), 1);
                        this.TextBrush = new SolidBrush(Color.White);
                        this.DisabledTextBrush = new SolidBrush(Color.Red);
                        this.HoveringTextBrush = new SolidBrush(Color.White);
                        this.Background = new SolidBrush(Color.FromArgb(1, 170, 237));
                        this.DisabledBackground = new SolidBrush(Color.FromArgb(217, 217, 217));
                        this.HoveringBackground = new SolidBrush(Color.FromArgb(1, 190, 237));
                        this.FocusedBackground = new SolidBrush(Color.FromArgb(1, 150, 237));
                        break;
                    case ButtonStyle.Calendar_today:
                        this.BorderBrush = new Pen(Color.FromArgb(255, 87, 34), 0);
                        this.FocusCuesBrush1 = new Pen(Color.FromArgb(255, 64, 34), 0);
                        this.FocusCuesBrush2 = new Pen(Color.FromArgb(255, 64, 34), 1);
                        this.TextBrush = new SolidBrush(Color.White);
                        this.DisabledTextBrush = new SolidBrush(Color.FromArgb(85, 85, 85));
                        this.HoveringTextBrush = new SolidBrush(Color.White);
                        this.Background = new SolidBrush(Color.FromArgb(255, 87, 34));
                        this.DisabledBackground = new SolidBrush(Color.FromArgb(255, 107, 54));
                        this.HoveringBackground = new SolidBrush(Color.FromArgb(255, 104, 34));
                        this.FocusedBackground = new SolidBrush(Color.FromArgb(255, 64, 34));
                        break;
                    case ButtonStyle.Return:
                        this.BorderBrush = new Pen(Color.FromArgb(190, 190, 190), 0);
                        this.FocusCuesBrush1 = new Pen(Color.FromArgb(170, 170, 170), 0);
                        this.FocusCuesBrush2 = new Pen(Color.FromArgb(170, 170, 170), 1);
                        this.TextBrush = new SolidBrush(Color.White);
                        this.DisabledTextBrush = new SolidBrush(Color.FromArgb(85, 85, 85));
                        this.HoveringTextBrush = new SolidBrush(Color.White);
                        this.Background = new SolidBrush(Color.FromArgb(190, 190, 190));
                        this.DisabledBackground = new SolidBrush(Color.FromArgb(217, 217, 217));
                        this.HoveringBackground = new SolidBrush(Color.FromArgb(210, 210, 210));
                        this.FocusedBackground = new SolidBrush(Color.FromArgb(170, 170, 170));
                        break;
                    case ButtonStyle.OK:
                        this.BorderBrush = new Pen(Color.FromArgb(42, 131, 113), 0);
                        this.FocusCuesBrush1 = new Pen(Color.FromArgb(42, 131, 113), 0);
                        this.FocusCuesBrush2 = new Pen(Color.FromArgb(42, 131, 113), 1);
                        this.TextBrush = new SolidBrush(Color.White);
                        this.DisabledTextBrush = new SolidBrush(Color.White);
                        this.HoveringTextBrush = new SolidBrush(Color.White);
                        this.Background = new SolidBrush(Color.FromArgb(42, 131, 113));
                        this.DisabledBackground = new SolidBrush(Color.FromArgb(217, 217, 217));
                        this.HoveringBackground = new SolidBrush(Color.FromArgb(51, 144, 125));
                        this.FocusedBackground = new SolidBrush(Color.FromArgb(42, 131, 113));
                        break;
                    case ButtonStyle.FF22A7:
                        this.BorderBrush = new Pen(Color.FromArgb(255, 34, 167), 0);
                        this.FocusCuesBrush1 = new Pen(Color.FromArgb(255, 34, 167), 0);
                        this.FocusCuesBrush2 = new Pen(Color.FromArgb(255, 34, 167), 1);
                        this.TextBrush = new SolidBrush(Color.White);
                        this.DisabledTextBrush = new SolidBrush(Color.White);
                        this.HoveringTextBrush = new SolidBrush(Color.White);
                        this.Background = new SolidBrush(Color.FromArgb(255, 34, 167));
                        this.DisabledBackground = new SolidBrush(Color.FromArgb(255, 34, 167));
                        this.HoveringBackground = new SolidBrush(Color.FromArgb(255, 34, 167));
                        this.FocusedBackground = new SolidBrush(Color.FromArgb(255, 34, 167));
                        break;
                    case ButtonStyle.FF5722:
                        this.BorderBrush = new Pen(Color.FromArgb(255, 87, 34), 0);
                        this.FocusCuesBrush1 = new Pen(Color.FromArgb(255, 87, 34), 0);
                        this.FocusCuesBrush2 = new Pen(Color.FromArgb(255, 87, 34), 1);
                        this.TextBrush = new SolidBrush(Color.White);
                        this.DisabledTextBrush = new SolidBrush(Color.White);
                        this.HoveringTextBrush = new SolidBrush(Color.White);
                        this.Background = new SolidBrush(Color.FromArgb(255, 87, 34));
                        this.DisabledBackground = new SolidBrush(Color.FromArgb(255, 87, 34));
                        this.HoveringBackground = new SolidBrush(Color.FromArgb(255, 87, 34));
                        this.FocusedBackground = new SolidBrush(Color.FromArgb(255, 87, 34));
                        break;
                    default: break;
                }
            }

            /// <summary>
            /// 获取按钮边框样式
            /// </summary>
            internal Pen BorderBrush { get; private set; }

            /// <summary>
            /// 获取按钮获取焦点时的样式
            /// </summary>
            internal Pen FocusCuesBrush1 { get; private set; }

            /// <summary>
            /// 获取按钮获取焦点时的样式
            /// </summary>
            internal Pen FocusCuesBrush2 { get; private set; }

            /// <summary>
            /// 获取按钮文字样式
            /// </summary>
            internal Brush TextBrush { get; private set; }

            /// <summary>
            /// 获取按钮悬停时的文字样式
            /// </summary>
            internal Brush HoveringTextBrush { get; private set; }

            /// <summary>
            /// 获取按钮不可用的文字样式
            /// </summary>
            internal Brush DisabledTextBrush { get; private set; }

            /// <summary>
            /// 获取按钮背景色
            /// </summary>
            internal Brush Background { get; private set; }

            /// <summary>
            /// 获取按钮悬停时的背景色
            /// </summary>
            internal Brush HoveringBackground { get; private set; }

            /// <summary>
            /// 获取按钮不可用时的背景色
            /// </summary>
            internal Brush DisabledBackground { get; private set; }

            /// <summary>
            /// 获取按钮获得焦点时的背景色
            /// </summary>
            internal Brush FocusedBackground { get; private set; }
        }
    }

    /// <summary>
    /// 按钮风格
    /// </summary>
    public enum ButtonStyle
    {
        /// <summary>
        /// 绿色风格
        /// </summary>
        Green,

        /// <summary>
        /// 灰色风格
        /// </summary>
        Gray,

        /// <summary>
        /// 查询样式
        /// </summary>
        Query,

        /// <summary>
        /// 保存样式
        /// </summary>
        Save,

        /// <summary>
        /// 删除样式
        /// </summary>
        Del,
        /// <summary>
        /// 日历样式
        /// </summary>
        Calendar_day,
        /// <summary>
        /// 日历周末样式
        /// </summary>
        Calendar_weekend,
        /// <summary>
        /// 日历今日样式
        /// </summary>
        Calendar_today,
        /// <summary>
        /// 返回样式
        /// </summary>
        Return,
        /// <summary>
        /// OK(确定)样式
        /// </summary>
        OK,
        /// <summary>
        /// 放号审核按钮样式
        /// </summary>
        FF5722,
        /// <summary>
        /// 普通特需互转按钮样式
        /// </summary>
        FF22A7

    }
}
