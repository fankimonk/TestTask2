using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IFilesRepository
    {
        Task<List<SheetFile>> GetAllAsync(int pageNumber = 1, int pageSize = 20);
        Task<SheetFile?> GetByIdAsync(int fileId);
        Task<SheetFile?> GetByNameAsync(string fileName);
        Task<SheetFile?> AddAsync(SheetFile file);
        Task<int?> DeleteAsync(int id);
        Task<int?> DeleteAsync(string fileName);
    }
}
