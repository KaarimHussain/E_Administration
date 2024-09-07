using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class AdditionalInfo
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ProfilePicture { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public virtual User User { get; set; } = null!;
}
