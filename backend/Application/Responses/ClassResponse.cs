namespace Application.Responses
{
    public record ClassResponse
    (
        string ClassNumber,
        string ClassName,
        ClassTotalResponse ClassTotal,
        List<GroupResponse> Groups
    );
}
