using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class FilesRepository : IFilesRepository
    {
        private readonly TestTask2Context _dbContext;

        public FilesRepository(TestTask2Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SheetFile?> AddAsync(SheetFile file)
        {
            var existingFile = await GetByNameAsync(file.FileName);
            if (existingFile != null) return null;

            var createdFile = await _dbContext.SheetFiles.AddAsync(file);
            if (createdFile == null) return null;

            await _dbContext.SaveChangesAsync();
            return createdFile.Entity;
        }

        public async Task<int?> DeleteAsync(int id)
        {
            var rowsDeleted = await _dbContext.SheetFiles.Where(f => f.FileId == id).ExecuteDeleteAsync();
            if (rowsDeleted < 1) return null;
            await _dbContext.SaveChangesAsync();

            return id;
        }

        public async Task<int?> DeleteAsync(string fileName)
        {
            var file = await GetByNameAsync(fileName);
            if (file == null) return null;

            var fileId = await DeleteAsync(file.FileId);
            if (fileId == null) return null;

            return fileId;
        }

        public async Task<List<SheetFile>> GetAllAsync(int pageNumber = 1, int pageSize = 20)
        {
            var skipNumber = (pageNumber - 1) * pageSize;
            return await _dbContext.SheetFiles.AsNoTracking()
                .Skip(skipNumber).Take(pageSize)
                .Include(f => f.Bank).Include(f => f.Period).ToListAsync();
        }

        public async Task<SheetFile?> GetByIdAsync(int fileId)
        {
            return await _dbContext.SheetFiles.AsNoTracking().FirstOrDefaultAsync(f => f.FileId == fileId);
        }

        public async Task<SheetFile?> GetByNameAsync(string fileName)
        {
            return await _dbContext.SheetFiles.AsNoTracking().FirstOrDefaultAsync(f => f.FileName == fileName);
        }
    }
}
