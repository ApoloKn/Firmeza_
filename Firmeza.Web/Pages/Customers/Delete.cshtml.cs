using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Firmeza.Data.Data;
using Firmeza.Data.Models;

namespace Firmeza.Web.Pages.Customers;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Customer Customer { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Customers == null)
        {
            return NotFound();
        }

        var customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);

        if (customer == null)
        {
            return NotFound();
        }
        else 
        {
            Customer = customer;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Customers == null)
        {
            return NotFound();
        }
        var customer = await _context.Customers.FindAsync(id);

        if (customer != null)
        {
            Customer = customer;
            _context.Customers.Remove(Customer);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
