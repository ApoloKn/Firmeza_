using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firmeza.Data.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del producto es requerido")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "El SKU es requerido")]
    [StringLength(50, ErrorMessage = "El SKU no puede exceder 50 caracteres")]
    public string SKU { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es requerido")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "La cantidad en stock es requerida")]
    [Range(0, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor o igual a 0")]
    public int Stock { get; set; }

    [StringLength(50)]
    public string? Unit { get; set; } // e.g., "unidad", "metro", "kg", etc.

    [StringLength(100)]
    public string? Category { get; set; } // e.g., "Materiales de construcción", "Herramientas", etc.

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
