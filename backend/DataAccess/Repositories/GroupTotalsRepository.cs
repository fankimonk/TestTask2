using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class GroupTotalsRepository : IGroupTotalsRepository
    {
        private readonly TestTask2Context _dbContext;

        public GroupTotalsRepository(TestTask2Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GroupTotal?> AddAsync(GroupTotal total)
        {
            var createdTotal = await _dbContext.GroupTotals.AddAsync(total);
            if (createdTotal == null) return null;

            await _dbContext.SaveChangesAsync();
            return createdTotal.Entity;
        }
    }
}
