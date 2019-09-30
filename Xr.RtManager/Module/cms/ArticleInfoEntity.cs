using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Common.Validation;

namespace Xr.RtManager.Module.cms
{
    /// <summary>
    /// 文章管理
    /// </summary>
    public class ArticleInfoEntity
    {
        //public String createById { get; set; }
        //public String createByName { get; set; }
        public String createDate { get; set; }
        public String hospitalId { get; set; }
        public String id { get; set; }
        public String isUse { get; set; }
        public String name { get; set; }
        //public String updateById { get; set; }
        //public String updateByName { get; set; }
        public String updateDate { get; set; }
    }
    /// <summary>
    /// 分类
    /// </summary>
    public class FirstOne
    {
    public String id { get; set; }
    public String name { get; set; }
    }
    /// <summary>
    /// 树形下拉菜单
    /// </summary>
    public class TreeList
    {
        public String id { get; set; }
        public String parentId { get; set; }
        public String name { get; set; }
        public String hospitalId { get; set; }
        public String code { get; set; }
    }
}
