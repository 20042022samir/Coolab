using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        public CourseController(CoolAppDbContext context,IWebHostEnvironment enviroment)
        {
            _context= context;
            _enviroment= enviroment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> Courses = await _context.Courses.Where(x => !x.IsDeleted)
                .Include(x=>x.Subject)
                .ToListAsync();
            return View(Courses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Subjects = await _context.Subjects.Where(x => !x.IsDeleted)
                .ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            ViewBag.Subjects = await _context.Subjects.Where(x => !x.IsDeleted)
                .ToListAsync();
            if (course.file == null || course.SubjectId==null || course.Price==null || course.RealStartedDate==null)
                return View(course);
            course.CreatedDate = DateTime.Now;
            course.Image = course.file.CreateImage(_enviroment.WebRootPath, "Assets/img");
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Course? course = await _context.Courses.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (course == null)
                return NotFound();
            course.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Subjects = await _context.Subjects.Where(x => !x.IsDeleted)
             .ToListAsync();
            Course? course = await _context.Courses.Where(x => !x.IsDeleted && x.Id == id )
               .FirstOrDefaultAsync();
            if (course == null)
                return NotFound();
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,Course courseUpadte)
        {
            ViewBag.Subjects = await _context.Subjects.Where(x => !x.IsDeleted)
             .ToListAsync();
            if(courseUpadte.Name==null || courseUpadte.Description == null || courseUpadte.Price==null || courseUpadte.RealStartedDate==null)
            {
                TempData["Course"] = "Kursun adı və məlumatı boş ola biləz";
                return View(courseUpadte);
            }
            Course? course = await _context.Courses.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (course == null)
                return NotFound();
            course.Image = courseUpadte.file != null ? courseUpadte.file.CreateImage(_enviroment.WebRootPath, "Assets/img"):course.Image;
            course.Name = courseUpadte.Name;course.RealStartedDate= courseUpadte.RealStartedDate;
            course.SubjectId= courseUpadte.SubjectId;course.Price= courseUpadte.Price;
            course.Description= courseUpadte.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ShowCourseContacts(int courseId)
        {
            Course? course = await _context.Courses
                .Where(x => !x.IsDeleted && x.Id == courseId)
                .Include(x => x.CourseContacts.Where(x => !x.IsDeleted))
                .FirstOrDefaultAsync();
            if (course == null)
                return NotFound();
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteContact(int id,int modelId)
        {
            CourseContact? contact = await _context.CourseContacts
                .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if(contact==null)
                return NotFound();
            contact.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> MakeForHomeScreen(int courseId)
        {
            Course? course = await _context.Courses.Where(x => !x.IsDeleted && x.Id == courseId)
         .FirstOrDefaultAsync();
            if(course==null)
                return NotFound();
            if (course.ForHomeScreen == true)
            {
                TempData["Registered"] = "Bu kurs zatən əsas ekrandadır";
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<Course> Courses = await _context.Courses
                .Where(x => !x.IsDeleted && x.ForHomeScreen).ToListAsync();
            if (Courses.Count()== 6)
            {
                TempData["Registered"] = "Əsas Səhifədə 6-dən çox kurs ola bilmez!";
                return RedirectToAction(nameof(Index));
            }
            course.ForHomeScreen = true;
            TempData["Registered"] = "Əsas Səhifəyə əlavə edildi";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
     
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFormHomeScreen(int courseId)
        {
            Course? course = await _context.Courses.Where(x => !x.IsDeleted && x.Id == courseId)
         .FirstOrDefaultAsync();
            if (course == null)
                return NotFound();
            if (course.ForHomeScreen == false)
            {
                TempData["Registered"] = "Bu kurs zatən əsas ekranda deyil";
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<Course> Courses = await _context.Courses
                .Where(x => !x.IsDeleted && x.ForHomeScreen).ToListAsync();
            if (Courses.Count() == 3)
            {
                TempData["Registered"] = "Əsas Səhifədə 3-dən çox az ola bilmez!";
                return RedirectToAction(nameof(Index));
            }
            course.ForHomeScreen = false;
            TempData["Registered"] = "Əsas ekrandan silindi";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MakeSpecial(int Id)
        {
            Course? course=await _context.Courses
                .Where(x=>!x.IsDeleted && x.Id==Id)
                .FirstOrDefaultAsync();
            if(course==null)
                return NotFound();
            course.IsSpecial = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> ShowAllComments()
        {
            IEnumerable<Comment> comments=await _context.Comments
                .Where(x=>!x.IsDeleted).Include(x=>x.Course)
                .ToListAsync();
            return View(comments);
        }

        [HttpGet]
        public async Task<IActionResult> MakeCommentDForHome(int id)
        {
            Comment? comment=await _context.Comments.Where(x=>!x.IsDeleted && x.Id==id)
                .FirstOrDefaultAsync();
            if (comment == null)
                return NotFound();
            comment.ForHomeScreen = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowAllComments));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveCommentFormHome(int id)
        {
            Comment? comment = await _context.Comments.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (comment == null)
                return NotFound();
            comment.ForHomeScreen = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowAllComments));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCourseComment(int id)
        {
            Comment? commnet=await _context.Comments.Where(x=>!x.IsDeleted && x.Id==id)
                .FirstOrDefaultAsync();
            if (commnet == null)
                return NotFound();
            commnet.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowAllComments));
        }


        [HttpGet]
        public async Task<IActionResult> ShowAllCourseContacts()
        {
            IEnumerable<CourseContact> contacts = await _context.CourseContacts
                .Where(x => !x.IsDeleted).Include(x => x.Course)
                .ToListAsync();
            return View(contacts);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteCourseContact(int id)
        {
            CourseContact? contact = await _context.CourseContacts.Where(x => !x.IsDeleted && x.Id==id)
                .FirstOrDefaultAsync();
            if (contact == null)
                return NotFound();
            contact.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowAllCourseContacts));
        }


        [HttpGet]
        public async Task<IActionResult> ShowCourseMessagee(int id)
        {
            Comment? comment = await _context.Comments.Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.Course).FirstOrDefaultAsync();
            if(comment==null)
                return NotFound();
            return View(comment);
        }

        [HttpGet]
        public async Task<IActionResult> MakeSimpleCourse(int id)
        {
            Course? course = await _context.Courses
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if(course==null)
                return NotFound();
            course.IsSpecial= false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

   

    }
}
