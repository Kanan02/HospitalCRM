using Application.Interfaces.IServices;
using Application.Interfaces.IUoW;
using Application.Models.Request;
using Application.Models.Request.Base;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Models.Response.Base;
using Application.Services.Base;
using Application.Spesifications.Base;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Services
{
    public class ClinicService : BaseService<Clinic>, IClinicService
    {
        private readonly IMapper _mapper;
        public ClinicService(IUoW unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }
        public async Task<PagedResponseList<ClinicDto>> Get(PagingUi pager, Guid? userId = null)
        {

            var req = new ClinicReq
            {
                //IsRead = false,
                UserId = userId,
                orderByDescExpression = (x) => x.Id,
                Pager = pager.CurrentPage == 0 && pager.PageSize == 0 ? null : new PagingOptions
                {
                    PageSize = pager.PageSize,
                    CurrentPage = pager.CurrentPage
                }
            };
            var result = await GetListByFilter<ClinicDto>(req);
            var count = await GetCountByFilter(req);
            return new PagedResponseList<ClinicDto>() { Count = count, List = result };
        }

        public async Task<Clinic> Add(ClinicDto req)
        {
            Clinic? clinic = (await _unitOfWork.Repository<Clinic>().GetAsync(u => u.Name == req.Name)).FirstOrDefault();
            if (clinic != null)
                throw new UnauthorizedException($"clinic_already_exists");

            clinic = _mapper.Map<Clinic>(req);
            ClinicModule clinicModule = new()
            {
                Clinic = clinic,
                Module = (await _unitOfWork.Repository<Module>().GetAsync(m => m.Name == "Queue Module")).FirstOrDefault()
            };
            _unitOfWork.Repository<ClinicModule>().Add(clinicModule);
            _unitOfWork.Repository<Clinic>().Add(clinic);
            await SaveUoW();
            return clinic;
        }
        protected override ISpecification<Clinic> FilterList(BaseReq<Clinic> request, ISpecification<Clinic> spec)
        {
            var req = (ClinicReq)request;
            if (req.Pager != null)
                spec.ApplyPaging(req.Pager.Skip, req.Pager.PageSize);

            if (req.orderByDescExpression != null)
                spec.ApplyOrderByDescending(req.orderByDescExpression);

            if (req.UserId != null)
                spec.Filters.Add(f => f.Users.Any(u => u.Id == req.UserId));
            return base.FilterList(request, spec);
        }
    }
}
