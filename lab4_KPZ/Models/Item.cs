using System;
using System.Collections.Generic;

namespace lab4_KPZ.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ItemType { get; set; } = null!;

    public string Rarity { get; set; } = null!;

    public int Score { get; set; }
}
