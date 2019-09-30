using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.RtCall.Model
{
    /// <summary>
    /// 记录主键帮助类
    /// </summary>
    public class HelperClass
    {
        /// <summary>
        /// 分诊记录主键,用于呼号到诊、完成下一位、过号下一位时
        /// </summary>
        public static String triageId { get; set; }
        /// <summary>
        /// 当前医生登录工号
        /// </summary>
        public static String Code { set; get; }
        /// <summary>
        /// 诊室ID
        /// </summary>
        public static String clinicId { get; set; }
        /// <summary>
        /// 医院主键
        /// </summary>
        public static String hospitalId { get; set; }
            //get
            //{
            //    return string.Join(",", from w in list where w.code == AppContext.AppConfig.hospitalCode select w.id);
            //}
        
        /// <summary>
        /// 科室主键
        /// </summary>
        public static String deptId { get; set; }
            //get
            //{
            //    return string.Join(",", from d in Departmentlist where d.code == AppContext.AppConfig.deptCode select d.id);
            //}
        
        /// <summary>
        /// 医生主键
        /// </summary>
        public static String doctorId { get; set; }
        /// <summary>
        /// 医院集合
        /// </summary>
        public static List<HelperClassDoctor> list { get; set; }
        /// <summary>
        /// 科室列表集合
        /// </summary>
        public static List<HelperClassDoctorID> Departmentlist { get; set; }
        /// <summary>
        /// 医生列表集合
        /// </summary>
        public static List<HelperClinc> doctorlist { get; set; }
        /// <summary>
        /// 诊室列表集合
        /// </summary>
        public static List<HelperClinc> clinclist { get; set; }
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
    /// 科室数据
    /// </summary>
    public class HelperClassDoctorID
    {
        public String code { get; set; }
        public String id { get; set; }
        public String name { get; set; }
    }
    /// <summary>
    /// 诊室数据
    /// </summary>
    public class HelperClinc
    {
        public String code { get; set; }
        public String id { get; set; }
        public String name { get; set; }
    }
}
