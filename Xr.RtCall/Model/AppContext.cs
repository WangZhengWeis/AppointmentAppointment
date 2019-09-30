using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Xr.RtCall.Model
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
            //updateAppConfig();
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
            AppConfig.StartUpSocket = ConfigurationManager.AppSettings["StartUpSocket"].ToString();
            AppConfig.OutPutLocationX = ConfigurationManager.AppSettings["OutPutLocationX"].ToString();
            AppConfig.OutPutLocationY = ConfigurationManager.AppSettings["OutPutLocationY"].ToString();
            AppConfig.sleepOutPutTime = ConfigurationManager.AppSettings["sleepOutPutTime"].ToString();
            AppConfig.WhetherToAssign = ConfigurationManager.AppSettings["WhetherToAssign"].ToString();
            AppConfig.ClincName = ConfigurationManager.AppSettings["ClincName"].ToString();
            AppConfig.Setting = ConfigurationManager.AppSettings["Setting"].ToString();
            AppConfig.WhetherToDisplay = ConfigurationManager.AppSettings["WhetherToDisplay"].ToString();
            AppConfig.RefreshTime = ConfigurationManager.AppSettings["RefreshTime"].ToString();
            AppConfig.IsStart = ConfigurationManager.AppSettings["IsStart"].ToString();
            AppConfig.IsDoctorClinic = ConfigurationManager.AppSettings["IsDoctorClinic"].ToString();
        }
        /// <summary>
        /// 修改配置文件
        /// </summary>
        private static void updateAppConfig()
        {
            //获取自动更新的配置文件，用于读取版本号
            string file = System.Windows.Forms.Application.StartupPath + "\\Xr.AutoUpdate.exe";
            WebConfigHelper config = new WebConfigHelper(file, ConfigType.ExeConfig);
            Double version = System.Convert.ToDouble(config.GetValueByKey("version"));

            //获取本应用程序的配置文件并根据版本号进行修改
            file = System.Windows.Forms.Application.StartupPath + "\\Xr.RtCall.exe";
            WebConfigHelper appConfig = new WebConfigHelper(file, ConfigType.ExeConfig);
            //if (version > 1.0)
            //{
            //    //添加应用程序配置节点，如果已经存在此节点，则不做操作
                appConfig.AddAppSetting("test", "测试");
            //    //添加应用程序配置节点，如果已经存在此节点，则会修改该节点的值  
            //    appConfig.AddOrModifyAppSetting("test1", "测试");
            //    //修改应用程序配置节点，如果不存在此节点，则会添加此节点及对应的值  
            //    appConfig.ModifyAppSetting("test2", "测试");
            //    //保存所作的修改  
                appConfig.Save();
            //}
        }
    }
}
