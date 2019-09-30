using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    public class OfficeEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        [Required]
        [ObjectPoint("parent.id")]
        public String parentId { get; set; }

        /// <summary>
        /// 归属区域ID
        /// </summary>
        [ObjectPoint("area.id")]
        [Required]
        public String areaId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        public String code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public String name { get; set; }

        /// <summary>
        /// 单位(机构)类型（1：公司；2：部门；3：小组）
        /// </summary>
        [Required]
        public String type { get; set; }

        /// <summary>
        /// 机构等级（1：一级；2：二级；3：三级；4：四级）
        /// </summary>
        [Required]
        public String grade { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public String address { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public String zipCode { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public String master { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public String phone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public String fax { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public String email { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public String remarks { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        public String areaName { get; set; }

        [IgnoreParam]
        public List<UserEntity> userList { get; set; }

        [IgnoreParam]
        public String typeName {
            get{return DictUtils.getName(DictUtils.jglx(), this.code);}
        }
    }
}
