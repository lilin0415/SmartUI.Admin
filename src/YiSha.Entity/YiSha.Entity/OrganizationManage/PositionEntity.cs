using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Entity.OrganizationManage
{
    [Table("sysposition")]
    public class PositionEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity
    {
        public string PositionName { get; set; }
        public int? PositionSort { get; set; }
        public int? PositionStatus { get; set; }
        public string Remark { get; set; }
    }
}
