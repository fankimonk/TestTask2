namespace Application.Responses
{
    public record TableResponse
    (
        List<ClassResponse> Classes,
        GlobalTotalResponse GlobalTotal
    );
}
