using System.Transactions;
using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<TransactionContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TransactionConnection") ?? "Data Source=transaction.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HTTP client to talk to OrderService (base URL configured via OrderServiceUrl)
builder.Services.AddHttpClient<IOrderClient, OrderClient>();

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

            new TransactionService.Models.Transaction { OrderId = 1, Amount = 49.98m, Status = "Succeeded", ProcessedAt = DateTime.UtcNow.AddDays(-1), PaymentMethod = "Card" },
            new TransactionService.Models.Transaction { OrderId = 2, Amount = 19.99m, Status = "Succeeded", ProcessedAt = DateTime.UtcNow.AddHours(-6), PaymentMethod = "UPI" }
        );
        db.SaveChanges();
    }
}

app.Run();
