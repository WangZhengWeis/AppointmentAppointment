using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.AutoUpdate.Log4net
{
    /// <summary>
    /// Log4net日志记录帮助类（日志记录在程序文件夹omsLog(错误信息文件夹)和sysLog(一般信息文件夹)下）
    /// </summary>
    public static class LogHelper
    {
        private static ILog log = LogManager.GetLogger("FileLogger");//一般的日志信息
        private static ILog logs = LogManager.GetLogger("ErrorLogger");//错误的日志信息
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
