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

namespace Xr.RtManager.Utils
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
    }
}
