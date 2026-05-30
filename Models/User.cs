using System.ComponentModel.DataAnnotations;

public class User
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
    [StringLength(255, MinimumLength = 8)]
    public string? Password { get; set; }

    public enum UserRole
    {
        Admin,
        User
    }

    [Required]
    public UserRole Role { get; set; }

    [Required]
    public bool IsActive { get; set; }
}