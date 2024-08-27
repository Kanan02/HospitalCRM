using System.Linq.Expressions;

namespace Application.Spesifications.Base
{
    public interface ISpecification<T>
    {
        List<Expression<Func<T, bool>>> Filters { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
        void ApplyPaging(int skip, int take);
        void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression);
    }
}
