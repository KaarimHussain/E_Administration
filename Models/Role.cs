using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public int InstituteId { get; set; }

    public virtual Institute Institute { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
