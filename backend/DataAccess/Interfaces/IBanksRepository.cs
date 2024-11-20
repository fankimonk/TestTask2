using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IBanksRepository
    {
        Task<Bank?> AddOrGetExistingAsync(Bank bank);
    }
}
