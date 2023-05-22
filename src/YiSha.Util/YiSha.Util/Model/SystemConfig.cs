using Newtonsoft.Json;

namespace YiSha.Util.Model
{
    public class SystemConfig
    {
        public SystemConfig()
        {
            DBSlowSqlLogTime = 5;
        }
      

        public const string CompayName = "毫木软件";
        public const string CompayUrl = "#";

        public const string ProductName = "新石器自动化测试系统";
        public const string ProductFullName = "新石器自动化测试系统";
        public const string ProductDescription = "新石器自动化测试系统";

        public const string CurrentVersion = "1.0.0";

        public const string BackendRouter = "Member";

     

        [JsonIgnore]
        public string CaseName
        {
            get
            {
                return "用例";
            }
        }

        /// <summary>
        /// 是否是Demo模式
        /// </summary>
        public bool Demo { get; set; }

        /// <summary>
        /// 是否是调试模式
        /// </summary>
        public bool Debug { get; set; }

        [JsonIgnore]
        public  bool IsReleased
        {
            get
            {
                return Debug ? false : true;
            }
        }

        /// <summary>
        /// 允许一个用户在多个电脑同时登录
        /// </summary>
        public bool LoginMultiple { get; set; }

        public string LoginProvider { get; set; }

        /// <summary>
        /// Snow Flake Worker Id
        /// </summary>
        public int SnowFlakeWorkerId { get; set; }

        /// <summary>
        /// api地址
        /// </summary>
        public string ApiSite { get; set; }

        /// <summary>
        /// 允许跨域的站点
        /// </summary>
        public string AllowCorsSite { get; set; }

        /// <summary>
        /// 网站虚拟目录
        /// </summary>
        public string VirtualDirectory { get; set; }

        public string DBProvider { get; set; }

        public string DBConnectionString { get; set; }

        /// <summary>
        ///  数据库超时间（秒）
        /// </summary>
        public int DBCommandTimeout { get; set; }

        /// <summary>
        /// 慢查询记录Sql(秒),保存到文件以便分析
        /// </summary>
        public int DBSlowSqlLogTime { get; set; }

        public string CacheProvider { get; set; }

        public string RedisConnectionString { get; set; }

        public string GetDatabaseName()
        {
            var name = HtmlHelper.Resove(GlobalContext.SystemConfig.DBConnectionString.ToLower(), "database=", ";");
            return name;
        }
    }
}