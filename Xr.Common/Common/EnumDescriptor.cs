using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Common
{
    /// <summary>
    /// 枚举描述信息
    /// </summary>
    public class EnumDescriptor
    {
        internal EnumDescriptor(Type enumType)
        {
            this.EnumType = enumType;
            this.Items = new List<EnumItemDescriptor>();
        }

        /// <summary>
        /// 获取枚举类型
        /// </summary>
        public Type EnumType { get; private set; }

        /// <summary>
        /// 获取枚举项目描述信息
        /// </summary>
        public List<EnumItemDescriptor> Items { get; private set; }
    }

    /// <summary>
    /// 枚举项目描述信息
    /// </summary>
    public class EnumItemDescriptor
    {
        public EnumItemDescriptor(string code, string text, string foreColor, string backColor, object value)
        {
            this.Code = code;
            this.Text = text;
            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.Value = value;
        }

        /// <summary>
        /// 获取枚举项目编码
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 获取枚举项目名称
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// 获取表示枚举项目前景色的字符串表示
        /// </summary>
        public string ForeColor { get; private set; }

        /// <summary>
        /// 获取表示枚举项目背景色的字符串表示
        /// </summary>
        public string BackColor { get; private set; }

        /// <summary>
        /// 获取枚举项目值
        /// </summary>
        public object Value { get; private set; }
    }
}
