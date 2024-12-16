using Microsoft.AspNetCore.Mvc;
using Webbandothethao.Models;

namespace Webbandothethao.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly DataContext _context;
		public CategoryController(DataContext context)
		{
			_context = context;
		}
		public IActionResult Display(int page = 1, int pageSize = 3)
		{
			var totalRecords = _context.tblCategories.Count();
			var categories = _context.tblCategories
				.OrderBy(a => a.id)
				.Skip((page - 1) * pageSize) // Bỏ qua số bản ghi trước trang hiện tại
				.Take(pageSize) // Lấy số bản ghi theo pageSize
				.ToList();

			// Tính tổng số trang
			int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = totalPages;

			return View(categories);
		}
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(tblCategory model)
        {
            if (ModelState.IsValid)
            {
                _context.tblCategories.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Display");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var menu = _context.tblCategories.Find(id);
            return View(menu);
        }

        [HttpPost]
        public IActionResult Edit(tblCategory model)
        {
            if (ModelState.IsValid)
            {
                _context.tblCategories.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Display");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delmenu = _context.tblCategories.Find(id);
            if (delmenu != null)
            {
                _context.tblCategories.Remove(delmenu);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }

        [HttpPost]
        public IActionResult UpdateIsShow(int id, bool isActive)
        {
            try
            {
                var menuItem = _context.tblCategories.Find(id);

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
    }
		
}
