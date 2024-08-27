using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Security
{
    [Table("roles")]
    public class Role : BaseEntity
    {
        [Required]
        [Column("name")]
        public string Name { get; set; }
    }
}
