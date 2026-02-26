using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddHttpClient("userservice", client =>
    {
        client.BaseAddress = new Uri("http://userservice:8080");
    }).AddStandardResilienceHandler();


builder.Services.AddHttpClient("productservice", client =>
{
    client.BaseAddress = new Uri("http://productservice:8080");
}).AddStandardResilienceHandler();

builder.Services.AddScoped<UserValidatorService>();
builder.Services.AddScoped<ProductValidatorService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Migration skipped: " + ex.Message);
    }
}

app.MapGet("/health", () => "Healthy");

app.Run();
