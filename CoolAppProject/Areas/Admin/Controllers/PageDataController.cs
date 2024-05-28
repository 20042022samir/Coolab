using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PageDataController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        public PageDataController(CoolAppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<PageData> datas = await _context.PageDatas
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(datas);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PageData data)
        {
            data.Logo = data.file.CreateImage(_enviroment.WebRootPath, "Assets/img");
            await _context.PageDatas.AddAsync(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            PageData? data = await _context.PageDatas.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (data == null)
                return NotFound();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,PageData dataUpdate)
        {
            if (dataUpdate.Address == null || dataUpdate.Email==null || dataUpdate.PhoneNumber==null)
            {
                TempData["Data"] = "Adres Email və telefon nömrısi boş ola bilməz";
                return View(dataUpdate);
            }
            PageData? data = await _context.PageDatas.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (data == null)
                return NotFound();
            data.Address= dataUpdate.Address; 
            data.PhoneNumber= dataUpdate.PhoneNumber; data.Email=dataUpdate.Email;
            data.Instagram=dataUpdate.Instagram;data.Linkedin=dataUpdate.Linkedin;
            data.Facebook=dataUpdate.Facebook;
            data.Youtube=dataUpdate.Youtube;
            data.Logo = dataUpdate.file != null ? dataUpdate.file.CreateImage(_enviroment.WebRootPath, "Assets/img") : data.Logo;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
