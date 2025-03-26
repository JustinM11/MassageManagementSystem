using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MassageManagementSystem.Models;

namespace MassageManagementSystem.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Therapists> Therapists { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed some therapist data.
            modelBuilder.Entity<Therapists>().HasData(
               new Therapists { 
                   Id = 1, 
                   Name = "John Doe", 
                   Specialty = "Swedish Massage", 
                   Location = "123 Main St",
                   //Latitude = 43.6532,  // Example coordinates (Toronto)
                   //Longitude = -79.3832
               },
               new Therapists { 
                   Id = 2, 
                   Name = "Jane Smith", 
                   Specialty = "Deep Tissue", 
                   Location = "456 Elm St",
                   //Latitude = 43.7000,  // Example coordinates
                   //Longitude = -79.4000
               }
            );
            // Seed a promo code.
            modelBuilder.Entity<PromoCode>().HasData(
               new PromoCode { Id = 1, Code = "WELCOME10", DiscountAmount = 10, IsActive = true }
            );
        }
    }
}
