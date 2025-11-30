using System.ComponentModel.DataAnnotations;

namespace Firmeza.Api.DTOs;

/// <summary>
/// DTO for creating a new Sale
/// </summary>
public class CreateSaleDto
{
    [Required(ErrorMessage = "Customer ID is required")]
    public int CustomerId { get; set; }
    
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    
    [Range(0, double.MaxValue, ErrorMessage = "Tax must be greater than or equal to 0")]
    public decimal Tax { get; set; } = 0;
    
    [Range(0, double.MaxValue, ErrorMessage = "Discount must be greater than or equal to 0")]
    public decimal Discount { get; set; } = 0;
    
    [StringLength(50)]
    public string Status { get; set; } = "Completed";
    
    [StringLength(50)]
    public string? PaymentMethod { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    [Required(ErrorMessage = "At least one sale item is required")]
    [MinLength(1, ErrorMessage = "At least one sale item is required")]
    public List<CreateSaleDetailDto> SaleDetails { get; set; } = new();
}
