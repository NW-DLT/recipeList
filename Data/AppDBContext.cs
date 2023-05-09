﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using recipeList.Models;

namespace recipeList.Data
{
    public class AppDBContext : IdentityDbContext<User>
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
