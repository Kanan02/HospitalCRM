using Application.Models.Response.Base;
using Domain.Entities;
using Newtonsoft.Json;

namespace Application.Models.Response
{
    public class SpecialityDto : IListFilterDto<Speciality, SpecialityDto>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public string? UpdatedBy { get; set; }
        public SpecialityDto() { }
        public SpecialityDto SetDto(Speciality entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            return this;
        }
    }
}
