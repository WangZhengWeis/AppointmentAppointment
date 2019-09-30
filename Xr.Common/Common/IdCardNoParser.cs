using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Xr.Common.Common
{
    /// <summary>
    /// 身份证号码解析
    /// </summary>
    public class IdCardNoParser
    {
        private string idCardNo;

        public IdCardNoParser(string idCardNo)
        {
            this.idCardNo = idCardNo ?? string.Empty;
            Parse();
        }

        /// <summary>
        /// 获取身份证号码是否有效
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// 获取出生日期
        /// </summary>
        public DateTime Birthday { get; private set; }

        /// <summary>
        /// 获取性别是否为男性
        /// </summary>
        public bool IsMale { get; private set; }

        /// <summary>
        /// 对身份证号码进行解析
        /// </summary>
        private void Parse()
        {
            try
            {
                if (idCardNo.Length == 15 && Regex.IsMatch(idCardNo, @"\d{15}"))
                {
                    this.IsValid = true;
                    Parse15();
                }
                else if (idCardNo.Length == 18 && Regex.IsMatch(idCardNo, @"\d{18}|\d{17}[xX]") && CheckSumIsValid())
                {
                    this.IsValid = true;
                    Parse18();
                }
                else
                {
                    this.IsValid = false;
                }
            }
            catch
            {
                this.IsValid = false;
            }
        }

        /// <summary>
        /// 解析15位身份证号码
        /// </summary>
        private void Parse15()
        {
            //从第7位开始，截取6个数字，作为出生日期
            this.Birthday = DateTime.ParseExact("19" + idCardNo.Substring(6, 6), "yyyyMMdd", null);

            //截取最后一位，奇数为男性
            this.IsMale = int.Parse(idCardNo.Substring(idCardNo.Length - 1, 1)) % 2 == 1;
        }

        /// <summary>
        /// 解析18位身份证号码
        /// </summary>
        private void Parse18()
        {
            //从第7为开始，截取8个数字，作为出生日期
            this.Birthday = DateTime.ParseExact(idCardNo.Substring(6, 8), "yyyyMMdd", null);

            //截取倒数第二位，奇数为男性
            this.IsMale = int.Parse(idCardNo.Substring(idCardNo.Length - 2, 1)) % 2 == 1;
        }

        /// <summary>
        /// 计算校验码是否正确
        /// </summary>
        private bool CheckSumIsValid()
        {
            /*
             * 1、将前面的身份证号码17位数分别乘以不同的系数。从第一位到第十七位的系数分别为：7 9 10 5 8 4 2 1 6 3 7 9 10 5 8 4 2 ；
             * 2、将这17位数字和系数相乘的结果相加；
             * 3、用加出来和除以11，看余数是多少；
             * 4、余数只可能有0 1 2 3 4 5 6 7 8 9 10这11个数字。其分别对应的最后一位身份证的号码为1 0 X 9 8 7 6 5 4 3 2；
             */

            var coefficients = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            var checkSumResults = new string[] { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };

            var result = 0;
            for (var i = 0; i < 17; i++)
            {
                result = result + int.Parse(idCardNo[i].ToString()) * coefficients[i];
            }

            return string.Compare(checkSumResults[result % 11], idCardNo[17].ToString(), true) == 0;
        }
    }
}
