using System;
using System.Collections.Generic;

namespace DataAccess.Models;

//Запись по счету
public partial class BalanceSheetRecord
{
    public int RecordId { get; set; }

    public int AccountId { get; set; }

    public decimal OpeningBalancesActive { get; set; }

    public decimal OpeningBalancesPassive { get; set; }

    public decimal TurnoversDebit { get; set; }

    public decimal TurnoversCredit { get; set; }

    public decimal ClosingBalancesActive { get; set; }

    public decimal ClosingBalancesPassive { get; set; }

    public virtual Account Account { get; set; } = null!;
}
