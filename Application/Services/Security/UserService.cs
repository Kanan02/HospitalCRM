using Application.Constants;
using Application.Interfaces.IServices.Security;
using Application.Interfaces.IServices.Sms;
using Application.Interfaces.IUoW;
using Application.Models.Request.Base;
using Application.Models.Request.Security;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Models.Response.Base;
using Application.Models.Response.Sms;
using Application.Services.Base;
using Application.Spesifications.Base;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using Domain.Exceptions;
using System.Security.Cryptography;
using System.Xml;

namespace Application.Services.Security
{
    public class UserService : BaseService<User>, IUserService
    {
        private ISecurityService _securityService { get; }
        private IOtpService _otpService { get; }
        private readonly IMapper _mapper;
        public UserService(IUoW unitOfWork, ISecurityService securityService, IOtpService otpService, IMapper mapper) : base(unitOfWork)
        {
            _securityService = securityService;
            _mapper = mapper;
            _otpService = otpService;
        }

        public async Task<LoginRes> SignIn(UserReq req)
        {
            req.IncludeRole = true;
            ValidateLoginInput(req);
            User? user = (await _unitOfWork.Repository<User>().GetAsync(u => u.Msisdn == req.Value.Msisdn, includes: $"{nameof(User.Role)}")).FirstOrDefault();
            if (user == null)
                throw new NotFoundException(nameof(User), req.Value.Msisdn);
            if (!VerifyPasswordHash(req.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new NotFoundException($"wrong_password", req.Value.Msisdn);
            }
            var jwt = _securityService.GenerateToken(user);
            GenerateRefreshToken(user);
            await Save(user);
            return new LoginRes(user, jwt);
        }
        public async Task<LoginRes> RefreshToken(ManageTokenReq model)
        {
            User? user = (await _unitOfWork.Repository<User>().GetAsync(u => u.RefreshToken == model.Token, includes: $"{nameof(User.Role)}")).FirstOrDefault();
            if (user == null)
                throw new UnauthorizedException($"user_with_token_not_found");

            // return null if token is no longer active
            if (DateTime.Now >= user.RefreshTokenExpiryTime || user.RefreshToken == null)
                throw new UnauthorizedException("token_expired");

            var jwt = _securityService.GenerateToken(user);
            // replace old refresh token with a new one and save
            GenerateRefreshToken(user);
            // generate new jwt

            await Save(user);
            return new LoginRes(user, jwt);
        }

        public async Task<bool> SignOut(ManageTokenReq model)
        {
            User? user = (await _unitOfWork.Repository<User>().GetAsync(u => u.RefreshToken == model.Token, includes: $"{nameof(User.Role)}")).FirstOrDefault();
            if (user == null)
                throw new UnauthorizedException($"user_with_token_not_found");

            // return null if token is no longer active
            if (DateTime.UtcNow >= user.RefreshTokenExpiryTime) throw new UnauthorizedException("token_expired");

            user.RefreshToken = null;

            await Save(user);

            return true;
        }
        public async Task<LoginRes> SignUp(SignUpReq req, string role)
        {
            req.IncludeRole = true;
            var user = (await GetListByFilter(req)).FirstOrDefault();
            if (user != null)
                throw new BadRequestException("user_already_exists");

            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user = req.Value;
            user.CreatedBy = "Manual";
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = (await _unitOfWork.Repository<Role>().GetAsync(r => r.Name == role)).FirstOrDefault() ?? throw new ApiException($"{role}_role_not_found");
            var jwt = _securityService.GenerateToken(user);
            GenerateRefreshToken(user);
            _unitOfWork.Repository<User>().Add(user);

            await SaveUoW();
            return new LoginRes(user, jwt);
        }

        public async Task<bool> ResetPassword(ResetPwdReq req)
        {

            User? user = (await _unitOfWork.Repository<User>().GetAsync(u => u.Msisdn == req.Msisdn, includes: $"{nameof(User.Role)}"))
                .FirstOrDefault();

            if (user == null)
                throw new NotFoundException("user", req.Msisdn);

            var isOtpCorrect = await _otpService.VerifyCode(new VerifyOtpReq()
            {
                Msisdn = user.Msisdn,
                OtpCode = req.OtpCode,
                ServiceName = "sozoyunu"
            });
            if (!isOtpCorrect.Success)
            {
                throw new OtpCodeExpiredException("OTP Code Expired");
            }

            CreatePasswordHash(req.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await Save(user);
            return true;
        }

        public async Task<User> Add(UserUi req)
        {
            User? user = (await _unitOfWork.Repository<User>().GetAsync(u => u.Msisdn == req.Msisdn)).FirstOrDefault();
            if (user != null)
                throw new UnauthorizedException($"user_already_exists");

            user = _mapper.Map<User>(req);
            user = await SetUserData(req, user);
            _unitOfWork.Repository<User>().Add(user);
            await SaveUoW();
            return user;
        }

        public async Task<User> Update(UserUi req)
        {
            if (req == null || req.Id == null)
            {
                throw new BadRequestException("User Id is required");
            }
            User? user = (await _unitOfWork.Repository<User>().GetAsync(u => u.Id == req.Id, includes: $"{nameof(User.Role)}")).FirstOrDefault();
            if (user == null)
                throw new NotFoundException("user", req.Id);

            user = (await _unitOfWork.Repository<User>().GetAsync(u => u.Msisdn == req.Msisdn, includes: $"{nameof(User.Role)}")).FirstOrDefault();
            if (user != null && user.Id != req.Id)
                throw new BadRequestException("user_already_exists");
            _mapper.Map(req, user);
            user = await SetUserData(req, user);
            user.LastModifiedAt = DateTime.Now;
            _unitOfWork.Repository<User>().Update(user);
            await SaveUoW();
            return user;
        }
        public async Task<PagedResponseList<UserDto>> Get(UserUi request)
        {
            var req = new UserReq(request);
            if (request.Role != null)
                req.Value.Role = (await _unitOfWork.Repository<Role>().GetAsync(r => r.Name == request.Role)).FirstOrDefault() ?? throw new ApiException($"{request.Role}_role_not_found");
            req.IncludeRole = true;
            req.IncludeClinics = true;
            req.IncludeSpecialities = true;
            var result = await GetListByFilter<UserDto>(req);
            var count = await GetCountByFilter(req);
            return new PagedResponseList<UserDto>() { List = result, Count = count };
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        }
        private void GenerateRefreshToken(User user)
        {
            user.RefreshToken = getUniqueToken();

            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(14);

            string getUniqueToken()
            {
                // token is a cryptographically strong random sequence of values
                // Securely generate random data
                var randomData = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

                // Combine user-specific information, timestamp, and random data
                var tokenRaw = $"{user.Id}|{DateTime.UtcNow.Ticks}|{randomData}";

                //  hash the combined string for a consistent format and added security
                using var sha256 = SHA256.Create();
                var tokenHash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(tokenRaw));
                var token = Convert.ToBase64String(tokenHash);

                return token;
            }
        }



        private async Task<User> SetUserData(UserUi req, User user)
        {
            user.Role = (await _unitOfWork.Repository<Role>().GetAsync(r => r.Name == req.Role)).FirstOrDefault() ?? throw new BadRequestException($"{req.Role}_role_not_found");
            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            if (req.Clinics != null)
            {
                var clinicsToUpdate = await _unitOfWork.Repository<Clinic>().GetAsync(c => req.Clinics.Contains(c.Id));
                user.Clinics = new List<Clinic>(clinicsToUpdate);
            }
            if (req.Specialities != null && user.Role.Name == RoleConstant.Doctor)
            {
                var specialitiesToUpdate = await _unitOfWork.Repository<Speciality>().GetAsync(s => req.Specialities.Contains(s.Id));
                user.Specialities = new List<Speciality>(specialitiesToUpdate);
            }
            return user;
        }
        private void ValidateLoginInput(UserReq req)
        {
            if (req.Value == null
                || string.IsNullOrEmpty(req.Value.Msisdn)
                || string.IsNullOrEmpty(req.Password))
                throw new BadRequestException("phone_and_password_required");
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
        protected override ISpecification<User> FilterList(BaseReq<User> request, ISpecification<User> spec)
        {
            var req = (UserReq)request;
            var value = req.Value;
            
            if (req.IncludeRole)
            {
                var roleInclude = $"{nameof(User.Role)}";
                spec.IncludeStrings.Add(roleInclude);
            }
            if (req.IncludeClinics)
            {
                var clinicsInclude = $"{nameof(User.Clinics)}";
                spec.IncludeStrings.Add(clinicsInclude);
              
            }
            if (req.IncludeSpecialities)
            {
                var specialitiesInclude = $"{nameof(User.Specialities)}";
                spec.IncludeStrings.Add(specialitiesInclude);

            }

            if (value != null)
            {
                if (!string.IsNullOrEmpty(value.Msisdn))
                    spec.Filters.Add(f => f.Msisdn == value.Msisdn.Trim());
                if (value.Status > 0)
                    spec.Filters.Add(x => x.Status == value.Status);
                if (!string.IsNullOrEmpty(value.FirstName))
                    spec.Filters.Add(f => f.FirstName == value.FirstName.Trim());
                if (!string.IsNullOrEmpty(value.LastName))
                    spec.Filters.Add(f => f.LastName == value.LastName.Trim());
                if (!string.IsNullOrEmpty(value.Patronymic))
                    spec.Filters.Add(f => f.Patronymic == value.Patronymic.Trim());
                if (value.Role != null)
                    spec.Filters.Add(f => f.Role == value.Role);
                if (req.Pager != null)
                    spec.ApplyPaging(req.Pager.Skip, req.Pager.PageSize);
                if (req.orderByDescExpression != null)
                    spec.ApplyOrderByDescending(req.orderByDescExpression);

            }

            return base.FilterList(request, spec);
        }

    }
}
