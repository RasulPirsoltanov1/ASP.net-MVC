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
            HttpContext.Session.SetString("name", "The Doctor");
            Response.Cookies.Append("surname", "Filankesov");
            var ShippingItems = _context.ShippingItems;
            HomeViewModel model = new()
            {
                ShippingItems = _context.ShippingItems,
                SlideItems=_context.SlideItems
            };
            return View(model);
        }
        public IActionResult test()
        {
            var s = HttpContext.Session.GetString("name");
            var s2 = Request.Cookies["surname"];
            return Json(s+" +" +s2);
        }
    }
}
