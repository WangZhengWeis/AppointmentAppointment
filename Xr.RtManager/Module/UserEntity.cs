using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager
{
    public class UserEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public String imgPath { get; set; }

        /// <summary>
        /// 归属公司id
        /// </summary>
        [ObjectPoint("company.id")]
        public String companyId { get; set; }

        /// <summary>
        /// 归属公司名称
        /// </summary>
        [IgnoreParam]
        public String companyName { get; set; }

        /// <summary>
        /// 归属部门id
        /// </summary>
        [ObjectPoint("office.id")]
        public String officeId { get; set; }

        /// <summary>
        /// 归属部门名称
        /// </summary>
        [IgnoreParam]
        public String officeName { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [Required]
        public String loginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public String password { get; set; }


        /// <summary>
        /// 再次确认密码
        /// </summary>
        public String newPassword { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public String name { get; set; }

        /// 邮箱
        /// </summary>
        [Required,Mailbox]
        public String email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public String phone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public String mobile { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String remarks { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public String roleIds { get; set; }

        /// <summary>
        /// 角色名字
        /// </summary>
        public String roleNames { get; set; }

        /// <summary>
        /// 是否是管理员 1是0否
        /// </summary>
        [Required]
        public String userType { get; set; }

        /// <summary>
        /// 是否更换密码周期限制 1是0否
        /// </summary>
        [Required]
        public String isResetPwd { get; set; }

        /// <summary>
        /// // 是否锁定，0开放、1锁定
        /// </summary>
        [IgnoreParam]
        public String isLocked { get; set; }

        /// <summary>
        /// 是否锁定名称
        /// </summary>
        [IgnoreParam]
        public String lockedName { get { return DictUtils.getName(DictUtils.sfsd(), this.isLocked); } }
    }
}
