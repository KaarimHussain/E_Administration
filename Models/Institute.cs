using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Institute
{
    public int InstituteId { get; set; }

    public string InstituteName { get; set; } = null!;

    public string? InstituteDescription { get; set; }

    public string? InstituteAddress { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Floor> Floors { get; set; } = new List<Floor>();
}
