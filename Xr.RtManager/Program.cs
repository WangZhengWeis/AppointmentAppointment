using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Xr;
using Xr.Common;

namespace Xr.RtManager
{
    static class Program
    {
        //static string RunFormFullName
        //{
        //    get
        //    {
        //        string setRunFormFullName = CIPACE.Sys.Configuration.RunFormFullName;
        //        if (setRunFormFullName == null)
        //            setRunFormFullName = DigiForm.SETRUNFORMFULLNAME;

        //        return setRunFormFullName;
        //    }
        //}
        /// <summary>
        ///   应用程序的主入口点。
        /// </summary>
        public static ApplicationContext context;
        private static Form mainForm;

        [STAThread]
        private static void Main()
        {
            try
            {
                //处理未捕获的异常   
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //处理UI线程异常   
                Application.ThreadException += Application_ThreadException;
                //处理非UI线程异常   
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                var aProcessName = Process.GetCurrentProcess().ProcessName;
                if ((Process.GetProcessesByName(aProcessName)).GetUpperBound(0) > 0)
                {
                    MessageBoxUtils.Show(@"系统已经在运行中，如果要重新启动，请从进程中关闭...", @"系统警告", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, mainForm);
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Xr.Log4net.LogHelper.InitLog();
                    context = new ApplicationContext();
                    Application.Idle += Application_Idle; //注册程序运行空闲去执行主程序窗体相应初始化代码
                    Application.Run(context);

                    //Application.EnableVisualStyles();
                    //Application.SetCompatibleTextRenderingDefault(false);
                    ////Application.Run(new MainForm());
                    //LoginForm login = new LoginForm();
                    //login.ShowDialog();
                    //if (login.DialogResult == DialogResult.OK)
                    //{
                    //    Application.Run(new MainForm());
                    //}
                }
            }
            catch (Exception ex)
            {
                Xr.Log4net.LogHelper.Error("Main:" + ex);
                //MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")) + ",请重启程序。");
                MessageBoxUtils.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")) + ",请重启程序。", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, mainForm);
                //DigiForm digiForm = new DigiForm();
                //digiForm.UpdateAppSettings(DigiForm.RUNFORMFULLNAME, DigiForm.LOGINFORMFULLNAME);
            }
        }

        private static void Application_Idle(object sender, EventArgs e)
        {
            Application.Idle -= Application_Idle;
            if (context.MainForm == null)
            {
                AppContext.Load();
                AppContext.AppConfig.serverUrl = ConfigurationManager.AppSettings["serverUrl"].ToString();
                bool jxFlag = true; 
                if (AppContext.AppConfig.firstStart.Equals("1"))
                {
                    SetUpForm setUp = new SetUpForm();
                    setUp.ShowDialog();
                    if (setUp.DialogResult != DialogResult.OK)
                    {
                        jxFlag = false;
                    }
                }
                if (jxFlag)
                {
                    LoginForm login = new LoginForm();
                    login.ShowDialog();
                    if (login.DialogResult == DialogResult.OK)
                    {
                        mainForm = new MainForm();
                        context.MainForm = mainForm;
                        mainForm.Show();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            if (ex != null)
            {
                Xr.Log4net.LogHelper.Error("Application_ThreadException:" + ex);
            }
            if (ex.Message.Equals("远程服务器返回错误: (400) 错误的请求。") || ex.Message.Equals("会话失效，请重启程序")
                || ex.Message.Equals("the remote server returned error.(400) Bad Request.") 
                || ex.Message.Equals("The remote server returned an error:(405) Method Not Allowed."))
            {
                if (MessageBoxUtils.Show("系统出现异常：" + ex.Message + "\r\n请重新登录系统", MessageBoxButtons.OKCancel, new[] { "重新登录", "退出系统" }, mainForm) == DialogResult.OK)
                {
                    //Application.Restart();
                    Application.ExitThread();
                    Restart();
                }
                else
                {
                    //Application.Exit();
                    Application.ExitThread();
                }
                //if (MessageBoxUtils.Show(, MessageBoxButtons.OKCancel,
                // MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                //{
                //    Application.ExitThread();
                //    Restart();
                //}
            }
            else
            {
                MessageBoxUtils.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, mainForm);
            }
            //MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")));
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                Xr.Log4net.LogHelper.Error("CurrentDomain_UnhandledException:" + ex);
            }
            MessageBoxUtils.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, mainForm);
            //MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")));
        }

        /// <summary>
        /// 重启程序
        /// </summary>
        private static void Restart()
        {
            Thread thtmp = new Thread(new ParameterizedThreadStart(run));
            object appName = Application.ExecutablePath;
            int i = 0;
            var aProcessName = Process.GetCurrentProcess().ProcessName;
            while ((Process.GetProcessesByName(aProcessName)).GetUpperBound(0) > 0||i<8)//等待旧进程关闭之前，不开新进程
            {
                Thread.Sleep(2000);
                i++;
            }
            thtmp.Start(appName);
        }

        private static void run(Object obj)
        {
            Process ps = new Process();
            ps.StartInfo.FileName = obj.ToString();
            ps.Start();
        }
        ///// <summary>
        ///// 应用程序的主入口点。
        ///// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    //Application.Run(new MainForm());
        //    LoginForm login = new LoginForm();
        //    login.ShowDialog();
        //    if (login.DialogResult == DialogResult.OK)
        //    {
        //        Application.Run(new MainForm());
        //    }
        //}
    }
}
