using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Software
{
    public int SoftId { get; set; }

    public string SoftwareName { get; set; } = null!;

    public virtual ICollection<Pc> Pcs { get; set; } = new List<Pc>();
}
