using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeedBackController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        public FeedBackController(CoolAppDbContext context,IWebHostEnvironment enviroment)
        {
            _context= context;
            _enviroment= enviroment;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<StudentFeedback> feedBacks = await _context.StudentFeddPacks.Where(x => !x.IsDeleted).ToListAsync();
            return View(feedBacks);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(StudentFeedback feedBack)
        {
            if(feedBack.FullName==null || feedBack.Profession==null || feedBack.Description==null || feedBack.file == null)
            {
                TempData["Registered"] = "Butun Datalar doldurulmalidir";
                return View(feedBack);
            }
            feedBack.Image = feedBack.file.CreateImage(_enviroment.WebRootPath, "Assets/img");
            await _context.StudentFeddPacks.AddAsync(feedBack);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            StudentFeedback? feedBack = await _context.StudentFeddPacks.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (feedBack == null)
                return NotFound();
            feedBack.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            StudentFeedback? feedBack = await _context.StudentFeddPacks.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if(feedBack==null)
                return NotFound();
            return View(feedBack);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,StudentFeedback feddBackUpadte)
        {
            if(feddBackUpadte.FullName==null || feddBackUpadte.Profession==null || feddBackUpadte.Description==null)
            {
                TempData["FeedBack"] = "Tələbə rəylərinin adı ixtisası və məlumatı boş ola bilməz!";
                return View(feddBackUpadte);
            }
            StudentFeedback? feedBack = await _context.StudentFeddPacks.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if(feedBack==null)
                return NotFound();
            feedBack.FullName=feddBackUpadte.FullName; feedBack.Profession = feddBackUpadte.Profession; feedBack.Description = feddBackUpadte.Description;
            feedBack.Image = feddBackUpadte.file != null ? feddBackUpadte.file.CreateImage(_enviroment.WebRootPath, "Assets/img") : feedBack.Image;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
