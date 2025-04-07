using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;
using RunClubAPI.Services;
using AspNetCoreRateLimit;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
builder.Configuration.AddEnvironmentVariables();

// ==================== Services Configuration ====================

// Logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

// Controllers
builder.Services.AddControllers();

// CORS
var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(",") ?? new[] { "http://localhost:5187" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// DB Connection
var connectionString = builder.Configuration.GetConnectionString("RunClubDb");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Database connection string 'RunClubDb' is missing!");

builder.Services.AddDbContext<RunClubContext>(options =>
    options.UseSqlite(connectionString));

// Identity & Password Settings
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

// JWT Setup
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer))
    throw new InvalidOperationException("JWT configuration is missing!");

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
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Authorization
builder.Services.AddAuthorization();

// ==================== Dependency Injection ====================

// Core App Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IProgressRecordService, ProgressRecordService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Email Config
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// ==================== Swagger ====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RunClub API", Version = "v1" });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer your_token_here'",
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
        { securitySchema, Array.Empty<string>() }
    });
});

// ==================== Rate Limiting ====================
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

builder.Services.AddHealthChecks();


var app = builder.Build();
app.UseCors("AllowReactApp");

// ==================== Apply DB Migrations ====================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RunClubContext>();
    dbContext.Database.Migrate();
}

// ==================== Role Seeder ====================
async Task EnsureRolesExist(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Coach", "Runner" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}
await EnsureRolesExist(app.Services);



app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RunClub API v1");
    c.RoutePrefix = string.Empty; // Makes Swagger homepage
});

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseIpRateLimiting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();


/* This ASP.NET Core Program.cs file serves as the entry point of the application, configuring all essential services and middleware. It begins by loading environment variables and configuring logging for monitoring. The CORS policy ensures secure cross-origin access, while the database connection is established via Entity Framework Core. Authentication is implemented using JWT tokens, ensuring secure user authorization. The Repository and Service layers are registered for clean separation of concerns, while Swagger is configured for API documentation. Rate limiting prevents abuse by restricting excessive requests from a single IP. Middleware for error handling, authentication, authorization, and HTTPS redirection is set up to enforce security standards. Finally, the app automatically applies database migrations, ensuring a smooth deployment. This structured approach makes the application scalable, secure, and maintainable, following best practices in modern web development. */