using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class BanksRepository : IBanksRepository
    {
        private readonly TestTask2Context _dbContext;

        public BanksRepository(TestTask2Context dbContext)
        {
            _dbContext = dbContext;
        }

        //Если в БД существует банк с таким название вернуть его, иначе - создать
        public async Task<Bank?> AddOrGetExistingAsync(Bank bank)
        {
            var existingBank = await _dbContext.Banks.AsNoTracking()
                .FirstOrDefaultAsync(b => b.BankName == bank.BankName);
            if (existingBank != null) return existingBank;

            var createdBank = await _dbContext.Banks.AddAsync(bank);
            if (createdBank == null) return null;

            await _dbContext.SaveChangesAsync();
            return createdBank.Entity;
        }
    }
}
