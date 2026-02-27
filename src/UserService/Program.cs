using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("UserConnection") ?? "Data Source=users.db"));

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? Environment.GetEnvironmentVariable("JWT_KEY") ?? "super_secret_key_123!";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserContext>();
    // use migrations when available
    db.Database.Migrate();

    // seed admin and sample users
    if (!db.Users.Any())
    {
        var admin = new UserService.Models.User
        {
            Username = "admin",
            PasswordHash = UserService.Services.PasswordHasher.Hash("Admin@123"),
            Email = "admin@example.com",
            Role = "Admin",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var buyer = new UserService.Models.User
        {
            Username = "buyer1",
            PasswordHash = UserService.Services.PasswordHasher.Hash("buyerpass"),
            Email = "buyer1@example.com",
            Role = "Buyer",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var seller = new UserService.Models.User
        {
            Username = "seller1",
            PasswordHash = UserService.Services.PasswordHasher.Hash("sellerpass"),
            Email = "seller1@example.com",
            Role = "Seller",
            IsSeller = true,
            IsActive = true,
            RentPaid = true,
            CreatedAt = DateTime.UtcNow
        };

        db.Users.AddRange(admin, buyer, seller);
        db.SaveChanges();
    }
}

app.Run();
