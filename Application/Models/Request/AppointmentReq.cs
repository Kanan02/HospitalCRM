using Application.Models.Request.Base;
using Application.Models.Request.Ui;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Models.Request
{
    public class AppointmentReq : PagingReq<Appointment>
    {
        public bool IncludeUser { get; set; }
        public DateTime? FromDt { get; set; }
        public Guid? ClinicId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? SpecialityId { get; set; }
        public DateTime? ToDt { get; set; }
        public Expression<Func<Speciality, object>>? orderByDescExpression;

        public AppointmentReq() { }
        public AppointmentReq(AppointmentUi req)
        {
            FromDt = req.FromDt;
            ToDt = req.ToDt;
            ClinicId = req.ClinicId;
            SpecialityId = req.SpecialityId;
            UserId = req.UserId;
            Value = new Appointment
            {
                PatientMobileNumber = req.PatientMobileNumber,
                PatientName = req.PatientName,
            };
        }
    }
}
