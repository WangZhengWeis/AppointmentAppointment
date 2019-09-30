using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    /// <summary>
    /// 医生默认出诊设置(默认出诊时间设置模块的那个表格的数据实体)
    /// </summary>
    public class DoctorVSEntity
    {
        /// <summary>
        /// 多选按钮值
        /// </summary>
        public bool check { get; set; }

        /// <summary>
        /// 科室id
        /// </summary>
        public String deptId{ get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public String deptName { get; set; }

        /// <summary>
        /// 医生id
        /// </summary>
        public String doctorId { get; set; }

        /// <summary>
        /// 医生名称
        /// </summary>
        public String doctorName { get; set; }

        /// <summary>
        /// 周一上午
        /// </summary>
        public String mondayMorning { get; set; }

        /// <summary>
        /// 周一下午
        /// </summary>
        public String mondayAfternoon { get; set; }

        /// <summary>
        /// 周一晚上
        /// </summary>
        public String mondayNight { get; set; }

        /// <summary>
        /// 周一全天
        /// </summary>
        public String mondayAllAay { get; set; }

        /// <summary>
        /// 周二上午
        /// </summary>
        public String tuesdayMorning { get; set; }

        /// <summary>
        /// 周二下午
        /// </summary>
        public String tuesdayAfternoon { get; set; }

        /// <summary>
        /// 周二晚上
        /// </summary>
        public String tuesdayNight { get; set; }

        /// <summary>
        /// 周二全天
        /// </summary>
        public String tuesdayAllAay { get; set; }

        /// <summary>
        /// 周三上午
        /// </summary>
        public String wednesdayMorning { get; set; }

        /// <summary>
        /// 周三下午
        /// </summary>
        public String wednesdayAfternoon { get; set; }

        /// <summary>
        /// 周三晚上
        /// </summary>
        public String wednesdayNight { get; set; }

        /// <summary>
        /// 周三全天
        /// </summary>
        public String wednesdayAllAay { get; set; }

        /// <summary>
        /// 周四上午
        /// </summary>
        public String thursdayMorning { get; set; }

        /// <summary>
        /// 周四下午
        /// </summary>
        public String thursdayAfternoon { get; set; }

        /// <summary>
        /// 周四晚上
        /// </summary>
        public String thursdayNight { get; set; }

        /// <summary>
        /// 周四全天
        /// </summary>
        public String thursdayAllAay { get; set; }

        /// <summary>
        /// 周五上午
        /// </summary>
        public String fridayMorning { get; set; }

        /// <summary>
        /// 周五下午
        /// </summary>
        public String fridayAfternoon { get; set; }

        /// <summary>
        /// 周五晚上
        /// </summary>
        public String fridayNight { get; set; }

        /// <summary>
        /// 周五全天
        /// </summary>
        public String fridayAllAay { get; set; }

        /// <summary>
        /// 周六上午
        /// </summary>
        public String saturdayMorning { get; set; }

        /// <summary>
        /// 周六下午
        /// </summary>
        public String saturdayAfternoon { get; set; }

        /// <summary>
        /// 周六晚上
        /// </summary>
        public String saturdayNight { get; set; }

        /// <summary>
        /// 周六全天
        /// </summary>
        public String saturdayAllAay { get; set; }

        /// <summary>
        /// 周日上午
        /// </summary>
        public String sundayMorning { get; set; }

        /// <summary>
        /// 周日下午
        /// </summary>
        public String sundayAfternoon { get; set; }

        /// <summary>
        /// 周日晚上
        /// </summary>
        public String sundayNight { get; set; }

        /// <summary>
        /// 周日全天
        /// </summary>
        public String sundayAllAay { get; set; }

        /// <summary>
        /// 周一上午
        /// </summary>
        public String mondayMorningValue { get; set; }

        /// <summary>
        /// 周一下午
        /// </summary>
        public String mondayAfternoonValue { get; set; }

        /// <summary>
        /// 周一晚上
        /// </summary>
        public String mondayNightValue { get; set; }

        /// <summary>
        /// 周一全天
        /// </summary>
        public String mondayAllAayValue { get; set; }

        /// <summary>
        /// 周二上午
        /// </summary>
        public String tuesdayMorningValue { get; set; }

        /// <summary>
        /// 周二下午
        /// </summary>
        public String tuesdayAfternoonValue { get; set; }

        /// <summary>
        /// 周二晚上
        /// </summary>
        public String tuesdayNightValue { get; set; }

        /// <summary>
        /// 周二全天
        /// </summary>
        public String tuesdayAllAayValue { get; set; }

        /// <summary>
        /// 周三上午
        /// </summary>
        public String wednesdayMorningValue { get; set; }

        /// <summary>
        /// 周三下午
        /// </summary>
        public String wednesdayAfternoonValue { get; set; }

        /// <summary>
        /// 周三晚上
        /// </summary>
        public String wednesdayNightValue { get; set; }

        /// <summary>
        /// 周三全天
        /// </summary>
        public String wednesdayAllAayValue { get; set; }

        /// <summary>
        /// 周四上午
        /// </summary>
        public String thursdayMorningValue { get; set; }

        /// <summary>
        /// 周四下午
        /// </summary>
        public String thursdayAfternoonValue { get; set; }

        /// <summary>
        /// 周四晚上
        /// </summary>
        public String thursdayNightValue { get; set; }

        /// <summary>
        /// 周四全天
        /// </summary>
        public String thursdayAllAayValue { get; set; }

        /// <summary>
        /// 周五上午
        /// </summary>
        public String fridayMorningValue { get; set; }

        /// <summary>
        /// 周五下午
        /// </summary>
        public String fridayAfternoonValue { get; set; }

        /// <summary>
        /// 周五晚上
        /// </summary>
        public String fridayNightValue { get; set; }

        /// <summary>
        /// 周五全天
        /// </summary>
        public String fridayAllAayValue { get; set; }

        /// <summary>
        /// 周六上午
        /// </summary>
        public String saturdayMorningValue { get; set; }

        /// <summary>
        /// 周六下午
        /// </summary>
        public String saturdayAfternoonValue { get; set; }

        /// <summary>
        /// 周六晚上
        /// </summary>
        public String saturdayNightValue { get; set; }

        /// <summary>
        /// 周六全天
        /// </summary>
        public String saturdayAllAayValue { get; set; }

        /// <summary>
        /// 周日上午
        /// </summary>
        public String sundayMorningValue { get; set; }

        /// <summary>
        /// 周日下午
        /// </summary>
        public String sundayAfternoonValue { get; set; }

        /// <summary>
        /// 周日晚上
        /// </summary>
        public String sundayNightValue { get; set; }

        /// <summary>
        /// 周日全天
        /// </summary>
        public String sundayAllAayValue { get; set; }
    }
}
