using Application.Models.Request.Base;
using Application.Models.Request.Ui;
using Domain.Entities;
using Domain.Enums;
using System.Linq.Expressions;

namespace Application.Models.Request;

public class MessageTemplateReq : PagingReq<MessageTemplate>
{

    public Expression<Func<MessageTemplate, object>>? orderByDescExpression;
    public Guid? ClinicId { get; set; }
    public TemplateType? TemplateType { get; set; }
    public MessageTemplateReq() { }
    public MessageTemplateReq(MessageTemplateUi req)
    {
        ClinicId = req.ClinicId;
        TemplateType = req.TemplateType;
        Pager = req.CurrentPage == 0 && req.PageSize == 0 ? null : new PagingOptions
        {
            PageSize = req.PageSize,
            CurrentPage = req.CurrentPage
        };
        orderByDescExpression = (x) => x.Id;
    }
}