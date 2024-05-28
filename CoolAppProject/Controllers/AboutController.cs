using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Controllers
{
    public class AboutController : Controller
    {
        private readonly CoolAppDbContext _context;
        public AboutController(CoolAppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            AboutPageViewModel model = new();
            model.Abouts = await _context.Aboutts
                .Where(x => !x.IsDeleted).ToListAsync();
            model.feedBacks = await _context.StudentFeddPacks
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(model);
        }
    }
}
