using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IGroupsRepository
    {
        Task<Group?> AddAsync(Group group);
    }
}
