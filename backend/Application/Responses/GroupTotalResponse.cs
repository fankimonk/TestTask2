namespace Application.Responses
{
    public record GroupTotalResponse
    (
        decimal OpeningActive,
        decimal OpeningPassive,
        decimal TurnoverDebit,
        decimal TurnoverCredit,
        decimal ClosingActive,
        decimal ClosingPassive
    );
}
