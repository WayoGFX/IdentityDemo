using Microsoft.AspNetCore.Identity;
using IdentityDemo.Api.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Numerics;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IdentityDemo.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// register appdbcontext
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddOpenApi();
builder.Services.AddControllers(); // register controllers

builder.Services.AddScoped<TokenService>();

// register configuration JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // scalar reference
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers(); // controllers response 

app.Run();