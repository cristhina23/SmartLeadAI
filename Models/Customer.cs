using System.ComponentModel.DataAnnotations;

public class Customer
{
    [Required]
    [Key]
    
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string? FullName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [Phone]
    public string? PhoneNumber { get; set; }

    [Required]
    [StringLength(100)]
    public string? InterestType { get; set; }

    [Required]
    [StringLength(50)]
    public string? Status { get; set; }

    [Required]
    [StringLength(1000)]
    public string? Notes { get; set; }
}