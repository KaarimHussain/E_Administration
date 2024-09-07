using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Lab
{
    public int LabId { get; set; }

    public int FloorId { get; set; }

    public string LabName { get; set; } = null!;

    public virtual Floor Floor { get; set; } = null!;

    public virtual ICollection<Pc> Pcs { get; set; } = new List<Pc>();
}
