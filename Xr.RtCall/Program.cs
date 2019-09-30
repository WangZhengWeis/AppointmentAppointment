using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xr.RtCall
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
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
                if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("IsStart"))
                {
                    SetAppSetting("IsStart", "true");
                }
                if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("IsDoctorClinic"))
                {
                    SetAppSetting("IsDoctorClinic", "false");
                }
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件
                //Xr.RtCall.Model.IsOpen.IsOpenOrClose = false;
                if ((Process.GetProcessesByName(aProcessName)).GetUpperBound(0) > 0)
                {
                    MessageBox.Show(@"系统已经在运行中，如果要重新启动，请从进程中关闭...", @"系统警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    #region 
                    //if (MessageBox.Show(@"系统已经在运行中，是否需要在启动一个", @"系统警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)==DialogResult.OK)
                    //{
                    //    Xr.RtCall.pages.DoctorCodeFrm dcf = new Xr.RtCall.pages.DoctorCodeFrm();
                    //    dcf.ShowDialog();
                    //}
                    //else
                    //{
                    //    MessageBox.Show(@"系统已经在运行中，如果要重新启动，请从进程中关闭...", @"系统警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    //}
                    #endregion
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Xr.RtCall.Model.AppContext.Load();
                    Xr.Log4net.LogHelper.InitLog();
                    if (Xr.RtCall.Model.AppContext.AppConfig.Setting == "1")
                    {
                        Application.Run(new Form1());
                    }
                    else
                    {
                        Application.Run(new SettingFrm());
                    }
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("Main:" + ex);
                // MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")) + ",请重启程序。");
            }
        }
        #region 向配置文件中添加键值对，有则修改，无则添加
        public static void SetAppSetting(string key, string value)
        {
            if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                System.Configuration.ExeConfigurationFileMap map = new System.Configuration.ExeConfigurationFileMap()
                {
                    ExeConfigFilename = Environment.CurrentDirectory +
                        @"\Xr.RtCall.exe.config"
                };
                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(map, System.Configuration.ConfigurationUserLevel.None);
                config.AppSettings.Settings.Add(key, value);
                config.Save();
            }
            else
            {
                System.Configuration.ExeConfigurationFileMap map = new System.Configuration.ExeConfigurationFileMap()
                {
                    ExeConfigFilename = Environment.CurrentDirectory +
                        @"\Xr.RtCall.exe.config"
                };
                System.Configuration.Configuration cfa = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(map, System.Configuration.ConfigurationUserLevel.None);
                cfa.AppSettings.Settings[key].Value = value;
                cfa.Save();
            }
        }
        #endregion
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            if (ex != null)
            {
                Log4net.LogHelper.Error("Application_ThreadException:" + ex);
            }

            MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")));
            Application.ExitThread();
            Application.Exit();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                Log4net.LogHelper.Error("CurrentDomain_UnhandledException:" + ex);
            }

            MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")));
            Application.ExitThread();
            Application.Exit();
        }
    }
}
