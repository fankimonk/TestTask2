using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IGlobalTotalsRepository
    {
        Task<GlobalTotal?> AddAsync(GlobalTotal total);
        Task<GlobalTotal?> GetByFileId(int fileId);
    }
}
