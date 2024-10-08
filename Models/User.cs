﻿using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RoleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AdditionalInfo> AdditionalInfos { get; set; } = new List<AdditionalInfo>();

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual ICollection<HodCourseAssignTeacher> HodCourseAssignTeacherAssignByNavigations { get; set; } = new List<HodCourseAssignTeacher>();

    public virtual ICollection<HodCourseAssignTeacher> HodCourseAssignTeacherAssignToNavigations { get; set; } = new List<HodCourseAssignTeacher>();

    public virtual ICollection<HodInstitute> HodInstitutes { get; set; } = new List<HodInstitute>();

    public virtual Role Role { get; set; } = null!;
}
