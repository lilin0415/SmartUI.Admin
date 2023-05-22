using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Model.WebApis
{
    public class CheckNewVersionRequest
    {
        public int AppType
        {
            get;set;
        }
        public string CurrentVersion
        {
            get;set;
        }
    }

    public class CheckNewVersionResponse
    {
        public bool HasNewVersion
        {
            get;set;
        }

        public string NewVersion
        {
            get;set;
        }

        public bool IsOptional
        {
            get; set;
        } = true;

        public string DownloadUrl
        {
            get;set;
        }
    }
}
