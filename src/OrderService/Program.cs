using Microsoft.EntityFrameworkCore;
using OrderService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<OrderContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("OrderConnection") ?? "Data Source=order.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HTTP client to talk to CatalogService (base URL via env var 'CATALOG_SERVICE_URL' or configuration key 'CatalogServiceUrl')
builder.Services.AddHttpClient("catalog", client =>
{
    var url = builder.Configuration["CatalogServiceUrl"] ?? Environment.GetEnvironmentVariable("CATALOG_SERVICE_URL") ?? "http://localhost:5000";
    client.BaseAddress = new Uri(url);
});

builder.Services.AddScoped<OrderService.Services.ICatalogClient, OrderService.Services.CatalogClient>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// ensure database is migrated and seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderContext>();
    // apply migrations if any
    db.Database.Migrate();

    // seed sample orders if none exist
    if (!db.Orders.Any())
    {
        db.Orders.AddRange(
            new OrderService.Models.Order { ProductId = 1, Quantity = 2, Total = 49.98m, CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new OrderService.Models.Order { ProductId = 2, Quantity = 1, Total = 19.99m, CreatedAt = DateTime.UtcNow.AddDays(-1) }
        );
        db.SaveChanges();
    }
}

app.Run();
