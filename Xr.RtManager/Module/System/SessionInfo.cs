using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Xr.RtManager
{
    public class SessionInfo
    {
        /// <summary>
        /// id
        /// </summary>
        private string userId = null;

        /// <summary>
        /// 获取或设置当前用户Id
        /// </summary>
        public string UserId
        {
            get { return userId; }
            set
            {
                if (userId != value)
                {
                    userId = value;
                    if (UserChanged != null) UserChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 获取或设置用户发生改变时触发的时间
        /// </summary>
        public event EventHandler UserChanged;

        /// <summary>
        /// 账号
        /// </summary>
        public String userName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String password { get; set; }

        /// <summary>
        /// 用户名（姓名）
        /// </summary>
        public String name { get; set; }

        /// <summary>
        /// 所属机构ID
        /// </summary>
        public String officeId { get; set; }

        /// <summary>
        /// 用户所拥有的菜单列表
        /// </summary>
        public List<MenuEntity> menuList { get; set; }

        /// <summary>
        /// 用户所拥有的功能列表
        /// </summary>
        public List<FunctionEntity> functionList { get; set; }

        public String userType { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public String loginDate { get; set; }

        /// <summary>
        /// 存储验证码cookie
        /// </summary>
        public CookieContainer cookie { get; set; }

        /// <summary>
        /// 医院id
        /// </summary>
        public String hospitalId { get; set; }

        /// <summary>
        /// 用户所属科室id字符串
        /// </summary>
        public String deptIds { get; set; }

        /// <summary>
        /// 用户所属科室名称字符串
        /// </summary>
        public String deptNames { get; set; }

        /// <summary>
        /// 科室列表
        /// </summary>
        public List<DeptEntity> deptList { get; set; }

        /// <summary>
        /// 角色id列表
        /// </summary>
        public List<String> roleIdList { get; set; }


        public System.Windows.Forms.Control waitControl { get; set; }

        public Xr.Common.Controls.OpaqueCommand cmd { get; set; }

        public bool openStatus { get; set; }
    }
}
