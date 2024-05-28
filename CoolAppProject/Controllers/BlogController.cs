using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using CoolAppProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CoolAppProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        private readonly UserManager<AppUser> _userManager;
        public BlogController(CoolAppDbContext context,IWebHostEnvironment enviroment,UserManager<AppUser> userManager)
        {
            _context = context;
            _enviroment = enviroment;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
        {

            ViewBag.Catgeories = await _context.Categories
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            var blogs = await _context.Blogs
                            .Where(x => !x.IsDeleted && x.IsAproved)
                            .OrderByDescending(x=>x.CreatedDate)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
            var totalItems = await _context.Blogs.Where(x=>!x.IsDeleted && x.IsAproved).CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var sortedBlogs = blogs.ToList();
            var viewModel = new PagginationViewModel<Blog>
            {
                Items = blogs,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                ExtraBlogs = await _context.Blogs
                .Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate)
                .Take(3)
                .ToListAsync(),
                Catgeories = await _context.Categories
                .Where(x => !x.IsDeleted).Include(x => x.BlogCategories.Where(x=>!x.IsDeleted))
                .ThenInclude(x=>x.Blog)
                .Where(x=>!x.IsDeleted)
                .ToListAsync()
            };
            viewModel.blogWords = await _context.BlogWords.Where(x => !x.IsDeleted && x.Id == 1)
                .FirstOrDefaultAsync();
            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            Blog? blog = await _context.Blogs
                .Where(x => !x.IsDeleted && x.Id==id)
                .Include(x=>x.BlogCategories.Where(x=>!x.IsDeleted))
                .ThenInclude(x=>x.Category)
                .FirstOrDefaultAsync();
            if (blog == null)
                return NotFound();
            ViewBag.Categories = await _context.Categories
                .Where(x => !x.IsDeleted)
                .Include(x=>x.BlogCategories.Where(x=>!x.IsDeleted))
                .ToListAsync();
            ViewBag.ExtraBlogs = await _context.Blogs
                .Where(x => !x.IsDeleted && x.Id != id)
                .OrderByDescending(x => x.CreatedDate)
                .Take(3)
                .ToListAsync();
            return View(blog);
        }


        [HttpGet]
        public async Task<IActionResult> BlogCategoryDetail(int id, int page = 1, int pageSize = 4)
        {
           
            Category? cat=await _context.Categories.Where(x=>!x.IsDeleted && x.Id==id)
              .FirstOrDefaultAsync();
            if (cat == null)
                return NotFound();
            ViewBag.Cat = cat;
            List<BlogCategory> blogs = await _context.BlogCategories
         .Where(x => !x.IsDeleted && x.Category == cat)
         .Include(x => x.Blog)
         .Where(x => !x.Blog.IsDeleted && x.Blog.IsAproved) 
         .OrderByDescending(x => x.CreatedDate)
         .Skip((page - 1) * pageSize)
         .Take(pageSize)
         .ToListAsync();


            var totalItems = await _context.BlogCategories
                .Where(x => !x.IsDeleted && x.Category == cat)
                 .Include(x=>x.Blog)
                 .Where(x=>!x.IsDeleted)
                .CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var sortedBlogs = blogs.ToList();
            var viewModel = new PagginationViewModel<BlogCategory>
            {
                Items = blogs,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages,
            };
            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> CreateeBlog()
        {
            ViewBag.Categories = await _context.Categories
                .Where(x => !x.IsDeleted).ToListAsync();
             return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateeBlog(Blog blog)
        {
            ViewBag.Categories = await _context.Categories
             .Where(x => !x.IsDeleted).ToListAsync();
            if (blog.file == null || blog.Description == null || blog.Title == null)
                return View(blog);
            blog.FullName = "fullname";
            blog.CreatedDate = DateTime.Now;
            blog.Image = blog.file.CreateImage(_enviroment.WebRootPath, "Assets/img");
            blog.CreatedByUser = true;
            if (blog.CategoryIds!= null)
            {
                foreach (var item in blog.CategoryIds)
                {
                    BlogCategory catgegory = new()
                    {
                        CategoryId = item,
                        Blog = blog,
                    };
                    await _context.BlogCategories.AddAsync(catgegory);
                }
            }
            TempData["CreatedBlog"] = "Blog təsdiq edildikdən sonra əlavə ediləcək";
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> FilteredBlogs(PagginationViewModel<Blog> model)
        {
            ViewBag.Catgeories = await _context.Categories
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            if(model.CreatedByUserNumber==0 && model.CategoryId == 0)
            {
                TempData["NotBloq"] = "Filter Üçün seçməlisiniz!";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("ShowFilteredBlogs", "blog", new
            {
                CategoryId=model.CategoryId,
                CreatedByuser=model.CreatedByUserNumber
            });
        }

      

        [HttpGet]
        public async Task<IActionResult> ShowFilteredBlogs(int CategoryId,int CreatedByuser)
        {
            ViewBag.Catgeories = await _context.Categories
              .Where(x => !x.IsDeleted)
              .ToListAsync();
            if (CategoryId == 0 && CreatedByuser != 0)
            {
                if (CreatedByuser == 1)
                {
                    List<Blog> blogssss = await _context.Blogs
                        .Where(x => !x.IsDeleted && x.CreatedByUser && x.IsAproved)
                        .ToListAsync();
                    return View(blogssss);
                }
                List<Blog> blogs = await _context.Blogs
                   .Where(x => !x.IsDeleted && x.IsAproved && !x.CreatedByUser)
                   .ToListAsync();
                return View(blogs);
            }
            if (CategoryId != 0 && CreatedByuser == 0)
            {
                IEnumerable<BlogCategory> blogs = await _context.BlogCategories
                    .Where(x => !x.IsDeleted)
                    .Include(x => x.Blog)
                    .Where(x => !x.Blog.IsDeleted && x.Blog.IsAproved)
                    .Include(x => x.Category)
                    .ToListAsync();
                List<Blog> blogsFiltered = new List<Blog>();
                foreach (var item in blogs)
                {
                    if (item.Category.Id == CategoryId)
                    {
                        blogsFiltered.Add(item.Blog);
                    }
                }
                return View(blogsFiltered);
            }
            if (CreatedByuser == 1)
            {
                IEnumerable<BlogCategory> blogss = await _context.BlogCategories
                       .Where(x => !x.IsDeleted)
                       .Include(x => x.Blog)

                       .Where(x => !x.Blog.IsDeleted && x.Blog.IsAproved && x.Blog.CreatedByUser)
                       .Include(x => x.Category)
                       .ToListAsync();

                List<Blog> blogsFilteredd = new List<Blog>();
                foreach (var item in blogss)
                {
                    if (item.Category.Id == CategoryId)
                    {
                        blogsFilteredd.Add(item.Blog);
                    }
                }
                return View(blogsFilteredd);
            }
            IEnumerable<BlogCategory> blogsss = await _context.BlogCategories
                      .Where(x => !x.IsDeleted)
                      .Include(x => x.Blog)
                      .Where(x => !x.Blog.IsDeleted && x.Blog.IsAproved && !x.Blog.CreatedByUser)
                      .Include(x => x.Category)
                      .ToListAsync();

            List<Blog> blogsFiltereddd = new List<Blog>();
            foreach (var item in blogsss)
            {
                if (item.Category.Id == CategoryId)
                {
                    blogsFiltereddd.Add(item.Blog);
                }
            }
            return View(blogsFiltereddd);
        }

        [HttpGet]
        public async Task<IActionResult> CreateBlogg(Blog blog)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Registered"] = "you must be logged in!";
                return View(blog);
            }
            blog.Image= blog.file != null ? blog.file.CreateImage(_enviroment.WebRootPath, "Assets/img") : "";
            string username = User.Identity.Name;
            AppUser? user=await _userManager.FindByNameAsync(username);
            if (user == null)
                return View(blog);
            blog.UserName = username;
            foreach (var item in blog.CategoryIds)
            {
                BlogCategory cat = new()
                {
                    CategoryId=item,
                    Blog=blog,
                };
                await _context.BlogCategories.AddAsync(cat);
                await _context.SaveChangesAsync();
            }
            TempData["Registered"] = "blog succesfuly created!";
            return View(blog);

        }




        
    }
}
