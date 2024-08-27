namespace Application.Models.Request.Ui
{
    public class AppointmentUi
    {
        public Guid? ClinicId { get; set; }
        public Guid? SpecialityId { get; set; }
        public string? PatientName { get; set; }
        public string? PatientMobileNumber { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? FromDt { get; set; }
        public DateTime? ToDt { get; set; }
    }
}
