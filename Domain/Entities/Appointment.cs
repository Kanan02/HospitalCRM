using Domain.Common;
using Domain.Entities.Security;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("appointments")]
    public class Appointment : BaseEntity
    {
        public Appointment()
        {
            if (AppointmentStartTime < DateTime.Now.AddHours(1))
            {
                Status = AppointmentStatus.LastHour;
            }
            else if (AppointmentStartTime <= DateTime.Now)
            {
                Status = AppointmentStatus.Completed;
            }
            else
            {
                Status = AppointmentStatus.Pending;
            }
        }
        [Column("patient_name")]
        public string? PatientName { get; set; }
        [Column("patient_mobile_number")]
        public string? PatientMobileNumber { get; set; }
        [ForeignKey("Clinic")]
        [Column("clinic_id")]
        [Required]
        public Guid ClinicId { get; set; }
        public Clinic? Clinic { get; set; } = null;
        [ForeignKey("User")]
        [Column("user_id")]
        [Required]
        public Guid UserId { get; set; }
        public User? User { get; set; } = null; //Doctor

        [Column("appointment_start_time")]
        public DateTime AppointmentStartTime { get; set; }
        [Column("appointment_end_time")]
        public DateTime AppointmentEndTime { get; set; }
        [Column("reminder_sent")]
        public int ReminderSent { get; set; } = 0;
        [Column("status")]
        public AppointmentStatus Status { get; set; }
    }
}
