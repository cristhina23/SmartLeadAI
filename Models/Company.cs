using System.ComponentModel.DataAnnotations;

public class Company
{
    [Required]
    [Key]
    
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string? Name { get; set; }


    [Required]
    [Phone]
    
    public string? PhoneNumber { get; set; }


    [Required]
    [EmailAddress]
    
    public string? Email { get; set; }

    [Required]
    public int OwnerUserId { get; set; }

    
    [Required]
    [DataType(DataType.DateTime)]
    
    public string? CreatedAt { get; set; }
}