namespace Application.Responses
{
    public record ClassTotalResponse
    (
        decimal OpeningActive,
        decimal OpeningPassive,
        decimal TurnoverDebit,
        decimal TurnoverCredit,
        decimal ClosingActive,
        decimal ClosingPassive
    );
}
