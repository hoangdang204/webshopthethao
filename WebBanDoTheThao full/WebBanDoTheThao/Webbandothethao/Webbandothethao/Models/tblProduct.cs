using System.ComponentModel.DataAnnotations;

namespace Webbandothethao.Models
{
    public class tblProduct
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public int categoryid { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public string? description { get; set; }
        public string? image_url { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

        public bool? IsActive { get; set; }

        public tblCategory? category { get; set; }
    }
}
