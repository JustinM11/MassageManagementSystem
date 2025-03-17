namespace MassageManagementSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Link to Identity user
        public int TherapistId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
