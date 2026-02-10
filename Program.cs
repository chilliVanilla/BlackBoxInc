using BlackBoxInc.Data;
using BlackBoxInc.Models.Entities;
using BlackBoxInc.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Add authentication
builder.Services.AddDbContext<AuthDbContext>(options => 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AuthDb"))
    );

builder.Services.AddAuthorization();

//builder.Services.AddIdentityApiEndpoints<IdentityUser>()
//    .AddEntityFrameworkStores<AuthDbContext>();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//List dependencies to be injected all over
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AppDb")
    )
);
builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));



builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductServices, ProductService>();

//Builds the app innit
var app = builder.Build();


app.MapIdentityApi<IdentityUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
