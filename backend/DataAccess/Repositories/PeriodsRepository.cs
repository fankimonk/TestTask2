using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PeriodsRepository : IPeriodsRepository
    {
        private readonly TestTask2Context _dbContext;

        public PeriodsRepository(TestTask2Context dbContext)
        {
            _dbContext = dbContext;
        }

        //Если в БД существует период с такими датами вернуть его, иначе - создать
        public async Task<Period?> AddOrGetExistingAsync(Period period)
        {
            var existingPeriod = await _dbContext.Periods.AsNoTracking()
                .FirstOrDefaultAsync(p => p.StartDate == period.StartDate && p.EndDate == period.EndDate);
            if (existingPeriod != null) return existingPeriod;

            var createdPeriod = await _dbContext.Periods.AddAsync(period);
            if (createdPeriod == null) return null;

            await _dbContext.SaveChangesAsync();
            return createdPeriod.Entity;
        }
    }
}
