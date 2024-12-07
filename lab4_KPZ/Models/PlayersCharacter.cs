using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lab4_KPZ.Models;

public partial class PlayersCharacter
{
    public int PlayerId { get; set; }

    public int CharacterId { get; set; }

    [JsonIgnore]
    public virtual Character Character { get; set; } = null!;

	[JsonIgnore]
	public virtual Player Player { get; set; } = null!;
}
