using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Util.Model
{
    public class OfficialModel
    {
        static OfficialModel()
        {
            var i = new OfficialModel();
            _instance = i;
        }

        private OfficialModel()
        {
        }
        private static OfficialModel _instance = null;
        public static OfficialModel Instance
        {
            get
            {
                return _instance;
            }
        }
        public string CompayName { get; } = "新石器RPA自动化系统";
        public string CompayUrl { get; } = "https://quickso.cn/";

        public string ProductName { get; } = "新石器RPA";
        public string ProductFullName { get; } = "新石器RPA-流程自动化系统";
        public string ProductDescription { get; } = "新石器RPA";
        public string CurrentVersion { get; } = "1.0.0";

        public string CheckNewVersionUrl
        {
            get;
        } = "";
    }
}
