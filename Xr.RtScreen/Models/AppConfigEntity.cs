using System;
using System.Collections.Generic;
using System.Linq;

namespace Xr.RtScreen.Models
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class AppConfigEntity
    {
        /// <summary>
        /// 服务器测试库ip+端口
        /// </summary>
        public String serverUrl { get; set; }
        /// <summary>
        /// 医院编码
        /// </summary>
        public String hospitalCode { get; set; }
        /// <summary>
        /// 科室编码
        /// </summary>
        public String deptCode { get; set; }
        /// <summary>
        /// 诊室名称
        /// </summary>
        public String clinicName { get; set; }
        /// <summary>
        /// 启动对应的屏幕窗口1（公共大屏）2（科室小屏）3（医生小屏）
        /// </summary>
        public String StartupScreen { get; set; }
        /// <summary>
        /// 是否启动Socket
        /// </summary>
        public String StartUpSocket { get; set; }
        /// <summary>
        /// 刷新时间
        /// </summary>
        public String RefreshTime { get; set; }
        /// <summary>
        /// 标识是否是第一次启动
        /// </summary>
        public String Setting { get; set; }
        /// <summary>
        /// 刷新候诊说明的时间间隔 单位 分钟
        /// </summary>
        public String RefreshTimeWaitingDesc { get; set; }
        /// <summary>
        /// 每行的高度
        /// </summary>
        public String Interval { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public String FontSize { get; set; }
        /// <summary>
        /// 每页显示的条数
        /// </summary>
        public String PageSize { get; set; }
        /// <summary>
        /// 每页数据刷新时间
        /// </summary>
        public String RollingTime { get; set; }
        /// <summary>
        /// 是否启用测试数据显示
        /// </summary>
        public String IsTextDate { get; set; }
        /// <summary>
        /// 是否打开语音播放窗体
        /// </summary>
        public String IsOpenVioce { get; set; }
        /// <summary>
        /// 滚动的时间
        /// </summary>
        public String VoicePlayTime { get; set; }
    }
}
