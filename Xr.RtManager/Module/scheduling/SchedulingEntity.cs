using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.RtManager
{
    /// <summary>
    /// 排班表
    /// </summary>
    public class SchedulingEntity
    {
        /// <summary>
        /// 医院id
        /// </summary>
        public String hospitalId { get; set; }

        /// <summary>
        /// 科室id
        /// </summary>
        public String deptId { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public String deptName { get; set; }

        /// <summary>
        /// 医生id
        /// </summary>
        public String doctorId { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public String doctorName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public String date { get; set; }

        /// <summary>
        /// 时段 0:上午 1：下午 2：晚上 3：全天
        /// </summary>
        public String period { get; set; }

        /// <summary>
        /// 门诊类型，1：普通门诊、2：特需门诊
        /// </summary>
        public String mzType { get; set; }
    }
}
