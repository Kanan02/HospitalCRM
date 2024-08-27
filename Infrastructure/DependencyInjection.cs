using Application.Helpers;
using Application.Interfaces.IRepository.Base;
using Application.Interfaces.IServices;
using Application.Interfaces.IServices.ApiBase;
using Application.Interfaces.IServices.Security;
using Application.Interfaces.IServices.Sms;
using Application.Interfaces.IUoW;
using Application.Models.AppSetting;
using Hangfire;
using Hangfire.MySql;
using Infrastructure.ApiBase;
using Infrastructure.Data.Context;
using Infrastructure.Data.UntOfWork;
using Infrastructure.Jobs;
using Infrastructure.Otp;
using Infrastructure.Repository.Base;
using Infrastructure.Security;
using Infrastructure.Sms;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            ConfigureJWT(services, configuration);
            var connectionString = configuration["ConnectionStrings:Dev"];
            var hangfireConnectionString = configuration["ConnectionStrings:HangfireDbDev"];
            services.AddDbContextPool<HospitalDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUoW, UoW>();
            // background services
            services.AddHangfire(configuration => configuration
            .UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })
             .UseStorage(
                 new MySqlStorage(
                     hangfireConnectionString,
                     new MySqlStorageOptions
                     {
                         QueuePollInterval = TimeSpan.FromSeconds(10),
                         JobExpirationCheckInterval = TimeSpan.FromHours(3),
                         CountersAggregateInterval = TimeSpan.FromMinutes(5),
                         PrepareSchemaIfNecessary = true,
                         DashboardJobListLimit = 25000,
                         TransactionTimeout = TimeSpan.FromMinutes(1),
                         TablesPrefix = "Hangfire",
                     }
                 )
             ));
            services.AddHangfireServer();
            // security services
            services.AddScoped<ISecurityService, SecurityService>();
            //additional services
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IMessageProcessingService, MessageProcessingService>();
            services.AddScoped<IApiBaseService, ApiBaseService>();
            services.AddScoped<IBackgroundService, BackgroundService>();
            services.AddHttpClient("OtpAPI", client =>
            {
                client.BaseAddress =
                    new Uri(configuration["OtpService:Url"]);
            });

            services.AddHttpClient("PartnerAPI", client =>
            {
                client.BaseAddress =
                    new Uri(configuration["PartnerService:Url"]);
            });

            services.AddHttpClient("SmsAPI", client =>
            {
                client.BaseAddress =
                    new Uri(configuration["SmsService:Url"]);
            });

            // common services

            return services;
        }

        private static void ConfigureJWT(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSetting = AppSettingHelper.BindSetting<JwtSetting>(configuration);
            var key = Encoding.ASCII.GetBytes(jwtSetting.SecretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(x =>
           {
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                   ClockSkew = TimeSpan.Zero,

                   IssuerSigningKey = new SymmetricSecurityKey(key)
               };
           });
        }
    }
}
