using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        public AboutController(CoolAppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Aboutt> abouts = await _context.Aboutts.Where(x => !x.IsDeleted)
                .ToListAsync();
            return View(abouts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Aboutt aboutt)
        {
            aboutt.Image = aboutt.file.CreateImage(_enviroment.WebRootPath, "Assets/img");
            await _context.Aboutts.AddAsync(aboutt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Aboutt? about = await _context.Aboutts
                .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            return View(about);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,Aboutt abouttUpdate)
        {
            if(abouttUpdate.Title==null || abouttUpdate.Description == null)
            {
                TempData["About"] = "Əsas və haqqında hissələri boş ola bilməz";
                return View(abouttUpdate);
            }
            Aboutt? about = await _context.Aboutts.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (about == null)
                return NotFound();
            about.Image = abouttUpdate.file != null ? abouttUpdate.file.CreateImage(_enviroment.WebRootPath, "Assets/img"):about.Image;
            about.Title = abouttUpdate.Title;
            about.Description = abouttUpdate.Description;
            await _context.SaveChangesAsync(); ;
            return RedirectToAction(nameof(Index));
        }
    }
}
