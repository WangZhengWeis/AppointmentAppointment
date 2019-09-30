using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace Xr.RtCall.Model
{
    public class WebConfigHelper
    {
        private Configuration _config;
        private readonly string _configPath;
        private readonly ConfigType _configType;

        /// <summary>  
        /// 对应的配置文件  
        /// </summary>  
        public Configuration Configuration
        {
            get { return _config; }
            set { _config = value; }
        }

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
            this._configPath = configPath;
            this._configType = configType;
            Initialize();
        }

        //实例化configuration,根据配置文件类型的不同，分别采取了不同的实例化方法  
        private void Initialize()
        {
            //如果是WinForm应用程序的配置文件  

            if (_configType == ConfigType.ExeConfig)
            {
                _config = ConfigurationManager.OpenExeConfiguration(_configPath);
            }
            else //WebForm的配置文件  
            {
                _config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(_configPath);
            }
        }

        public string GetValueByKey(string key)
        {
            //return ConfigurationManager.AppSettings[key]; //这个有些值取不到 
            return _config.AppSettings.Settings[key].Value;
        }

        /// <summary>  
        /// 添加应用程序配置节点，如果已经存在此节点，则不做操作  
        /// </summary>  
        /// <param name="key">节点名称</param>  
        /// <param name="value">节点值</param>  
        public void AddAppSetting(string key, string value)
        {
            var appSetting = (AppSettingsSection)_config.GetSection("appSettings");
            if (appSetting.Settings[key] == null) //如果不存在此节点，则添加  
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
            var appSetting = (AppSettingsSection)_config.GetSection("appSettings");
            if (appSetting.Settings[key] == null) //如果不存在此节点，则添加  
            {
                appSetting.Settings.Add(key, value);
            }
            else //如果存在此节点，则修改  
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
            var appSetting = (AppSettingsSection)_config.GetSection("appSettings");
            if (appSetting.Settings[key] != null) //如果存在此节点，则修改  
            {
                appSetting.Settings[key].Value = newValue;
            }
            else //如果不存在此节点，则添加  
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
            var connectionSetting = (ConnectionStringsSection)_config.GetSection("connectionStrings");
            if (connectionSetting.ConnectionStrings[key] == null) //如果不存在此节点，则添加  
            {
                var connectionStringSettings = new ConnectionStringSettings(key, connectionString);
                connectionSetting.ConnectionStrings.Add(connectionStringSettings);
            }
            else //如果存在此节点，则修改  
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
            var connectionSetting = (ConnectionStringsSection)_config.GetSection("connectionStrings");
            if (connectionSetting.ConnectionStrings[key] != null) //如果存在此节点，则修改  
            {
                connectionSetting.ConnectionStrings[key].ConnectionString = connectionString;
            }
            else //如果不存在此节点，则添加  
            {
                AddConnectionString(key, connectionString);
            }
        }

        /// <summary>  
        /// 保存所作的修改  
        /// </summary>  
        public void Save()
        {
            _config.Save();
        }
    }
    public enum ConfigType
    {
        /// <summary>  
        /// asp.net网站的config文件  
        /// </summary>  
        WebConfig = 1,
        /// <summary>  
        /// Windows应用程序的config文件  
        /// </summary>  
        ExeConfig = 2
    }  
}
