using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class GlobalTotalsRepository : IGlobalTotalsRepository
    {
        private readonly TestTask2Context _dbContext;

        public GlobalTotalsRepository(TestTask2Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GlobalTotal?> AddAsync(GlobalTotal total)
        {
            var createdTotal = await _dbContext.GlobalTotals.AddAsync(total);
            if (createdTotal == null) return null;

            await _dbContext.SaveChangesAsync();
            return createdTotal.Entity;
        }

        //Получить итоги по файлу
        public async Task<GlobalTotal?> GetByFileId(int fileId)
        {
            return await _dbContext.GlobalTotals.AsNoTracking().FirstOrDefaultAsync(gt => gt.FileId == fileId);
        }
    }
}
