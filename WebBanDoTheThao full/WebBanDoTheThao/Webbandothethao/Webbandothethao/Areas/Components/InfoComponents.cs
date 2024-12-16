using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbandothethao.Models;

namespace Webbandothethao.Areas.Admin.Components
{
    [ViewComponent(Name = "Info")]
    public class InfoComponents : ViewComponent
    {
        private readonly DataContext _context;

        public InfoComponents(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Accessing user ID from HttpContext.User
            var userId = HttpContext.User.FindFirst("ID")?.Value;


            if (userId == null)
            {

                // Handle case when user ID is not found, if needed
                return View("Default", new List<tblUser>()); // Empty list if no user is found
            }
            int id = int.Parse(userId);
            var listofMenu = await _context.tblUsers.Where(e => e.id == id).ToListAsync();

            return View("Default", listofMenu);
        }
    }
}
