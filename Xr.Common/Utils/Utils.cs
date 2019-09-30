using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xr.Common.Utils
{
    public  class UtilsRef<T> 
    {
        public string Msg  { get; set; }
        public List<T> Data { get; set; }
    }
    public static class Utils
    {
        #region 将json转换为实体
        public static UtilsRef<T> getObjectByJson<T>(string jsonString)
        {
            JObject objT = JObject.Parse(jsonString);
            if (string.Compare(objT["common_return"].ToString(), "true", true) == 0)
            {
                //JArray Lists = JArray.Parse(objT["return_info"].ToString());
                jsonString = JArray.Parse(objT["return_info"]["list"].ToString()).ToString();


                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<T>));
                //把Json传入内存流中保存
                //jsonString = "[" + jsonString + "]";
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                // 使用ReadObject方法反序列化成对象
                object ob = serializer.ReadObject(stream);
                List<T> ls = (List<T>)ob;

                return new UtilsRef<T>() { Data = ls ,Msg="成功"};
            }
            else
            {
                return new UtilsRef<T>() { Data = null, Msg = objT["return_info"]["result"].ToString() }; 
            }
        }
# endregion

        #region 将json转换为实体
        public static UtilsRef<T> getObjectByJson<T>(JObject objT)
        {
            string jsonString = null;
            if (string.Compare(objT["common_return"].ToString(), "true", true) == 0)
            {
                //JArray Lists = JArray.Parse(objT["return_info"].ToString());
                jsonString = JArray.Parse(objT["return_info"]["list"].ToString()).ToString();


                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<T>));
                //把Json传入内存流中保存
                //jsonString = "[" + jsonString + "]";
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                // 使用ReadObject方法反序列化成对象
                object ob = serializer.ReadObject(stream);
                List<T> ls = (List<T>)ob;

                return new UtilsRef<T>() { Data = ls, Msg = "成功" };
            }
            else
            {
                return new UtilsRef<T>() { Data = null, Msg = objT["return_info"]["result"].ToString() };
            }
        }
        # endregion

        #region 将json转换为实体
        public static UtilsRef<T> getObjectByJson<T>(string jsonString, string listName)
        {
            JObject objT = JObject.Parse(jsonString);
            if (string.Compare(objT["common_return"].ToString(), "true", true) == 0)
            {
                //JArray Lists = JArray.Parse(objT["return_info"].ToString());
                jsonString = JArray.Parse(objT["return_info"][listName].ToString()).ToString();


                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<T>));
                //把Json传入内存流中保存
                //jsonString = "[" + jsonString + "]";
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                // 使用ReadObject方法反序列化成对象
                object ob = serializer.ReadObject(stream);
                List<T> ls = (List<T>)ob;

                return new UtilsRef<T>() { Data = ls, Msg = "成功" };
            }
            else
            {
                return new UtilsRef<T>() { Data = null, Msg = objT["return_info"]["result"].ToString() };
            }
        }
        # endregion


        #region 将jsonList转换为实体
        public static List<T> getObjectByJsonList<T>(string objT)
        {
                //JArray Lists = JArray.Parse(objT["return_info"].ToString());
            //string jsonString = JArray.Parse(objT).ToString();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<T>));
                //把Json传入内存流中保存
                //jsonString = "[" + jsonString + "]";
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(objT));
                // 使用ReadObject方法反序列化成对象
                object ob = serializer.ReadObject(stream);
                List<T> ls = (List<T>)ob;
                return ls;
        }
        # endregion

        #region 将json转换为DataTable
        /// <summary>
        /// 将json转换为DataTable
        /// </summary>
        /// <param name="strJson">得到的json</param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string strJson)
        {
            //转换json格式
            strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").Replace(",    ", "*").Replace("    \\","\\").ToString();
            //取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名   
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));
            //获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value.Replace("    ", "");
                string[] strRows = strRow.Split('*');
                //创建表   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split('#');
                        if (strCell[0].Substring(0, 1) == "\"")
                        {
                            int a = strCell[0].Length;
                            dc.ColumnName = strCell[0].Substring(1, a - 2);
                        }
                        else
                        {
                            dc.ColumnName = strCell[0];
                        }
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }
                //增加内容   
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
        }
        # endregion

        /// <summary>
        /// 将中文转为首字母，不是文字部分返回空字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string getSpells(string input)
        {
            int len = input.Length;
            string reVal = "";
            for(int i=0;i<len;i++)
            {
            reVal += getSpell(input.Substring(i,1));
            }
            return reVal;
        }

 
        /// <summary>
        /// 将文字转成首字母，不是文字返回空字符串
        /// </summary>
        /// <param name="cn"></param>
        /// <returns></returns>
        public static string getSpell(string cn)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cn);
            if(arrCN.Length > 1)
            {
            int area = (short)arrCN[0];
            int pos = (short)arrCN[1];
            int code = (area<<8) + pos;
            int[] areacode = {45217,45253,45761,46318,46826,47010,47297,47614,48119,48119,49062,49324,49896,50371,50614,50622,50906,51387,51446,52218,52698,52698,52698,52980,53689,54481};
            for(int i=0;i<26;i++)
            {
                int max = 55290;
                if(i != 25) max = areacode[i+1];
                if(areacode[i]<=code && code<max)
                {
                return Encoding.Default.GetString(new byte[]{(byte)(65+i)});
                }
            }
            return "?";
            }
            else return "";
        }
    
        
        /// <summary>
        /// (截头)截取字符串，从指定位置startIdx开始，出现"结束字符"位置之间的字符串,是否忽略大小写，是否包含开始字符
        /// "房陈华是个高手".StartSubString(0,"陈",true,true)返回 房陈
        /// </summary>
        /// <param name="str">要截取字符串</param>
        /// <param name="startIdx">开始查找的序号</param>
        /// <param name="endStr"></param>
        /// <param name="isContainsint">是否包含开始字符</param>
        /// <param name="isIgnoreCase"></param>
        /// <returns>截取字符串</returns>
        public static string StartSubString(this string str, int startIdx, string endStr, bool isContains = false, bool isIgnoreCase = true)
        {
            if (string.IsNullOrEmpty(str) || startIdx > str.Length - 1 || startIdx < 0)
                return string.Empty;
            int idx = str.IndexOf(endStr, startIdx, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            if (idx < 0) //没找到
                return string.Empty;
            return str.Substring(0, isContains ? idx + endStr.Length : idx);
        }


        /// <summary>
        ///  (截中)截取字符串，根据开始字符,结束字符,是否包含开始字符,结束字符(默认为不包括),大小写是否敏感（从0位置开始，）
        ///  "Fang陈华是个高手".SubBetweenString("陈","手",true,true)返回 陈华是个高手
        /// </summary>
        /// <param name="str">要 截取字符串</param>
        /// <param name="startStr">开始字符</param>
        /// <param name="endstr">结束字符</param>
        /// <param name="isContainsStartStr">是否包含开始字符</param>
        /// <param name="isContainsEndStr">是否包含结束字符</param>
        /// <param name="isIgnoreCase">是否忽略大小写比较</param>
        /// <returns>截取字符串</returns>
        public static string SubBetweenString(this string str, string startStr, string endstr, bool isContainsStartStr = false, bool isContainsEndStr = false, bool isIgnoreCase = true)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            int staridx = str.IndexOf(startStr, 0, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            if (staridx < 0) //没找到
                return string.Empty;
            int endidx = str.IndexOf(endstr, staridx + startStr.Length, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            if (endidx < 0) //没找到
                return string.Empty;
            var start = isContainsStartStr ? staridx : staridx + startStr.Length;
            var end = isContainsEndStr ? endidx + endstr.Length : endidx;
            if (end <= start)
                return string.Empty;
            return str.Substring(start, end - start);
        }

        /// <summary>
        /// 深克隆方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RealObject"></param>
        /// <returns></returns>
        public static T Clone<T>(T RealObject)
        {
            using (Stream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, RealObject);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}
