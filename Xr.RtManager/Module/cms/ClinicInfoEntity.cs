using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    /// <summary>
    /// 诊室信息
    /// </summary>
    public class ClinicInfoEntity
    {
        public String id { get; set; }
        [ObjectPoint("hospital.id")]
        public String hospitalId
        {
            get
            {
                return AppContext.Session.hospitalId;
            }
        }
        [ObjectPoint("dept.id")]
        public String deptId{get;set;}
        [IgnoreParam]
        public String deptname { get; set; }
        public String name { get; set; }
        [IgnoreParam]
        public String code { get; set; }
        [IgnoreParam]
        public String prefix { get; set; }
        public String isUse { get; set; }
        [IgnoreParam]
        public String createDate { get; set; }
        [IgnoreParam]
        public String updateDate { get; set; }
        //[IgnoreParam]
        public String isOccupy { get; set; }
    }
}
