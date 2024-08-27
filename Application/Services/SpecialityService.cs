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
    public class SpecialityService : BaseService<Speciality>, ISpecialityService
    {
        private readonly IMapper _mapper;
        public SpecialityService(IUoW unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }
        public async Task<PagedResponseList<SpecialityDto>> Get(PagingUi pager)
        {

            var req = new SpecialityReq
            {
                //IsRead = false,
                orderByDescExpression = (x) => x.Id,
                Pager =pager.CurrentPage==0&&pager.PageSize==0?null: new PagingOptions
                {
                    PageSize =  pager.PageSize,
                    CurrentPage = pager.CurrentPage
                }
            };
            var result = await GetListByFilter<SpecialityDto>(req);
            var count = await GetCountByFilter(req);
            return new PagedResponseList<SpecialityDto>() { Count = count, List = result };
        }

        public async Task<Speciality> Add(SpecialityDto req)
        {
            Speciality? spec = (await _unitOfWork.Repository<Speciality>().GetAsync(u => u.Name == req.Name)).FirstOrDefault();
            if (spec != null)
                throw new UnauthorizedException($"speciality_already_exists");

            spec = _mapper.Map<Speciality>(req);
            _unitOfWork.Repository<Speciality>().Add(spec);
            await SaveUoW();
            return spec;
        }
        protected override ISpecification<Speciality> FilterList(BaseReq<Speciality> request, ISpecification<Speciality> spec)
        {
            var req = (SpecialityReq)request;
            if (req.Pager != null)
                spec.ApplyPaging(req.Pager.Skip, req.Pager.PageSize);

            if (req.orderByDescExpression != null)
                spec.ApplyOrderByDescending(req.orderByDescExpression);
            return base.FilterList(request, spec);
        }


    }
}
