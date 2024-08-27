using Application.Models.Response.Base;
using Domain.Entities;
using Domain.Entities.Security;
using Domain.Enums;

namespace Application.Models.Response
{
    public class UserDto : IListFilterDto<User, UserDto>
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public string? Msisdn { get; set; }
        public string? Role { get; set; }
        public ActivityStatus Status { get; set; }
        public List<ClinicDto>? Clinics { get; set; }
        public List<SpecialityDto>? Specialities { get; set; }
        public UserDto SetDto(User entity)
        {
            Id = entity.Id;
            FirstName = entity.FirstName;
            LastName = entity.LastName;
            Patronymic = entity.Patronymic;
            Msisdn = entity.Msisdn;
            Role = entity.Role.Name;
            Status=entity.Status;
            Clinics = entity.Clinics?.Select(clinic => new ClinicDto
            {
                Id = clinic.Id,
                Name = clinic.Name
            }).ToList();

            Specialities = entity.Specialities?.Select(speciality => new SpecialityDto
            {
                Id = speciality.Id,
                Name = speciality.Name
            }).ToList();
            return this;
        }
    }
}
