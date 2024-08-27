using Application.Interfaces.IRepository.Base;

namespace Application.Interfaces.IUoW
{
    public interface IUoW
    {
        IRepository<T> Repository<T>() where T : class;

        Task SaveChangesAsync();
    }
}
