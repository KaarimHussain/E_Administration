using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public DateOnly CourseDuration { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int InstituteId { get; set; }

    public virtual ICollection<HodCourseAssignTeacher> HodCourseAssignTeachers { get; set; } = new List<HodCourseAssignTeacher>();

    public virtual Institute Institute { get; set; } = null!;
}
