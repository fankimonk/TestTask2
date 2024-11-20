using System;
using System.Collections.Generic;

namespace DataAccess.Models;

//Итоги по файлу
public partial class GlobalTotal
{
    public int RecordId { get; set; }

    public int FileId { get; set; }

    public decimal OpeningBalancesActive { get; set; }

    public decimal OpeningBalancesPassive { get; set; }

    public decimal TurnoversDebit { get; set; }

    public decimal TurnoversCredit { get; set; }

    public decimal ClosingBalancesActive { get; set; }

    public decimal ClosingBalancesPassive { get; set; }

    public virtual SheetFile File { get; set; } = null!;
}
