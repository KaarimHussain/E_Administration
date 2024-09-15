using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Pc
{
    public int PcId { get; set; }

    public int HardId { get; set; }

    public int LabId { get; set; }

    public string PcName { get; set; } = null!;

    public DateTime PurchasedAt { get; set; }

    public int InstituteId { get; set; }

    public virtual Hardware Hard { get; set; } = null!;

    public virtual ICollection<InsertedSoftware> InsertedSoftwares { get; set; } = new List<InsertedSoftware>();

    public virtual Institute Institute { get; set; } = null!;

    public virtual Lab Lab { get; set; } = null!;
}
