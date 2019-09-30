using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.RtManager.Module.triage
{
    /// <summary>
    /// 异步操作实体
    /// </summary>
    public class AsyncEntity
    {
        public AsynchronousWorks WorkType { get; set; }
        public String[] Argument { get; set; }
        public String IsResulf { get; set; }
    }
    /// <summary>
    /// 异步返回结果
    /// </summary>
    public class SycResult
    {
        public AsynchronousWorks WorkType { get; set; }
        public bool result { get; set; }
        public string msg { get; set; }
        public object obj { get; set; }
        public object args { get; set; }
    }
    /// <summary>
    /// 异步操作枚举
    /// </summary>
    public enum AsynchronousWorks
    {
        IsResulf,
        /// <summary>
        /// 获取可挂号科室
        /// </summary>
        QueryDept,
        /// <summary>
        /// 获取科室可挂号医生
        /// </summary>
        QueryDoctor,
        /// <summary>
        /// 医生可预约日期
        /// </summary>
        QueryDoctorAvailableDate,
        /// <summary>
        /// 医生可预约时间段
        /// </summary>
        QueryDoctorAvailableTimeSpan,
        /// <summary>
        /// 查询患者
        /// </summary>
        QueryID,
        /// <summary>
        /// 查询患者预约信息
        /// </summary>
        QueryPatientReservation,
        /// <summary>
        /// 读取诊疗卡
        /// </summary>
        ReadzlCard,
        /// <summary>
        /// 读取身份证
        /// </summary>
        ReadIdCard,
        /// <summary>
        /// 读取社保卡
        /// </summary>
        ReadSocialcard,
        /// <summary>
        /// 获取科室坐诊列表
        /// </summary>
        RoomListQuery,
        /// <summary>
        /// 预约签到
        /// </summary>
        SingIn,
        /// <summary>
        /// 现场挂号
        /// </summary>
        Register,
        /// <summary>
        /// 分诊记录加急
        /// </summary>
        Urgent,
        /// <summary>
        /// 过号重排
        /// </summary>
        PassNum,
        /// <summary>
        /// 上移一位
        /// </summary>
        MoveUP,
        /// <summary>
        /// 下移一位
        /// </summary>
        MoveDown,
        /// <summary>
        /// 查询报告
        /// </summary>
        QueryLisReport,
        /// <summary>
        /// 复诊签到
        /// </summary>
        ReviewSignIn,
        /// <summary>
        /// 在诊患者转候诊
        /// </summary>
        ConvertWaiting,
        /// <summary>
        /// 取消候诊
        /// </summary>
        CancelWaiting,
        /// <summary>
        /// 获取候诊指引单
        /// </summary>
        WaitingList,
        /// <summary>
        /// 获取候诊患者列表
        /// </summary>
        WaitingPatientList,
        /// <summary>
        /// 该医生当日预约名单
        /// </summary>
        ReservationPatientList,
        /// <summary>
        /// 现场预约
        /// </summary>
        Reservation,
        /// <summary>
        /// 取消现场预约
        /// </summary>
        CancelReservation,
        /// <summary>
        /// 患者转诊
        /// </summary>
        PatientTranDoc,
        Null
    }
}
