using System;
using System.Collections.Generic;

namespace DataAccess.Models;

//Файл таблицы
public partial class SheetFile
{
    public int FileId { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime PublicationDate { get; set; }

    public int BankId { get; set; }

    public int PeriodId { get; set; }

    public virtual Bank Bank { get; set; } = null!;

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<GlobalTotal> GlobalTotals { get; set; } = new List<GlobalTotal>();

    public virtual Period Period { get; set; } = null!;
}
