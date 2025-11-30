using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Firmeza.Data.Data;
using Firmeza.Data.Models;

namespace Firmeza.Web.Pages.Products;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Product> Products { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Products != null)
        {
            Products = await _context.Products.ToListAsync();
        }
    }
}
