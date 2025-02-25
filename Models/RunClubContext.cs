using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RunClubAPI.Models
{
    // Custom User class
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Add other custom properties if needed
    }

    public class RunClubContext : IdentityDbContext<User> // Use 'User' as the custom Identity user class
    {
        public RunClubContext(DbContextOptions<RunClubContext> options)
            : base(options)
        {
        }

        // Define DbSets for your entities
        public DbSet<Role> Roles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<ProgressRecord> ProgressRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure Identity tables are correctly created

            // Seeding roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Coach", NormalizedName = "COACH" },
                new IdentityRole { Name = "Runner", NormalizedName = "RUNNER" }
            );
        }
    }
}
