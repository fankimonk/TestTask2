using Application.Enums;

namespace Application.Interfaces
{
    public interface IExcelService
    {
        Task<FilesUploadResult> Upload(string filePath);
    }
}