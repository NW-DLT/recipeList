using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using recipeList.Models;

namespace recipeList.Data
{
    public class AppDBContext : IdentityDbContext<User>
    {
        public DbSet<User> users { get; set; }
        public DbSet<Recipe> recipes { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Like> likes { get; set; }
        public DbSet<Subscribe> subscribers { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<comment> comments { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> dbContextOptions)
            :base(dbContextOptions)
        {
        }
    }
}
