using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Common
{
    /// <summary>
    /// 用于对枚举类型的值和显示文本等信息进行描述
    /// </summary>
    public class EnumDescriptorAttribute : Attribute
    {
        public EnumDescriptorAttribute(string code, string text)
            : this(code, text, null, null)
        {

        }

        public EnumDescriptorAttribute(string code, string text, string foreColor, string backColor)
        {
            this.Code = code;
            this.Text = text;
            this.ForeColor = foreColor;
            this.BackColor = backColor;
        }

        /// <summary>
        /// 获取枚举项代码
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 获取枚举项显示内容
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
    }
}
