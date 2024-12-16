using Microsoft.AspNetCore.Mvc;
using Webbandothethao.Models;

namespace Webbandothethao.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
