using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    /// <summary>
    /// 排班信息实体类
    /// </summary>
    public class WorkingDayEntity
    {
        /// <summary>
        /// 科室id
        /// </summary>
        public String deptId { get; set; }

        /// <summary>
        /// 医生id
        /// </summary>
        public String doctorId { get; set; }

        /// <summary>
        /// 周几
        /// </summary>
        public String week { get; set; }

        /// <summary>
        /// 0：上午，1：下午，2：晚上
        /// </summary>
        public String period { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public String isUse { get; set; }

        /// <summary>
        /// 是否自动排班
        /// </summary>
        public String autoSchedule { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public String beginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public String endTime { get; set; }

        /// <summary>
        /// 现场数量
        /// </summary>
        public String numSite { get; set; }

        /// <summary>
        /// 公开号源
        /// </summary>
        public String numOpen { get; set; }

        /// <summary>
        /// 诊间数量
        /// </summary>
        public String numClinic { get; set; }

        /// <summary>
        /// 应急数量
        /// </summary>
        public String numYj { get; set; }

        /// <summary>
        /// 门诊类型，1：普通门诊、2：特需门诊
        /// </summary>
        public String mzType { get; set; }

        /// <summary>
        /// 分段时间
        /// </summary>
        public String segmentalDuration{ get; set; }
    }
}
