using Domain.Common;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("message_templates")]
public class MessageTemplate : BaseEntity
{
    [Column("template_type")]
    public TemplateType TemplateType { get; set; }
    [Column("text")]
    public string Text { get; set; }
    [Column("clinic_id")]
    public Guid ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
}