using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Log4net
{
    /// <summary>
    /// Log4net日志记录帮助类
    /// </summary>
    public static class LogHelper
    {
        private static ILog log = LogManager.GetLogger("FileLogger");//一般的日志信息
        private static ILog logs = LogManager.GetLogger("ErrorLogger");//错误的日志信息
        #region 删除文件
        private static log4net.ILog _log = null;
        private static log4net.ILog _logs = null;
        /// <summary>
        /// 日志文件过期时间(单位:天)，默认一个月=30天
        /// </summary>
        private static int outdate_days = 30;

        private static object lockHelper = new object();
        private static log4net.ILog InnerLog
        {
            get
            {
                if (null == _log)
                {
                    lock (lockHelper)
                    {
                        if (null == _log)
                        {
                            try
                            {
                                string cfgfile = "Log4net//log4net.config";//当前(exe文件所在)目录
                                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(cfgfile));
                                _log = LogManager.GetLogger("FileLogger");
                                _logs = LogManager.GetLogger("ErrorLogger");
                                #region 删除过期的日期文件
                                System.Threading.Thread th = new System.Threading.Thread(() =>
                                {
                                    try
                                    {
                                        DeleteOutdatefiles(_log, cfgfile);
                                        DeleteOutdatefiles(_logs, cfgfile);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                });
                                th.IsBackground = true;
                                th.Start();
                                #endregion
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
                return _log;
            }
        }
        /// <summary>
        /// 删除过期的日志文件
        /// </summary>
        private static void DeleteOutdatefiles(ILog log, string cfgfile)
        {
            if (log == null ||
                !log.IsInfoEnabled ||
                !log.IsErrorEnabled)
            {
                return;
            }

            try
            {
                System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
                xdoc.Load(cfgfile);
                var node = xdoc.SelectSingleNode("log4net");
                //附带属性到log4net.config，只能在根节点设置添加。
                //这个附带属性设置还不能放到其它节点，放到其它节点log4net解析会报错（虽未影响结果，但看着不舒服）
                if (null != node || node.Attributes != null || node.Attributes.Count > 0)
                {
                    outdate_days = int.Parse(node.Attributes["outdate_days"].Value);
                }
            }
            catch (Exception)
            {
            }
            var apps = log.Logger.Repository.GetAppenders();
            if (apps.Length <= 0)
            {
                return;
            }
            var now = DateTime.Now;
            foreach (var item in apps)
            {
                if (item is log4net.Appender.RollingFileAppender)
                {
                    log4net.Appender.RollingFileAppender roll = item as log4net.Appender.RollingFileAppender;
                    var dir = System.IO.Path.GetDirectoryName(roll.File);
                    var files = System.IO.Directory.GetFiles(dir, "*.log");
                   // var sample = "log.txt2017-10-23.txt";
                    foreach (var file in files)
                    {
                        var name = System.IO.Path.GetFileName(file);
                        var ss = new string[]{};
                        if (name.Contains("sys"))
                        {
                             ss = name.Substring(3, 10).Split('-');
                        }
                        else
                        {
                             ss = name.Substring(0, 10).Split('-');
                        }
                        int year = int.Parse(ss[0]);
                        int month = int.Parse(ss[1]);
                        int day = int.Parse(ss[2]);
                        var date = new DateTime(year, month, day);
                        //TimeZone、TimeZoneInfo
                        TimeSpan ts = now - date;
                        if (ts.Days >= outdate_days)
                        {
                            try
                            {
                                System.IO.File.Delete(file);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
        }
        public static void InitLog()
        {
            ILog l = InnerLog;
        }
        #endregion 
        /// <summary>
        /// 一般信息的日志
        /// </summary>
        /// <param name="text"></param>
        public static void Info(string text)
        {
            log.Info(text);
        }
        /// <summary>
        /// 调试信息的日志
        /// </summary>
        /// <param name="text"></param>
        public static void Debug(string text)
        {
            log.Debug(text);
        }
        /// <summary>
        /// 警告信息的日志
        /// </summary>
        /// <param name="text"></param>
        public static void Warn(string text)
        {
            logs.Warn(text);
        }
        /// <summary>
        /// 错误信息的日志
        /// </summary>
        /// <param name="text"></param>
        public static void Error(string text)
        {
            logs.Error(text);
        }
        /// <summary>
        /// 重载错误信息方法为Exception类型
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            StringBuilder exMsg = new StringBuilder();
            exMsg.Append("程序异常信息：")
                 .Append("\r\n   " + ex.StackTrace.Trim().Replace("位置", "\r\n   在 触发文件："))
                 .Append("\r\n   在 触发方法：" + ex.TargetSite)
                 .Append("\r\n   在 异常信息：" + ex.Message);
            logs.Error(exMsg.ToString());
        }
        /// <summary>
        /// 致命错误的日志
        /// </summary>
        /// <param name="text"></param>
        public static void Fatal(string text)
        {
            logs.Fatal(text);
        }
    }
}
