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

//  Load Environment Variables
// This allows environment variables to be used in configuration settings, ensuring sensitive values are not hardcoded.
builder.Configuration.AddEnvironmentVariables();

// Configure Logging
// Logging helps monitor application behavior and diagnose issues.
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders(); // Removes default logging providers to avoid redundancy.
    logging.AddConsole(); // Enables logging output in the console.
    logging.AddDebug(); // Enables logging for debugging purposes.
});

// Add Controllers
// Registers MVC controllers to handle API requests.
builder.Services.AddControllers();

// Configure CORS (Cross-Origin Resource Sharing)
// Ensures that only trusted origins can access the API.
var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(",") ?? new string[] { "http://localhost:5187" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins) // Specifies which origins can access the API.
              .AllowAnyHeader() // Allows any HTTP headers in requests.
              .AllowAnyMethod(); // Allows any HTTP methods (GET, POST, PUT, DELETE).
    });
});

// Configure Database Connection
// Retrieves the connection string from configuration and sets up the database.
var connectionString = builder.Configuration.GetConnectionString("RunClubDb")
    ?? throw new ArgumentNullException("Database connection string is missing!");

builder.Services.AddDbContext<RunClubContext>(options =>
    options.UseSqlite(connectionString)); // Uses SQLite database (can be replaced with SQL Server).

// Register Identity Services
// Configures authentication and role-based access control.
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<RunClubContext>() // Uses EF Core for user data storage.
    .AddDefaultTokenProviders(); // Provides tokens for password reset, email verification, etc.

// JWT Authentication Configuration
// Configures authentication using JSON Web Tokens (JWT) to secure API endpoints.
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new ArgumentNullException("JWT Key is missing!");
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new ArgumentNullException("JWT Issuer is missing!");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Ensures the token is from a trusted source.
            ValidateAudience = true, // Ensures the token is meant for the correct audience.
            ValidateLifetime = true, // Ensures the token is still valid (not expired).
            ValidateIssuerSigningKey = true, // Ensures the signature of the token is valid.
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) // Secure key used for signing tokens.
        };
    });

// Register Authorization Service
builder.Services.AddAuthorization();

// Register Application Services (Dependency Injection)
// These services contain business logic for handling authentication, user management, and other API functions.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IProgressRecordService, ProgressRecordService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Register Repositories
// Repositories abstract database operations, improving maintainability.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IProgressRecordRepository, ProgressRecordRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Register Other Services (e.g., Email Service)
builder.Services.AddScoped<EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Configure Swagger for API Documentation
// Swagger generates interactive API documentation, making it easier to test endpoints.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RunClub API", Version = "v1" });

    // Define security scheme for JWT authentication in Swagger
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
// Limits the number of API requests to prevent abuse.
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

// Register necessary rate limit services
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

// Build the Application
var app = builder.Build();

// Apply Database Migrations Automatically
// Ensures that any pending database migrations are applied when the application starts.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RunClubContext>();
    dbContext.Database.Migrate();
}

// Global Error Handling Middleware
// Handles application-wide exceptions in a structured way.
app.UseGlobalExceptionHandler();

// Enable Swagger UI in Development Mode
// The Swagger UI allows API testing and is only enabled in development mode.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enforce HTTPS
app.UseHttpsRedirection();

// Apply Security Middleware
app.UseCors("AllowSpecificOrigins"); // Enables CORS with the allowed origins.
app.UseIpRateLimiting(); // Applies IP-based request rate limiting.
app.UseAuthentication(); // Enables authentication middleware.
app.UseAuthorization(); // Enables authorization middleware.

// Map API Controllers
app.MapControllers();

// Run the Application
app.Run();


/* This ASP.NET Core Program.cs file serves as the entry point of the application, configuring all essential services and middleware. It begins by loading environment variables and configuring logging for monitoring. The CORS policy ensures secure cross-origin access, while the database connection is established via Entity Framework Core. Authentication is implemented using JWT tokens, ensuring secure user authorization. The Repository and Service layers are registered for clean separation of concerns, while Swagger is configured for API documentation. Rate limiting prevents abuse by restricting excessive requests from a single IP. Middleware for error handling, authentication, authorization, and HTTPS redirection is set up to enforce security standards. Finally, the app automatically applies database migrations, ensuring a smooth deployment. This structured approach makes the application scalable, secure, and maintainable, following best practices in modern web development. */