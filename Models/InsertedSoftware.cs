using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class InsertedSoftware
{
    public int InstalledId { get; set; }

    public int PcId { get; set; }

    public int SoftId { get; set; }

    public virtual Pc Pc { get; set; } = null!;

    public virtual Software Soft { get; set; } = null!;
}
