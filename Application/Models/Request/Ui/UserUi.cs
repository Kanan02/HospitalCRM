using Application.Models.Request.Base;
using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.Models.Request.Ui
{
    public class UserUi:PagingUi
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public string? Msisdn { get; set; }
        public string? Password { get; set; }
        public ActivityStatus Status { get; set; } = ActivityStatus.Active;
        public string? Role { get; set; }
        public List<Guid>? Clinics { get; set; }
        public List<Guid>? Specialities { get; set; }
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public string? LastModifiedBy { get; set; }
    }
}
