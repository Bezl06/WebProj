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
                    LastOnline = DateTime.Now,

                };
                if (model.UserRole == "Frilancer")
                {
                    user.FrilancerInfo = new FrilancerAdditionalInfo();
                }
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
                await _context.Entry(extraInfo).Collection(x => x.Spells).LoadAsync();
                await _context.Entry(extraInfo).Collection(x => x.Tags).LoadAsync();
                if (extraInfo != null)
                {
                    model.Description = extraInfo.Resume;
                    model.Spells = extraInfo.Spells.Select(x => x.SpellID).ToList();
                    model.Tags = extraInfo.Tags.Select(x => x.TagID).ToList();
                    ViewBag.AllSpells = _context.Spells;
                    ViewBag.DefaultTags = _context.Tags;
                }
                return View("EditFrilancer", model);
            }
            return View("EditClient", (EditClientViewModel)model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditClient(EditClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.UserName = model.Login;
                    user.Email = model.Email;
                    user.NickName = model.NickName;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditFrilancer(EditFrilancerModel model)
        {
            if (model.Tags.Count == 0 || model.Spells.Count == 0)
            {
                ModelState.AddModelError("", "Вы должны выбрать хотя бы одно направление и указать хотя бы один ваш навык");
            }
            else if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.UserName = model.Login;
                    user.Email = model.Email;
                    user.NickName = model.NickName;
                    var result = await _userManager.UpdateAsync(user);

                    var newTags = model.Tags.Where(x => !_context.Tags.Select(x => x.TagID).Contains(x)).Select(x => new Tag { TagID = x });
                    await _context.Tags.AddRangeAsync(newTags);
                    await _context.SaveChangesAsync();

                    var extraInfo = await _context.Frilancers.Where(x => x.UserId == user.Id).Include(x => x.Tags).Include(x => x.Spells).FirstAsync();
                    var addedSpels = model.Spells.Where(x => !extraInfo.Spells.Select(x => x.SpellID).Contains(x)).Select(x => new Spell { SpellID = x }).ToList();
                    _context.AttachRange(addedSpels);
                    var addedTags = model.Tags.Where(x => !extraInfo.Tags.Select(x => x.TagID).Contains(x)).Select(x => new Tag { TagID = x }).ToList();
                    _context.AttachRange(addedTags);
                    for (int i = 0; i < extraInfo.Tags.Count; i++)
                    {
                        if (!model.Tags.Contains(extraInfo.Tags[i].TagID))
                            extraInfo.Tags.RemoveAt(i);
                    }
                    extraInfo.Tags.AddRange(addedTags);
                    for (int i = 0; i < extraInfo.Spells.Count; i++)
                    {
                        if (!model.Spells.Contains(extraInfo.Spells[i].SpellID))
                        {
                            extraInfo.Spells.RemoveAt(i);
                        }
                    }
                    extraInfo.Spells.AddRange(addedSpels);
                    extraInfo.Resume = model.Description;

                    _context.Frilancers.Update(extraInfo);
                    await _context.SaveChangesAsync();
                    if (result.Succeeded)
                    {
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
            }
            ViewBag.AllSpells = _context.Spells;
            ViewBag.DefaultTags = _context.Tags;
            return View(model);
        }
    }
}