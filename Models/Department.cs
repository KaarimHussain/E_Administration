using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public int InstituteId { get; set; }

    public virtual ICollection<HodInstitute> HodInstitutes { get; set; } = new List<HodInstitute>();

    public virtual Institute Institute { get; set; } = null!;
}
