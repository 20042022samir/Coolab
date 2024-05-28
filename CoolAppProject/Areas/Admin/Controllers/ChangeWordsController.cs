using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChangeWordsController : Controller
    {
        private readonly CoolAppDbContext _context;
        public ChangeWordsController(CoolAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateHomePage()
        {
            HomePageWords? words = await _context.HomeWords
                .Where(x => !x.IsDeleted && x.Id == 1)
                .FirstOrDefaultAsync();
            if (words == null)
                return NotFound();
            return View(words);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHomePage(HomePageWords words)
        {

            if (words.Saheler == null || words.DescriptionSaheler == null || words.SpecialCourse == null ||
    words.SpecialEvent == null || words.Semiarlar == null || words.SeminarlarDescription == null ||
    words.Teachers == null || words.TeachersDesc == null || words.Comment == null 
   || words.CommentDescription == null || words.Blogs == null ||
    words.BlogsDescription == null)
            {
                return View(words);
            }

            HomePageWords? wordsUpdate = await _context.HomeWords
              .Where(x => !x.IsDeleted && x.Id == 1)
              .FirstOrDefaultAsync();
            if(wordsUpdate==null)
                return NotFound();
            wordsUpdate.UpdatedDate = DateTime.Now;
            wordsUpdate.Saheler = words.Saheler; wordsUpdate.DescriptionSaheler=words.DescriptionSaheler;
            wordsUpdate.SpecialCourse= words.SpecialCourse;wordsUpdate.SpecialEvent=words.SpecialEvent;
            wordsUpdate.Semiarlar= words.Semiarlar;wordsUpdate.SeminarlarDescription=words.SeminarlarDescription;
            wordsUpdate.Teachers= words.Teachers;wordsUpdate.TeachersDesc=words.TeachersDesc;wordsUpdate.Comment = words.Comment;
            wordsUpdate.MakeContact= words.MakeContact;
            wordsUpdate.CommentDescription=words.CommentDescription;wordsUpdate.Blogs=words.Blogs;wordsUpdate.BlogsDescription=words.BlogsDescription;
            wordsUpdate.MakeContact = "context";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSeminarPage()
        {
            SeminarsPageWords? words = await _context.SeminarPageWords
                .Where(x => !x.IsDeleted && x.Id == 2)
                .FirstOrDefaultAsync();
            if (words == null)
                return NotFound();
            return View(words);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSeminarPage(SeminarsPageWords words)
        {
            if (words.Seminar == null || words.SeminarDesc == null || words.Filter == null || words.FilterDesc==null)
                return View(words);
            SeminarsPageWords? wordsUpdate=await _context.SeminarPageWords
                .Where(x=>!x.IsDeleted && x.Id==2)
                .FirstOrDefaultAsync();
            if(wordsUpdate==null)
                return NotFound();
            wordsUpdate.Seminar=words.Seminar;wordsUpdate.SeminarDesc = words.SeminarDesc;
            wordsUpdate.FilterDesc=words.FilterDesc;wordsUpdate.FilterDesc = words.FilterDesc;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> UpdateBlogWords()
        {
            BlogPageWords? words = await _context.BlogWords
                .Where(x => !x.IsDeleted && x.Id == 1)
                .FirstOrDefaultAsync();
            if (words == null)
                return NotFound();
            return View(words);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBlogWords(BlogPageWords words)
        {
            if (words.Categories == null || words.LastCategories == null || words.Filter == null || words.FilterDesc == null)
                return View(words);
            BlogPageWords? wordsUpdate = await _context.BlogWords
              .Where(x => !x.IsDeleted && x.Id == 1)
              .FirstOrDefaultAsync();
            if (wordsUpdate == null)
                return NotFound();
            wordsUpdate.Filter = words.Filter;wordsUpdate.FilterDesc = words.FilterDesc;
            wordsUpdate.CreateOwnBlog = words.CreateOwnBlog;
            wordsUpdate.Categories=words.Categories;wordsUpdate.LastCategories = words.LastCategories;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> UpdateContactPage()
        {
            ContactPageWords? words = await _context.ContactWords
                .Where(x => !x.IsDeleted && x.Id == 1)
                .FirstOrDefaultAsync();
            if(words==null)
                return NotFound();
            return View(words);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContactPage(ContactPageWords words)
        {
            if(words.Contact==null || words.ContactDesc==null)
                return View(words);
            ContactPageWords? wordsUpdate = await _context.ContactWords
           .Where(x => !x.IsDeleted && x.Id == 1)
           .FirstOrDefaultAsync();
            if (wordsUpdate == null)
                return NotFound();

            wordsUpdate.Contact = words.Contact;wordsUpdate.ContactDesc = words.ContactDesc;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> UpdateEventPage()
        {
            ContactPageWords? words = await _context.ContactWords
                .Where(x => !x.IsDeleted && x.Id == 1)
                .FirstOrDefaultAsync();
            if(words==null) 
                return NotFound();
            return View(words);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEventPage(ContactPageWords words)
        {
            if(words.Tedbirler==null || words.TedbirlerDesc==null)
                return View(words);
            ContactPageWords? wordsUpdate = await _context.ContactWords
               .Where(x => !x.IsDeleted && x.Id == 1)
               .FirstOrDefaultAsync();
            if (wordsUpdate == null)
                return NotFound();
            wordsUpdate.Tedbirler=words.Tedbirler;wordsUpdate.TedbirlerDesc=words.TedbirlerDesc;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateLoginPage()
        {
            ContactPageWords? words = await _context.ContactWords
                .Where(x => !x.IsDeleted && x.Id == 1)
                .FirstOrDefaultAsync();
            if(words==null)
                return NotFound();
            return View(words);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLoginPage(ContactPageWords words)
        {
            if (words.Password == null || words.Email == null)
                return View(words);
            ContactPageWords? wordsUpdate = await _context.ContactWords
               .Where(x => !x.IsDeleted && x.Id == 1)
               .FirstOrDefaultAsync();
            if (wordsUpdate == null)
                return NotFound();
            wordsUpdate.Email=words.Email;wordsUpdate.Password=words.Password;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRegisterPage()
        {
            ContactPageWords? words = await _context.ContactWords
                .Where(x => !x.IsDeleted && x.Id == 1)
                .FirstOrDefaultAsync();
            if (words == null)
                return NotFound();
            return View(words);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRegisterPage(ContactPageWords words)
        {
            if (words.Name == null || words.Surname == null || words.ConfirmPassword == null || words.Muellimem == null
                || words.Telebeyem == null 
                )
                return View(words);
            ContactPageWords? wordsUpdate = await _context.ContactWords
               .Where(x => !x.IsDeleted && x.Id == 1)
               .FirstOrDefaultAsync();
            if (wordsUpdate == null)
                return NotFound();
            wordsUpdate.Name=words.Name;wordsUpdate.Surname=words.Surname;
            wordsUpdate.ConfirmPassword=words.ConfirmPassword;wordsUpdate.Muellimem=words.Muellimem;
            wordsUpdate.Telebeyem = words.Telebeyem;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
