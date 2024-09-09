using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Administration.Models;

public partial class HodCourseAssignTeacher
{
    [Key]
    public int AssignId { get; set; }

    public int AssignBy { get; set; }

    public int AssignTo { get; set; }

    public int CourseId { get; set; }

    public DateTime AssignAt { get; set; }

    public virtual User AssignByNavigation { get; set; } = null!;

    public virtual User AssignToNavigation { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;
}
