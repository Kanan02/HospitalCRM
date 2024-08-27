using Application.Interfaces.IServices.Sms;
using Domain.Constants;

namespace Infrastructure.Sms
{
    public class MessageProcessingService : IMessageProcessingService
    {
        private readonly Dictionary<char, string> translitMap = new Dictionary<char, string>
        {
            // Lowercase
            {'ç', "c"}, {'ə', "e"}, {'ğ', "g"}, {'ı', "i"},
            {'ö', "o"}, {'ş', "s"}, {'ü', "u"},
            // Uppercase
            {'Ç', "C"}, {'Ə', "E"}, {'Ğ', "G"}, {'I', "I"},
            {'Ö', "O"}, {'Ş', "S"}, {'Ü', "U"}
        };
        public string ProcessMessage(string templateText, string clinicName, DateTime appointmentTime, string patientName, string doctorName, string speciality)
        {
            var modifiedText = templateText.Replace(TemplateConstant.ClinicName, clinicName)
            .Replace(TemplateConstant.DoctorName, doctorName)
                .Replace(TemplateConstant.DoctorSpeciality, speciality)
                .Replace(TemplateConstant.AppointmentTime, appointmentTime.ToString())
                .Replace(TemplateConstant.PatientName, patientName);

            return TranslitMessage(modifiedText);
        }
        private string TranslitMessage(string text)
        {
            var result = string.Empty;
            foreach (char c in text)
            {
                result += translitMap.TryGetValue(c, out string replacement) ? replacement : c.ToString();
            }
            return result;
        }
    }
}
