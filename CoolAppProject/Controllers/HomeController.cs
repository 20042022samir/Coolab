using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using CoolAppProject.Models;
using CoolAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CoolAppProject.Controllers
{
	public class HomeController : Controller
	{
		private readonly CoolAppDbContext _context;
		public HomeController(CoolAppDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			HomeViewModel model = new();
			model.Teachers = await _context.teachers.Where(x => !x.IsDeleted && x.ForHomePage).ToListAsync();
			model.feedbacks = await _context.StudentFeddPacks.Where(x => !x.IsDeleted).ToListAsync();
			model.abouts = await _context.Aboutts.Where(x => !x.IsDeleted).ToListAsync();
			model.sliders = await _context.Sliders.Where(x => !x.IsDeleted).ToListAsync();
			model.Subjects = await _context.Subjects.Where(x => !x.IsDeleted).Include(x => x.Courses.Where(x=>!x.IsDeleted))
				.OrderByDescending(x => x.CreatedDate)
				.Take(7)
				.ToListAsync();
			model.Courses = await _context.Courses.Where(x => !x.IsDeleted && x.ForHomeScreen).Include(x=>x.Subject).OrderByDescending(x=>x.CreatedDate)
				.ToListAsync();
			model.Blogs = await _context.Blogs.Where(x => !x.IsDeleted).OrderByDescending(x=>x.CreatedDate)
				.Take(3)
				.ToArrayAsync();
			model.Events = await _context.Events.Where(x => !x.IsDeleted && x.ForMainPage).ToListAsync();
			model.Comments = await _context.Comments.Where(x => !x.IsDeleted && x.ForHomeScreen).Include(x=>x.Course)
				.ToListAsync();
			model.SpecialEvent = await _context.Events.Where(x => !x.IsDeleted && x.SpecialEvent)
				.FirstOrDefaultAsync();
			model.specialCourse = await _context.Courses.Where(x => !x.IsDeleted && x.IsSpecial)
				.FirstOrDefaultAsync();
			model.specialEvents = await _context.Events.Where(x => !x.IsDeleted && x.SpecialEvent)
				.ToListAsync();
			model.specialCourses = await _context.Courses
				.Where(x => !x.IsDeleted && x.IsSpecial)
				.ToListAsync();
			model.homeWords = await _context.HomeWords.Where(x => !x.IsDeleted && x.Id == 1)
				.FirstOrDefaultAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> SubjectDetail(int id)
		{
			Subject? subject = await _context.Subjects
				.Where(x => !x.IsDeleted && x.Id==id).Include(x => x.Courses.Where(x=>!x.IsDeleted))
				.FirstOrDefaultAsync();
			if (subject == null)
				return NotFound();
			return View(subject);
		}


		[HttpPost]
		public async Task<IActionResult> CreateEmaailFromInput(string email)
		{
			IEnumerable<EmailUser> emails = await _context.EmailUsers
				.Where(x => !x.IsDeleted).ToListAsync();
			foreach (var item in emails)
			{
				if (item.Emaill == email)
				{
					TempData["Registered"] = "Bu email daxil edilib!";
					return RedirectToAction(nameof(Index));
				}
			}
			if(email==null || string.IsNullOrWhiteSpace(email))
			{
				TempData["Registered"]="Emaili daxil edin";
				return RedirectToAction(nameof(Index));
			}
			EmailUser emaill = new()
			{
				Emaill = email
			};
			TempData["Registered"] = "Ugurla emailiniz ellave edildi!";
			await _context.EmailUsers.AddAsync(emaill);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}


		[HttpGet]
		public async Task<IActionResult> GetAllCourses()
		{
			List<Course> courses = await _context.Courses
				.Where(x => !x.IsDeleted)
				.ToListAsync();
			return Json(courses);
		}


        [HttpGet]
        public async Task<IActionResult> SearchCourses(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty.");
            }

            searchTerm = searchTerm.ToLower(); // Convert the search term to lowercase for case-insensitive comparison

            List<Course> filteredCourses = await _context.Courses
                .Where(c => !c.IsDeleted && c.Name.ToLower().Contains(searchTerm))
                .ToListAsync();

            return Json(filteredCourses);
        }

        [HttpGet]
        public async Task<IActionResult> ShowSearch(string searchTearm)
        {
            List<Course> Courses = await _context.Courses
      .Where(c => !c.IsDeleted && c.Name.ToLower().Contains(searchTearm))
      .Include(x => x.Subject)
      .ToListAsync();
            List<Event> Events = await _context.Events
                .Where(c => !c.IsDeleted && c.Name.ToLower().Contains(searchTearm))
                .ToListAsync();
            List<Teacher> Teachers = await _context.teachers
                .Where(c => !c.IsDeleted && c.FullName.ToLower().Contains(searchTearm))
                .ToListAsync();

            SearchViewModel model = new()
            {
                Courses = Courses,
                Events = Events,
                Teachers = Teachers,
            };
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> MakeSearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                TempData["Enter"] = "Axtarmaq ucun daxil etmelisiniz!";
                return RedirectToAction("Index", "Home");
            }

            searchTerm = searchTerm.ToLower();
  

			return RedirectToAction("showsearch","home", new
			{
				searchTearm=searchTerm
			});

        }

		[HttpGet]
		public async Task<IActionResult> GetAllSubjects(int page = 1, int pageSize = 8)
        {
            var subjects = await _context.Subjects
                            .Where(x => !x.IsDeleted )
							.Include(x=>x.Courses.Where(x=>!x.IsDeleted))
                            .OrderByDescending(x => x.CreatedDate)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
            var totalItems = await _context.Subjects.Where(x => !x.IsDeleted ).CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var sortedBlogs = subjects.ToList();
            var viewModel = new PagginationViewModel<Subject>
            {
                Items = subjects,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages,
              
            };
           
            return View(viewModel);
        }


    }
}