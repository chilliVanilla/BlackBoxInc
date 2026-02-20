using BlackBoxInc.Data;
using BlackBoxInc.Models.Entities;
using BlackBoxInc.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add controllers .
builder.Services.AddControllers();
//builder.Services.AddOpenApi();


//To initialise authentication
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
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JwtKey"] ??
            throw new Exception("Check your user secrets")))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Auth failed: " + context.Exception.Message);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

//To include auth in swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BlackBoxInc", Version = "v1" });
    // To define the 'Bearer' security scheme
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    // To make the lock symbol appear on every endpoint
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
{
    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
    Id = "Bearer"
}
                //Reference = new OpenApiReference
                //{
                //    Type = ReferenceType.SecurityScheme,
                //    Id = "Bearer"
                //}
            },
            new string[]{}
        }
    });
});



//To properly set up the authentication with passwrod constraints
//To set up role based access
//To give the AuthController the services it needs i.e setting up Identity
builder.Services.AddIdentityApiEndpoints<User>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
})
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


//builder.Services.AddIdentity<User, IdentityRole>(options =>
//{
//    options.Password.RequireDigit = true;
//    options.Password.RequiredLength = 8;
//})
//    .AddRoles<IdentityRole>()
//    .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider)
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

// For services handling
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//db connectivity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MainDb")
    )
);

//To help convert the enums from numbers to their actualvalue, makes it more readable
builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

//List dependencies to be injected all over
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductServices, ProductService>();
builder.Services.AddScoped<ITokenService, TokenService>();


//Builds the app
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//To handle authentication and authorization
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

//To seed the roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Define the roles you need
    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();

// {
//     "username": "jesse@admin",
//     "password": "String_2026"
// }