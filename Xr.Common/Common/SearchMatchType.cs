using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Common
{
    /// <summary>
    /// 助记码查询时的匹配方式
    /// </summary>
    public enum SearchMatchType
    {
        /// <summary>
        /// 左匹配
        /// </summary>
        StartsWith = 1,

        /// <summary>
        /// 模糊
        /// </summary>
        Contains = 2,

        /// <summary>
        /// 右匹配
        /// </summary>
        EndsWith = 3
    }
}
