using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Complaint
{
    public int ComplaintsId { get; set; }

    public string ComplaintsResponse { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
