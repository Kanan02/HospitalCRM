using Domain.Common;
using Domain.Entities.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("clinics")]
    public class Clinic : BaseEntity
    {
        [Column("name")]
        [Required]
        public string? Name { get; set; }
        [Column("address")]
        public string? Address { get; set; }
        [Column("contact_info")]
        public string? ContactInfo { get; set; }
        [Column("partner_id")]
        public string? PartnerId { get; set; }
        [Column("transaction_id")]
        public long TransactionId { get; set; }
        public List<User>? Users { get; set; } = null; // Navigation property for Doctors, Call center and other staff

        public List<MessageTemplate>? MessageTemplates { get; set; } = null;

    }
}
