using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class GroupsRepository : IGroupsRepository
    {
        private readonly TestTask2Context _dbContext;

        public GroupsRepository(TestTask2Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Group?> AddAsync(Group group)
        {
            var createdGroup = await _dbContext.Groups.AddAsync(group);
            if (createdGroup == null) return null;

            await _dbContext.SaveChangesAsync();
            return createdGroup.Entity;
        }
    }
}
