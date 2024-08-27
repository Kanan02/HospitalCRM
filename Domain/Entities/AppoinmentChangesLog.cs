using Domain.Common;
using Domain.Entities.Security;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("appointment_changes_log")]
    public class AppointmentChangesLog : BaseEntity
    {
        [ForeignKey("Appointment")]
        [Column("appointment_id")]
        public Guid AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        [ForeignKey("User")]
        [Column("changed_by_user_id")]
        public Guid ChangedByUserId { get; set; }
        public User? User { get; set; }

        [Column("previous_appointment_time")]
        public DateTime? PreviousAppointmentTime { get; set; }

        [Column("new_appointment_time")]
        public DateTime? NewAppointmentTime { get; set; }

        [Column("change_date")]
        public DateTime ChangeDate { get; set; }
    }

}
