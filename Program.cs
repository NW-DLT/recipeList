using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using recipeList.Data;
using recipeList.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//DBConnect
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connection));

//Identity services
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


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
