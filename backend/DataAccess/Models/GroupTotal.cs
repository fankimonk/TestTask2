using System;
using System.Collections.Generic;

namespace DataAccess.Models;

//Итоги по группе
public partial class GroupTotal
{
    public int RecordId { get; set; }

    public int GroupId { get; set; }

    public decimal OpeningBalancesActive { get; set; }

    public decimal OpeningBalancesPassive { get; set; }

    public decimal TurnoversDebit { get; set; }

    public decimal TurnoversCredit { get; set; }

    public decimal ClosingBalancesActive { get; set; }

    public decimal ClosingBalancesPassive { get; set; }

    public virtual Group Group { get; set; } = null!;
}
