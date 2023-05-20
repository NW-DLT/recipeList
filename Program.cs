using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using recipeList;
using recipeList.Data;
using recipeList.Models;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//DBConnect
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connection));

//Identity services
builder.Services.AddScoped<JwtManager>();
builder.Services.AddIdentity<User, IdentityRole>(options => 
    options.Password = new PasswordOptions
    {
        RequireDigit = true,
        RequiredLength = 6,
        RequireLowercase = true,
        RequireUppercase = false
    })
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();

//AuthToken
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/login";
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
