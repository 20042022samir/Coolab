using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using CoolAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
  

        public BlogController(CoolAppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Blog> Blogs = await _context.Blogs
                .Where(x => !x.IsDeleted).Include(x => x.BlogCategories)
                .ThenInclude(x=>x.Category)
                .ToListAsync();
            return View(Blogs);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Blog? blog = await _context.Blogs.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (blog == null)
                return NotFound();
            BlogCategory? blogCat = await _context.BlogCategories.Where(x => !x.IsDeleted && x.Blog == blog)
                .FirstOrDefaultAsync();
            if (blogCat == null)
                return NotFound();
            blogCat.IsDeleted = true;
            blog.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.categories = await _context.BlogCategories
                .Where(x => !x.IsDeleted && x.BlogId == id)
                .Include(x=>x.Category)
                .ToListAsync();
      
            Blog? blog = await _context.Blogs.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (blog == null)
                return NotFound();
            return View(blog);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,Blog blogUppdate)
        {
            ViewBag.categories = await _context.BlogCategories
                .Where(x => !x.IsDeleted && x.BlogId == id)
                .Include(x => x.Category)
                .ToListAsync();
            if (blogUppdate.Title==null || blogUppdate.Description==null)
            {
                TempData["Blog"] = "Bloqun adı və məlumatı boş ola bilməz";
                return View(blogUppdate);
            }
            Blog? blog = await _context.Blogs.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (blog == null)
                return NotFound();
            blog.Title = blogUppdate.Title; blog.Description= blogUppdate.Description;
            blog.Image = blogUppdate.file != null ? blog.file.CreateImage(_enviroment.WebRootPath, "Assets/img") : blog.Image;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories=await _context.Categories
                .Where(x=>!x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blog blog)
        {
            ViewBag.Categories = await _context.Categories
                .Where(x => !x.IsDeleted).ToListAsync();
            if (blog.file == null || blog.Title==null || blog.Description==null)
                return View(blog);
            blog.Image = blog.file.CreateImage(_enviroment.WebRootPath, "Assets/img");
            if(blog.CategoryIds!=null)
            {
            foreach (var item in blog.CategoryIds)
            {
                BlogCategory category = new()
                {
                    Blog=blog,
                    CategoryId=item
                };
                await _context.BlogCategories.AddAsync(category);
            }
            }
            blog.IsAproved = true;
            blog.CreatedDate= DateTime.Now;
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UserBlogDetail(int id)
        {
            Blog? blog = await _context.Blogs
                .Where(x => !x.IsDeleted && x.Id == id && x.CreatedByUser)
                .FirstOrDefaultAsync();
            if (blog == null) 
                return NotFound();
            return View(blog);
        }


        [HttpGet]
        public async Task<IActionResult> IndexUserBlogs()
        {
            IEnumerable<Blog> blogs = await _context.Blogs
                .Where(x => !x.IsDeleted && x.CreatedByUser)
                .ToListAsync();
            return View(blogs);
        }

        [HttpGet]
        public async Task<IActionResult> ApproveBlog(int id)
        {
            Blog? blog = await _context.Blogs
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (blog == null)
                return NotFound();
            blog.IsAproved = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexUserBlogs));
        }


        [HttpGet]
        public async Task<IActionResult> DissApproveBlog(int id)
        {
            Blog? blog = await _context.Blogs
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            blog.IsAproved = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexUserBlogs));
        }

    

    }
}
