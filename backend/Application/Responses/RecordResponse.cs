namespace Application.Responses
{
    public record RecordResponse
    (
        string AccountNumber,
        decimal OpeningActive,
        decimal OpeningPassive,
        decimal TurnoverDebit,
        decimal TurnoverCredit,
        decimal ClosingActive,
        decimal ClosingPassive
    );
}
