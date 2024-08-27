using Application.Models.Request.Base;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Models.Request
{
    public class SpecialityReq : PagingReq<Speciality>
    {
        public bool IncludeUsers { get; set; }
        public Expression<Func<Speciality, object>> orderByDescExpression;
    }
}
