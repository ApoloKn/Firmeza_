using System.ComponentModel.DataAnnotations;

namespace Firmeza.Api.DTOs;

/// <summary>
/// DTO for creating a new Customer
/// </summary>
public class CreateCustomerDto
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Document number is required")]
    [StringLength(20, ErrorMessage = "Document cannot exceed 20 characters")]
    public string DocumentNumber { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string? DocumentType { get; set; }
    
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100)]
    public string? Email { get; set; }
    
    [Phone(ErrorMessage = "Invalid phone format")]
    [StringLength(20)]
    public string? Phone { get; set; }
    
    [StringLength(200)]
    public string? Address { get; set; }
    
    [StringLength(100)]
    public string? City { get; set; }
    
    [StringLength(50)]
    public string? CustomerType { get; set; }
    
    public bool IsActive { get; set; } = true;
}
