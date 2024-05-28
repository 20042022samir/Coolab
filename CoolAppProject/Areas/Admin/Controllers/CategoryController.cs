using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        public CategoryController(CoolAppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await _context.Categories
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(category.Name==null)
                return View(category);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Category? category=await _context.Categories.Where(x=>!x.IsDeleted && x.Id==id)
                .FirstOrDefaultAsync();
            if (category == null)
                return NotFound();
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Category? category = await _context.Categories.Where(x => !x.IsDeleted && x.Id == id)
              .FirstOrDefaultAsync();
            if (category == null)
                return NotFound();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Category categoryUpdate)
        {
            if(categoryUpdate.Name==null)
            {
                TempData["Category"] = "kateqoriya adı boş ola bilmız";
                return View(categoryUpdate);
            }
            Category? category = await _context.Categories.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (category == null)
                return NotFound();
            category.Name=categoryUpdate.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
