using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required (ErrorMessage = "Company ID is required.")]
    public int CompanyId { get; set; } 

    [Required (ErrorMessage = "Full name is required.")]
    [StringLength(255, ErrorMessage = "Full name cannot exceed 255 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required (ErrorMessage = "Email is required.")]
    [EmailAddress (ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [Required (ErrorMessage = "Phone number is required.")]
    [Phone (ErrorMessage = "Invalid phone number format.")]
    public string Phone { get; set; } = string.Empty;

    [Required (ErrorMessage = "Interest type is required.")]
    [StringLength(100, ErrorMessage = "Interest type cannot exceed 100 characters.")]
    public string InterestType { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
    public string Notes { get; set; } = string.Empty;

    // Navigation Properties
    public Company Company { get; set; } = null!;
    public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
}