using Application.Enums;
using Application.Helpers;
using Application.Interfaces;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    //Сервис для работы с файлами
    public class FilesService : IFilesService
    {
        //Путь загрузки файла
        private readonly string _uploadDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

        private readonly IFilesRepository _filesRepository;

        public FilesService(IFilesRepository filesRepository)
        {
            _filesRepository = filesRepository;
        }

        public async Task<FilesCreateResult> CreateFile(IFormFile file)
        {
            if (file.Length == 0) return new FilesCreateResult(CreateResult.Failed, null);

            var filePath = Path.Combine(_uploadDirectoryPath, file.FileName);

            try
            {
                using (var stream = File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception)
            {
                return new FilesCreateResult(CreateResult.Failed, null);
            }
            return new FilesCreateResult(CreateResult.Created, filePath);
        }

        public async Task<FilesDeleteResult> DeleteFile(string fileName)
        {
            var file = await _filesRepository.GetByNameAsync(fileName);
            if (file == null) return FilesDeleteResult.NotFound;

            return await DeleteFile(file.FileId);
        }

        public async Task<FilesDeleteResult> DeleteFile(int fileId)
        {
            var file = await _filesRepository.GetByIdAsync(fileId);
            if (file == null) return FilesDeleteResult.NotFound;

            var deletedId = await _filesRepository.DeleteAsync(fileId);
            if (deletedId == null) return FilesDeleteResult.Failed;

            try
            {
                File.Delete(Path.Combine(_uploadDirectoryPath, file.FileName));
            }
            catch (Exception)
            {
                return FilesDeleteResult.Failed;
            }

            return FilesDeleteResult.Deleted;
        }

        //Создание данных для скачивания файла для клиента
        public async Task<FileData?> DownloadFile(int fileId)
        {
            var file = await _filesRepository.GetByIdAsync(fileId);
            if (file == null) return null;

            var filePath = Path.Combine(_uploadDirectoryPath, file.FileName);
            if (!File.Exists(filePath))
            {
                return null;
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            var contentType = "application/octet-stream";
            return new FileData(memory, contentType, file.FileName);
        }
    }
}
