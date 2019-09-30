using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class AppConfigEntity
    {
        /// <summary>
        /// 服务器测试库ip+端口+公共路径
        /// </summary>
        public String serverUrl { get; set; }

        /// <summary>
        /// 医院编码
        /// </summary>
        [Required]
        public String hospitalCode { get; set; }

        /// <summary>
        /// 小票打印机名字
        /// </summary>
        public String PrinterName { get; set; }

        /// <summary>
        /// 第一次启动 1：是 0：否
        /// </summary>
        public String firstStart { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public String pagesize { get; set; }
        /// <summary>
        /// 挂号签到界面刷新时间
        /// </summary>
       // public String RefreshTime { get; set; }
        /// <summary>
        /// 服务器测试库ip+端口
        /// </summary>
        public String serverIp { get; set; }

        /// <summary>
        /// 登录后是否弹出更新公告 1：是 0：否
        /// </summary>
        public String showUpdateNotice { get; set; }
        /// <summary>
        /// 小票打印指定科室名
        /// </summary>
        public String LocalDeptName { get; set; }
        /// <summary>
        /// 是否弹框选择复诊签到 0:不用弹框 1:弹框选择
        /// </summary>
        public String CheckInAgain { get; set; }
        
    }
}
