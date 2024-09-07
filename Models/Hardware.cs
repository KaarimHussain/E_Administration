using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Hardware
{
    public int HardId { get; set; }

    public string? Processor { get; set; }

    public int Ram { get; set; }

    public int? StorageCapacity { get; set; }

    public string? OperatingSystem { get; set; }

    public string? HardwareName { get; set; }

    public virtual ICollection<Pc> Pcs { get; set; } = new List<Pc>();
}
