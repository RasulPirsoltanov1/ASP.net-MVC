using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingItemController : Controller
    {
        private AppDbContext _context;

        public ShippingItemController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.ShippingItems);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var model =await _context.ShippingItems.FindAsync(id);
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShippingItem item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            await _context.ShippingItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var dbItem =await _context.ShippingItems.FindAsync(id);
            if(dbItem is null)
            {
                return NotFound();
            }
            return View(dbItem);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,ShippingItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            var updateItem =await _context.ShippingItems.FindAsync(id);
            if(updateItem is null)
            {
                return NotFound();
            }
            updateItem.Id = id;
            updateItem.Description=item.Description;
            updateItem.Title = item.Title;
            updateItem.Image=item.Image;
            _context.ShippingItems.Update(updateItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var dbItem = await _context.ShippingItems.FindAsync(id);
            if (dbItem is null)
            {
                return NotFound();
            }
            _context.ShippingItems.Remove(dbItem);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
