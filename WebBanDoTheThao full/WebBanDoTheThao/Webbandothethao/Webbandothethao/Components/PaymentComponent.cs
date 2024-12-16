using Microsoft.AspNetCore.Mvc;
using Webbandothethao.Models;

namespace Webbandothethao.Components
{
    [ViewComponent(Name = "Payment")]
    public class PaymentComponent : ViewComponent
    {
        private readonly DataContext _context;

        public PaymentComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy danh sách các phương thức thanh toán
            var paymentMethods = await Task.Run(() => _context.tblPayments.ToList());

            return View("Default", paymentMethods);
        }
    }
}
