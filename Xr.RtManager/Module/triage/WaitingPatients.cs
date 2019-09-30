using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.RtManager.Module.triage
{
    /// <summary>
    /// 转诊弹窗的预约患者实体类
    /// </summary>
    public class WaitingPatientsEntity
    {

        private string _check = "0";
        /// <summary>
        /// 是否选中
        /// </summary>
        public string check
        {
            get { return _check; }
            set { _check = value; }
        }

        /// <summary>
        /// 预约id
        /// </summary>
        public String triageId { get; set; }

        /// <summary>
        /// 预约医生
        /// </summary>
        public String doctorName { get; set; }

        /// <summary>
        /// 预约号
        /// </summary>
        public String queueNum { get; set; }

        /// <summary>
        /// 预约患者名称
        /// </summary>
        public String patientName { get; set; }

        /// <summary>
        /// 预约日期
        /// </summary>
        public String workDate { get; set; }

        /// <summary>
        /// 预约时间段
        /// </summary>
        public String workTime { get; set; }
    }
}
