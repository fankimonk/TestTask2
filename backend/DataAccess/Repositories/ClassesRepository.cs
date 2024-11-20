using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ClassesRepository : IClassesRepository
    {
        private readonly TestTask2Context _dbContext;

        public ClassesRepository(TestTask2Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Class?> AddAsync(Class accountClass)
        {
            var createdClass = await _dbContext.Classes.AddAsync(accountClass);
            if (createdClass == null) return null;

            await _dbContext.SaveChangesAsync();
            return createdClass.Entity;
        }

        //Получить классы по id файла со всеми вложенными данными
        public async Task<List<Class>> GetWithIncludedDataAsync(int fileId)
        {
            return await _dbContext.Classes.AsNoTracking()
                .Where(c => c.FileId == fileId)
                .Include(c => c.ClassTotals)
                .Include(c => c.Groups)
                    .ThenInclude(g => g.GroupTotals)
                .Include(c => c.Groups)
                    .ThenInclude(g => g.Accounts)
                        .ThenInclude(a => a.BalanceSheetRecords)
                .ToListAsync();
        }
    }
}
