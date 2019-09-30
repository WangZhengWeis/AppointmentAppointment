using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Xr.RtManager
{
    public class Dict
    {
        public String code { get; set; }
        public String name { get; set; }
        public String pyCode { get; set; }
        public String dCode { get; set; }
    }
    public static class DictUtils
    {
        /// <summary>
        /// 根据code遍历指定字典，返回name，没有对应的返回传进来的code
        /// </summary>
        /// <param name="list"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static String getName(List<Dict> list, String code)
        {
            String name = code;
            foreach (Dict dict in list)
            {
                if (dict.code.Equals(code))
                {
                    name = dict.name;
                    break;
                }
            }
            return name;
        }

        /// <summary>
        /// 是否字典
        /// </summary>
        /// <returns></returns>
        public static List<Dict> sf()
        {
            List<Dict> list = new List<Dict>();
            Dict dict = new Dict();
            dict.code = "0";
            dict.name = "否";
            dict.pyCode = "f";
            list.Add(dict);

            dict = new Dict();
            dict.code = "1";
            dict.name = "是";
            dict.pyCode = "s";
            list.Add(dict);

            return list;
        }

        /// <summary>
        /// 机构类型字典
        /// </summary>
        /// <returns></returns>
        public static List<Dict> jglx()
        {
            List<Dict> list = new List<Dict>();
            Dict dict = new Dict();
            dict.code = "1";
            dict.name = "公司";
            dict.pyCode = "gs";
            list.Add(dict);

            dict = new Dict();
            dict.code = "2";
            dict.name = "部门";
            dict.pyCode = "bm";
            list.Add(dict);

            dict = new Dict();
            dict.code = "3";
            dict.name = "小组";
            dict.pyCode = "xz";
            list.Add(dict);

            return list;
        }

        /// <summary>
        /// 是否锁定  0：开放 1：锁定
        /// </summary>
        /// <returns></returns>
        public static List<Dict> sfsd()
        {
            List<Dict> list = new List<Dict>();
            Dict dict = new Dict();
            dict.code = "0";
            dict.name = "开放";
            list.Add(dict);

            dict = new Dict();
            dict.code = "1";
            dict.name = "锁定";
            list.Add(dict);

            return list;
        }

        /// <summary>
        /// 是否启用  0：启用 1：停用
        /// </summary>
        /// <returns></returns>
        public static List<Dict> sfqy()
        {
            List<Dict> list = new List<Dict>();
            Dict dict = new Dict();
            dict.code = "0";
            dict.name = "启用";
            list.Add(dict);

            dict = new Dict();
            dict.code = "1";
            dict.name = "停用";
            list.Add(dict);

            return list;
        }
    }
}
