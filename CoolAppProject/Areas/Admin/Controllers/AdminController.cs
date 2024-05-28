using CoolApp.Core.Entities;
using CoolAppProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AppUser> admins = await _userManager.GetUsersInRoleAsync("Admin");
            return View(admins);
        }

        [HttpGet]
        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> CreateAdmin()
        {
            return View();
        }

       
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> CreateAdmin(RegisterViewModel model)
        {
            if(model.Name==null || model.Surname==null || model.Email==null || model.UserName==null || model.Password==null || model.ConfirmPassword==null)
            {
                TempData["Registered"] = "Butu datalar doldurulmalidir!";
                return View(model);
            }
            AppUser admin = new()
            {
                Name=model.Name,
                UserName=model.UserName,
                Email=model.Email,
                Surname=model.Surname,
            };
            var result=await _userManager.CreateAsync(admin,model.Password);
            if(!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(model);
            }
            await _userManager.AddToRoleAsync(admin, "Admin");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string adminName)
        {
            AppUser? admin=await _userManager.FindByNameAsync(adminName);
            if (admin == null)
                return NotFound();
            await _userManager.DeleteAsync(admin);
            return RedirectToAction(nameof(Index));
        }

       


    }
}
