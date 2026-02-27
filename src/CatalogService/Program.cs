using Microsoft.EntityFrameworkCore;
using CatalogService.Data;
using CatalogService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CatalogConnection") ?? "Data Source=catalog.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// migrate and seed sample data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CatalogContext>();
    db.Database.Migrate();

    if (!db.Categories.Any())
    {
        var electronics = new Category { Name = "Electronics" };
        var books = new Category { Name = "Books" };
        db.Categories.AddRange(electronics, books);
        db.SaveChanges();

        if (!db.Products.Any())
        {
            db.Products.AddRange(new[] {
                new Product { Name = "Sample Product A", Description = "Desc A", Price = 9.99m, Stock = 100, CategoryId = electronics.Id },
                new Product { Name = "Sample Product B", Description = "Desc B", Price = 19.99m, Stock = 50, CategoryId = books.Id }
            });
            db.SaveChanges();
        }
    }
}

app.Run();
