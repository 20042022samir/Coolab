using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ContactController(CoolAppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> CreateContact()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                 ViewBag.User=await _userManager.FindByNameAsync(User.Identity.Name);
            }
            ViewBag.ContactWords = await _context.ContactWords.Where(x => !x.IsDeleted && x.Id == 1)
                .FirstOrDefaultAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(Contact contact)
        {
            ViewBag.ContactWords = await _context.ContactWords.Where(x => !x.IsDeleted && x.Id == 1)
             .FirstOrDefaultAsync();
            if (contact.Email==null || contact.PhoneNumber==null || contact.Message==null || contact.Name==null)
            {
                TempData["Registered"] = "Maili Telefon nömrəsini və mesaji daxil etməlisiniz!";
                return View(contact);
            }
            TempData["Registered"] = "Ugurla muraciet edildi";
            await _context.AddAsync(contact);
            await _context.SaveChangesAsync();
            return View();
        }
    }
}
