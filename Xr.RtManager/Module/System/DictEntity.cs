using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    public class DictEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// 数据值
        /// </summary>
        [Required]
        public String value { get; set; }

        /// <summary>
        /// 标签名
        /// </summary>
        [Required]
        public String label { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        public String type { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        public String description { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int sort { get; set; }

        [IgnoreParam]
        public String operation { get { return "添加键值"; } }
    }
}
