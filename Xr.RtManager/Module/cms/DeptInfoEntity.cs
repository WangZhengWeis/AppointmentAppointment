using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    /// <summary>
    /// 科室信息实体类
    /// </summary>
    public class DeptInfoEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// 医院
        /// </summary>
        [IgnoreParam]
        public HospitalInfoEntity hospital { get; set; }

        /// <summary>
        /// 医院id
        /// </summary>
        [Required]
        [ObjectPoint("hospital.id")]
        public String hospitalId { get; set; }

        /// <summary>
        /// 上级科室
        /// </summary>
        [IgnoreParam]
        public DeptInfoEntity parent { get; set; }

        /// <summary>
        /// 上级科室id
        /// </summary>
        [Required]
        [ObjectPoint("parent.id")]
        public String parentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public String name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        public String code { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public String telPhoneNo { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int showSort { get; set; }

        /// <summary>
        /// 打印顺序
        /// </summary>
        public int printSort { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public String kayword { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public String address { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Required]
        public String synopsis { get; set; }

        /// <summary>
        /// logoUrl
        /// </summary>
        public String logoUrl { get; set; }

        /// <summary>
        /// 宣传图Url
        /// </summary>
        public String pictureUrl { get; set; }

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
        /// 是否启用名称
        /// </summary>
        [IgnoreParam]
        public String isUseLabel { get; set; }

        /// <summary>
        /// 候诊说明
        /// </summary>
        public String waitingDesc { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [IgnoreParam]
        public String createDate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [IgnoreParam]
        public String updateDate { get; set; }

        /// <summary>
        /// 是否开放 0:开放预约 1：不开放预约
        /// </summary>
        [Required]
        public String isOpen { get; set; }

        /// <summary>
        /// 上级(父级)科室id(提供给科室列表使用)
        /// </summary>
        [IgnoreParam]
        public String superiorId { 
            get 
            { 
                if(parent!=null){
                    return parent.id; 
                }
                return null;
            } 
        }

        /// <summary>
        /// 提前上屏时间
        /// </summary>
        [Required]
        public String upScreenTime{ get; set; }

        /// <summary>
        /// 是否推送停诊通知 0：是、1否
        /// </summary>
        [Required]
        public String isPush { get; set; }

        /// <summary>
        /// 是否可约 0：是、1否
        /// </summary>
        [Required]
        public String isBook { get; set; }

        /// <summary>
        /// his科室编码
        /// </summary>
        public String hisDeptCode { get; set; }
    }
}
