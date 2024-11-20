using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IPeriodsRepository
    {
        Task<Period?> AddOrGetExistingAsync(Period period);
    }
}
