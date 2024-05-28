using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using CoolAppProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolAppProject.Controllers
{
    public class CourseController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _enviroment;
        public CourseController(CoolAppDbContext context,UserManager<AppUser> userManager,IWebHostEnvironment enviroment)
        {
            _context = context;
            _userManager = userManager;
            _enviroment = enviroment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            ViewBag.Prices = await _context.Priices.Where(x => !x.IsDeleted)
                .ToListAsync();
            ViewBag.Subjects = await _context.Subjects.Where(x => !x.IsDeleted)
                .ToListAsync();
            var Courses = await _context.Courses
                            .Where(x => !x.IsDeleted)
                            .Include(x=>x.Subject)
                            .OrderByDescending(x => x.CreatedDate)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
            var totalItems = await _context.Courses.Where(x=>!x.IsDeleted).CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var sortedBlogs = Courses.ToList();
            var viewModel = new PagginationViewModel<Course>
            {
                Items = Courses,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages,

            };
            viewModel.words = await _context.SeminarPageWords.Where(x => !x.IsDeleted && x.Id == 2)
                .FirstOrDefaultAsync();
            if (viewModel.words == null)
                return NotFound();
            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            Course? course = await _context.Courses
                .Where(x => !x.IsDeleted && x.Id == id).Include(x=>x.Teachers.Where(x=>!x.IsDeleted))
                .Include(x=>x.Subject)
                .FirstOrDefaultAsync();
            if (course == null)
                return NotFound();
            course.ViewCount++;
            CourseDetailViewModel model = new()
            {
                Course=course,
                CourseId=course.Id
            };
            await _context.SaveChangesAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Detail(CourseDetailViewModel model)
        {
            if(model.Email==null || model.PhoneNumber==null || model.FullName==null)
            {
                TempData["Registered"] = "Telefon nömrəsi Tam Ad və mail daxil edilməlidir!";
                return View(model);
            }
            CourseContact contact = new()
            {
                CourseId = model.CourseId,
                Email = model.Email,
                Message = model.Message != null ? model.Message : null,
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName,
                IsTeacher=model.IsTeacher,
                IsStudent=model.IsStudent
            };
            contact.SchoolUniName = model.SchoolUniName != null ? model.SchoolUniName : null;
            await _context.CourseContacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Müraciətiniz ücün təşəkkür edirik ən qısa zamanda cavablanacaq";
            return RedirectToAction("index", "home");
        }


        [HttpPost]
        public async Task<IActionResult> ShareComment(CourseDetailViewModel model,int courseId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                if (model.CommentUserName == null || model.CommentUserMessage == null)
                {
                    TempData["Registered"] = "Rəy yazmaq üçün mesajı və adınızı daxil etməlisiniz";
                    return RedirectToAction("detail", "course", new { id=courseId});
                }
                Comment comment = new()
                {
                    FullName = model.CommentUserName,
                    Description = model.CommentUserMessage,
                    CourseId = model.CourseId,
                };
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("index", "home");
            }
            if(model.CommentUserName==null || model.CommentUserMessage == null)
            {
                TempData["Registered"] = "Rəy yazmaq üçün mesajı və adınızı daxil etməlisiniz";
                return RedirectToAction("detail", "course",new {id=courseId});
            }
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();
            if (model.CommentUserName == null || model.CommentUserMessage == null)
                return View(model);
            Comment commentt = new()
            {
                FullName = model.CommentUserName,
                Description = model.CommentUserMessage,
                CourseId = model.CourseId,
                AppUser=user,
            };
            await _context.Comments.AddAsync(commentt);
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Rəy Əlave Edildi!";
            return RedirectToAction("index","home");
        }


        [HttpGet]
        public async Task<IActionResult> CreatePostByUser()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> FilterCourses(PagginationViewModel<Course> model)
        {
            ViewBag.Prices = await _context.Priices.Where(x => !x.IsDeleted)
               .ToListAsync();
            ViewBag.Subjects = await _context.Subjects.Where(x => !x.IsDeleted)
                .ToListAsync();
            if (model.SubjectId == 0 && model.PriceId == 0)
            {
                TempData["MustChoze"] = "Filter Ucun Secmelisiniz!";
                return RedirectToAction("index", "course");   
            }
            return RedirectToAction("ShowFilteredCourses", "course", new
            {
                priceId=model.PriceId,
                subjectId=model.SubjectId
            });
        }


     

        [HttpGet]
        public async Task<IActionResult> ShowFilteredCourses(int priceId,int subjectId)
        {
            ViewBag.Prices = await _context.Priices.Where(x => !x.IsDeleted)
            .ToListAsync();
            ViewBag.Subjects = await _context.Subjects.Where(x => !x.IsDeleted)
                .ToListAsync();
            var query = _context.Courses
                .Where(x => !x.IsDeleted);

            if (priceId != 0)
            {
                CooursePrice? price = await _context.Priices
                    .Where(x => !x.IsDeleted && x.Id == priceId)
                    .FirstOrDefaultAsync();
                if (price == null)
                    return NotFound();
                query = query.Where(x => x.Price <= price.ToPrice && x.Price >= price.FromPrice);
            }

            if (subjectId != 0)
            {
                query = query.Where(x => x.SubjectId == subjectId);
            }

            var filteredCourses = await query.ToListAsync();    
            return View(filteredCourses);
        }

       
    }
}
