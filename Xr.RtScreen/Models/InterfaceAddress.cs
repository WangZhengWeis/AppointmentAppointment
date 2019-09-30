using System;
using System.Collections.Generic;
using System.Linq;

namespace Xr.RtScreen.Models
{
    /// <summary>
    /// 接口地址
    /// </summary>
    public class InterfaceAddress
    {
        /// <summary>
        /// 医院列表接口地址
        /// </summary>
        public const String hostalInfo = "itf/intro/hospital/findAll";
        /// <summary>
        /// 医院接口地址
        /// </summary>
        public const String hostal = "itf/intro/hospitalByCode";
        /// <summary>
        /// 科室接口地址
        /// </summary>
        public const String dept = "itf/call/deptIsInstal";
        /// <summary>
        /// 诊室接口地址
        /// </summary>
        public const String clin = "itf/triage/findDoctorIdByClinicName";
        /// <summary>
        /// 获取候诊屏的主键
        /// </summary>
        public const String screenLogin = "itf/screen/screenLogin";
        /// <summary>
        /// 诊室列表接口
        /// </summary>
        public const String ClincInfo = "itf/call/findCliniList";
        /// <summary>
        /// 公共大屏接口地址
        /// </summary>
        public const String findPublicScreenData = "itf/screen/findPublicScreenData";
        /// <summary>
        /// 诊室小屏（方案一）接口地址
        /// </summary>
        public const String findRoomScreenDataOne = "itf/screen/findRoomScreenDataOne";
        /// <summary>
        /// 诊室小屏（方案二）接口地址
        /// </summary>
        public const String findRoomScreenDataTwo = "itf/screen/findRoomScreenDataTwo";
        /// <summary>
        /// 语音呼号记录接口地址
        /// </summary>
        public const String findCallList = "itf/screen/findCallList";
        /// <summary>
        /// 科室候诊说明（大屏）接口地址
        /// </summary>
        public const String findWaitingDesc = "itf/screen/findWaitingDesc";
        /// <summary>
        ///
        /// </summary>
        public const String findCallListByScreen = "itf/screen/findCallListByScreen";
        /// <summary>
        /// 科室医生说明
        /// </summary>
        public const String doctorScheduPlanList = "itf/screen/doctorScheduPlanList";
    }
}
