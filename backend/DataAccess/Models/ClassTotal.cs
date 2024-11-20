using System;
using System.Collections.Generic;

namespace DataAccess.Models;

//Итоги по классу
public partial class ClassTotal
{
    public int RecordId { get; set; }

    public int ClassId { get; set; }

    public decimal OpeningBalancesActive { get; set; }

    public decimal OpeningBalancesPassive { get; set; }

    public decimal TurnoversDebit { get; set; }

    public decimal TurnoversCredit { get; set; }

    public decimal ClosingBalancesActive { get; set; }

    public decimal ClosingBalancesPassive { get; set; }

    public virtual Class Class { get; set; } = null!;
}
