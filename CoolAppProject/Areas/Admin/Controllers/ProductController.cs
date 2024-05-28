using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        public ProductController(CoolAppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> Products = await _context.Products
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(Products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (product.Name == null || product.Price == null || product.file == null)
                return View(product);
            product.Image = product.file.CreateImage(_enviroment.WebRootPath, "Assets/img");
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Product? product = await _context.Products
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product productUpdate)
        {
            if(productUpdate.Name==null || productUpdate.Price==null)
                return View(productUpdate);
            Product? product = await _context.Products
               .Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (product == null)
                return NotFound();
            product.Name=productUpdate.Name;product.Price=productUpdate.Price;
            product.Image = productUpdate.file != null ? productUpdate.file.CreateImage(_enviroment.WebRootPath, "Assets/img") : product.Image;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Product? product = await _context.Products
            .Where(x => !x.IsDeleted && x.Id == id)
            .FirstOrDefaultAsync();
            if (product == null)
                return NotFound();
            product.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
