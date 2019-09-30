using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Xr.RtScreen
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                var aProcessName = Process.GetCurrentProcess().ProcessName;
                if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("Interval"))
                {
                    SetAppSetting("Interval", "80");
                }
                if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("Font"))
                {
                    SetAppSetting("Font", "38");
                }
                if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("PageSize"))
                {
                    SetAppSetting("PageSize", "10");
                }
                if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("PageSize") && System.Configuration.ConfigurationManager.AppSettings["PageSize"] == "10")
                {
                    SetAppSetting("PageSize", "8");
                }
                if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("RollingTime"))
                {
                    SetAppSetting("RollingTime", "6");
                }
                if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("IsTextDate"))
                {
                    SetAppSetting("IsTextDate", "false");
                }
                if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("IsOpenVioce"))
                {
                    SetAppSetting("IsOpenVioce", "false");
                }
                if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("VoicePlayTime"))
                {
                    SetAppSetting("VoicePlayTime", "20");
                }
                if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("RefreshTimeWaitingDesc"))
                {
                    SetAppSetting("RefreshTimeWaitingDesc", "60");
                }
                else
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["RefreshTimeWaitingDesc"] != "60")
                    {
                        SetAppSetting("RefreshTimeWaitingDesc", "60");
                    }
                }
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                if ((Process.GetProcessesByName(aProcessName)).GetUpperBound(0) > 0)
                {
                    MessageBox.Show(@"系统已经在运行中，如果要重新启动，请从进程中关闭...", @"系统警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Xr.RtScreen.Models.AppContext.Load();
                    Xr.Log4net.LogHelper.InitLog();
                    if (Xr.RtScreen.Models.AppContext.AppConfig.Setting == "1")
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
                LogClass.WriteLog("Main:" + ex);
                MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : string.Empty)) + ",请重启程序。");
            }
        }
        public static void SetAppSetting(string key, string value)
        {
            if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                var map = new System.Configuration.ExeConfigurationFileMap()
                { ExeConfigFilename = Environment.CurrentDirectory +
                        @"\Xr.RtScreen.exe.config" };
                var config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(map, System.Configuration.ConfigurationUserLevel.None);
                config.AppSettings.Settings.Add(key, value);
                config.Save();
            }
            else
            {
                var map = new System.Configuration.ExeConfigurationFileMap()
                { ExeConfigFilename = Environment.CurrentDirectory +
                        @"\Xr.RtScreen.exe.config" };
                var cfa = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(map, System.Configuration.ConfigurationUserLevel.None);
                cfa.AppSettings.Settings[key].Value = value;
                cfa.Save();
            }
        }
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            if (ex != null)
            {
                LogClass.WriteLog("Application_ThreadException:" + ex);
            }

            MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : string.Empty)));
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                LogClass.WriteLog("CurrentDomain_UnhandledException:" + ex);
            }

            MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : string.Empty)));
        }
    }
}
