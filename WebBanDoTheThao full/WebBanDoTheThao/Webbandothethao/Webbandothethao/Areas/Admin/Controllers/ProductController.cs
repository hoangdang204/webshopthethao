using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using Webbandothethao.Models;

namespace Webbandothethao.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }
		public IActionResult Display(int page = 1, int pageSize = 3)
		{
			var totalRecords = _context.tblProducts.Count();

			// Truy vấn lấy danh sách sản phẩm
			var blogs = _context.tblProducts.Include(p => p.category)
				.OrderBy(a => a.id)
				.Skip((page - 1) * pageSize)  // Bỏ qua số bản ghi trước trang hiện tại
				.Take(pageSize)               // Lấy số bản ghi theo pageSize
				.ToList();

			// Tính tổng số trang
			int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

			// Gửi thông tin phân trang tới View
			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = totalPages;

			return View(blogs);
		}


		[HttpGet]
        public IActionResult Create()
		{
			ViewBag.Category = _context.tblCategories
			 .Where(c => c.IsActive == true)
			 .Select(c => new SelectListItem
			 {
				 Value = c.id.ToString(),
				 Text = c.name
			 }).ToList();

			return View();
		}

        [HttpPost]
		public IActionResult Create(tblProduct model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					// Create the Article object with user-specific data
					var product = new tblProduct
					{
						name = model.name,
						categoryid = model.categoryid,
						price = model.price,
						image_url = model.image_url,
						quantity = model.quantity,
						description = model.description,
				
					};

					// Save the new article to the database
					_context.tblProducts.Add(product);
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
			ViewBag.Category = _context.tblCategories
			 .Where(c => c.IsActive == true)
			 .Select(c => new SelectListItem
			 {
				 Value = c.id.ToString(),
				 Text = c.name
			 }).ToList();

			// If the model state is invalid, return the model back to the view with validation messages
			return View(model);
		}
		public IActionResult Edit(int id)
		{
			var product = _context.tblProducts.Find(id);
			ViewBag.Category = _context.tblCategories
			 .Where(c => c.IsActive == true)
			 .Select(c => new SelectListItem
			 {
				 Value = c.id.ToString(),
				 Text = c.name
			 }).ToList();

			return View(product);
		}

		[HttpPost]
		public IActionResult Edit(tblProduct model)
		{
			if (ModelState.IsValid)
			{
				var product = _context.tblProducts.Find(model.id);
				if (product != null)
				{
					product.name = model.name;
					product.categoryid = model.categoryid;
					product.image_url = model.image_url;
					product.price = model.price;
					product.quantity = model.quantity;
					product.description = model.description;
					product.IsActive = model.IsActive;

					_context.Update(product);
					_context.SaveChanges();
				}
				return RedirectToAction("Display");
			}
			return View(model);
		}
		public IActionResult Delete(int id)
		{
			var delmenu = _context.tblProducts.Find(id);
			if (delmenu != null)
			{
				_context.tblProducts.Remove(delmenu);
				_context.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false });

		}
		public IActionResult UpdateIsShow(int id, bool isActive)
		{
			try
			{
				var menuItem = _context.tblProducts.Find(id);

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
