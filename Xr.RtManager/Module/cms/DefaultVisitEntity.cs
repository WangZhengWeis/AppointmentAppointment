using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    /// <summary>
    /// 默认出诊时间实体类
    /// </summary>
    public class DefaultVisitEntity
    {
        /// <summary>
        /// 上午开始到结束(08:00-12:00)
        /// </summary>
        public String defaultVisitTimeAm { get; set; }

        /// <summary>
        /// 下午开始到结束(14:00-18:00)
        /// </summary>
        public String defaultVisitTimePm { get; set; }

        /// <summary>
        /// 晚上开始到结束(14:00-18:00)
        /// </summary>
        public String defaultVisitTimeNight { get; set; }

        /// <summary>
        /// 全天开始到结束(8:00-18:00)
        /// </summary>
        public String defaultVisitTimeAllDay { get; set; }

        /// <summary>
        /// 分段时间
        /// </summary>
        public String segmentalDuration { get; set; }

        /// <summary>
        /// 分配信息(10-4-5-2)
        /// </summary>
        public String defaultSourceNumber { get; set; }

        /// <summary>
        /// 上午开始时间
        /// </summary>
        public String mStart { get; set; }

        /// <summary>
        /// 上午结束时间
        /// </summary>
        public String mEnd { get; set; }

        /// <summary>
        /// 上午分段时间
        /// </summary>
        public String mSubsection { get; set; }

        /// <summary>
        /// 上午现场号源
        /// </summary>
        public String mScene { get; set; }

        /// <summary>
        /// 上午公开号源
        /// </summary>
        public String mOpen { get; set; }

        /// <summary>
        /// 上午诊间号源
        /// </summary>
        public String mRoom { get; set; }

        /// <summary>
        /// 上午应急号源
        /// </summary>
        public String mEmergency { get; set; }

        /// <summary>
        /// 下午开始时间
        /// </summary>
        public String aStart { get; set; }

        /// <summary>
        /// 下午结束时间
        /// </summary>
        public String aEnd { get; set; }

        /// <summary>
        /// 下午分段时间
        /// </summary>
        public String aSubsection { get; set; }

        /// <summary>
        /// 下午现场号源
        /// </summary>
        public String aScene { get; set; }

        /// <summary>
        /// 下午公开号源
        /// </summary>
        public String aOpen { get; set; }

        /// <summary>
        /// 下午诊间号源
        /// </summary>
        public String aRoom { get; set; }

        /// <summary>
        /// 下午应急号源
        /// </summary>
        public String aEmergency { get; set; }

        /// <summary>
        /// 晚上开始时间
        /// </summary>
        public String nStart { get; set; }

        /// <summary>
        /// 晚上结束时间
        /// </summary>
        public String nEnd { get; set; }

        /// <summary>
        /// 晚上分段时间
        /// </summary>
        public String nSubsection { get; set; }

        /// <summary>
        /// 晚上现场号源
        /// </summary>
        public String nScene { get; set; }

        /// <summary>
        /// 晚上公开号源
        /// </summary>
        public String nOpen { get; set; }

        /// <summary>
        /// 晚上诊间号源
        /// </summary>
        public String nRoom { get; set; }

        /// <summary>
        /// 晚上应急号源
        /// </summary>
        public String nEmergency { get; set; }

        /// <summary>
        /// 全天开始时间
        /// </summary>
        public String allStart { get; set; }

        /// <summary>
        /// 全天结束时间
        /// </summary>
        public String allEnd { get; set; }

        /// <summary>
        /// 全天分段时间
        /// </summary>
        public String allSubsection { get; set; }

        /// <summary>
        /// 全天现场号源
        /// </summary>
        public String allScene { get; set; }

        /// <summary>
        /// 全天公开号源
        /// </summary>
        public String allOpen { get; set; }

        /// <summary>
        /// 全天诊间号源
        /// </summary>
        public String allRoom { get; set; }

        /// <summary>
        /// 全天应急号源
        /// </summary>
        public String allEmergency { get; set; }
    }
}
