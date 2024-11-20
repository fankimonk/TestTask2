using API.Contracts;
using Application.Responses;
using Application.Enums;
using Application.Interfaces;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //Контроллер для получения запросов клиента, связанных с файлами
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly IFilesService _filesService;
        private readonly IExcelService _excelService;
        private readonly ITableService _tableService;
        private readonly IFilesRepository _filesRepository;

        public FilesController(
            IFilesService filesService,
            IExcelService excelService,
            ITableService tableService,
            IFilesRepository filesRepository)
        {
            _filesService = filesService;
            _excelService = excelService;
            _tableService = tableService;
            _filesRepository = filesRepository;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] FilesQuery query)
        {
            var files = await _filesRepository.GetAllAsync(query.PageNumber, query.PageSize);
            var response = files.Select(f => new FilesResponse(
                f.FileId,
                f.FileName,
                f.PublicationDate,
                new BankResponse(f.Bank.BankName),
                new PeriodResponse(f.Period.StartDate, f.Period.EndDate)
            ));
            return Ok(response);
        }

        [HttpGet("gettable/{fileId}")]
        public async Task<IActionResult> GetTable([FromRoute] int fileId)
        {
            var tableResponse = await _tableService.GetTable(fileId);
            if (tableResponse == null)
                return StatusCode(500, "Failed to get table");

            return Ok(tableResponse);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile fileToUpload)
        {
            var createResult = await _filesService.CreateFile(fileToUpload);
            if (createResult.CreateResult == CreateResult.Failed || createResult.FilePath == null)
                return StatusCode(500, new { resultMessage = "Failed to create file" } );

            var uploadResult = await _excelService.Upload(createResult.FilePath);
            if (uploadResult == FilesUploadResult.Failed)
            {
                await _filesService.DeleteFile(Path.GetFileName(createResult.FilePath));
                return StatusCode(500, new { resultMessage = "Failed to upload file" });
            }
            else if (uploadResult == FilesUploadResult.FileExists)
                return StatusCode(500, new { resultMessage = "File already uploaded" });

            return Ok(new { resultMessage = "File was uploaded successfuly" } );
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleteResult = await _filesService.DeleteFile(id);
            if (deleteResult == FilesDeleteResult.NotFound)
                return NotFound();
            else if (deleteResult == FilesDeleteResult.Failed)
                return StatusCode(500, "Failed to delete file");

            return NoContent();
        }

        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> DownloadFile([FromRoute] int fileId)
        {
            var fileData = await _filesService.DownloadFile(fileId);
            if (fileData == null) return NotFound();

            return File(fileData.MemoryStream, fileData.ContentType, fileData.FileName);
        }
    }
}
