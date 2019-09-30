using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.RtManager
{
    /// <summary>
    /// 未放号列表
    /// </summary>
    public class NotOpenEntity
    {
        /// <summary>
        /// 序号
        /// </summary>
        public String num { get; set; }

        /// <summary>
        /// 科室主键
        /// </summary>
        public String deptId { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public String deptName { get; set; }

        /// <summary>
        /// 医生主键
        /// </summary>
        public String doctorId { get; set; }

        /// <summary>
        /// 医生名称
        /// </summary>
        public String doctorName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public String workDate { get; set; }

        /// <summary>
        /// 时段编码
        /// </summary>
        public String period { get; set; }

        /// <summary>
        /// 时段
        /// </summary>
        public String periodText { get; set; }

        /// <summary>
        /// 排班状态、0：正常，1：停诊，2：转诊，3：已删除
        /// </summary>
        public String status { get; set; }

        /// <summary>
        /// 是否开放号源 0是、1否
        /// </summary>
        public String isOpen { get; set; }

        /// <summary>
        /// 上午
        /// </summary>
        public String am { get; set; }

        /// <summary>
        /// 下午
        /// </summary>
        public String pm { get; set; }

        /// <summary>
        /// 晚上
        /// </summary>
        public String night { get; set; }

        /// <summary>
        /// 全天
        /// </summary>
        public String allday { get; set; }
    }
}
