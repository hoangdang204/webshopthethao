using System.ComponentModel.DataAnnotations;

namespace Webbandothethao.Models
{
    public class tblCategory
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public DateTime? created_at { get; set; } = DateTime.Now;
        public DateTime? updated_at { get; set; } = DateTime.Now;

        public bool? IsActive { get; set; }
        public ICollection<tblProduct>? products { get; set; }

    }
}
