using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.RtManager
{
    /// <summary>
    /// 修改转字符串时的名字
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class ObjectPoint : Attribute
    {
        public string ColumnName { get; set; }
        public ObjectPoint(string columnName)
        {
            this.ColumnName = columnName;
        }
    }
}
