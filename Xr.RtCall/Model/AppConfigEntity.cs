using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.RtCall.Model
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
        public String ClincName { get; set; }
        /// <summary>
        /// 是否启动Socket(true , false)
        /// </summary>
        public String StartUpSocket { get; set; }
        /// <summary>
        /// 患者ID注入的X轴距
        /// </summary>
        public String OutPutLocationX { get; set; }
        /// <summary>
        /// 患者ID注入的Y轴距
        /// </summary>
        public String OutPutLocationY { get; set; }
        /// <summary>
        /// 注入患者ID休眠的时间
        /// </summary>
        public String sleepOutPutTime { get; set; }
        /// <summary>
        /// 是否启动注入患者ID(true , false)
        /// </summary>
        public String WhetherToAssign { get; set; }
        /// <summary>
        /// 标识是否是第一次启动
        /// </summary>
        public String Setting { get; set; }
        /// <summary>
        /// 是否显示完成就诊按钮
        /// </summary>
        public String WhetherToDisplay { get; set; }
        /// <summary>
        /// 刷新时间间隔
        /// </summary>
        public String RefreshTime { get; set; }
        /// <summary>
        /// 是否启用了医生工作站
        /// </summary>
        public String IsStart { get; set; }
        /// <summary>
        /// 允许医生的诊间预约是否只看自己的（true为是，false为否）
        /// </summary>
        public String IsDoctorClinic { get; set; }
    }
}
