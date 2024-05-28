using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PriceController : Controller
    {
        private readonly CoolAppDbContext _context;
        public PriceController(CoolAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<CooursePrice> prices = await _context.Priices
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(prices);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CooursePrice price)
        {
            if (price.FromPrice == null || price.ToPrice == null)
                return View(price);
            await _context.Priices.AddAsync(price);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            CooursePrice? price = await _context.Priices
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (price == null)
                return NotFound();
            price.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            CooursePrice? price = await _context.Priices
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (price == null)
                return NotFound();
            return View(price);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,CooursePrice priceUpdate)
        {
            if(priceUpdate.FromPrice==null || priceUpdate.ToPrice==null)
                return View(priceUpdate);
            CooursePrice? price= await _context.Priices
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if(price==null)
                return NotFound();
            price.FromPrice= priceUpdate.FromPrice;price.ToPrice= priceUpdate.ToPrice;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
