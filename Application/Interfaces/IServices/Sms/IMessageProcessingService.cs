namespace Application.Interfaces.IServices.Sms
{
    public interface IMessageProcessingService
    {
        public string ProcessMessage(string templateText, string clinicName, DateTime appointmentTime, string patientName, string doctorName, string speciality);
    }
}
