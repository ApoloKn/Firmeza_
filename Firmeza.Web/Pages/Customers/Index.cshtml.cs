using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Firmeza.Data.Data;
using Firmeza.Data.Models;

namespace Firmeza.Web.Pages.Customers;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Customer> Customers { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Customers != null)
        {
            Customers = await _context.Customers.ToListAsync();
        }
    }
}
