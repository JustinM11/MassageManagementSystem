using Microsoft.EntityFrameworkCore;

namespace MassageManagementSystem.Models.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public required DbSet<Therapists> Therapists { get; set; }
        public required DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Therapists>().HasData(
                new Therapists { Id = 1, Name = "John Doe", Specialty = "Swedish Massage" },
                new Therapists { Id = 2, Name = "Jane Smith", Specialty = "Deep Tissue" }
            );
        }
    }


}
