using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubjectController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        public SubjectController(CoolAppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Subject> subjects = await _context.Subjects
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(subjects);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Subject subject)
        {
            if (subject.file == null)
                return NotFound();
            subject.CreatedDate = DateTime.Now;
            subject.Image = subject.file.CreateImage(_enviroment.WebRootPath, "Assets/img");
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Subject? subject = await _context.Subjects.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if(subject==null)
                return NotFound();
            IEnumerable<Course> courses = await _context.Courses
                .Where(x => !x.IsDeleted && x.Subject == subject)
                .ToListAsync();
            foreach (var item in courses)
            {
                item.IsDeleted = true;
            }
            subject.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Subject? subject = await _context.Subjects.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (subject == null)
                return NotFound();
            return View(subject);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,Subject subjectUpdate)
        {
            if (subjectUpdate.Name==null)
            {
                TempData["Subject"] = "Sahənin adı boş ola bilməz";
                return View(subjectUpdate);
            }
            Subject? subject = await _context.Subjects.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (subject == null)
                return NotFound();
            subject.Image = subjectUpdate.file != null ? subjectUpdate.file.CreateImage(_enviroment.WebRootPath, "Assets/img") : subject.Image;
            subject.Name = subjectUpdate.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
