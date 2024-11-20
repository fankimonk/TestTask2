namespace Application.Helpers
{
    //Данные для загрузки файла клиентом
    public class FileData
    {
        public MemoryStream MemoryStream { get; private set; }
        public string ContentType { get; private set; }
        public string FileName { get; private set; }

        public FileData(MemoryStream memoryStream, string contentType, string fileName)
        {
            MemoryStream = memoryStream;
            ContentType = contentType;
            FileName = fileName;
        }
    }
}
