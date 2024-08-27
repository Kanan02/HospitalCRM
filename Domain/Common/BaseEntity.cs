using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common
{
    public abstract class BaseEntity : AuditableEntity, IHasKey<Guid>
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

    }
}
