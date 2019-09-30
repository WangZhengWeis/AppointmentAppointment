using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Xr.Common;
using Xr.Common.Controls;


namespace Xr.Common.Internal
{
    internal partial class MessageBoxForm : BaseForm
    {
        private const int MessageMargin = 20;     //消息内容距离窗体的边距
        private const int MessageMaxSize = 2500;  //允许显示的最大消息长度
        private const int ButtonMargin = 8;       //按钮与容器控件的边距
        private const int ButtonSpace = 8;        //按钮之间的边距
        private const int MaxHeight = 600;        //对话框最大高度
        private const int MinHeight = 150;        //对话框最小高度
        private const int MaxWidth = 800;         //对话框最大宽度
        private const int MinWidth = 450;         //对话框最小宽度

        private List<ButtonControl> buttons = new List<ButtonControl>();

        private Control controlP; //将窗体显示在此控件的中间，为null则显示在屏幕中间

        public MessageBoxForm()
        {
            InitializeComponent();
        }

        public MessageBoxForm(Control control)
        {
            InitializeComponent();
            controlP = control;
        }

        public string Message { get; set; }

        public string Caption
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public MessageBoxButtons Buttons { get; set; }

        public MessageBoxIcon MessageIcon { get; set; }

        public MessageBoxDefaultButton DefaultButton { get; set; }

        public string[] ButtonTexts { get; set; }

        public void Initialize()
        {
            lcMessage.Text = this.Message;

            CreateButtons();     //创建对话框按钮
            SetMessageIcon();
            SetCancelButton();   //确定按Esc键触发的对话框按钮             
            LayoutControls();
        }

        /// <summary>
        /// 创建按钮
        /// </summary>
        private void CreateButtons()
        {
            //创建按钮
            switch (this.Buttons)
            {
                case MessageBoxButtons.AbortRetryIgnore:
                    CreateButton(0, "中止", DialogResult.Abort);
                    CreateButton(1, "重试", DialogResult.Retry);
                    CreateButton(2, "忽略", DialogResult.Ignore);
                    break;

                case MessageBoxButtons.OK:
                    CreateButton(0, "确定", DialogResult.OK);
                    break;

                case MessageBoxButtons.OKCancel:
                    CreateButton(0, "确定", DialogResult.OK);
                    CreateButton(1, "取消", DialogResult.Cancel);
                    break;

                case MessageBoxButtons.RetryCancel:
                    CreateButton(0, "重试", DialogResult.Retry);
                    CreateButton(1, "取消", DialogResult.Cancel);
                    break;

                case MessageBoxButtons.YesNo:
                    CreateButton(0, "是", DialogResult.Yes);
                    CreateButton(1, "否", DialogResult.No);
                    break;

                case MessageBoxButtons.YesNoCancel:
                    CreateButton(0, "是", DialogResult.Yes);
                    CreateButton(1, "否", DialogResult.No);
                    CreateButton(2, "取消", DialogResult.Cancel);
                    break;

                default:
                    break;
            }

            //标记默认按钮(默认按钮修改样式暂不需要)
            //switch (this.DefaultButton)
            //{
            //    case MessageBoxDefaultButton.Button1:
            //        if (buttons.Count > 0) buttons[0].Style = ButtonStyle.Green;
            //        break;

            //    case MessageBoxDefaultButton.Button2:
            //        if (buttons.Count > 1) buttons[1].Style = ButtonStyle.Green;
            //        break;

            //    case MessageBoxDefaultButton.Button3:
            //        if (buttons.Count > 2) buttons[2].Style = ButtonStyle.Green;
            //        break;

            //    default:
            //        break;
            //}
        }

        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <param name="index">按钮序号</param>
        /// <param name="defaultText">按钮默认文本</param>
        /// <param name="dialogResult">按钮对应的返回值</param>
        /// <returns></returns>
        private ButtonControl CreateButton(int index, string defaultText, DialogResult dialogResult)
        {
            var button = new ButtonControl();
            button.Text = GetButtonText(index, defaultText);
            button.DialogResult = dialogResult;
            button.Click += Button_Click;
            if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
            {
                button.Style = ButtonStyle.OK;
            }
            else if (dialogResult == DialogResult.No || dialogResult == DialogResult.Cancel)
            {
                button.Style = ButtonStyle.Return;
            }
            pcButtons.Controls.Add(button);
            buttons.Add(button);

            return button;
        }

        /// <summary>
        /// 获取按钮文本
        /// </summary>
        /// <param name="index">按钮序号</param>
        /// <param name="defaultText">按钮默认文本</param>
        /// <returns></returns>
        private string GetButtonText(int index, string defaultText)
        {
            if (this.ButtonTexts != null && this.ButtonTexts.Length > index)
            {
                return this.ButtonTexts[index];
            }

            return defaultText;
        }

        /// <summary>
        /// 设置对话框图标
        /// </summary>
        private void SetMessageIcon()
        {
            switch (this.MessageIcon)
            {
                case MessageBoxIcon.Asterisk:
                    pbIcon.Image = SystemIcons.Asterisk.ToBitmap();
                    break;

                case MessageBoxIcon.Error:
                    pbIcon.Image = SystemIcons.Error.ToBitmap();
                    break;

                case MessageBoxIcon.Exclamation:
                    pbIcon.Image = SystemIcons.Exclamation.ToBitmap();
                    break;

                case MessageBoxIcon.None:
                    break;

                case MessageBoxIcon.Question:
                    pbIcon.Image = SystemIcons.Question.ToBitmap();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 设置取消按钮，当按下Esc键时，自动触发该按钮
        /// </summary>
        private void SetCancelButton()
        {
            if (buttons.Count > 0)
            {
                var lastButton = buttons[buttons.Count - 1];
                if (lastButton.DialogResult == DialogResult.No || lastButton.DialogResult == DialogResult.Cancel)
                {
                    this.CancelButton = lastButton;
                }
            }
        }

        /// <summary>
        /// 执行默认按钮事件
        /// </summary>
        private void ExecuteDefaultButton()
        {
            switch (this.DefaultButton)
            {
                case MessageBoxDefaultButton.Button1:
                    if (buttons.Count > 0) buttons[0].PerformClick();
                    break;

                case MessageBoxDefaultButton.Button2:
                    if (buttons.Count > 1) buttons[1].PerformClick();
                    break;

                case MessageBoxDefaultButton.Button3:
                    if (buttons.Count > 2) buttons[2].PerformClick();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 将错误信息复制到剪贴板
        /// </summary>
        private void CopyMessageToClipboard()
        {
            Clipboard.SetText(this.Message);
        }

        private void LayoutControls()
        {
            //计算窗体的尺寸
            if (lcMessage.Right + MessageMargin > MaxWidth)
            {
                lcMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
                lcMessage.Width = MaxWidth - MessageMargin - lcMessage.Left;

                this.Width = MaxWidth;
                this.Height = Math.Max(lcMessage.Top + lcMessage.Height + MessageMargin*2 + pcButtons.Height, MinHeight);
                if (this.Height > MaxHeight)
                {
                    this.Height = MaxHeight;
                    lcMessage.Height = pcContent.Height - MessageMargin - lcMessage.Top;
                }
            }
            else
            {
                this.Width = Math.Max(lcMessage.Left + lcMessage.Width + MessageMargin, MinWidth);
                this.Height = MinHeight;
                String[] arr = lcMessage.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length - 2 > 0)//默认是有两行的，所以要超过一个换行符才加高度
                {
                    Graphics vGraphics = CreateGraphics();
                    SizeF vSizeF = vGraphics.MeasureString("测试", lcMessage.Font);
                    int wzHeight = Convert.ToInt32(Math.Ceiling(vSizeF.Height));
                    this.Height = MinHeight + wzHeight * (arr.Length - 2);
                }
            }

            //计算按钮区域占据的宽度
            int buttonAreaWidth = (buttons.Count - 1) * ButtonSpace;
            buttonAreaWidth += buttons.Sum(button => button.Width);

            int buttonLeft = (this.Width - buttonAreaWidth) / 2;
            foreach (var button in buttons)
            {
                button.Left = buttonLeft;
                button.Top = (pcButtons.Height - button.Height) / 2;
                buttonLeft += button.Width + ButtonSpace;
            }

            if (controlP == null)
            {
                var workingArea = Screen.GetWorkingArea(this);
                this.Location = new Point(workingArea.Left + (workingArea.Width - this.Width) / 2,
                                          workingArea.Top + (workingArea.Height - this.Height) / 2);
            }
            else
            {
                //Point p = controlP.PointToScreen(controlP.Location);
                this.Location = new Point((controlP.Width - this.Width) / 2 + controlP.Location.X, 
                    (controlP.Height - this.Height) / 2 + controlP.Location.Y);//相对程序居中
            }
            
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var button = sender as ButtonControl;
            if (button != null)
            {
                this.DialogResult = button.DialogResult;
            }
        }

        private void MessageBoxForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.None)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
                {
                    ExecuteDefaultButton();
                }
            }

            if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.C)
                {
                    CopyMessageToClipboard();
                }
            }

        }
    }
}
