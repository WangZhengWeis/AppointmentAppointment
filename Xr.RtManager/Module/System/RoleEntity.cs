using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    public class RoleEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// 归属机构
        /// </summary>
        [Required]
        [ObjectPoint("office.id")]
        public String officeId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Required]
        public String name { get; set; }

        /// <summary>
        /// 数据范围
        /// </summary>
        [Required]
        public String dataScope { get; set; }

        /// <summary>
        /// 数据范围名称
        /// </summary>
        [Required]
        public String dataScopeName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public String officeName { get; set; }

        /// <summary>
        /// 菜单权限id字符串
        /// </summary>
        public String menuIds { get; set; }

        /// <summary>
        /// 功能权限id字符串
        /// </summary>
        public String functionIds { get; set; }
    }
}
