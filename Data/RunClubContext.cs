using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RunClubAPI.Models
{
    // ✅ The database context class for the RunClub application.
    public class RunClubContext : IdentityDbContext<User> // Using a custom User class
    {
        public RunClubContext(DbContextOptions<RunClubContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<ProgressRecord> ProgressRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeding roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Coach", NormalizedName = "COACH" },
                new IdentityRole { Id = "3", Name = "Runner", NormalizedName = "RUNNER" }
            );
        }
    }
}



/*
The RunClubContext class in my ASP.NET Core Web API serves as the database context, managing interactions with the underlying database using Entity Framework Core. It extends IdentityDbContext<User>, which integrates ASP.NET Identity, providing built-in authentication, authorization, and role management. This allows for secure user handling and access control.

The DbSet properties define the application's main entities—such as Users, Roles, Events, Enrollments, and ProgressRecords—which correspond to tables in the database. The OnModelCreating method ensures that the Identity framework's schema is correctly initialized while also seeding roles (Admin, Coach, Runner) at startup, ensuring that the application begins with predefined roles.

Using dependency injection, this context is passed DbContextOptions from the application’s configuration, ensuring flexibility and database independence (e.g., it can work with SQL Server, PostgreSQL, or SQLite). Overall, this structure promotes a modular, scalable, and secure database implementation, essential for managing users, roles, and event participation in the RunClub system.
*/