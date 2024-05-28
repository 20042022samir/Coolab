using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly CoolAppDbContext _context;
        public ContactController(CoolAppDbContext context)
        {
            _context= context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Contact> Contacts = await _context.Contacts
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(Contacts);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Contact? contact = await _context.Contacts.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (contact == null)
                return NotFound();
            contact.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
    }
}
