using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Floor
{
    public int FloorId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? FloorName { get; set; }

    public virtual ICollection<Lab> Labs { get; set; } = new List<Lab>();
}
