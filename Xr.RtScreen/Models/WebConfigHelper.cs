using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Xr.RtScreen.Models
{
    public class WebConfigHelper
    {
        private readonly string _configPath;
        private readonly ConfigType _configType;

        /// <summary>  
        /// 对应的配置文件  
        /// </summary>  
        public Configuration Configuration { get; set; }

        /// <summary>  
        /// 构造函数  
        /// </summary>  
        public WebConfigHelper()
        {
            _configPath = HttpContext.Current.Request.ApplicationPath;
            Initialize();
        }

        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="configPath">.config文件的位置</param>  
        /// <param name="configType">.config文件的类型，只能是网站配置文件或者应用程序配置文件</param>  
        public WebConfigHelper(string configPath, ConfigType configType)
        {
            _configPath = configPath;
            _configType = configType;
            Initialize();
        }

        private void Initialize()
        {
            if (_configType == ConfigType.ExeConfig)
            {
                Configuration = ConfigurationManager.OpenExeConfiguration(_configPath);
            }
            else 
            {
                Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(_configPath);
            }
        }

        public string GetValueByKey(string key)
        {
            return Configuration.AppSettings.Settings[key].Value;
        }

        /// <summary>  
        /// 添加应用程序配置节点，如果已经存在此节点，则不做操作  
        /// </summary>  
        /// <param name="key">节点名称</param>  
        /// <param name="value">节点值</param>  
        public void AddAppSetting(string key, string value)
        {
            var appSetting = (AppSettingsSection)Configuration.GetSection("appSettings");
            if (appSetting.Settings[key] == null) 
            {
                appSetting.Settings.Add(key, value);
            }
        }

        /// <summary>  
        /// 添加应用程序配置节点，如果已经存在此节点，则会修改该节点的值  
        /// </summary>  
        /// <param name="key">节点名称</param>  
        /// <param name="value">节点值</param>  
        public void AddOrModifyAppSetting(string key, string value)
        {
            var appSetting = (AppSettingsSection)Configuration.GetSection("appSettings");
            if (appSetting.Settings[key] == null) 
            {
                appSetting.Settings.Add(key, value);
            }
            else 
            {
                ModifyAppSetting(key, value);
            }
        }

        /// <summary>  
        /// 修改应用程序配置节点，如果不存在此节点，则会添加此节点及对应的值  
        /// </summary>  
        /// <param name="key">节点名称</param>  
        /// <param name="newValue">节点值</param>  
        public void ModifyAppSetting(string key, string newValue)
        {
            var appSetting = (AppSettingsSection)Configuration.GetSection("appSettings");
            if (appSetting.Settings[key] != null) 
            {
                appSetting.Settings[key].Value = newValue;
            }
            else 
            {
                AddAppSetting(key, newValue);
            }
        }

        /// <summary>  
        /// 添加数据库连接字符串节点，如果已经存在此节点，则会修改该节点的值  
        /// </summary>  
        /// <param name="key">节点名称</param>  
        /// <param name="connectionString">节点值</param>  
        public void AddConnectionString(string key, string connectionString)
        {
            var connectionSetting = (ConnectionStringsSection)Configuration.GetSection("connectionStrings");
            if (connectionSetting.ConnectionStrings[key] == null) 
            {
                var connectionStringSettings = new ConnectionStringSettings(key, connectionString);
                connectionSetting.ConnectionStrings.Add(connectionStringSettings);
            }
            else 
            {
                ModifyConnectionString(key, connectionString);
            }
        }

        /// <summary>  
        /// 修改数据库连接字符串节点，如果不存在此节点，则会添加此节点及对应的值  
        /// </summary>  
        /// <param name="key">节点名称</param>  
        /// <param name="connectionString">节点值</param>  
        public void ModifyConnectionString(string key, string connectionString)
        {
            var connectionSetting = (ConnectionStringsSection)Configuration.GetSection("connectionStrings");
            if (connectionSetting.ConnectionStrings[key] != null) 
            {
                connectionSetting.ConnectionStrings[key].ConnectionString = connectionString;
            }
            else 
            {
                AddConnectionString(key, connectionString);
            }
        }

        /// <summary>  
        /// 保存所作的修改  
        /// </summary>  
        public void Save()
        {
            Configuration.Save();
        }
    }
    public enum ConfigType
    {
        /// <summary>  
        /// Windows应用程序的config文件  
        /// </summary>  
        ExeConfig = 2,
        /// <summary>  
        /// asp.net网站的config文件  
        /// </summary>  
        WebConfig = 1
    } 
       
}
