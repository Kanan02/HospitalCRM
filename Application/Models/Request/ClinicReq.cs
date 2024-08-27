using Application.Models.Request.Base;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Models.Request
{
    public class ClinicReq : PagingReq<Clinic>
    {
        public Guid? UserId { get; set; } = null;
        public Expression<Func<Clinic, object>> orderByDescExpression;
    }
}
