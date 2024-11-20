using System;
using System.Collections.Generic;

namespace DataAccess.Models;

//Группа
public partial class Group
{
    public int GroupId { get; set; }

    public string GroupNumber { get; set; } = null!;

    public int ClassId { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<GroupTotal> GroupTotals { get; set; } = new List<GroupTotal>();
}
