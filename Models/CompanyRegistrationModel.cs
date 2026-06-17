using System.ComponentModel.DataAnnotations;

namespace SmartLeadAI.Models;

public class CompanyRegistrationModel
{
    [Required(ErrorMessage = "Company Name is required")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string CompanyEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company Phone is required")]
    public string CompanyPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Administrator name is required")]
    public string AdminName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Admin Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string AdminEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 255 characters.")]
    public string AdminPassword { get; set; } = string.Empty;
}
