using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    public class LogEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// 所在公司
        /// </summary>
        public String companyName { get; set; }

        /// <summary>
        /// 所在部门
        /// </summary>
        public String officeName { get; set; }

        /// <summary>
        /// 操作用户
        /// </summary>
        public String createByName { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public String requestUri { get; set; }

        /// <summary>
        /// 提交方式
        /// </summary>
        public String method { get; set; }

        /// <summary>
        /// 操作者IP
        /// </summary>
        public String remoteAddr { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public String createDate { get; set; }

        /// <summary>
        /// 操作用户代理信息
        /// </summary>
        public String userAgent { get; set; }

        /// <summary>
        /// 操作提交的数据
        /// </summary>
        public String param { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public String exception { get; set; }
    }
}
