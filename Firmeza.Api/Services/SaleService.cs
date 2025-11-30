using AutoMapper;
using Firmeza.Api.DTOs;
using Firmeza.Data.Data;
using Firmeza.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Api.Services;

public class SaleService : ISaleService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SaleService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Sale>> GetAllSalesAsync(int page, int pageSize)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
            .OrderByDescending(s => s.SaleDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Sale?> GetSaleByIdAsync(int id)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Sale>> GetSalesByCustomerIdAsync(int customerId)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
            .Where(s => s.CustomerId == customerId)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<Sale> CreateSaleAsync(CreateSaleDto createSaleDto)
    {
        // Verify customer exists
        var customerExists = await _context.Customers.AnyAsync(c => c.Id == createSaleDto.CustomerId);
        if (!customerExists)
        {
            throw new ArgumentException("Customer not found");
        }

        // Verify all products exist and have sufficient stock
        foreach (var detailDto in createSaleDto.SaleDetails)
        {
            var product = await _context.Products.FindAsync(detailDto.ProductId);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {detailDto.ProductId} not found");
            }
            if (product.Stock < detailDto.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
            }
        }

        // Create the sale
        var sale = _mapper.Map<Sale>(createSaleDto);
        sale.CreatedAt = DateTime.UtcNow;

        // Create sale details and calculate totals
        decimal subTotal = 0;
        foreach (var detailDto in createSaleDto.SaleDetails)
        {
            var saleDetail = _mapper.Map<SaleDetail>(detailDto);
            saleDetail.SubTotal = (saleDetail.UnitPrice * saleDetail.Quantity) - saleDetail.Discount;
            subTotal += saleDetail.SubTotal;
            
            sale.SaleDetails.Add(saleDetail);

            // Update product stock
            var product = await _context.Products.FindAsync(detailDto.ProductId);
            if (product != null)
            {
                product.Stock -= detailDto.Quantity;
                product.UpdatedAt = DateTime.UtcNow;
            }
        }

        // Calculate sale totals
        sale.SubTotal = subTotal;
        sale.Total = subTotal + sale.Tax - sale.Discount;

        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();

        // Reload with navigation properties
        return await GetSaleByIdAsync(sale.Id) ?? sale;
    }

    public async Task UpdateSaleAsync(int id, UpdateSaleDto updateSaleDto)
    {
        var sale = await _context.Sales
            .Include(s => s.SaleDetails)
            .FirstOrDefaultAsync(s => s.Id == id);
            
        if (sale == null)
        {
            throw new KeyNotFoundException("Sale not found");
        }

        // Verify customer exists
        var customerExists = await _context.Customers.AnyAsync(c => c.Id == updateSaleDto.CustomerId);
        if (!customerExists)
        {
            throw new ArgumentException("Customer not found");
        }

        // Revert stock for existing details
        foreach (var existingDetail in sale.SaleDetails)
        {
            var product = await _context.Products.FindAsync(existingDetail.ProductId);
            if (product != null)
            {
                product.Stock += existingDetail.Quantity;
            }
        }

        // Clear existing details to replace them (simplest approach for update)
        // Note: In a real app, you might want to diff the details to avoid deleting/re-adding
        _context.SaleDetails.RemoveRange(sale.SaleDetails);
        sale.SaleDetails.Clear();

        // Map basic properties
        _mapper.Map(updateSaleDto, sale);
        
        // Process new details and validate stock
        decimal subTotal = 0;
        foreach (var detailDto in updateSaleDto.SaleDetails)
        {
            var product = await _context.Products.FindAsync(detailDto.ProductId);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {detailDto.ProductId} not found");
            }
            if (product.Stock < detailDto.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
            }

            var saleDetail = _mapper.Map<SaleDetail>(detailDto);
            saleDetail.SubTotal = (saleDetail.UnitPrice * saleDetail.Quantity) - saleDetail.Discount;
            subTotal += saleDetail.SubTotal;
            
            sale.SaleDetails.Add(saleDetail);

            // Deduct new stock
            product.Stock -= detailDto.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
        }

        // Recalculate total
        sale.SubTotal = subTotal;
        sale.Total = subTotal + sale.Tax - sale.Discount;
        sale.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteSaleAsync(int id)
    {
        var sale = await _context.Sales
            .Include(s => s.SaleDetails)
            .FirstOrDefaultAsync(s => s.Id == id);
            
        if (sale == null)
        {
            throw new KeyNotFoundException("Sale not found");
        }

        // Restore product stock
        foreach (var detail in sale.SaleDetails)
        {
            var product = await _context.Products.FindAsync(detail.ProductId);
            if (product != null)
            {
                product.Stock += detail.Quantity;
                product.UpdatedAt = DateTime.UtcNow;
            }
        }

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync();
    }
}
