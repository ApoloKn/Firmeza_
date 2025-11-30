using System.ComponentModel.DataAnnotations;

namespace Firmeza.Api.DTOs;

/// <summary>
/// Data Transfer Object for Product entity
/// </summary>
public class ProductDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public string SKU { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    public int Stock { get; set; }
    
    public string? Unit { get; set; }
    
    public string? Category { get; set; }
    
    public bool IsActive { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}
