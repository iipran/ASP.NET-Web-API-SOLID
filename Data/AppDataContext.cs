using Microsoft.EntityFrameworkCore;
using MySolidAPI.Models;

namespace MySolidAPI.Data{

    public class AppDataContext : DbContext{
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=MySolidAPI;Username=admin;Password=admin123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product{ProductId = 1, Name = "Product 1", Description = "Product 1 description", Price = 100, CreatedDate = DateTime.UtcNow}
            );
        }
    }
}