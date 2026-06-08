using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Company
{
    [Key]
    public int Id { get; set; }

    [Required (ErrorMessage = "Company name is required.")]
    [StringLength(255, ErrorMessage = "Company name cannot exceed 255 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required (ErrorMessage = "Phone number is required.")]
    [Phone (ErrorMessage = "Invalid phone number format.")]
    public string Phone { get; set; } = string.Empty;

    [Required (ErrorMessage = "Email is required.")]
    [EmailAddress (ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [Required (ErrorMessage = "Creation date is required.")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
}