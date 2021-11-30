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
                ModelState.AddModelError("", "Вы должны выбрать хотя бы одно направление и указать хотя бы один требуемый навык");
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
        [HttpGet]
        public async Task<IActionResult> EditOrder(int id)
        {
            ViewBag.Spells = new SelectList(_context.Spells, "SpellID", "SpellID");
            ViewBag.DefaultTags = _context.Tags;
            var order = await _context.Orders.FindAsync(id);
            _context.Entry(order).Collection(x => x.Tags).Load();
            var model = new EditOrderViewModel()
            {
                ID = id,
                Name = order.Name,
                Description = order.Description,
                Price = order.Price,
                TypePrice = order.TypePrice,
                OrderType = order.OrderType,
                Tags = order.Tags.Select(x => x.TagID).ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditOrder(EditOrderViewModel model)
        {
            if (model.OrderType == null || model.Tags.Count == 0)
            {
                ModelState.AddModelError("", "Вы должны выбрать хотя бы одно направление и указать хотя бы один требуемый навык");
            }
            else if (ModelState.IsValid)
            {
                var newTags = model.Tags.Where(x => !_context.Tags.Select(x => x.TagID).Contains(x)).Select(x => new Tag { TagID = x });
                await _context.Tags.AddRangeAsync(newTags);
                await _context.SaveChangesAsync();

                var order = await _context.Orders.FindAsync(model.ID);
                _context.Entry(order).Collection(x => x.Tags).Load();
                order.Name = model.Name;
                order.Description = model.Description;
                order.Price = model.Price;
                order.TypePrice = model.TypePrice;
                order.OrderType = model.OrderType;
                order.CreateDate = DateTime.Now;

                var addedTags = model.Tags.Where(x => !order.Tags.Select(x => x.TagID).Contains(x)).Select(x => new Tag { TagID = x }).ToList();
                _context.AttachRange(addedTags);
                for (int i = 0; i < order.Tags.Count; i++)
                {
                    if (!model.Tags.Contains(order.Tags[i].TagID))
                        order.Tags.RemoveAt(i);
                }
                order.Tags.AddRange(addedTags);

                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Spells = new SelectList(_context.Spells, "SpellID", "SpellID");
            ViewBag.DefaultTags = _context.Tags;
            return View(model);
        }
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderList");
        }
    }
}