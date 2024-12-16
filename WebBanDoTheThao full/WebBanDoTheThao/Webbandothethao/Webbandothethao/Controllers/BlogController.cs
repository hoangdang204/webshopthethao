using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbandothethao.Models;
using Webbandothethao.Utilities;

namespace Webbandothethao.Controllers
{
    public class BlogController : Controller
    {
        private readonly DataContext _context;

        public BlogController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Display()
        {   
            var blogs = _context.tblBlogs.Where(b => b.IsActive == true).ToList();
            return View(blogs);
        }
        [Route("/Blog/post-{slug}-{id:long}.html", Name = "BlogDetail")]
        public IActionResult BlogDetail(long? id)
        {
            var blog = _context.tblBlogs.FirstOrDefault(b => b.id == id);
            if(blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

    }
}
