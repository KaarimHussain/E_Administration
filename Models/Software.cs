using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Software
{
    public int SoftId { get; set; }

    public string SoftwareName { get; set; } = null!;

    public int InstituteId { get; set; }

    public DateOnly ExpireDate { get; set; }

    public DateOnly PurchasedDate { get; set; }

    public virtual ICollection<InsertedSoftware> InsertedSoftwares { get; set; } = new List<InsertedSoftware>();

    public virtual Institute Institute { get; set; } = null!;
}
