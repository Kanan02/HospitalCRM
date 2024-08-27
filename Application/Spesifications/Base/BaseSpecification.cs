using System.Linq.Expressions;

namespace Application.Spesifications.Base
{
    public class BaseSpecification<T> : ISpecification<T>
    {

        //public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, bool>>> Filters { get; set; } = new List<Expression<Func<T, bool>>>();
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; } = false;

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);

        protected virtual void AddCriteria(Expression<Func<T, bool>> criteria) => Filters.Add(criteria);

        protected virtual void AddInclude(string includeString) => IncludeStrings.Add(includeString);

        public virtual void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression) => OrderBy = orderByExpression;

        public virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        => OrderByDescending = orderByDescendingExpression;

    }
}
