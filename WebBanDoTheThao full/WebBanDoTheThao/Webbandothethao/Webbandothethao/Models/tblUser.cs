using System.ComponentModel.DataAnnotations;

namespace Webbandothethao.Models
{
    public class tblUser
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public string? role { get; set; }

        public string? img { get; set; }
        public DateTime? created_at { get; set; } 
        public DateTime? updated_at { get; set; }

        public bool? Status { get; set; }    
    }
}
