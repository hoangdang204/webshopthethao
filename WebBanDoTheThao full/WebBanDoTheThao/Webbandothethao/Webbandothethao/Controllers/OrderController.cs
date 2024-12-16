using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Webbandothethao.Models;
using Webbandothethao.ViewModels;
namespace Webbandothethao.Controllers
{
   public class OrderController : Controller
{
    private readonly DataContext _context;

    public OrderController(DataContext context)
    {
        _context = context;
    }

[HttpGet]
public IActionResult UserOrders()
{
    var userId = HttpContext.User.FindFirst("ID")?.Value;
    if (string.IsNullOrEmpty(userId))
    {
        return RedirectToAction("Login", "Account");
    }

    int id = int.Parse(userId);

    // Lấy danh sách đơn hàng của người dùng
    var orders = _context.tblOrders
        .Where(o => o.userId == id)
        .OrderByDescending(o => o.createdAt)
        .ToList();

    // Nếu không có đơn hàng, trả về danh sách rỗng
    if (!orders.Any())
    {
        var emptyViewModel = new UserOrdersViewModel
        {
            Orders = new List<tblOrder>(), // Trả về danh sách rỗng
            OrderDetails = new List<OrderDetailViewModel>()
        };
        return View(emptyViewModel);
    }

    // Lấy chi tiết đơn hàng
    var orderDetails = _context.tblOrderDetails
        .Where(od => orders.Select(o => o.id).Contains(od.orderId))
        .Select(od => new OrderDetailViewModel
        {
            Name = od.Product.name,
            ImageUrl = od.Product.image_url,
            Quantity = od.quantity,
            Price = od.price,
            Total = od.quantity * od.price
        })
        .ToList();

    var viewModelWithOrders = new UserOrdersViewModel
    {
        Orders = orders,  // orders là một danh sách, nên không cần phải thay đổi.
        OrderDetails = orderDetails
    };

    return View(viewModelWithOrders);
}

[HttpGet]
public IActionResult OrderDetails(int id)
{
    var userId = HttpContext.User.FindFirst("ID")?.Value;
    if (string.IsNullOrEmpty(userId))
    {
        return RedirectToAction("Login", "Account");
    }

    int userIdInt = int.Parse(userId);

    // Lấy thông tin đơn hàng
    var order = _context.tblOrders.FirstOrDefault(o => o.id == id && o.userId == userIdInt);
    if (order == null)
    {
        return NotFound("Đơn hàng không tồn tại hoặc không thuộc về bạn.");
    }

    // Lấy chi tiết đơn hàng
    var orderDetails = _context.tblOrderDetails
        .Where(od => od.orderId == id)
        .Select(od => new OrderDetailViewModel
        {
            Name = od.Product.name,
            ImageUrl = od.Product.image_url,
            Quantity = od.quantity,
            Price = od.price,
            Total = od.quantity * od.price
        }).ToList();

    // ViewModel chứa cả đơn hàng và chi tiết đơn hàng
    var viewModel = new OrderDetailViewModel
    {
        Orders = order,  // Chỉ trả về 1 đơn hàng
        OrderDetails = orderDetails  // Chi tiết của đơn hàng
    };

    return View(viewModel);
}

}

}
