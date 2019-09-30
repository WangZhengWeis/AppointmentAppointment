using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Xr.RtScreen.Models
{
    public static class AppContext
    {
        /// <summary>
        /// 本地配置信息
        /// </summary>
        public static AppConfigEntity AppConfig { get; private set; }
        public static void Load()
        {
            loadAppConfig();
        }
        /// <summary>
        /// 加载本地配置信息
        /// </summary>
        private static void loadAppConfig()
        {
            AppConfig = new AppConfigEntity();
            AppConfig.serverUrl = ConfigurationManager.AppSettings["ServerUrl"].ToString();
            AppConfig.hospitalCode = ConfigurationManager.AppSettings["hospitalCode"].ToString();
            AppConfig.deptCode = ConfigurationManager.AppSettings["deptCode"].ToString();
            AppConfig.StartupScreen = ConfigurationManager.AppSettings["StartupScreen"].ToString();
            AppConfig.StartUpSocket = ConfigurationManager.AppSettings["StartUpSocket"].ToString();
            AppConfig.clinicName = ConfigurationManager.AppSettings["clinicName"].ToString();
            AppConfig.RefreshTime = ConfigurationManager.AppSettings["RefreshTime"].ToString();
            AppConfig.Setting = ConfigurationManager.AppSettings["Setting"].ToString();
            AppConfig.RefreshTimeWaitingDesc = ConfigurationManager.AppSettings["RefreshTimeWaitingDesc"].ToString();
            AppConfig.Interval = ConfigurationManager.AppSettings["Interval"].ToString();
            AppConfig.FontSize = ConfigurationManager.AppSettings["Font"].ToString();
            AppConfig.PageSize = ConfigurationManager.AppSettings["PageSize"].ToString();
            AppConfig.RollingTime = ConfigurationManager.AppSettings["RollingTime"].ToString();
            AppConfig.IsTextDate = ConfigurationManager.AppSettings["IsTextDate"].ToString();
            AppConfig.IsOpenVioce = ConfigurationManager.AppSettings["IsOpenVioce"].ToString();
            AppConfig.VoicePlayTime = ConfigurationManager.AppSettings["VoicePlayTime"].ToString();
        }
        /// <summary>
        /// 修改配置文件
        /// </summary>
        private static void updateAppConfig()
        {
            var file = System.Windows.Forms.Application.StartupPath + "\\Xr.AutoUpdate.exe";
            var config = new WebConfigHelper(file, ConfigType.ExeConfig);
            System.Convert.ToDouble(config.GetValueByKey("version"));

            file = System.Windows.Forms.Application.StartupPath + "\\Xr.RtScreen.exe";
        }
    }
}
