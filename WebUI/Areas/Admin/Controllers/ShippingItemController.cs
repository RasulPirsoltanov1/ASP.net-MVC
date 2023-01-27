using Core.Entities;
using DataAccess.Contexts;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingItemController : Controller
    {
        private IShippingItemRepository _repository;

        public ShippingItemController(IShippingItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var a = await _repository.GetAllAsync();
            return View(a);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var model =await _repository.GetAsync(id);
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
            await _repository.CreateAsync(item);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var dbItem = await _repository.GetAsync(id);
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
            var updateItem =await _repository.GetAsync(id);
            if(updateItem is null)
            {
                return NotFound();
            }
            updateItem.Id = id;
            updateItem.Description=item.Description;
            updateItem.Title = item.Title;
            updateItem.Image=item.Image;
            _repository.Update(updateItem);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var dbItem = await _repository.GetAsync(id);
            if (dbItem is null)
            {
                return NotFound();
            }
            _repository.Delete(dbItem);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
