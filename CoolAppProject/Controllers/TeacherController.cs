using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Controllers
{

    public class TeacherController : Controller
    {
        private readonly CoolAppDbContext _context;
        public TeacherController(CoolAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
        {
            var Teachers = await _context.teachers
                            .Where(x => !x.IsDeleted)
                            .OrderByDescending(x => x.CreatedDate)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
            var totalItems = await _context.teachers.Where(x=>!x.IsDeleted).CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var sortedBlogs = Teachers.ToList();
            var viewModel = new PagginationViewModel<Teacher>
            {
                Items = Teachers,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages,
            };
            ViewBag.Teachers = await _context.HomeWords.Where(x => !x.IsDeleted && x.Id == 1)
                .FirstOrDefaultAsync();
            return View(viewModel);
        }
    }
}
