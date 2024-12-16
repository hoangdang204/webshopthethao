using System.ComponentModel.DataAnnotations;

namespace Webbandothethao.Models
{
    public class tblOrderDetail
    {
        [Key]
        public int id { get; set; }
        public int orderId { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }

        public tblOrder? Order { get; set; }
        public tblProduct? Product { get; set; }
    }
}
