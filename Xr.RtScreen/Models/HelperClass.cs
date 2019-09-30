using System;
using System.Collections.Generic;
using System.Linq;

namespace Xr.RtScreen.Models
{
    public class HelperClass
    {
        /// <summary>
        /// 叫号文本
        /// </summary>
        public static String cellText { get; set; }
        /// <summary>
        /// 诊室ID
        /// </summary>
        public static String clincId { get;
            set; }
        /// <summary>
        /// 医院主键
        /// </summary>
        public static String hospitalId { get;
            set; }
        /// <summary>
        /// 科室主键
        /// </summary>
        public static String deptId { get;
            set; }
        /// <summary>
        /// 科室列表集合
        /// </summary>
        public static List<HelperClassDoctor> list { get; set; }
        public static List<HelperClinc> DepartmentList { get; set; }
        public static List<HelperClinc> ClincList { get; set; }
    }
    /// <summary>
    /// 医院介绍
    /// </summary>
    public class HelperClassDoctor
    {
        public String address { get; set; }
        public String code { get; set; }
        public String hospitalType { get; set; }
        public String id { get; set; }
        public String isUse { get; set; }
        public String logoUrl { get; set; }
        public String name { get; set; }
        public String pictureUrl { get; set; }
        public String telPhoneNo { get; set; }
    }
    /// <summary>
    /// 科室列表
    /// </summary>
    public class HelperClinc
    {
        public String code { get; set; }
        public String id { get; set; }
        public String name { get; set; }
    }
}
