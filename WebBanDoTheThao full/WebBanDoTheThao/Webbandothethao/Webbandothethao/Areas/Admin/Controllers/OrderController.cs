using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using Webbandothethao.Models;

namespace Webbandothethao.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }
		public IActionResult Display(int page = 1, int pageSize = 3)
		{
			var totalRecords = _context.tblOrders.Count();

			// Truy vấn lấy danh sách sản phẩm
			var orders = _context.tblOrders.Include(p => p.OrderDetails)
				.OrderBy(a => a.id)
				.Skip((page - 1) * pageSize)  // Bỏ qua số bản ghi trước trang hiện tại
				.Take(pageSize)               // Lấy số bản ghi theo pageSize
				.ToList();

			// Tính tổng số trang
			int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

			// Gửi thông tin phân trang tới View
			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = totalPages;
            ViewBag.OrderDetails = orders.SelectMany(o => o.OrderDetails ?? new List<tblOrderDetail>()).ToList();

			return View(orders);
		}

[HttpGet]
public IActionResult OrderDetail(int orderId)
{
    // Tìm đơn hàng cùng với chi tiết của nó
    var order = _context.tblOrders
                        .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                        .FirstOrDefault(o => o.id == orderId);

    if (order == null)
    {
        return NotFound("Đơn hàng không tồn tại.");
    }

    return View(order);
}



[HttpGet]
public IActionResult EditOrder(int orderId)
{
    // Tìm đơn hàng cùng với chi tiết của nó
    var order = _context.tblOrders
                        .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                        .FirstOrDefault(o => o.id == orderId);

    if (order == null)
    {
        return NotFound("Đơn hàng không tồn tại.");
    }

    // Tạo view model với đơn hàng và chi tiết đơn hàng
    var viewModel = new EditOrderViewModel
    {
        Id = order.id,
        Status = order.status,
        OrderDetails = order.OrderDetails?.Select(od => new EditOrderDetailViewModel
        {
            OrderDetailId = od.id,
            ProductId = od.productId,
            Quantity = od.quantity,
            Price = od.price
        }).ToList() ?? new List<EditOrderDetailViewModel>()
    };

    return View(viewModel);
}

[HttpPost]
public IActionResult EditOrder(EditOrderViewModel model)
{
    var order = _context.tblOrders
                        .Include(o => o.OrderDetails)
                        .FirstOrDefault(o => o.id == model.Id);

    if (order == null)
    {
        return NotFound("Đơn hàng không tồn tại.");
    }

    order.status = model.Status;

    foreach (var detail in model.OrderDetails)
    {
        var orderDetail = order.OrderDetails?.FirstOrDefault(od => od.id == detail.OrderDetailId);
        if (orderDetail != null)
        {
            orderDetail.quantity = detail.Quantity;
            orderDetail.price = detail.Price;
        }
    }

    // Lưu thay đổi
    var changes = _context.SaveChanges(); // Trả về số lượng bản ghi bị thay đổi
    Console.WriteLine($"Số bản ghi thay đổi: {changes}");

    // Kiểm tra trạng thái đơn hàng
    Console.WriteLine($"Trạng thái đơn hàng cập nhật: {order.status}");
    
    foreach (var detail in order.OrderDetails)
    {
        Console.WriteLine($"Chi tiết đơn hàng: ID = {detail.id}, Quantity = {detail.quantity}, Price = {detail.price}");
    }

    return RedirectToAction("Display","Order");
}





[HttpPost]
public IActionResult DeleteOrder(int id)
{
    var order = _context.tblOrders
        .Include(o => o.OrderDetails)
        .FirstOrDefault(o => o.id == id);

    if (order == null)
    {
        return NotFound();
    }

    _context.tblOrderDetails.RemoveRange(order.OrderDetails);
    _context.tblOrders.Remove(order);
    _context.SaveChanges();

    return RedirectToAction("AdminOrderList");
}


	}
}
