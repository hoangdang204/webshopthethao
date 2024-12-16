using Microsoft.AspNetCore.Mvc;
using Webbandothethao.Models;

namespace Webbandothethao.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly DataContext _context;

        public BlogController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Display(int page = 1, int pageSize = 3)
        {
            var totalRecords = _context.tblBlogs.Count();
            var blogs = _context.tblBlogs
                .OrderBy(a => a.id)
                .Skip((page - 1) * pageSize) // Bỏ qua số bản ghi trước trang hiện tại
                .Take(pageSize) // Lấy số bản ghi theo pageSize
                .ToList();

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(blogs);
        }


        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(tblBlog model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                  

                    // Create the Article object with user-specific data
                    var article = new tblBlog
                    {
                        articletitle = model.articletitle,
                        content = model.content,
                        img = model.img,
                        IsActive = model.IsActive,
                        description = model.description,
                        created_at = DateTime.Now,
                    };

                    // Save the new article to the database
                    _context.tblBlogs.Add(article);
                    _context.SaveChanges();

                    // Redirect to a success or "read" page after saving
                    return RedirectToAction("Display");
                }
                catch (Exception ex)
                {
                    // Handle and log the exception if something goes wrong
                    ModelState.AddModelError("", $"An error occurred while creating the article:  {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            // If the model state is invalid, return the model back to the view with validation messages
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var blogs = _context.tblBlogs.Find(id);
            return View(blogs);
        }
        public IActionResult Edit( tblBlog model)
        {
            if (ModelState.IsValid)
            {
                var article = _context.tblBlogs.Find(model.id);
                if (article != null)
                {
                    article.articletitle = model.articletitle;
                    article.description = model.description;
                    article.img = model.img;
                    article.IsActive = model.IsActive;
                    article.content = model.content;
                    article.updated_at = model.updated_at;

                    _context.SaveChanges();
                }
                return RedirectToAction("Display");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delmenu = _context.tblBlogs.Find(id);
            if (delmenu != null)
            {
                _context.tblBlogs.Remove(delmenu);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }
        public IActionResult UpdateIsShow(int id, bool isActive)
        {
            try
            {
                var menuItem = _context.tblBlogs.Find(id);

                if (menuItem != null)
                {
                    menuItem.IsActive = isActive;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Update successful" });
                }

                return Json(new { success = false, message = "Menu item not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "/img/", upload.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await upload.CopyToAsync(stream);
                }

                // Trả về URL của file để hiển thị trong CKEditor
                return Json(new { url = "/img/" + upload.FileName });
            }

            return Json(new { error = "Không có file nào được chọn." });
        }

    }

}

