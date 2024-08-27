using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common
{
    public class AuditableEntity
    {
        [Column("created_by")]
        public string? CreatedBy { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("last_modified_by")]
        public string? LastModifiedBy { get; set; }
        [Column("last_modified_at")]
        public DateTime? LastModifiedAt { get; set; }
    }
}
