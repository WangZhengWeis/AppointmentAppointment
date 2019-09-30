using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Xr.Common.Common
{
    /// <summary>
    /// 类型处理相关单元
    /// </summary>
    public static class TypeUtils
    {
        private static Dictionary<Type, TypeConverter> typeConverters = new Dictionary<Type, TypeConverter>();

        /// <summary>
        /// 将字符串转换成指定的类型
        /// </summary>
        /// <typeparam name="T">返回值的类型</typeparam>
        /// <param name="value">需要进行转换的字符串</param>
        /// <returns></returns>
        public static T ConvertFromString<T>(string value)
        {
            return (T)ConvertFromString(value, typeof(T));
        }

        /// <summary>
        /// 将字符串转换成指定的类型
        /// </summary>
        /// <param name="value">需要进行转换的字符串</param>
        /// <param name="resultType">需要返回的类型</param>
        /// <returns></returns>
        public static object ConvertFromString(string value, Type resultType)
        {
            var typeConverter = GetTypeConverter(resultType);
            return typeConverter.ConvertFromString(value);
        }

        /// <summary>
        /// 获取指定类型的类型转换器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeConverter GetTypeConverter(Type type)
        {
            if (type.IsEnum) return new EnumConverter(type);
            if (typeConverters.ContainsKey(type)) return typeConverters[type];

            //尝试在System.ComponentModel命名空间中查找
            var converterType = typeof(TypeConverter).Assembly.GetType(string.Format("System.ComponentModel.{0}Converter", type.Name), false, true);
            if (converterType == null)
            {
                //查询是否有TypeConverterAttribute
                var attrs = type.GetCustomAttributes(typeof(TypeConverterAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    converterType = Type.GetType((attrs[0] as TypeConverterAttribute).ConverterTypeName);
                }
            }

            if (converterType != null)
            {
                var converter = Activator.CreateInstance(converterType) as TypeConverter;
                typeConverters.Add(type, converter);

                return converter;
            }

            return null;
        }

        /// <summary>
        /// 获取指定类型的默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(Type type)
        {
            if (type.IsClass) return null;
            if (type == typeof(int)) return 0;
            if (type == typeof(decimal)) return 0m;
            if (type == typeof(double)) return 0d;
            if (type == typeof(bool)) return false;

            var exp = Expression.Lambda<Func<object>>(Expression.Convert(Expression.Default(type), typeof(object)), null);
            return exp.Compile()();
        }
    }
}
