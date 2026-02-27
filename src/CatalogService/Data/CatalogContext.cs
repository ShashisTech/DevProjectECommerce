using Microsoft.EntityFrameworkCore;
using CatalogService.Models;

namespace CatalogService.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(p =>
            {
                p.HasKey(p => p.Id);
                p.HasOne(p => p.Category)
                 .WithMany(c => c.Products)
                 .HasForeignKey(p => p.CategoryId);
            });

            modelBuilder.Entity<Category>(c =>
            {
                c.HasKey(c => c.Id);
                c.Property(c => c.Name).IsRequired();
            });
        }
    }
}
