using System.ComponentModel.DataAnnotations;

public class Interaction
{
    [Required]
    [Key]
    public int Id { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    [StringLength(255)]
    public string? EmployeeName { get; set; }

    [Required]
    [StringLength(100)]
    public string? Type { get; set; }

    [Required]
    [StringLength(1000)]
    public string? Notes { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public string? InteractionDate { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public string? NextFollowUp { get; set; }
}