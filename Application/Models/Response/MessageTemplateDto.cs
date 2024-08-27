using Application.Models.Response.Base;
using Domain.Entities;
using Domain.Enums;
using Newtonsoft.Json;

namespace Application.Models.Response;

public class MessageTemplateDto : IListFilterDto<MessageTemplate, MessageTemplateDto>
{
    public Guid? Id { get; set; }
    public TemplateType TemplateType { get; set; }
    public string Text { get; set; }
    public Guid ClinicId { get; set; }

    [JsonIgnore]
    public string? CreatedBy { get; set; }
    [JsonIgnore]
    public string? LastModifiedBy { get; set; }
    public MessageTemplateDto SetDto(MessageTemplate entity)
    {
        Id = entity.Id;
        Text = entity.Text;
        TemplateType = entity.TemplateType;
        CreatedBy = entity.CreatedBy;
        ClinicId = entity.ClinicId;
        LastModifiedBy = entity.LastModifiedBy;
        return this;
    }
}