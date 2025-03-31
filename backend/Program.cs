using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RunClubAPI.Models;
using RunClubAPI.Interfaces;
using RunClubAPI.Services;
using RunClubAPI.Repositories;
using RunClubAPI.Middleware;
using AspNetCoreRateLimit;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Load Environment Variables
builder.Configuration.AddEnvironmentVariables();

// Configure Logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

// Add Controllers
builder.Services.AddControllers();

// Configure CORS
var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(",") ?? new string[] { "http://localhost:5187" };
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
var connectionString = builder.Configuration.GetConnectionString("RunClubDb");
if (string.IsNullOrEmpty(connectionString))
{
    throw new ArgumentNullException("Database connection string is missing!");
}

builder.Services.AddDbContext<RunClubContext>(options =>
    options.UseSqlite(connectionString));

// Register Identity Services
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<RunClubContext>()
    .AddDefaultTokenProviders();

// JWT Authentication Configuration
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer))
{
    throw new ArgumentNullException("JWT configuration is missing in appsettings!");
}

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

// Register Authorization Service
builder.Services.AddAuthorization();

// Register Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IProgressRecordService, ProgressRecordService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IProgressRecordRepository, ProgressRecordRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Register Other Services (e.g., Email Service)
builder.Services.AddScoped<EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Configure Swagger for API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RunClub API", Version = "v1" });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer [space] your_token' below:",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securitySchema, new string[] { } }
    });
});

// Configure Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

// Register necessary rate limit services
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Apply Database Migrations Automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RunClubContext>();
    dbContext.Database.Migrate();
}

// Ensure Roles Exist
async Task EnsureRolesExist(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roleNames = { "Admin", "Coach", "Runner" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Ensure role creation before running app
await EnsureRolesExist(app.Services);

// Global Error Handling Middleware (Ensure Middleware is implemented)
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Enable Swagger UI in Development Mode
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

// Map API Controllers
app.MapControllers();

// Run the Application
app.Run();

/* This ASP.NET Core Program.cs file serves as the entry point of the application, configuring all essential services and middleware. It begins by loading environment variables and configuring logging for monitoring. The CORS policy ensures secure cross-origin access, while the database connection is established via Entity Framework Core. Authentication is implemented using JWT tokens, ensuring secure user authorization. The Repository and Service layers are registered for clean separation of concerns, while Swagger is configured for API documentation. Rate limiting prevents abuse by restricting excessive requests from a single IP. Middleware for error handling, authentication, authorization, and HTTPS redirection is set up to enforce security standards. Finally, the app automatically applies database migrations, ensuring a smooth deployment. This structured approach makes the application scalable, secure, and maintainable, following best practices in modern web development. */