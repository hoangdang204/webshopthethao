using Microsoft.EntityFrameworkCore;
using Webbandothethao.Areas.Admin.Models;

namespace Webbandothethao.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<tblUser> tblUsers { get; set; }

        public DbSet<tblCategory> tblCategories { get; set; }

        public DbSet<tblProduct> tblProducts { get; set; }

        public DbSet<tblOrder> tblOrders { get; set; }

        public DbSet<tblOrderDetail> tblOrderDetails { get; set; }


        public DbSet<tblCart> tblCarts { get; set; }

        public DbSet<tblMenu> tblMenus { get; set; }

        public DbSet<tblAdminMenu> tblAdminMenus { get; set; }

        public DbSet<tblBlog> tblBlogs { get; set; }

        public DbSet<tblCartItem> tblCartItems { get; set; }

        public DbSet<tblAddress> tblAddresses {get; set;}

        public DbSet<tblPayment> tblPayments {get; set;}

    }
}
