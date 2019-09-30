using System;
using System.Collections.Generic;
using System.Linq;

namespace Xr.RtScreen.Models
{
    /// <summary>
    /// 大屏帮助类
    /// </summary>
    public class ScreenClass
    {
        public String nextPatient { get; set; }
        public String name { get; set; }
        public String waitPatient { get; set; }
        public String bespeakNum { get; set; }
        public String visitPatient { get; set; }
        public String isStop { get; set; }
        public String waitNum { get; set; }
    }
    /// <summary>
    /// 科室小屏
    /// </summary>
    public class SmallScreenClass
    {
        public String waitNum { get; set; }
        public String nextPatient { get; set; }
        public String waitingDesc { get; set; }
        public String clinicName { get; set; }
        public String doctorName { get; set; }
        public String waitPatient { get; set; }
        public String visitPatient { get; set; }
    }
    /// <summary>
    /// 医生小屏
    /// </summary>
    public class DoctorSmallScreenClass
    {
        public String doctorExcellence { get; set; }
        public String doctorIntro { get; set; }
        public String nextPatient { get; set; }
        public String clinicName { get; set; }
        public String doctorName { get; set; }
        public String waitPatient { get; set; }
        public String doctorSpecialty { get; set; }
        public String doctorJob { get; set; }
        public String visitPatient { get; set; }
        public String doctorHeader { get; set; }
    }

    public class StardIsFrom
    {
        public String hospitalId { get; set; }
        public String deptId { get; set; }
        public String clinicId { get; set; }
    }
}
