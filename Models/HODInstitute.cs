using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class HodInstitute
{
    public int HodId { get; set; }

    public int InstituteId { get; set; }

    public int UserId { get; set; }

    public int DepartmentId { get; set; }

    public DateOnly CreatedAt { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Institute Institute { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
