using System.ComponentModel.DataAnnotations;

namespace Webbandothethao.Models
{
    public class tblOrder
    {
        [Key]
        public int id { get; set; }
        public int userId { get; set; }
        public decimal totalPrice { get; set; }
        public string? status { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        public DateTime updatedAt { get; set; } = DateTime.Now;
        public int AddressId { get; set; }
         public int paymentid { get; set; } // "COD", "PayPal", "VNPay", etc.
        public bool isPaid { get; set; }
        public tblUser? user { get; set; }

         public tblPayment? Payment { get; set; }
        public ICollection<tblOrderDetail>? OrderDetails { get; set; }
    }
}
