using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Xr.RtCall.Model
{
    /// <summary>
    /// 添加和修改配置文件
    /// </summary>
    public class AddAndUpdateConfig
    {
        /// <summary>
        /// 向配置文件中添加配置
        /// </summary>
        /// <param name="name">要添加的Key</param>
        /// <param name="Value">要添加的值</param>
        public static void AddConfig(String ConfigName,String name,String Value)
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap()
            {
                ExeConfigFilename = Environment.CurrentDirectory +
                    @"\"+ConfigName
            };
            Configuration cfa = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            var appSetting = (AppSettingsSection)cfa.GetSection("appSettings");
            if (appSetting.Settings[name] != null)
            {
                //添加
                cfa.AppSettings.Settings.Add(name, Value);
            }
            // 最后调用 
            cfa.Save();
            //当前的配置文件更新成功。
            ConfigurationManager.RefreshSection("appSettings");// 刷新命名节，在下次检索它时将从磁盘重新读取它。记住应用程序要刷新节点
        }
        /// <summary>
        /// 修改配置文件
        /// </summary>
        /// <param name="name">修改的名称</param>
        /// <param name="Value">修改的值</param>
        public static void UpdateConfig(String ConfigName,String name, String Value)
        {
            //更新配置文件 
            //Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ExeConfigurationFileMap map = new ExeConfigurationFileMap()
            {
                ExeConfigFilename = Environment.CurrentDirectory +
                    @"\" + ConfigName
            };
            Configuration cfa = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            //修改
            cfa.AppSettings.Settings[name].Value = Value;
            // 最后调用 
            cfa.Save();
            //当前的配置文件更新成功。
            ConfigurationManager.RefreshSection("appSettings");// 刷新命名节，在下次检索它时将从磁盘重新读取它。记住应用程序要刷新节点
        }
    }
}
