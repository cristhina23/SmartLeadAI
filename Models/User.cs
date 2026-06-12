using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SmartLeadAI.Models;

public class User
{
[Key]
public int Id { get; set; }


[Required]
public int CompanyId { get; set; }

[Required]
[StringLength(255)]
public string FullName { get; set; } = string.Empty;

[Required]
[EmailAddress]
public string Email { get; set; } = string.Empty;

[Required]
[StringLength(255, MinimumLength = 8)]
public string PasswordHash { get; set; } = string.Empty;

[Required]
public string Role { get; set; } = "Employee";

[Required]
public bool IsActive { get; set; } = false;

public Guid? ActivationToken { get; set; }

public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

public DateTime? ActivatedAt { get; set; }

// Navigation Properties
public Company Company { get; set; } = null!;
public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();


}
