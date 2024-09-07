using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Administration.Models;

public partial class User
{
    public int Id { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string? Username { get; set; } = null!;
    [Required]
    public string? Password { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string? Email { get; set; } = null!;

    public int? RoleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AdditionalInfo> AdditionalInfos { get; set; } = new List<AdditionalInfo>();

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual Role? Role { get; set; } = null!;
}
