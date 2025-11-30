using System.ComponentModel.DataAnnotations;

namespace Firmeza.Api.DTOs;

/// <summary>
/// DTO for updating an existing Sale
/// </summary>
public class UpdateSaleDto
{
    [Required(ErrorMessage = "Customer ID is required")]
    public int CustomerId { get; set; }
    
    public DateTime SaleDate { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Tax must be greater than or equal to 0")]
    public decimal Tax { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Discount must be greater than or equal to 0")]
    public decimal Discount { get; set; }
    
    [StringLength(50)]
    public string Status { get; set; } = "Completed";
    
    [StringLength(50)]
    public string? PaymentMethod { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
}
