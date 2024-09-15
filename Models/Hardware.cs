using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Hardware
{
    public int HardId { get; set; }

    public string HardwareName { get; set; } = null!;

    public string? Processor { get; set; }

    public int Ram { get; set; }

    public string OsName { get; set; } = null!;

    public int? StorageCapacity { get; set; }

    public int InstituteId { get; set; }

    public virtual Institute Institute { get; set; } = null!;

    public virtual ICollection<Pc> Pcs { get; set; } = new List<Pc>();
}
