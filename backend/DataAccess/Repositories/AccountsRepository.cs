using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly TestTask2Context _dbContext;

        public AccountsRepository(TestTask2Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account?> AddAsync(Account account)
        {
            var createdAccount = await _dbContext.Accounts.AddAsync(account);
            if (createdAccount == null) return null;

            await _dbContext.SaveChangesAsync();
            return createdAccount.Entity;
        }
    }
}
