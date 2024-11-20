namespace API.Contracts
{
    //Объект запроса для получения файлов с определенной страницы
    public record FilesQuery
    (
        int PageNumber = 1,
        int PageSize = 20
    );
}
