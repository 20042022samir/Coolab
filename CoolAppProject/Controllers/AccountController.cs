using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using MimeKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CoolAppProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly CoolAppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, CoolAppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            IdentityRole role1 = new() { Name = Roles.Admin.ToString() };
            IdentityRole role2 = new() { Name = Roles.SuperAdmin.ToString() };
            IdentityRole role3 = new() { Name = Roles.User.ToString() };
            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role2);
            await _roleManager.CreateAsync(role3);
            return Json("Createddd!!");
        }


        [HttpGet]
        public async Task<IActionResult> AdminCreate()
        {
            AppUser admin = new()
            {
                Name="Admin",
                Surname="Adminov",
                Email="admin@gmail.com",
                UserName="Admin123",
            };
            await _userManager.CreateAsync(admin, "Admin123@");
            await _userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            AppUser SuperAdmin = new()
            {
                Name = "SuperAdmin",
                Surname="SuperAdminov",
                Email="SuperAdmin@gmail.com",
                UserName="SuperAdmin123",
            };
            await _userManager.CreateAsync(SuperAdmin, "SuperAdmin123@");
            await _userManager.AddToRoleAsync(SuperAdmin,Roles.SuperAdmin.ToString());
            return Json("Admins Created!!");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            ViewBag.LoginWords = await _context.ContactWords.Where(x => !x.IsDeleted && x.Id == 1)
                .FirstOrDefaultAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewBag.LoginWords = await _context.ContactWords.Where(x => !x.IsDeleted && x.Id == 1)
              .FirstOrDefaultAsync();
            if (model.Email==null || model.Password == null)
            {
                TempData["Registered"] = "email ve şifrə daxil edilməlidir!";
                return View(model);
            }  
            ViewBag.Emphty = false;
            AppUser? user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "email və ya şifrə yanlışdır!");
                return View(model);
            }
            var resultt = await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRememberMe, true);
            if (!resultt.Succeeded)
            {
                if (resultt.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "çoxlu uğursuz cəhdə görə 1 dəqiqəlik bloklandınız!");
                    return View(model);
                }
                ModelState.AddModelError(string.Empty, "email və ya şifrə yanlışdır!");
                return View(model);
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains("Admin") || userRoles.Contains("SuperAdmin"))
                TempData["Registered"] = "Siz adminlərdən birisiniz ;)";
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.RegisterWords = await _context.ContactWords
                .Where(x => !x.IsDeleted && x.Id == 1)
                .FirstOrDefaultAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.RegisterWords = await _context.ContactWords
                         .Where(x => !x.IsDeleted && x.Id == 1)
                         .FirstOrDefaultAsync();
            if (model.Name==null || model.Surname==null || model.Email==null 
                || model.Password==null || model.ConfirmPassword==null
                )
            {
                TempData["Registered"] = "Bütün datalar doldurulmalıdır";
                return View(model);
            }
            AppUser user = new()
            {
                Name=model.Name,
                Surname=model.Surname,
                Email=model.Email,
                UserName=model.UserName,
                IsTeacher=model.IsTeacher,
                IsStudent=model.IsStudent,
            };
            user.SchoolUni = model.SchoolUniName != null ? model.SchoolUniName : null;
            var result= await _userManager.CreateAsync(user,model.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(model);
            }
            await _userManager.AddToRoleAsync(user, "User");
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(action: "VerifyEmail", controller: "account", values: new { token = token, email = user.Email }, protocol: Request.Scheme);
            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("samir.ismayilov2004@gmail.com");
                mm.Subject = "Verify Email";
                mm.To.Add(user.Email);
                mm.Body = $"<div style='display:flex;justify-content:center;align-items:center;background-color:grey;border-radius:10px;height:150px;' ><a href='{link}' style='text-decoration:none;padding:5px 7px;margin:auto;border:3px solid red;border-radius:10px; background-color:white;color:black;' >mailnizzi tesdiqleyin!</a></div>";
                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("coolabyouthcenter@gmail.com", "jgat kwpw zlsi nbqw");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    try
                    {
                        smtp.Send(mm);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            TempData["Registered"] = "Zehmet olmasa mailinizi yoxlayin :)";
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();
            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["Registered"] = "Bele bir emaile sahib istifadeci tapilmadi :(";
                return RedirectToAction("index", "home");
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action(action: "resetpassword", controller: "account", values: new { token = token, email = email }, protocol: Request.Scheme);


            MimeMessage emailMessage = new MimeMessage();
            //MailboxAddress emailFrom = new MailboxAddress("Samir Ismayilov", _emailSettings.EmailId);
            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("samir.ismayilov2004@gmail.com");
                mm.Subject = "reset password";
                mm.To.Add(user.Email);
                mm.Body = $"<a href='{link}'>passwordu deyisin :)</a> ";
                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("coolabyouthcenter@gmail.com", "jgat kwpw zlsi nbqw");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    try
                    {
                        smtp.Send(mm);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            TempData["Registered"] = "Zehmet olmasa emailinizi yoxlayin :)";
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();

            ResetPasswordViewModel model = new()
            {

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound();
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!result.Succeeded)
            {
                List<object> errors = new List<object>();
                foreach (var item in result.Errors)
                {
                    errors.Add(item.Description);
                }
                return Json(errors);
            }
            TempData["Registered"] = "Passwordunuz ugurla deyisildi Yeniden daxil ola bilersiniz";
            return RedirectToAction("login", "account");

        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateProfile()
        {
            string username = User.Identity.Name;
            AppUser? user=await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();
            UpdateProfileViewModel model = new()
            {
                Name=user.Name,
                Surname=user.Surname,
                UserName=user.UserName,
            };
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();
            user.Name = model.Name;user.Surname = model.Surname;user.UserName = model.UserName;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(model);
            }
            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var resulltPassword = await _userManager.ChangePasswordAsync(user, model.CurrecntPassword, model.NewPassword);
                if (!resulltPassword.Succeeded)
                {
                    foreach (var item in resulltPassword.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                    return View(model);
                }
            }
            await _signInManager.SignInAsync(user, true);
            TempData["Registered"] = "Ugurla Guncellendi!";
            return RedirectToAction("index", "home");
        }



     
    }
    public enum Roles
    {
        Admin,SuperAdmin,User
    }
}
