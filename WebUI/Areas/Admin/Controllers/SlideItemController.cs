using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.ViewModels.Slider;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideItemController : Controller
    {
        private readonly AppDbContext _context;

        public SlideItemController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.SlideItems);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var item =await _context.SlideItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SlideCreateVM item)
        {
            return Content(item.Photo.FileName);
        }
    }
}
