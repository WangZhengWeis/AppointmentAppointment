using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    /// <summary>
    /// 节假日
    /// </summary>
    public class HolidayInfoEntity
    {
        public String id { set; get; }
        /// <summary>
        /// 医院id
        /// </summary>
        [Required]
        [ObjectPoint("hospital.id")]
        public String hospitalid
        {
            get
            {
                return AppContext.Session.hospitalId;
            }
        }
        [Required]
        public String name { get; set; }
        [Required]
        public String year { get; set; }
        [Required]
        public String beginDate { get; set; }
        [Required]
        public String endDate { get; set; }
        [Required]
        public String isUse { get; set; }
    }
}
