using System;
using System.Collections.Generic;

namespace lab4_KPZ.Models;

public partial class CharityFund
{
    public int CharityFundId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string CardNumber { get; set; } = null!;

    public string TelephoneNumber { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
