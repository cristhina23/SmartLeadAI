using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

    [Required (ErrorMessage = "Password is required.")]
    [StringLength(255, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 255 characters.")]
    public string PasswordHash { get; set; } = string.Empty; 

    [Required (ErrorMessage = "Role is required.")]
    public string Role { get; set; } = "Employee"; 

    [Required (ErrorMessage = "IsActive is required.")]
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public Company? Company { get; set; } 
    public ICollection<Interaction>? Interactions { get; set; } = new List<Interaction>();
}