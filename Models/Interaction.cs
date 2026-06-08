using System;
using System.ComponentModel.DataAnnotations;

public class Interaction
{
    [Key]
    public int Id { get; set; }

    [Required (ErrorMessage = "Customer ID is required.")]
    public int CustomerId { get; set; }

    [Required (ErrorMessage = "User ID is required.")]
    public int UserId { get; set; } 

    [Required (ErrorMessage = "Interaction type is required.")]
    [StringLength(100, ErrorMessage = "Interaction type cannot exceed 100 characters.")]
    public string Type { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
    public string Notes { get; set; } = string.Empty;

    [Required (ErrorMessage = "Interaction date is required.")]
    public DateTime InteractionDate { get; set; } = DateTime.UtcNow;

    public DateTime? NextFollowUp { get; set; }

    // Navigation Properties
    public Customer Customer { get; set; } = null!;
    public User User { get; set; } = null!;
}