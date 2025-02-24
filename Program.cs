using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RunClubAPI.Models;
using RunClubAPI.Interfaces;
using RunClub.DTOs;
using RunClub.Services;
using RunClub.Repositories;
using RunClub.Controllers;
using Microsoft.Extensions.Logging; // Ensure this is included
using RunClubAPI.Middleware; // Import the Middleware namespace
using AspNetCoreRateLimit;



var builder = WebApplication.CreateBuilder(args);

// Load Environment Variables
builder.Configuration.AddEnvironmentVariables();

// Configure Services
builder.Services.AddControllers();

// Add Logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

// CORS Configuration
var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(",") ?? new string[] { "http://localhost:3000" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure Database Connection
builder.Services.AddDbContext<RunClubContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RunClubDb") ??
                      throw new ArgumentNullException("Database connection string is missing!")));

// Register Identity Services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<RunClubContext>()
    .AddDefaultTokenProviders();

// JWT Authentication Configuration
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing!");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer is missing!");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Dependency Injection for Services and Repositories
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IProgressRecordService, ProgressRecordService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<EnrollmentRepository>();
builder.Services.AddScoped<ProgressRecordRepository>();
builder.Services.AddScoped<RoleRepository>();

builder.Services.AddScoped<EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RunClub API", Version = "v1" });
});

// Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();

// Global Error Handling Middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Enable Swagger in Development Mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enforce HTTPS
app.UseHttpsRedirection();

// Apply Security Middleware
app.UseCors("AllowSpecificOrigins");
app.UseIpRateLimiting();
app.UseAuthentication();
app.UseAuthorization();
app.UseGlobalExceptionHandler();


// Map Controllers
app.MapControllers();

app.Run();
