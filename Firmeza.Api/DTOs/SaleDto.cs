namespace Firmeza.Api.DTOs;

/// <summary>
/// Data Transfer Object for Sale entity
/// </summary>
public class SaleDto
{
    public int Id { get; set; }
    
    public int CustomerId { get; set; }
    
    public string CustomerName { get; set; } = string.Empty;
    
    public DateTime SaleDate { get; set; }
    
    public decimal SubTotal { get; set; }
    
    public decimal Tax { get; set; }
    
    public decimal Discount { get; set; }
    
    public decimal Total { get; set; }
    
    public string Status { get; set; } = string.Empty;
    
    public string? PaymentMethod { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public List<SaleDetailDto> SaleDetails { get; set; } = new();
}
