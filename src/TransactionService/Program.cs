using Microsoft.EntityFrameworkCore;
using TransactionService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<TransactionContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TransactionConnection") ?? "Data Source=transaction.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HTTP client to talk to OrderService (base URL via env var 'ORDER_SERVICE_URL' or configuration key 'OrderServiceUrl')
builder.Services.AddHttpClient("orders", client =>
{
    var url = builder.Configuration["OrderServiceUrl"] ?? Environment.GetEnvironmentVariable("ORDER_SERVICE_URL") ?? "http://localhost:5001";
    client.BaseAddress = new Uri(url);
});

builder.Services.AddScoped<TransactionService.Services.IOrderClient, TransactionService.Services.OrderClient>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TransactionContext>();
    db.Database.EnsureCreated();

    // seed sample transactions
    if (!db.Transactions.Any())
    {
        db.Transactions.AddRange(
            new TransactionService.Models.Transaction { OrderId = 1, Amount = 49.98m, Status = "Succeeded", ProcessedAt = DateTime.UtcNow.AddDays(-1) },
            new TransactionService.Models.Transaction { OrderId = 2, Amount = 19.99m, Status = "Succeeded", ProcessedAt = DateTime.UtcNow.AddHours(-6) }
        );
        db.SaveChanges();
    }
}

app.Run();
