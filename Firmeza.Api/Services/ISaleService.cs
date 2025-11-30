using Firmeza.Api.DTOs;
using Firmeza.Data.Models;

namespace Firmeza.Api.Services;

public interface ISaleService
{
    Task<IEnumerable<Sale>> GetAllSalesAsync(int page, int pageSize);
    Task<Sale?> GetSaleByIdAsync(int id);
    Task<IEnumerable<Sale>> GetSalesByCustomerIdAsync(int customerId);
    Task<Sale> CreateSaleAsync(CreateSaleDto createSaleDto);
    Task UpdateSaleAsync(int id, UpdateSaleDto updateSaleDto);
    Task DeleteSaleAsync(int id);
}
