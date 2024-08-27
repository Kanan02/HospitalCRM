using Application.Constants;
using Application.Interfaces.IServices;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Models.Response.Base;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace web.Controllers;


[Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Superadmin}")]
public class MessageTemplateController : BaseController<MessageTemplate>
{
    private IMessageTemplateService _messageTemplateService { get; }

    public MessageTemplateController(IMessageTemplateService messageTemplateService) : base(messageTemplateService)
    {
        _messageTemplateService = messageTemplateService;
    }

    [HttpPost]
    public async Task<ApiValueResponse<MessageTemplate>> Add([FromBody] MessageTemplateDto entity)
    {

        var claim = HttpContext.User.Claims.First(c => c.Type == ClaimConstant.UserId);
        entity.CreatedBy = claim.Value;
        return new ApiValueResponse<MessageTemplate>(await _messageTemplateService.Add(entity));
    }
    [HttpPut]
    public async Task<ApiValueResponse<MessageTemplate>> Update([FromBody] MessageTemplateDto entity)
    {

        var claim = HttpContext.User.Claims.First(c => c.Type == ClaimConstant.UserId);
        entity.LastModifiedBy = claim.Value;
        return new ApiValueResponse<MessageTemplate>(await _messageTemplateService.Update(entity));
    }

    [HttpGet]
    public async Task<ApiValueResponse<PagedResponseList<MessageTemplateDto>>> GetAll([FromQuery] MessageTemplateUi req)
    {
        return new ApiValueResponse<PagedResponseList<MessageTemplateDto>>(await _messageTemplateService.Get(req));
    }
}