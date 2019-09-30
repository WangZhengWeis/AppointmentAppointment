using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Xr.Common.Common
{
    /// <summary>
    /// Xml扩展类
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// 读取指定属性的内容，当指定的属性不存在时，返回默认值
        /// </summary>
        /// <param name="element">需要读取的节点</param>
        /// <param name="name">需要读取的属性名称</param>
        /// <param name="defaultValue">属性不存在时的默认值</param>
        /// <returns></returns>
        public static string GetAttribute(this XElement element, string name, string defaultValue)
        {
            var attr = element.Attribute(name);
            return attr != null ? attr.Value : defaultValue;
        }

        /// <summary>
        /// 读取指定属性的内容，当指定的属性不存在时，返回默认值
        /// </summary>
        /// <param name="element">需要读取的节点</param>
        /// <param name="name">需要读取的属性名称</param>
        /// <param name="defaultValue">属性不存在时的默认值</param>
        /// <returns></returns>
        public static int GetAttribute(this XElement element, string name, int defaultValue)
        {
            var result = defaultValue;
            if (!int.TryParse(GetAttribute(element, name, string.Empty), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// 读取指定属性的内容，当指定的属性不存在时，返回默认值
        /// </summary>
        /// <param name="element">需要读取的节点</param>
        /// <param name="name">需要读取的属性名称</param>
        /// <param name="defaultValue">属性不存在时的默认值</param>
        /// <returns></returns>
        public static bool GetAttribute(this XElement element, string name, bool defaultValue)
        {
            var result = defaultValue;
            if (!bool.TryParse(GetAttribute(element, name, string.Empty), out result))
            {
                result = defaultValue;
            }

            return result;
        }
    }
}
