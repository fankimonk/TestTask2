using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IGroupTotalsRepository
    {
        Task<GroupTotal?> AddAsync(GroupTotal total);
    }
}
