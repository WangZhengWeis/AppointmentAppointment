using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Common
{
    public static class EnumUtils
    {
        private static Dictionary<Type, EnumDescriptor> descriptors = new Dictionary<Type, EnumDescriptor>();

        /// <summary>
        /// 获取指定枚举所有项目的描述符
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static EnumDescriptor GetDescriptor(Type enumType)
        {
            if (descriptors.ContainsKey(enumType)) return descriptors[enumType];

            var descriptor = new EnumDescriptor(enumType);
            descriptors.Add(enumType, descriptor);

            foreach (var fieldInfo in enumType.GetFields())
            {
                var attr = ReflectionUtils.GetAttribute<EnumDescriptorAttribute>(fieldInfo, true);
                if (attr != null) descriptor.Items.Add(new EnumItemDescriptor(attr.Code, attr.Text, attr.ForeColor, attr.BackColor, fieldInfo.GetValue(null)));
            }

            return descriptor;
        }

        /// <summary>
        /// 将指定的编码转换成枚举
        /// </summary>
        /// <typeparam name="T">枚举的类型</typeparam>
        /// <param name="code">枚举编码</param>
        /// <returns></returns>
        public static T FromCode<T>(string code) where T : struct
        {
            return (T)FromCode(typeof(T), code);
        }

        /// <summary>
        /// 将指定的编码转换成枚举
        /// </summary>
        /// <param name="enumType">枚举的类型</param>
        /// <param name="code">枚举编码</param>
        /// <returns></returns>
        public static object FromCode(Type enumType, string code)
        {
            CheckIsValidEnumType(enumType);

            var descriptor = EnumUtils.GetDescriptor(enumType);
            foreach (var item in descriptor.Items)
            {
                if (string.Compare(item.Code, code, true) == 0)
                {
                    return item.Value;
                }
            }

            throw new ZeboneException(string.Format("无法将编码 {0} 转换成枚举 {1}", code, enumType.FullName));
        }

        /// <summary>
        /// 获取指定枚举的编码
        /// </summary>
        /// <typeparam name="T">枚举的类型</typeparam>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static string GetCode<T>(T value) where T : struct
        {
            return GetCode(typeof(T), value);
        }

        /// <summary>
        /// 获取指定枚举的编码
        /// </summary>
        /// <param name="enumType">枚举的类型</param>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static string GetCode(Type enumType, object value)
        {
            CheckIsValidEnumType(enumType);

            var descriptor = EnumUtils.GetDescriptor(enumType);
            foreach (var item in descriptor.Items)
            {
                if (value.Equals(item.Value))
                {
                    return item.Code;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取指定枚举的显示名称
        /// </summary>
        /// <typeparam name="T">枚举的类型</typeparam>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static string GetText<T>(T value) where T : struct
        {
            return GetText(typeof(T), value);
        }

        /// <summary>
        /// 获取指定枚举的显示名称
        /// </summary>
        /// <param name="enumType">枚举的类型</param>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static string GetText(Type enumType, object value)
        {
            CheckIsValidEnumType(enumType);

            var descriptor = EnumUtils.GetDescriptor(enumType);
            foreach (var item in descriptor.Items)
            {
                if (value.Equals(item.Value))
                {
                    return item.Text;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 根据枚举编码 取名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetText<T>(string code) where T : struct
        {
            if (string.IsNullOrEmpty(code))
                throw new ZeboneException("编码不能为空！");

            var descriptor = EnumUtils.GetDescriptor(typeof(T));
            foreach (var item in descriptor.Items)
            {
                if (code.Equals(item.Code))
                {
                    return item.Text;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 检查指定的类型是否为有效的枚举类型
        /// </summary>
        /// <param name="type"></param>
        private static void CheckIsValidEnumType(Type type)
        {
            if (!type.IsEnum) throw new ZeboneException(string.Format("类型 {0} 不是一个有效的枚举类型！", type.FullName));
        }
    }
}
