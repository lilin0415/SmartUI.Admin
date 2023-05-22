using Koo.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YiSha.Util
{
    public class SqlStringHelper
    {
        public static string QuoteString(params string[] strs)
        {
            var items = strs.Select(x => "'" + x + "'");
            return string.Join(",", items);
        }
        public static bool IsSafeSqWhere(long? value)
        {
          return value.HasValue && value.Value > 0;
        }
        public static bool IsSafeSqWhere(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return !Regex.IsMatch(value, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        public static bool IsSafeSqlParam(string value, bool ignoreEmpty = true)
        {
            if (ignoreEmpty)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }
            }

            return !Regex.IsMatch(value, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        public static string ToSafeSqlParam(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (!Regex.IsMatch(value, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']"))
            {
                return value;
            }
            return string.Empty;
        }
        public static string ToSafeSqlIds(string ids)
        {
            long[] idArr = StringHelper.SplitToArray<long>(ids, ',');
            return string.Join(",", idArr);
        }
        public static List<long> ToSafeSqlIdArray(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return new List<long>();
            }
            long[] idArr = StringHelper.SplitToArray<long>(ids, ',');
            return idArr.ToList();
        }
    }
}
