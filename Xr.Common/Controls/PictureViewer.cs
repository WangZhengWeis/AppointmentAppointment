using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Xr.Common.Controls
{
    public partial class PictureViewer : Form
    {
        public String imgPathStr { get; set; }

        public PictureViewer()
        {
            InitializeComponent();
        }

        public PictureViewer(Image image)
        {
            InitializeComponent();
            this.pictureBox1.Image = image;
        }

        int tyh = 0;//图片原来的高度
        int tyw = 0;//图片原来的宽度
        int[] bls = new int[]{5, 6, 7, 8, 9, 11, 13, 16, 19, 23, 28, 33, 40, 48, 58, 69, 83, 100, 
            120, 144, 173, 208, 250, 300, 360, 432, 518, 622, 746, 895, 1074, 1289, 1547, 1600};

        private void PictureViewer_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseWheel);
            String imgPath = "";
            //imgPath = "http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-02-20/Teemo_Splash_3.jpg";
            //imgPath = "http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-02-20/QQ图片20190220112758.jpg";
            if (this.pictureBox1.Image != null) { }
            else if (imgPathStr != null)
            {
                imgPath = imgPathStr;
            }
            else
            {
                this.Close();
            }
            try
            {
                if (this.pictureBox1.Image == null)
                {
                    WebClient web = new WebClient();
                    var bytes = web.DownloadData(imgPath);
                    this.pictureBox1.Image = Bitmap.FromStream(new MemoryStream(bytes));
                }
                this.pictureBox1.Height = pictureBox1.Image.Height;
                this.pictureBox1.Width = pictureBox1.Image.Width;
                tyh = pictureBox1.Image.Height;
                tyw = pictureBox1.Image.Width;
                if (this.pictureBox1.Width > 715)
                {
                    this.Width = this.pictureBox1.Width + 5;
                }

                if (this.pictureBox1.Height > 535)
                {
                    this.Height = this.pictureBox1.Height + 5;
                    //panel1.Parent = this.pictureBox1;
                }

                this.pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, (this.Height - pictureBox1.Height) / 2);//居中
                System.Drawing.Rectangle rec = Screen.GetWorkingArea(this);
                int SH = rec.Height;
                int SW = rec.Width;
                this.Location = new Point((SW - this.Width) / 2, (SH - this.Height) / 2);//居中
            }
            catch (Exception ex)
            {
                Xr.Log4net.LogHelper.Error(ex.Message);
            }
        }

        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                label2_Click(null, null);
            }
            else
            {
                label4_Click(null, null);
            }
            //System.Drawing.Size t = pictureBox1.Size;
            //t.Width += e.Delta;
            //t.Height += e.Delta;
            //pictureBox1.Width = t.Width;
            //pictureBox1.Height = t.Height;
        }


        private void PictureViewer_Resize(object sender, EventArgs e)
        {
            this.pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, (this.Height - pictureBox1.Height) / 2);//居中
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left &&
                this.WindowState != FormWindowState.Maximized)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 274, 61440 + 9, 0);
            }
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(200, 247, 126, 117);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PictureViewer_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = PointToClient(MousePosition);
            if (p.Y < this.Height / 4)
            {
                panel1.Visible = true;
            }
            else
            {
                panel1.Visible = false;
            }

            if (p.Y > this.Height * 3 / 4)
            {
                panel2.Visible = true;
            }
            else
            {
                panel2.Visible = false;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = PointToClient(MousePosition);
            if (p.Y < this.Height / 4)
            {
                panel1.Visible = true;
            }
            else
            {
                panel1.Visible = false;
            }

            if (p.Y > this.Height * 3 / 4)
            {
                panel2.Visible = true;
            }
            else
            {
                panel2.Visible = false;
            }
        }

        int blIndex = 17;
        /// <summary>
        /// 扩大图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label2_Click(object sender, EventArgs e)
        {
            if (blIndex == 33) return;
            else
            {
                blIndex++;
                this.pictureBox1.Height = tyh * bls[blIndex] / 100;
                this.pictureBox1.Width = tyw * bls[blIndex] / 100;
                this.pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, (this.Height - pictureBox1.Height) / 2);//居中
                label3.Text = bls[blIndex] + "%";
            }
            //int bl = int.Parse(label3.Text.Replace("%",""));
            //if (bl * 1.2 >1600)
            //{
            //    this.pictureBox1.Height = tyh*16;
            //    this.pictureBox1.Width = tyw*16;
            //    //SetCenterScreen(pictureBox1);
            //    this.pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, (this.Height - pictureBox1.Height) / 2);//居中
            //    label3.Text = "1600%";
            //}
            //else if(bl==1600)
            //{
            //    return;
            //}
            //else
            //{
            //    pictureBox1.SuspendLayout();
            //    pictureBox1.Scale(new SizeF { Width = 1.2f, Height = 1.2f });
            //    //SetCenterScreen(pictureBox1);
            //    this.pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, (this.Height - pictureBox1.Height) / 2);//居中
            //    pictureBox1.ResumeLayout();
            //    double dbl = double.Parse(bl.ToString()) * 1.2f;
            //    label3.Text = Math.Round(Convert.ToDecimal(dbl), 0, MidpointRounding.AwayFromZero) + "%";
            //}
        }

        /// <summary>
        /// 缩小图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label4_Click(object sender, EventArgs e)
        {
            if (blIndex == 0) return;
            else
            {
                blIndex--;
                this.pictureBox1.Height = tyh * bls[blIndex] / 100;
                this.pictureBox1.Width = tyw * bls[blIndex] / 100;
                this.pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, (this.Height - pictureBox1.Height) / 2);//居中
                label3.Text = bls[blIndex] + "%";
            }
            //int bl = int.Parse(label3.Text.Replace("%", ""));
            //if (bl * 0.87 < 5)
            //{
            //    this.pictureBox1.Height = tyh / 20;
            //    this.pictureBox1.Width = tyw / 20;
            //    //SetCenterScreen(pictureBox1);
            //    this.pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, (this.Height - pictureBox1.Height) / 2);//居中
            //    label3.Text = "5%";
            //}
            //else if (bl == 5)
            //{
            //    return;
            //}
            //else
            //{
            //    pictureBox1.SuspendLayout();
            //    pictureBox1.Scale(new SizeF { Width = 0.83f, Height = 0.83f });
            //    //SetCenterScreen(pictureBox1);
            //    this.pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, (this.Height - pictureBox1.Height) / 2);//居中
            //    pictureBox1.ResumeLayout();
            //    double dbl = double.Parse(bl.ToString()) * 0.83f;
            //    label3.Text = Math.Round(Convert.ToDecimal(dbl), 0, MidpointRounding.AwayFromZero) + "%";
            //}
        }

        //控件居中(相对屏幕居中的)
        void SetCenterScreen(System.Windows.Forms.Control control)
        {
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int targetLocationLeft;
            int targetLocationTop;
            targetLocationLeft = (screenWidth - control.Width) / 2;
            targetLocationTop = (screenHeight - control.Height) / 2;
            if (control.Parent != null)
                control.Location = control.Parent.PointToClient(new Point(targetLocationLeft, targetLocationTop));
            else
                control.Location = new Point(targetLocationLeft, targetLocationTop);
        }

        int yh = 0; //全屏前的高度
        int yw = 0; //全屏前的宽度
        /// <summary>
        /// 全屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label5_Click(object sender, EventArgs e)
        {
            System.Drawing.Rectangle rec=Screen.GetWorkingArea(this);
            int SH=rec.Height;
            int SW=rec.Width;
            if (label5.Text.Equals("全屏"))
            {
                yh = this.Height;
                yw = this.Width;
                label5.Text = "窗口";
                this.Height = SH;
                this.Width = SW;
            }
            else
            {
                label5.Text = "全屏";
                this.Height = yh;
                this.Width = yw;
            }
            this.Location = new Point((SW - this.Width) / 2, (SH - this.Height) / 2);//居中
        }

        [DllImport("user32.dll")]
        public static extern int ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int msg, int up, int lp);

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WindowsMessage.WM_NCHITTEST:
                    this.WM_NCHITTEST(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        public static int LOWORD(int value)
        {
            return value & 0xFFFF;
        }

        public static int HIWORD(int value)
        {
            return value >> 16;
        }

        private void WM_NCHITTEST(ref Message m)
        {
            int wparam = m.LParam.ToInt32();
            Point point = new Point(LOWORD(wparam), HIWORD(wparam));
            point = this.PointToClient(point);

            if (point.X <= 5)
            {
                if (point.Y <= 5)
                    m.Result = (IntPtr)WinAPIConst.HTTOPLEFT;
                else if (point.Y > this.Height - 5)
                    m.Result = (IntPtr)WinAPIConst.HTBOTTOMLEFT;
                else
                    m.Result = (IntPtr)WinAPIConst.HTLEFT;
            }
            else if (point.X >= this.Width - 5)
            {
                if (point.Y <= 5)
                    m.Result = (IntPtr)WinAPIConst.HTTOPRIGHT;
                else if (point.Y >= this.Height - 5)
                    m.Result = (IntPtr)WinAPIConst.HTBOTTOMRIGHT;
                else
                    m.Result = (IntPtr)WinAPIConst.HTRIGHT;
            }
            else if (point.Y <= 5)
            {
                m.Result = (IntPtr)WinAPIConst.HTTOP;
            }
            else if (point.Y >= this.Height - 5)
            {
                m.Result = (IntPtr)WinAPIConst.HTBOTTOM;
            }
            else
                base.WndProc(ref m);
        }

    }
    public enum WindowsMessage
    {
        /// <summary>
        /// 移动鼠标，桉树或释放鼠标时发生
        /// </summary>
        WM_NCHITTEST = 0x84,
    }
    public class WinAPIConst
    {
        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 0x10;//16
        public const int HTBOTTOMRIGHT = 17;
        public const int HTCAPTION = 2;

    }
}
