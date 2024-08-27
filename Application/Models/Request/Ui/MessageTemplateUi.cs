using Application.Models.Request.Base;
using Domain.Enums;

namespace Application.Models.Request.Ui
{
    public class MessageTemplateUi:PagingUi
    {
        public Guid? ClinicId { get; set; }
        public TemplateType? TemplateType { get; set; }

    }
}
