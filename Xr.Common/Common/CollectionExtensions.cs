using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Xr.Common.Common
{
    /// <summary>
    /// 集合操作扩展类
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 将对应的集合连接成一个字符串
        /// </summary>
        /// <param name="items"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> items, string separator)
        {
            bool first = true;
            string result = string.Empty;

            IEnumerator<T> enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!first) result += separator;
                result += enumerator.Current.ToString();
                first = false;
            }

            return result;
        }

        /// <summary>
        /// 向字典中添加一个键值对，如果键在字典中已经存在，则覆盖对应的值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrSetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        /// <summary>
        /// 将字典dict2中的内容的合并到当前字典中，当存在相同键时，当前字典中的值会被覆盖
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict1"></param>
        /// <param name="dict2"></param>
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> dict1, IDictionary<TKey, TValue> dict2)
        {
            foreach (var item in dict2)
            {
                dict1.AddOrSetValue(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 对于枚举项目执行指定操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        /// <summary>
        /// 向 BindingList 中批量添加记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bindingList"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this BindingList<T> bindingList, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                bindingList.Add(item);
            }
        }
    }
}
