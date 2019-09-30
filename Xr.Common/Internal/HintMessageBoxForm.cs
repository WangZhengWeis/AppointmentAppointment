using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Xr.Common.Properties;

namespace Xr.Common.Internal
{
    internal partial class HintMessageBoxForm : Form
    {
        private const int Radius = 5;       //窗体圆角半角

        public bool flag = false; //是否自适应大小
        //Form PraentFrm;
        public HintMessageBoxForm()
        {
            InitializeComponent();
            // PraentFrm = praentFrm;
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
            switch (this.MessageBoxIcon)
            {
                case HintMessageBoxIcon.Error:
                    pbIcon.Image = Resources.HintMessageError;
                    break;

                default:
                    pbIcon.Image = Resources.HintMessageSuccess;
                    break;
            }

            timer1.Enabled = true;
        }

        private void HintMessageBoxForm_Resize(object sender, EventArgs e)
        {
            SetRegion();
        }
        private bool timerClose = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timerClose = true;
            Close();
        }

        private void HintMessageBoxForm_Deactivate(object sender, EventArgs e)
        {
            if (!this.KeepAliveOnOuterClick)
            {
                timerClose = false;
                IntPtr ParenthWnd = new IntPtr(0);
                IntPtr ParenthWnd1 = new IntPtr(0);
                IntPtr ParenthWnd2 = new IntPtr(0);
                ParenthWnd = FindWindow(null, "预约分诊系统");
                ParenthWnd1 = FindWindow(null, "叫号");
                ParenthWnd2 = FindWindow(null, "候诊屏");
                if (ParenthWnd1 == IntPtr.Zero && ParenthWnd2 == IntPtr.Zero)
                {
                    //判断这个窗体是否有效
                    if (ParenthWnd != IntPtr.Zero)
                    {
                        RECT rect = new RECT();
                        GetWindowRect(ParenthWnd, ref rect);
                        int width = rect.Right - rect.Left;                        //窗口的宽度
                        int height = rect.Bottom - rect.Top;                   //窗口的高度
                        int x = width + rect.Left - 10;
                        int y = height + rect.Top - 10;
                        int cx = Cursor.Position.X + 1;
                        int cy = Cursor.Position.Y + 1;
                        RECT rect2 = new RECT() { Top = rect.Top, Left = rect.Left, Bottom = rect.Top+30, Right = rect.Right };
                        if (!IsPointIn(rect2, new Point(cx, cy)))
                        {
                            mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, x * 65535 / Screen.PrimaryScreen.Bounds.Width, y * 65535 / Screen.PrimaryScreen.Bounds.Height, 0, 0);//移动到需要点击的位置
                            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_ABSOLUTE, x * 65535 / Screen.PrimaryScreen.Bounds.Width, y * 65535 / Screen.PrimaryScreen.Bounds.Height, 0, 0);//点击
                            mouse_event(MOUSEEVENTF_LEFTUP | MOUSEEVENTF_ABSOLUTE, x * 65535 / Screen.PrimaryScreen.Bounds.Width, y * 65535 / Screen.PrimaryScreen.Bounds.Height, 0, 0);//抬起
                            mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, cx * 65535 / Screen.PrimaryScreen.Bounds.Width, cy * 65535 / Screen.PrimaryScreen.Bounds.Height, 0, 0);//移回到点击前的位置
                        }
                        //MessageBox.Show("找到窗口");
                    }
                }
                Close();
            }
        }
        public static bool IsPointIn(RECT rect, Point pt)
        {
            if (pt.X >= rect.Left && pt.Y >= rect.Top && pt.X <= rect.Right && pt.Y <= rect.Bottom)
            {
                return true;
            }
            else return false;
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
                        if (sizeF.Width > 220)
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
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        private readonly int MOUSEEVENTF_LEFTDOWN = 0x0002;//模拟鼠标移动
        private readonly int MOUSEEVENTF_MOVE = 0x0001;//模拟鼠标左键按下
        private readonly int MOUSEEVENTF_LEFTUP = 0x0004;//模拟鼠标左键抬起
        private readonly int MOUSEEVENTF_ABSOLUTE = 0x8000;//鼠标绝对位置
        private readonly int MOUSEEVENTF_RIGHTDOWN = 0x0008; //模拟鼠标右键按下 
        private readonly int MOUSEEVENTF_RIGHTUP = 0x0010; //模拟鼠标右键抬起 
        private readonly int MOUSEEVENTF_MIDDLEDOWN = 0x0020; //模拟鼠标中键按下 
        private readonly int MOUSEEVENTF_MIDDLEUP = 0x0040;// 模拟鼠标中键抬起 
        /// <summary>
        /// 获得当前前台窗体句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;                             //最左坐标
            public int Top;                             //最上坐标
            public int Right;                           //最右坐标
            public int Bottom;                        //最下坐标
        }
        /// <summary>
        /// 根据窗体的类名和窗口的名称获得目标窗体
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// 找到窗体后对其的简单处理，比如开关，隐藏
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        private void HintMessageBoxForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //IntPtr ParenthWnd = new IntPtr(0);
            //ParenthWnd = FindWindow(null, "HintMessageBoxForm");
            ////判断这个窗体是否有效
            //if (ParenthWnd != IntPtr.Zero)
            //{
            //    ShowWindow(ParenthWnd,8);
            //    //MessageBox.Show("找到窗口");
            //}
            //else
            //MessageBox.Show("没有找到窗口");
            //if (!timerClose)
            //{
            //    //模拟鼠标点击一下让后面主窗体马上获取焦点
            //    int cx = Cursor.Position.X + 1;
            //    int cy = Cursor.Position.Y + 1;
            //    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_ABSOLUTE, cx * 65535 / Screen.PrimaryScreen.Bounds.Width, cy * 65535 / Screen.PrimaryScreen.Bounds.Height, 0, 0);//点击
            //    mouse_event(MOUSEEVENTF_LEFTUP | MOUSEEVENTF_ABSOLUTE, cx * 65535 / Screen.PrimaryScreen.Bounds.Width, cy * 65535 / Screen.PrimaryScreen.Bounds.Height, 0, 0);//抬起
            //}
        }
    }
}
