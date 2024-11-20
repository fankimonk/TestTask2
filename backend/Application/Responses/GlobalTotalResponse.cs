namespace Application.Responses
{
    public record GlobalTotalResponse
    (
        decimal OpeningActive,
        decimal OpeningPassive,
        decimal TurnoverDebit,
        decimal TurnoverCredit,
        decimal ClosingActive,
        decimal ClosingPassive
    );
}
