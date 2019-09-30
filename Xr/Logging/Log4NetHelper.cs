using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/** ***********************************************************************
** 程序集			: HttpHelper
** 作者				: wzw
** 创建时间			: 2019-01-07
** 最后修改者		: wzw
** ***********************************************************************/
namespace Xr.Logging
{
    /// <summary>
    /// 利用log4net来记录日志信息
    /// </summary>
    public static class Log4NetHelper
    {
        #region
        private static log4net.ILog log = log4net.LogManager.GetLogger("FileLogger");
        #endregion
        //app.Config要在配置文件加上  <add key="log4net.config" value="log4net.config"/>
        /// <summary>
        /// 调试信息的日志
        /// </summary>
        /// <param name="str"></param>
        public static void RecordingDebug(string str)
        {
            log.Debug(str);
        }
        /// <summary>
        /// 一般信息的日志
        /// </summary>
        /// <param name="str"></param>
        public static void RecordingInfo(string str)
        {
            log.Info(str);
        }
        /// <summary>
        /// 警告信息的日志
        /// </summary>
        /// <param name="str"></param>
        public static void RecordingWarn(string str)
        {
            log.Info(str);
        }
        /// <summary>
        /// 错误信息的日志
        /// </summary>
        /// <param name="str"></param>
        public static void RecordingError(string str)
        {
            log.Error(str);
        }
        /// <summary>
        /// 致命错误的日志
        /// </summary>
        /// <param name="str"></param>
        public static void RecordingFatal(string str)
        {
            log.Fatal(str);
        }
    }
}
