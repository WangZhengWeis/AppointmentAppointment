using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.RtManager.Module.triage
{
    /// <summary>
    /// 医生坐诊设置
    /// </summary>
   public class DoctorSittingInfoEntity
    {
       //诊室
       public String clinicName { get; set; }
       //诊室ID
       public String deptId { get; set; }
       //诊室名称
       public String deptName { get; set; }
       //医生ID
       public String doctorId { get; set; }
       //医生姓名
       public String doctorName { get; set; }
       //医院ID 
       public String hospitalId { get; set; }
       //ID
       public String id { get; set; }
       //是否停诊
       public String isStop { get; set; }
       //是否启用
       public String isUse { get; set; }
       //时段(0 上午，1下午，2晚上，3全天)
       public String period { get; set; }
       //坐诊日期
       public String workDate { get; set; }
    }
    /// <summary>
    /// 获取医生坐诊查询
    /// </summary>
   public class DoctorSrtting
   {
       public String deptName { get; set; }
       public String doctorName { get; set; }
       //坐诊日期
       public String workDate { get; set; }
       //坐诊时段中文
       public String periodTxt { get; set; }
       //时段(0 上午，1下午，2晚上，3全天)
       public String period { get; set; }
       //诊室ID
       public String clinicId { get; set; }
       public String doctorId { get; set; }
       public String clinicName { get; set; }
       public String deptId { get; set; }
   }
   public class objJson
   {
       public String deptId { get; set; }
       public String doctorId { get; set; }
       public String workDate { get; set; }
       public String period { get; set; }
       public String clinicId { get; set; }
   }
   public class OveradeJson
   {
       public String deptId { get; set; }
       public String doctorId { get; set; }
       public String workDate { get; set; }
       public List<info> values { get; set; }
   }
   public class info
   {
       public String period { get; set; }
       public String clinicId { get; set; }
   }
}
