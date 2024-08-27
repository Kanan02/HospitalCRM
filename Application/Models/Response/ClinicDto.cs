using Application.Models.Response.Base;
using Domain.Entities;
using Newtonsoft.Json;

namespace Application.Models.Response
{
    public class ClinicDto : IListFilterDto<Clinic, ClinicDto>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? ContactInfo { get; set; }
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public string? UpdatedBy { get; set; }
        public ClinicDto() { }
        public ClinicDto SetDto(Clinic entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Address = entity.Address;
            ContactInfo = entity.ContactInfo;
            return this;
        }
    }
}
