using Application.Enums;
using Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IFilesService
    {
        Task<FilesCreateResult> CreateFile(IFormFile file);
        Task<FilesDeleteResult> DeleteFile(int fileId);
        Task<FilesDeleteResult> DeleteFile(string fileName);
        Task<FileData?> DownloadFile(int fileId);
    }
}