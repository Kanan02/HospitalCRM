using Application.Interfaces.IServices.Base;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Models.Response.Base;
using Domain.Entities;

namespace Application.Interfaces.IServices;

public interface IMessageTemplateService : IBaseService<MessageTemplate>
{
    Task<PagedResponseList<MessageTemplateDto>> Get(MessageTemplateUi ui);
    Task<MessageTemplate> Add(MessageTemplateDto req);
    Task<MessageTemplate> Update(MessageTemplateDto req);
}