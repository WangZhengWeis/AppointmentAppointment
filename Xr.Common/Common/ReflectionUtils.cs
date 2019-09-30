using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Xr.Common.Common
{
    /// <summary>
    /// 反射处理单元
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        /// 将源对象中的属性值浅拷贝到目标对象中，拷贝时，源对象和目标对象之间的属性名称相同、属性类型可以进行转换时才进行拷贝
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        public static void CopyProperties(object source, object target)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (target == null) throw new ArgumentNullException("target");

            var sourceProperties = source.GetType().GetProperties();
            foreach (var sourceProperty in sourceProperties)
            {
                if (sourceProperty.CanRead)
                {
                    var targetProperty = target.GetType().GetProperty(sourceProperty.Name);
                    if (targetProperty != null &&
                        targetProperty.CanWrite &&
                        targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    {
                        targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
                    }
                }
            }
        }

        /// <summary>
        /// 根据 DataRow 对象设置指定对象的属性
        /// </summary>
        /// <param name="obj">需要进行属性设置的对象</param>
        /// <param name="row">属性值来源DataRow</param>
        public static void SetProperties(object obj, DataRow row)
        {
            foreach (var property in obj.GetType().GetProperties().Where(p => p.CanWrite))
            {
                var col = row.Table.Columns[property.Name];
                if (col == null) continue;

                var colValue = row[col];
                if (colValue == null || colValue == DBNull.Value) continue;

                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(obj, colValue.ToString(), null);
                }
                else
                {
                    property.SetValue(obj, TypeUtils.ConvertFromString(colValue.ToString(), property.PropertyType), null);
                }
            }
        }

        /// <summary>
        /// 获取指定的属性并返回
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(MemberInfo member, bool inherit) where TAttribute : Attribute
        {
            var attrs = member.GetCustomAttributes(typeof(TAttribute), inherit);
            if (attrs != null && attrs.Length > 0) return (TAttribute)attrs[0];

            return null;
        }

        /// <summary>
        /// 获取指定的属性并返回
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public static TAttribute[] GetAttributes<TAttribute>(MemberInfo member, bool inherit) where TAttribute : Attribute
        {
            var attrs = member.GetCustomAttributes(typeof(TAttribute), inherit);
            if (attrs != null && attrs.Length > 0) return (TAttribute[])attrs;

            return new TAttribute[0];
        }
    }
}
