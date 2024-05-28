using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Controllers
{

    public class EventController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public EventController(CoolAppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var events = await _context.Events
                            .Where(x => !x.IsDeleted)
                            .OrderByDescending(x => x.CreatedDate)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
            var totalItems = await _context.Events.Where(x => !x.IsDeleted).CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var sortedBlogs = events.ToList();
            var viewModel = new PagginationViewModel<Event>
            {
                Items = events,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages,

            };
            viewModel.contactWords = await _context.ContactWords
                .Where(x => !x.IsDeleted && x.Id == 1)
                .FirstOrDefaultAsync();
            if (viewModel.contactWords == null)
                return NotFound();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            Event? eventt = await _context.Events.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (eventt == null)
                return NotFound();
            EventDetailViewModel model = new()
            {
                Event=eventt,
            };
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Detail(int id,EventDetailViewModel model)
        {
            if(model.FullName==null || model.Email==null || model.PhoneNumber==null)
                return View(model);
            Event? eventt = await _context.Events
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (eventt == null)
                return NotFound();
            EventMessage message = new()
            {
                UserName=model.FullName,
                Email=model.Email,
                PhoneNumber=model.PhoneNumber,
                Event=eventt,
                IsTeacher=model.IsTeacher,
                IsStudent=model.IsStudent,
            };
            message.CreatedDate = DateTime.Now;
            message.SchoolUniName = model.SchoolUniName != null ? model.SchoolUniName : null;
            await _context.EventMessages.AddAsync(message);
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Muraciet edildi!";
            return RedirectToAction("index", "home");
        }

    }
}
