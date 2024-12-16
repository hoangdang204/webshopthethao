using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webbandothethao.Models
{
    public class tblAddress
    {
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; } // Liên kết với người dùng
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }

    public string? Email {get; set;}

    public string? Note {get; set;}
    public bool IsDefault { get; set; }
    }
}