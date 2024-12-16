using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbandothethao.Models;

namespace Webbandothethao.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AccountController : Controller
	{
		private readonly DataContext _context;

		public AccountController(DataContext context)
		{
			_context = context;
		}
		public IActionResult Display(int page = 1, int pageSize = 3)
		{
			var totalRecords = _context.tblUsers.Count();

			// Truy vấn lấy danh sách sản phẩm
			var user = _context.tblUsers
				.Skip((page - 1) * pageSize)  // Bỏ qua số bản ghi trước trang hiện tại
				.Take(pageSize)               // Lấy số bản ghi theo pageSize
				.ToList();

			// Tính tổng số trang
			int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

			// Gửi thông tin phân trang tới View
			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = totalPages;

			return View(user);
		}

		public IActionResult Create()
		{
			return View();
		}


		[HttpPost]
		public IActionResult Create(tblUser model)
		{
			if (ModelState.IsValid)
			{
				_context.tblUsers.Add(model);
				_context.SaveChanges();
				return RedirectToAction("Display");
			}
			return View(model);
		}


		public IActionResult Edit(int id)
		{
			var user = _context.tblUsers.Find(id);
			if (user == null) {
				return NotFound();
			}

			return View(user);
		}

		[HttpPost]
		public IActionResult Edit(tblUser model)
		{
			if (ModelState.IsValid)
			{
				_context.tblUsers.Update(model);
				_context.SaveChanges();
				return RedirectToAction("Display");
			}
			return View(model);
		}
		public IActionResult Delete(int id)
		{
			var delmenu = _context.tblUsers.Find(id);
			if (delmenu != null)
			{
				_context.tblUsers.Remove(delmenu);
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
				var menuItem = _context.tblUsers.Find(id);

				if (menuItem != null)
				{
					menuItem.Status = isActive;
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
