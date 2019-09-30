using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    /// <summary>
    /// 消息模板
    /// </summary>
    public class MessageInfoEntity
    {
        public String id { get; set; }
        //public String hostile = "博爱医院";
        //[IgnoreParam]
        //public string Hostile
        //{
        //    get { return hostile; }
        //    set { hostile = value; }
        //}
        /// <summary>
        /// 医院id
        /// </summary>
        [Required]
        [ObjectPoint("hospital.id")]
        public String hospitalId { get { return AppContext.Session.hospitalId; } }
        [Required]
        public String type { get; set; }
        [Required]
        public String content { get; set; }
    }
}
