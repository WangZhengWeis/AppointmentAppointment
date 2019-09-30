using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager.Module.System
{
    public class ModifyPasswordEntity
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [Required]
        public String loginName { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        [Required]
        public String oldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        public String newPassword { get; set; }

        /// <summary>
        /// 再次输入密码
        /// </summary>
        [Required,IgnoreParam]
        public String againPassword { get; set; }
    }
}
