using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbandothethao.Models;

namespace Webbandothethao.Components
{
    [ViewComponent(Name = "OrderView")]
    public class OrderComponent : ViewComponent
    {
        private readonly DataContext _context;

        public OrderComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy thông tin UserId từ HttpContext
            var userId = HttpContext.User.FindFirst("ID")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                // Nếu không có UserId, trả về danh sách rỗng
                return View("Default", new List<tblCartItem>());
            }

            int id;
            if (!int.TryParse(userId, out id))
            {
                // Nếu UserId không hợp lệ, trả về danh sách rỗng
                return View("Default", new List<tblCartItem>());
            }

            // Lấy giỏ hàng của người dùng
            var cart = await _context.tblCarts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.userid == id);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                // Nếu không có giỏ hàng hoặc giỏ hàng trống, trả về danh sách rỗng
                return View("Default", new List<tblCartItem>());
            }

            // Trả về View với danh sách sản phẩm trong giỏ
            return View("Default", cart.CartItems.ToList());
        }
    }
}
