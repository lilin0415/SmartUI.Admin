using Koo.Utilities.Data;
using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Model.Publishes
{

    public class PublishedInfo:NotifyObject
    {
        [JsonIgnore]
        public PublishedInfoHeader Header
        {
            get; set;
        } = new PublishedInfoHeader(1);

        public long ProductId
        {
            get;set;
        }
        public long CateId
        {
            get;set;
        }

        private string _projectGuid;
        /// <summary>
        /// 项目id
        /// </summary>
        public string ProjectGuid
        {
            get
            {
                return _projectGuid;
            }
            set
            {
                SetProperty(ref _projectGuid, value);
            }
        }
        private string _Name;
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetProperty(ref _Name, value);
            }
        }
        private string _Description;
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark
        {
            get
            {
                return _Description;
            }
            set
            {
                SetProperty(ref _Description, value);
            }
        }
        private string _Version;
        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get
            {
                return _Version;
            }
            set
            {
                SetProperty(ref _Version, value);
            }
        }
        private string _ReleaseNote;
        public string ReleaseNote
        {
            get
            {
                return _ReleaseNote;
            }
            set
            {
                SetProperty(ref _ReleaseNote, value);
            }
        }
        private DateTime _ReleaseTime;
        public DateTime ReleaseDate
        {
            get
            {
                return _ReleaseTime;
            }
            set
            {
                SetProperty(ref _ReleaseTime, value);
            }
        }
        private string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                SetProperty(ref _Password, value);
            }
        }

        public ReleasedDocumentCollection Documents
        {
            get; set;
        } = new ReleasedDocumentCollection();


        public void SetParentId()
        {
            foreach (var doc in this.Documents)
            {
                PublishedInfoHelper.SetParntId(doc, this);
            }
        }

        public string ToVarJson()
        {
            var ret = PublishedInfoHelper.ToJson(this);
            return ret;
        }
    }
}
