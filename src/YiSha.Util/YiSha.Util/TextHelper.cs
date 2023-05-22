using Koo.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YiSha.Util.Extension;

namespace YiSha.Util
{
    public class TextHelper
    {
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetCustomValue(string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetSubString(string value, int length, bool ellipsis = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            if (value.Length > length)
            {
                value = value.Substring(0, length);
                if (ellipsis)
                {
                    value += "...";
                }
            }
            return value;
        }

        /// <summary>
        /// 字符串转指定类型数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static T[] SplitToArray<T>(string value, char split)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Array.Empty<T>();
            }

            var items = value.Split(new string[] { split.ToString() }, StringSplitOptions.RemoveEmptyEntries);

            var list  = new List<T>();
            foreach (var item in items)
            {
                list.Add(DataConverter.ToType<T>(item));
            }

            return list.ToArray();
        }
    }
}
