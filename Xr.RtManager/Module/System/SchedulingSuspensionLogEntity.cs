using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    public class SchedulingSuspensionLogEntity
    {
        /// <summary>
        /// 医院名称
        /// </summary>
        public String hospitalName { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public String deptName { get; set; }

        /// <summary>
        /// 医生名称
        /// </summary>
        public String doctorName { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public String workDate { get; set; }

        /// <summary>
        /// 时间段代码
        /// </summary>
        public String periodTxt { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public String operatTime { get; set; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public String operatorName { get; set; }
    }
}
