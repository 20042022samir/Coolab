using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeacherController : Controller
    {
        private readonly CoolAppDbContext _context;
        public readonly IWebHostEnvironment _enviroment;
        public TeacherController(CoolAppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Teacher> teachers = await _context.teachers
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(teachers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Courses = await _context.Courses.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            ViewBag.Courses = await _context.Courses.Where(x => !x.IsDeleted).ToListAsync();
            if (teacher.file==null || teacher.FullName==null || teacher.Profession == null)
            {
                TempData["Registered"] = "Sekil, Tamad ve profession mutleq qeyd edilmelidir!";
                return View(teacher);
            }
            teacher.Image = teacher.file.CreateImage(_enviroment.WebRootPath, "Assets/img");
            teacher.CreatedDate = DateTime.Now;
            await _context.teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Teacher? teacher=await _context.teachers
                .Where(x=>!x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (teacher == null)
                return NotFound();
            teacher.IsDeleted= true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Courses = await _context.Courses.Where(x => !x.IsDeleted).ToListAsync();
            Teacher? teacher = await _context.teachers
                .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (teacher == null)
                return NotFound();
            return View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,Teacher teacherUpdate)
        {
            ViewBag.Courses = await _context.Courses.Where(x => !x.IsDeleted).ToListAsync();
            if(teacherUpdate.FullName==null || teacherUpdate.Profession == null)
            {
                TempData["Teacher"] = "Müəllimin tam adı və ixtisası boş ola bilməz";
                return View(teacherUpdate);
            }
            Teacher? teacher = await _context.teachers.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            teacher.Image = teacherUpdate.file != null ? teacherUpdate.file.CreateImage(_enviroment.WebRootPath, "Assets/img") : teacher.Image;
            teacher.FullName = teacherUpdate.FullName;teacher.UpdatedDate=DateTime.Now; teacher.Profession=teacherUpdate.Profession;
            teacher.FaceBook1= teacherUpdate.FaceBook1; teacher.Instagram= teacherUpdate.Instagram; teacher.Linkedin= teacherUpdate.Linkedin;
            teacher.CourseId = teacherUpdate.CourseId;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AddToHomePage(int id)
        {
            Teacher? teacher = await _context.teachers.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if(teacher==null)
                return NotFound();
            IEnumerable<Teacher> homeTeachers = await _context.teachers.Where(x => !x.IsDeleted && x.ForHomePage)
                .ToListAsync();
            if (homeTeachers.Count() == 8)
                return RedirectToAction(nameof(Index));
            teacher.ForHomePage=true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromHome(int id)
        {
            Teacher? teacher = await _context.teachers.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (teacher == null)
                return NotFound();
            IEnumerable<Teacher> homeTeachers = await _context.teachers.Where(x => !x.IsDeleted && x.ForHomePage)
               .ToListAsync();
            if (homeTeachers.Count() == 4)
                return RedirectToAction(nameof(Index));
            teacher.ForHomePage = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
