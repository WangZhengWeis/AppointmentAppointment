using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    /// <summary>
    /// 医生信息实体类
    /// </summary>
    public class DoctorInfoEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public String name { get; set; }

        /// <summary>
        /// 编码(工号)
        /// </summary>
        [Required]
        public String code { get; set; }

        /// <summary>
        /// 性别 
        /// </summary>
        [Required]
        public String sex { get; set; }

        /// <summary>
        /// 节假日显示排班(忽略假期 0：忽略，1：不忽略)
        /// </summary>
        public String ignoreHoliday { get; set; }

        /// <summary>
        /// 忽略春节 0：忽略，1：不忽略, 默认0
        /// </summary>
        public String ignoreYear { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Required]
        public int sort { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public String job { get; set; }

        /// <summary>
        /// 是否显示编码
        /// </summary>
        [Required]
        public String isShow { get; set; }

        /// <summary>
        /// 是否启用编码
        /// </summary>
        [Required]
        public String isUse { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        [IgnoreParam]
        public DeptInfoEntity dept { get; set; }

        /// <summary>
        /// 医院id
        /// </summary>
        [Required]
        [ObjectPoint("hospital.id")]
        public String hospitalId { get; set; }

        /// <summary>
        /// 科室id
        /// </summary>
        [Required]
        [ObjectPoint("dept.id")]
        public String deptId { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public String telPhone { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public String mobile { get; set; }

        /// <summary>
        /// 加号数量
        /// </summary>
        public String addNum { get; set; }

        /// <summary>
        /// 挂号类型
        /// </summary>
        [Required]
        public String workrankid { get; set; }

        /// <summary>
        /// 挂号价格
        /// </summary>
        public String price { get; set; }

        /// <summary>
        /// 出诊地址
        /// </summary>
        public String address { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public String specialty { get; set; }

        /// <summary>
        /// 特长
        /// </summary>
        public String excellence { get; set; }

        /// <summary>
        /// 预约提示
        /// </summary>
        public String tipsMsg { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Required]
        public String synopsis { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Required]
        public String pictureUrl { get; set; }

        /// <summary>
        /// 微信openId
        /// </summary>
        public String wxOpenId { get; set; }

        /// <summary>
        /// 候诊说明
        /// </summary>
        public String waitingDesc { get; set; }

        /// <summary>
        /// 是否开放 0:开放预约 1：不开放预约
        /// </summary>
        [Required]
        public String isOpen { get; set; }

        /// <summary>
        /// 多选按钮值
        /// </summary>
        [IgnoreParam]
        public bool check { get; set; }

        /// <summary>
        /// his医生编码
        /// </summary>
        public String hisDoctorCode { get; set; }
    }
}
