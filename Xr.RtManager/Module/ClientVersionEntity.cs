using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    /// <summary>
    /// 客户端版本实体类
    /// </summary>
    public class ClientVersionEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// 版本标题
        /// </summary>
        [Required]
        public String title { get; set; }

        /// <summary>
        /// 更新文件路径
        /// </summary>
        [Required]
        public String updateFilePath { get; set; }

        /// <summary>
        /// 更新说明
        /// </summary>
        public String updateDesc { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [Required]
        public String version { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        public String isUse { get; set; }

        /// <summary>
        /// 客户端类型，0：管理端、1：候诊屏、2：呼号端
        /// </summary>
        [Required]
        public String type { get; set; }
    }
}
