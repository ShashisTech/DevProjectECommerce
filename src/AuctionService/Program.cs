using Microsoft.EntityFrameworkCore;
using AuctionService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AuctionConnection") ?? "Data Source=auction.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuctionContext>();
    db.Database.EnsureCreated();
}

app.Run();
