using Application.Spesifications.Base;
using System.Linq.Expressions;

namespace Application.Interfaces.IRepository.Base
{
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        string includes = null,
                                        bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAsync(List<Expression<Func<T, bool>>> predicates = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        List<Expression<Func<T, object>>> includes = null,
                                        bool disableTracking = true);

        Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec, bool trackdb = true);
        Task<T> GetByIdAsync<TId>(TId id);
        T Add(T entity);
        public void Update(T entity);
        void Update(T @new, T old);
        void Delete(T entity);
        Task<int> CountAsync(ISpecification<T> spec);
        void RemoveAll(List<T> list);
        Task AddAllAsync(List<T> list);
        Task LoadCollectionAsync<TProperty>(
    T entity,
    Expression<Func<T, IEnumerable<TProperty>>> navigationPropertyPath)
    where TProperty : class;
    }
}
