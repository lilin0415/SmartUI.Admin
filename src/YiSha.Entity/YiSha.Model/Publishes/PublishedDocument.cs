using Koo.Utilities.Data;
using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace YiSha.Model.Publishes
{
    public class PublishedDocument: NotifyObject
    {
        /// <summary>
        /// 文档类型
        /// </summary>
        public DocumentType DocumentType
        {
            get; set;
        }

        private string _id;
        /// <summary>
        /// 文档id
        /// </summary>
        [JsonIgnore]
        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                {
                    _id = MD5Helper.ComputeHash(RelativePath.Trim().ToLower());
                }
                return _id;
            }
         
        }
        public string ParentId
        {
            get;set;
        }

        private string _Name;
        /// <summary>
        /// 文档名称
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
        private string _FullPath;
        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativePath
        {
            get
            {
                return _FullPath;
            }
            set
            {
                SetProperty(ref _FullPath, value);
            }
        }
        private bool _EnableVars=true;
        /// <summary>
        /// 相对路径
        /// </summary>
        public bool EnableVars
        {
            get
            {
                return _EnableVars;
            }
            set
            {
                SetProperty(ref _EnableVars, value);
            }
        }

        public ReleasedVarCollection Vars { get; set; } = new ReleasedVarCollection();
    }

    public class ReleasedDocumentCollection : List<PublishedDocument>
    {
        public ReleasedDocumentCollection()
        {
        }
        public ReleasedDocumentCollection(List<PublishedDocument> items):base(items) 
        {

        }
        public PublishedDocument GetByPath(string relativePath)
        {
            return this.FirstOrDefault(x => x.RelativePath == relativePath);
        }
        public PublishedDocument GetById(string id)
        {
            return this.FirstOrDefault(x => x.Id == id);
        }
    }

}
