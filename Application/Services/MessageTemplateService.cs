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

namespace Application.Services;

public class MessageTemplateService : BaseService<MessageTemplate>, IMessageTemplateService
{
    private readonly IMapper _mapper;
    public MessageTemplateService(IUoW unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<PagedResponseList<MessageTemplateDto>> Get(MessageTemplateUi ui)
    {
        MessageTemplateReq req = new(ui);
        var result = await GetListByFilter<MessageTemplateDto>(req);
        var count = await GetCountByFilter(req);
        return new PagedResponseList<MessageTemplateDto>() { Count = count, List = result };
    }

    public async Task<MessageTemplate> Add(MessageTemplateDto req)
    {
        MessageTemplate? messageTemplate = (await _unitOfWork.Repository<MessageTemplate>().GetAsync(u => u.TemplateType == req.TemplateType && u.ClinicId == req.ClinicId)).FirstOrDefault();
        if (messageTemplate != null)
            throw new BadRequestException($"message_template_of_this_type_for_this_clinic_already_exists");
        messageTemplate = _mapper.Map<MessageTemplate>(req);
        _unitOfWork.Repository<MessageTemplate>().Add(messageTemplate);

        await SaveUoW();

        return messageTemplate;
    }

    public async Task<MessageTemplate> Update(MessageTemplateDto req)
    {
        if (req == null || req.Id == null)
        {
            throw new BadRequestException("message_template_id_is_required");
        }
        MessageTemplate? messageTemplate = (await _unitOfWork.Repository<MessageTemplate>().GetAsync(u => u.Id == req.Id)).FirstOrDefault();
        if (messageTemplate == null)
            throw new NotFoundException("message_template", req.Id);

        messageTemplate = (await _unitOfWork.Repository<MessageTemplate>().GetAsync(u => u.TemplateType == req.TemplateType && u.ClinicId == req.ClinicId)).FirstOrDefault();
        if (messageTemplate != null && messageTemplate.Id != req.Id)
            throw new BadRequestException($"message_template_of_this_type_for_this_clinic_already_exists");
        //messageTemplate = _mapper.Map<MessageTemplate>(req);
        _mapper.Map(req, messageTemplate);
        messageTemplate.LastModifiedAt = DateTime.Now;
        _unitOfWork.Repository<MessageTemplate>().Update(messageTemplate);
        await SaveUoW();
        return messageTemplate;
    }


    protected override ISpecification<MessageTemplate> FilterList(BaseReq<MessageTemplate> request, ISpecification<MessageTemplate> spec)
    {

        var req = (MessageTemplateReq)request;
        if (req.Pager != null)
            spec.ApplyPaging(req.Pager.Skip, req.Pager.PageSize);

        if (req.orderByDescExpression != null)
            spec.ApplyOrderByDescending(req.orderByDescExpression);

        if (req.ClinicId != null)
            spec.Filters.Add(f => f.ClinicId == req.ClinicId);
        if (req.TemplateType != null)
            spec.Filters.Add(f => f.TemplateType == req.TemplateType);

        return base.FilterList(request, spec);
    }
}