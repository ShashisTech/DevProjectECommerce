using Microsoft.EntityFrameworkCore;
using AuctionService.Models;

namespace AuctionService.Data
{
    public class AuctionContext : DbContext
    {
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options) { }

        public DbSet<AuctionItem> AuctionItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuctionItem>().HasKey(a => a.Id);
        }
    }
}
