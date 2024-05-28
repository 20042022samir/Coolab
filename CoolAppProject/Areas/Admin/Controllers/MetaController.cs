using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MetaController : Controller
    {
        private readonly CoolAppDbContext _context;
        public MetaController(CoolAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            MetaTag? tag = await _context.MetaTags
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync();
            return View(tag);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            MetaTag? tag = await _context.MetaTags
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (tag == null)
                return NotFound();
            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,MetaTag tag)
        {
            MetaTag? tagUPdate = await _context.MetaTags
               .Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (tagUPdate == null)
                return NotFound();
            tagUPdate.MetaWord1 = tag.MetaWord1;
            tagUPdate.MetaWord2= tag.MetaWord2;
            tagUPdate.MetaWord3= tag.MetaWord3;
            tagUPdate.Title=tag.Title;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
