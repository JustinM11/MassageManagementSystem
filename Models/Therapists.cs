namespace MassageManagementSystem.Models
{
    public class Therapists
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Specialty { get; set; }
        // For map integration, you can store an address or coordinates.
        public string? Location { get; set; }

        // properties for mapping
        //public double Latitude { get; set; }
        //public double Longitude { get; set; }
    }
}
