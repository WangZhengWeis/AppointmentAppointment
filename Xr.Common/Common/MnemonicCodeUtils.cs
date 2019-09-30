//暂不处理


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml.Linq;

//using Zebone.Properties;

//namespace ZeboneContrl.Common
//{
//    /// <summary>
//    /// 助记码处理单元
//    /// </summary>
//    public static class MnemonicCodeUtils
//    {
//        /// <summary>
//        /// 获取指定字符串的首拼码
//        /// </summary>
//        /// <param name="s"></param>
//        /// <returns></returns>
//        public static string GetSpellCode(string s)
//        {
//            return GetCode(s, true);
//        }

//        /// <summary>
//        /// 取 指定长度的拼音首字母
//        /// </summary>
//        /// <param name="s">指定字符</param>
//        /// <param name="length">首字母长度</param>
//        /// <returns></returns>
//        public static string GetSpellCode(string s, int length)
//        {
//            var code = GetCode(s, true);
//            return code.Length > length ? code.Substring(0, length) : code;
//        }

//        /// <summary>
//        /// 获取指定字符串的五笔码
//        /// </summary>
//        /// <param name="s"></param>
//        /// <returns></returns>
//        public static string GetWuBiCode(string s)
//        {
//            return GetCode(s, false);
//        }

//        /// <summary>
//        ///  取 指定长度的五笔首字母
//        /// </summary>
//        /// <param name="s">指定字符</param>
//        /// <param name="length">首字母长度</param>
//        /// <returns></returns>
//        public static string GetWuBiCode(string s, int length)
//        {
//            var code = GetCode(s, false);
//            return code.Length > length ? code.Substring(0, length) : code;
//        }

//        private static string GetCode(string s, bool spellCode)
//        {
//            var result = string.Empty;
//            var document = XDocument.Parse(Resources.MnemonicCode);

//            var element = document.Root.Element(spellCode ? "SpellCode" : "WBCode");
//            foreach (var c in s)
//            {
//                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
//                {
//                    result += c;
//                }
//                else
//                {
//                    foreach (var e in element.Elements())
//                    {
//                        if (e.Value.IndexOf(c) != -1)
//                        {
//                            result += e.Name.ToString();
//                        }
//                    }
//                }
//            }

//            return result.ToUpperInvariant();
//        }
//    }
//}
