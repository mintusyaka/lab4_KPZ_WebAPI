using System;
using System.Collections.Generic;

namespace lab4_KPZ.Models;

public partial class TopDonate
{
    public int PlayerId { get; set; }

    public string Nickname { get; set; } = null!;

    public double DonatedFundsCount { get; set; }

    public int? DonationsCount { get; set; }

    public double? MaxDonate { get; set; }
}
