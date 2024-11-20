using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IAccountsRepository
    {
        Task<Account?> AddAsync(Account account);
    }
}
