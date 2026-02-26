using System;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().HasData(
            new Order { OrderId = 1, ProductId = 1, Quantity = 1, UserId = 1 },
            new Order { OrderId = 3, ProductId = 2, Quantity = 3, UserId = 2 }
        );
    }
}
