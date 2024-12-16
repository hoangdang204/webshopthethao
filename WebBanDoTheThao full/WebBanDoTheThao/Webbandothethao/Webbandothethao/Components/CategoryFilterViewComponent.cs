using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbandothethao.Models;

namespace Webbandothethao.Components
{
    [ViewComponent(Name = "CategoryFilter")]
    public class CategoryFilterViewComponent : ViewComponent
    {
        private readonly DataContext _context;

        public CategoryFilterViewComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy danh sách các category đang hoạt động
            var categories = await _context.tblCategories
                                           .Where(c => c.IsActive == true)
                                           .OrderBy(c => c.name)
                                           .ToListAsync();

            return View("Default", categories);
        }
    }
}
