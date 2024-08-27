using Application.Interfaces.IServices;
using Application.Interfaces.IServices.Base;
using Application.Interfaces.IServices.Security;
using Application.Services;
using Application.Services.Base;
using Application.Services.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IClinicService), typeof(ClinicService));
            services.AddScoped(typeof(ISpecialityService), typeof(SpecialityService));
            services.AddScoped(typeof(IAppointmentService), typeof(AppointmentService));
            services.AddScoped(typeof(IMessageTemplateService), typeof(MessageTemplateService));

            return services;
        }



    }
}
