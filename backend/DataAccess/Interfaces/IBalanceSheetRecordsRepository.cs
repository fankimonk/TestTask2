using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IBalanceSheetRecordsRepository
    {
        Task<BalanceSheetRecord?> AddAsync(BalanceSheetRecord record);
    }
}
