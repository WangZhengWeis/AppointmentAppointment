using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.RtManager
{
    /// <summary>
    /// 排班表提交参数
    /// </summary>
    public class SchedulingSubEntity
    {
        /// <summary>
        /// 医生id
        /// </summary>
        public String doctorId { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public String workDate { get; set; }

        /// <summary>
        /// 周几：一、二、三。。。
        /// </summary>
        public String week { get; set; }

        /// <summary>
        /// 时段 0:上午 1：下午 2：晚上 3：全天
        /// </summary>
        public String period { get; set; }

        /// <summary>
        /// 是否排班
        /// </summary>
        public String isPlan { get; set; }


        /// <summary>
        /// 门诊类型，1：普通门诊、2：特需门诊
        /// </summary>
        public String mzType { get; set; }
    }
}
