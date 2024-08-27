using Application.Models.Response.Base;
using Domain.Entities;
using Domain.Enums;

namespace Application.Models.Response
{
    public class AppoinmentDto : IListFilterDto<Appointment, AppoinmentDto>
    {
        public Guid? Id { get; set; }
        public string? PatientName { get; set; }
        public string? PatientMobileNumber { get; set; }
        public Guid ClinicId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime AppointmentStartTime { get; set; }
        public DateTime AppointmentEndTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public AppoinmentDto SetDto(Appointment entity)
        {
            Id = entity.Id;
            PatientName = entity.PatientName;
            PatientMobileNumber = entity.PatientMobileNumber; ;
            DoctorId = entity.UserId;
            AppointmentStartTime = entity.AppointmentStartTime;
            Status = entity.Status;
            ClinicId = entity.ClinicId;
            CreatedBy = entity.CreatedBy;
            LastModifiedBy = entity.LastModifiedBy;
            AppointmentEndTime = entity.AppointmentEndTime;
            return this;
        }
    }
}
