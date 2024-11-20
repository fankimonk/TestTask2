namespace Application.Enums
{
    public enum CreateResult
    {
        Created = 0,
        Failed
    }

    public class FilesCreateResult
    {
        public readonly CreateResult CreateResult;
        public readonly string? FilePath;

        public FilesCreateResult(CreateResult createResult, string? filePath)
        {
            CreateResult = createResult;
            FilePath = filePath;
        }
    }
}
