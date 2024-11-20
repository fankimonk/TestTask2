namespace Application.Responses
{
    public record FilesResponse
    (
        int FileId,
        string FileName,
        DateTime PublicationDate,
        BankResponse bank,
        PeriodResponse period
    );
}
