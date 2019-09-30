using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    /// <summary>
    /// 医院信息实体类
    /// </summary>
    public class HospitalInfoEntity
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
        /// 编码
        /// </summary>
        [Required]
        public String code { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Required]
        public String telPhoneNo { get; set; }

        /// <summary>
        /// 医院类型
        /// </summary>
        public String hospitalType { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        public String isUse { get; set; }

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
        /// logoUrl
        /// </summary>
        public String logoUrl { get; set; }

        /// <summary>
        /// 宣传图Url
        /// </summary>
        public String pictureUrl { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Required]
        public String address { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Required]
        public String information { get; set; }

        /// <summary>
        /// 临时坐诊数量
        /// </summary>
        public String temporaryNumSite { get; set; }
    }
}
