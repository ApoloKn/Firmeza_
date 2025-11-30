using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Firmeza.Data.Data;
using Firmeza.Data.Models;

namespace Firmeza.Web.Pages.Sales;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Sale> Sales { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Sales != null)
        {
            Sales = await _context.Sales
                .Include(s => s.Customer)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }
    }
}
