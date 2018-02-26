using ProductsApp.Dal.Entity;
using Microsoft.EntityFrameworkCore;

namespace ProductsApp.Dal
{
    public class OnFeetContext : DbContext
    {
        public OnFeetContext(DbContextOptions<OnFeetContext> options): base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<Like> Likes { get; set; }
    }
}
