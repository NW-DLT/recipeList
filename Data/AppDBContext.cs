using Microsoft.EntityFrameworkCore;
using recipeList.Models;

namespace recipeList.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Recipe> recipes { get; set; }
        public DbSet<Product> products { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> dbContextOptions)
            :base(dbContextOptions)
        {
        }
    }
}
