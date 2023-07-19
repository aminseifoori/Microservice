using Employee.Model;

namespace Employee.Interface
{
    public interface IStaffRepository
    {
        Task CreateAsync(Staff entity);
        Task<IReadOnlyCollection<Staff>> GetAllAsync();
        Task<Staff> GetByIdAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(Staff entity);
    }
}