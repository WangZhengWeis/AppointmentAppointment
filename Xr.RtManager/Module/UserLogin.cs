using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication1
{
    public static class UserLogin
    {
        /// <summary>
        /// id
        /// </summary>
        public static String id = null;

        /// <summary>
        /// 账号
        /// </summary>
        public static String userName = null;

        /// <summary>
        /// 密码
        /// </summary>
        public static String password = null;

        /// <summary>
        /// 用户名（姓名）
        /// </summary>
        public static String name = null;

        /// <summary>
        /// 所属机构ID
        /// </summary>
        public static String officeId = null;

        public static List<MenuEntity> menuList = null;
    }
}
