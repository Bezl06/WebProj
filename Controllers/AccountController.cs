using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MvcFrilance.ViewModels;
using MvcFrilance.Data;
using MvcFrilance.Models;

namespace MvcFrilance.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    UserName = model.Name,
                    Email = model.Email,
                    Login = model.Login,
                    RegisterDate = DateTime.Now,
                    LastOnline = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        // [HttpPost]
        // public async Task<IActionResult> Login(LoginViewModel model)
        // {
        //     if(ModelState.IsValid)
        //     {

        //     }
        // }
    }
}