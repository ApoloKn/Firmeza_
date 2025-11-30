using System.ComponentModel.DataAnnotations;

namespace Firmeza.Data.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El documento de identidad es requerido")]
    [StringLength(20, ErrorMessage = "El documento no puede exceder 20 caracteres")]
    public string DocumentNumber { get; set; } = string.Empty;

    [StringLength(50)]
    public string? DocumentType { get; set; } // e.g., "DNI", "RUC", "Pasaporte"

    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "El formato del teléfono no es válido")]
    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(50)]
    public string? CustomerType { get; set; } // e.g., "Retail", "Wholesale", "Corporate"

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    // Computed property
    public string FullName => $"{FirstName} {LastName}";
}
