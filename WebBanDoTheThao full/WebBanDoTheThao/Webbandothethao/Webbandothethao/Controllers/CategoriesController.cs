using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbandothethao.Models;

namespace Webbandothethao.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Display(int? category_id)
        {
            var products = _context.tblProducts.AsQueryable();

            if (category_id.HasValue)
            {
                products = products.Where(p => p.categoryid == category_id.Value);
              
            }

            ViewBag.Categories = _context.tblCategories.ToList();
            return View(products.ToList());
        }


    }
}
