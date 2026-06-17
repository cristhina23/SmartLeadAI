using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SmartLeadAI.Models;

public class User
{
[Key]
public int Id { get; set; }


    [Required (ErrorMessage = "CompanyId is required.")]
    public int CompanyId { get; set; }

    [Required (ErrorMessage = "Full Name is required.")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Full Name must be between 2 and 255 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required (ErrorMessage = "Email is required.")]
    [EmailAddress (ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty; 

    [Required (ErrorMessage = "Role is required.")]
    public string Role { get; set; } = "Employee"; 

    [Required (ErrorMessage = "IsActive is required.")]
    public bool IsActive { get; set; } = true;

    public Guid? ActivationToken { get; set; }

    [Required(ErrorMessage = "Creation date is required.")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ActivatedAt { get; set; }

    // Navigation Properties
    public Company? Company { get; set; } 
    public ICollection<Interaction>? Interactions { get; set; } = new List<Interaction>();
}
