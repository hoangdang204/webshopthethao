using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Webbandothethao.Models;

namespace Webbandothethao.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;

        private readonly DataContext _context;

        public ContactController(ILogger<ContactController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Display()
        {
           
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
