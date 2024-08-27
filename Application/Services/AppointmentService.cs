using Application.Constants;
using Application.Interfaces.IServices;
using Application.Interfaces.IServices.Sms;
using Application.Interfaces.IUoW;
using Application.Models.Request;
using Application.Models.Request.Base;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Services.Base;
using Application.Spesifications.Base;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using Domain.Enums;
using Domain.Exceptions;
using System.Reflection;
using System.Security.Claims;

namespace Application.Services
{
    public class AppointmentService : BaseService<Appointment>, IAppointmentService
    {
        private readonly IMapper _mapper;
        private readonly ISmsService _smsService;
        private readonly IMessageProcessingService _messageProcessingService;
        private readonly IBackgroundService _backgroundService;
        public AppointmentService(IUoW unitOfWork, IMapper mapper, ISmsService smsService, IMessageProcessingService messageProcessingService, IBackgroundService backgroundService) : base(unitOfWork)
        {
            _mapper = mapper;
            _smsService = smsService;
            _messageProcessingService = messageProcessingService;
            _backgroundService = backgroundService;
        }
        public async Task<IReadOnlyList<AppoinmentDto>> Get(AppointmentUi model, IEnumerable<Claim> claims)
        {
            var role = claims.First(c => c.Type == ClaimTypes.Role).Value;
            var req = new AppointmentReq(model);
            if (role == RoleConstant.Doctor)
            {
                var userId = claims.First(c => c.Type == ClaimConstant.UserId).Value;
                req.UserId = new Guid(userId);
            }
            req.IncludeUser = true;
            var result = await GetListByFilter<AppoinmentDto>(req);
            return result;
        }

        public async Task<Appointment> Add(AppoinmentDto req)
        {
            var appointment = await ValidateAppointmentAndSendSms(req, TemplateType.AppointmentCreation);
            _unitOfWork.Repository<Appointment>().Add(appointment);
            await SaveUoW();
            return appointment;
        }

        public async Task<Appointment> Update(AppoinmentDto req)
        {
            if (req == null || req.Id == null)
            {
                throw new BadRequestException("appointmentId_is_required");
            }
            Appointment? appointment = (await _unitOfWork.Repository<Appointment>().GetAsync(u => u.Id == req.Id)).FirstOrDefault();
            if (appointment == null)
                throw new NotFoundException("Appointment", req.Id);

            _mapper.Map(req, appointment);
            appointment.LastModifiedAt = DateTime.Now;
            appointment = await ValidateAppointmentAndSendSms(req,
                req.Status == AppointmentStatus.Cancelled ? TemplateType.AppointmentCancellation : TemplateType.AppointmentModification,
                appointment);
            _unitOfWork.Repository<Appointment>().Update(appointment);
            await SaveUoW();
            return appointment;
        }
        private async Task<Appointment> ValidateAppointmentAndSendSms(AppoinmentDto req, TemplateType templateType, Appointment? app = null)
        {
            var existingAppointment = (await _unitOfWork.Repository<Appointment>()
    .GetAsync(u => u.AppointmentStartTime == req.AppointmentStartTime && u.UserId == req.DoctorId && u.Status != AppointmentStatus.Cancelled && u.Id != req.Id))
    .FirstOrDefault();

            if (existingAppointment is not null)
                throw new BadRequestException($"appointment_for_doctor_{req.DoctorId}_at_{req.AppointmentStartTime}_already_exists");

            var appointment = app ?? _mapper.Map<Appointment>(req);
            var clinic = (await _unitOfWork.Repository<Clinic>()
                .GetAsync(c => c.Id == req.ClinicId)
                ).FirstOrDefault();
            if (clinic is null)
                throw new NotFoundException("Clinic", req.ClinicId);

            var user = (await _unitOfWork.Repository<User>()
                .GetAsync(c => c.Id == req.DoctorId, includes: "Specialities")
                ).FirstOrDefault();
            if (user is null)
                throw new NotFoundException("User", req.DoctorId);

            //var text =await SendSms(templateType, appointment, clinic, user);
            //var notificationDateTime = DateTime.Now.AddHours(2);
            //if (templateType!=TemplateType.AppointmentCancellation&&appointment.AppointmentTime>notificationDateTime.AddMinutes(30))
            //{
            //    _backgroundService.Schedule(()=> SendSmsWrapper(TemplateType.AppointmentReminder, appointment, clinic, user), notificationDateTime);
            //}
            //if (true) //user.SendSmsNotification
            //{
            //    await _smsService.SendSms(text, user.Msisdn, "dsctest", ++clinic.TransactionId);
            //}

            return appointment;

        }
        public void SendSmsWrapper(TemplateType templateType, Appointment appointment, Clinic clinic, User user)
        {
            SendSms(templateType, appointment, clinic, user).GetAwaiter().GetResult();
        }

        public async Task<string> SendSms(TemplateType templateType, Appointment appointment, Clinic clinic, User user)
        {
            Console.WriteLine("1 " + DateTime.Now);

            var template = (await _unitOfWork.Repository<MessageTemplate>()
                .GetAsync(t => t.TemplateType == templateType && t.ClinicId == appointment.ClinicId)).FirstOrDefault();

            if (template is null)
                throw new NotFoundException("Template", templateType);

            var text = _messageProcessingService.ProcessMessage(template.Text,
                clinic.Name, appointment.AppointmentStartTime, appointment.PatientName, "Dr." +
                user.LastName + " " + user.FirstName, string.Join('/', user.Specialities));
            await _smsService.SendSms(text, appointment.PatientMobileNumber, "dsctest", ++clinic.TransactionId);
            Console.WriteLine(DateTime.Now);
            return text;
        }
        protected override ISpecification<Appointment> FilterList(BaseReq<Appointment> request, ISpecification<Appointment> spec)
        {
            var req = (AppointmentReq)request;
            var value = req.Value;

            if (req.IncludeUser)
            {
                spec.IncludeStrings.Add(nameof(Appointment.User));
            }
            if (req.UserId != null)
            {
                spec.Filters.Add(f => f.UserId == (Guid)req.UserId);
            }

            //to do
            if (req.FromDt != null)
            {
                spec.Filters.Add(x => x.AppointmentStartTime >= req.FromDt);
            }
            if (req.ToDt != null)
            {
                spec.Filters.Add(x => x.AppointmentStartTime <= req.ToDt);
            }



            if (req.ClinicId != null)
            {
                spec.Filters.Add(x => x.ClinicId == req.ClinicId);
            }
            if (req.SpecialityId != null)
            {
                spec.Filters.Add(f => f.User.Specialities.Any(u => u.Id == req.SpecialityId));
            }
            if (value != null)
            {
                if (!string.IsNullOrEmpty(value.PatientName))
                    spec.Filters.Add(f => f.PatientName == value.PatientName.Trim());
                if (!string.IsNullOrEmpty(value.PatientMobileNumber))
                    spec.Filters.Add(f => f.PatientMobileNumber == value.PatientMobileNumber.Trim());
            }
            return base.FilterList(request, spec);
        }
    }
}




// from must be less than start date || 
// start date must be less



