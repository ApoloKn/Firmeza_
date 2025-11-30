namespace Firmeza.Api.DTOs;

/// <summary>
/// DTO for Sale Detail (nested in Sale)
/// </summary>
public class SaleDetailDto
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    
    public string ProductName { get; set; } = string.Empty;
    
    public string ProductSKU { get; set; } = string.Empty;
    
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public decimal Discount { get; set; }
    
    public decimal SubTotal { get; set; }
}
