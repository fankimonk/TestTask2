using System;
using System.Collections.Generic;

namespace DataAccess.Models;

//Период
public partial class Period
{
    public int PeriodId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual ICollection<SheetFile> SheetFiles { get; set; } = new List<SheetFile>();
}
