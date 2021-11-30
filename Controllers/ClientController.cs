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
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private FrilanceDbContext _context;
        public ClientController(FrilanceDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult CreateOrder()
        {
            ViewBag.Spells = new SelectList(_context.Spells, "SpellID", "SpellID");
            ViewBag.DefaultTags = _context.Tags;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model)
        {
            if (model.OrderType == null || model.Tags.Count == 0)
            {
                ModelState.AddModelError("", "Вы должны выбрать хотя бы одно направление и указать хотя бы один ваш навык");
            }
            else if (ModelState.IsValid)
            {
                Order order = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    TypePrice = model.TypePrice,
                    OrderType = model.OrderType,
                    CreateDate = DateTime.Now
                };
                var newTags = model.Tags.Where(x => !_context.Tags.Select(x => x.TagID).Contains(x)).Select(x => new Tag { TagID = x });
                await _context.Tags.AddRangeAsync(newTags);
                await _context.SaveChangesAsync();

                order.Tags = await _context.Tags.Where(x => model.Tags.Contains(x.TagID)).ToListAsync();
                order.User = await _context.Users.FirstAsync(x => x.UserName == User.Identity.Name);

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Spells = new SelectList(_context.Spells, "SpellID", "SpellID");
            ViewBag.DefaultTags = _context.Tags;
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> OrderList()
        {
            var user = await _context.Users.FirstAsync(x => x.UserName == User.Identity.Name);
            var model = await _context.Orders.Where(x => x.UserId == user.Id).Include(x => x.Tags).ToListAsync();
            return View(model);
        }
    }
}