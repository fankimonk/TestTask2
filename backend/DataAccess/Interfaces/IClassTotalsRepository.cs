using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IClassTotalsRepository
    {
        Task<ClassTotal?> AddAsync(ClassTotal total);
    }
}
