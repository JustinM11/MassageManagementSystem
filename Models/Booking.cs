namespace MassageManagementSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        // Link to the ApplicationUser via UserId.
        public string? UserId { get; set; }
        public int TherapistId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
