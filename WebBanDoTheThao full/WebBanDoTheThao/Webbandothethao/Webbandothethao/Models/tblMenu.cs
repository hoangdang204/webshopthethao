using System.ComponentModel.DataAnnotations;

namespace Webbandothethao.Models
{
    public class tblMenu
    {
        [Key]

        public int MenuID { get; set; }

        public string? MenuName { get; set; }

        public string? ControllerName { get; set; }

        public string? ActionName { get; set; }

        public int MenuOrder { get; set; }

        public bool? IsActive { get; set; }

        public int Position {get; set;}
    }
}
