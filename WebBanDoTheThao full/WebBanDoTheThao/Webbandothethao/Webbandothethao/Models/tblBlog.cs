using System.ComponentModel.DataAnnotations;

namespace Webbandothethao.Models
{
    public class tblBlog
    {
        [Key]
        public int id { get; set; }

        public string? articletitle { get; set; }

        public string? description { get; set; }

        public string? content { get; set; }

        public string? img { get; set; }

        public DateTime created_at { get; set; } = DateTime.Now;

        public DateOnly? updated_at { get; set; }

        public bool? IsActive {  get; set; }
    }
}
