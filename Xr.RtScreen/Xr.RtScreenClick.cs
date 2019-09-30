using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Xr.RtScreen.pages;
using System.Configuration;
using Xr.RtScreen.Models;
using Newtonsoft.Json.Linq;
using Xr.Http;
using Xr.Common;

namespace Xr.RtScreen
{
    public partial class Form1 : Form
    {
        public static Form1 pCurrentWin = null;
        public static String ScreenType { get; set; }
        private LodingFrm loadingfrm;
        private SplashScreenManager loading;
        //AutoReSizeForm arsf = new AutoReSizeForm();
        public Form1()
        {
            InitializeComponent();
            int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = Screen.PrimaryScreen.Bounds.Width;
            this.Size = new Size(SW, SH);
            WindowState = FormWindowState.Maximized;
            SetStyle(ControlStyles.ResizeRedraw |
                  ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.Opaque |
                  ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            //arsf.controllInitializeSize(this);//记录变化之前的位置
            pCurrentWin = this;
            loadingfrm = new LodingFrm(this);
            loading = new SplashScreenManager(loadingfrm);
            loading.ShowLoading();
            Log4net.LogHelper.Info("程序启动");
            GetDoctorAndClinc();
            #region 设置显示屏幕
            ScreenType = AppContext.AppConfig.StartupScreen;
            switch (AppContext.AppConfig.StartupScreen)
            {
                case "1":
                    var rcf = new RtScreenFrm();
                    rcf.Dock = DockStyle.Fill;
                    panelControl1.Controls.Add(rcf);
                    break;
                case "2":
                    var rscf = new RtSmallScreenFrm();
                    rscf.Dock = DockStyle.Fill;
                    panelControl1.Controls.Add(rscf);
                    break;
                case "3":
                    var rdscf = new RtDoctorSmallScreenFrm();
                    rdscf.Dock = DockStyle.Fill;
                    panelControl1.Controls.Add(rdscf);
                    break;
                case "4":
                    var ddd = new DepartmentDoctorDescription();
                    ddd.Dock = DockStyle.Fill;
                    panelControl1.Controls.Add(ddd);
                    break;
                default:
                    MessageBoxUtils.Show("配置的启动屏不正确，请检查后重启", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, null);
                    System.Environment.Exit(0);
                    break;
            }
            #endregion
        }
        #region 注释内容（用不上）
        //public void SetDeptCode()
        //{
        //    try
        //    {
        //        if (AppContext.AppConfig.deptCode == "1030400")
        //        {
        //            if (AppContext.AppConfig.deptCode != "1040300,1030400,1030601,1030602,1030603,1030604")
        //            {
        //                var map = new ExeConfigurationFileMap()
        //                { ExeConfigFilename = Environment.CurrentDirectory +
        //                        @"\Xr.RtScreen.exe.config" };
        //                var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        //                config.AppSettings.Settings["deptCode"].Value = "1040300,1030400,1030601,1030602,1030603,1030604";
        //                config.Save();
        //                ConfigurationManager.RefreshSection("appSettings");
        //                AppContext.Load();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log4net.LogHelper.Error("设置科室编码错误信息：" + ex.Message);
        //    }
        //}
        #endregion
        #region 获取主键
        /// <summary>
        /// 获取主键
        /// </summary>
        public void GetDoctorAndClinc()
        {
            try
            {
                var urls = string.Empty;
                if (AppContext.AppConfig.StartupScreen != "1" && AppContext.AppConfig.StartupScreen != "4")
                {
                    urls = AppContext.AppConfig.serverUrl + InterfaceAddress.screenLogin + "?hospitalCode=" + AppContext.AppConfig.hospitalCode + "&deptCode=" + AppContext.AppConfig.deptCode + "&clinicName=" + AppContext.AppConfig.clinicName;
                }
                else
                {
                    urls = AppContext.AppConfig.serverUrl + InterfaceAddress.screenLogin + "?hospitalCode=" + AppContext.AppConfig.hospitalCode + "&deptCode=" + AppContext.AppConfig.deptCode;
                }
                var datass = HttpClass.httpPost(urls);
                var objTss = JObject.Parse(datass);
                if (string.Compare(objTss["state"].ToString(), "true", true) == 0)
                {
                    var list = objTss["result"].ToObject<List<StardIsFrom>>();
                    HelperClass.hospitalId = list[0].hospitalId;
                    HelperClass.deptId = list[0].deptId;
                    if (list[0].clinicId == null)
                    {
                        HelperClass.clincId = string.Empty;
                    }
                    else
                    {
                        HelperClass.clincId = list[0].clinicId;
                    }
                }
                else
                {
                    loading.CloseWaitForm();
                    MessageBoxUtils.Show(objTss["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, null);
                    Close();
                }
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBoxUtils.Show("程序启动出现错误,请检查后重启", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, null);
                Log4net.LogHelper.Error("叫号获取科室和医院主键错误信息：" + ex.Message);
                Close();
            }
            finally
            {
                loading.CloseWaitForm();
            }
        }
        #endregion
        #region 按钮退出方法
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        /// <summary>
        /// 重写按键监视方法，用于操作窗体
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            var WM_KEYDOWN = 256;
            var WM_SYSKEYDOWN = 260;
            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        if (Xr.Common.MessageBoxUtils.Show("您确定要退出程序吗？", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, this) == DialogResult.OK)
                        {
                            Log4net.LogHelper.Info("退出系统成功");
                            System.Environment.Exit(0);
                        }
                        break;
                }
            }
            return false;
        }
        #endregion
        #region 最大化最小化
        private void 最大化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = Screen.PrimaryScreen.Bounds.Width;
            WindowState = FormWindowState.Normal;
            this.MaximumSize = new Size(SW, SH);
            WindowState = FormWindowState.Maximized;
        }

        private void 最小化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = Screen.PrimaryScreen.Bounds.Width;
            if (SW > 3000)
            {
                this.Size = new Size((SW / 4), (SH / 4));
            }
            else
            {
                this.Size = new Size(1024,600);
            }
        }
        #endregion
        #region 更随鼠标移动
        public enum MouseDirection
        {
            Herizontal,
            Vertical,
            Declining,
            None
        }
        private bool isMouseDown = false;
        private MouseDirection direction = MouseDirection.None;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Location.X >= Width - 5 && e.Location.Y > Height - 5)
            {
                Cursor = Cursors.SizeNWSE;
                direction = MouseDirection.Declining;
            }
            else
            {
                if (e.Location.X >= Width - 5)
                {
                    Cursor = Cursors.SizeWE;
                    direction = MouseDirection.Herizontal;
                }
                else
                {
                    if (e.Location.Y >= Height - 5)
                    {
                        Cursor = Cursors.SizeNS;
                        direction = MouseDirection.Vertical;
                    }
                    else
                    {
                        Cursor = Cursors.Arrow;
                    }
                }
            }
            ResizeWindow();
        }
        private void ResizeWindow()
        {
            if (!isMouseDown)
            {
                return;
            }
            if (direction == MouseDirection.Declining)
            {
                Cursor = Cursors.SizeNWSE;
                Width = MousePosition.X - Left;
                Height = MousePosition.Y - Top;
            }
            if (direction == MouseDirection.Herizontal)
            {
                Cursor = Cursors.SizeWE;
                Width = MousePosition.X - Left;
            }
            else
            {
                if (direction == MouseDirection.Vertical)
                {
                    Cursor = Cursors.SizeNS;
                    Height = MousePosition.Y - Top;
                }
                else
                {
                    Cursor = Cursors.Arrow;
                }
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            direction = MouseDirection.None;
        }
        #endregion
        #region 窗体大小改变事件
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
           // arsf.controlAutoSize(this);//窗体变化位置
        }
        #endregion
    }
}
