using Application.Responses;

namespace Application.Interfaces
{
    public interface ITableService
    {
        Task<TableResponse?> GetTable(int fileId);
    }
}