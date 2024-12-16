using System.ComponentModel.DataAnnotations;

namespace Webbandothethao.Models
{
    public class tblPayment
    {
        [Key]
        public int id { get; set; }
        public string? paymentMethod { get; set; }
        public string? status { get; set; }
    }
}
