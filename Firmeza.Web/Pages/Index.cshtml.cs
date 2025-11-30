using Firmeza.Data.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _context;

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public int TotalProducts { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalSales { get; set; }
    public decimal TotalRevenue { get; set; }

    public async Task OnGetAsync()
    {
        TotalProducts = await _context.Products.CountAsync();
        TotalCustomers = await _context.Customers.CountAsync();
        TotalSales = await _context.Sales.CountAsync();
        TotalRevenue = await _context.Sales.SumAsync(s => s.Total);
    }
}
