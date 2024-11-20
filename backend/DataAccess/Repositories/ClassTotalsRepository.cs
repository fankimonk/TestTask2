using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class ClassTotalsRepository : IClassTotalsRepository
    {
        private readonly TestTask2Context _dbContext;

        public ClassTotalsRepository(TestTask2Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ClassTotal?> AddAsync(ClassTotal total)
        {
            var createdTotal = await _dbContext.ClassTotals.AddAsync(total);
            if (createdTotal == null) return null;

            await _dbContext.SaveChangesAsync();
            return createdTotal.Entity;
        }
    }
}
