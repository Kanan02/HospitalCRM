using Domain.Common;
using Domain.Entities.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("specialities")]
    public class Speciality : BaseEntity
    {
        [Required]
        [Column("name")]
        public string? Name { get; set; }

        public List<User>? Users { get; set; } = null;
    }
}
