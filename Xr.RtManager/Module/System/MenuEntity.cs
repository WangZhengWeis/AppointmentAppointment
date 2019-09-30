using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    public class MenuEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// 父级菜单
        /// </summary>
        [ObjectPoint("parent.id")]
        public String parentId { get; set; }

        /// <summary>
        /// 所有父级编号
        /// </summary>
        public String parentIds { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public String name { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public String href { get; set; }

        /// <summary>
        /// 目标（ mainFrame、_blank、_self、_parent、_top）
        /// </summary>
        public String target { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public String icon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int sort { get; set; }

        /// <summary>
        /// 是否在菜单中显示（1：显示；0：不显示）
        /// </summary>
        [Required]
        public String isShow { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        public String permission { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String remarks { get; set; }
    }
}
