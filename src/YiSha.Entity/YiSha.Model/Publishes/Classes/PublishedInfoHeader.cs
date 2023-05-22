using Koo.Utilities.Helpers;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Koo.Utilities.Logging;

namespace YiSha.Model.Publishes
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PublishedInfoDataFormat
    {
        Raw=0,
        Json = 1,
        Bytes = 2,
    }



    public class PublishedInfoHeader
    {
        public static readonly int[] VersionList = { 1 };

        #region properties



        [JsonIgnore]
        public int Version
        {
            get; private set;
        } = 1;

        [JsonIgnore]
        public bool Encrypted
        {
            get;set;
        }
        /// <summary>
        /// 是否压缩
        /// </summary>
        [JsonIgnore]
        public bool Compressed
        {
            get;set;
        }


        [JsonIgnore]
        public PublishedInfoDataFormat DataFormat
        {
            get; set;
        }
        #endregion

      

        public PublishedInfoHeader(int version)
        {
            this.Version = version;
        }

        private static bool CheckHeader(string rawStr, out int version)
        {
            version = -1;
            var versionStr = rawStr.Substring(0, 2);
            if (int.TryParse(versionStr, out version))
            {
                if (VersionList.Contains(version))
                {
                    return true;
                }
                return true;
            }
            return false;
        }

        public static PublishedInfoHeader Read(string rawStr,out string dataStr)
        {
            if (CheckHeader(rawStr, out int version))
            {
                var header = new PublishedInfoHeader(version);

                header.Version = int.Parse(rawStr.Substring(0, 2));
                //header.HeaderLength=int.Parse(rawStr.Substring(2, 3));
                header.Encrypted = DataConverter.ToBool(rawStr.Substring(5, 1), false);
                header.Compressed = DataConverter.ToBool(rawStr.Substring(6, 1), false);
                header.DataFormat = DataConverter.ToEnum<PublishedInfoDataFormat>(rawStr.Substring(7, 1), PublishedInfoDataFormat.Raw).Value;

                dataStr = rawStr.Substring(header.GetHeaderLength());
                return header;
            }
            else
            {
                dataStr = rawStr;
                return null;
            }
        }

        public string ToData()
        {
            var sb = new StringBuilder();
            sb.Append(this.Version.ToString().PadLeft(2));
            sb.Append(this.GetHeaderLength().ToString().PadLeft(3));
            sb.Append(this.Encrypted ? 1 : 0);
            sb.Append(this.Compressed ? 1 : 0);
            sb.Append((int)this.DataFormat);

            return sb.ToString().PadRight(this.GetHeaderLength(), ' ');
        }


        public int GetHeaderLength()
        {
            if (this.Version == 1)
            {
                return 120;
            }
            throw new Exception("未识别版本");
        }

    }
}
