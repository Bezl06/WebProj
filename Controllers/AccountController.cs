using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MvcFrilance.ViewModels;
using MvcFrilance.Models;
using MvcFrilance.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MvcFrilance.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signManager;
        private FrilanceDbContext _context;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, FrilanceDbContext context)
        {
            _userManager = userManager;
            _signManager = signInManager;
            _context = context;
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
                var alreadyExsist = await _userManager.FindByNameAsync(model.Login);
                if (alreadyExsist != null)
                {
                    ModelState.AddModelError("", "Такой логин уже существует");
                    return View(model);
                }
                User user = new User()
                {
                    UserName = model.Login,
                    Email = model.Email,
                    NickName = model.NickName,
                    RegisterDate = DateTime.Now,
                    LastOnline = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                var result1 = await _userManager.AddToRoleAsync(user, model.UserRole);
                if (result.Succeeded && result1.Succeeded)
                {
                    await _signManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors.Concat(result1.Errors))
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(model.Login, model.Password, false, false);
                if (result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            EditFrilancerModel model = new()
            {
                Id = user.Id,
                Login = user.UserName,
                NickName = user.NickName,
                Email = user.Email
            };
            if (await _userManager.IsInRoleAsync(user, "Frilancer"))
            {
                var extraInfo = _context.Frilancers.SingleOrDefault(x => x.UserId == user.Id);
                if (extraInfo != null)
                {
                    model.Description = extraInfo.Resume;
                    model.Spells = extraInfo.Spells.Select(x => x.SpellID).ToList();
                    model.Tags = extraInfo.Tags.Select(x => x.TagID).ToList();
                    ViewBag.Spells = await _context.Spells.Select(x => x.SpellID).ToListAsync();
                    ViewBag.Tags = await _context.Tags.Take(18).Select(x => x.TagID).ToListAsync();
                }
                return View("EditFrilancer", model);
            }
            return View("EditClient", (EditClientViewModel)model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditClientViewModel model, List<string> tags = null, List<string> spells = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Id);
                if (user != null)
                {
                    user.UserName = model.Login;
                    user.Email = model.Email;
                    user.NickName = model.NickName;
                    if (await _userManager.IsInRoleAsync(user, "Frilancer"))
                    {
                        if (tags != null)
                        {

                        }
                    }
                }
            }
        }
    }
}