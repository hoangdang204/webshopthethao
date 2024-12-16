
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq;
using System.Threading.Tasks;
using Webbandothethao.Models;
using System.Security.Claims;
using QRCoder;


namespace Webbandothethao.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _context;

        private readonly ILogger<CartController> _logger;

        public CartController(DataContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

  public IActionResult Display()
{
            var userId = HttpContext.User.FindFirst("ID")?.Value;
            if (userId == null) return NotFound();

    var cartItems = _context.tblCartItems
        .Include(c => c.Product)
        .Where(c => c.Cart.userid == int.Parse(userId))
        .ToList();

    return View(cartItems);
}

[HttpGet]
public IActionResult GetCartCount()
{
    try
    {
         var userId = HttpContext.User.FindFirst("ID")?.Value;
            if (userId == null) return NotFound();
        var cartCount = _context.tblCartItems
            .Where(c => c.Cart.userid == int.Parse(userId))
            .Sum(c => c.quantity);

        return Json(cartCount); // Đảm bảo chỉ trả về số
    }
    catch (Exception ex)
    {
        return Json(0); // Trả về 0 nếu xảy ra lỗi
    }
}





[HttpPost]
public JsonResult AddToCart(int productId, int quantity)
{
    var userId = HttpContext.User.FindFirst("ID")?.Value;
    if (string.IsNullOrEmpty(userId))
    {
        return Json(new { success = false, message = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng!" });
    }

    int id = int.Parse(userId);
    try
    {
        // Kiểm tra giỏ hàng hiện tại của người dùng
        var cart = _context.tblCarts.FirstOrDefault(c => c.userid == id);
        if (cart == null)
        {
            // Nếu chưa có giỏ hàng, tạo mới
            cart = new tblCart
            {
                userid = id,
                CreatedAt = DateTime.Now
            };
            _context.tblCarts.Add(cart);
            _context.SaveChanges(); // Lưu để có `cart.id`
        }

        // Kiểm tra xem sản phẩm có tồn tại không
        var product = _context.tblProducts.FirstOrDefault(p => p.id == productId);
        if (product == null)
        {
            return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
        }

        // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
        var cartItem = _context.tblCartItems.FirstOrDefault(c => c.productid == productId && c.cartid == cart.id);
        if (cartItem != null)
        {
            // Nếu có, tăng số lượng
            cartItem.quantity += quantity;
        }
        else
        {
            // Nếu chưa có, thêm mới sản phẩm vào giỏ hàng
            cartItem = new tblCartItem
            {
                productid = productId,
                quantity = quantity,
                price = product.price, // Lưu giá tại thời điểm thêm
                cartid = cart.id
            };
            _context.tblCartItems.Add(cartItem);
        }

        // Lưu thay đổi vào cơ sở dữ liệu
        _context.SaveChanges();

    return Json(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng!"});
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = "Lỗi: " + ex.Message });
    }
}   


[HttpPost]
public JsonResult UpdateQuantity(int productId, int quantity)
{
    var userId = HttpContext.User.FindFirst("ID")?.Value;
    if (string.IsNullOrEmpty(userId))
    {
        return Json(new { success = false, message = "Bạn cần đăng nhập để cập nhật giỏ hàng!" });
    }

    int id = int.Parse(userId);
    try
    {
        // Kiểm tra giỏ hàng hiện tại của người dùng
        var cart = _context.tblCarts.FirstOrDefault(c => c.userid == id);
        if (cart == null)
        {
            return Json(new { success = false, message = "Không tìm thấy giỏ hàng!" });
        }

        // Kiểm tra xem sản phẩm có trong giỏ hàng hay không
        var cartItem = _context.tblCartItems.FirstOrDefault(c => c.productid == productId && c.cartid == cart.id);
        if (cartItem == null)
        {
            return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng!" });
        }

        if (quantity > 0)
        {
            // Cập nhật số lượng sản phẩm
            cartItem.quantity = quantity;
        }
        else
        {
            // Xóa sản phẩm khỏi giỏ hàng nếu số lượng <= 0
            _context.tblCartItems.Remove(cartItem);
        }

        // Lưu thay đổi vào cơ sở dữ liệu
        _context.SaveChanges();

        return Json(new { success = true, message = "Cập nhật số lượng sản phẩm thành công!" });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = "Lỗi: " + ex.Message });
    }
}

[HttpPost]
public JsonResult RemoveFromCart(int productId)
{
    var userId = HttpContext.User.FindFirst("ID")?.Value;
    if (string.IsNullOrEmpty(userId))
    {
        return Json(new { success = false, message = "Bạn cần đăng nhập để xóa sản phẩm khỏi giỏ hàng!" });
    }

    int id = int.Parse(userId);
    try
    {
        // Lấy giỏ hàng của người dùng
        var cart = _context.tblCarts.FirstOrDefault(c => c.userid == id);
        if (cart == null)
        {
            return Json(new { success = false, message = "Giỏ hàng không tồn tại!" });
        }

        // Tìm sản phẩm trong giỏ hàng
        var cartItem = _context.tblCartItems.FirstOrDefault(c => c.productid == productId && c.cartid == cart.id);
        if (cartItem == null)
        {
            return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng!" });
        }

        // Xóa sản phẩm khỏi giỏ hàng
        _context.tblCartItems.Remove(cartItem);
        _context.SaveChanges();

        return Json(new { success = true, message = "Đã xóa sản phẩm khỏi giỏ hàng!" });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = "Lỗi: " + ex.Message });
    }
}


[HttpGet]
public IActionResult Checkout()
{
    // Lấy ID người dùng từ context
    var userId = HttpContext.User.FindFirst("ID")?.Value;
    if (string.IsNullOrEmpty(userId))
    {
        return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
    }

    int id = int.Parse(userId);


    // Lấy danh sách phương thức thanh toán từ cơ sở dữ liệu
    var paymentMethods = _context.tblPayments.ToList();

    // Truyền phương thức thanh toán vào view
    ViewBag.PaymentMethods = paymentMethods;

    return View();
}


[HttpPost]
public IActionResult Checkout(tblAddress newAddress, int PaymentMethod)
{
    var userId = HttpContext.User.FindFirst("ID")?.Value;

    if (string.IsNullOrEmpty(userId))
    {
        return RedirectToAction("Login", "Account");
    }

    int id = int.Parse(userId);

    using var transaction = _context.Database.BeginTransaction();
    try
    {
        // Xác minh thông tin giỏ hàng
        var cart = _context.tblCarts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefault(c => c.userid == id);

        if (cart == null || !cart.CartItems.Any())
        {
            return RedirectToAction("Display", "Cart");
        }

        // Lưu địa chỉ nếu người dùng nhập
       if (newAddress != null)
{
    if (string.IsNullOrEmpty(newAddress.FullName) || string.IsNullOrEmpty(newAddress.Address) ||
        string.IsNullOrEmpty(newAddress.City) || string.IsNullOrEmpty(newAddress.Phone))
    {
        return View("Error", new ErrorViewModel
        {
            Message = "Bạn cần nhập đầy đủ thông tin địa chỉ giao hàng!"
        });
    }

    newAddress.UserId = id;

    _context.tblAddresses.Add(newAddress);
    _context.SaveChanges();
}
else
{
    return View("Error", new ErrorViewModel
    {
        Message = "Bạn cần nhập địa chỉ giao hàng!"
    });
}

        // Xác minh phương thức thanh toán
        var selectedPayment = _context.tblPayments.FirstOrDefault(p => p.id == PaymentMethod);
        if (selectedPayment == null)
        {
            return View("Error", new ErrorViewModel
            {
                Message = "Phương thức thanh toán không hợp lệ!"
            });
        }

        // Lưu đơn hàng
        var order = new tblOrder
        {
            userId = id,
            createdAt = DateTime.Now,
            AddressId = newAddress.Id,
            totalPrice = cart.CartItems.Sum(ci => ci.price * ci.quantity),
            status = "Đang xử lý",
            paymentid = PaymentMethod,
            isPaid = false
        };

        _context.tblOrders.Add(order);
        _context.SaveChanges();

        // Lưu chi tiết đơn hàng
        foreach (var item in cart.CartItems)
        {
            var orderDetail = new tblOrderDetail
            {
                orderId = order.id,
                productId = item.productid,
                quantity = item.quantity,
                price = item.price
            };

            _context.tblOrderDetails.Add(orderDetail);
        }

        _context.SaveChanges();

        // Xóa sản phẩm trong giỏ hàng
        _context.tblCartItems.RemoveRange(cart.CartItems);
        _context.tblCarts.Remove(cart);
        _context.SaveChanges();

        // Commit transaction
        transaction.Commit();

        return View("ConfirmPayment", order);
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        _logger.LogError($"Lỗi trong quá trình thanh toán: {ex.Message} - {ex.StackTrace}", ex);

        return View("Error", new ErrorViewModel
        {
            Message = "Đã xảy ra lỗi trong quá trình thanh toán. Vui lòng thử lại!"
        });
    }
}

[HttpPost]
public IActionResult CreateAddress(tblAddress newAddress)
{
    if (ModelState.IsValid)
    {
        var userId = HttpContext.User.FindFirst("ID")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        newAddress.UserId = int.Parse(userId);
        newAddress.IsDefault = false;

        _context.tblAddresses.Add(newAddress);
        _context.SaveChanges();
        return RedirectToAction("Checkout");
    }
    return View("Checkout");
}

[HttpGet]
public IActionResult GetQRCode()
{
    try
    {
        string successUrl = Url.Action("ConfirmPayment", "Cart", null, Request.Scheme);
        var qrCode = GenerateQRCode(successUrl); // Hàm GenerateQRCode trả về Base64 của mã QR

        return Json(new { qrCodeBase64 = qrCode });
    }
    catch (Exception ex)
    {
        _logger.LogError($"Lỗi khi tạo mã QR: {ex.Message}", ex);
        return Json(new { qrCodeBase64 = "" });
    }
}


private string GenerateQRCode(string url)
{
    if (string.IsNullOrEmpty(url))
    {
        throw new ArgumentException("URL không hợp lệ");
    }

    using (var qrGenerator = new QRCoder.QRCodeGenerator())
    {
        var qrCodeData = qrGenerator.CreateQrCode(url, QRCoder.QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCoder.QRCode(qrCodeData);
        using (var bitmap = qrCode.GetGraphic(20))
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                var base64String = Convert.ToBase64String(stream.ToArray());
                return $"data:image/png;base64,{base64String}";
            }
        }
    }
}


}

}
