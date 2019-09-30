using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xr.Common.Common
{
    /// <summary>
    /// DataRow对象扩展类
    /// </summary>
    public static class DataRowExtensions
    {
        /// <summary>
        /// 获取指定列的值，指定列的值为null时，返回指定的默认值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName">需要读取的列名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetValue(this DataRow row, string columnName, string defaultValue)
        {
            var value = row[columnName];
            return value != null && value != DBNull.Value ? value.ToString() : defaultValue;
        }

        /// <summary>
        /// 获取指定列的值，指定列的值为null时，返回指定的默认值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName">需要读取的列名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetValue(this DataRow row, string columnName, int defaultValue)
        {
            var value = defaultValue;
            int.TryParse(GetValue(row, columnName, string.Empty), out value);
            return value;
        }

        /// <summary>
        /// 获取指定列的值，指定列的值为null时，返回指定的默认值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName">需要读取的列名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static decimal GetValue(this DataRow row, string columnName, decimal defaultValue)
        {
            var value = defaultValue;
            decimal.TryParse(GetValue(row, columnName, string.Empty), out value);
            return value;
        }
    }
}
