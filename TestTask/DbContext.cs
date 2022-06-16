using Microsoft.EntityFrameworkCore;
using TestTask.Models;

namespace TestTask
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
            public DbSet<Product> Products { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<CustomParameter> CustomParameters { get; set; } 
            public DbSet<ProductParameter> ProductParameters { get; set; }
            

        public DbContext(DbContextOptions<DbContext> options)
                : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
