using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Floor
{
    public int FloorId { get; set; }

    public int InstituteId { get; set; }

    public int FloorName { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Institute Institute { get; set; } = null!;

    public virtual ICollection<Lab> Labs { get; set; } = new List<Lab>();
}
