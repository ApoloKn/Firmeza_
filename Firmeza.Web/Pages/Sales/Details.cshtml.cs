using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Firmeza.Data.Data;
using Firmeza.Data.Models;

namespace Firmeza.Web.Pages.Sales;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Sale Sale { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Sales == null)
        {
            return NotFound();
        }

        var sale = await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (sale == null)
        {
            return NotFound();
        }
        else 
        {
            Sale = sale;
        }
        return Page();
    }
}
