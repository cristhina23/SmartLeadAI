using System;
using System.ComponentModel.DataAnnotations;
namespace SmartLeadAI.Models;

public class Interaction
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int UserId { get; set; } 

    [Required]
    [StringLength(100)]
    public string Type { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Notes { get; set; } = string.Empty;

    [Required]
    public DateTime InteractionDate { get; set; } = DateTime.UtcNow;

    public DateTime? NextFollowUp { get; set; }

    // Navigation Properties
    public Customer Customer { get; set; } = null!;
    public User User { get; set; } = null!;
}