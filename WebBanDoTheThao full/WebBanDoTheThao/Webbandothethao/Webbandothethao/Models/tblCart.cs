using System.ComponentModel.DataAnnotations;

namespace Webbandothethao.Models
{
    public class tblCart
    {
        [Key]
        public int id { get; set; }
        public int userid { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<tblCartItem>? CartItems { get; set; }
    }

}
