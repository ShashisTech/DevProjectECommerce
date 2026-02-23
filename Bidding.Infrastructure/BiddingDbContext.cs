using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Bidding.Domain.Entities;

namespace Bidding.Infrastructure;

public class BiddingDbContext : DbContext
{
    public BiddingDbContext(DbContextOptions<BiddingDbContext> options) : base(options) { }

    public DbSet<Auction> Auctions { get; set; } = null!;
    public DbSet<Bid> Bids { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auction>(b =>
        {
            b.HasKey(a => a.Id);
            b.Property(a => a.Title).IsRequired();
            b.Property(a => a.Description);
            b.Property(a => a.StartDate);
            b.Property(a => a.EndDate);
            b.Property(a => a.StartingPrice).HasColumnType("decimal(18,2)");

            // Configure relationship to Bid using the public navigation and use the backing field for storage
            b.HasMany(a => a.Bids)
             .WithOne()
             .HasForeignKey(nameof(Bid.AuctionId));

            b.Navigation(a => a.Bids).UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<Bid>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            b.Property(x => x.Bidder).IsRequired();
            b.Property(x => x.PlacedAt);
        });
    }
}
