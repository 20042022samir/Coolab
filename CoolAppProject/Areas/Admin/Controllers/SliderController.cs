using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        public SliderController(CoolAppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> Sliders = await _context.Sliders
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(Sliders);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            if(slider.file==null || slider.Title==null || slider.Description == null)
                return View(slider);
            slider.Image = slider.file.CreateImage(_enviroment.WebRootPath, "Assets/img");
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Slider? slider = await _context.Sliders
                .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (slider == null)
                return NotFound();
            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,Slider sliderUpdate)
        {
            if(sliderUpdate.Description==null || sliderUpdate.Title == null)
            {
                TempData["Slider"] = "Sliderin adı və məlumatı boş ola bilməz";
                return View(sliderUpdate);
            }
            Slider? slider = await _context.Sliders.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (slider == null)
                return NotFound();
            slider.Image = sliderUpdate.file != null ? sliderUpdate.file.CreateImage(_enviroment.WebRootPath, "Assets/img") : slider.Image;
            slider.Description = sliderUpdate.Description; slider.Title=sliderUpdate.Title;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Slider? slider = await _context.Sliders.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if(slider==null)
                return NotFound();
            slider.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
