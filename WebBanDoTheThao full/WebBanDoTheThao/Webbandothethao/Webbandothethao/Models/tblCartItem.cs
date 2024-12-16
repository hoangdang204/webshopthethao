using System.ComponentModel.DataAnnotations;

namespace Webbandothethao.Models
{
    public class tblCartItem
    {
        [Key]
        public int id { get; set; }
        public int cartid { get; set; }
        public int productid { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }

        public tblCart? Cart { get; set; }
        public tblProduct? Product { get; set; }
    }
}
