using Microsoft.AspNetCore.Identity; // Identity framework for authentication and authorization
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RunClubAPI.Models
{
    // ✅ The database context class for the RunClub application.
    // ✅ Inherits from IdentityDbContext to include authentication and role management features.
    public class RunClubContext : IdentityDbContext<User> // Using a custom User class
    {
        // ✅ Constructor: Accepts DbContext options and passes them to the base constructor
        public RunClubContext(DbContextOptions<RunClubContext> options)
            : base(options)
        {
        }

        // ✅ DbSet properties represent tables in the database.

        // Stores user information, inherits from Identity's User management.
        public DbSet<User> Users { get; set; }

        // Stores roles for different types of users (Admin, Coach, Runner).
        public DbSet<Role> Roles { get; set; }

        // Stores information about events that runners can participate in.
        public DbSet<Event> Events { get; set; }

        // Tracks which users have enrolled in which events.
        public DbSet<Enrollment> Enrollments { get; set; }

        // Keeps progress records for users, useful for tracking performance.
        public DbSet<ProgressRecord> ProgressRecords { get; set; }

        // ✅ This method configures the database schema and seeds initial data.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ Ensures Identity framework's tables (Users, Roles, etc.) are set up correctly.
            base.OnModelCreating(modelBuilder);

            // ✅ Seeding predefined roles into the database at application startup.
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" }, // Admin has full control
                new IdentityRole { Name = "Coach", NormalizedName = "COACH" }, // Coaches can manage training
                new IdentityRole { Name = "Runner", NormalizedName = "RUNNER" } // Runners participate in events
            );
        }
    }
}

/*
The RunClubContext class in my ASP.NET Core Web API serves as the database context, managing interactions with the underlying database using Entity Framework Core. It extends IdentityDbContext<User>, which integrates ASP.NET Identity, providing built-in authentication, authorization, and role management. This allows for secure user handling and access control.

The DbSet properties define the application's main entities—such as Users, Roles, Events, Enrollments, and ProgressRecords—which correspond to tables in the database. The OnModelCreating method ensures that the Identity framework's schema is correctly initialized while also seeding roles (Admin, Coach, Runner) at startup, ensuring that the application begins with predefined roles.

Using dependency injection, this context is passed DbContextOptions from the application’s configuration, ensuring flexibility and database independence (e.g., it can work with SQL Server, PostgreSQL, or SQLite). Overall, this structure promotes a modular, scalable, and secure database implementation, essential for managing users, roles, and event participation in the RunClub system.
*/