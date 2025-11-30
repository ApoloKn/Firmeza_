namespace Firmeza.Api.DTOs;

/// <summary>
/// Data Transfer Object for Customer entity
/// </summary>
public class CustomerDto
{
    public int Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string FullName { get; set; } = string.Empty;
    
    public string DocumentNumber { get; set; } = string.Empty;
    
    public string? DocumentType { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Address { get; set; }
    
    public string? City { get; set; }
    
    public string? CustomerType { get; set; }
    
    public bool IsActive { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}
