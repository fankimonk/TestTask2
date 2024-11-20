using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class BalanceSheetRecordsRepository : IBalanceSheetRecordsRepository
    {
        private readonly TestTask2Context _dbContext;

        public BalanceSheetRecordsRepository(TestTask2Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BalanceSheetRecord?> AddAsync(BalanceSheetRecord record)
        {
            var createdRecord = await _dbContext.BalanceSheetRecords.AddAsync(record);
            if (createdRecord == null) return null;

            await _dbContext.SaveChangesAsync();
            return createdRecord.Entity;
        }
    }
}
