using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IClassesRepository
    {
        Task<Class?> AddAsync(Class accountClass);
        Task<List<Class>> GetWithIncludedDataAsync(int fileId);
    }
}
