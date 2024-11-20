using System;
using System.Collections.Generic;

namespace DataAccess.Models;

//Класс
public partial class Class
{
    public int ClassId { get; set; }

    public string ClassNumber { get; set; } = null!;

    public string ClassName { get; set; } = null!;

    public int FileId { get; set; }

    public virtual ICollection<ClassTotal> ClassTotals { get; set; } = new List<ClassTotal>();

    public virtual SheetFile File { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
