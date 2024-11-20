using System;
using System.Collections.Generic;

namespace DataAccess.Models;

//Счет
public partial class Account
{
    public int AccountId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public int GroupId { get; set; }

    public virtual ICollection<BalanceSheetRecord> BalanceSheetRecords { get; set; } = new List<BalanceSheetRecord>();

    public virtual Group Group { get; set; } = null!;
}
