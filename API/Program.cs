using System.Text;
using API.Data;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>( opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));    
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<IMemberRepository,MemberRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenkey =builder.Configuration["TokenKey"]
          ?? throw new Exception("Token key not found - program.cs");
        options.TokenValidationParameters= new TokenValidationParameters
        {
            ValidateIssuerSigningKey=true,
            IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey)),
            ValidateIssuer=false,
            ValidateAudience=false
        };
    });

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.

// تفعيل CORS
app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope =app.Services.CreateScope();
var services =scope.ServiceProvider;
try
{
    var context =services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger= services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex,"An error occured during migration");
}

app.Run();
