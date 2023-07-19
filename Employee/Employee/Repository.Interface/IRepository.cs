using Employee.Model;
using Employee.Model.Interface;

namespace Employee.Repository.Interface
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(T entity);
    }
}
