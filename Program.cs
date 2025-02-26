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
using RunClubAPI.DTOs;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load Environment Variables
builder.Configuration.AddEnvironmentVariables();

// ✅ Configure Logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

// ✅ Add Controllers
builder.Services.AddControllers();

// ✅ Configure CORS
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

// ✅ Configure Database Connection
var connectionString = builder.Configuration.GetConnectionString("RunClubDb")
    ?? throw new ArgumentNullException("Database connection string is missing!");

builder.Services.AddDbContext<RunClubContext>(options =>
    options.UseSqlite(connectionString)); // Or UseSqlServer(connectionString) for SQL Server

// ✅ Register Identity Services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<RunClubContext>()
    .AddDefaultTokenProviders();

// ✅ JWT Authentication Configuration
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new ArgumentNullException("JWT Key is missing!");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new ArgumentNullException("JWT Issuer is missing!");

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

// ✅ Register Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IProgressRecordService, ProgressRecordService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// ✅ Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IProgressRecordRepository, ProgressRecordRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// ✅ Register Other Services
builder.Services.AddScoped<EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// ✅ Configure Swagger for API Documentation
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

// ✅ Configure Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

// Register the necessary rate limit services
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

// ✅ Build Application
var app = builder.Build();

// ✅ Apply Database Migrations Automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RunClubContext>();
    dbContext.Database.Migrate();
}

// ✅ Global Error Handling Middleware (Ensure you have implemented this middleware)
app.UseGlobalExceptionHandler();

// ✅ Enable Swagger in Development Mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ Enforce HTTPS
app.UseHttpsRedirection();

// ✅ Apply Security Middleware
app.UseCors("AllowSpecificOrigins");
app.UseIpRateLimiting(); // Make sure this middleware is applied after necessary services are added
app.UseAuthentication();
app.UseAuthorization();

// ✅ Map Controllers
app.MapControllers();

// ✅ Run Application
app.Run();
