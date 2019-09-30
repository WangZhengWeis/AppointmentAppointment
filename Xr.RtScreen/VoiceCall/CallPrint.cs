using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Xr.RtScreen.VoiceCall
{
    public class CallPrint
    {
        private string showNumber { get; set; }
        public CallPrint(string _showNumber)
        {
            showNumber = _showNumber;
        }
        public string CallVoiceString()
        {
            return String.Format("{0}", showNumber);
        }

        public string LogString()
        {
            return String.Format("{0}", showNumber);
        }
        /// <summary>
        /// 阿拉伯数字转中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ConvertToChinese(string str)
        {
            var numResult = System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", string.Empty);
            if (numResult != String.Empty)
            {
                var num = Double.Parse(numResult);
                var s = num.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
                var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
                var refStr = Regex.Replace(d, ".", m => "负 空零壹二三四五六七八九空空空空空空空分角十百千万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
                refStr = refStr.Trim();
                if (refStr.Length > 1 && refStr.Trim().Length < 4)
                {
                    refStr = refStr.TrimStart('一');
                }
                return refStr + GetChineseWord(str);
            }
            return str;
        }
        /// <summary>
        /// 匹配中文姓名
        /// </summary>
        /// <param name="oriText"></param>
        /// <returns></returns>
        public static string GetChineseWord(string oriText)
        {
            var x = @"[\u4E00-\u9FFF]+";
            var Matches = Regex.Matches
            (oriText, x, RegexOptions.IgnoreCase);
            var sb = new StringBuilder();
            foreach (Match NextMatch in Matches)
            {
                sb.Append(NextMatch.Value);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 移除特殊符号
        /// </summary>
        /// <param name="String"></param>
        /// <returns></returns>
        public static string SqlSafeString(string String)
        {
            String = String.Replace("(", string.Empty);
            String = String.Replace("急", string.Empty);
            String = String.Replace("骨", string.Empty);
            String = String.Replace(")", string.Empty);
            return String;
        }
    }
}
