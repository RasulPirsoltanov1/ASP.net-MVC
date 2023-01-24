using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var ShippingItems = _context.ShippingItems;
            HomeViewModel model = new()
            {
                ShippingItems = _context.ShippingItems,
                SlideItems=_context.SlideItems
            };
            return View(model);
        }
    }
}
