using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Web.Code;

namespace YiSha.Entity.SystemManage
{
    [Table("sysmenu")]
    public class MenuEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? ParentId { get; set; }

        public string MenuName { get; set; }

        public string MenuIcon { get; set; }

        public string MenuUrl { get; set; }

        public string MenuTarget { get; set; }

        public int? MenuSort { get; set; }

        public int? MenuType { get; set; }

        public int? MenuStatus { get; set; }
        public string Authorize { get; set; }

        public string Remark { get; set; }


        public byte? IsSystem
        {
            get; set;
        }
        public byte? IsPublic
        {
            get; set;
        }
        [NotMapped]
        public string ParentName { get; set; }

     
        
    }
}
