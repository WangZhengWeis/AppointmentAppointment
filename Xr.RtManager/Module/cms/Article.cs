using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager.Module.cms
{
    /// <summary>
    /// 编辑文章保存类
    /// </summary>
    public class Article
    {

        //[Required]
        public String id { get; set; }//ID 
        //[Required]
        public String hospitalId
        {
            get
            {
                return AppContext.Session.hospitalId;
            }
        }//医院主键
         //[Required]
        public String deptId { get; set; }//科室主键
        //[Required]
        public String doctorId { get; set; }//医生主键
        //[Required]
        public String type { get; set; }//类型
        //[Required]
        public String categoryId { get; set; }//类型主键
        //[Required]
        public String title { get; set; }//标题
        //[Required]
        public String content { get; set; }//内容
        //[Required]
        public String isUse { get; set; }//是否启用
        //[IgnoreParam]
        //public String createById { get; set; }
        //[IgnoreParam]
        //public String createByName { get; set; }
        [IgnoreParam]
        public String createDate { get; set; }
        [IgnoreParam]
        public String readNum { get; set; }
        //[IgnoreParam]
        //public String updateById { get; set; }
        //[IgnoreParam]
        //public String updateByName { get; set; }
        [IgnoreParam]
        public String updateDate { get; set; }
    }
}
