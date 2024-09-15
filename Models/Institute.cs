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

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Hardware> Hardwares { get; set; } = new List<Hardware>();

    public virtual ICollection<HodInstitute> HodInstitutes { get; set; } = new List<HodInstitute>();

    public virtual ICollection<Lab> Labs { get; set; } = new List<Lab>();

    public virtual ICollection<Pc> Pcs { get; set; } = new List<Pc>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<Software> Softwares { get; set; } = new List<Software>();
}
