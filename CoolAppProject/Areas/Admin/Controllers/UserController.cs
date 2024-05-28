using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly CoolAppDbContext _context;
        public UserController(UserManager<AppUser> userManager,CoolAppDbContext context)
        {
            _userManager=userManager;
            _context=context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AppUser> users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EnteredEmails()
        {
            IEnumerable<EmailUser> emails = await _context.EmailUsers
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(emails);
        }
    }
}
