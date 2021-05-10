using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuideMVC_.Models;
using GuideMVC_.ViewModels;

namespace GuideMVC_.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly GuideDBContext _db;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 GuideDBContext db)
        {   
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser() { Email = registerModel.Email, UserName = registerModel.Email };

                var operation = await _userManager.CreateAsync(user, registerModel.Password);

                if (operation.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, false);
                    await _db.Persons.AddAsync(new Person()
                    {
                        UserId = user.Id,
                        GenderId = 1
                    });
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in operation.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _signInManager.PasswordSignInAsync(loginModel.Login,
                                                                  loginModel.Password,
                                                                  loginModel.RememberMe,
                                                                  false);

            if (result.Succeeded &&
                !string.IsNullOrEmpty(loginModel.ReturnUrl) &&
                Url.IsLocalUrl(loginModel.ReturnUrl))
            {
                return Redirect(loginModel.ReturnUrl);
            }
            else if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Неверные логин или пароль!");
            }
            return View(new LoginModel { ReturnUrl = loginModel.ReturnUrl });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
