using System.Text;
using GoonAuctionAPI.BackgroundServices;
using GoonAuctionAPI.Hubs;
using GoonAuctionBLL.Interfaces;
using GoonAuctionBLL.Services;
using GoonAuctionDAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<BidService>();
builder.Services.AddScoped<IBidRepository, BidRepository>();

builder.Services.AddHostedService<AuctionStatusWorker>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtKey = builder.Configuration["Jwt:Key"] ?? "FallbackDevJwtKey_ChangeMe_32Bytes!!";
    var issuer = builder.Configuration["Jwt:Issuer"] ?? "http://localhost";
    var audience = builder.Configuration["Jwt:Audience"] ?? "http://localhost";

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception is SecurityTokenExpiredException)
            {
                context.Response.Headers["Token-Expired"] = "true";
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("jwt"))
            {
                context.Token = context.Request.Cookies["jwt"];
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(origin => true); // This is needed for SignalR negotiation
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

StripeConfiguration.ApiKey = builder.Configuration["StripeConfigurtion:ApiKey"];

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.UseDefaultFiles();
app.MapHub<BidHub>("/hub");

app.Run();

public partial class Program { }