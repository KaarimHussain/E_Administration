using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Pc
{
    public int PcId { get; set; }

    public int SoftId { get; set; }

    public int HardId { get; set; }

    public int LabId { get; set; }

    public string PcName { get; set; } = null!;

    public DateTime PurchasedAt { get; set; }

    public virtual Hardware Hard { get; set; } = null!;

    public virtual Lab Lab { get; set; } = null!;

    public virtual Software Soft { get; set; } = null!;
}
