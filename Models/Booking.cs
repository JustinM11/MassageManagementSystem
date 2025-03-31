namespace MassageManagementSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        // Link to the ApplicationUser via UserId (if the user is registered)
        public string? UserId { get; set; }

        // Name of the guest or user (optional if you want to support guest bookings)
        public string Name { get; set; } = string.Empty;

        public int TherapistId { get; set; }

        public DateTime AppointmentTime { get; set; }

        public bool IsConfirmed { get; set; }

        // 🔽 Navigation property
        public Therapists? Therapist { get; set; }
    }
}
