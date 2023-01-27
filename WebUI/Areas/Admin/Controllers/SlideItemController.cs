using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.ViewModels.Slider;
using WebUI.Utilities;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideItemController : Controller
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;
        public SlideItemController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.SlideItems);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var item = await _context.SlideItems.FindAsync(id);
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
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            if (item.Photo == null)
            {
                ModelState.AddModelError("Photo", "Select Photo");
                return View(item);
            }
            if (item.Photo.CheckFileSize(100))
            {
                ModelState.AddModelError("Photo", "Image size must be less than 3kb");
                return View(item);
            }
            if (!item.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File must be image type");
                return View(item);
            }
            var fileName=await item.Photo.SaveFileAsync(_env.WebRootPath, "wwwroot","assets", "images", "website-images");
            SlideItem slideItem = new SlideItem()
            {
                Title = item.Title,
                Photo = fileName,
                Description = item.Description,
                Offer = item.Offer,
            };
            await _context.AddAsync(slideItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
