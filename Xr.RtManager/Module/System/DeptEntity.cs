using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    /// <summary>
    /// 科室信息实体类(这个是登录的时候获取所有科室用的)
    /// </summary>
    public class DeptEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public String code { get; set; }

        /// <summary>
        /// 医院id
        /// </summary>
        public String hospitalId { get; set; }

        /// <summary>
        /// 父级科室
        /// </summary>
        public String parentId { get; set; }
    }
}
