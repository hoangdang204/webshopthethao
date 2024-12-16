using Microsoft.AspNetCore.Mvc;
using Webbandothethao.Models;

namespace Webbandothethao.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly DataContext _context;
        public MenuController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Display(int page = 1, int pageSize = 3)
        {
            var totalRecords = _context.tblMenus.Count();
            var menu = _context.tblMenus
                .OrderBy(a => a.MenuID)
                .Skip((page - 1) * pageSize) // Bỏ qua số bản ghi trước trang hiện tại
                .Take(pageSize) // Lấy số bản ghi theo pageSize
                .ToList();

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(menu);
        }
        [HttpGet]
        public IActionResult Create()
        {
           return View();
        }

        [HttpPost]
        public IActionResult Create(tblMenu model)
        {
			if (ModelState.IsValid)
			{
				_context.tblMenus.Add(model);
				_context.SaveChanges();
				return RedirectToAction("Display");
			}
			return View(model);
		}

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var menu = _context.tblMenus.Find(id);
            return View(menu);
        }

        [HttpPost]
        public IActionResult Edit(tblMenu model)
        {
            if (ModelState.IsValid)
            {
                _context.tblMenus.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Display");
            }
            return View(model);
        }
         [HttpPost]
        public IActionResult Delete(int id)
        {
            var delmenu = _context.tblMenus.Find(id);
            if (delmenu != null)
            {
                _context.tblMenus.Remove(delmenu);
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
				var menuItem = _context.tblMenus.Find(id);

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
