using Bidding.Application.DTOs;
using Bidding.Application.Interfaces;
using Bidding.Application.Services;
using Bidding.Domain.Entities;
using Bidding.Infrastructure.Repositories;
using Bidding.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure HTTPS redirection port
//builder.Services.AddHttpsRedirection(options =>
//{
//    options.HttpsPort = 5001;
//});

// Add CORS policy that allows any origin, header and method
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register DbContext (SQLite)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=bidding.db";
builder.Services.AddDbContext<BiddingDbContext>(options => options.UseSqlite(connectionString));

// Application services and repository
// Keep InMemory for tests; register EF implementation by default
builder.Services.AddScoped<IAuctionRepository, EfAuctionRepository>();
builder.Services.AddScoped<BiddingService>();

var app = builder.Build();

// Ensure database is created/migrated and seed sample data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<BiddingDbContext>();
        db.Database.Migrate();

        if (!db.Auctions.Any())
        {
            var auction1 = new Auction
            {
                Title = "Vintage Camera",
                Description = "A classic 1950s film camera in working condition.",
                StartDate = DateTime.UtcNow.AddMinutes(-10),
                EndDate = DateTime.UtcNow.AddDays(3),
                StartingPrice = 50m
            };

            var bid1 = new Bid
            {
                AuctionId = auction1.Id,
                Bidder = "alice@example.com",
                Amount = 60m,
                PlacedAt = DateTime.UtcNow.AddMinutes(-5)
            };

            auction1.AddBid(bid1);

            var auction2 = new Auction
            {
                Title = "Antique Vase",
                Description = "Porcelain vase from 19th century.",
                StartDate = DateTime.UtcNow.AddHours(-1),
                EndDate = DateTime.UtcNow.AddDays(7),
                StartingPrice = 200m
            };

            db.Auctions.AddRange(auction1, auction2);
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetService<ILoggerFactory>()?.CreateLogger("Startup");
        logger?.LogError(ex, "An error occurred while migrating/seeding the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

app.MapPost("/auctions", async (CreateAuctionDto dto, BiddingService service) =>
{
    var auction = new Auction
    {
        Title = dto.Title,
        Description = dto.Description,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        StartingPrice = dto.StartingPrice
    };

    var created = await service.CreateAuctionAsync(auction);
    return Results.Created($"/auctions/{created.Id}", created);
});

app.MapGet("/auctions", async (BiddingService service) =>
{
    var list = await service.GetAllAsync();
    return Results.Ok(list);
});

app.MapGet("/auctions/{id}", async (Guid id, BiddingService service) =>
{
    var auction = await service.GetAsync(id);
    return auction is not null ? Results.Ok(auction) : Results.NotFound();
});

app.MapPost("/auctions/{id}/bids", async (Guid id, PlaceBidDto dto, BiddingService service) =>
{
    var bid = new Bid
    {
        AuctionId = id,
        Bidder = dto.Bidder,
        Amount = dto.Amount
    };

    try
    {
        await service.PlaceBidAsync(bid);
        return Results.Ok(bid);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();
