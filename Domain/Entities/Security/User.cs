using Domain.Common;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities.Security
{
    [Table("users")]
    public class User : BaseEntity
    {
        [Required]
        [Column("msisdn")]
        public string Msisdn { get; set; }
        [JsonIgnore]
        [Required]
        [Column("password_hash")]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        [Required]
        [Column("password_salt")]
        public byte[] PasswordSalt { get; set; }
        [Column("refresh_token")]
        public string? RefreshToken { get; set; }
        [Column("refresh_token_expiry_time")]
        public DateTime RefreshTokenExpiryTime { get; set; }
        [Column("status")]
        public ActivityStatus Status { get; set; } = ActivityStatus.Active;
        [Column("first_name")]
        public string? FirstName { get; set; }
        [Column("last_name")]
        public string? LastName { get; set; }
        [Column("patronymic")]
        public string? Patronymic { get; set; }
        [ForeignKey("Role")]
        [Required]
        [Column("role_id")]
        public Guid RoleId { get; set; }
        [JsonIgnore]
        public Role Role { get; set; } = null;
        public List<Speciality>? Specialities { get; set; } = null; //For doctros only
        public List<Clinic>? Clinics { get; set; } = null;
    }
}
