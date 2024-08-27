using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("clinic_modules")]
    public class ClinicModule : IHasKey<Guid>
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey("Clinic")]
        [Column("clinic_id")]
        public Guid ClinicId { get; set; }
        [ForeignKey("Module")]
        [Column("module_id")]
        public Guid ModuleId { get; set; }
        [Column("enabled")]
        public bool Enabled { get; set; } = true;

        public Clinic? Clinic { get; set; }
        public Module? Module { get; set; }
    }

}
