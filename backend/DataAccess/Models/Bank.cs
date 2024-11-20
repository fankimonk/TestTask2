using System;
using System.Collections.Generic;

namespace DataAccess.Models;

//Банк
public partial class Bank
{
    public int BankId { get; set; }

    public string BankName { get; set; } = null!;

    public virtual ICollection<SheetFile> SheetFiles { get; set; } = new List<SheetFile>();
}
