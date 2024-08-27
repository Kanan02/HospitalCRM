using Application.Models.Request.Ui;
using Application.Models.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // For users
            CreateMap<UserUi, User>()
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.Clinics, opt => opt.Ignore())
                .ForMember(dest => dest.Specialities, opt => opt.Ignore())
                .ReverseMap();
            //For Clinics
            CreateMap<ClinicDto, Clinic>()
                .ReverseMap();
            //For Specialities
            CreateMap<SpecialityDto, Speciality>()
                .ReverseMap();
            //For appointment
            CreateMap<AppoinmentDto, Appointment>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.DoctorId));
            //For message template
            CreateMap<MessageTemplateDto, MessageTemplate>()
                .ReverseMap();
        }
    }
}
