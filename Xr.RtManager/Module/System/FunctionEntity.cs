using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    public class FunctionEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// 用于树状列表
        /// </summary>
        [IgnoreParam]
        public String parentId { get; set; }

        /// <summary>
        /// 功能名
        /// </summary>
        [Required]
        public String name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public String description { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public String isUse { get; set; }
    }
}
