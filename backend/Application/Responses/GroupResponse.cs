namespace Application.Responses
{
    public record GroupResponse
    (
        string GroupNumber,
        GroupTotalResponse GroupTotal,
        List<RecordResponse> Records
    );
}
